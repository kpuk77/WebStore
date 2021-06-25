using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

namespace WebStore.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValuesController : ControllerBase
    {
        private static readonly List<string> __Values = Enumerable
            .Range(1, 10)
            .Select(v => $"Value-{v}")
            .ToList();

        [HttpGet]
        public IActionResult Get() => Ok(__Values);

        [HttpGet("{index}")]
        public IActionResult GetByIndex(int index)
        {
            if (index < 0 || index > __Values.Count)
                return BadRequest();

            return Ok(__Values[index]);
        }

        [HttpPost]
        public IActionResult Add(string value)
        {
            if (value is null)
                return BadRequest();

            __Values.Add(value);

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(int index)
        {
            if (index > __Values.Count || index < 0)
                return BadRequest();

            __Values.RemoveAt(index);
            return Ok();
        }

        [HttpPut("update/{index}")]
        public IActionResult Update(int index, string value)
        {
            if (index > __Values.Count || index < 0 || value is null)
                return BadRequest();

            __Values[index] = value;
            return Ok();
        }
    }
}
