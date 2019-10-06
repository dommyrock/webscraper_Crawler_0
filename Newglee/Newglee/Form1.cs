using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Newglee
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //hardcoded site input list for easyer testing /can be changed
            textBox1.Text = "https://www.abrakadabra.com"; /*"https://www.24sata.hr" + Environment.NewLine + "https://www.index.hr" + Environment.NewLine + "https://www.njuskalo.hr" + Environment.NewLine + "https://www.jutarnji.hr" + Environment.NewLine + "http://twitter.com" + Environment.NewLine;*/
            this.listBox1 = null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync(); //run in new thread
            backgroundWorker1.WorkerSupportsCancellation = true; //cancell--> true
        }

        private void button2_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //NEW
            RunSearch runSearch = new RunSearch(textBox1.Text, textBox2.Text);
            runSearch.CrawlUrl();
            /////////////////////////////////////////////////////////////

            //if (backgroundWorker1.CancellationPending) remake this
            //{
            //    isActive = false;
            //}
        }
    }

    //TODO  FEtch UrlsWithSearchedKeywords method to output into listbox
    //private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
    //{
    //    //foreach (CrawledURL url in checkedUrlsDictionary.Values)
    //    //{
    //    //    if (url.UrlContainsKeyword(keyword) == true)
    //    //    {
    //    //        //listBox1.Items.Add(url.Url);
    //    //    }
    //    //}
    //}
}