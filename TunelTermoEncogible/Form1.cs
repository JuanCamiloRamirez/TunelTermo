using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TunelTermoEncogible
{
    public partial class Form1 : Form
    {
        bool encendido = false;
        const int desp = 15;
        int dir = 1;
        int time = 0;
        double voltaje = 3;
        double anterior = 21;
        double actual = 0;
        //double[] num = new double[] { 1.1 * 0.008594, 1.1 * 0.008548 };
        //double[] den = new double[] { 1.984, -0.9841 };
        //double[] u = new double[] { 1, 1, 1, 1 };
        //double[] referencia = new double[] { 0, 0, 0, 1 };
        //double[] salidaSis = new double[] { 0, 0, 0, 0 };
        List<double> num = new List<double>();
        List<double> den = new List<double>();
        List<double> u = new List<double>();
        List<double> referencia = new List<double>();
        List<double> salidaSis = new List<double>();        
        
        int ii = 3;
        double ts = 1;

        private void inicializarValores()
        {
            num.Insert(0, 1.1 * 0.008594); num.Insert(1, 1.1 * 0.008548);
            u.Insert(0, 1); u.Insert(1, 1); u.Insert(2, 1); u.Insert(3, 1);
            den.Insert(0, 1.984); den.Insert(1, -0.9841);
            referencia.Insert(0, 0); referencia.Insert(1, 0); referencia.Insert(1, 0); referencia.Insert(ii, 1);
            salidaSis.Insert(0, 0); salidaSis.Insert(1, 0); salidaSis.Insert(2, 0); salidaSis.Insert(3, 0);
        }
        

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            inicializarValores();
            voltaje = 3;
            encendido = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            btnStart.Visible = false;
            btnStop.Visible = true;
            timer1.Enabled = true;
            timer2.Enabled = true;
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            encendido = true;
            btnStart.Visible = true;
            voltaje = 0;
            voltajePantalla.Text = "0";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mover();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            actual = transferencia(time) + 81.5;
            //actual = (double.Parse(voltajePantalla.Text) * double.Parse(muestreo.Text)) + anterior;
            Temperatura.Series["Temperatura"].Points.AddXY(time, actual);
            //anterior = actual;
            time++;
            //time+= double.Parse(muestreo.Text);
            //textReferencia.Text = actual.ToString();
            escribirBD(time, actual);            
        }

        private void escribirBD(double time, double actual) {
            string path = @"E:\tempBD.txt";
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine("tiempo;temperatura");
                    sw.WriteLine(time + ";" + actual);

                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(time + ";" + actual);
                }
            }

        }
        private void mover()
        {
            if (dir < 4000)
            {
                pictureBox1.Left -= desp;
                pictureBox2.Left -= desp;
                dir += dir;
            }
            else if (dir >= 4000)
            {
                pictureBox1.Left += 180;
                pictureBox2.Left += 180;
                dir = 1;
            }            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            voltaje = 0;
        }

        private double transferencia(int i)
        {
            if (i>=3)
            {
                referencia.Insert(i,1);
                double parte1 = (num[0] * u[i - 1]);
                double parte2 = (num[1] * u[i - 2]);
                double parte3 = (den[0] * salidaSis[i - 1]);
                double parte4 = (den[1] * salidaSis[i - 2]);
                double suma = parte1 + parte2 + parte3 + parte4;
                salidaSis.Insert(i, suma);                
                i = i + 1;
                u.Insert(i,1);
                return suma;
            }
            else
            {
                return salidaSis[i];
            }
        }            
    }
}
