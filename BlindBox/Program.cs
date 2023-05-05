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

#region ����֮ǰ��ע����־���ڲ����쳣
Log.Logger = new LoggerConfiguration()
           .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
           .Enrich.FromLogContext()
           .WriteTo.Console()
           .WriteTo.File("Logs/bootlog.txt", rollingInterval: RollingInterval.Month)//������־����
           .CreateBootstrapLogger();//����ʽ������û��ע�����֮ǰ������־������¼
#endregion ����֮ǰ��ע�����־

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

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterType<StaffDtoImplement>().As<IStaffDtoService>().InstancePerDependency();
});
//AutoMapper
builder.Services.AddAutoMapper(typeof(ModelProfile));

//���ݿ�����
builder.Services.AddDbContext<MyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration["Data:BlindBox:ConnectionString"], b => b.MigrationsAssembly("BlindBox.EF"));
});

//json������
builder.Services.AddControllers();

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
