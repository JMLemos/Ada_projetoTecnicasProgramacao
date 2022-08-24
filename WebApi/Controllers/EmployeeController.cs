using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmployeeWebApi.Models;

namespace EmployeeWebApi.Controllers
{
    [ApiController]

    [Route("employee")]
    public class EmployeeController : ControllerBase
    {
       
        private readonly ILogger<EmployeeController> _logger;
        private static List<Employee> employee;

        public EmployeeController(ILogger<EmployeeController> logger)
        {
            _logger = logger;
            
            using var reader = new StreamReader("./data.json");
            var json = reader.ReadToEnd();
            employee = JsonSerializer.Deserialize<List<Employee>>(json);
        }

        [HttpGet]
        public IEnumerable<Employee> Get()
        {
            return employee.OrderBy(x=> x.Id);
        }

        [HttpGet("gender")]
        public IEnumerable<Employee> GetGenderType(string type)
        {
            var genderTypeMasc = employee.Where(x => x.Gender == "M").ToList();
            var genderTypeFem = employee.Where(x => x.Gender == "F").ToList();

            if (type == "M")
            {
                return genderTypeMasc;
            }
            return genderTypeFem;

        }

        [HttpGet("age")]
        public IEnumerable<Employee> GetAge()
        {
            var age = employee.Where(x => x.Age >= 18).ToList().OrderBy(x => x.Age);
           
            return age;

        }

        [HttpGet("email")]
        public IEnumerable<Employee> GetEmail(string emailType)
        {
            var email = employee.Where(x => x.Email.Contains(emailType)).OrderBy(x => x.Id).ToList();

            return email;

        }

      
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employeeCreate)
        {

            employee.Add(employeeCreate);

            var serializer = JsonSerializer.Serialize(employee);
            System.IO.File.WriteAllTextAsync("./data.json", serializer);

            return Ok(employee);
        }

        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee(int id, Employee employeeUpdate)
        {
                      
            var search = employee.Find(x => x.Id == id);

            if (search == null)
               return BadRequest("EMPREGADO NÃO EXISTE!");
            
            search.FirstName = employeeUpdate.FirstName;
            search.LastName = employeeUpdate.LastName;
            search.Gender = employeeUpdate.Gender;
            search.Email = employeeUpdate.Email;
            search.Age = employeeUpdate.Age;
            search.Login = employeeUpdate.Login;

           
            
            var serializer = JsonSerializer.Serialize(employee);
            System.IO.File.WriteAllTextAsync("./data.json", serializer);

            return Ok(employee);

        }


        [HttpDelete]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            var search = employee.Find(x => x.Id == id);

            if (search == null)
                return BadRequest("EMPREGADO NÃO EXISTE!");

            var result = employee.Remove(search);

            var serializer = JsonSerializer.Serialize(employee);
            System.IO.File.WriteAllTextAsync("./data.json", serializer);

            return Ok(result);
        }         
    }
}