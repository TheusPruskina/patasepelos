using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using Org.BouncyCastle.Asn1.X509;
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
    public partial class frmMenu : Form
    {
        public frmMenu()
        {
            InitializeComponent();
            CarregarDgvProduto();
            CarregarDgvServico();
        }


        //inicio dos metodos

 

        private void CarregarDgvServico()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionarS = "select * from tbl_servico where statusServico = 'ATIVO' order by nomeServico;";
                MySqlCommand cmdS = new MySqlCommand(selecionarS, banco.conexao);
                MySqlDataAdapter daS = new MySqlDataAdapter(cmdS);//adaptar ao c#
                DataTable dtS = new DataTable();//criando uma restrutura de tabela
                daS.Fill(dtS);//preencher a tabela (dt)

                dgvServico.DataSource = dtS;//coloca a tabela na datagridview
                dgvServico.Columns[0].Visible = false;
                dgvServico.Columns[1].HeaderText = "NOME";
                dgvServico.Columns[2].Visible = false;
                dgvServico.Columns[3].Visible = false;
                dgvServico.Columns[4].HeaderText = "DESCRICAO";
                dgvServico.Columns[5].HeaderText = "STATUS";

                dgvProduto.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar os SERVICO \n\n" + erro);
            }
        }

        private void CarregarDgvProduto()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionarP = "select * from tbl_produto where statusProduto = 'ATIVO' order by nomeProduto;";
                MySqlCommand cmdP = new MySqlCommand(selecionarP, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmdP);//adaptar ao c#
                DataTable dtP = new DataTable();//criando uma restrutura de tabela
                da.Fill(dtP);//preencher a tabela (dt)

                dgvProduto.DataSource = dtP;//coloca a tabela na datagridview
                dgvProduto.Columns[0].Visible = false;
                dgvProduto.Columns[1].HeaderText = "NOME";
                dgvProduto.Columns[2].HeaderText = "MARCA";
                dgvProduto.Columns[3].HeaderText = "VALOR";
                dgvProduto.Columns[4].Visible = false;
                dgvProduto.Columns[5].Visible = false;
                dgvProduto.Columns[6].Visible = false;
                dgvProduto.Columns[7].HeaderText = "STATUS";
                dgvProduto.Columns[8].Visible = false;

                dgvProduto.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar os PRODUTOS \n\n" + erro);
            }
        }

        private void CarregarProdutoNome()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionarP = "Select * from tbl_produto where nomeProduto LIKE '%" + txtProduto.Text + "%'order by nomeProduto;";
                MySqlCommand cmdP = new MySqlCommand(selecionarP, banco.conexao);
                MySqlDataAdapter daP = new MySqlDataAdapter(cmdP);//adaptar ao c#
                DataTable dtP = new DataTable();//criando uma restrutura de tabela
                daP.Fill(dtP);//preencher a tabela (dt)

                dgvProduto.DataSource = dtP;//coloca a tabela na datagridview
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
                MessageBox.Show("Erro ao carregar a agenda por cliente \n\n" + erro);
            }
        }

        private void CarregarServicoNome()
        {
            try
            {
                banco.Conectar(); //abrir o banco de dados
                string selecionarS = "Select * from tbl_servico where nomeServico LIKE '%" + txtServico.Text + "%'order by nomeServico;";
                MySqlCommand cmdS = new MySqlCommand(selecionarS, banco.conexao);
                MySqlDataAdapter daS = new MySqlDataAdapter(cmdS);//adaptar ao c#
                DataTable dtS = new DataTable();//criando uma restrutura de tabela
                daS.Fill(dtS);//preencher a tabela (dt)

                dgvServico.DataSource = dtS;//coloca a tabela na datagridview
                dgvServico.Columns[0].Visible = false;
                dgvServico.Columns[1].HeaderText = "NOME";
                dgvServico.Columns[2].Visible = false;
                dgvServico.Columns[3].Visible = false;
                dgvServico.Columns[4].HeaderText = "DESCRICAO";
                dgvServico.Columns[5].HeaderText = "STATUS";
                dgvServico.ClearSelection();//nao ficar nada selecionado
                banco.Desconectar();//fechar o banco de dados
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar a agenda por cliente \n\n" + erro);
            }
        }



        private void btnProduto_Click(object sender, EventArgs e)
        {
            new frmProduto().Show();
            Hide();
        }

        private void button1_Click(object sender, EventArgs e)
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

        private void btnFuncionario_Click(object sender, EventArgs e)
        {
            new frmFuncionario().Show();
            Hide();
        }

        private void btnServico_Click(object sender, EventArgs e)
        {
            new frmServico().Show();
            Hide();
        }


        private void lblHora_Click(object sender, EventArgs e)
        {
            lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void frmMenu_Load(object sender, EventArgs e)
        {
            lblUsuario.Text = Variaveis.usuario;
            lblEspecialidade.Text = Variaveis.especialidade;
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void txtProduto_TextChanged(object sender, EventArgs e)
        {
            if (txtProduto.Text == "")
            {
                CarregarDgvProduto();

            }
            else
            {
                CarregarProdutoNome();
            }
        }

        private void txtServico_TextChanged(object sender, EventArgs e)
        {
            if (txtServico.Text == "")
            {
                                                                                                                                                                                                    CarregarDgvServico();

            }
            else
            {
                CarregarServicoNome();
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

        private void dgvServico_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Variaveis.linhaSelecionada = int.Parse(e.RowIndex.ToString());
            if (Variaveis.linhaSelecionada >= 0)
            {
                Variaveis.idServico = Convert.ToInt32(dgvServico[0, Variaveis.linhaSelecionada].Value);
            }
        }

        private void dgvProduto_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvProduto.Sort(dgvProduto.Columns[1], ListSortDirection.Ascending);
            dgvProduto.ClearSelection();
        }

        private void dgvServico_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            dgvServico.Sort(dgvServico.Columns[1], ListSortDirection.Ascending);
            dgvServico.ClearSelection();
        }

    }
}
