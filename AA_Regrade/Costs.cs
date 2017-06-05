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
        public int greenCharmCost = 0;
        public int blueCharmCost = 0;
        public int yellowCharmCost = 0;
        public int redCharmCost = 0;
        public int supYellowCharmCost = 0;
        public int supRedCharmCost = 0;
        public int silverCharmCost = 0;
        public int celestCharm10Cost = 0;
        public int celestCharm15Cost = 0;
        public int celestCharm20Cost = 0;
        public int divineCharm10Cost = 0;
        public int divineCharm15Cost = 0;
        public int divineCharm20Cost = 0;
        public int regScrollCost = 0;
        public int resplendScrollCost = 0;
        public int enchantCost = 0;

        public Costs()
        {
            InitializeComponent();
        }
        
        private void buttonAccept_Click(object sender, EventArgs e)
        {
            //Validate Input
            try
            {
                greenCharmCost      = int.Parse(textBoxGreenCharm.Text);
                blueCharmCost       = int.Parse(textBoxBlueCharm.Text);
                yellowCharmCost     = int.Parse(textBoxYellowCharm.Text);
                redCharmCost        = int.Parse(textBoxRedCharm.Text);
                supYellowCharmCost  = int.Parse(textBoxSupYellowCharm.Text);
                supRedCharmCost     = int.Parse(textBoxSupRedCharm.Text);
                silverCharmCost     = int.Parse(textBoxSilverCharm.Text);
                celestCharm10Cost   = int.Parse(textBoxCelestialAnchor10.Text);
                celestCharm15Cost   = int.Parse(textBoxCelestialAnchor15.Text);
                celestCharm20Cost   = int.Parse(textBoxCelestialAnchor20.Text);
                divineCharm10Cost   = int.Parse(textBoxDivineAnchor10.Text);
                divineCharm15Cost   = int.Parse(textBoxDivineAnchor15.Text);
                divineCharm20Cost   = int.Parse(textBoxDivineAnchor20.Text);
                regScrollCost       = int.Parse(textBoxStandardScroll.Text);
                resplendScrollCost  = int.Parse(textBoxResplendScroll.Text);
                enchantCost         = int.Parse(textBoxEnchantCost.Text);
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
