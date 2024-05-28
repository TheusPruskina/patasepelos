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



        //FIM DOS METODOS


        private void button1_Click(object sender, EventArgs e)
        {
            Variaveis.usuario = txtEmail.Text;

            new frmMenu().Show();
            Hide();
        }

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

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
