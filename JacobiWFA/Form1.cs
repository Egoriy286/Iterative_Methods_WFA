using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace JacobiWFA
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 1; 
        }

        private List<List<double>> InputMatrix(int Size)
        {
            List<List<double>> Matrix = new List<List<double>>();
            int k = 0;
            double a = 0;
            for (int i = 0; i < Size; i++)
            {
                Matrix.Add(new List<double>());
                for (int j = 0; j < Size; j++)
                {
                    if (!double.TryParse(groupBox1.Controls[k].Text, out a))
                    {
                        groupBox1.Controls[k].Text = "0";
                        groupBox1.Controls[k].Focus();
                        MessageBox.Show($"Ячейка[{i},{j}] пуст пожалуйста заполните");
                    }
                    Matrix[i].Add(a);
                    k++;
                }
                k++;
            }
            return Matrix;
        }

        private List<double> InputVector(int Size)
        {
            List<double> Vector = new List<double>();
            int k = 0;
            double a = 0;
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    k++;
                }
                if (!double.TryParse(groupBox1.Controls[k].Text, out a))
                {
                    groupBox1.Controls[k].Text = "0";
                    groupBox1.Controls[k].Focus();
                    MessageBox.Show($"Ячейка[{i},{Size}] пуст пожалуйста заполните");
                }
                Vector.Add(a);
                k++;
            }
            return Vector;
        }

        private void Iterative(IterativeMethod iterativeMethod)
        {
            if (textBox2.Text != "")
            {
                iterativeMethod.Itermax = int.Parse(textBox2.Text);
            }
            if (textBox3.Text != "")
            {
                iterativeMethod.Eps = double.Parse(textBox3.Text);
            }
            if (checkBox3.Checked)
            {
                printMatrix(iterativeMethod.Symmetrization(), iterativeMethod.Size);
                iterativeMethod.Matrix = new List<List<double>>(iterativeMethod.Symmetrization());
            }
            richTextBox1.Text = iterativeMethod.Print();
            resultBox.Text += iterativeMethod.Calculate();
            richTextBox1.Text += iterativeMethod.Debug();
            
        }
        private void printMatrix(List<List<double>> Matrix,int Size)
        {
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    richTextBox1.Text += Matrix[i][j].ToString() + " ";
                }
                richTextBox1.Text += "\n";
                
            }
        }

        private void JacobiMethod()
        {
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            List<List<double>> Matrix= InputMatrix(n); List<double> VectorB=InputVector(n); List<double> VectorX0 = new List<double>();
            for (int i = 0; i < n; i++)
                VectorX0.Add(VectorB[i] / Matrix[i][i]);
            IterativeMethod iterativeMethod = new Jacobi(Matrix, VectorB, VectorX0, n);
            Iterative(iterativeMethod);
        }

        private void ZaydelMethod()
        {
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            List<List<double>> Matrix = InputMatrix(n); List<double> VectorB = InputVector(n); List<double> VectorX0 = new List<double>();
            for (int i = 0; i < n; i++)
                VectorX0.Add(VectorB[i] / Matrix[i][i]);
            IterativeMethod iterativeMethod = new Zaydel(Matrix, VectorB, VectorX0, n);
            Iterative(iterativeMethod);
        }

        private void MinRasidual()
        {
            Random rand = new Random();
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            List<List<double>> Matrix = InputMatrix(n); List<double> VectorB = InputVector(n); List<double> VectorX0 = new List<double>();
            
            for(int i = 0; i < n;i++)
            {
                VectorX0.Add(rand.NextDouble());
            }
            IterativeMethod iterativeMethod = new MinResidual(Matrix, VectorB, VectorX0, n);
            Iterative(iterativeMethod);
        }

        private void SpeedyDescent()
        {
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            List<List<double>> Matrix = InputMatrix(n); List<double> VectorB = InputVector(n); List<double> VectorX0 = new List<double>();

            for (int i = 0; i < n; i++)
            {
                VectorX0.Add(0.0);
            }
            VectorX0[0] = 1;
            VectorX0[1] = 1;
            VectorX0[2] = 1;
            IterativeMethod iterativeMethod = new SpeedyDescent(Matrix, VectorB, VectorX0, n);
            Iterative(iterativeMethod);
        }

        private void Jacobi_Click(object sender, EventArgs e)
        {
            JacobiMethod();
        }

        private void Zaydel_Click(object sender, EventArgs e)
        {
            ZaydelMethod();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            groupBox1.Controls.Clear();
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            for (int i=0; i< n; i++) 
            {
                for (int j = 0; j < n + 1 ; j++)
                {
                    System.Windows.Forms.TextBox textBox = new System.Windows.Forms.TextBox();
                    textBox.Size = new Size(30, 20);
                    textBox.Location = new Point(30 + 35 * j, 25 * i + 20);
                    groupBox1.Controls.Add(textBox);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int n = Convert.ToInt32(comboBox1.SelectedItem);
            int k = 0;
            List<List<double>> Matrix  = new List<List<double>>();
            Matrix.Add(new List<double>() { 63, 47, 4 });
            Matrix.Add(new List<double>() { 47, 95, 9 });
            Matrix.Add(new List<double>() { 4, 9, 36 });
            List<double> VectorB = new List<double>() { 36, 19, 31 };
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    groupBox1.Controls[k].Text = Matrix[i][j].ToString();
                    k++;
                }
                groupBox1.Controls[k].Text = VectorB[i].ToString();
                k++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MinRasidual();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SpeedyDescent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            button1_Click(sender, e);
            textBox2.Text = "100000";
            textBox3.Text = "0.0000001";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            resultBox.Text = "";
        }
    }
}
