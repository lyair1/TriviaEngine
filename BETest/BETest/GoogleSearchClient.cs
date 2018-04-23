using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Net;
using System.Xml;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.XPath;

namespace BETest
{
    class GoogleSearchClient
    {
        HttpClient client;

        public GoogleSearchClient()
        {
            this.client = new HttpClient();
        }

        public async Task<int> SearchAndCountResults(string query)
        {
            // This old one was using the Search API
            /*Uri uri = new Uri(string.Format(
                "https://www.googleapis.com/customsearch/v1?key=AIzaSyBE4I5atSm6zw4493xpn0k00-x-XbMZMjA&cx=007906809693880875883:rnzjkgcav2e&q={0}",
                HttpUtility.UrlEncode(query)));
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseString);
            JObject j = JObject.Parse(responseString);
            Console.WriteLine(obj);
            return obj.searchInformation.totalResults;*/

            Uri uri = new Uri(string.Format(
                "https://www.google.com/search?&q={0}",
                HttpUtility.UrlEncode(query)));
            HttpResponseMessage response = await this.client.GetAsync(uri);
            string responseString = await response.Content.ReadAsStringAsync();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(WebUtility.HtmlDecode(responseString));
            HtmlNode node = doc.GetElementbyId("resultStats");
            string numString = node.InnerText;
            Console.WriteLine(numString);
            return int.Parse(numString.Split(' ')[1], System.Globalization.NumberStyles.AllowThousands);
        }
    }
}
