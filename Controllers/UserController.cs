using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Wee_XYYY.Extensions;
using Wee_XYYY.Model;

namespace Wee_XYYY.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController : ControllerBase
{
    private readonly ILogger<UserController> _logger;
    private readonly ISqlSugarClient _db;
    public UserController(ILogger<UserController> logger, ISqlSugarClient db)
    {
        _logger = logger;
        _db = db;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginUser"></param>
    /// <returns></returns>
    [HttpPost(Name = "Login")]
    public async Task<IActionResult> Login(LoginUser loginUser)
    {
        try
        {
            // 验证码为空
            if (string.IsNullOrEmpty(loginUser.VerifyCode) || string.IsNullOrEmpty(loginUser.VerifyKey))
            {
                return Problem("验证码错误");
            }

            // 
            if (string.IsNullOrEmpty(loginUser.IdCode))
            {
                return Problem("请输入身份证号");
            }

            // 获取验证码
            var verifyCodeInRedis = new RedisCache().Get<string>("vc_" + loginUser.VerifyKey);
            if (string.IsNullOrEmpty(verifyCodeInRedis))
            {
                return Problem("验证码错误");
            }
            new RedisCache().Remove<string>("vc_" + loginUser.VerifyKey);

            // 判断验证码
            if (!verifyCodeInRedis.Equals(loginUser.VerifyCode))
            {
                return Problem("验证码错误");
            }

            // 获取用户信息
            var user = await _db.Queryable<User>().FirstAsync(e => e.IdCode == loginUser.IdCode);

            // 不存在 新增
            if (user == null)
            {
                user = new User
                {
                    IdCode = loginUser.IdCode,
                    // Name = loginUser.Name,
                    CreateTime = DateTime.Now,
                    IsAdmin = false
                };
                var insert = _db.Insertable(user);
                await insert.ExecuteCommandAsync();
            }

            // token构造返回
            string token = JwtHelper.GenerateJsonWebToken(new JwtUser
            {
                Id = user.Id.ToString(),
                UserName = user.IdCode,
                UserRole = user.IsAdmin ? "Admin" : "Common"
            });
            return Ok(new { token = token, idCode = user.IdCode, userId = user.Id, isAdmin = user.IsAdmin });
        }
        catch (Exception ex)
        {
            return Problem(ex.Message, ex.StackTrace);
        }
    }
}
