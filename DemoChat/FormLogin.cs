using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoChat
{
    public partial class FormLogin : Form
    {
        //Varivavel dos formulario chat
        private FormChat chatForm;


        public FormLogin()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {

            //Ao fazer login abre o janela do chat e esconde a janela do login
            chatForm = new FormChat(this);
            chatForm.Show();
            this.Hide();
        }
    }
}
