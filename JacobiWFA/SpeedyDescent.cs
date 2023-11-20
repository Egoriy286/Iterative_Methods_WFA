using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
namespace JacobiWFA
{
    public class SpeedyDescent : IterativeMethod // Метод Быстрого спуска
    {
        List<double> residual = new List<double>();
        public SpeedyDescent(){}

        public SpeedyDescent(List<List<double>> Matrix, List<double> VectorB, List<double> VectorX0, int Size)
        {
            this.Size = Size;
            for (int i = 0; i < Size; i++)
            {
                this.residual.Add(0);
                this.Matrix.Add(new List<double>());
                this.VectorX0.Add(VectorX0[i]);
                this.VectorB.Add(VectorB[i]);
                for (int j = 0; j < Size; j++)
                {
                    this.Matrix[i].Add(Matrix[i][j]);
                }
            }
        }

        public double MultiVector(List<double> vector1, List<double> vector2)
        {
            double temp = 0.0;
            for (int i = 0; i < Size; i++)
            {
                temp += (vector1[i] * vector2[i]);
            }
            return temp;
        }

        public override string Calculate()
        {
            VectorX = new List<double>();
            List<double> Ar = new List<double>();
            List<double> VectorX1 = new List<double>();
            double Ar_r, Ar_Ar, Tao, r_r;
            for (int i = 0; i < Size; i++)
            {
                VectorX1.Add(0.0);
                VectorX.Add(0.0);
                Ar.Add(0.0);
            }
            for (int i = 0; i < Size; i++)
            {
                VectorX[i] = VectorX0[i];
            }


            int k = 0;
            while (true)
            {
                residual = Difference(MultiMatrixVector(Matrix, VectorX), VectorB); // Невязка вектор

                Ar = MultiMatrixVector(Matrix, residual);                           // Матрица A * Невязка

                r_r = MultiVector(residual, residual);                              // double Скалярное произведение( Невязка * Невязка )

                Ar_r = MultiVector(Ar, residual);                                   // double Скалярное произведение( (A * Невязка) * Невязка )

                Ar_Ar = MultiVector(Ar, Ar);                                        // Скалярное произведение( (A * Невязка) * (A * Невязка) )

                Tao = r_r / Ar_r;                                                   // Нахождение  Тао ( ( Невязка * Невязка ) / Скалярное произведение( (A * Невязка) * Невязка ) )

                for (int i = 0; i < Size; i++)
                {
                    VectorX1[i] = VectorX[i] - Tao * residual[i];
                }

                if (CheckSpeedyDescent(VectorX1, VectorX))
                    return Result(VectorX1);
                else if (Iter > Itermax)
                {
                    string s = $"iter ={Iter}; norm:{Norm} ;Vectors";
                    for (int i = 0; i < Size; i++)
                        s += VectorX1[i].ToString() + " ";
                    return s;
                }
                for (int i = 0; i < Size; i++)
                {
                    VectorX[i] = VectorX1[i];
                }
                k++;
            }
        }

        private string VectorToString(List<double> vector)
        {
            string s = "";
            for (int i = 0; i < Size; i++)
            {
                s += vector[i].ToString() + " ";
            }
            s += "\n";
            return s;
        }
    }
}
