using System;
using System.Windows.Forms;

namespace AA_Regrade
{
    public partial class Help : Form
    {
        public Help()
        {
            InitializeComponent();
        }

        public void getHelp()
        {
            this.Show();
        }

        private void labelProjectLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.github.com/adarbyem/AARegrade");
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
