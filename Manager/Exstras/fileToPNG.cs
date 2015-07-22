using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Manager.Data_Structure;

namespace Manager.Exstras
{
    public partial class fileToPNG : Form
    {
        public fileToPNG()
        {
            InitializeComponent();
            runner(@"");//itamar add path files no binary
        }
        public void toChart(List<RowdataAll> listRowData)
        {
            string path = @"C//:";//add the path


            this.chart1.Titles.Add("Itamar feg");

            Series Ask1 = this.chart1.Series.Add("Ask1");
            Ask1.ChartType = SeriesChartType.Line;
            Series Bid1 = this.chart1.Series.Add("Bid1");
            Bid1.ChartType = SeriesChartType.Line;

            Series Ask2 = this.chart1.Series.Add("Ask2");
            Ask1.ChartType = SeriesChartType.Line;
            Series Bid2 = this.chart1.Series.Add("Bid2");
            Bid2.ChartType = SeriesChartType.Line;

            Series Ask3 = this.chart1.Series.Add("Ask3");
            Ask3.ChartType = SeriesChartType.Line;
            Series Bid3 = this.chart1.Series.Add("Bid3");
            Bid3.ChartType = SeriesChartType.Line;

            foreach (RowdataAll r in listRowData)
            {
                Ask1.Points.AddXY(r.datetime, r.ask1);
                Bid1.Points.AddXY(r.datetime, r.bid1);

                Ask2.Points.AddXY(r.datetime, r.ask2);
                Bid2.Points.AddXY(r.datetime, r.bid2);

                Ask3.Points.AddXY(r.datetime, r.ask3);
                Bid3.Points.AddXY(r.datetime, r.bid3);

            }
            this.chart1.SaveImage(path, ChartImageFormat.Png);

        }
        private void chart1_Click(object sender, EventArgs e)
        {

        }
        public void runner(string path)
        {
            String[] names = Directory.GetFiles(path);

            foreach (var i in names)
            {
                if (-1 != i.IndexOf(".csv"))
                {
                    List<RowdataAll> sendlist = datacollector(i);
                    toChart(sendlist);
                }
            }
        }
        public List<RowdataAll> datacollector(string path)
        {
            StreamReader reader = new StreamReader(path);
            Dictionary<String, int> headersIDs = new Dictionary<string, int>();
            String line = reader.ReadLine();
            String[] headers = line.Split(',');

            for (int i = 0; i < headers.Length; i++)
            {
                headersIDs.Add(headers[i], i);
            }
            List<RowdataAll> rowdatalist = new List<RowdataAll>();

            RowdataAll r = new RowdataAll();
            int counter = 0;
            bool start = true;
            double ask1 = 0;
            double ask2 = 0;
            double ask3 = 0;
            double bid1 = 0;
            double bid2 = 0;
            double bid3 = 0;
            DateTime time = new DateTime();
            while ((line = reader.ReadLine()) != null)
            {

                String[] fields = line.Split(',');
                r.datetime = DateTime.Parse(fields[headersIDs["DateTime"]].ToString());
                r.ask1 = double.Parse(fields[headersIDs["ask1"]].ToString());
                r.bid1 = double.Parse(fields[headersIDs["bid1"]].ToString());
                r.ask2 = double.Parse(fields[headersIDs["ask2"]].ToString());
                r.bid2 = double.Parse(fields[headersIDs["bid2"]].ToString());
                r.ask3 = double.Parse(fields[headersIDs["ask3"]].ToString());
                r.bid3 = double.Parse(fields[headersIDs["bid3"]].ToString());
                r.isTrade = int.Parse(fields[headersIDs["isTrade"]].ToString());
                r.askV = int.Parse(fields[headersIDs["ask1volume"]].ToString());
                r.bidV = int.Parse(fields[headersIDs["bid1Volume"]].ToString());
                r.dir = 0;
                if (start)
                {
                    start = false;
                    time = new DateTime(r.datetime.Year,r.datetime.Month,r.datetime.Day,r.datetime.Hour,r.datetime.Minute,r.datetime.Second);

                    counter++;
                    ask1 += r.ask1;
                    ask2 += r.ask2;
                    ask3 += r.ask3;
                    bid1 += r.bid1;
                    bid2 += r.bid2;
                    bid3 += r.bid3;

                }

                if (time.Minute - r.datetime.Minute < 30)
                {
                    counter++;
                    ask1 += r.ask1;
                    ask2 += r.ask2;
                    ask3 += r.ask3;
                    bid1 += r.bid1;
                    bid2 += r.bid2;
                    bid3 += r.bid3;
                }
                else
                {
                    RowdataAll finalr = new RowdataAll();

                    finalr.ask1 = ask1 / counter;
                    finalr.ask2 = ask2 / counter;
                    finalr.ask3 = ask3 / counter;
                    finalr.bid1 = bid1 / counter;
                    finalr.bid2 = bid2 / counter;
                    finalr.bid3 = bid3 / counter;
                    finalr.datetime = time;
                    counter = 0;
                    ask1 = r.ask1;
                    ask2 = r.ask2;
                    ask3 = r.ask3;
                    bid1 = r.bid1;
                    bid2 = r.bid2;
                    bid3 = r.bid3;

                    time = new DateTime(r.datetime.Year, r.datetime.Month, r.datetime.Day, r.datetime.Hour, r.datetime.Minute, r.datetime.Second);
                    rowdatalist.Add(finalr);
                }

            }
            return rowdatalist;

        }

    }

}




