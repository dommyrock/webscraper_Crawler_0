using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string input = "https://twitter.com/" + Environment.NewLine + "https://www.youtube.com/" + Environment.NewLine + "https://www.njuskalo.hr/" + Environment.NewLine + "https://stackoverflow.com/questions/303248/what-is-the-proper-way-to-load-up-a-listbox" + Environment.NewLine + "http://instagram.com";

            string urls = input.Split(new char[] { ' ', ',', '\n' }).ToString();

            Console.ReadKey();
        }

        #region Old form1 code before remaster

        //        using System;
        //using System.Collections.Generic;
        //using System.ComponentModel;
        //using System.Data;
        //using System.Drawing;
        //using System.IO;
        //using System.Linq;
        //using System.Net;
        //using System.Text;
        //using System.Text.RegularExpressions;
        //using System.Threading.Tasks;
        //using System.Windows.Forms;

        //namespace Newglee
        //    {
        //        public partial class Form1 : Form
        //        {
        //            public Form1()
        //            {
        //                InitializeComponent();
        //                //hardcoded site input list for easyer testing /can be changed
        //                //textBox1.Text = "https://twitter.com" + Environment.NewLine + "https://www.index.hr" + Environment.NewLine + "https://www.njuskalo.hr" + Environment.NewLine + "https://www.jutarnji.hr" + Environment.NewLine + "http://instagram.com" + Environment.NewLine;
        //            }

        //            private void button1_Click(object sender, EventArgs e)
        //            {
        //                backgroundWorker1.RunWorkerAsync(); //run in new thread
        //                backgroundWorker1.WorkerSupportsCancellation = true; //cancell--> true
        //            }

        //            private void button2_Click(object sender, EventArgs e)
        //            {
        //                backgroundWorker1.CancelAsync();
        //            }

        //            #region Object instanciation example

        //            //1. nacin inicijalizacije konstruktora sa parametrima

        //            //CrawledURL url1 = new CrawledURL("Facebook",false);

        //            //2. nacin inicijalizacije konstruktora

        //            //CrawledURL url = new CrawledURL
        //            //{
        //            //    Url = "facebook",
        //            //    IsChecked = false
        //            //};

        //            //3. nacin inicijalizacije praznog konstruktora

        //            //CrawledURL url1 = new CrawledURL();
        //            //url1.Url = "Twitter";
        //            //url1.IsChecked = false;

        //            #endregion Object instanciation example

        //            private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        //            {
        //                //NEW
        //                RunSearch runSearch = new RunSearch(textBox1.Text);
        //                runSearch.CrawlUrl();
        //                /////////////////////////////////////////////////////////////

        //                inputUrls = textBox1.Text.Split(new char[] { ' ', ',', '\n' }).ToList();

        //                ServicePointManager.Expect100Continue = true;//dodano zbog TSL/SSL exceptiona
        //                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

        //                while (isActive)
        //                {
        //                    //Download

        //                    CrawledURL currentUrl = new CrawledURL(inputUrls[currentUrlIndex], false);
        //                    //Uri siteUrl = new Uri(listOfProcessedUrls[currentUrlIndex]);//test.. nie nuzno potrebno

        //                    List<string> keyword = textBox2.Text.ToLower().Split(new char[] { ' ', ',' }).ToList();

        //                    if (!checkedUrlsDictionary.ContainsKey(currentUrl.Url)) // uvijet po kojem skidam samo vec ne skinute urlove
        //                    {
        //                        try
        //                        {
        //                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(inputUrls[currentUrlIndex]);
        //                            request.ContentType = "txt/html";
        //                            request.Accept = "txt/html";//specifies the types of data you want to receive
        //                            request.AllowAutoRedirect = false;//because of sites with >50 AutoRedirect's
        //                            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse()) //absolute uri ...nejbitniji
        //                            {
        //                                using (Stream st = response.GetResponseStream())
        //                                using (StreamReader reader = new StreamReader(st))
        //                                {
        //                                    result = reader.ReadToEnd();
        //                                }

        //                                //result = wc.DownloadString(currentUrl.Url); -->if we use WebClient class
        //                                currentUrl.IsChecked = true;

        //                                #region UrlKeywordSearch

        //                                //If we would search for keywoards in url we use this code :

        //                                //string[] tempStrings = currentUrl.Url.Split(new char[] { '.', '/','-','_' }).ToArray();//ako sadrzi trazen keyword dodajemo , ne sve keyworde iz urla

        //                                //foreach (string url in keyword)//add Keywords to "FoundKeywords"if Url contains them
        //                                //{
        //                                //    if (tempStrings.Contains(url))
        //                                //    {
        //                                //        currentUrl.FoundKeywords.Add(url);
        //                                //    }
        //                                //}

        //                                #endregion UrlKeywordSearch

        //                                foreach (string kw in keyword)//Source Keyword Search
        //                                {
        //                                    if (Regex.IsMatch(result, kw))
        //                                    {
        //                                        currentUrl.FoundKeywords.Add(kw);
        //                                    }
        //                                }
        //                                checkedUrlsDictionary.Add(currentUrl.Url, currentUrl);
        //                                //searchedUrlsObject.Add(currentUrl);
        //                            }
        //                        }

        //                        #region WebException

        //                        catch (WebException wex)
        //                        {
        //                            HttpWebResponse responseException = (HttpWebResponse)wex.Response;
        //                            if (responseException == null)
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            if (responseException.StatusCode == HttpStatusCode.NotFound)//http 404
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode.ToString() == "999")//custom handler za linkedin(custom exception)
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.RequestTimeout)//http 408
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.Forbidden)//403
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.NotAcceptable)//406
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.InternalServerError)//500
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.ServiceUnavailable)//503
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.Gone)
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            else if (responseException.StatusCode == HttpStatusCode.BadGateway)
        //                            {
        //                                inputUrls.RemoveAt(currentUrlIndex);
        //                                continue;
        //                            }
        //                            throw wex;
        //                        }

        //                        #endregion WebException

        //                        catch (Exception ex)
        //                        {
        //                            inputUrls.RemoveAt(currentUrlIndex);//remove currently striped url form "temp"list
        //                            continue;
        //                        }
        //                    }
        //                    else
        //                    {
        //                        inputUrls.RemoveAt(currentUrlIndex);
        //                        continue;
        //                    }

        //                    #region Regex for href matching

        //                    string data = String.Join(" ", result); // has to be joined --> cant transform directly from list to string ,(returns just name of class to string)
        //                    Regex urlRegex = new Regex(@"(?<=\bhref="")[^""]*", RegexOptions.IgnoreCase);//Regex pattern za filriranje Url-ova iz source Htmla
        //                    MatchCollection matches = urlRegex.Matches(data);

        //                    foreach (Match match in urlRegex.Matches(data))
        //                    {
        //                        inputUrls.Add(match.Value); //insert bellow 1st Url ...
        //                    }

        //                    #endregion Regex for href matching

        //                    inputUrls.RemoveAt(currentUrlIndex);//remove currently striped Url form "temp"List
        //                    if (backgroundWorker1.CancellationPending)
        //                    {
        //                        isActive = false;
        //                    }
        //                }
        //            }

        //            private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //            {
        //                List<string> keyword = textBox2.Text.ToLower().Split(new char[] { ' ', ',' }).ToList();

        //                foreach (CrawledURL url in checkedUrlsDictionary.Values)
        //                {
        //                    if (url.UrlContainsKeyword(keyword) == true)
        //                    {
        //                        listBox1.Items.Add(url.Url);
        //                    }
        //                }
        //            }

        //            //File.WriteAllText(@"C:\Users\Dommy\Desktop\Test.txt", result); output to file
        //        }
        //    }

        #endregion Old form1 code before remaster
    }
}