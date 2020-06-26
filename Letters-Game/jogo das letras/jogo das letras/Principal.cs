using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jogo_das_letras
{
    public partial class Principal : Form
    {
        SQL ola = new SQL();
        bool resposta;
        public Principal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 oi = new Form1();
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Escolha uma Dificuldade antes de iniciar o jogo");
            }
            if (comboBox1.Text != "")
            {
                this.Hide();
                oi.dificuldade(comboBox1.Text, int.Parse(textBox3.Text),true);
                this.Close();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(button2.Text != "Cadastrar")
            {
                button2.Text = "Cadastrar";
                label4.Visible = textBox3.Visible = false;
            }
            else
            {
                button2.Text = "Logar";
                label4.Visible = textBox3.Visible = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                resposta = ola.cadastra(textBox1.Text, textBox2.Text);
                if(resposta)
                {
                    MessageBox.Show("Cadastrado com sucesso");
                }
                else
                {
                    MessageBox.Show("Erro ao tentar cadastrar");
                }
            }
            else
            {
                resposta = ola.loga(textBox1.Text, textBox2.Text, textBox3.Text);
                if(resposta)
                {
                    MessageBox.Show("Logado com sucesso");
                    button1.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Erro ao tentar logar");
                }
            }
            dataGridView1.DataSource = ola.nomes();
        }

        private void Principal_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = ola.nomes();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form1 oi = new Form1();
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Escolha uma Dificuldade antes de iniciar o jogo");
            }
            if (comboBox1.Text != "")
            {
                this.Hide();
                oi.dificuldade(comboBox1.Text, -1,false);
                this.Close();
            }
        }
    }
}
