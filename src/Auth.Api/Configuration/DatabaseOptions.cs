namespace Auth.Api.Configuration
{
    public class DatabaseOptions
    {
        public const string SectionName = "DatabaseOptions";

        public required string ConnectionString { get; set; }
    }
}
