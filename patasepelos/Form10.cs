using MySql.Data.MySqlClient;
using System;
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

namespace patasepelos
{
    public partial class frmCadServico : Form
    {
        public frmCadServico()
        {
            InitializeComponent();
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
            WebClient ftpServico = new WebClient();
            ftpServico.Credentials = new NetworkCredential(Variaveis.usuarioFtp, Variaveis.senhaFtp);
            try
            {
                byte[] imageToByte = ftpServico.DownloadData(caminhoArquivoFtp);
                return imageToByte;
            }
            catch
            {
                byte[] imageToByte = ftpServico.DownloadData("ftp://127.0.0.1/admin/img/Servicos/semimagem.png");
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

        private void AlterarServico()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_servico set nomeServico = @nome, fotoServico = @foto, descricaoServico = @descricao, statusServico = @status where idServico = @idServ;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome", Variaveis.nomeServico);
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoServico);
                cmd.Parameters.AddWithValue("@descricao", Variaveis.descricaoServico);
                cmd.Parameters.AddWithValue("@status", Variaveis.statusServico);

                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Servico alterado com sucesso", "ALTERAR Servico");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao alterar Servico. \n\n" + erro);
            }
        }

        private void AlterarFotoServico()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_Servico set fotoServico = @foto where idServico = @codigo;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoServico);
                cmd.Parameters.AddWithValue("@codigo", Variaveis.idServico);
                //fim parametros
                cmd.ExecuteNonQuery();
                banco.Desconectar();
                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoServico))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/servico/" + Path.GetFileName(Variaveis.fotoServico);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoServico, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
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
                MessageBox.Show("Erro ao alterar FOTO Servico. \n\n" + erro);
            }
        }

        private void InserirServico()
        {
            try
            {
                banco.Conectar();
                string inserir = "insert into tbl_servico (nomeServico, fotoServico, descricaoServico, statusServico) values (@nome,@foto,@descricao,@status);";
                MySqlCommand cmd = new MySqlCommand(inserir, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome", Variaveis.nomeServico);
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoServico);
                cmd.Parameters.AddWithValue("@descricao", Variaveis.descricaoServico);
                cmd.Parameters.AddWithValue("@status", Variaveis.statusServico);

                cmd.ExecuteNonQuery();
                MessageBox.Show("Servico cadastrado com sucesso", "CADASTRO DO Servico");
                banco.Desconectar();
                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoServico))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/servico/" + Path.GetFileName(Variaveis.fotoServico);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoServico, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
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
                MessageBox.Show("Erro ao inserir Servico. \n\n" + erro);
            }
        }


        //FIM DOS METODOSSSSS


        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            lblNome.ForeColor = Color.FromArgb(255, 154, 91);
            lblFoto.ForeColor = Color.FromArgb(255, 154, 91);
            lblDescricao.ForeColor = Color.FromArgb(255, 154, 91);
            lblStatus.ForeColor = Color.FromArgb(255, 154, 91);


            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencher o nome do Servico");
                lblNome.ForeColor = Color.FromArgb(163, 140, 214);
                txtNome.Focus();
            }
            else if (lblFoto.Text == "")
            {
                MessageBox.Show("Preencher a foto do Servico");
                lblFoto.ForeColor = Color.FromArgb(255, 154, 91);
                btnAdicionar.Focus();
            }
            else if (txtDescricao.Text == "")
            {
                MessageBox.Show("Preencher a descrição do Servico");
                lblDescricao.ForeColor = Color.FromArgb(163, 140, 214);
                txtDescricao.Focus();
            }
            else if (cmbStatus.Text == "")
            {
                MessageBox.Show("Preencher o status do Servico");
                lblStatus.ForeColor = Color.FromArgb(163, 140, 214);
                cmbStatus.Focus();
            }
            else
            {
                Variaveis.nomeServico = txtNome.Text;
                Variaveis.descricaoServico = txtDescricao.Text;
                Variaveis.statusServico = cmbStatus.Text;
                Variaveis.fotoServico = pctFoto.Text;

                if (Variaveis.funcao == "Cadastrar")
                {
                    InserirServico();
                    btnLimpar.PerformClick();
                }
                else if (Variaveis.funcao == "Alterar")
                {
                    AlterarServico();
                    if (Variaveis.altFotoServico == "S")
                    {
                        AlterarFotoServico();
                    }
                }
                Variaveis.funcao = "Cadastrar";
                new frmServico().Show();
                Hide();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            new frmServico().Show();
            Close();
        }

        private void btnLimpar_Click(object sender, EventArgs e)
        {
            cmbStatus.SelectedIndex = -1;
            txtNome.Clear();
            txtDescricao.Clear();
            pctFoto.Image = null;

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
                    Variaveis.fotoServico = "servico/" + Regex.Replace(txtNome.Text, @"\s", "").ToLower() + ".png";

                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            Variaveis.altFotoServico = "S";
                            Variaveis.caminhoFotoServico = ofdFoto.FileName;

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
    }
}
