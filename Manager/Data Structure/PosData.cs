using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    class saveData
    {
        // 2 RowData = position
            private RowData rFirst; // enter RowData
            private RowData rEnd; // exit RowData
            private bool posEnd = false; // bool if position is full
            private bool isProfit = false; // bool if position is profitable
            private double profit; // profit per position
            private double totalSum; //strategy sum untill end of pos for test
            private int dirAtEnter; // dir for first entrie

            public void setProfit(double p)
            {
                this.profit = p;
                if (this.posEnd)
                {
                    if (this.profit > 0)
                        this.isProfit = true;
                    else
                        this.isProfit = false;
                }
                
            }
            public double getProfit()
            {
                return this.profit;
            }

            public int getDir()
            {
                return this.dirAtEnter;
            }

            public double getTotalSum()
            {
                return this.totalSum;
            }
            public void setTotalSum(double sum)
            {
                this.totalSum = sum;
            }
  
            public void setBoolProfit(bool ok)
            {
                this.isProfit = ok;
            }
            public bool getBoolProfit()
            {
                return this.isProfit;
            }

            public saveData(RowData r)
            {
                this.rFirst = new RowData(r);
                this.posEnd = false;
                this.dirAtEnter = r.dir;
            } // constractor

            public void setEndRowData(RowData r)
            {
                this.rEnd = new RowData(r);
                posEnd = true;
            } // set RowData at exit

            public RowData getFirst()
            {
                return this.rFirst;
            }
            public RowData getEnd()
            {
                return this.rEnd;
            }

            public bool getPosEnd()
            {
                return posEnd;
            } // returns true if pos ended | false if not
            public void setPosEndBool(bool ok)
            {
                this.posEnd = ok;
            }
        
            public String printString()
            {
                String s = "";
                s = this.rFirst.datetime.ToString();
                s += "," + this.rFirst.ask1.ToString();
                s += "," + this.rFirst.askV.ToString();
                s += "," + this.rFirst.bid1.ToString();
                s += "," + this.rFirst.bidV.ToString();
                s += "," + this.dirAtEnter.ToString();
               
                if ((rEnd.ask1!= 0) && (rEnd.bid1!=0))
                {
                    s += "," + this.rEnd.datetime.ToString();
                    s += "," + this.rEnd.ask1.ToString();
                    s += "," + this.rEnd.askV.ToString();
                    s += "," + this.rEnd.bid1.ToString();
                    s += "," + this.rEnd.bidV.ToString();
                    s += "," + this.rEnd.dir.ToString();
                    s += "," + this.profit.ToString();
                }
               
                return s;
            }

            public String clearRow()
            {
                String s = "";
                s = "  ";
                s += "," + "  ";
                s += "," + "  ";
                s += "," + "  ";
                s += "," + "  ";
                s += "," + "  ";

                if ((rEnd.ask1 != 0) && (rEnd.bid1 != 0))
                {
                    s += "," + "  ";
                    s += "," + "  ";
                    s += "," + "  ";
                    s += "," + "  ";
                    s += "," + "  ";
                    s += "," + "  ";
                    s += "," + "  ";
                }
                return s;
            }


        }
    }
