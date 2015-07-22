using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Strategies
{
    class StupidMiller : Strategy
    {

        public static DateTime borsStart = new DateTime(2014, 3, 18, 9, 15, 0, 0);
        public static int PlaInArrNumOfRows = 0;
        public static RowData helper = new RowData();
        public static Queue<RowData> saveData1 = new Queue<RowData>();
        public static int autoNum = 0;
        private int stratNum;
        private double[] avgbuy;
        private double[] avgsell;
        private double avgbuysum;
        private double avgsellsum;
        public static double[] avgToChange = new double[2];
        private bool isInWait;
        private bool dirForAction; // false - sell || true - buy
        private int timeInWait;
        private bool firstTime = true;
        private bool inPos;
        private bool toPass = false;
        private bool inBuy;
        private bool miller = true;
        
        public StupidMiller(int numOfRows, double Sin/*, double Sout, double fault*/, int numOfAvg, int numToWait)
            : base()
        {


            StupidMiller.autoNum++;
            this.stratNum = autoNum;
            this.name = @"Monkey - FirstIdea No " + stratNum.ToString();
            this.paramArr[PlaInArrNumOfRows] = numOfRows;
            
            this.paramArr[1] = Sin;
            this.paramArr[2] = Sin/2;
            
            this.paramArr[3] = numOfAvg/5;
            
            this.paramArr[4] = numOfAvg;
            this.paramArr[5] = numToWait;
            this.avgbuy = new double[numOfAvg];
            this.avgsell = new double[numOfAvg];
            this.isInWait = false;
            this.timeInWait = 0;
            this.inPos = false;
        }

        public override void getInfo(RowData r)
        {

            if (ProgramMain.deltaMin(ProgramMain.rowDataToSave.Peek().datetime, borsStart) >= 15)
            {
                int p;
                if (this.firstTime)
                {
                    for (int k = 0; k < paramArr[4]; k++)
                    {
                        this.avgsellsum = 0;
                        this.avgbuysum = 0;
                        for (int i = Convert.ToInt32(paramArr[PlaInArrNumOfRows]) * k; i < Convert.ToInt32(paramArr[PlaInArrNumOfRows]) * k + Convert.ToInt32(paramArr[PlaInArrNumOfRows]); i++)
                        {
                            helper.ToChange(ProgramMain.rowDataToSave.Dequeue());
                            this.avgbuysum += helper.ask1;
                            this.avgsellsum += helper.bid1;
                            saveData1.Enqueue(helper);

                        }


                        this.avgbuy[k] = this.avgbuysum / this.paramArr[PlaInArrNumOfRows];
                        this.avgsell[k] = this.avgsellsum / this.paramArr[PlaInArrNumOfRows];



                    }
                    while (ProgramMain.rowDataToSave.Count != 0)
                    {
                        saveData1.Enqueue(ProgramMain.rowDataToSave.Dequeue());
                    }
                    while (saveData1.Count != 0)
                    {
                        ProgramMain.rowDataToSave.Enqueue(saveData1.Dequeue());
                    }
                    this.firstTime = false;

                }
                else
                {
                    this.avgbuy[0] = (this.avgbuy[0] * this.paramArr[PlaInArrNumOfRows] - ProgramMain.row.ask1 + ProgramMain.toGetPointer[Convert.ToInt32(this.paramArr[PlaInArrNumOfRows]) - 1].ask1) / this.paramArr[PlaInArrNumOfRows];
                    this.avgsell[0] = (this.avgsell[0] * this.paramArr[PlaInArrNumOfRows] - ProgramMain.row.bid1 + ProgramMain.toGetPointer[Convert.ToInt32(this.paramArr[PlaInArrNumOfRows]) - 1].bid1) / this.paramArr[PlaInArrNumOfRows];
                    for (p = 1; p < this.paramArr[4] - 1; p++)
                    {
                        this.avgbuy[p] = (this.avgbuy[p] * this.paramArr[PlaInArrNumOfRows] - ProgramMain.toGetPointer[Convert.ToInt32(this.paramArr[PlaInArrNumOfRows]) * p - 1].ask1 + ProgramMain.toGetPointer[Convert.ToInt32((this.paramArr[PlaInArrNumOfRows]) * (p + 1)) - 1].ask1) / this.paramArr[PlaInArrNumOfRows];
                        this.avgsell[p] = (this.avgsell[p] * this.paramArr[PlaInArrNumOfRows] - ProgramMain.toGetPointer[Convert.ToInt32(this.paramArr[PlaInArrNumOfRows]) * p - 1].bid1 + ProgramMain.toGetPointer[Convert.ToInt32((this.paramArr[PlaInArrNumOfRows]) * (p + 1)) - 1].bid1) / this.paramArr[PlaInArrNumOfRows];

                    }

                }

                //--------------------
                if (!this.inPos)
                {
                    if(miller)
                    {
                        this.isInWait = this.Algo(r,  this.paramArr[1]);
                    }
                    if (isInWait)
                    {
                        miller = false;
                        if (this.timeInWait <= this.paramArr[5])
                        {
                            if (dirForAction)
                            {
                                if (r.ask1 <= this.avgbuy[avgbuy.Length - 1])
                                {
                                    buy(r, 1);
                                    this.isInWait = false;
                                    this.timeInWait = 0;
                                    this.inPos = true;
                                    this.toPass = true;
                                    this.inBuy = true;
                                    this.miller = true;
                                }
                            }

                            if (!dirForAction)
                            {
                                if (r.bid1 >= this.avgsell[avgsell.Length - 1])
                                {
                                    sell(r, 1);
                                    this.isInWait = false;
                                    this.timeInWait = 0;
                                    this.inPos = true;
                                    this.toPass = true;
                                    this.inBuy = false;
                                    this.miller = true;
                                }
                            }
                            if (!this.toPass)
                            {
                                this.toPass = false;
                                this.timeInWait++;
                            }
                        }
                        else
                        {
                            this.isInWait = false;
                            this.miller = true;
                            this.timeInWait = 0;

                        }
                    }


                    //------------------------------------------------------------------
                }
                else
                {
                    if (miller)
                    {
                        this.isInWait = this.Algo2(r, this.paramArr[2], this.getList()[this.getList().Count - 1].getDir());
                    }                 
                    if (isInWait)
                    {
                        this.miller = false;
                        if (this.timeInWait <= this.paramArr[5])
                        {
                            if (dirForAction)
                            {
                                int dir = this.getList()[this.getList().Count - 1].getDir();
                                if (dir == -1)
                                {
                                    if (r.ask1 <= this.avgbuy[avgbuy.Length - 1])
                                    {
                                        buy(r, 1);
                                        this.isInWait = false;
                                        this.timeInWait = 0;
                                        this.inPos = false;
                                        this.toPass = true;
                                        this.inBuy = true;
                                        this.miller = true;
                                    }
                                }
                            }

                            if (!dirForAction)
                            {
                                if (this.getList()[this.getList().Count - 1].getDir() == 1)
                                {
                                    if (r.bid1 >= this.avgsell[avgsell.Length - 1])
                                    {
                                        sell(r, 1);
                                        this.isInWait = false;
                                        this.timeInWait = 0;
                                        this.inPos = false;
                                        this.toPass = true;
                                        this.inBuy = false;
                                        this.miller = true;
                                    }
                                }
                            }
                          
                            
                            if (!this.toPass)
                            {
                                this.toPass = false;
                                this.timeInWait++;
                            }
                        }
                        else
                        {
                            this.miller = true;
                            this.isInWait = false;
                            this.timeInWait = 0;
                        }

                    }
                }
            }
        }
                   /* this.isInWait = this.Algo(r);
                        if (!this.isInWait)
                        {
                            if (inBuy)
                            {
                                //if ((this.avgbuy[this.avgbuy.Length - 1] < this.avgbuy[this.avgbuy.Length - 2]) && (this.avgbuy[this.avgbuy.Length - 1] < this.avgbuy[this.avgbuy.Length - 3]))
                            {
                                this.isInWait = true;
                                this.dirForAction = false;

                            }




                        }
                        else
                        {
                            //if ((this.avgbuy[this.avgbuy.Length - 1] > this.avgbuy[this.avgbuy.Length - 2]) && (this.avgbuy[this.avgbuy.Length - 1] > this.avgbuy[this.avgbuy.Length - 3]))
                            {
                                this.isInWait = true;
                                this.dirForAction = true;



                            }




                        }
                    }
                    else
                    {
                        if (this.timeInWait <= this.paramArr[5])
                        {
                            if (dirForAction)
                            {
                                if (r.ask1 <= this.avgbuy[avgbuy.Length - 1])
                                {
                                    buy(r, 1);
                                    this.isInWait = false;
                                    this.timeInWait = 0;
                                    this.inPos = false;
                                    this.toPass = true;



                                }
                            }

                            if (!dirForAction)
                            {
                                if (r.bid1 >= this.avgsell[avgsell.Length - 1])
                                {
                                    sell(r, 1);
                                    this.isInWait = false;
                                    this.timeInWait = 0;
                                    this.inPos = false;
                                    this.toPass = true;

                                }
                            }
                            if (!this.toPass)
                            {
                                this.toPass = false;
                                this.timeInWait++;
                            }
                        }
                        else
                        {
                            this.isInWait = false;

                            this.timeInWait = 0;

                        }


                    }
                }


            }*/

        public bool Algo(RowData r, double pram)
        {
            bool ok = true;
            int counter = 0;
            int i = 1;
            while (counter <= this.paramArr[3])
            {
                while (avgbuy[i - 1] < avgbuy[i] && avgbuy[i - 1] / avgbuy[i] >= pram)
                {
                    if (i == (avgbuy.Length - 1))
                    {

                        this.dirForAction = true;
                        return true;
                    }
                    i++;
                    ok = false;
                }
                if (ok)
                {
                    i++;
                    ok = true;
                }
                counter++;
            }

            if (counter <= this.paramArr[3] && i < avgbuy.Length)
            {
                this.dirForAction = true;
                return true;
            }



            i = 1;
            ok = true;
            counter = 0;
            while (counter <= this.paramArr[3])
            {
                while (avgsell[i - 1] < avgsell[i] && avgsell[i - 1] / avgsell[i] >= pram)
                {
                    if (i == (avgsell.Length - 1))
                    {

                        this.dirForAction = false;
                        return true;

                    }
                    i++;
                    ok = false;
                }
                if (ok)
                {
                    i++;
                    ok = true;
                }
                counter++;
            }
            if (counter <= this.paramArr[3] && i < avgsell.Length)
            {
                this.dirForAction = true;
                return true;
            }


            return false;


        }
        public bool Algo2(RowData r, double pram, int dir)
        {
            bool ok = true;
            int counter = 0;
            int i = 1;
            if (dir == -1)
            {
                
                while (counter <= this.paramArr[3])
                {
                    while (avgbuy[i - 1] < avgbuy[i] && avgbuy[i - 1] / avgbuy[i] >= pram)
                    {
                        if (i == (avgbuy.Length - 1))
                        {

                            this.dirForAction = true;
                            return true;
                        }
                        i++;
                        ok = false;
                    }
                    if (ok)
                    {
                        i++;
                        ok = true;
                    }
                    counter++;
                }

                if (counter <= this.paramArr[3] && i < avgbuy.Length)
                {
                    this.dirForAction = true;
                    return true;
                }

            }
            else
            {

                i = 1;
                ok = true;
                counter = 0;
                while (counter <= this.paramArr[3])
                {
                    while (avgsell[i - 1] < avgsell[i] && avgsell[i - 1] / avgsell[i] >= pram)
                    {
                        if (i == (avgsell.Length - 1))
                        {

                            this.dirForAction = false;
                            return true;

                        }
                        i++;
                        ok = false;
                    }
                    if (ok)
                    {
                        i++;
                        ok = true;
                    }
                    counter++;
                }
                if (counter <= this.paramArr[3] && i < avgsell.Length)
                {
                    this.dirForAction = false;
                    return true;
                }

            }
            return false;
        }
        public void reset() 
        {
            this.miller = true;
            this.timeInWait = 0;
            this.inPos = false;
            this.firstTime = true;
            this.toPass = false;
        }
    }
}
