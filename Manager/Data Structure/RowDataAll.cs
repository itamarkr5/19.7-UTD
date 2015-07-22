using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Data_Structure
{
    public struct RowdataAll
    {
        public DateTime datetime;
        public double ask1;
        public double bid1;
        public int askV;
        public int bidV;
        public int dir;
        public int isTrade;
        public double ask2;
        public double bid2;
        public double ask3;
        public double bid3;

        public RowdataAll(RowdataAll r)
        {
            this.datetime = r.datetime;
            this.ask1 = r.ask1;
            this.bid1 = r.bid1;
            this.ask2 = r.ask2;
            this.bid2 = r.bid2;
            this.ask3 = r.ask3;
            this.bid3 = r.bid3;
            this.askV = r.askV;
            this.bidV = r.bidV;
            this.dir = r.dir;
            this.isTrade = r.isTrade;
        }
    }
}
