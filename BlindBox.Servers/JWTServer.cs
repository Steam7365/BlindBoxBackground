using BilndBox.Dto.Help;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlindBox.Servers.Help
{
    public class JWTServer
    {
        public static string BuildToken(IEnumerable<Claim> claims, JWTOptions options)
        {
            DateTime expires = DateTime.Now.AddSeconds(options.ExpireSeconds);
            byte[] keyBytes = Encoding.UTF8.GetBytes(options.SigningKey);
            var secKey = new SymmetricSecurityKey(keyBytes);//获取对称安全密钥
            var credentials = new SigningCredentials(secKey,
                SecurityAlgorithms.HmacSha256Signature);//令牌的加密方式
            var tokenDescriptor = new JwtSecurityToken(expires: expires,
                signingCredentials: credentials, claims: claims);//完成令牌的制作
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);//返回完成令牌数据
        }
    }
}
