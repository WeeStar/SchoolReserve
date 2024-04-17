using Microsoft.AspNetCore.Mvc.ModelBinding;
using SqlSugar;

namespace Wee_XYYY.Model;

[SugarTable("xyyy_reservation")]
public class Reservation
{
    //数据是自增需要加上IsIdentity 
    //数据库是主键需要加上IsPrimaryKey 
    //注意：要完全和数据库一致2个属性
    [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    /// <summary>
    /// 状态
    /// 1.有效 0.过期
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "status")]
    public int Status { get; set; }

    /// <summary>
    /// 活动ID
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "active_id")]
    public long ActiveId { get; set; }


    /// <summary>
    /// 用户ID
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "user_id")]
    public long UserId { get; set; }

    [SugarColumn(ColumnName = "id_code")]
    public string IdCode { get; set; }


    /// <summary>
    /// 预约人
    /// </summary> 
    [SugarColumn(ColumnName = "rev_name")]
    public string ReName { get; set; }

    /// <summary>
    /// 预约身份证号
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "rev_id_code")]
    public string ReIdCode { get; set; }

    /// <summary>
    /// 预约时间
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "re_date")]
    public DateTime ReDate { get; set; }

    [SugarColumn(ColumnName = "create_time")]
    public DateTime CreateTime { get; set; }
}

public class ReservationAddModel
{
    [BindRequired]
    public string ReIdCode { get; set; }

    [BindRequired]
    public string ReName { get; set; }
    
    [BindRequired]
    public DateTime ReDate { get; set; }
}
