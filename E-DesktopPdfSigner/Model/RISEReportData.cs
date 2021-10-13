using Newtonsoft.Json;
using System;


namespace DesktopPdfSigner
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class RISEReportData
    {
        [JsonProperty(PropertyName = "Id")]
        public long Id { get; set; }
        [JsonProperty(PropertyName = "CreatedTime")]
        public DateTime CreatedTime { get; set; }
        [JsonProperty(PropertyName = "ReportWordDocPath")]
        public string ReportWordDocPath { get; set; }
        [JsonProperty(PropertyName = "ReportFullPath")]
        public string ReportFullPath { get; set; }
        [JsonProperty(PropertyName = "ReportTitle")]
        public string ReportTitle { get; set; }
        [JsonProperty(PropertyName = "readDoctorUserName")]
        public string readDoctorUserName { get; set; }
        [JsonProperty(PropertyName = "patientName")]
        public string patientName { get; set; }
        [JsonProperty(PropertyName = "hospitalName")]
        public string hospitalName { get; set; }
        [JsonProperty(PropertyName = "patientId")]
        public string patientId { get; set; }

       


    }
}
