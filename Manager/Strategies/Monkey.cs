using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class Monkey : Strategy
    {

        private RowData[] deals;
        private int counterMax;
        private int counterMin;

        public Monkey(int n)
            : base()
        {
            this.name = "Monkey - Miller No " + (n - 1).ToString();
            this.paramArr[0] = n;
            //this.paramArr[1] = n * 0.2;
            this.deals = new RowData[Convert.ToInt32(this.paramArr[0])];
            this.counterMax = 0;
            this.counterMin = 0;
        }

        public override void getInfo(RowData r)
        {
            if (r.isTrade == 1)
            {
                AddToArr(r);
                Algo(r);
            }
        }

        private void AddToArr(RowData r)
        {
            int i;
            for (i = 0; i < deals.Length; i++)
            {
                if (deals[i].ask1 == 0)
                {
                    deals[i] = r;
                    return;
                }
            }
            deals[0] = default(RowData);
            for (i = 1; i < deals.Length; i++)
            {
                deals[i - 1] = deals[i];

                deals[0] = default(RowData);

            }
            deals[i - 1] = r;


        }

        public void Algo(RowData r)
        {

            if (deals[deals.Length - 1].ask1 != 0)
            {
                double max = deals[1].ask1;
                double min = deals[1].ask1;
                for (int i = 1; i < deals.Length; i++)
                {
                    if (max == deals[i - 1].ask1)
                    {
                        if (deals[i].ask1 > max)
                        {
                            max = deals[i].ask1;
                        }
                        else
                        {
                            max = 0;
                            this.counterMax++;
                        }
                    }
                    if (deals[i - 1].ask1 == min)
                    {
                        if (deals[i].ask1 < min)
                        {
                            min = deals[i].ask1;

                        }
                        else
                        {
                            min = 0;
                            this.counterMin++;
                        }
                    }
                }
                if (max != 0 && getPVol() != 1 && this.counterMax <= this.paramArr[1])
                {
                    this.buy(r, 1);
                }
                if (min != 0 && getPVol() != -1 && this.counterMin <= this.paramArr[1])
                {
                    this.sell(r, 1);
                }

            }

        }


    }
}


