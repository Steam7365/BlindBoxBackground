
namespace BlindBox.Controllers
{
    [Route("api/[controller]/[action]")]
    //[Authorize]
    [ApiController]
    public class StaffsController : ControllerBase
    {
        private readonly IStaffDtoService _staffService;
        
        public static string Img;
        private IConfiguration Configuration { get; }

        public StaffsController(IStaffDtoService staffService, IConfiguration configuration)
        {
            _staffService = staffService;
            Configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return Ok(await _staffService.ModelQueryable.ToListAsync());
        }

        [HttpPost]
        public async Task<StaffDto> Create(StaffDto staffDto)
        {
            var success = await _staffService.CreateAsync(staffDto);
            return success;
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
            Img = GetCompleteUrl(src);
            
            //return newPath;
            return Ok(src);
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
