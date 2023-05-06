using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using Autofac;
using Serilog;
using Serilog.Events;
using BlindBox.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;

#region 启动之前先注册日志用于捕获异常
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("Logs/bootlog.txt", rollingInterval: RollingInterval.Month)//启动日志按月
           .CreateBootstrapLogger();//二段式启动，没有注入服务之前启动日志先做记录
#endregion 启动之前先注册的日志

#region 日志输出
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
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region 配置

#region Swagger配置
//Swagger配置
builder.Services.AddSwaggerGen(c =>
{
    var scheme = new OpenApiSecurityScheme()
    {
        Description = "Authorization header. \r\nExample: 'Bearer 12345abcdef'",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Authorization"
        },
        Scheme = "oauth2",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
    };
    c.AddSecurityDefinition("Authorization", scheme);
    var requirement = new OpenApiSecurityRequirement();
    requirement[scheme] = new List<string>();
    c.AddSecurityRequirement(requirement);
});
#endregion

#region 跨域
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
#endregion

#region Autofac
//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<StaffDtoServer>().As<IStaffDtoService>().InstancePerDependency();
}); 
#endregion

#region BlindBox数据库配置
//数据库配置
builder.Services.AddDbContext<MyDbContext>(options =>
{
options.UseSqlServer(builder.Configuration["Data:BlindBox:ConnectionString"], b => b.MigrationsAssembly("BlindBox.EF"));
}); 
#endregion

#region Identity配置
//Identity配置
builder.Services.AddDbContext<IdentityContext>(option =>
{
    option.UseSqlServer(builder.Configuration["Data:Identity:ConnectionString:Default"], b => b.MigrationsAssembly("BlindBox.Identity"));
});
builder.Services.AddDataProtection();
//注意不是AddIdentity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;//设置密码是否必须是数字
    options.Password.RequireLowercase = false;//是否必须小写
    options.Password.RequireNonAlphanumeric = false;//是否必须非数字
    options.Password.RequireUppercase = false;//是否必须大写
    options.Password.RequiredLength = 6;//长度为6
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;//设置密码令牌
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;//设置邮箱令牌
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(BlindBox.Identity.Model.Role), builder.Services);
idBuilder.AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders().AddRoleManager<RoleManager<BlindBox.Identity.Model.Role>>()
    .AddUserManager<UserManager<User>>();

//json包配置
builder.Services.AddControllers(); 
#endregion

#region Jwt
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
#endregion

//Redis
//AutoMapper配置
builder.Services.AddAutoMapper(typeof(ModelProfile));
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
