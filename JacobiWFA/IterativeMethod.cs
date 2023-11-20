using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JacobiWFA
{
    public abstract class IterativeMethod 
    {
        public int Size { get; set; }
        public List<List<double>> Matrix { get; set; } = new List<List<double>>();
        public List<double> VectorX0 { get; set; } = new List<double>();
        public List<double> VectorB { get; set; } = new List<double>();
        public double Eps { get; set; } = 0.001;
        public int Itermax { get; set; } = 100000;
        public List<double> VectorX { get; set; }
        public double Norm { get; set; }
        protected int Iter { get; set; } = 0;
        public string debug { get; set; } = "Start\n";

        public abstract string Calculate();
       
        public string Print()
        {
            string text = "Matrix A:\n";
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    text += Matrix[i][j].ToString() + " ";
                }
                text += "\n";
            }
            text += "VectorX0:\n";
            for (int i = 0; i < Size; i++)
            {
                text += VectorX0[i].ToString() + " ";
            }
            text += "\n";
            text += "VectorB:\n";
            for (int i = 0; i < Size; i++)
            {
                text += VectorB[i].ToString() + " ";
            }
            text += "\n";
            return text;
        }

        public bool CheckSimpleIterative(List<double> VectorX1)
        {
            if (Math.Pow(NormMethod(VectorX1), 2) <= Eps)
                return true;
            else
                if (Iter > Itermax)
                {
                    return false;
                }
                else
                    Iter++;
            return false;
        }

        public List<double> Difference(List<double> Ax, List<double> B)
        {
            List<double> vector = new List<double>();
            for (int i = 0; i < Size; i++)
            {
                vector.Add(Ax[i] - B[i]);
            }
            return vector;
        }

        public List<double> MultiMatrixVector(List<List<double>> X, List<double> Y)
        {
            List<double> vector = new List<double>();
            for (int i = 0; i < Size; i++)
            {
                vector.Add(0.0);
                for (int j = 0; j < Size; j++)
                {
                    vector[i] += Y[j] * X[i][j];
                }
            }
            return vector;
        }

        public string Debug()
        {
            return debug;
        }

        public string Result(List<double> Vector)
        {
            string res = "Otvet:\n";
            for (int i = 0; i < Size; i++)
            {
                res += $"x{i+1}: " + Math.Round(Vector[i],7).ToString() + "\t";
            }
            res += "\n";
            res += $"iter: {Iter}\n\n";
            return res;
        }

        public double NormMethodVector(List<double> vector)
        {
            double temp = 0;
            for (int i = 0; i < Size; i++)
            {
                temp += vector[i]*vector[i];
            }
            return Math.Sqrt(temp);
        }

        public double NormMethod(List<double> VectorX1)
        {
            Norm = 0.0;
            double temp = 0;
            List<double> VectorMV = MultiMatrixVector(Matrix, VectorX1);
            VectorMV = Difference(VectorMV, VectorB);
            for (int i = 0; i < Size; i++)
            {
                temp += VectorMV[i] * VectorMV[i];
            }
            Norm = temp;
            return Math.Sqrt(temp);
        }

        public bool CheckRasdial(List<double> VectorX1,List<double> VectorX) 
        {
            if (Math.Pow(NormMethod(VectorX1)- NormMethod(VectorX), 2) < Eps)
                return true;
            else
                if (Iter > Itermax)
            {
                return false;
            }
            else
                Iter++;
            return false;
        }
        
        public bool CheckSpeedyDescent(List<double> VectorX1, List<double> VectorX)
        {
            if (Math.Abs(NormMethod(VectorX1)-NormMethod(VectorX)) < Eps)
                return true;
            else
                if (Iter > Itermax)
            {
                return false;
            }
            else
                Iter++;
            return false;
        }

        public List<List<double>> MultiMatrixMatrix(List<List<double>> X, List<List<double>> Y)
        {
            List<List<double>> tempMatrix = new List<List<double>>();
            for (int i = 0; i < Size; i++)
            {
                tempMatrix.Add(new List<double>());
                for (int j = 0; j < Size; j++)
                {
                    tempMatrix[i].Add(0.0);
                    for (int  k = 0; k < Size; k++)
                        tempMatrix[i][j]+=X[i][k]*Y[k][j];
                    tempMatrix[i][j] = 1 / tempMatrix[i][j];
                }
            }
            return tempMatrix;
        }
        public List<List<double>> Symmetrization()
        {
            List<List<double>>TransMatrix= new List<List<double>>();
            for (int i = 0;i<Size;i++)
            {
                TransMatrix.Add(new List<double>());
                for (int j = 0; j < Size; j++)
                {
                    TransMatrix[i].Add(Matrix[j][i]);
                }
            }
            return MultiMatrixMatrix(Matrix,TransMatrix);
        }
    }
}
