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
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel7_Paint(object sender, PaintEventArgs e)
        {

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

        private void btnCliente_Click(object sender, EventArgs e)
        {
            new frmCliente().Show();
            Hide();
        }

        private void btnServico_Click(object sender, EventArgs e)
        {
            new frmServico().Show();
            Hide();
        }

        private void btnPet_Click(object sender, EventArgs e)
        {
            new frmPet().Show();
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
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblData.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblHora.Text = DateTime.Now.ToString("HH:mm:ss");
        }
    }
}
