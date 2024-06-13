using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1.Cmp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace patasepelos
{
    public partial class frmFuncionario : Form
    {
        public frmFuncionario()
        {
            InitializeComponent();
            CarregarFuncionario();
        }

        //metodos para fotos ftp

        /*Validação ftp*/
        private bool ValidarFTP()
        {
            if (string.IsNullOrEmpty(Variaveis.enderecoServidorFtp) || string.IsNullOrEmpty(Variaveis.usuarioFtp) || string.IsNullOrEmpty(Variaveis.senhaFtp))
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        //CONVERTER A IMAGEM EM bYTE

        public byte[] GetImgToByte(string caminhoArquivoFtp)
        {
            WebClient ftpFuncionario = new WebClient();
            ftpFuncionario.Credentials = new NetworkCredential(Variaveis.usuarioFtp, Variaveis.senhaFtp);
            try
            {
                byte[] imageToByte = ftpFuncionario.DownloadData(caminhoArquivoFtp);
                return imageToByte;
            }
            catch
            {
                byte[] imageToByte = ftpFuncionario.DownloadData("ftp://127.0.0.1/admin/img/Funcionario/semimagem.png");
                return imageToByte;
            }
        }

        //INICIO DOS METODOS 

        private void CarregarFuncionarioNome()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_funcionario where nomeFuncionario LIKE '%" + txtFuncionario.Text + "%' order by nomeFuncionario asc;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvFuncionario.DataSource = dt;//coloca a tabela na datagridview
                dgvFuncionario.Columns[0].Visible = false;
                dgvFuncionario.Columns[1].HeaderText = "NOME";
                dgvFuncionario.Columns[2].HeaderText = "ENDERECO";
                dgvFuncionario.Columns[3].HeaderText = "TELEFONE";
                dgvFuncionario.Columns[4].HeaderText = "EMAIL";
                dgvFuncionario.Columns[5].HeaderText = "SENHA";
                dgvFuncionario.Columns[6].HeaderText = "FOTO";
                dgvFuncionario.Columns[7].Visible = false;
                dgvFuncionario.Columns[8].HeaderText = "STATUS";
                dgvFuncionario.Columns[9].Visible = false;
                dgvFuncionario.Columns[10].HeaderText = "ESPECIALIDADE";
                dgvFuncionario.Columns[11].Visible = false;

                dgvFuncionario.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o FUNCIONARIO por NOME \n\n" + erro);
            }
        }

        private void ExcluirFuncionario()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_funcionario set statusFuncionario = 'DESATIVADO' where idFuncionario = @id;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@id", Variaveis.idFuncionario);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Funcionario desativado com sucesso", "EXCLUIR FUNCIONARIO");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao excluir FUNCIONARIO. \n\n" + erro);
            }
        }

        private void CarregarFuncionario()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_funcionario where statusFuncionario <> 'DESATIVADO' order by nomeFuncionario;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvFuncionario.DataSource = dt;//coloca a tabela na datagridview
                dgvFuncionario.Columns[0].Visible = false;
                dgvFuncionario.Columns[1].HeaderText = "NOME";
                dgvFuncionario.Columns[2].HeaderText = "ENDERECO";
                dgvFuncionario.Columns[3].HeaderText = "TELEFONE";
                dgvFuncionario.Columns[4].HeaderText = "EMAIL";
                dgvFuncionario.Columns[5].HeaderText = "SENHA";
                dgvFuncionario.Columns[6].HeaderText = "FOTO";
                dgvFuncionario.Columns[7].Visible = false;
                dgvFuncionario.Columns[8].HeaderText = "STATUS";
                dgvFuncionario.Columns[9].Visible = false;
                dgvFuncionario.Columns[10].HeaderText = "ESPECIALIDADE";
                dgvFuncionario.Columns[11].Visible = false;

                dgvFuncionario.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o FUNCIONARIO \n\n" + erro);
            }
        }

        private void CarregarFuncionarioStatus()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_funcionario where statusFuncionario = @status order by nomeFuncionario;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvFuncionario.DataSource = dt;//coloca a tabela na datagridview
                dgvFuncionario.Columns[0].Visible = false;
                dgvFuncionario.Columns[1].HeaderText = "NOME";
                dgvFuncionario.Columns[2].HeaderText = "MARCA";
                dgvFuncionario.Columns[3].HeaderText = "VALOR";
                dgvFuncionario.Columns[4].HeaderText = "VALIDADE";
                dgvFuncionario.Columns[5].HeaderText = "QUANTIDADE";
                dgvFuncionario.Columns[6].HeaderText = "CODIGO DE BARRAS";
                dgvFuncionario.Columns[7].HeaderText = "STATUS";
                dgvFuncionario.Columns[8].HeaderText = "STATUS";
                dgvFuncionario.Columns[9].Visible = false;
                dgvFuncionario.Columns[10].HeaderText = "ESPECIALIDADE";
                dgvFuncionario.Columns[11].Visible = false;


                dgvFuncionario.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o FUNCIONARIO por STATUS \n\n" + erro);
            }
        }
        //FIM DOS METODOS 

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            Variaveis.funcao = "CADASTRAR";
            new frmFunc().Show();
            Hide();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            new frmMenu().Show();
            Close();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.Text == "TODOS")
            {
                CarregarFuncionario();
            }
            else
            {
                CarregarFuncionarioStatus();
            }
        }

        private void frmFuncionario_Load(object sender, EventArgs e)
        {
            dgvFuncionario.ClearSelection();
        }

        private void txtFuncionario_TextChanged(object sender, EventArgs e)
        {
            if (txtFuncionario.Text == "")
            {
                cmbStatus.Enabled = true;
                cmbStatus.Text = "TODOS";
            }
            else
            {
                cmbStatus.Enabled = false;
                CarregarFuncionarioNome();
            }
        }

        private void txtFuncionario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtFuncionario.Text == "")
                {
                    cmbStatus.Enabled = true;
                    cmbStatus.Text = "TODOS";
                    CarregarFuncionario();
                }
                else
                {
                    cmbStatus.Enabled = false;
                    CarregarFuncionarioNome();
                }
            }
        }

        private void dgvFuncionario_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Variaveis.linhaSelecionada = int.Parse(e.RowIndex.ToString());
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.idFuncionario = Convert.ToInt32(dgvFuncionario[0, Variaveis.linhaSelecionada].Value);
            }
        }

        private void dgvFuncionario_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvFuncionario.Sort(dgvFuncionario.Columns[1], ListSortDirection.Ascending);
            dgvFuncionario.ClearSelection();
        }

        private void btnExluir_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                var resposta = MessageBox.Show("Deseja mesmo exluir o funcionario?", "EXCLUIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    var resposta2 = MessageBox.Show("Tem certeza? Essa ação não poderá ser desfeita", "CONFIRMAÇÃO", MessageBoxButtons.YesNo);
                    if (resposta2 == DialogResult.Yes)
                    {
                        ExcluirFuncionario();
                        CarregarFuncionario();
                    }
                }
            }
            else
            {
                MessageBox.Show("Para excluir selecione um funcionario da lista");
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.funcao = "Alterar";
                new frmFuncionario().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Para alterar selecione um funcionario da lista");
            }
        }
    }
    
}
