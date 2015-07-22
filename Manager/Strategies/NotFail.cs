using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Strategies
{



       class MillerData 
       {
          
          private int _min;
          private double _b;
          private double _s;
          private int _counteRow;
          public MillerData(int min,double b,double s) 
          {
              _b = b;
              _min = min;
              _s = s;
              _counteRow = 0;
          }
          public MillerData()
          {
              _b = 0;
              _min = 0;
              _s = 0;
              _counteRow = 0;
          }
          public MillerData(MillerData other) 
          {
              this._b = other._b;
              this._min = other._min;
              this._s = other._s;
              this._counteRow = other._counteRow;
                         
          }

          public int getMin() 
          {
              return _min; 
           
          }
          public double getB ()
          {
              return _b;
              
          }
          public double getS() 
          {
              return _s;
          }
          public  int getCountRow() 
          {
              return _counteRow;
          
          }
          public void setB(double b) 
          {
              _b = b;
          }
          public void setS(double b)
          {
              _s = b;
          }
          public void setCounterRow(int b)
          {
              _counteRow = b;
          }
          public void setMin(int b)
          {
              _min = b;
          }


           
       
       }
    

    class NotFail : Strategy
    {
        private double NewHelper;
        public static int c = 1;
        private bool finishingLoading = false;
        private bool pos = false; 
        private bool wantToDoAction;
        private bool dirofAction;
        private MillerData[] _myDataInArr;
        private bool canTrade=false;
        private int myCounter=0;
        private double myB = 0;
        private double myS = 0;
        private  Queue < MillerData > _myData = new Queue<MillerData>();
        private int _NumOfSaveBack;
        private int _dt;
        private int _waitDt;
        private MillerData millerhelper = new MillerData();
        public NotFail (int numOfSaveBack, int dt , int waitDt  )
         {
             this.name = "Miller Strat: " + c;
             c++;
             _NumOfSaveBack = numOfSaveBack;
             _dt = dt;
             _waitDt = waitDt;
             this.paramArr[0] = numOfSaveBack;
             this.paramArr[1] = dt;
             this.paramArr[2] = waitDt;
    
         }
        public override void getInfo(RowData r)
        {
            if (!pos)
            {
                if (!wantToDoAction)
                {
                    if (!canTrade)
                    {
                        canTrade = setInfo(r);
                     
                    }
                    if(canTrade)
                    {
                        setInfo(r);
                        wantToDoAction = algo();
                        canTrade = false;
                    }
                   


                }
                if(wantToDoAction)
                {
                    setInfo(r);
                    if (r.datetime.Minute - NewHelper < _waitDt)
                    {
                        pos = ifToDoAction(r);
                    }
                    else 
                    {
                        wantToDoAction = false;
                        canTrade = false; 

                    }

                }
            }
            else 
            {
                if(!wantToDoAction)
                {
                   if (!setInfo(r) || finishingLoading ) 
                   {
                    finishingLoading = true;
                   wantToDoAction = algoEnd(); 


                   }
                }
                if (wantToDoAction)
                {
                    setInfo(r);
                    if (r.datetime.Minute - _myDataInArr[_myDataInArr.Length - 1].getMin() < _waitDt)
                    {
                        dirofAction = !dirofAction;
                        pos = !ifToDoAction(r);
                    }
                    else 
                    {
                        wantToDoAction = false ;
                        finishingLoading = false;
                    }
                }
               

            }
        }
        private bool setInfo(RowData r)
        {
            bool ok = false;
      
                if (millerhelper.getMin() == 0 || ((r.datetime.Minute - millerhelper.getMin()) < _dt))
                {
                    if (millerhelper.getMin() == 0)
                    {
                        millerhelper.setMin(r.datetime.Minute);
                    }
                    myB += r.ask1;
                    myS += r.bid1;
                    myCounter++;
                }
                else 
                {
                    myB = myB / myCounter;
                    myS = myS / myCounter;
                    millerhelper.setS(myS);
                    millerhelper.setB(myB);
                    millerhelper.setCounterRow(myCounter);
                    myCounter = 0;
                    myB = 0;
                    myS = 0;
                    _myData.Enqueue(new MillerData (millerhelper));
                    millerhelper.setMin(r.datetime.Minute);
                    if (_myData.Count >= this.paramArr[0]) 
                    {
                        if (_myData.Count > this.paramArr[0]) 
                        {
                            _myData.Dequeue();
                        }
                        ok = true;
                        NewHelper = millerhelper.getMin();

                    


                }
                

            }

            return ok;
            /*
            bool ok =false;
            
                if (millerhelper.getMin() == 0)
                {
                    millerhelper.setMin(r.datetime.Minute);
                    myCounter++;
                    myB += r.ask1;
                    myS += r.bid1;
                }
                else
                {
                    if (this._myData.Count == 0)
                    {
                        if (r.datetime.Minute - millerhelper.getMin() != _dt)
                        {
                            myCounter++;
                            myB += r.ask1;
                            myS += r.bid1;
                        }
                        else
                        {
                            myB = myB / myCounter;
                            myS = myS / myCounter;
                            millerhelper.setS(myS);
                            millerhelper.setB(myB);
                            millerhelper.setCounterRow(myCounter);
                            _myData.Enqueue(new MillerData(millerhelper));
                            if (_myData.Count > _NumOfSaveBack)
                            {
                                ok = true;
                                _myData.Dequeue();
                            }
                            myB = r.ask1;
                            myS = r.bid1;
                            myCounter = 1;
                            millerhelper.setMin(r.datetime.Minute);

                        } 
                    }
                    else
                    {
                        if (((this._myData.Peek().getMin()) + (_dt * _myData.Count)) - r.datetime.Minute == _dt)
                        {
                            myB = myB / myCounter;
                            myS = myS / myCounter;
                            millerhelper.setS(myS);
                            millerhelper.setB(myB);
                            millerhelper.setCounterRow(myCounter);
                            _myData.Enqueue(new MillerData(millerhelper));
                            if (_myData.Count > _NumOfSaveBack)
                            {
                                ok = true;
                                _myData.Dequeue();
                            }
                            myB = r.ask1;
                            myS = r.bid1;
                            myCounter = 1;
                            millerhelper.setMin(r.datetime.Minute);
                        }
                        else 
                        {
                            myCounter++;
                            myB += r.ask1;
                            myS += r.bid1;

                        }
                    }

                } 
            return ok; */
        } 

        private bool algo() 
        { 
            int cBuy=0,cSell=0;
            _myDataInArr = _myData.ToArray();
            for (int i = 0; i < _myDataInArr.Length-1; i++ ) 
            {
                if ((_myDataInArr[i].getB() > _myDataInArr[i + 1].getB()) && (_myDataInArr[i].getS() > _myDataInArr[i + 1].getS())) 
                {
                    cSell++;
                }
                if ((_myDataInArr[i].getB() < _myDataInArr[i + 1].getB()) && (_myDataInArr[i].getS() < _myDataInArr[i + 1].getS()))
                {
                    cBuy++;
                }



            }
            if (cBuy == _myDataInArr.Length-2)
            {
                dirofAction = true;
                return true;
            }
            if (cSell == _myDataInArr.Length - 2)
            {
                dirofAction = false;
                return true;
            }

            return false;
        }
        private bool ifToDoAction(RowData r) 
        {
            bool ok =false;
            if (dirofAction)
            {
                if (r.ask1 < _myDataInArr[_myDataInArr.Length - 1].getB())
                {
                    wantToDoAction = false; 
                    ok = buy(r, 1);
                }
            }
            else 
            {
                if (r.bid1 > _myDataInArr[_myDataInArr.Length - 1].getS())
                {
                    wantToDoAction = false; 
                    ok = sell(r, 1);
                }

            }
            return ok; 
                
        }
        private bool algoEnd() 
        {
            int counterForExit = 0;
            _myDataInArr = _myData.ToArray();
            int n = _myDataInArr.Length/2 ;
            bool ok = false;
            if (n%2 == 0 )
            {
                ok = true ; 
            }
            if (dirofAction) 
            {
                for (int i = n; i < _myDataInArr.Length - 1; i++)
                {
                    if ((_myDataInArr[i].getB() > _myDataInArr[i + 1].getB()) && (_myDataInArr[i].getS() > _myDataInArr[i + 1].getS()))
                    {
                       counterForExit++;
                    }
                }    

             }
            else
            {
                for (int i = n; i < _myDataInArr.Length - 1; i++)
                {
                    if ((_myDataInArr[i].getB() < _myDataInArr[i + 1].getB()) && (_myDataInArr[i].getS() < _myDataInArr[i + 1].getS()))
                    {
                       counterForExit++;
                    }
                }    

            }
                if (ok) 
                {
                    if ((_myDataInArr.Length- (n+1)) == counterForExit  ) 
                    {
                        return true; 
                    }
                }
                else
                {
                    if(counterForExit*2 == (_myDataInArr.Length-1) )
                    {
                        return true;
                    }
                }


                return false;

            }
        public override void reset()
        {
            MillerData toGC;
            this.finishingLoading = false;
            this.pos = false; 
            canTrade=false;
            this.myCounter=0;
            this.myB = 0;
            this.myS = 0;
            foreach (MillerData md in this._myData)
            {
                toGC = md;
                toGC = null;
            }
            this._myData.Clear();
            millerhelper.setB(0);
            millerhelper.setS(0);
            millerhelper.setMin(0);
            millerhelper.setCounterRow(0);
        }
        }






        // add fanction here bitc%


    }


