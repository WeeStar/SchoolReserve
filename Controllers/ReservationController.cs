using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;
using Wee_XYYY.Model;

namespace Wee_XYYY.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class ReservationController : ControllerBase
{
    private readonly ILogger<ReservationController> _logger;
    private readonly ISqlSugarClient _db;

    public ReservationController(ILogger<ReservationController> logger, ISqlSugarClient db)
    {
        _logger = logger;
        _db = db;
    }

    [HttpGet(Name = "MyList")]
    public async Task<IActionResult> MyList(int pageIndex = 1, int pageSize = 10)
    {
        // 获取身份证号
        var claimsPrincipal = this.HttpContext.User;
        var idCode = claimsPrincipal.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Name)?.Value;

        // 分页查询
        RefAsync<int> total = 0;
        var resList = await _db.Queryable<Reservation>().Where(e => e.IdCode == idCode).OrderByDescending(e => e.ReDate).ToPageListAsync(pageIndex, pageSize, total);

        return Ok(new PaginatedViewModel<Reservation>(pageIndex, pageSize, total, resList));
    }

    [HttpPost(Name = "Add")]
    public async Task<IActionResult> Add([FromBody] ReservationAddModel info)
    {
        // 获取身份证号 用户ID
        var claimsPrincipal = this.HttpContext.User;
        var userId = claimsPrincipal.Claims.FirstOrDefault(r => r.Type == "ID")?.Value;
        var idCode = claimsPrincipal.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Name)?.Value;

        // 构造实体
        var entity = new Reservation
        {
            ActiveId = 1,
            UserId = Convert.ToInt64(userId),
            IdCode = idCode,
            ReIdCode = info.ReIdCode,
            ReName = info.ReName,
            ReDate = info.ReDate.Date,
            CreateTime = DateTime.Now,
        };
        var insert = _db.Insertable(entity);
        await insert.ExecuteCommandAsync();

        return Ok(entity);
    }

    [HttpGet(Name = "Stat")]
    public async Task<IActionResult> Statistic()
    {
        // 获取角色
        var claimsPrincipal = this.HttpContext.User;
        var role = claimsPrincipal.Claims.FirstOrDefault(r => r.Type == ClaimTypes.Role)?.Value;
        
        // 非管理员不可用
        if (role == null || !role.Equals("Admin"))
        {
            return Ok(new List<dynamic>());
        }

        var getOrderBy = await _db.Queryable<Reservation>()
            .GroupBy(it => it.ReDate.Date)
            .Select(it => new
            {
                ReDate = it.ReDate.Date,
                Count = SqlFunc.AggregateCount(it.Id)
            })
            .ToListAsync();

        return Ok(getOrderBy);
    }
}
