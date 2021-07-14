using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Interfaces;
using WebStore.Interfaces.Services;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route(APIAddress.EMPLOYEES)]
    public class EmployeesAPIController : ControllerBase
    {
        private readonly IEmployeesData _EmployeesData;

        public EmployeesAPIController(IEmployeesData employeesData) => _EmployeesData = employeesData;

        [HttpGet]
        public IActionResult GetAll()
        {
            var employees = _EmployeesData.GetList();

            if (employees is null)
                return BadRequest();

            return Ok(employees);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var employee = _EmployeesData.Get(id);

            if (employee is null)
                return BadRequest();

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            var result = _EmployeesData.Add(employee);

            if (result == -1)
                return BadRequest();

            return Ok();
        }

        [HttpPut]
        public IActionResult Update(Employee employee)
        {
            _EmployeesData.Update(employee);
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var result = _EmployeesData.Delete(id);

            if (!result)
                return BadRequest(false);

            return Ok(true);
        }
    }
}