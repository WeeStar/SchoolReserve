using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Wee_XYYY.Extensions;

public class JwtHelper
{
    public static string GenerateJsonWebToken(JwtUser userInfo)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.Secret));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
        claimsIdentity.AddClaim(new Claim("ID", userInfo.Id));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Name, userInfo.UserName));
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, userInfo.UserRole));
        var token = new JwtSecurityToken(TokenConfig.Issuer,
          TokenConfig.Audience,
          claimsIdentity.Claims,
          expires: DateTime.Now.AddMinutes(TokenConfig.AccessExpiration),
          signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
public class TokenConfig
{
    public const string Issuer = "WeeStarIssuer";//颁发者        
    public const string Audience = "WeeStarAudience";//接收者    
    public const string Secret = "fdc894f3d26f0297a506a3e450ceeacea874803506c70c91fbd3b15f971eebdb";//签名秘钥        
    public const int AccessExpiration = 30;//AccessToken过期时间（分钟）
}


public class JwtUser
{
    public string Id { get; set;}

    public string UserName { get; set; }

    public string UserRole { get; set; }
}
