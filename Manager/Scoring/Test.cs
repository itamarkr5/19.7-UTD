using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Data_Structure;
namespace Manager
{
    class Test
    {
        public static List<RowData> back = new List<RowData>();

        public static double MaxDrop(List<saveData> ListSaveData)
        {
            if (ListSaveData.Count() >= 3)
            {
                double prev = ListSaveData[0].getTotalSum();
                double now = ListSaveData[0].getTotalSum();
                double next = ListSaveData[0].getTotalSum();
                int count = 0;
                bool b = false;
                double Max = 0, MaxDrop = 0;
                foreach (saveData o in ListSaveData)
                {
                    if (count < 3)
                        count++;
                    else
                    {
                        if (!b)
                        {
                            if (now > prev && now < next)
                            {
                                Max = now;
                                b = true;
                            }
                            else
                            {
                                prev = now;
                                now = next;
                                next = o.getProfit();
                            }
                        }
                        else
                        {
                            if (now < prev && now > next)
                            {
                                if (MaxDrop < (Max - now))
                                    MaxDrop = Max - now;
                                b = false;
                            }
                            else
                            {
                                prev = now;
                                now = next;
                                next = o.getProfit();
                            }

                        }
                    }
                }
                return MaxDrop;
            }
            else
            {
                if (ListSaveData.Count() == 1 /*|| ListSaveData.Count() == 0*/)
                    return 0.0;
                else
                {
                    return Math.Abs(ListSaveData[0].getTotalSum() - ListSaveData[1].getTotalSum());
                }
            }//foreach End
         }
        public static double MaxSum(List<saveData> ListSaveData, List<RowData> ListRowData)
        //ListRowData => info of all the day
        //ListSaveData => info of all my pos's
        {
            double first = 0;
            double second = 0;
            double Max = 0;
            back.Clear();
            Stack<RowData> StackRowData = new Stack<RowData>();
            foreach (saveData s in ListSaveData)
            {
                foreach (RowData r in ListRowData)
                {
                    if (r.Equals(s.getFirst()))
                        first = helpMaxsum(StackRowData, s.getFirst());
                    else
                    {
                        if (r.Equals(s.getEnd()))
                            second = helpMaxsum(StackRowData, s.getEnd());

                        else
                            StackRowData.Push(r);
                    }
                   
                    if (first != 0 && second != 0)
                    {
                        if (Math.Abs(first - second) > Max)
                        {
                            Max = Math.Abs(first - second);
                            first = 0;
                            second = 0;
                        }
                    }

                }

            }
            
            return Max;
        }
        public static double helpMaxsum(Stack<RowData> back, RowData r)//send back reverse
        {
            RowData prev = null;
            if (back.Count != 0)
            {
                foreach (RowData rev in back)
                {
                    if (r.dir == 1)//buy 
                    {
                        if (rev.ask1 > r.ask1)
                        {
                            if (prev == null)
                                return rev.ask1;
                            return prev.ask1;

                        }
                        prev = rev;
                    }
                    else
                    {
                        if (rev.bid1 > r.bid1)
                        {
                            if (prev == null)
                                return rev.bid1;
                            return prev.bid1;

                        }
                        prev = rev;
                    }
                }
            }
            else
            {
                if (r.dir == 1)
                    return r.ask1;
                return r.bid1;
            }
            return 0;
        }
        public static double[] linerAndA(Point[] points)
        {
            double[] returns = new double[3];
            double AVGx = 0, AVGy = 0, s = 0, f = 0, c = 0;
            foreach (Point p in points)
            {
                s += (p.x - AVGx)*(p.y - AVGy);
                c += Math.Pow((p.x - AVGx), 2);
            }
            returns[0] = (s  / c);
            returns[2] = AVGy - (returns[0] * AVGx);
            s = 0;
            f = 0;
            foreach (Point p in points)
            {
                f += Math.Pow((((p.x) * returns[0]) - AVGy), 2);
                s += Math.Pow((p.y - AVGy), 2);
            }
            returns[1] = f / s;


            return returns;
        }
        public static double[] pointGenerator(List<double> lis)
        {
            Point[] p = new Point[lis.Count()];
            for (int i = 0; i < p.Length; i++)
            {
                p[i] = new Point(i, lis[i]);    
                
            }
            return linerAndA(p);
        }

        public static double testScore(List<saveData> ListData, double maxSum, List<double> SumPerDay, double sumstrat)
        {
            double MaxD = 0;
            double x = 0;
            double MaxS = 0;
            
                x = MaxDrop(ListData);
                if (x > MaxD)
                    MaxD = x;
                MaxS = maxSum;

            
            double[] liner = pointGenerator(SumPerDay);
            double sumEnd = ((Math.Abs(sumstrat)*liner[1]*(sumstrat-MaxD))/MaxS); 
            return sumEnd;
        }
    }

}

