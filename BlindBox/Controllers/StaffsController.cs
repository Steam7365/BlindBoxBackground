
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace BlindBox.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffDtoService _staffService;
        private IConfiguration Configuration { get; }

        public StaffsController(IStaffDtoService staffService, IConfiguration configuration)
        {
            _staffService = staffService;
            Configuration = configuration;
        }

        /// <summary>
        /// 获取全部的Staff数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _staffService.ModelQueryable.ToListAsync());
        }

        /// <summary>
        /// 新增一个Staff数据
        /// </summary>
        /// <param name="t">StaffDto类型的数据</param>
        /// <returns>添加后的StaffDto，失败为返回500错误</returns>
        [HttpPost]
        public async Task<ActionResult> Create(StaffDto staffDto)
        {
            var success = await _staffService.CreateAsync(staffDto);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpGet]
        public async Task<ActionResult> FuzzyStaff(string name)
        {
            var success = await _staffService.FuzzyAsync(name);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpGet]
        public async Task<ActionResult> SingStaff(int id)
        {
            var success = await _staffService.SingAsync(id);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(StaffDto staffDto)
        {
            var success = await _staffService.UpdateAsync(staffDto);
            return success != null ? Ok(success) : StatusCode(500);
        }

        [HttpDelete]
        public async Task<ActionResult> Delete(int? id)
        {
            bool success = await _staffService.DeleteAsync(id);
            return success ? Ok() : StatusCode(500);
        }

        #region 图片上传
        /// <summary>
        /// 图片上传
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            var files = Request.Form.Files;
            if (files.Count == 0)
            {
                return BadRequest("没有选择的图片！");
            }
            var file = files[0];
            // 获取上传的图片名称和扩展名称
            string fileFullName = Path.GetFileName(file.FileName);
            string fileExtName = Path.GetExtension(fileFullName);
            var fileExtNames = Configuration["ImgUp:FileExtName"].Split(',');
            if (!fileExtNames.Contains(fileExtName))
            {
                return BadRequest("选择的文件不是图片的格式！");
            }
            //获取当前项目所在的路径
            string imgPath = Configuration["ImgUp:StaffsImgPath"];

            var src = imgPath + fileFullName;
            // 如果目录不存在则要先创建
            if (!Directory.Exists(imgPath))
            {
                Directory.CreateDirectory(imgPath);
            }
            using (FileStream fs = System.IO.File.Create(src))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
            string ImgWebPath = GetCompleteUrl(src);

            //return newPath;
            return Ok(ImgWebPath);
        }

        private string GetCompleteUrl(string scr)
        {
            return new StringBuilder()
                 .Append(HttpContext.Request.Scheme)
                 .Append("://")
                 .Append(HttpContext.Request.Host)
                 .Append(scr)
                 .ToString();
        }
        #endregion
    }
}
