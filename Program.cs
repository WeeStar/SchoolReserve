using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SixLaborsCaptcha.Mvc.Core;
using SqlSugar;
using Wee_XYYY.Extensions;
using Wee_XYYY.Model.Common;


var builder = WebApplication.CreateBuilder(args);

// 初始化appsetting
AppSettings.Init(builder.Configuration);
// 初始化redis服务
RedisServer.Initalize();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// 验证码
builder.Services.AddSixLabCaptcha(x=>{
    x.DrawLines=4;

    x.TextColor= new SixLabors.ImageSharp.Color[4]
    {
        SixLabors.ImageSharp.Color.Blue,
        SixLabors.ImageSharp.Color.Red,
        SixLabors.ImageSharp.Color.Black,
        SixLabors.ImageSharp.Color.Green
    };
});

//注册上下文：AOP里面可以获取IOC对象
builder.Services.AddHttpContextAccessor();


//注册SqlSugar用AddScoped
//注册SqlSugar
builder.Services.AddSingleton<ISqlSugarClient>(s =>
{
    SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
    {
        DbType = SqlSugar.DbType.MySql,
        ConnectionString = builder.Configuration.GetConnectionString("DefaultConnection"),
        // ConnectionString = builder.Configuration.GetConnectionString("DevConnection"),
        IsAutoCloseConnection = true,
    },
    db =>
    {
        //每次上下文都会执行

        //获取IOC对象不要求在一个上下文
        //vra log=s.GetService<Log>()

        //获取IOC对象要求在一个上下文
        //var appServive = s.GetService<IHttpContextAccessor>();
        //var log= appServive?.HttpContext?.RequestServices.GetService<Log>();

        db.Aop.OnLogExecuting = (sql, pars) =>
        {

        };
    });
    return sqlSugar;
});

//添加JWT身份验证服务
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,//是否效验Issuer 
                ValidateAudience = true,//是否效验Audience
                ValidateLifetime = true,//是否验证失效时间
                ValidateIssuerSigningKey = true,//是否效验SigningKey
                ValidIssuer = TokenConfig.Issuer,//颁发者
                ValidAudience = TokenConfig.Audience,//接收者 
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(TokenConfig.Secret))
            };
        });
        
//配置Swagger身份验证输入（可选）
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入token,格式为 【Bearer JWT字符串】（注意中间必须有空格）",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    //添加安全要求
    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme{
                Reference =new OpenApiReference{
                    Type = ReferenceType.SecurityScheme,
                    Id ="Bearer"
                }
            },new string[]{ }
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
 
app.UseAuthorization();

app.MapControllers();

app.Run();


