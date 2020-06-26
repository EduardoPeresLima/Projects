using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data;
using MySql.Data.MySqlClient;

namespace jogo_das_letras
{
    class SQL
    {
        string end = "SERVER=localhost;DATABASE=jogos;UID=root;PASSWORD=;";
        MySqlConnection conecta = new MySqlConnection();
        MySqlDataAdapter adap = new MySqlDataAdapter();

        public bool cadastra(string nome, string senha)
        {
            try
            {
                string cad = "INSERT INTO letras(nome, senha) VALUES ('" + nome + "','" + senha + "')";
                conecta = new MySqlConnection(end);
                conecta.Open();
                adap = new MySqlDataAdapter(cad, conecta);
                adap.InsertCommand = conecta.CreateCommand();
                adap.InsertCommand.CommandText = cad;
                adap.InsertCommand.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                conecta.Close();
            }
        }

        public bool loga(string nome, string senha, string id)
        {
            try
            {
                conecta = new MySqlConnection(end);
                conecta.Open();
                string[] asd = new string[2];
                string log = "SELECT nome FROM letras WHERE id = '" + id + "'";
                adap = new MySqlDataAdapter(log, conecta);
                adap.SelectCommand = conecta.CreateCommand();
                adap.SelectCommand.CommandText = log;
                adap.SelectCommand.ExecuteNonQuery();
                asd[0] = adap.SelectCommand.ExecuteScalar().ToString();
                conecta.Close();
                conecta.Open();
                string sm = "SELECT senha FROM letras WHERE id = '" + id + "'";
                adap = new MySqlDataAdapter(sm, conecta);
                adap.SelectCommand = conecta.CreateCommand();
                adap.SelectCommand.CommandText = sm;
                adap.SelectCommand.ExecuteNonQuery();
                asd[1] = adap.SelectCommand.ExecuteScalar().ToString();
                conecta.Close();

                if (nome == asd[0] && senha == asd[1])
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public DataTable nomes()
        {
            DataTable oi = new DataTable();
            try
            {
                conecta = new MySqlConnection(end);
                conecta.Open();
                string seleciona = "SELECT id AS ID, nome AS Nome FROM letras";
                adap = new MySqlDataAdapter(seleciona, conecta);
                adap.SelectCommand = conecta.CreateCommand();
                adap.SelectCommand.CommandText = seleciona;
                adap.SelectCommand.ExecuteNonQuery();
                adap.Fill(oi);
                conecta.Close();
                return oi;
            }
            catch
            {
                return oi;
            }
        }

        public void salvapont(string pont, int id)
        {
            conecta = new MySqlConnection(end);
            conecta.Open();
            string save = "UPDATE letras SET pontuaçãototal=pontuaçãototal+'" + pont + "' WHERE id = '" + id + "'";
            adap = new MySqlDataAdapter(save, conecta);
            adap.SelectCommand = conecta.CreateCommand();
            adap.SelectCommand.CommandText = save;
            adap.SelectCommand.ExecuteNonQuery();
            conecta.Close();
        }

        public void salvapontmax(int pontmax, int id)
        {
            conecta = new MySqlConnection(end);
            conecta.Open();
            string save = "SELECT pontuaçãomaxima FROM letras WHERE id = '" + id + "'";
            adap = new MySqlDataAdapter(save, conecta);
            adap.SelectCommand = conecta.CreateCommand();
            adap.SelectCommand.CommandText = save;
            adap.SelectCommand.ExecuteNonQuery();
            int oi = int.Parse(adap.SelectCommand.ExecuteScalar().ToString());
            conecta.Close();
            if (pontmax > oi)
            {
                conecta.Open();
                string save2 = "UPDATE letras SET pontuaçãomaxima=pontuaçãomaxima+'" + pontmax + "' WHERE id = '" + id + "'";
                adap = new MySqlDataAdapter(save, conecta);
                adap.SelectCommand = conecta.CreateCommand();
                adap.SelectCommand.CommandText = save2;
                adap.SelectCommand.ExecuteNonQuery();
                conecta.Close();
            }
        }
    }
}
