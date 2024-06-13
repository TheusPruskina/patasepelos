using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Security;
using System.Text.RegularExpressions;

namespace patasepelos
{
    public partial class frmFunc : Form
    {
        public frmFunc()
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
            WebClient ftpFuncionario = new WebClient();
            ftpFuncionario.Credentials = new NetworkCredential(Variaveis.usuarioFtp, Variaveis.senhaFtp);
            try
            {
                byte[] imageToByte = ftpFuncionario.DownloadData(caminhoArquivoFtp);
                return imageToByte;
            }
            catch
            {
                byte[] imageToByte = ftpFuncionario.DownloadData("ftp://127.0.0.1/admin/img/funcionario/semimagem.png");
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


        //INICIO DOS METODOS

        private void AlterarFuncionario()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_funcionario set nomeFuncionario = @nome, enderecoFuncionario = @endereco, telefoneFuncionario = @telefone, emailFuncionario = @email, senhaFuncionario = @senha, fotoFuncionario = @foto, statusFuncionario = @status, dataFuncionario = @data, especialidadeFuncionario = @especialidade, descFuncionario = descricao where idFuncionario = @idFuncionario;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome", Variaveis.nomeFuncionario);
                cmd.Parameters.AddWithValue("@endereco", Variaveis.enderecoFuncionario);
                cmd.Parameters.AddWithValue("@telefone", Variaveis.telefoneFuncionario);
                cmd.Parameters.AddWithValue("@email", Variaveis.emailFuncionario);
                cmd.Parameters.AddWithValue("@senha", Variaveis.senhaFuncionario);
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoFuncionario);
                cmd.Parameters.AddWithValue("@status", Variaveis.statusFuncionario);
                cmd.Parameters.AddWithValue("@data", Variaveis.dataFuncionario);
                cmd.Parameters.AddWithValue("@especialidade", Variaveis.especialidadeFuncionario);
                cmd.Parameters.AddWithValue("@descricao", Variaveis.descFuncionario);


                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Funcionario alterado com sucesso", "ALTERAR FUNCIONARIO");
                banco.Desconectar();
            }
            catch (Exception erro)
            {
                MessageBox.Show("Erro ao alterar FUNCIONARIO. \n\n" + erro);
            }
        }

        private void AlterarFotoFuncionario()
        {
            try
            {
                banco.Conectar();
                string alterar = "update tbl_funcionario set fotoFuncionario = @foto where idFuncionario = @funcionario;";
                MySqlCommand cmd = new MySqlCommand(alterar, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoFuncionario);
                cmd.Parameters.AddWithValue("@codigo", Variaveis.idFuncionario);
                //fim parametros
                cmd.ExecuteNonQuery();
                banco.Desconectar();
                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoFuncionario))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/funcionario/" + Path.GetFileName(Variaveis.fotoFuncionario);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoFuncionario, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
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
                MessageBox.Show("Erro ao alterar FOTO FUNCIONARIO. \n\n" + erro);
            }
        }
        private void InserirFuncionario()
        {
            try
            {
                banco.Conectar();
                string inserir = "insert into tbl_funcionario (nomeFuncionario, enderecoFuncionario, telefoneFuncionario, emailFuncionario, senhaFuncionario, fotoFuncionario, statusFuncionario, dataFuncionario, especialidadeFuncionario, descFuncionario ) values (@nome,@endereco,@telefone,@email,@senha,@foto, @status,@data,@especialidade,@descricao);";
                MySqlCommand cmd = new MySqlCommand(inserir, banco.conexao);
                //parametros
                cmd.Parameters.AddWithValue("@nome", Variaveis.nomeFuncionario);
                cmd.Parameters.AddWithValue("@endereco", Variaveis.enderecoFuncionario);
                cmd.Parameters.AddWithValue("@telefone", Variaveis.telefoneFuncionario);
                cmd.Parameters.AddWithValue("@email", Variaveis.emailFuncionario);
                cmd.Parameters.AddWithValue("@senha", Variaveis.senhaFuncionario);
                cmd.Parameters.AddWithValue("@foto", Variaveis.fotoFuncionario);
                cmd.Parameters.AddWithValue("@status", Variaveis.statusFuncionario);
                cmd.Parameters.AddWithValue("@data", Variaveis.dataFuncionario);
                cmd.Parameters.AddWithValue("@especialidade", Variaveis.especialidadeFuncionario);
                cmd.Parameters.AddWithValue("@descricao", Variaveis.descFuncionario);
                //fim parametros
                cmd.ExecuteNonQuery();
                MessageBox.Show("Funcionário cadastrado com sucesso", "CADASTRO DO FUNCIONÁRIO");
                banco.Desconectar();

                if (ValidarFTP())
                {
                    if (!string.IsNullOrEmpty(Variaveis.fotoFuncionario))
                    {
                        string urlEnviarArquivo = Variaveis.enderecoServidorFtp + "img/funcionario/" + Path.GetFileName(Variaveis.fotoFuncionario);
                        try
                        {
                            Ftp.EnviarArquivoFtp(Variaveis.caminhoFotoFuncionario, urlEnviarArquivo, Variaveis.usuarioFtp, Variaveis.senhaFtp);
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
                MessageBox.Show("Erro ao inserir Funcionario. \n\n" + erro);
            }
        }
        //FIM DOS METODOS




        private void btnSair_Click(object sender, EventArgs e)
        {
            new frmFuncionario().Show();
            Close();
        }

        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            lblNome.ForeColor = Color.FromArgb(255, 154, 91);
            lblEndereco.ForeColor = Color.FromArgb(255, 154, 91);
            lblTelefone.ForeColor = Color.FromArgb(255, 154, 91);
            lblEmail.ForeColor = Color.FromArgb(255, 154, 91);
            lblSenha.ForeColor = Color.FromArgb(255, 154, 91);
            lblFoto.ForeColor = Color.FromArgb(255, 154, 91);
            lblStatus.ForeColor = Color.FromArgb(255, 154, 91);
            lblData.ForeColor = Color.FromArgb(255, 154, 91);
            lblEspecialidade.ForeColor = Color.FromArgb(255, 154, 91);
            lblDescricao.ForeColor = Color.FromArgb(255, 154, 91);


            if (txtNome.Text == "")
            {
                MessageBox.Show("Preencher o nome do Funcionario");
                lblNome.ForeColor = Color.FromArgb(163, 140, 214);
                txtNome.Focus();
            }
            else if (txtEndereco.Text == "")
            {
                MessageBox.Show("Preencher o endereco do Funcionario");
                lblEndereco.ForeColor = Color.FromArgb(163, 140, 214);
                txtEndereco.Focus();
            }
            else if (txtTelefone.Text == "")
            {
                MessageBox.Show("Preencher o telefone do Funcionario");
                lblTelefone.ForeColor = Color.FromArgb(163, 140, 214);
                txtTelefone.Focus();
            }
            else if (txtEmail.Text == "")
            {
                MessageBox.Show("Preencher o email do Funcionario");
                lblEmail.ForeColor = Color.FromArgb(163, 140, 214);
                txtEmail.Focus();
            }
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Preencher a senha do Funcionario");
                lblSenha.ForeColor = Color.FromArgb(163, 140, 214);
                txtSenha.Focus();
            }
            else if (cmbStatus.Text == "")
            {
                MessageBox.Show("Preencher o status do Funcionario");
                lblStatus.ForeColor = Color.FromArgb(163, 140, 214);
                lblStatus.Focus();
            }
            else if (mtbData.Text == "")
            {
                MessageBox.Show("Preencher a data de cadastro do Funcionario");
                lblData.ForeColor = Color.FromArgb(163, 140, 214);
                mtbData.Focus();
            }
            else if (txtEspecialidade.Text == "")
            {
                MessageBox.Show("Preencher a especialidade do Funcionario");
                lblEspecialidade.ForeColor = Color.FromArgb(163, 140, 214);
                txtEspecialidade.Focus();
            }
            else if (txtDescricao.Text == "")
            {
                MessageBox.Show("Preencher a descrição do Funcionario");
                lblDescricao.ForeColor = Color.FromArgb(163, 140, 214);
                lblDescricao.Focus();
            }
            else
            {
                Variaveis.nomeFuncionario = txtNome.Text;
                Variaveis.enderecoFuncionario = txtDescricao.Text;
                Variaveis.telefoneFuncionario = double.Parse(txtTelefone.Text);
                Variaveis.emailFuncionario = txtEmail.Text;
                Variaveis.senhaFuncionario = txtSenha.Text;
                Variaveis.fotoFuncionario = pctFoto.Text;
                Variaveis.statusFuncionario = cmbStatus.Text;
                Variaveis.dataFuncionario = Convert.ToDateTime(mtbData.Text);
                Variaveis.especialidadeFuncionario = txtEspecialidade.Text;
                Variaveis.descFuncionario = txtDescricao.Text;
                if (Variaveis.funcao == "CADASTRAR")
                {
                    InserirFuncionario();
                    btnLimpar.PerformClick();
                }
                else if (Variaveis.funcao == "ALTERAR")
                {
                    AlterarFuncionario();
                    if (Variaveis.altFotoFuncionario == "S")
                    {
                        AlterarFotoFuncionario();
                    }
                }
                Variaveis.funcao = "CADASTRAR";
                new frmFuncionario().Show();
                Hide();
            }
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
                    Variaveis.fotoFuncionario = "funcionario/" + Regex.Replace(txtNome.Text, @"\s", "").ToLower() + ".png";

                    if (result == DialogResult.OK)
                    {
                        try
                        {
                            Variaveis.altFotoFuncionario = "S";
                            Variaveis.caminhoFotoFuncionario = ofdFoto.FileName;

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
