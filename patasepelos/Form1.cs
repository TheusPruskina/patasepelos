using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace patasepelos
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        //INICIO DOS METODOS

        private void login()
        {
            try
            {
                banco.Conectar();
                string selecionar = "select nomeFuncionario, emailFuncionario, senhaFuncionario, especialidadeFuncionario from tbl_funcionario where emailFuncionario = @email and senhaFuncionario = @senha and statusFuncionario = @status;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                cmd.Parameters.AddWithValue("@email", Variaveis.usuario);
                cmd.Parameters.AddWithValue("@senha", Variaveis.senha);
                cmd.Parameters.AddWithValue("@status", "ATIVO");
                MySqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    Variaveis.usuario = reader.GetString(0);
                    Variaveis.especialidade = reader.GetString(3);
                    new frmMenu().Show();
                    Hide();
                }
                else
                {
                    Variaveis.tentativa = Variaveis.tentativa + 1;
                    if (Variaveis.tentativa == 3)
                    {
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("ACESSO NEGADO");
                        txtEmail.Clear();
                        txtSenha.Clear();
                        txtEmail.Focus();
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Erro ao efetuar o login");
            }
        }

        //FIM DOS METODOS


        private void btnSair_Click(object sender, EventArgs e)
        {
            var resposta = MessageBox.Show("Deseja sair do sistema", "SAIR", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);

            if (resposta == DialogResult.Yes)

            {

                Application.Exit();

            }

            else if (resposta == DialogResult.No)

            {

                new frmLogin().Show();

                Close();

            }
        }


        private void lblLogin_Click(object sender, EventArgs e)
        {
            Variaveis.usuario = txtEmail.Text;
            Variaveis.senha = txtSenha.Text;

            if (Variaveis.usuario == "" && Variaveis.senha == "")
            {
                new frmMenu().Show();
                Hide();
            }
            else
            {
                login();
            }

        }
    }
}
