using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;
using System.Text.Json;

namespace MvcMovie.Controllers
{
    public class MyProjectController : Controller
    {
        private readonly MyProjectContext _context;

        public MyProjectController(MyProjectContext context)
        {
            _context = context;
        }

        //Получение данных с БД
        public async Task<IActionResult> Read()
        {
            DataList dataList = new();
            JsonSerializerOptions options = new()
            {

                WriteIndented = true
            };
            var myProjectContext = _context.Staffs.Include(c => c.Company).Include(s => s.Profile).Include(d => d.Depts);
            await myProjectContext.ForEachAsync(data =>
            {

                dataList.Data.Add(new DataPage
                {
                    Id = data.Id,
                    Name = data.Name,
                    Company = data.Company.Name,
                    DeptL = DeptToString(data.Depts),
                    Login = data.Profile.Login,
                    Password = data.Profile.Password
                });
            });
            string str = JsonSerializer.Serialize(dataList, options);
            return new ObjectResult(str);

        }
        //Добавление записи в БД
        public async Task<IActionResult> Create(string str)
        {

            DataPage? data = JsonSerializer.Deserialize<DataPage>(str);
            Company? company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == data.Company);

            StaffProfile? staffProfile = new() { Login = data.Login, Password = data.Password };

            Staff? staff = new()
            {
                Name = data.Name,
                Company = company,
                Profile = staffProfile
            };

            foreach (string buff in data.DeptL)
            {
                Dept? dept = await _context.Depts.FirstOrDefaultAsync(c => c.Department == buff);
                if (dept!=null)staff.Depts.Add(dept);
            }

            await _context.AddAsync(staff);
            await _context.SaveChangesAsync();

            return new OkResult();
        }
        //Удаление записи из БД
        public async Task<IActionResult> Delete(int id)
        {
            Staff? staff = await _context.Staffs.Include(c => c.Company).Include(d => d.Depts).Include(p => p.Profile).FirstOrDefaultAsync(f => f.Id == id);
            if (staff != null)
            {
                staff.Depts.Clear();
                staff.Company.Staffs.Clear();
                _context.StaffProfiles.Remove(staff.Profile);
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
            Staff? staff = await _context.Staffs.Include(c => c.Company).Include(d => d.Depts).Include(p => p.Profile).FirstOrDefaultAsync(f => f.Id == data.Id);
            if (staff != null)
            {
                if (data.Name != null) staff.Name = data.Name;
                if (data.Company != null) staff.Company = await _context.Companies.FirstOrDefaultAsync(c => c.Name == data.Company);
                if (data.Login != null) staff.Profile.Login = data.Login;
                if (data.Password != null) staff.Profile.Password = data.Password;
                if (data.DeptL != null) 
                {
                    staff.Depts.Clear();
                    foreach (string item in data.DeptL)
                    {
                        staff.Depts.Add(await _context.Depts.FirstOrDefaultAsync(d => d.Department == item));
                    }
                 }
                await _context.SaveChangesAsync();
            }
            return new ObjectResult(staff)
            {
                StatusCode = staff == null? StatusCodes.Status204NoContent : StatusCodes.Status200OK
            };
        }
        //Возвращает список строк отделов в которых находиться сотрудник для сериализации 
        public List<string> DeptToString(IList<Dept> depts)
        {
            List<string> buff = new();
            foreach (var item in depts)
            {
                buff.Add(item.Department);
            }
            return buff;
        }
    }
}