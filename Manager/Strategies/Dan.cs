using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Strategies
{
    class Dan:Strategy
    {
        private DateTime timetoadd;
        private bool exit;
        private bool check;
        private int dataCounter;
        private double avgsellsum ;
        private double avgbuysum;
        private double[] avgbuy;
        private double[] avgsell;
        private RowData pRowData;
        public static int autoNum = 0;
        private int stratNum;
        private int arrPointer;
        private bool firstTimeIn;
        private char kind;
        private bool inthing;
        private bool bool2;
        private bool bool1;
        private double lastcase;
        private int myCounterBuy;
        private int myCounterSell;
        
        public Dan(int minutesJump, int numberOfCels, double diffrence, int updownfaults, double exitinnotime, double lost, double percentToExit) //constractor
            : base() 
        {
            this.bool1 = true;
            this.bool2 = true;
            this.inthing = false;
            this.myCounterBuy = 0;
            this.myCounterSell = 0;
            this.timetoadd = new DateTime(2015, 1, 1, 1, 0, 1);
            this.lastcase = 1.8;
            this.exit = false;
            this.dataCounter = 0;
            this.arrPointer = 0;
            this.paramArr[1] = minutesJump;
            this.paramArr[2] = numberOfCels;
            this.paramArr[3] = updownfaults;
            this.paramArr[0] = diffrence;
            this.paramArr[4] = exitinnotime;
            this.paramArr[5] = lost;
            this.paramArr[6] = percentToExit;
            Dan.autoNum++;
            this.stratNum = autoNum;
            this.name = @"Monkey - Dan No " + stratNum.ToString();
            this.firstTimeIn = true;
            this.avgbuy = new double[numberOfCels];
            this.avgsell = new double[numberOfCels];
            this.check = false;
        }
        public override void getInfo(RowData r)
        {
            if (!this.exit)
            {
                trys(r);
                if (!this.firstTimeIn)
                {
                    if (!this.check)//going in pos 
                    {
                        if (r.datetime.Hour <= 15)
                        {
                            switch (algoIn())
                            {
                                case 'b':

                                    this.kind = 's';
                                    if (sell(r, 1))
                                        this.check = true;
                                    break;
                                case 's':

                                    this.kind = 'b';
                                    this.check = buy(r, 1);
                                    break;
                                case 'n':
                                    this.kind = 'n';
                                    break;

                            }
                        }
                    }
                    else
                    {

                        switch (kind)
                        {
                            case 's':
                                if (algoOutSell())
                                {
                                    double z = this.getList()[this.getList().Count - 1].getFirst().bid1;
                                    if (r.datetime.Hour >= 15 && r.datetime.Hour < 16)
                                    {
                                        if (z - r.ask1 > this.paramArr[4])//add pram
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (r.datetime.Hour >= 16)
                                        {
                                            if (z - r.ask1 > 0)
                                            {
                                                if (buy(r, 1))
                                                {
                                                    this.exit = true;
                                                    this.check = false;//stop
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                }//buy
                                else
                                {
                                    double z = this.getList()[this.getList().Count - 1].getFirst().bid1;
                                    if (r.ask1 / z <= this.paramArr[0])
                                    {
                                        //if (z - r.ask1 > 1)
                                        //{
                                        if (buy(r, 1))
                                        {
                                            this.check = false;
                                        }
                                    }   
                                    if (r.datetime.Hour >= 15 && r.datetime.Hour < 16)
                                    {
                                        
                                        if (z - r.ask1 > this.paramArr[5])//add pram
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                    if (r.datetime.Hour >= 16)
                                    {
                                        
                                        if (z - r.ask1 > -2)
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                }
                                break;

                            case 'b':
                                if (algoOutBuy())
                                {
                                    double z = this.getList()[this.getList().Count - 1].getFirst().ask1;

                                    if (r.datetime.Hour >= 15)
                                    {
                                        if (r.bid1 - z > this.paramArr[4])//add pram 
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }

                                    }
                                    else
                                    {
                                        if (r.datetime.Hour >= 16)
                                        {
                                            if (r.bid1 - z > 0)
                                            {
                                                if (sell(r, 1))
                                                {
                                                    this.check = false;//stop
                                                    this.exit = true;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }
                                    }
                                }
                                
                                else
                                {
                                    double z = this.getList()[this.getList().Count - 1].getFirst().ask1;
                                    if (z / r.bid1 <= this.paramArr[0])
                                    {
                                        //if(r.bid1-z>1)
                                        //{
                                        if (sell(r, 1))
                                        {
                                            this.check = false;
                                        }
                                        //}
                                    }
                                    if (r.datetime.Hour >= 15)
                                    {
                                        
                                        if (r.bid1 - z > this.paramArr[5])//add pram 
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }

                                    }
                                    if (r.datetime.Hour >= 16)
                                    {
                                        
                                        if (r.bid1 - z > -2)
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }
                                    }
                                }



                                //sell
                                break;
                            case 'n':
                                break;

                        }


                    }
                }
            }
        }
       

        public bool algoOutBuy()
        {
            double max = this.avgbuy.Max();
            if (this.avgbuy[this.avgbuy.Length - 1] > this.avgbuy[this.avgbuy.Length - 2])//תנאי מכירה
            {
                if ((1 - (this.avgbuy[this.avgbuy.Length - 1] / max)) * 100 > 1 - paramArr[6])
                {
                    return true;
                }
                
            }
            return false;

        }
        public bool algoOutSell()
        {
            double max = this.avgsell.Max();
            if (this.avgsell[this.avgsell.Length-1]<this.avgsell[this.avgsell.Length-2])//תנאי מכירה
            {
                if ((1-(this.avgsell[this.avgsell.Length-1] / max)) * 100 > 1 - paramArr[6])
                {
                    return true;
                }
                
            }
            return false;
        }
        public char algoIn()
        {
            int buy = 0;
            int sell = 0;
            for (int i = 1; i < this.avgsell.Length; i++)
            {
                if (this.avgbuy[i - 1] < this.avgbuy[i])
                {
                    buy++;
                }
                if (this.avgsell[i - 1] > this.avgsell[i])
                {
                    sell++;
                }
            }
            if (buy == this.avgbuy.Length - 1)
            {
                return 's';
            }

            if (sell == this.avgsell.Length - 1)
            {
                return 'b';
            }
            return 'n';
        }
        public override void reset()
        {
            this.timetoadd = new DateTime(2015, 1, 1, 1, 2, 1);
            this.lastcase = 1.8;
            this.pRowData = new RowData();

            for (int i = 0; i < avgbuy.Length; i++)
            {
                this.avgbuy[i] = 0;
                this.avgsell[i] = 0;
            }
            this.check = false;
            this.firstTimeIn = true;
            this.dataCounter = 0;
            this.arrPointer = 0;
            this.exit = false;
        }
        public void trys(RowData r)
        {
            if (this.pRowData.ask1 == 0)
            {
                this.pRowData = new RowData(r);
            }
            if (this.firstTimeIn)
            {
                int x = ProgramMain.deltaMin(r.datetime, this.pRowData.datetime);
                if (x < this.paramArr[1])
                {
                    this.avgbuysum += r.ask1;
                    this.avgsellsum += r.bid1;
                    this.dataCounter++;
                }
                else
                {
                    this.avgbuy[arrPointer] = this.avgbuysum / this.dataCounter;
                    this.avgsell[arrPointer] = this.avgsellsum / this.dataCounter;
                    arrPointer++;
                    this.dataCounter = 1;
                    this.avgbuysum = r.ask1;
                    this.avgsellsum = r.bid1;
                    this.pRowData = new RowData(r);

                }
                if (this.avgbuy[this.avgbuy.Length - 1] != 0)
                {
                    this.firstTimeIn = false;
                }
            }
            else
            {
                int x = ProgramMain.deltaMin(r.datetime, this.pRowData.datetime);
                if (x < this.paramArr[1])
                {
                    this.avgbuysum += r.ask1;
                    this.avgsellsum += r.bid1;
                    this.dataCounter++;
                }
                else
                {
                    for (int i = 1; i <= this.avgsell.Length - 1; i++)
                    {
                        this.avgsell[i - 1] = this.avgsell[i];
                        this.avgbuy[i - 1] = this.avgbuy[i];
                    }
                    this.avgbuy[this.avgbuy.Length - 1] = this.avgbuysum / this.dataCounter;
                    this.avgsell[this.avgsell.Length - 1] = this.avgsellsum / this.dataCounter;
                    this.dataCounter = 1;
                    this.avgbuysum = r.ask1;
                    this.avgsellsum = r.bid1;
                    this.pRowData = new RowData(r);

                }

            }

        }
    }
}
                            

                        
                                   /* if (r.ask1 / z <= this.paramArr[0])
                                    {
                                        //if (z - r.ask1 > 1)
                                        //{
                                        if(buy(r, 1))
                                        {
                                        this.check = false;
                                        }
                                            //}
                                    }
                                    if (r.datetime.Hour >= 15 && r.datetime.Hour < 16)
                                    {
                                        if (z - r.ask1 > this.paramArr[4])//add pram
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                    if (Math.Abs(z - r.ask1) > this.lastcase)
                                    {
                                        if (buy(r, 1))
                                        {
                                            this.exit = true;
                                            this.check = false;//stop
                                        }
                                    }
                                    if (r.datetime.Minute - this.timetoadd.Minute == 2)
                                    {
                                        this.lastcase += 0.1;
                                        this.timetoadd.AddMinutes(2);
                                    }
                                }//buy
                                else 
                                {
                                    if (buy(r, 1))
                                    {
                                        this.check = false;
                                    }
                                if (r.datetime.Hour >= 15 && r.datetime.Hour < 16)
                                    {
                                        if (z - r.ask1 > this.paramArr[5])//add pram
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                    }
                                    if (r.datetime.Hour >= 16)
                                    {
                                        if (Math.Abs(z - r.ask1) >this.lastcase)
                                        {
                                            if (buy(r, 1))
                                            {
                                                this.exit = true;
                                                this.check = false;//stop
                                            }
                                        }
                                        if (r.datetime.Minute - this.timetoadd.Minute == 2)
                                        {
                                            this.lastcase += 0.1;
                                            this.timetoadd.AddMinutes(2);
                                        }
                                    }
                                }*/
                                
                           
                            
                                   /* if (z / r.bid1 <= this.paramArr[0])
                                    {
                                        //if(r.bid1-z>1)
                                        //{
                                        if(sell(r, 1))
                                        {
                                        this.check = false;
                                        }
                                        //}
                                    }
                                    if (r.datetime.Hour >= 15)
                                    {
                                        if (r.bid1 - z > this.paramArr[4])//add pram 
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }

                                    }
                                    if (r.datetime.Hour >= 16)
                                    {
                                        if (Math.Abs(r.bid1 - z) > this.lastcase)
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }
                                        if (r.datetime.Minute - this.timetoadd.Minute == 2)
                                        {
                                            this.lastcase += 0.1;
                                            this.timetoadd.AddMinutes(2);
                                        }
                                    }
                                }
                                else 
                                {
                                    if (sell(r, 1))
                                    {
                                        this.check = false;
                                    }    
                                    if (r.datetime.Hour >= 15)
                                    {
                                        if (r.bid1 - z > this.paramArr[5])//add pram 
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }

                                    }
                                    if (r.datetime.Hour >= 16)
                                    {
                                        if (Math.Abs(r.bid1 - z) > this.lastcase)
                                        {
                                            if (sell(r, 1))
                                            {
                                                this.check = false;//stop
                                                this.exit = true;
                                            }
                                        }
                                        if(r.datetime.Minute-this.timetoadd.Minute==2)
                                        {
                                            this.lastcase += 0.1;
                                            this.timetoadd.AddMinutes(2);
                                        }
                                    }
                                }
                            
                            
                            
                            //sell*/
                               
                           


                    
/*public void setQueue(RowData r)
        {
            if (this.pRowData.ask1 == 0)
            {
                this.pRowData = new RowData(r);
            }
            if (this.firstTimeIn)//to build the first deltaTime min
            {
                int x = ProgramMain.deltaMin(r.datetime, this.pRowData.datetime);
                if (x > this.paramArr[0])
                {
                    this.firstTimeIn = false;
                    //=======orgenizing
                    if (x <= this.paramArr[3] * (arrPointer + 1))
                    {
                        this.avgbuysum += r.ask1;
                        this.avgsellsum += r.bid1;
                        this.dataCounter++;
                    }
                    else
                    {
                        this.avgbuy[arrPointer] = this.avgbuysum / this.dataCounter;
                        this.avgsell[arrPointer] = this.avgsellsum / this.dataCounter;
                        arrPointer++;
                        this.dataCounter = 1;
                        this.avgbuysum = r.ask1;
                        this.avgsellsum = r.bid1;
                    }
                }
                else
                {
                    if (ProgramMain.deltaMin(r.datetime, this.pRowData.datetime) >= this.paramArr[1])
                    {
                        this.pRowData = new RowData(r);
                        QRD.Enqueue(r);
                        while (ProgramMain.deltaMin(pRowData.datetime, QRD.Peek().datetime) > this.paramArr[0])
                        {
                            QRD.Dequeue();
                        }
                        this.pRowData = new RowData(r);

                        orgenizer(this.sumAsk1, this.sumBid1, this.numberData);
                        this.numberData = 0;
                        this.sumBid1 = 0;
                        this.sumAsk1 = 0;

                    }
                    else
                    {
                        this.sumBid1 += r.bid1;
                        this.sumAsk1 += r.ask1;
                        this.numberData++;
                        QRD.Enqueue(r);
                    }
                }
            }
        }
        public void orgenizer(double sumAsk1,double sumBid1, int c) 
        {
            for (int i = 1; i < this.avgsell.Length - 1; i++)
            {
                this.avgsell[i - 1] = this.avgsell[i];
                this.avgbuy[i - 1] = this.avgbuy[i];
            }
            this.avgbuy[this.avgbuy.Length - 1] = sumAsk1 / c;
            this.avgsell[this.avgsell.Length - 1] = sumBid1 / c;
        }*/