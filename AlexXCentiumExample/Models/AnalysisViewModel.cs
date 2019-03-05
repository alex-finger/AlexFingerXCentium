using Microsoft.AspNetCore.Html;
using System.Collections.Generic;

namespace AlexXCentiumExample.Models
{
    public class AnalysisViewModel
    {
        public HtmlString CarouselHtml;

        public int WordCount;

        public int UniqueWordCount;

        public List<FrequentWord> MostFrequentWords;
    }
}