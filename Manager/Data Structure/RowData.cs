using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;

namespace Manager
{
    [Serializable]
    // class RowData
    public struct RowData
    {
        //learn how to with date time
        public DateTime datetime;
        public double ask1;
        public double bid1;
        public int askV;
        public int bidV;
        public int dir;
        public int isTrade;

        public RowData(RowData r)
        {
            this.datetime = r.datetime;
            this.ask1 = r.ask1;
            this.bid1 = r.bid1;
            this.askV = r.askV;
            this.bidV = r.bidV;
            this.dir = r.dir;
            this.isTrade = r.isTrade;
        }

        /*
        public RowData()
        {
            this.datetime =new DateTime ();
            this.ask1 = 0;
            this.bid1 = 0;
            this.askV = 0;
            this.bidV = 0;
            this.dir = 0;
            this.isTrade = 0;
        }
        */
        public  void ToChange(RowData other) {
            this.datetime = other.datetime;
            this.datetime = other.datetime;
            this.ask1 = other.ask1;
            this.bid1 = other.bid1;
            this.askV = other.askV;
            this.bidV = other.bidV;
            this.dir = other.dir;
            this.isTrade = other.isTrade;

        
        
        }
    }
}
 

  