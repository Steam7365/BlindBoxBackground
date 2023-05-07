using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BlindBox.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class GradeController : ControllerBase
    {
        private readonly IGradeDtoService _gradeDto;
        private IConfiguration Configuration { get; }

        public GradeController(IGradeDtoService gradeDto, IConfiguration configuration)
        {
            _gradeDto = gradeDto;
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _gradeDto.ModelQueryable.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> Create(GradeDto gradeDto)
        {
            var success = await _gradeDto.CreateAsync(gradeDto);
            return Ok(success);
        }

        [HttpGet]
        public async Task<ActionResult> SingGrade(string name)
        {
            var success = await _gradeDto.FuzzyAsync(name);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(GradeDto gradeDto)
        {
            var success = await _gradeDto.UpdateAsync(gradeDto);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            bool success = await _gradeDto.DeleteAsync(id);
            return success ? Ok() : StatusCode(500);
        }
    }
}
