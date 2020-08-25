using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        int voltaje = 3;
        int anterior = 21;
        int actual = 0;


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
            encendido = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
            btnStart.Visible = false;
            btnStop.Visible = true;
            timer1.Enabled = true;
            timer2.Enabled = true;

            ;

        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            encendido = true;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            btnStop.Visible = false;
            btnStart.Visible = true;
            timer1.Enabled = false;
            timer2.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            mover();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            actual = (voltaje * 1) + anterior;
            Temperatura.Series["Temperatura"].Points.AddXY(time, actual);
            anterior = actual;
            time++;
            textReferencia.Text = actual.ToString();
            escribirBD(time, actual);
        }

        private void escribirBD(int time, int actual) {
            string path = @"d:\temp\tempBD.txt";
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
    }
}
