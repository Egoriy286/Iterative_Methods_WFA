using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JacobiWFA
{
    public class MinResidual : IterativeMethod
    {
        List<double> residual = new List<double>();
        public MinResidual() { }

        public MinResidual(List<List<double>> Matrix, List<double> VectorB, List<double> VectorX0, int Size)
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

        public double MultiVector(List<double> vector1,List<double> vector2)
        {
            double temp = 0.0;
            for (int i = 0; i<Size;i++)
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
            double Ar_r, Ar_Ar, Tao;
            for (int i = 0; i < Size; i++)
            {
                VectorX1.Add(0.0);
                VectorX.Add(VectorX0[i]);
                Ar.Add(0.0);
            }

            int k = 0;
            while (true)
            {
                residual = Difference(MultiMatrixVector(Matrix, VectorX), VectorB); // Невязка

                Ar = MultiMatrixVector(Matrix, residual);                           // A * Невязка
                
                Ar_r = MultiVector(Ar,residual);                                    // Скалярное произведение( (A * Невязка) * Невязка )

                Ar_Ar = MultiVector(Ar, Ar);                                        // Скалярное произведение( (A * Невязка) * (A * Невязка) )

                Tao = Ar_r / Ar_Ar;                                                 // Нахождение  Тао ( ( Невязка * Невязка ) / Скалярное произведение( (A * Невязка) * Невязка ) )

                //debug += $"residual{k}: " + VectorToString(residual);             
                //debug += $"Vector_Ar{k}: " + VectorToString(Ar);
                //debug += $"Ar{k}_r{k}: " + Ar_r.ToString()+"\n";
                //debug += $"Ar{k}_Ar{k}: " + Ar_Ar.ToString()+"\n";
                //debug += $"Tao{k + 1}: " + Tao.ToString()+"\n";
                //debug += $"VectorX{k + 1}: " + VectorToString(VectorX1);
                //debug += "\n\n";

                for (int i = 0; i < Size; i++)
                {
                    VectorX1[i] = VectorX[i] - Tao *residual[i];
                }

                if (CheckRasdial(VectorX1,VectorX))
                    return Result(VectorX1);
                else if (Iter > Itermax)
                {
                    return NoResult(VectorX1,VectorX);
                }
                for (int i = 0; i < Size; i++)
                {
                    VectorX[i] = VectorX1[i];
                }
                k++;
            }
        }

        public string NoResult(List<double> vector1, List<double> vector2)
        {
           
            string s = $"iter ={Iter}; norm:{Norm} ;Vectors\n";
            s += "VectorsX1: ";
            for (int i = 0; i < Size; i++)
                s += vector1[i].ToString() + " ";
            s += "\nVectorsX: ";
            for (int i = 0; i < Size; i++)
                s += vector2[i].ToString() + " ";

            s += $"\n Norma X1-X: {Math.Pow(NormMethodVector(Difference(vector1, vector2)), 2)}";

            return s;
        }

        private string VectorToString(List<double> vector)
        {
            string s = "";
            for(int i =0; i < Size;i++)
            {
                s += vector[i].ToString() + " ";
            }
            s += "\n";
            return s;
        }
    }
}
