using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyProjectServer.Data;
using MyProjectServer.Models;
using System.IO;
using System.Text.Json;

namespace MyProjectServer.Controllers
{
    public class MyProjectController : Controller
    {
        private readonly MyProjectContext _context;
        private readonly JsonSerializerOptions options = new() { WriteIndented = true };

        public MyProjectController(MyProjectContext context)
        {
            _context = context;
        }

        //Получение данных с БД
        public async Task<IActionResult> Read()
        {
            ListStaffDTO listStaffDTO = new();

            var myProjectContext = _context.Staffs.AsNoTracking().Include(c => c.Company).Include(s => s.Profile).Include(d => d.Depts);

            await myProjectContext.ForEachAsync(data =>
            {
                listStaffDTO.Data.Add(new StaffDTO
                {
                    Id = data.Id,
                    Name = data.Name,
                    Company = data.Company.Name,
                    DeptL = from item in data.Depts where item != null select item.Department,
                    Login = data.Profile.Login,
                    Password = data.Profile.Password
                });
            });


            string str = JsonSerializer.Serialize(listStaffDTO, options);
            return new ObjectResult(str);

        }
        
        //Добавление записи в БД
        public async Task<IActionResult> Create(string str)
        {
            DataPage? data = JsonSerializer.Deserialize<DataPage>(str);

            Company? company = await _context.Companies.FirstAsync(c => c.Name == data.Company);

            StaffProfile? staffProfile = new() { Login = data.Login, Password = data.Password };

            Staff? staff = new()
            {
                Name = data.Name,
                Depts = await (from item in _context.Depts where data.DeptL.Contains(item.Department) select item).ToListAsync(),
                Company = company,
                Profile = staffProfile
            };
            await _context.AddAsync(staff);
            await _context.SaveChangesAsync();

            return new OkResult();
        }

        //Удаление записи из БД
        public async Task<IActionResult> Delete(int id)
        {
            Staff? staff = await _context.Staffs.FirstAsync(f => f.Id == id);
            if (staff != null)
            {
                _context.Staffs.Remove(staff);
                await _context.SaveChangesAsync();
            }

            return new ObjectResult(staff)
            {
                StatusCode = staff == null ? StatusCodes.Status204NoContent : StatusCodes.Status200OK
            };
        }

        //Обновление записи в БД
        public async Task<IActionResult> Update(string str)
        {
            DataPage? data = JsonSerializer.Deserialize<DataPage>(str);
            Staff? staff = await _context.Staffs.Include(p => p.Profile).Include(d => d.Depts).FirstAsync(f => f.Id == data.Id);
            if (staff != null)
            {
                staff.Name = data.Name;
                staff.Company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == data.Company);
                staff.Depts = await (from item in _context.Depts where data.DeptL.Contains(item.Department) select item).ToListAsync();
                staff.Profile.Login = data.Login;
                staff.Profile.Password = data.Password;
                _context.Update(staff);
                await _context.SaveChangesAsync();
                return new OkResult();
            }
            else
            {
                return new NoContentResult();
            }
        }
    }
}