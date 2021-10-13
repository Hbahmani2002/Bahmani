using Newtonsoft.Json;
using System;


namespace DesktopPdfSigner
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RISEReportfileData
    {
        [JsonProperty(PropertyName = "fileData")]
        public byte[] fileData { get; set; }

    }
}
