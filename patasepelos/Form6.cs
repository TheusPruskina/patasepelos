using MySql.Data.MySqlClient;
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
    public partial class frmServico : Form
    {
        public frmServico()
        {
            InitializeComponent();
            CarregarServico();
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
            WebClient ftpServico = new WebClient();
            ftpServico.Credentials = new NetworkCredential(Variaveis.usuarioFtp, Variaveis.senhaFtp);
            try
            {
                byte[] imageToByte = ftpServico.DownloadData(caminhoArquivoFtp);
                return imageToByte;
            }
            catch
            {
                byte[] imageToByte = ftpServico.DownloadData("ftp://127.0.0.1/admin/img/servico/semimagem.png");
                return imageToByte;
            }
        }

        //INICIO DOS METODOS 

        private void CarregarServicoNome()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_servico where nomeServico LIKE '%" + txtServico.Text + "%' order by nomeServico asc;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvServico.DataSource = dt;//coloca a tabela na datagridview
                dgvServico.Columns[0].Visible = false;
                dgvServico.Columns[1].HeaderText = "NOME";
                dgvServico.Columns[2].HeaderText = "FOTO";
                dgvServico.Columns[3].Visible = false;
                dgvServico.Columns[4].HeaderText = "DESCRICAO";
                dgvServico.Columns[5].HeaderText = "STATUS";

                dgvServico.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o Servico por NOME \n\n" + erro);
            }
        }

        private void ExcluirServico()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_servico set statusServico = 'DESATIVADO' where idServico = @id;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@id", Variaveis.idServico);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Servico desativado com sucesso", "EXCLUIR Servico");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao excluir Servico. \n\n" + erro);
            }
        }

        private void CarregarServico()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_servico where statusServico <> 'DESATIVADO' order by nomeServico;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvServico.DataSource = dt;//coloca a tabela na datagridview
                dgvServico.Columns[0].Visible = false;
                dgvServico.Columns[1].HeaderText = "NOME";
                dgvServico.Columns[2].HeaderText = "FOTO";
                dgvServico.Columns[3].Visible = false;
                dgvServico.Columns[4].HeaderText = "DESCRICAO";
                dgvServico.Columns[5].HeaderText = "STATUS";

                dgvServico.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o Servico \n\n" + erro);
            }
        }

        private void CarregarServicoStatus()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionar = "select * from tbl_servico where statusServico = @status order by nomeServico;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);//adaptar ao c#
                DataTable dt = new DataTable();//criando uma restrutura de tabela
                da.Fill(dt);//preencher a tabela (dt)

                dgvServico.DataSource = dt;//coloca a tabela na datagridview
                dgvServico.Columns[0].Visible = false;
                dgvServico.Columns[1].HeaderText = "NOME";
                dgvServico.Columns[2].HeaderText = "FOTO";
                dgvServico.Columns[3].Visible = false;
                dgvServico.Columns[4].HeaderText = "DESCRICAO";
                dgvServico.Columns[5].HeaderText = "STATUS";


                dgvServico.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao selecionar o Servico por STATUS \n\n" + erro);
            }
        }
        //FIM DOS METODOS 

        private void button1_Click(object sender, EventArgs e)
        {
            new frmMenu().Show();
            Close();
        }

        private void txtServico_TextChanged(object sender, EventArgs e)
        {
            if (txtServico.Text == "")
            {
                cmbStatus.Enabled = true;
                cmbStatus.Text = "TODOS";
            }
            else
            {
                cmbStatus.Enabled = false;
                CarregarServicoNome();
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStatus.Text == "TODOS")
            {
                CarregarServico();
            }
            else
            {
                CarregarServicoStatus();
            }
        }

        private void frmServico_Load(object sender, EventArgs e)
        {
            dgvServico.ClearSelection();
        }

        private void txtServico_TextChanged_1(object sender, EventArgs e)
        {
            if (txtServico.Text == "")
            {
                cmbStatus.Enabled = true;
                cmbStatus.Text = "TODOS";
            }
            else
            {
                cmbStatus.Enabled = false;
                CarregarServicoNome();
            }
        }

        private void txtServico_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                if (txtServico.Text == "")
                {
                    cmbStatus.Enabled = true;
                    cmbStatus.Text = "TODOS";
                    CarregarServico();
                }
                else
                {
                    cmbStatus.Enabled = false;
                    CarregarServicoNome();
                }
            }
        }

        private void dgvServico_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Variaveis.linhaSelecionada = int.Parse(e.RowIndex.ToString());
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.idServico = Convert.ToInt32(dgvServico[0, Variaveis.linhaSelecionada].Value);
            }
        }

        private void dgvServico_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvServico.Sort(dgvServico.Columns[1], ListSortDirection.Ascending);
            dgvServico.ClearSelection();
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                var resposta = MessageBox.Show("Deseja mesmo exluir o Servico?", "EXCLUIR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (resposta == DialogResult.Yes)
                {
                    var resposta2 = MessageBox.Show("Tem certeza? Essa ação não poderá ser desfeita", "CONFIRMAÇÃO", MessageBoxButtons.YesNo);
                    if (resposta2 == DialogResult.Yes)
                    {
                        ExcluirServico();
                        CarregarServico();
                    }
                }
            }
            else
            {
                MessageBox.Show("Para excluir selecione um Servico da lista");
            }
        }

        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.funcao = "Alterar";
                new frmCadServico().Show();
                Hide();
            }
            else
            {
                MessageBox.Show("Para alterar selecione um Servico da lista");
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            Variaveis.funcao = "Cadastrar";
            new frmCadServico().Show();
            Hide();
        }

        private void dgvServico_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
