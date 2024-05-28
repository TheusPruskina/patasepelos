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
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/produto/" + Path.GetFileName(Variaveis.fotoProduto);
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
                MessageBox.Show("Erro ao alterar PRODUTO. \n\n" + erro);
            }
        }


        //INICIO DOS METODOSS

        private void CarregarCmbProduto()
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
                MessageBox.Show("Erro ao carregar a lista de marcas. \n\n" + erro);
            }
        }
        private void InserirProduto()
        {
            try
            {
                banco.Conectar();
                string inserir = "insert into tbl_produto (nomeProduto, codMarca, valorProduto, dataValProduto, qtdeProduto, barrasProduto, statusProduto,fotoProduto) values (@nome,@marca,@valor,@dataval,@qtde,@barras,@status,@foto);";
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

        private void AlterarProduto()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_produto set nomeProduto = @nome, codmarca = @marca,  valorProduto = @valor, dataValProduto = @dataval, qtdeProduto = @qtde, barrasProduto = @barras, statusProduto = @status, fotoProduto = @foto where idProduto = @id;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome", Variaveis.nomeProduto);
                cmd.Parameters.AddWithValue("@marca", Variaveis.codMarca);
                cmd.Parameters.AddWithValue("@valor", Variaveis.valorProduto);
                cmd.Parameters.AddWithValue("@dataval", Variaveis.dataValProduto);
                cmd.Parameters.AddWithValue("@qtde", Variaveis.qtdeProduto);
                cmd.Parameters.AddWithValue("@barras", Variaveis.barrasProduto);
                cmd.Parameters.AddWithValue("@status", Variaveis.statusProduto);
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoProduto);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Produto alterado com sucesso", "ALTERAR PRODUTO");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao alterar Produto. \n\n" + erro);
            }
        }


        //FIM DOS METODOSSSSS





        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            lblNome.ForeColor =         Color.FromArgb(255, 154, 91); 
            lblMarca.ForeColor =        Color.FromArgb(255, 154, 91); 
            lblValor.ForeColor =        Color.FromArgb(255, 154, 91); 
            lblQuantidade.ForeColor =   Color.FromArgb(255, 154, 91); 
            lblStatus.ForeColor =       Color.FromArgb(255, 154, 91); 
            lblDataVenc.ForeColor =     Color.FromArgb(255, 154, 91);
            lblDataVenc.ForeColor =     Color.FromArgb(255, 154, 91);
            lblCodBarras.ForeColor =    Color.FromArgb(255, 154, 91);


            if (cmbMarca.Text == "")
            {
                MessageBox.Show("Preencher o nome da Marca"); 
                lblMarca.ForeColor = Color.FromArgb(163, 140, 214);
                cmbMarca.Focus();
            }
            else if (txtNome.Text == "")
            {
                MessageBox.Show("Preencher o nome do Produto");
                lblNome.ForeColor = Color.FromArgb(163, 140, 214);
                txtNome.Focus();
            }
            else if (txtValor.Text == "")
            {
                MessageBox.Show("Preencher o valor do Produto");
                lblValor.ForeColor = Color.FromArgb(163, 140, 214);
                txtValor.Focus();
            }
            else if (txtQuantidade.Text == "")
            {
                MessageBox.Show("Preencher a quantidade do Produto");
                lblQuantidade.ForeColor = Color.FromArgb(163, 140, 214);
                txtQuantidade.Focus();
            }
            else if (cmbStatus.Text == "")
            {
                MessageBox.Show("Preencher o status do Produto"); 
                lblStatus.ForeColor = Color.FromArgb(163, 140, 214);
                cmbStatus.Focus();
            }
            else if (mtbData.Text == "")
            {
                MessageBox.Show("Preencher a senha do Serviço");
                lblDataVenc.ForeColor = Color.FromArgb(163, 140, 214);
                mtbData.Focus();
            }
            else
            {
                Variaveis.nomeProduto = txtNome.Text;
                Variaveis.codMarca = cmbMarca.Text;
                Variaveis.valorProduto = txtValor.Text;
                Variaveis.qtdeProduto = txtQuantidade.Text;
                Variaveis.statusProduto = cmbStatus.Text;

                if (Variaveis.funcao == "CADASTRAR")
                {
                    InserirProduto();
                    btnLimpar.PerformClick();
                }
                else if (Variaveis.funcao == "ALTERAR")
                {
                    AlterarProduto();
                    if (Variaveis.altFotoProduto == "S")
                    {
                        AlterarFotoProduto();
                    }
                }
                MessageBox.Show("Cadastrar");
            }
        }

        private void frmCadCadastro_Load(object sender, EventArgs e)
        {
            if (Variaveis.funcao == "CADASTRAR")
            {
                lblCadastro.Text = "CADASTRAR";
            }
            else if (Variaveis.funcao == "ALTERAR")
            {
                lblCadastro.Text = "ALTERAR";
                //CarregarDadosCliente();
            }
        }


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
                    ofdFoto.InitialDirectory = @"C:";
                    ofdFoto.Title = "SELECIONE UMA FOTO";
                    ofdFoto.Filter = "JPG ou PNG (*.jpg ou (*png|*.jpg;*.png";
                    ofdFoto.CheckFileExists = true;
                    ofdFoto.CheckPathExists = true;
                    ofdFoto.RestoreDirectory = true;

                    DialogResult result = ofdFoto.ShowDialog();
                    pctFoto.Image = Image.FromFile(ofdFoto.FileName);
                    Variaveis.fotoProduto = "produto/" + Regex.Replace(txtNome.Text, @"\s", "").ToLower() + ".png";

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

        private void button1_Click(object sender, EventArgs e)
        {
            new frmProduto().Show();
            Close();
        }
    }
}
