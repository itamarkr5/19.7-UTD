using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Strategies
{
    class MD: Strategy
    {
        private bool ok;
        public static int autoNum = 0;
        private int stratNum;
        private int midarr;
        private bool inbuypos;
        private double lastavgarr;
        private int pointerForArr;
        private bool buybool;
        private bool sellbool;
        private  int counterLast = 0; 
        private int firstPoint;
        private int lastPoint;
        private double[] sell1;
        private double[] buy1;
        private double dT;
        
        private int counterRowData;
        private Boolean inPosition;
        public MD( int ndt,/*int foult,*/ double timeToSave,double sec) : base()
        {
            ok = true;
            MD.autoNum++;
            this.stratNum = autoNum;
            this.name = @"millerHomo " + stratNum.ToString(); ;
            this.paramArr[2] = sec;
            this.buybool = false;
            this.sellbool = false;
            this.paramArr[1] = timeToSave;
            this.firstPoint = 0;
            this.lastPoint = 0;
            this.inPosition = false;
            this.counterRowData = 0;
            this.dT = this.paramArr[1] / ndt;
            this.paramArr[3] = 0 ;
            this.paramArr[0] = ndt;
            this.sell1 = new double[int.Parse(""+this.paramArr[0])];
            this.buy1 = new double[int.Parse(""+this.paramArr[0])];
            for (int i = 0; i < this.paramArr[0] - 1;i++ )
            {
                sell1[i] = 0;
                buy1[i] = 0 ;
            }
            if (this.paramArr[0] % 2 == 0)
            {
                midarr = (int.Parse("" + this.paramArr[0]) / 2) - 1;
            }
            else 
            {
                midarr = (int.Parse("" + this.paramArr[0]) / 2);
            }
        }// change to para[]
        public void avgOrg() 
        {
            counterRowData = 0;
            int p = 0;
            int j = lastPoint;
            for (int i = lastPoint; j <= firstPoint; j++) 
            {
                if (ProgramMain.deltaMin(ProgramMain.lr[i].datetime, ProgramMain.lr[j].datetime) < dT)
                {
                    counterRowData++;
                    sell1[p] += ProgramMain.lr[lastPoint].bid1;
                    buy1[p] += ProgramMain.lr[lastPoint].ask1;
                }
                else 
                {
                    sell1[p] = sell1[p]/counterRowData;
                    buy1[p] = buy1[p] / counterRowData;
                    p++;
                    counterRowData = 0;
                    j--;
                    i = j;
                }
            }
            sell1[p] = sell1[p] / counterRowData;
            buy1[p] = buy1[p] / counterRowData;

        }// sets the buy/sell arr
        public override void getInfo(RowData r)
        {
            setPointer();
            if (ProgramMain.deltaMin(ProgramMain.lr[lastPoint].datetime, ProgramMain.lr[firstPoint].datetime) >= this.paramArr[1] - (this.paramArr[1])/10)
            {
                if (!buybool && !sellbool)
                {
                    avgOrg();
                    if (!inPosition)
                    {
                        algo();
                    }
                    else
                    {
                        algoExit();
                    }
                }
                else 
                {
                    if (ProgramMain.deltaMin(r.datetime, ProgramMain.lr[pointerForArr].datetime) < this.paramArr[2])
                    {
                        ifAction(r);
                    }
                    else 
                    {
                        sellbool = false;
                        buybool = false;
                    }
                }
                //firstPoint++;
            }
        }
        public void ifAction(RowData r) 
        {
            if (buybool)
            {
                if (r.ask1 <= lastavgarr)
                {
                    buy(r, 1);
                    inPosition = true;
                    sellbool = false;
                    buybool = false;
                    inbuypos = true;
                }
            }
            if (sellbool)
            {
                if (r.bid1 >= lastavgarr)
                {
                    sell(r, 1);
                    inPosition = true;
                    sellbool = false;
                    buybool = false;
                    inbuypos = false;
                }
            }
        }
        public void setPointer()
        {
            if (counterLast == 0)
            {
                lastPoint = 0;
                counterLast++;
            }
            else
            {
                if (ok)//איפוס בכל סוף קובץ
                {

                    int x = ProgramMain.deltaMin(ProgramMain.lr[firstPoint].datetime, ProgramMain.lr[lastPoint].datetime);
                    if (x == this.paramArr[1])
                    {
                        ok = false;
                    }
                    else
                    {
                        firstPoint++;
                    }
                }
                else
                {
                    firstPoint++;
                    for (int i = 0; firstPoint > lastPoint; lastPoint++)
                    {

                        int x = ProgramMain.deltaMin(ProgramMain.lr[firstPoint].datetime, ProgramMain.lr[lastPoint].datetime);
                        if (x < this.paramArr[1])
                        {
                            break;
                        }
                       
                        
                    }
                }

                /*lastPoint++;
                int x =0;
                for (int i = firstPoint; i < lastPoint; i++)
                {
                        x = ProgramMain.deltaMin(ProgramMain.lr[i].datetime, ProgramMain.lr[lastPoint].datetime);
                        if (x > this.paramArr[1])
                        {
                            firstPoint++;
                        }
                    }
                */
            }
            
        }
        public void algo() 
        {
            bool s = true;
            bool b = true;
            for (int i = 1; i < buy1.Length - 1; i++)
            {
                if (buy1[i - 1] <  buy1[i])
                {
                    b = false;
                    break;
                }
                else
                {
                    b = true; 
                }
                if (sell1[i - 1] > sell1[i])
                {
                    s = false;
                    break;
                }
                else 
                {
                    s = true;
                }
            }
            if (b)
            {
                buybool = true;
                lastavgarr = buy1[buy1.Length - 1];
                pointerForArr = lastPoint;
            }
            if (s)
            {
                sellbool = true;
                lastavgarr = sell1[sell1.Length - 1];
                pointerForArr = lastPoint;
            }
            
        }
        public void reset() 
        {
            this.firstPoint = 0;
            this.lastPoint = 0;
            ok = true;
            this.buybool = false;
            this.sellbool = false;
            this.inPosition = false;
            this.counterRowData = 0;

        }
        public void algoExit() 
        {
            bool s = true;
            bool b = true;
            if (inbuypos)
            {//in buy pos
                for (int i = midarr; i < sell1.Length - 1; i++)
                {
                    if (sell1[i - 1] > sell1[i])
                    {
                        s = false;
                        break;
                    }
                    else
                    {
                        s = true;
                    }
                    if (s)
                    {
                        sellbool = true;
                        lastavgarr = sell1[sell1.Length - 1];
                        pointerForArr = lastPoint;
                    }
                }
            }
            else
            {//in sell pos
                for (int i = midarr; i < buy1.Length - 1; i++)
                {
                    if (buy1[i - 1] < buy1[i])
                    {
                        b = false;
                        break;

                    }
                    else
                    {
                        b = true;
                    }
                    if (b)
                    {
                        buybool = true;
                        lastavgarr = buy1[buy1.Length - 1];
                        pointerForArr = lastPoint;
                    }
                }
            }
        }
    }
}
