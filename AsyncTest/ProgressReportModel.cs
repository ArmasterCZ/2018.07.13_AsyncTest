using System.Collections.Generic;

namespace AsyncTest
{
    public class ProgressReportModel
    {
        public int PercentageComplete { get; set; } = 0;

        public List<WebSiteDataModel> SitesDownloaded { get; set; } = new List<WebSiteDataModel>();
    }
}