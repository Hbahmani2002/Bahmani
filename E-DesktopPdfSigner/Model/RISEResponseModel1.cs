using Newtonsoft.Json;

namespace DesktopPdfSigner
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RISEResponseModel<T>
    {
        [JsonProperty(PropertyName = "Type")]
        public string Type { get; set; }
        [JsonProperty(PropertyName = "Message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "StackTrace")]
        public string StackTrace { get; set; }
        [JsonProperty(PropertyName = "Data")]
        public  T Data { get; set; }
    }
}
