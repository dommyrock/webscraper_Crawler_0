using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newglee
{
    public class CrawledURL
    {
        public string Url { get; set; }
        public bool IsChecked { get; set; }

        public List<string> FoundKeywords { get; private set; }

        public CrawledURL(string url, bool isChecked)
        {
            this.Url = url;
            this.IsChecked = isChecked;
            this.FoundKeywords = new List<string>();
        }

        public bool UrlContainsKeyword(List<string> keywords)//checks if crawled url has any of the keys
        {
            foreach (var kw in keywords)
            {
                if (this.FoundKeywords.Contains(kw))
                    return true;
            }

            return false;
        }
    }
}