using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class FirstDayCheck : Strategy
    {
        private RowData rAtEnter;

        public FirstDayCheck(int n)
            : base()
        {
            this.name = "First Day Monkey No " + n.ToString();
            this.paramArr[0] = n * 0.2;
            this.paramArr[1] = n * 0.05;

        }
        /*
        public override void getInfo(RowData r)
        {
            Random random = new Random();
            int x = random.Next(Convert.ToInt32(paramArr[0]), 30);
            if (x == 21 || x == 14 || x == 9)
            {
                if (this.getPVol() == 1)
                { this.sell(r, 1); }
                else
                { this.buy(r, 1); }
            }
        }
         */ // random method

        public override void getInfo(RowData r)
        {
            
                if (this.getList().Count() != 0)
                {
                    if (this.getList()[this.getList().Count() - 1].getPosEnd())
                    {
                        if (r.ask1 < prevRowData.ask1)
                        {
                            buy(r, 1);
                            this.rAtEnter = this.getList()[this.getList().Count() - 1].getFirst();
                        }

                        if (r.bid1 > prevRowData.bid1)
                        {
                            sell(r, 1);
                            this.rAtEnter = this.getList()[this.getList().Count() - 1].getFirst();
                        }
                    }

                    else
                    {
                        if (this.rAtEnter.dir == 1)
                        {
                            if (r.bid1 / this.rAtEnter.bid1 > this.paramArr[1])
                            {
                                sell(r, 1);
                            }
                            if (this.rAtEnter.bid1 / r.bid1 > this.paramArr[0])
                            {
                                sell(r, 1);
                            }
                        }
                        if (this.rAtEnter.dir == -1)
                        {
                            if (this.rAtEnter.ask1 / r.ask1 > this.paramArr[1])
                            {
                                buy(r, 1);
                            }
                            if (r.ask1 / this.rAtEnter.ask1 > this.paramArr[0])
                            {
                                buy(r, 1);
                            }
                        }

                    }
                }

                if (this.getList().Count == 0)
                {
                    if (r.ask1 < prevRowData.ask1)
                    {
                        buy(r, 1);
                        this.rAtEnter = this.getList()[this.getList().Count() - 1].getFirst();
                    }

                    if (r.bid1 > prevRowData.bid1)
                    {
                        sell(r, 1);
                        this.rAtEnter = this.getList()[this.getList().Count() - 1].getFirst();
                    }
                }
            
        }


    }
}

