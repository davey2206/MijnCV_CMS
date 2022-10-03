using Newtonsoft.Json;

namespace MijnCV_CMS.Models
{
    public class Section
    {
        [Newtonsoft.Json.JsonProperty("Id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int ID { get; set; }

        public string CV { get; set; }
        public string? Title { get; set; }
        public string? Paragraph { get; set; }
        public string? Image { get; set; }
        public int Layout { get; set; }
        public int Position { get; set; }
        public int PageID { get; set; }
    }
}