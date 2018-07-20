using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace parralelTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<ProcessItem> processList = testData();
            linq();
            /*Parallel.ForEach<ProcessItem>(processList, (item) =>
            item.myLittleEvent += writeToTextBox ,item.doThing()
            );*/
        }

        private int getRandomNumber()
        {
            Random random = new Random();
            return random.Next(500, 1300);
        }

        private List<ProcessItem> testData()
        {
            List<ProcessItem> someTestData = new List<ProcessItem>()
            {
            new ProcessItem() { text = "nahodna funkce 1", waitTime = getRandomNumber() },
            new ProcessItem() { text = "nahodna funkce 2", waitTime = getRandomNumber() },
            new ProcessItem() { text = "nahodna funkce 3", waitTime = getRandomNumber() },
            new ProcessItem() { text = "nahodna funkce 4", waitTime = getRandomNumber() }
            };

            return someTestData;
        }

        private void writeToTextBox(object sender, EventArgs e)
        {
            richTextBox1.Text += ((EventForProcess)e).text.ToString() + Environment.NewLine;
        }

        public class ProcessItem : IComparable
        {
            public EventHandler myLittleEvent;
            public int waitTime { get; set; } = 0;
            public string text { get; set; } = "";
            public void doThing()
            {
                Thread.Sleep(waitTime);
                callEvent();
            }

            private void callEvent()
            {
                if (myLittleEvent != null)
                {
                    EventForProcess myCustomEventForProcess = new EventForProcess() { text = this.ToString() };
                    myLittleEvent(null, myCustomEventForProcess);
                }
            }

            public override string ToString()
            {
                return (text + " - " + waitTime.ToString());
            }

            public int CompareTo(object obj)
            {
                if (obj.ToString().Equals(ToString()))
                {
                    return 1;
                } else
                {
                    return 0;
                }
            }
        }

        public class EventForProcess : EventArgs
        {
            public string text { get; set; } = string.Empty;
        }

        private void linq()
        {
            /**/List<ProcessItem> testDataList = testData();
            var selectedData  = from data in testDataList
                         where data.ToString().Contains("1")
                         orderby data
                         select data;
            var listSelectedData = selectedData.ToList<ProcessItem>();
            /**/

            /*int[] testDataList = new int[] { 5, 78, 10, 25, 51, 69, 30, 54 };

            var selectedData = from data in testDataList
                               where (data > 10) & (data < 70)
                               orderby data descending
                               select data;*/

            foreach (var item in listSelectedData)
            {
                EventForProcess infoEvent = new EventForProcess() { text = item.ToString() };
                writeToTextBox(null, infoEvent);
            }
        }
    }
}
