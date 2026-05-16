namespace Auth.Api.Configuration
{
    public class GoogleAuthConfig
    {
        public const string SectionName = "GoogleAuth";

        /// <summary>
        /// Словарь конфигураций для разных сайтов
        /// Ключ - идентификатор сайта (например, "site1", "site2")
        /// </summary>
        public Dictionary<string, GoogleAuthSiteConfig> Sites { get; init; }
    }

    public class GoogleAuthSiteConfig
    {
        public string ClientId { get; init; }

        public string ClientSecret { get; init; }

        public string ApplicationName { get; init; }

        public string RedirectUrl { get; init; }

        /// <summary>
        /// Дополнительные параметры для конкретного сайта
        /// </summary>
        public Dictionary<string, string> AdditionalParams { get; init; }
    }
}
