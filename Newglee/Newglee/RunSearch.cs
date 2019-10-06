using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Newglee
{
    public class RunSearch
    {
        #region Properties

        public bool IsActive { get; set; } = true;
        private const int CurrentUrlIndex = 0; //const index val
        public string CurrentUrlString { get; set; }
        public string InputUrl { get; set; }
        public string Result { get; set; }
        public int TotalProcessedURLs { get; set; }

        #endregion Properties

        #region Fields

        public List<string> RemovedFromQueue { get; set; }

        public List<string> ScrapedQueue;

        public Dictionary<string, CrawledURL> CheckedUrlsDictionary;

        public List<string> Keywords;
        public List<string> OutputUrlList { get; set; }//focus on for start : https://www.abrakadabra.com

        #endregion Fields

        #region Object instanciation example

        //1. nacin inicijalizacije konstruktora sa parametrima

        //CrawledURL url1 = new CrawledURL("Facebook",false);

        //2. nacin inicijalizacije konstruktora

        //CrawledURL url = new CrawledURL
        //{
        //    Url = "facebook",
        //    IsChecked = false
        //};

        //3. nacin inicijalizacije praznog konstruktora

        //CrawledURL url1 = new CrawledURL();
        //url1.Url = "Twitter";
        //url1.IsChecked = false;

        #endregion Object instanciation example

        public RunSearch(string input, string keywords)
        {
            this.InputUrl = input; //can have more than 1 , if so has to be split
            this.ScrapedQueue = new List<string>();
            this.CheckedUrlsDictionary = new Dictionary<string, CrawledURL>();
            this.ScrapedQueue = input.Split(new char[] { ' ', ',', '\n' }).ToList();
            this.Keywords = keywords.ToLower().Split(new char[] { ' ', ',' }).ToList();
            TotalProcessedURLs = 0;
            this.RemovedFromQueue = new List<string>();
        }

        public void CrawlUrl()
        {
            while (this.IsActive)
            {
                this.CurrentUrlString = this.ScrapedQueue[CurrentUrlIndex];
                this.TotalProcessedURLs++;

                ServicePointManager.Expect100Continue = true;//dodano zbog TSL/SSL exceptiona
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                if (this.CurrentUrlString != string.Empty && ValidateUri(this.CurrentUrlString)) //TODO ...change StartsWith to ..append part after / to doman ..same as in selenium scrapers
                {
                    if (!this.CheckedUrlsDictionary.ContainsKey(this.CurrentUrlString))
                    {
                        try
                        {
                            CrawledURL urlObject = new CrawledURL(this.CurrentUrlString, false);
                            urlObject.Url = this.CurrentUrlString;

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(this.CurrentUrlString);
                            request.ContentType = "text/html";
                            request.Accept = "txt/html";
                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                            {
                                using (Stream st = response.GetResponseStream())
                                using (StreamReader reader = new StreamReader(st))
                                {
                                    this.Result = reader.ReadToEnd();
                                }

                                urlObject.IsChecked = true;

                                //add containing keywords to object list
                                foreach (string keyword in Keywords)
                                {
                                    if (Regex.IsMatch(this.Result, keyword))
                                    {
                                        urlObject.FoundKeywords.Add(keyword);
                                    }
                                }

                                this.CheckedUrlsDictionary.Add(urlObject.Url, urlObject); //add checked url to list of checked urls
                            }
                        }

                        #region WebException

                        catch (WebException wex)
                        {
                            //Ignored atm , not that important for basic scraper
                            //continues if it gets exception that is not handled

                            //HttpWebResponse responseException = (HttpWebResponse)wex.Response;
                            //if (responseException == null)
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //if (responseException.StatusCode == HttpStatusCode.NotFound)//http 404
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode.ToString() == "999")//custom handler za linkedin(custom exception)
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.RequestTimeout)//http 408
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.Forbidden)//403
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.NotAcceptable)//406
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.InternalServerError)//500
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.ServiceUnavailable)//503
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.Gone)
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //else if (responseException.StatusCode == HttpStatusCode.BadGateway)
                            //{
                            //    this.InputUrls.RemoveAt(this.CurrentUrlIndex);
                            //    continue;
                            //}
                            //throw wex;
                            Debug.Print(wex.Message);
                            this.ScrapedQueue.RemoveAt(CurrentUrlIndex);
                            continue;
                        }

                        #endregion WebException
                    }
                    else
                    {
                        this.RemovedFromQueue.Add(this.CurrentUrlString);//backup removed log

                        this.ScrapedQueue.RemoveAt(CurrentUrlIndex);
                        continue;
                    }
                }
                else
                {
                    this.RemovedFromQueue.Add(this.CurrentUrlString);//backup removed log

                    this.ScrapedQueue.RemoveAt(CurrentUrlIndex);
                    continue;
                }
                //crawl source for links here
                ExtractLinks();
                //Remove already scraped link from queue
                this.ScrapedQueue.RemoveAt(CurrentUrlIndex);
            }
        }

        public void ExtractLinks()
        {
            string data = String.Join(" ", this.Result);
            Regex urlRegex = new Regex(@"(?<=\bhref="")[^""]*", RegexOptions.IgnoreCase);
            MatchCollection matches = urlRegex.Matches(data);

            foreach (Match match in matches)
            {
                this.ScrapedQueue.Add(match.Value); //insert bellow 1st Url ...
            }
        }

        private bool ValidateUri(string uriString)
        {
            //Check if uri starts with / , if so ,prepend searched site to relative uri(usefull when focusing single page)
            if (uriString.StartsWith("/") && !uriString.EndsWith("ico") && !uriString.EndsWith("png") && !uriString.EndsWith("xml"))
            {
                this.CurrentUrlString = this.InputUrl + uriString;//todo
            }

            Uri absUr;
            if (Uri.TryCreate(this.CurrentUrlString, UriKind.Absolute, out absUr) && (absUr.Scheme == Uri.UriSchemeHttp || absUr.Scheme == Uri.UriSchemeHttps))
            {
                absUr = new Uri(this.CurrentUrlString);

                this.CurrentUrlString = absUr.AbsoluteUri;
            }
            else
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns list of sites that contain at least one of searched keywords
        /// </summary>
        /// <returns>List<string></string>s</returns>
        public List<string> UrlsWithSearchedKeywords()
        {
            foreach (CrawledURL url in this.CheckedUrlsDictionary.Values)
            {
                if (url.FoundKeywords.Count > 0)
                {
                    this.OutputUrlList.Add(url.Url);
                }
            }
            return this.OutputUrlList;
        }

        //Test with htmlParser -->htmlAgility library
        private string hardCodeUrl = "https://www.24sata.hr/";

        private List<HtmlNode> nodes = new List<HtmlNode>();

        private List<string> HtmlCrawl()
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            HtmlDocument document = new HtmlDocument();
            document = htmlWeb.Load(hardCodeUrl);

            //HtmlNodeCollection body = document.DocumentNode.SelectNodes("//a[@href]");
            foreach (HtmlNode link in document.DocumentNode.SelectNodes("//a[@href]"))
            {
                string stringLink = link.GetAttributeValue("href", string.Empty);
                this.ScrapedQueue.Add(stringLink);
            }

            return this.ScrapedQueue; //just for test we add results here
        }

        //SAMPLE CODE
        //        public ISet<string> GetNewLinks(string content)
        //{
        //    Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

        //    ISet<string> newLinks = new HashSet<string>();
        //    foreach (var match in regexLink.Matches(content))
        //    {
        //        if (!newLinks.Contains(match.ToString()))
        //            newLinks.Add(match.ToString());
        //    }

        //    return newLinks;
        //}
    }
}