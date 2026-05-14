using Auth.Api.Configuration;

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

            _builder.Services.AddControllers();
        }

        private void InitConfigs()
        {
            _builder.Services.Configure<GoogleAuthConfig>(_builder.Configuration.GetSection(GoogleAuthConfig.SectionName));
        }
    }
}
