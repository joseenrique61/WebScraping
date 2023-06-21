using HtmlAgilityPack;
using ScrapySharp.Extensions;
using System.Globalization;

namespace WebScraping
{
    internal class Program
    {
        static void Main(string[] args)
        {
            HtmlWeb oWeb = new();
            HtmlDocument doc = oWeb.Load("https://holamundo.day/");
            IEnumerable<HtmlNode> nodes = doc.DocumentNode.CssSelect("article");
            HtmlNode node = nodes.First();

            foreach (HtmlNode tempNode in nodes)
            {
                if (tempNode.InnerHtml.Contains("Agenda 4 de mayo"))
                {
                    node = tempNode;
                }
            }

            Dictionary<DateTime, string> datos = new();

            string date = "4";
            bool primerDia = true;

            foreach (HtmlNode tempNode in node.CssSelect("blockquote"))
            {
                List<HtmlNode> spanNodes = tempNode.CssSelect("span").ToList();

                string fullString = "";

                for (int i = 0; i < spanNodes.Count; i++)
                {
                    HtmlNode spanNode = spanNodes[i];
                    if (spanNode.InnerHtml.StartsWith("<"))
                    {
                        continue;
                    }
                    fullString += spanNode.InnerText.Trim();
                }


                string[] split = fullString.Split("|", StringSplitOptions.RemoveEmptyEntries);
                datos.Add(DateTime.ParseExact($"{date}/05/23 {split[0]}", "d/MM/yy HH:mm", CultureInfo.InvariantCulture), split[1]);

                if (split[0] == "21:00" && primerDia)
                {
                    primerDia = false;
                    date = "8";
                }
            }

            for (int i = 0; i < datos.Count;i++)
            {
                Console.WriteLine($"{datos.Keys.ToList()[i]}: {datos.Values.ToList()[i]}");
            }
            Console.ReadLine();
        }
    }
}