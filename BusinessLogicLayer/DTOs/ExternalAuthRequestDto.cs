using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace RealTimeChatApi.BusinessLogicLayer.DTOs
{
    public class ExternalAuthRequestDto
    {
        public const string PROVIDER = "google";

        [JsonProperty("idToken")]
        [Required]
        public string IdToken { get; set; }
    }
}
