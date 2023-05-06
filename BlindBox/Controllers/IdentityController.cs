using BilndBox.Dto.Identity;
using BlindBox.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace Identity_WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly RoleManager<Role> roleManager;
        private readonly UserManager<User> userManager;
        public IdentityController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }


        /// <summary>
        /// 创建角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateRole(string roleName)
        {
            bool roleExists = await roleManager.RoleExistsAsync(roleName);//判断是否存在这个角色
            if (!roleExists)
            {
                Role role = new Role { Name = roleName };
                var r = await roleManager.CreateAsync(role);
                //创建失败返回错误信息
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            else
            {
                return BadRequest("角色已存在");
            }
            return Ok("创建成功");
        }

        /// <summary>
        /// 创建用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CreateUser(UserDto user)
        {
            bool roleExists = await roleManager.RoleExistsAsync(user.RoleName);//判断是否存在这个角色
            if (!roleExists)
            {
                return BadRequest("角色不存在");
            }
            User findUser = await this.userManager.FindByNameAsync(user.UserName);//获取某用户信息
            if (findUser == null)
            {
                findUser = new User
                {
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    PhoneNumberConfirmed = true,
                    EmailConfirmed = true
                };

                var r = await userManager.CreateAsync(findUser,user.Password);
                //创建用户信息失败返回错误信息
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
                //将角色和用户进行关联
                r = await userManager.AddToRoleAsync(findUser,user.RoleName);
                if (!r.Succeeded)
                {
                    return BadRequest(r.Errors);
                }
            }
            else
            {
                return BadRequest("用户已存在");
            }
            return Ok("创建成功");
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest req,
                    [FromServices] IOptions<JWTOptions> jwtOptions)
        {
            string userName = req.UserName;
            string password = req.Password;
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {
                return NotFound($"用户名不存在{userName}");
            }
            var success = await userManager.CheckPasswordAsync(user, password);
            if (!success)
            {
                return BadRequest("Failed");
            }
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            var roles = await userManager.GetRolesAsync(user);
            foreach (string role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //登录成功后生成令牌，将令牌发送给客户端
            string jwtToken = JWTServer.BuildToken(claims, jwtOptions.Value);
            return Ok(jwtToken);
        }
    }
}
