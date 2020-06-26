using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace jogo_das_letras
{
    public partial class Form1 : Form
    {
        bool isLogged;
        Point a1 = new Point();
        Point a2 = new Point();
        Point a3 = new Point();
        Point a4 = new Point();
        Point a5 = new Point();
        Point inicio1 = new Point(12, 9);
        Point inicio2 = new Point(151, 9);
        Point inicio3 = new Point(318, 9);
        Point inicio4 = new Point(446, 9);
        Point inicio5 = new Point(599, 9);
        Palavras pa = new Palavras();
        SQL gg = new SQL();
        int ad, b, c, d, ef, pont, v1, v2, v3, v4, v5, value, id;
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 135)
            {
                tudo("ç");
            }
            else tudo(e.KeyChar.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
            //Process.Start("C:\\Users\\Edilene Peres Pc\\Desktop\\Eduardo FNE\\LP\\VisualStudio\\jogo das letras\\jogo das letras\\bin\\Debug\\jogo das letras.exe");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int la;
            la = pa.label();
            if (la == 1 && ad == 0)
            {
                a1.X = label1.Location.X;
                a1.Y = label1.Location.Y;
                label1.Text = pa.palavra();
                ad++;
            }
            if (la == 2 && b == 0)
            {
                a2.X = label2.Location.X;
                a2.Y = label2.Location.Y;
                label2.Text = pa.palavra();
                b++;
            }
            if (la == 3 && c == 0)
            {
                a3.X = label3.Location.X;
                a3.Y = label3.Location.Y;
                label3.Text = pa.palavra();
                c++;
            }
            if (la == 4 && d == 0)
            {
                a4.X = label4.Location.X;
                a4.Y = label4.Location.Y;
                label4.Text = pa.palavra();
                d++;
            }
            if (la == 5 && ef == 0)
            {
                a5.X = label5.Location.X;
                a5.Y = label5.Location.Y;
                label5.Text = pa.palavra();
                ef++;
            }
            /////////////////////////////////////////////////////////////////////
            if (ad != 0)
            {
                a1.Y = a1.Y + v1;
                label1.Location = a1;
            }
            if (b != 0)
            {
                a2.Y = a2.Y + v2;
                label2.Location = a2;
            }
            if (c != 0)
            {
                a3.Y = a3.Y + v3;
                label3.Location = a3;
            }
            if (d != 0)
            {
                a4.Y = a4.Y + v4;
                label4.Location = a4;
            }
            if (ef != 0)
            {
                a5.Y = a5.Y + v5;
                label5.Location = a5;
            }
            /////////////////////////////////////////////////////////////////////////////////////////
            if(label1.Location.Y > progressBar1.Location.Y-23)
            {
                progressBar1.Increment(-1);
                label1.Location = inicio1;
                ad = 0;
            }
            if (label2.Location.Y > progressBar1.Location.Y - 23)
            {
                progressBar1.Increment(-1);
                label2.Location = inicio2;
                b = 0;
            }
            if (label3.Location.Y > progressBar1.Location.Y - 23)
            {
                progressBar1.Increment(-1);
                label3.Location = inicio3;
                c = 0;
            }
            if (label4.Location.Y > progressBar1.Location.Y - 23)
            {
                progressBar1.Increment(-1);
                label4.Location = inicio4;
                d = 0;
            }
            if (label5.Location.Y > progressBar1.Location.Y - 23)
            {
                progressBar1.Increment(-1);
                label5.Location = inicio5;
                ef = 0;
            }
            ///////////////////////////////////////////////
            label7.Text = pont.ToString();
            ///////////////////////////////////////////////
            if(progressBar1.Value == 100)
            {
                timer1.Stop();
                MessageBox.Show("PARABÉNS, VOCÊ GANHOU, SUA PONTUAÇÃO FOI " + pont);
                button2.Visible = true;
                if (isLogged)
                {
                    gg.salvapont(pont.ToString(), id);
                    gg.salvapontmax(pont, id);
                }
                
            }
            if(progressBar1.Value == 0)
            {
                timer1.Stop();
                MessageBox.Show("Você Perdeu, sua pontuação foi " + pont);
                button2.Visible = true;
                if (isLogged)
                {
                    gg.salvapont(pont.ToString(), id);
                    gg.salvapontmax(pont, id);
                }
            }
            label8.Text = "Vida: " + progressBar1.Value;
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void timer2_Tick(object sender, EventArgs e)
        {

        }

        public void tudo(string asd)
        {
            if (label1.Text == asd)
            {
                textBox1.Text = textBox1.Text + asd;
                pont = pont + value;
                progressBar1.Increment(1);
                label1.Location = inicio1;
                ad = 0;
            }
            else if (label2.Text == asd)
            {
                textBox1.Text = textBox1.Text + asd;
                pont = pont + value;
                progressBar1.Increment(1);
                label2.Location = inicio2;
                b = 0;
            }
            else if (label3.Text == asd)
            {
                textBox1.Text = textBox1.Text + asd;
                pont = pont + value;
                progressBar1.Increment(1);
                label3.Location = inicio3;
                c = 0;
            }
            else if (label4.Text == asd)
            {
                textBox1.Text = textBox1.Text + asd;
                pont = pont + value;
                progressBar1.Increment(1);
                label4.Location = inicio4;
                d = 0;
            }
            else if (label5.Text == asd)
            {
                textBox1.Text = textBox1.Text + asd;
                pont = pont + value;
                progressBar1.Increment(1);
                label5.Location = inicio5;
                ef = 0;
            }
            else
            {
                progressBar1.Increment(-1);
            }
        }

        public void dificuldade(string dif, int a, bool isLogged)
        {
            this.isLogged = isLogged;
            if(dif == "Fácil")
            {
                progressBar1.Increment(50);
                v1 = 10; v2 = 9; v3 = 7; v4 = 5; v5 = 12;
                value = 2;
            }
            if(dif == "Médio")
            {
                progressBar1.Increment(30);
                v1 = 12; v2 = 11; v3 = 9; v4 = 7; v5 = 14;
                value = 4;
            }
            if(dif == "Difícil")
            {
                progressBar1.Increment(10);
                v1 = 15; v2 = 14; v3 = 12; v4 = 10; v5 = 17;
                value = 7;
            }
            id = a;
            this.ShowDialog();
        }
    }
}