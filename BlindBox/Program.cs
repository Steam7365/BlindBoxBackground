using Autofac.Extensions.DependencyInjection;
using BlindBox.Models.HelpModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StackExchange.Redis;
using BilndBox.Dto;
using Autofac;
using BlindBox.IServers.IDtoServers;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Compact;

#region 启动之前先注册日志用于捕获异常
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("Logs/bootlog.txt", rollingInterval: RollingInterval.Month)//启动日志按月
           .CreateBootstrapLogger();//二段式启动，没有注入服务之前启动日志先做记录
#endregion 启动之前先注册的日志

Log.Information("Starting web host");//日志开始
Log.Information("程序日志开始记录");//日志开始
var builder = WebApplication.CreateBuilder(args);//创建Builder 这行特别重要，不要动位置！！！！！！！！！！！

Log.Information("builder服务创建结束");//日志开始

builder.Logging.ClearProviders();//清除默认log日志组件
builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
                #if DEBUG
                //官方的说法是debug的输出耗费性能，所以debug相关日志的不要加到正式环境里
                .MinimumLevel.Override("BlindBox.Controllers", LogEventLevel.Debug)//仅debug
                .WriteTo.Debug()//仅debug下
                #endif
);//二段式注册Serilog日志组件，这里采用配置文件注册

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region 配置
//web api跨域
builder.Services.AddCors(c =>
{
    c.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<StaffDtoImplement>().As<IStaffDtoService>().InstancePerDependency();
});
//AutoMapper
builder.Services.AddAutoMapper(typeof(ModelProfile));

//数据库配置
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Data:BlindBox:ConnectionString"], b => b.MigrationsAssembly("BlindBox.EF"));
});

//json包配置
builder.Services.AddControllers();

//JWT
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    //将配置文件的JWT数据转换为JWToptions这个类
    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    //创建对称安全密钥
    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
    var secKey = new SymmetricSecurityKey(keyBytes);
    //令牌验证参数配置
    x.TokenValidationParameters = new()
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = secKey
    };
});

//Redis

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
