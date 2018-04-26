using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TriviaBackend
{
    class GoogleSearchClient
    {
        HttpClient client;

        public GoogleSearchClient()
        {
            this.client = new HttpClient();
            this.client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "74a710c2df10419490a82243d9a6796a");
        }
        
        public async Task<double> SearchAndCountMatchesOnPage(string query, string answer)
        {
            string responseString = await this.MakeSearch(query);

            int sum = 0;
            int count = 0;
            foreach(string word in answer.Trim().ToLower().Split(' '))
            {
                if (word.Length < 3 || Regex.Matches(query.ToLower(), word).Count > 0)
                {
                    continue;
                }
                count++;
                sum += Regex.Matches(responseString.ToLower(), word).Count;
            }

            sum += Regex.Matches(responseString.ToLower(), answer.Trim().ToLower()).Count * 10;

            return 1.0 * sum / (count + 10);
        }

        public async Task<int> SearchAndCountResults(string query)
        {
            //string responseString = WebUtility.HtmlDecode(await this.MakeSearch(query));
            //HtmlDocument doc = new HtmlDocument();
            //doc.LoadHtml(responseString);
            //HtmlNode node = doc.GetElementbyId("resultStats");
            //string numString = node.InnerText;
            //return int.Parse(numString.Split(' ')[1], System.Globalization.NumberStyles.AllowThousands);

            try
            {
                string responseString = WebUtility.HtmlDecode(await this.MakeSearch(query));
                JObject jsonObj = JObject.Parse(responseString);
                var count = int.Parse(jsonObj["webPages"]["totalEstimatedMatches"].ToString());

                return count;
            }catch(Exception e)
            {
                return 0;
            }
        }

        private async Task<string> MakeSearch(string query)
        {
            Thread.Sleep(1000);
            Uri uri = new Uri(string.Format(
                "https://api.cognitive.microsoft.com/bing/v7.0/search?q={0}",
                HttpUtility.UrlEncode(query)));
            HttpResponseMessage response = await this.client.GetAsync(uri);
            string res = await response.Content.ReadAsStringAsync();
            return res;
        }
    }
}
