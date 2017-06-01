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
    public partial class FormResult : Form
    {
        public FormResult()
        {
            InitializeComponent();
        }
        //1 = Success, 2 = Great Success, 3 = Failure, 4 = Major Failure, 5 = no change
        public void displayResult(int successType, int previousGrade, int nextGrade, bool isAnchored, int itemType, int x, int y)
        {
            switch (successType)
            {
                case 1:
                    labelResult.ForeColor = Color.DarkGoldenrod;
                    labelResult.Text = "Success!";
                    labelSteps.Text = "->";
                    break;
                case 2:
                    labelResult.ForeColor = Color.SkyBlue;
                    labelResult.Text = "Great Success!";
                    labelSteps.Text = "->->";
                    break;
                case 3:
                    labelResult.ForeColor = Color.Red;
                    labelResult.Text = "Failure";
                    labelSteps.Text = "<-<-<-";
                    labelFailNarrative.Text = "Item downgraded...";
                    if (isAnchored)
                    {
                        labelSteps.Text = "";
                        labelFailNarrative.Text = "";
                    }
                    break;
                case 4:
                    labelResult.ForeColor = Color.Red;
                    labelResult.Text = "Major Failure";
                    labelSteps.Text = "";
                    labelFailNarrative.Text = "The item disappeared in a blinding light.";
                    labelPreviousGrade.Text = "";
                    labelNextGrade.Text = "";
                    if (isAnchored)
                    {
                        labelFailNarrative.Text = "";
                    }
                    break;
                case 5:
                    labelResult.ForeColor = Color.Red;
                    labelResult.Text = "Failure";
                    labelSteps.Text = "";
                    labelPreviousGrade.Text = "";
                    labelNextGrade.Text = "";
                    labelFailNarrative.Text = "The item failed the regrade.";
                    break;
            }
            if((successType > 2 && !isAnchored) || successType <= 2)
            {
                if(successType != 5 && successType != 4)
                {
                    labelPreviousGrade.Text = getGrade(previousGrade);
                    labelNextGrade.Text = getGrade(nextGrade);
                }
            }
            setGrade(nextGrade, itemType);
            this.ShowDialog();
            this.SetDesktopLocation(x, y);
            this.BringToFront();
        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
        public string getGrade(int grade)
        {
            switch (grade)
            {
                case 1:
                    return "[Basic]";
                case 2:
                    return "[Grand]";
                case 3:
                    return "[Rare]";
                case 4:
                    return "[Arcane]";
                case 5:
                    return "[Heroic]";
                case 6:
                    return "[Unique]";
                case 7:
                    return "[Celestial]";
                case 8:
                    return "[Divine]";
                case 9:
                    return "[Epic]";
                case 10:
                    return "[Legendary]";
                case 11:
                    return "[Mythic]";
                case 12:
                    return "[Eternal]";
                default:
                    return "Unknown";
            }
        }
        public void setGrade(int grade, int itemType)
        {
            if (itemType == 2) pictureBox2.BackgroundImage = Properties.Resources.ship;
            else pictureBox2.BackgroundImage = Properties.Resources.gear;
            switch (grade)
            {
                case 0:
                    pictureBox2.Image = null;
                    pictureBox2.BackColor = Color.Transparent;
                    break;
                case 2:
                    pictureBox2.Image = Properties.Resources.grand;
                    break;
                case 3:
                    pictureBox2.Image = Properties.Resources.rare;
                    break;
                case 4:
                    pictureBox2.Image = Properties.Resources.arcane;
                    break;
                case 5:
                    pictureBox2.Image = Properties.Resources.heroic;
                    break;
                case 6:
                    pictureBox2.Image = Properties.Resources.unique;
                    break;
                case 7:
                    pictureBox2.Image = Properties.Resources.celestial;
                    break;
                case 8:
                    pictureBox2.Image = Properties.Resources.divine;
                    break;
                case 9:
                    pictureBox2.Image = Properties.Resources.epic;
                    break;
                case 10:
                    pictureBox2.Image = Properties.Resources.legendary;
                    break;
                case 11:
                    pictureBox2.Image = Properties.Resources.mythic;
                    break;
                case 12:
                    pictureBox2.Image = Properties.Resources.eternal;
                    break;
            }
        }
    }
}
