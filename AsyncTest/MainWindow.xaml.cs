using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AsyncTest
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void bStart_Click(object sender, RoutedEventArgs e)
        {
            //setUpTable();
            setUpTableV2();
            //trySync();
            trySyncV2();
        }

        private void addDataToTable(object sender, EventStringArgs e)
        {
            WebSiteDataModel reportModel = e.model;
            WpfDataGrid1.Items.Add(reportModel);

        }

        /*/// <summary>
        /// set up DataGrid, clear items and collumn
        /// </summary>
        private void setUpTable()
        {
            WpfDataGrid1.Items.Clear();
            WpfDataGrid1.Columns.Clear();
            double gridWidth = Math.Round(WpfDataGrid1.ActualWidth, 2);

            //binding map item from specific class (you can add items)
            List<DataGridTextColumn> gridColumns = new List<DataGridTextColumn>()
            {
                new DataGridTextColumn() { Header = "ID"   , Binding = new Binding("ID")   , Width = gridWidth/16*2 },
                new DataGridTextColumn() { Header = "Date" , Binding = new Binding("Date") , Width = gridWidth/16*4 },
                new DataGridTextColumn() { Header = "Name" , Binding = new Binding("Name") , Width = gridWidth/16*8 },
                new DataGridTextColumn() { Header = "Age"  , Binding = new Binding("Age")  , Width = gridWidth/16*2 }
            };

            foreach (DataGridTextColumn gridCollumn in gridColumns)
            {
                WpfDataGrid1.Columns.Add(gridCollumn);
            }
            

        }*/

        /// <summary>
        /// set up collums in WpfDataGrid1
        /// </summary>
        private void setUpTableV2()
        {
            WpfDataGrid1.Items.Clear();
            WpfDataGrid1.Columns.Clear();
            double gridWidth = Math.Round(WpfDataGrid1.ActualWidth, 2);

            //binding map item from specific class (you can add items)
            List<DataGridTextColumn> gridColumns = new List<DataGridTextColumn>()
            {
                new DataGridTextColumn() { Header = "timeBeg"   , Binding = new Binding("timeBeg")   , Width = gridWidth/16*2.5 },
                new DataGridTextColumn() { Header = "timeEnd" , Binding = new Binding("timeEnd") , Width = gridWidth/16*2.5 },
                new DataGridTextColumn() { Header = "WebsiteUrl" , Binding = new Binding("WebsiteUrl") , Width = gridWidth/16*6 },
                new DataGridTextColumn() { Header = "characterCount"  , Binding = new Binding("characterCount")  , Width = gridWidth/16*1 }
            };

            foreach (DataGridTextColumn gridCollumn in gridColumns)
            {
                WpfDataGrid1.Columns.Add(gridCollumn);
            }


        }

        /// <summary>
        /// download text form pages and count it
        /// </summary>
        private void trySync()
        {
            //counting time
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            websiteDownloader myDownloader = new websiteDownloader();
            myDownloader.downloadReport += addDataToTable;
            myDownloader.downloadSync();
            //myDownloader.downloadSync();

            watch.Stop();

            WpfDataGrid1.Items.Add(new WebSiteDataModel() { timeBeg= watch.ElapsedMilliseconds.ToString() });
        }

        /// <summary>
        /// download text form pages and count it
        /// </summary>
        private async void trySyncV2()
        {
            //this will call method when progress change
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;

            //counting time
            System.Diagnostics.Stopwatch watch = System.Diagnostics.Stopwatch.StartNew();

            websiteDownloader myDownloader = new websiteDownloader();
            myDownloader.progress = progress;
            var waitToComplete = await myDownloader.downloadAsync();

            //myDownloader.downloadReport += addDataToTable;
            //myDownloader.downloadSync();

            watch.Stop();

            WpfDataGrid1.Items.Add(new WebSiteDataModel() { timeBeg = watch.ElapsedMilliseconds.ToString() });
        }

        /// <summary>
        /// show progress in progressBar
        /// </summary>
        private void ReportProgress(object sender, ProgressReportModel e)
        {
            progressBarMain.Value = e.PercentageComplete;
            PrintResults(e.SitesDownloaded);

            //show sites
            //var sites = e.SitesDownloaded;
            //if (e.SitesDownloaded.Count > 0)
            //{
                //WpfDataGrid1.Items.Add(sites[sites.Count - 1]);
            //}
            
        }

        private void PrintResults(List<WebSiteDataModel> sites)
        {
            WpfDataGrid1.Items.Clear();
            foreach (WebSiteDataModel page in sites)
            {
                WpfDataGrid1.Items.Add(page);
            }
        }
    }
}
