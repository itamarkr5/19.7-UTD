using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class finalAllStratLog
    {
        public DateTime start;
        public DateTime end;
        public String name; // config name
        public String pLog; // path to config log
        public int numOfPos; // num of positions in total
        public double avg; // avg pos time
        public double totalSum; // total sum of config
        public double avgSum; // avg price
        public double percentProfit; // % of profitable pos
        public double[] paramArr; // pramaters array
        public double Score = 0;

        public finalAllStratLog(int n)
        {
            this.paramArr = new double[n];
        } // constractor
        public finalAllStratLog(finalAllStratLog l)
        {
            this.start = l.start;
            this.end = l.end;
            this.name = l.name;
            this.pLog = l.pLog;
            this.numOfPos = l.numOfPos;
            this.avg = l.avg;
            this.totalSum = l.totalSum;
            this.avgSum = l.avgSum;
            this.percentProfit = l.percentProfit;
            this.paramArr = new double[l.paramArr.Length]; 
            for(int i = 0; i<l.paramArr.Length; i++)
            {
                this.paramArr[i] = l.paramArr[i];
            }
            this.Score = l.Score;
        } // copy constractor
        public String printString()
        {
            String s = "";
            s += this.name;
            for (int i = 0; i < this.paramArr.Length; i++)
            {
                if (this.paramArr[i] != -999)
                    s += "," + this.paramArr[i].ToString();
                else
                    s += "," + "";
            }
            s += "," + this.start.ToString();
            s += "," + this.end.ToString();
            s += "," + this.pLog;
            s += "," + this.numOfPos.ToString();
            s += "," + this.avg.ToString();
            s += "," + this.totalSum.ToString();
            s += "," + this.avgSum.ToString();
            s += "," + this.percentProfit.ToString();
            s += "," + this.Score.ToString();

            return s;
        } // creates string (like tostring)
        

    }
}
