using Auth.Api.AppStart.Extensions;
using Auth.Api.BLL.Abstract;
using Auth.Api.BLL.Services;
using Auth.Api.Configuration;
using Auth.Api.DAL;

namespace Auth.Api.AppStart
{
    public class Startup
    {
        private WebApplicationBuilder _builder;

        public Startup(WebApplicationBuilder builder)
        {
            _builder = builder;
        }

        public void Initialize()
        {
            if (_builder.Environment.IsDevelopment())
            {
                _builder.Services.AddSwaggerGen();
            }

            InitConfigs();            
            ConfigureServices();
            SetupDb();

            _builder.Services.AddControllers();
        }

        private void InitConfigs()
        {
            if (!_builder.Environment.IsDevelopment())
            {
                _builder.Configuration.AddKeyPerFile("/run/secrets", optional: true);
            }

            _builder.Services.Configure<DatabaseOptions>(_builder.Configuration.GetSection(DatabaseOptions.SectionName));
            _builder.Services.Configure<GoogleAuthConfig>(_builder.Configuration.GetSection(GoogleAuthConfig.SectionName));

            _builder.Services.AddOptions<JwtConfig>()
                .Bind(_builder.Configuration.GetSection(JwtConfig.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();
        }

        private void SetupDb()
        {
            _builder.Services.AddDAL(_builder.Configuration);
        }

        private void ConfigureServices()
        {
            CorsExtensions.ConfigureCors(_builder.Services);
            _builder.Services.AddScoped<IAuthService, AuthService>();
            _builder.Services.AddScoped<IJwtProvider, JwtProvider>();
            _builder.Services.AddScoped<GoogleAuthService>();
            _builder.Services.AddHttpClient<GoogleAuthService>();
        }
    }
}
