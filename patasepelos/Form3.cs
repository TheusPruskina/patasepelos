using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace patasepelos
{
    public partial class frmProduto : Form
    {
        public frmProduto()
        {
            InitializeComponent();
            CarregarProduto();
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
            WebClient ftpProduto = new WebClient();
            ftpProduto.Credentials = new NetworkCredential(Variaveis.usuarioFtp, Variaveis.senhaFtp);
            try
            {
                byte[] imageToByte = ftpProduto.DownloadData(caminhoArquivoFtp);
                return imageToByte;
            }
            catch
            {
                byte[] imageToByte = ftpProduto.DownloadData("ftp://127.0.0.1/admin/img/Produto/semimagem.png");
                return imageToByte;
            }
        }


        //INICIO DOS METODOS 

        private void CarregarProdutoNome()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_produto where nomeProduto LIKE '%" + txtProduto.Text + "%' order by nomeProduto asc;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvProduto.DataSource = dt;//coloca a tabela na datagridview
                dgvProduto.Columns[0].Visible = false;
                dgvProduto.Columns[1].HeaderText = "NOME";
                dgvProduto.Columns[2].HeaderText = "MARCA";
                dgvProduto.Columns[3].HeaderText = "VALOR";
                dgvProduto.Columns[4].HeaderText = "VALIDADE";
                dgvProduto.Columns[5].HeaderText = "QUANTIDADE";
                dgvProduto.Columns[6].HeaderText = "CODIGO DE BARRAS";
                dgvProduto.Columns[7].HeaderText = "STATUS";

                dgvProduto.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o PRODUTO por NOME \n\n" + erro);
            }
        }

        private void ExcluirProduto()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_produto set statusProduto = 'DESATIVADO' where idProduto = @id;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@id", Variaveis.idProduto);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produto desativado com sucesso", "EXCLUIR PRODUTO");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao excluir PRODUTO. \n\n" + erro);
            }
        }

        private void CarregarProduto()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_produto where statusProduto <> 'DESATIVADO' order by nomeProduto;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvProduto.DataSource = dt;//coloca a tabela na datagridview
                dgvProduto.Columns[0].Visible = false;
                dgvProduto.Columns[1].HeaderText = "NOME";
                dgvProduto.Columns[2].HeaderText = "MARCA";
                dgvProduto.Columns[3].HeaderText = "VALOR";
                dgvProduto.Columns[4].HeaderText = "VALIDADE";
                dgvProduto.Columns[5].HeaderText = "QUANTIDADE";
                dgvProduto.Columns[6].HeaderText = "CODIGO DE BARRAS";
                dgvProduto.Columns[7].HeaderText = "STATUS";


                dgvProduto.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o PRODUTO \n\n" + erro);
            }
        }

        private void CarregarProdutoStatus()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_produto where statusProduto = @status order by nomeProduto;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvProduto.DataSource = dt;//coloca a tabela na datagridview
                dgvProduto.Columns[0].Visible = false;
                dgvProduto.Columns[1].HeaderText = "NOME";
                dgvProduto.Columns[2].HeaderText = "MARCA";
                dgvProduto.Columns[3].HeaderText = "VALOR";
                dgvProduto.Columns[4].HeaderText = "VALIDADE";
                dgvProduto.Columns[5].HeaderText = "QUANTIDADE";
                dgvProduto.Columns[6].HeaderText = "CODIGO DE BARRAS";
                dgvProduto.Columns[7].HeaderText = "STATUS";

                dgvProduto.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o PRODUTO por STATUS \n\n" + erro);
            }
        }
        //FIM DOS METODOS   


        private void btnFechar_Click(object sender, EventArgs e)
        {
            new frmMenu().Show();
            Close();
        }

        private void frmProduto_Load(object sender, EventArgs e)
        {
            dgvProduto.ClearSelection();
        }



        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                var resposta = MessageBox.Show("Deseja mesmo exluir o produto?", "EXCLUIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    var resposta2 = MessageBox.Show("Tem certeza? Essa ação não poderá ser desfeita", "CONFIRMAÇÃO", MessageBoxButtons.YesNo);
                    if (resposta2 == DialogResult.Yes)
                    {
                        ExcluirProduto();
                        CarregarProduto();  
                    }
                }
            }
            else
            {
                MessageBox.Show("Para alterar selecione um produto da lista");
            }
        }

        private void dgvProduto_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Variaveis.linhaSelecionada = int.Parse(e.RowIndex.ToString());
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.idProduto = Convert.ToInt32(dgvProduto[0, Variaveis.linhaSelecionada].Value);
            }
        }

        private void dgvProduto_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvProduto.Sort(dgvProduto.Columns[1], ListSortDirection.Ascending);
            dgvProduto.ClearSelection();
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.Text == "TODOS")
            {
                CarregarProduto();
            }
            else
            {
                CarregarProdutoStatus();
            }
        }

        private void txtProduto_TextChanged(object sender, EventArgs e)
        {
            if (txtProduto.Text == "")
            {
                cmbStatus.Enabled = true;
                cmbStatus.Text = "TODOS";
            }
            else
            {
                cmbStatus.Enabled = false;
                CarregarProdutoNome();
            }
        }

        private void txtProduto_TextChanged_1(object sender, EventArgs e)
        {
            if (txtProduto.Text == "")
            {
                cmbStatus.Enabled = true;
                cmbStatus.Text = "TODOS";
            }
            else
            {
                cmbStatus.Enabled = false;
                CarregarProdutoNome();
            }
        }

        private void txtProduto_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtProduto.Text == "")
                {
                    cmbStatus.Enabled = true;
                    cmbStatus.Text = "TODOS";
                    CarregarProduto();
                }
                else
                {
                    cmbStatus.Enabled = false;
                    CarregarProdutoNome();
                }
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.funcao = "Alterar";
                new frmProCadastro().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Para alterar selecione um produto da lista");
            }
        }

        private void lblCadastrar_Click(object sender, EventArgs e)
        {
            Variaveis.funcao = "Cadastrar";
            new frmProCadastro().Show();
            Hide();
        }

        private void btnFechar_Click_1(object sender, EventArgs e)
        {
            new frmMenu().Show();
            Close();
        }
    }
}
