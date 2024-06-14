using MySql.Data.MySqlClient;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace patasepelos
{
    public partial class frmProCadastro : Form
    {
        public frmProCadastro()
        {
            InitializeComponent();
            CarregarCmbMarca();
            CarregarCmbProduto();

        }

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
                byte[] imageToByte = ftpProduto.DownloadData("ftp://127.0.0.1/admin/img/produtos/semimagem.png");
                return imageToByte;
            }
        }


        //converter a imagem de byte para imagem
        public static Bitmap ByteToImage(byte[] blob)
        {
            MemoryStream mStream = new MemoryStream();
            byte[] pData = blob;
            mStream.Write(pData, 0, Convert.ToInt32(pData.Length));
            Bitmap bm = new Bitmap(mStream, false);
            mStream.Dispose();
            return bm;
        }

        //INICIO DOS METODOSS


        private void CarregarDadosProduto()
        {
            try
            {
                banco.Conectar();
                string selecionar = "SELECT tbl_produto.nomeProduto, tbl_marca.nomeMarca, tbl_produto.valorProduto, tbl_produto.dataValProduto, tbl_produto.qtdeProduto, tbl_produto.barrasProduto, tbl_produto.statusProduto, tbl_produto.fotoProduto FROM tbl_produto  INNER JOIN tbl_marca ON tbl_produto.codMarca = tbl_marca.codMarca  WHERE tbl_produto.idProduto = @codigo;";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                cmd.Parameters.AddWithValue("@codigo", Variaveis.idProduto);
                MySqlDataReader dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    Variaveis.nomeProduto = dr.GetString(0);
                    Variaveis.nomeMarca = dr.GetString(1);
                    Variaveis.valorProduto = dr.GetDouble(2);
                    Variaveis.dataValProduto = dr.GetDateTime(3);
                    Variaveis.qtdeProduto = dr.GetDouble(4);
                    Variaveis.barrasProduto = dr.GetString(5);
                    Variaveis.statusProduto = dr.GetString(6);
                    Variaveis.fotoProduto = dr.GetString(7);
                    Variaveis.fotoProduto = Variaveis.fotoProduto.Remove(0, 8);

                    txtNome.Text = Variaveis.nomeProduto;
                    cmbMarca.Text = Variaveis.nomeMarca;
                    txtValor.Text = Variaveis.valorProduto.ToString("N2");
                    mtbData.Text = Variaveis.dataValProduto.ToShortDateString();
                    txtQuantidade.Text = Variaveis.qtdeProduto.ToString();
                    mtbCodBarras.Text = Variaveis.barrasProduto.ToString();
                    cmbStatus.Text = Variaveis.statusProduto;

                    pctFoto.Image = ByteToImage(GetImgToByte(Variaveis.enderecoServidorFtp + "img/produtos" + Variaveis.fotoProduto)); 
                    cmbStatus.Text = Variaveis.statusProduto;
                }
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar os dados dos Produtos. \n\n" + erro);
            }
        }

        private void AlterarProduto()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_produto set nomeproduto = @nome, codMarca = @marca, valorProduto = @valor, dataValProduto = @dataval, qtdeProduto = @qtde,barrasProduto = @barras, statusProduto = @status, fotoProduto = @foto WHERE tbl_produto.idProduto = @codigo;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                cmd.Parameters.AddWithValue("@codigo", Variaveis.idProduto);
                //parametros
                cmd.Parameters.AddWithValue("@nome",          Variaveis.nomeProduto);
                cmd.Parameters.AddWithValue("@marca",      Variaveis.codMarca);
                cmd.Parameters.AddWithValue("@valor",      Variaveis.valorProduto);
                cmd.Parameters.AddWithValue("@dataval",         Variaveis.dataValProduto);
                cmd.Parameters.AddWithValue("@qtde",         Variaveis.qtdeProduto);
                cmd.Parameters.AddWithValue("@barras",          Variaveis.barrasProduto);
                cmd.Parameters.AddWithValue("@status",        Variaveis.statusProduto);
                cmd.Parameters.AddWithValue("@foto",          Variaveis.fotoProduto);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produto alterado com sucesso", "Alterar produto");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao alterar produto. \n\n" + erro);
            }
        }

        private void AlterarFotoProduto()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_produto set fotoProduto = @foto where idProduto = @codigo;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoProduto);
                cmd.Parameters.AddWithValue("@codigo", Variaveis.idProduto);
                //fim parametros
                cmd.ExecuteNonQuery();
                banco.Desconectar();
                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoProduto))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/produtos/" + Path.GetFileName(Variaveis.fotoProduto);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoProduto, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
                        }
                        catch
                        {
                            MessageBox.Show("Foto não foi selecionada ou existente no servidor.", "FOTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }

            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao alterar FOTO PRODUTO. \n\n" + erro);
            }
        }

        private void CarregarCmbProduto()
        {
            try
            {
                banco.Conectar();
                string selecionar = "SELECT codMarca, nomeMarca  FROM tbl_marca ORDER BY nomeMarca";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbMarca.DataSource = dt;
                cmbMarca.DisplayMember = "nomeMarca";
                cmbMarca.ValueMember = "codMarca";
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar a lista de marcas. \n\n" + erro);
            }
        }

        private void InserirProduto()
        {
            try
            {
                banco.Conectar();
                string inserir = "insert into tbl_produto (nomeProduto, codMarca, valorProduto, dataValProduto, qtdeProduto, barrasProduto, statusProduto, fotoProduto) values (@nome,@marca,@valor,@dataval,@qtde,@barras,@status,@foto);";
                MySqlCommand cmd = new MySqlCommand(inserir, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome",     Variaveis.nomeProduto);
                cmd.Parameters.AddWithValue("@marca", Variaveis.codMarca);
                cmd.Parameters.AddWithValue("@valor", Variaveis.valorProduto);
                cmd.Parameters.AddWithValue("@dataval",    Variaveis.dataValProduto);
                cmd.Parameters.AddWithValue("@qtde",    Variaveis.qtdeProduto);
                cmd.Parameters.AddWithValue("@barras",     Variaveis.barrasProduto);
                cmd.Parameters.AddWithValue("@status",      Variaveis.statusProduto);
                cmd.Parameters.AddWithValue("@foto",   Variaveis.fotoProduto);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produto cadastrado com sucesso", "CADASTRO DO PRODUTO");
                banco.Desconectar();
                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoProduto))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/produtos/" + Path.GetFileName(Variaveis.fotoProduto);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoProduto, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
                        }
                        catch
                        {
                            MessageBox.Show("Foto não foi selecionada ou existente no servidor.", "FOTO", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao inserir Produto. \n\n" + erro);
            }
        }


        private void CarregarCmbMarca()
        {
            try
            {
                banco.Conectar();
                string selecionar = "SELECT codMarca, nomeMarca FROM tbl_marca ORDER BY nomeMarca";
                MySqlCommand cmd = new MySqlCommand(selecionar, banco.conexao);
                MySqlDataAdapter da = new MySqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                cmbMarca.DataSource = dt;
                cmbMarca.DisplayMember = "nomeMarca";
                cmbMarca.ValueMember = "codMarca";

                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao carregar a lista de Produtos. \n\n" + erro);
            }
        }



        //FIM DOS METODOSSSSS

        private void btnLimpar_Click_1(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = -1;
            txtNome.Clear();
            cmbMarca.SelectedIndex = -1;
            txtQuantidade.Clear();
            txtValor.Clear();
            txtValor.Clear();
            cmbStatus.SelectedIndex = -1;
            mtbData.Clear();
            mtbCodBarras.Clear();

            txtNome.Focus();
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            {
                try
                {
                    OpenFileDialog ofdFoto = new OpenFileDialog();
                    ofdFoto.Multiselect = false;
                    ofdFoto.FileName = "";
                    ofdFoto.InitialDirectory = @"C:\xampp\htdocs\patasepelos\admin\img";
                    ofdFoto.Title = "SELECIONE UMA FOTO";
                    ofdFoto.Filter = "JPG ou PNG (*.jpg ou (*png|*.jpg;*.png";
                    ofdFoto.CheckFileExists = true;
                    ofdFoto.CheckPathExists = true;
                    ofdFoto.RestoreDirectory = true;

                    DialogResult result = ofdFoto.ShowDialog();
                    pctFoto.Image = Image.FromFile(ofdFoto.FileName);
                    Variaveis.fotoProduto = "produtos/" + Regex.Replace(txtNome.Text, @"\s", "").ToLower() + ".png";

                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            Variaveis.altFotoProduto = "S";
                            Variaveis.caminhoFotoProduto = ofdFoto.FileName;

                        }
                        catch (SecurityException erro)
                        {
                            MessageBox.Show("Erro de segurança - Fale com o Admin \n Mensagem: " + erro + "\n Detalhe: " + erro.StackTrace);
                        }
                        catch (Exception erro)
                        {
                            MessageBox.Show("Você não tem permissão. \n Detalhe: " + erro);
                        }
                    }
                    btnCadastrar.Focus();
                }
                catch (Exception erro)
                {
                    btnCadastrar.Focus();
                }
            }
        }



        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtValor.Enabled = true;
                txtValor.Focus();
            }
        }


        private void txtValor_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtQuantidade.Enabled = true;
                txtQuantidade.Focus();
            }
        }

        private void txtQuantidade_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnAdicionar.Enabled = true;
                btnAdicionar.Focus();
            }
        }

        private void btnAdicionar_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mtbData.Enabled = true;
                mtbData.Focus();
            }
        }

        private void mtbData_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                mtbData.Enabled = true;
                mtbData.Focus();
            }
        }

        private void mtbCodBarras_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnCadastrar.Enabled = true;
                btnCadastrar.Focus();
            }
        }

        private void frmProCadastro_Load(object sender, EventArgs e)
        {
            if (Variaveis.funcao == "Cadastrar")
            {
                lblCadastro.Text = "Cadastrar";
            }
            else if (Variaveis.funcao == "Alterar")
            {
                lblCadastro.Text = "Alterar";
                CarregarDadosProduto();
            }
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            lblNome.ForeColor = Color.FromArgb(255, 154, 91);
            lblMarca.ForeColor = Color.FromArgb(255, 154, 91);
            lblValor.ForeColor = Color.FromArgb(255, 154, 91);
            lblDataVenc.ForeColor = Color.FromArgb(255, 154, 91);
            lblQuantidade.ForeColor = Color.FromArgb(255, 154, 91);
            lblCodBarras.ForeColor = Color.FromArgb(255, 154, 91);
            lblStatus.ForeColor = Color.FromArgb(255, 154, 91);
            lblFoto.ForeColor = Color.FromArgb(255, 154, 91);


            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencher o nome do Produto");
                lblNome.ForeColor = Color.FromArgb(163, 140, 214);
                txtNome.Focus();
            }
            else if (cmbMarca.Text == "")
            {
                MessageBox.Show("Preencher o nome da Marca");
                lblMarca.ForeColor = Color.FromArgb(163, 140, 214);
                cmbMarca.Focus();
            }
            else if (txtValor.Text == "")
            {
                MessageBox.Show("Preencher o valor do Produto");
                lblValor.ForeColor = Color.FromArgb(163, 140, 214);
                txtValor.Focus();
            }
            else if (mtbData.Text == "")
            {
                MessageBox.Show("Preencher a senha do Serviço");
                lblDataVenc.ForeColor = Color.FromArgb(163, 140, 214);
                mtbData.Focus();
            }
            else if (txtQuantidade.Text == "")
            {
                MessageBox.Show("Preencher a quantidade do Produto");
                lblQuantidade.ForeColor = Color.FromArgb(163, 140, 214);
                txtQuantidade.Focus();
            }
            else if (mtbCodBarras.Text == "")
            {
                MessageBox.Show("Preencher o codigo de barras do Produto");
                lblCodBarras.ForeColor = Color.FromArgb(163, 140, 214);
                mtbCodBarras.Focus();
            }
            else if (cmbStatus.Text == "")
            {
                MessageBox.Show("Preencher o status do Produto");
                lblStatus.ForeColor = Color.FromArgb(163, 140, 214);
                cmbStatus.Focus();
            }
            else
            {
                Variaveis.nomeProduto = txtNome.Text;
                Variaveis.codMarca = Convert.ToInt32(cmbMarca.SelectedValue);
                Variaveis.valorProduto = double.Parse(txtValor.Text);
                Variaveis.dataValProduto = Convert.ToDateTime(mtbData.Text);
                Variaveis.qtdeProduto = double.Parse(txtValor.Text);
                Variaveis.barrasProduto = mtbCodBarras.Text;
                Variaveis.statusProduto = lblStatus.Text;


                if (Variaveis.funcao == "Cadastrar")
                {
                    InserirProduto();
                    btnLimpar.PerformClick();
                }
                else if (Variaveis.funcao == "Alterar")
                {
                    AlterarProduto();
                    if (Variaveis.altFotoProduto == "S")
                    {
                        AlterarFotoProduto();
                    }
                }
                new frmProduto().Show();
                Hide();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {

        }

        private void cmbMarca_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
