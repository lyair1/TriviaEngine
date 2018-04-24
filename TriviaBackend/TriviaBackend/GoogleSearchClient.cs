using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace TriviaBackend
{
    class GoogleSearchClient
    {
        HttpClient client;

        public GoogleSearchClient()
        {
            this.client = new HttpClient();
        }
        
        public async Task<int> SearchAndCountMatchesOnPage(string query, string answer)
        {
            string responseString = await this.MakeSearch(query);
            return Regex.Matches(responseString, answer).Count;
        }

        public async Task<int> SearchAndCountResults(string query)
        {
            string responseString = WebUtility.HtmlDecode(await this.MakeSearch(query));
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(responseString);
            HtmlNode node = doc.GetElementbyId("resultStats");
            string numString = node.InnerText;
            return int.Parse(numString.Split(' ')[1], System.Globalization.NumberStyles.AllowThousands);
        }

        private async Task<string> MakeSearch(string query)
        {
            Uri uri = new Uri(string.Format(
                "https://www.google.com/search?&q={0}",
                HttpUtility.UrlEncode(query)));
            HttpResponseMessage response = await this.client.GetAsync(uri);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
