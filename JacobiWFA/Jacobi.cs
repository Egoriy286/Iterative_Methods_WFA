using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JacobiWFA
{
    public class Jacobi : IterativeMethod
    {
        public Jacobi()
        {
            this.Size = 4;
            Matrix.Add(new List<double>() { 2, 1, 1, 0 });
            Matrix.Add(new List<double>() { 4, 3, 3, 1 });
            Matrix.Add(new List<double>() { 8, 7, 9, 5 });
            Matrix.Add(new List<double>() { 6, 7, 9, 8 });
            VectorB  = new List<double>() { 4, 11, 29, 30 };
            VectorX0 = new List<double>() { VectorB[0] / Matrix[0][0], VectorB[1]/Matrix[1][1], VectorB[2]/ Matrix[2][2], VectorB[3]/Matrix[3][3] };
        }
        
        public Jacobi(List<List<double>> Matrix, List<double> VectorB, List<double> VectorX0, int Size)
        {
            this.Size = Size;
            for (int i = 0; i < Size; i++)
            {
                this.Matrix.Add(new List<double>());
                this.VectorX0.Add(VectorX0[i]);
                this.VectorB.Add(VectorB[i]);
                for (int j = 0; j < Size; j++)
                {
                    this.Matrix[i].Add(Matrix[i][j]);
                }
            }
        }

        public override string Calculate()
        {
            VectorX = new List<double>();
            List<double> VectorX1 = new List<double>();
            for (int i = 0; i < Size; i++)
            {
                VectorX1.Add(0.0);
                VectorX.Add(0.0);
            }
            for(int i=0;i<Size;i++)
            {
                VectorX[i] = VectorX0[i];
            }
            while (true)
            {


                for (int  i=0; i < Size;i++)
                {
                    VectorX1[i]=VectorB[i];
                    for(int j = 0; j < Size; j++)
                    {
                        if(i!=j) 
                            VectorX1[i] -= Matrix[i][j] * VectorX[j];
                    }
                    VectorX1[i] /= Matrix[i][i];
                }



                for(int i = 0; i < Size; i++)
                {
                    VectorX[i] = VectorX1[i];
                }
                if (CheckSimpleIterative(VectorX1))
                    return Result(VectorX1);
                else if (Iter > Itermax)
                {
                    string s = $"iter ={Iter}; norm:{Norm} ;Vectors";
                    for (int i = 0; i < Size; i++)
                        s += VectorX1[i].ToString() + " ";
                    return s;
                }
            }
        }
    }
}
