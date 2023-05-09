
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BlindBox.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class DescribeTypeController : ControllerBase
    {
        private readonly IDescribeTypeDtoService _service;
        private IConfiguration Configuration { get; }

        public DescribeTypeController(IDescribeTypeDtoService service, IConfiguration configuration)
        {
            _service = service;
            Configuration = configuration;
        }

        /// <summary>
        /// 获取全部的DescribeType数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _service.ModelQueryable.ToListAsync());
        }

        /// <summary>
        /// 新增一个DescribeType数据
        /// </summary>
        /// <param name="t">DescribeTypeDto类型的数据</param>
        /// <returns>添加后的DescribeTypeDto，失败为返回500错误</returns>
        [HttpPost]
        public async Task<ActionResult> Create(DescribeTypeDto describeTypeDto)
        {
            var success = await _service.CreateAsync(describeTypeDto);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpGet]
        public async Task<ActionResult> FuzzyDescribeType(string name)
        {
            var success = await _service.FuzzyAsync(name);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpGet]
        public async Task<ActionResult> SingDescribeType(int id)
        {
            var success = await _service.SingAsync(id);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(DescribeTypeDto describeTypeDto)
        {
            var success = await _service.UpdateAsync(describeTypeDto);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            bool success = await _service.DeleteAsync(id);
            return success ? Ok() : StatusCode(500);
        }
    }
}
