using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Manager.Data_Structure;

namespace Manager.Scoring
{
    public class Test2
    {
        public static double[] linerAndA(Point[] points)
        {
            double[] returns = new double[3];
            double  s = 0, f = 0, c = 0;
            Point avgpoints = new Point(avg(points));
            foreach (Point p in points)
            {
                s += (p.x - avgpoints.x) * (p.y - avgpoints.y);
                c += Math.Pow((p.x - avgpoints.x), 2);
            }
            returns[0] = (s / c);
            returns[2] = avgpoints.y - (returns[0] * avgpoints.x);
            s = 0;
            f = 0;
            foreach (Point p in points)
            {
                f += Math.Pow((((p.x) * returns[0]) - avgpoints.y), 2);
                s += Math.Pow((p.y - avgpoints.y), 2);
            }
            returns[1] = f / s;


            return returns;
        }//[0] = a , [1] = r^2 , [2]=b
        public static Point avg(Point[] points) 
        {
            double x = 0, y = 0;
            foreach(Point p in points)
            {
                x += p.x;
                y += p.y;
            }
            return new Point(x / points.Length, y / points.Length);

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

        
        public static double score(int uppos,int pos,double sum,List<double> lis)         
        {
            double[] arr = pointGenerator(lis);
            Console.WriteLine(arr[1]);
            return ((arr[1]*uppos)/pos)*sum*(Math.Abs(arr[0]));
        }
    }
}