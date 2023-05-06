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

#region ����֮ǰ��ע����־���ڲ����쳣
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("Logs/bootlog.txt", rollingInterval: RollingInterval.Month)//������־����
           .CreateBootstrapLogger();//����ʽ������û��ע�����֮ǰ������־������¼
#endregion ����֮ǰ��ע�����־

#region ��־���
Log.Information("Starting web host");//��־��ʼ
Log.Information("������־��ʼ��¼");//��־��ʼ
var builder = WebApplication.CreateBuilder(args);//����Builder �����ر���Ҫ����Ҫ��λ�ã���������������������

Log.Information("builder���񴴽�����");//��־��ʼ

builder.Logging.ClearProviders();//���Ĭ��log��־���
builder.Host.UseSerilog((context, services, configuration) => configuration
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .Enrich.FromLogContext()
#if DEBUG
                //�ٷ���˵����debug������ķ����ܣ�����debug�����־�Ĳ�Ҫ�ӵ���ʽ������
                .MinimumLevel.Override("BlindBox.Controllers", LogEventLevel.Debug)//��debug
                .WriteTo.Debug()//��debug��
#endif
);//����ʽע��Serilog��־�����������������ļ�ע�� 
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region ����

#region Swagger����
//Swagger����
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

#region ����
//web api����
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

#region BlindBox���ݿ�����
//���ݿ�����
builder.Services.AddDbContext<MyDbContext>(options =>
{
options.UseSqlServer(builder.Configuration["Data:BlindBox:ConnectionString"], b => b.MigrationsAssembly("BlindBox.EF"));
}); 
#endregion

#region Identity����
//Identity����
builder.Services.AddDbContext<IdentityContext>(option =>
{
    option.UseSqlServer(builder.Configuration["Data:Identity:ConnectionString:Default"], b => b.MigrationsAssembly("BlindBox.Identity"));
});
builder.Services.AddDataProtection();
//ע�ⲻ��AddIdentity
builder.Services.AddIdentityCore<User>(options =>
{
    options.Password.RequireDigit = false;//���������Ƿ����������
    options.Password.RequireLowercase = false;//�Ƿ����Сд
    options.Password.RequireNonAlphanumeric = false;//�Ƿ���������
    options.Password.RequireUppercase = false;//�Ƿ�����д
    options.Password.RequiredLength = 6;//����Ϊ6
    options.Tokens.PasswordResetTokenProvider = TokenOptions.DefaultProvider;//������������
    options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultEmailProvider;//������������
});
var idBuilder = new IdentityBuilder(typeof(User), typeof(BlindBox.Identity.Model.Role), builder.Services);
idBuilder.AddEntityFrameworkStores<IdentityContext>()
    .AddDefaultTokenProviders().AddRoleManager<RoleManager<BlindBox.Identity.Model.Role>>()
    .AddUserManager<UserManager<User>>();

//json������
builder.Services.AddControllers(); 
#endregion

#region Jwt
//JWT
builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(x =>
{
    //�������ļ���JWT����ת��ΪJWToptions�����
    var jwtOpt = builder.Configuration.GetSection("JWT").Get<JWTOptions>();
    //�����Գư�ȫ��Կ
    byte[] keyBytes = Encoding.UTF8.GetBytes(jwtOpt.SigningKey);
    var secKey = new SymmetricSecurityKey(keyBytes);
    //������֤��������
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
//AutoMapper����
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
