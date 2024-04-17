using SqlSugar;

namespace Wee_XYYY.Model;

/// <summary>
/// 用户表
/// </summary>
[SugarTable("xyyy_users")]
public class User
{
    //数据是自增需要加上IsIdentity 
    //数据库是主键需要加上IsPrimaryKey 
    //注意：要完全和数据库一致2个属性
    [SugarColumn(ColumnName = "id", IsPrimaryKey = true, IsIdentity = true)]
    public long Id { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    /// <value></value>
    [SugarColumn(ColumnName = "id_code")]
    public string IdCode { get; set; }

    // [SugarColumn(ColumnName = "name")]
    // public string Name { get; set; }

    [SugarColumn(ColumnName = "create_time")]
    public DateTime CreateTime { get; set; }

    [SugarColumn(ColumnName = "is_admin")]
    public bool IsAdmin { get; set; }
}


/// <summary>
/// 登录用户模型
/// </summary>

public class LoginUser
{
    public string IdCode { get; set; }
    
    // public string Name { get; set; }

    public string VerifyCode { get; set; }

    public string VerifyKey { get; set; }
}
