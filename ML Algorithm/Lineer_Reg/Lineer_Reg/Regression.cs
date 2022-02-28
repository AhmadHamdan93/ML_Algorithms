using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lineer_Reg
{
    class Regression
    {
        List<SampleData> data;
        double b,a;
        public Regression(List<SampleData> readData)
        {
            data = new List<SampleData>();
            data = readData;
        }
        void find_b()
        {
            int xx = squareColumn(0);
            int xy = squareColumn(1);
            int n = data.Count;
            double x_ = getAvgColumn(false);
            double y_ = getAvgColumn(true);
            b = ( xy - n * x_ * y_) / (xx - n * x_ * x_);
            b = Math.Round(b, 4);
        }
        void find_a()
        {
            a = getAvgColumn(true) - b * getAvgColumn(false);
            a = Math.Round(a, 4);
        }
        double getAvgColumn(Boolean column) // for x : column = false ///// for y : column = true 
        {
            int sum = 0;
            double avg =0.0;
            for (int i = 0; i < data.Count; i++)
            {
                if (column)
                {
                    sum += data[i].Y;
                }
                else
                {
                    sum += data[i].X;
                }
            }
            avg = 1.0 * sum / data.Count; 
            return avg;
        }
        int squareColumn(int type)   // for find x2 : type 0 ///// for find xy : type 1
        {
            int sum = 0;
            for (int i = 0; i < data.Count; i++)
            {
                if (type == 0)
                {
                    sum += data[i].X * data[i].X;
                }
                else
                {
                    sum += data[i].X * data[i].Y;
                }
            }
            return sum;
        }
        public void find_const()
        {
            find_b();
            find_a();
            //double ss = find_rate_error();
        }

        public double calculate(double val)
        {
            double res = a + b * val;
            res = Math.Round(res, 4);
            return res;
        }

        public double find_rate_error()
        {
            double res = 0.0,differance;
            for (int i = 0; i < data.Count; i++)
            {
                differance = data[i].Y - calculate(data[i].X);
                differance = differance * differance;
                res += differance; 
            }
            res = res / (data.Count - 2);
            res = Math.Sqrt(res);
            res = Math.Round(res, 4);
            return res;
        }

        public double get_a() { return a; }
        public double get_b() { return b; }

    }
}
