using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace patasepelos
{
    public static class banco
    {
        //strin conexão com banco de dados
        //para banco de dados local (xampp)
        //public static string db = "SERVER=localhost;USER=root;DATABASE=dbautomestre";

        //para banco de dados online (servidor web)
        public static string db = "SERVER=smpsistema.com.br;USER=u283879542_pethouse;PASSWORD=Senac@pethouse01;DATABASE=u283879542_pethouse;SSL MODE=None";

        public static MySqlConnection conexao;

        //metodo para abrir a conexão

        public static void Conectar()
        {
            try
            {

                conexao = new MySqlConnection(db);
                conexao.Open();
            }
            catch
            {
                MessageBox.Show("Erro ao conectar com o banco de dados");
            }
        }
        //metodo para fechar a conexao
        public static void Desconectar()
        {
            try
            {
                conexao = new MySqlConnection(db);
                conexao.Close();
            }
            catch
            {
                MessageBox.Show("Erro ao desconectar com o banco de dados");
            }
        }
    }
}