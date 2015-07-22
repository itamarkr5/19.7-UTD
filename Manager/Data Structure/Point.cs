using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manager.Data_Structure
{
    public class Point
    {
        public double x;
        public double y;

        public Point(double x, double y)
        {
            this.x = x;
            this.y = y;
        }
        public Point(Point p) 
        {
            this.x = p.x;
            this.y = p.y;
        }

    }
}
