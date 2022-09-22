using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System.Text.Json.Serialization;

namespace CryptoMarket.Hub.Client
{
    public class CustomUserAccount : RemoteUserAccount
    {
        [JsonPropertyName("roles")]
        public string[] Roles { get; set; } = Array.Empty<string>();
    }
}
