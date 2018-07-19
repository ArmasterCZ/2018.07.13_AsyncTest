using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AsyncTest
{
    class websiteDownloader
    {
        public EventHandler<EventStringArgs> downloadReport;
        public IProgress<ProgressReportModel> progress;

        /// <summary>
        /// download all web pages
        /// </summary>
        public void downloadSync()
        {
            List<String> websites = testData();
            foreach (string site in websites)
            {
                WebSiteDataModel results = downloadWebSite(site);
                reportWebSiteInfo(results);
            }

        }

        /// <summary>
        /// download all web pages and report progress
        /// </summary>
        public async Task<List<WebSiteDataModel>> downloadAsync()
        {
            List<string> websites = testData();
            List<WebSiteDataModel> output = new List<WebSiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            foreach (string site in websites)
            {
                WebSiteDataModel results = await downloadWebSiteAsync(site);
                output.Add(results);
                report.SitesDownloaded = output;
                report.PercentageComplete = (output.Count() * 100) / websites.Count(); // (2 * 100) / 10 = 20
                progress.Report(report);
            }
            return output;
        }

        /// <summary>
        /// download all web pages paralel and report progress
        /// </summary>
        public async Task<List<WebSiteDataModel>> downloadParallelAsync(IProgress<ProgressReportModel> progress)
        {
            List<string> websites = testData();
            List<WebSiteDataModel> output = new List<WebSiteDataModel>();
            ProgressReportModel report = new ProgressReportModel();

            await Task.Run(() =>
            {
                Parallel.ForEach<string>(websites, (site) =>
                {
                    WebSiteDataModel results = downloadWebSite(site);

                    report.SitesDownloaded = output;
                    report.PercentageComplete = (output.Count() * 100) / websites.Count(); // (2 * 100) / 10 = 20
                    progress.Report(report);
                });

            });
            return output;
        }

        /// <summary>
        /// download all 
        /// </summary>
        private async Task<WebSiteDataModel> downloadWebSiteAsync(string websiteUrl)
        {
            WebSiteDataModel output = new WebSiteDataModel();
            WebClient client = new WebClient();

            output.timeBeg = System.DateTime.Now.ToString();
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = await client.DownloadStringTaskAsync(websiteUrl);
            output.characterCount = output.WebsiteData.Length;
            output.timeEnd = System.DateTime.Now.ToString();

            return output;
        }

        /// <summary>
        /// Download data from web page
        /// </summary>
        private WebSiteDataModel downloadWebSite(string websiteUrl)
        {
            WebSiteDataModel output = new WebSiteDataModel();
            WebClient client = new WebClient();

            output.timeBeg = System.DateTime.Now.ToString();
            output.WebsiteUrl = websiteUrl;
            output.WebsiteData = client.DownloadString(websiteUrl);
            output.characterCount = output.WebsiteData.Length;
            output.timeEnd = System.DateTime.Now.ToString();

            return output;
        }

        /// <summary>
        /// create test Pages URL
        /// </summary>
        private List<String> testData()
        {
            List<String> someUrlData = new List<string>();
            someUrlData.Add("https://www.youtube.com");
            //someUrlData.Add("https://github.com");
            someUrlData.Add("https://forums.faforever.com");
            //someUrlData.Add("https://www.linkedin.com/");
            someUrlData.Add("https://www.google.cz");
            someUrlData.Add("https://www.alza.cz");
            someUrlData.Add("https://www.ulozto.cz");
            someUrlData.Add("https://cs.wikipedia.org/wiki/Hlavní_strana");

            return someUrlData;
        }

        /// <summary>
        /// sen out info about download
        /// </summary>
        /// <param name="data"></param>
        private void reportWebSiteInfo(WebSiteDataModel data)
        {
            //string outputText = $"{data.WebsiteUrl} obsahuje {data.WebsiteData.Length} znaků. {Environment.NewLine}";

            if (downloadReport != null)
            {
                //EventStringArgs eventStringArgs = new EventStringArgs { text = outputText };
                EventStringArgs eventStringArgs = new EventStringArgs { model = data };
                downloadReport(null, eventStringArgs);
            }

        }
    }
}
