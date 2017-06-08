using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AA_Regrade
{
    public partial class Updates : Form
    {
        public Updates()
        {
            InitializeComponent();
        }

        public void getUpdates()
        {
            this.Show();
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
