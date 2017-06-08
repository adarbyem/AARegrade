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
    public partial class Costs : Form
    {
        //Global Variables
        public double greenCharmCost = 0;
        public double blueCharmCost = 0;
        public double yellowCharmCost = 0;
        public double redCharmCost = 0;
        public double supYellowCharmCost = 0;
        public double supRedCharmCost = 0;
        public double silverCharmCost = 0;
        public double celestCharm10Cost = 0;
        public double celestCharm15Cost = 0;
        public double celestCharm20Cost = 0;
        public double divineCharm10Cost = 0;
        public double divineCharm15Cost = 0;
        public double divineCharm20Cost = 0;
        public double regScrollCost = 0;
        public double resplendScrollCost = 0;
        public double enchantCost = 0;

        public Costs()
        {
            InitializeComponent();
        }
        
        private void buttonAccept_Click(object sender, EventArgs e)
        {
            //Validate Input
            try
            {
                greenCharmCost      = double.Parse(textBoxGreenCharm.Text);
                blueCharmCost       = double.Parse(textBoxBlueCharm.Text);
                yellowCharmCost     = double.Parse(textBoxYellowCharm.Text);
                redCharmCost        = double.Parse(textBoxRedCharm.Text);
                supYellowCharmCost  = double.Parse(textBoxSupYellowCharm.Text);
                supRedCharmCost     = double.Parse(textBoxSupRedCharm.Text);
                silverCharmCost     = double.Parse(textBoxSilverCharm.Text);
                celestCharm10Cost   = double.Parse(textBoxCelestialAnchor10.Text);
                celestCharm15Cost   = double.Parse(textBoxCelestialAnchor15.Text);
                celestCharm20Cost   = double.Parse(textBoxCelestialAnchor20.Text);
                divineCharm10Cost   = double.Parse(textBoxDivineAnchor10.Text);
                divineCharm15Cost   = double.Parse(textBoxDivineAnchor15.Text);
                divineCharm20Cost   = double.Parse(textBoxDivineAnchor20.Text);
                regScrollCost       = double.Parse(textBoxStandardScroll.Text);
                resplendScrollCost  = double.Parse(textBoxResplendScroll.Text);
                enchantCost         = double.Parse(textBoxEnchantCost.Text);
                if (greenCharmCost     < 0) throw new Exception();
                if (blueCharmCost      < 0) throw new Exception();
                if (yellowCharmCost    < 0) throw new Exception();
                if (redCharmCost       < 0) throw new Exception();
                if (supYellowCharmCost < 0) throw new Exception();
                if (supRedCharmCost    < 0) throw new Exception();
                if (silverCharmCost    < 0) throw new Exception();
                if (celestCharm10Cost  < 0) throw new Exception();
                if (celestCharm15Cost  < 0) throw new Exception();
                if (celestCharm20Cost  < 0) throw new Exception();
                if (divineCharm10Cost  < 0) throw new Exception();
                if (divineCharm15Cost  < 0) throw new Exception();
                if (divineCharm20Cost  < 0) throw new Exception();
                if (regScrollCost      < 0) throw new Exception();
                if (resplendScrollCost < 0) throw new Exception();
                if (enchantCost        < 0) throw new Exception();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("One or more fields are invalid, please check your numbers and try again.");
            }
        }
    }
}
