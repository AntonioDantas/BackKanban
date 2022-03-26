using Newtonsoft.Json;
using System;

namespace BACK.Models
{
    /// <summary>
    /// Card do Kanban.
    /// </summary>
    public class Card
    {
        /// <summary>
        /// Id do card.
        /// </summary>
        [JsonProperty("id")]
        public string id { get; set; }
        /// <summary>
        /// Título do card.
        /// </summary>
        [JsonProperty("titulo")]
        public string titulo { get; set; }
        /// <summary>
        /// Conteúdo do card.
        /// </summary>
        [JsonProperty("conteudo")]
        public string conteudo { get; set; }
        /// <summary>
        /// Lista onde se encontra o card.
        /// </summary>
        [JsonProperty("lista")]
        public string lista { get; set; }
    }
}