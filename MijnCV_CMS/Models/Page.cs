namespace MijnCV_CMS.Models
{
    public class Page
    {
        [Newtonsoft.Json.JsonProperty("Id", NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
        public int Id { get; set; }

        public int UserID { get; set; }
        public string Name { get; set; }
    }
}