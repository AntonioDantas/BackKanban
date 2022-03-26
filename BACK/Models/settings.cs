namespace BACK.Models
{
    /// <summary>
    /// Variáveis de Configuração.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Key para JWT.
        /// </summary>
        public string Secret { get; set; }
        /// <summary>
        /// Tempo de Expiração.
        /// </summary>
        public int ExpiresTimeInHour { get; set; }
        /// <summary>
        /// Nome do sistema.
        /// </summary>
        public string Issuer { get; set; }
    }

    /// <summary>
    /// Variáveis de Acesso.
    /// </summary>
    public class AppAccess
    {
        /// <summary>
        /// Login de acesso.
        /// </summary>
        public string login { get; set; }
        /// <summary>
        /// Token de acesso.
        /// </summary>
        public string senha { get; set; }
    }
}
