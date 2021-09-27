// solr
using SolrNet.Attributes;

namespace SolrNET5WebAPI.Api.Models
{
    public class WeatherForecast
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("temperature")]
        public int Temperature { get; set; }

        [SolrField("summary")]
        public string Summary { get; set; }
    }
}