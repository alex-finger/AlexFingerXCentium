using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using AlexXCentiumExample.Models;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Html;
using System.Text.RegularExpressions;

namespace AlexXCentiumExample.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Analysis()
        {
            AnalysisViewModel model = new AnalysisViewModel();
            try
            {
                // Encase in try catch statement for primitive error handling
                // Instantiate varriables
                string url = Request.Query["url"].ToString(); // Grab Query String param
                Uri siteUri = new Uri(url); // Format into Uri Object -- often this is where failures occur
                HtmlDocument page = new HtmlWeb().Load(siteUri); // Load page using HtmlAgilityPack Library
                HtmlNodeCollection images = page.DocumentNode.SelectNodes("//img"); // Get images on page
                HtmlNode body = page.DocumentNode.SelectSingleNode("//body"); // Get body content

                // Carousel
                var carouselhtml = "";
                if (images != null)
                {
                    foreach (HtmlNode node in images)
                    {
                        // account for relative url srcs
                        if (!node.Attributes["src"].Value.Contains(siteUri.Host))
                        {
                            node.Attributes["src"].Value = siteUri + "/" + node.Attributes["src"].Value;
                        }
                        //build carousel html
                        carouselhtml += string.Format("<div class=\"slide\">{0}</div>", node.OuterHtml);
                    }
                }
                //set carousel html in view model
                model.CarouselHtml = new HtmlString(carouselhtml);


                // Word Count
                string trim = body.InnerText.Trim();
                if (string.IsNullOrEmpty(trim)) // if body text is empty -> zero words
                    model.WordCount = 0; //set word count in view model

                string[] words = Regex.Split(trim, "\\s+");  // split string around spaces
                model.WordCount = words.Length; //set word count in view model


                // Unique Words
                var wordCount =
                    from word in words
                    group word by word into g
                    orderby g.Count() descending
                    select new { Word = g.Key, Count = g.Count() }; // Use LINQ Query in order to get AnonymousType response

                model.UniqueWordCount = wordCount.Count(); //set unique word count in view model

                // Most Frequent Words
                List<FrequentWord> topten = new List<FrequentWord>();
                foreach (var freq in wordCount.Take(10)) // Get first 10 unique words
                {
                    // Add new FrequentWord to the array
                    topten.Add(new FrequentWord { Word = freq.Word, Count = freq.Count });
                }

                model.MostFrequentWords = topten; //set frequent words in view model
            }
            catch (Exception ex)
            {
                // Primitive Error Handling -- Simply renders a view with a message in a ViewModel
                ErrorViewModel error = new ErrorViewModel { Message = ex.Message };
                return View("Error", error);
            }

            return View("Analysis", model);
        }
        
    }

   

   
}
