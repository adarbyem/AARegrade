//Coded by - Aaron Darby
//Kyrios: Felthas
// April 29th, 2017

using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;


namespace AA_Regrade
{
    public partial class Main : Form
    {

        //Global Variables
        int currentGrade, charmedGrade, targetGrade, iterations, classification, totalVocation = 0, lastSelection = 0;
        double[] attempts = new double[11];
        double[] successes = new double[11];
        double[] cumulativeCost = new double[11];
        double[] majorFails = new double[11];
        double patch = 2.9;
        double totalCost = 0;
        double totalFail = 0;
        double GSUD = 0;
        double GSCE = 0;
        double GSDL = 0;
        double GSEM = 0;
        double GSLP = 0;
        double totalGold = 0;
        bool isRegradeEvent = false;
        bool isDoneEnchanting = true;

        
        
        //Global Object
        Help help = new Help();
        Updates updates = new Updates();
        Yield yield = new Yield();
        List<Yield.crop> crops;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //Initialize Values and Prepopulate Dropdowns
            comboBoxGrade.SelectedIndex = 0;
            comboBoxTarget.SelectedIndex = 1;
            comboBoxCharmGrade.SelectedIndex = 0;
            yield.generateCrops();
            crops = new List<Yield.crop>();
            //Initialize and disable
            label63.Enabled = false;
            comboBoxClassification.SelectedIndex = 3;
            comboBoxClassification.Enabled = false;
            checkBoxBundle.Enabled = false;
            buttonAddToCrop.Enabled = false;
        }

        private void buttonEnchant_Click(object sender, EventArgs e)
        {
            //Local Variables
            double scroll, rScroll, charm, enchant;

            //Step 1: Validate User Input
            bool validated = validate();
            if (!validated)
            {
                //Validates for correct entries in the text boxes
                MessageBox.Show("One or more entries are invalid, please check your numbers and try again.");
            }
            else
            {
                //Step 2: Parse Values
                scroll = double.Parse(textBoxStandardScroll.Text);
                rScroll = double.Parse(textBoxResplenScroll.Text);
                charm = double.Parse(textBoxCharm.Text);
                enchant = double.Parse(textBoxEnchant.Text);
                iterations = int.Parse(textBoxIterations.Text);
                isRegradeEvent = checkBoxEvent.Checked;

                //Setup the Progress Bar
                progressBar.Maximum = iterations;
                progressBar.Value = 0;

                //Step 3: Call Enchanting Process
                isDoneEnchanting = false;
                currentGrade = comboBoxGrade.SelectedIndex;
                charmedGrade = comboBoxCharmGrade.SelectedIndex;
                targetGrade = comboBoxTarget.SelectedIndex;
                classification = comboBoxClassification.SelectedIndex;
                buttonEnchant.Enabled = false;
                buttonEnchant.Text = "Enchanting...";

                //Initialize Global Variables
                //Initialize global variables
                attempts = new double[11];
                cumulativeCost = new double[11];
                majorFails = new double[11];
                successes = new double[11];

                //Reset the displayed values
                refreshPage();

                //Begin Threaded Work
                try
                {
                    backgroundWorker.RunWorkerAsync();
                }
                catch
                {
                    MessageBox.Show("Something went wrong, you should disregard the current results. The program should still work correctly for future tests.");
                }
            }
        }

        //Validate User Input
        public bool validate()
        {
            //Local Variables
            double test = 0.0;
            int iteration = 0;
            bool isValid = false;
            //Validate Entries in the Textboxes, they should correctly parse as double values
            try
            {
                test = double.Parse(textBoxStandardScroll.Text);
                test = double.Parse(textBoxResplenScroll.Text);
                test = double.Parse(textBoxCharm.Text);
                test = double.Parse(textBoxEnchant.Text);
                test = double.Parse(textBoxCelestAnchor.Text);
                test = double.Parse(textBoxDivineAnchor.Text);
                iteration = int.Parse(textBoxIterations.Text);
                isValid = true;
            }
            catch
            {
                //Nothing happens here, it's simply to catch an error and do nothing with it
            }
            if (comboBoxGrade.SelectedIndex >= comboBoxTarget.SelectedIndex) isValid = false;
            return isValid;
        }

        //Peform the Enchantments
        public void doEnchant(double scroll, double rScroll, double charm, double enchant, int iterations, int currentGrade, int charmedGrade, int targetGrade, bool isRegradeEvent, bool isShip)
        {
            /* Grade Values
             * 0 = Basic
             * 1 = Grand
             * 2 = Rare
             * 3 = Arcane
             * 4 = Heroic
             * 5 = Unique
             * 6 = Celestial
             * 7 = Divine
             * 8 = Epic
             * 9 = Legendary
             * 10 = Mythic
             * 11 = Primordial (Coming Soon)
             */

            //Local Objects
            Random rng = new Random();

            //Local Variables
            bool isDone = false;
            bool getCharmed = false;
            double odds = 0;
            double roll = 0;
            double sessionCost = 0;
            double trino = 1; 
            double miscModifier = 0;
            double enchantCost = 0;
            int initialGrade = currentGrade;

            //Initialize Global Variables
            GSUD = 0;
            GSCE = 0;
            GSDL = 0;
            GSEM = 0;

            //Begin Loop
            for (int count = 1; count <= iterations; count++)
            {
                currentGrade = initialGrade; //We don't want to modify the inital grade
                enchantCost = enchant;
                trino = 1; //Reset the Trino RNG modifyer

                //Begin enchanting an item until it reaches the target grade
                
                while (!isDone)
                {
                    //Step 1: Get Odds of Success and increment the attempt count
                    if (currentGrade == 6 && checkBoxIsAnchorCelest.Checked) getCharmed = true;
                    else if (currentGrade == 7 && checkBoxIsAnchorDivine.Checked) getCharmed = true;
                    else if (checkBoxCharms.Checked) getCharmed = true;
                    else getCharmed = false;
                    odds = getOdds(currentGrade, checkBoxResplend.Checked, getCharmed, charmedGrade, isShip, classification);
                    attempts[currentGrade]++;
                    //Step 2: Apply Statistics
                    //Add the cost of a resplendent scroll if applicable
                    if (currentGrade > 4 && currentGrade != 9)
                    {
                        if (checkBoxResplend.Checked) sessionCost += rScroll;
                        else sessionCost += scroll;
                    }
                    //Otherwise add the cost of a regular scroll
                    else sessionCost += scroll;
                    //Add the enchantment cost
                    sessionCost += enchantCost;
                    //Add the charm cost if applicable
                    if (currentGrade >= charmedGrade && (checkBoxCharms.Checked || checkBoxIsAnchorCelest.Checked || checkBoxIsAnchorDivine.Checked))
                    {
                        if (currentGrade == 6 && checkBoxIsAnchorCelest.Checked) sessionCost += double.Parse(textBoxCelestAnchor.Text);
                        else if (currentGrade == 7 && checkBoxIsAnchorDivine.Checked) sessionCost += double.Parse(textBoxDivineAnchor.Text);
                        else if (checkBoxCharms.Checked) sessionCost += charm;
                    }
                    cumulativeCost[currentGrade] += sessionCost;

                    //Step 3: Roll the Enchantment
                    roll = rng.Next(1, 10000) * (trino + miscModifier);
                    //Add modifier for a regrade event (Using modifiers from November 2016 Event)
                    if (isRegradeEvent && currentGrade <= 5) odds *= 1.5;
                    //Check for Success
                    if (roll <= odds)
                    {
                        //Success
                        successes[currentGrade]++;
                        trino += trinoRNG();
                        currentGrade++;
                        enchantCost += (enchantCost * .25); //This is a rough estimate to account for increasing enchanting costs
                        sessionCost = 0;
                        //Check for Great Success if using a resplendent
                        if (currentGrade >= 6 && ((patch != 3.5 && currentGrade != 10) || (patch == 3.5 && currentGrade != 11)) && checkBoxResplend.Checked)
                        {
                            if (roll <= odds * .25)
                            {
                                //Great Success
                                //if (currentGrade > 9) Console.WriteLine("Great Success! " + currentGrade + " -> " + (currentGrade + 1) + " " + roll + " " + (odds*.25));
                                trino += trinoRNG();
                                //Increment the Great Success count for the appropriate grade
                                if (currentGrade - 1 == 5) GSUD++;
                                if (currentGrade - 1 == 6) GSCE++;
                                if (currentGrade - 1 == 7) GSDL++;
                                if (currentGrade - 1 == 8) GSEM++;
                                if (currentGrade - 1 == 9) GSLP++;
                                currentGrade++;
                                enchantCost += (enchantCost * .25);
                            }
                        }
                    }
                    else
                    {
                        //Failure
                        sessionCost = 0;

                        //All ship parts break on fail
                        if (isShip && currentGrade >= 6)
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = enchant;
                            trino = 1;
                        }

                        //
                        if (!isShip && currentGrade == 6 && majorFail(checkBoxCharms.Checked, checkBoxIsAnchorCelest.Checked, isRegradeEvent, roll))
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = enchant;
                            trino = 1;
                        }
                        else if(!isShip && currentGrade == 6)
                        {
                            //Check for Mistsong Option
                            //Counts it as a "blown up" item on degrade for those who would rather just loot another unique T1
                            if (checkBoxMist.Checked)
                            {
                                majorFails[currentGrade]++;
                                currentGrade = initialGrade;
                                enchantCost = enchant;
                                trino = 1;
                            }
                            //Downgrade Item
                            enchantCost = enchant;
                            currentGrade = 3; //Downgrade
                            if (currentGrade > initialGrade)
                            {
                                for (int a = 3; a > initialGrade; a--)
                                {
                                    enchantCost *= 1.25;
                                }
                            }
                            else if (currentGrade < initialGrade)
                            {
                                for (int a = 3; a < initialGrade; a++)
                                {
                                    enchantCost /= 1.25;
                                }
                            }
                        }

                        //Break Divine if Not Anchored
                        if(currentGrade == 7 && !checkBoxIsAnchorDivine.Checked && !isShip)
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = enchant;
                            trino = 1;
                        }
                        //Break Epic and Above
                        else if (currentGrade > 7 && !isShip)
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = enchant;
                            trino = 1;
                        }
                    }

                    //Step 4: Did it get to the target grade?
                    if (currentGrade >= targetGrade)
                    {
                        trino = 1;
                        isDone = true; //Sentinel Value to Break the Loop
                    }
                }

                //Set done to false and reset values
                sessionCost = 0;
                isDone = false;
                //Report progress to the thread handler and update progress bar
                backgroundWorker.ReportProgress(count);
            }

            //All items have reached the target grade, calculate the averages and complete the thread
            totalCost = 0;
            totalFail = 0;
            //Calculate averages
            for(int count = 0; count < 10; count++)
            {
                attempts[count] = Math.Round(attempts[count] / iterations, 2);
                successes[count] = Math.Round(successes[count] / iterations, 2);
                cumulativeCost[count] = Math.Round(cumulativeCost[count] / iterations, 2);
                totalCost += cumulativeCost[count];
                majorFails[count] = Math.Round(majorFails[count] / iterations, 2);
                totalFail += majorFails[count];
            }
        }

        //Handles celestial break/degrade event
        public bool majorFail(bool charmed, bool anchored, bool regradeEvent, double roll)
        {
            //local variables
            double odds;

            switch (charmed)
            {
                case true:
                    odds = 6000;
                    break;
                default:
                    odds = 5500;
                    break;
            }

            if (roll > odds && !regradeEvent && !anchored) return true;
            else return false;
        }

        //Returns the trino RNG modifier when called
        public double trinoRNG()
        {
            if (checkBoxTrino.Checked)
            {
                //Trino can't let us be successful all the time :(
                return 0.05;
            }
            else return 0;
        }

        //Refreshes the GUI Panel to display the results of the simulation
        public void refreshPage()
        {
            if (attempts[0] > 0) labelGA.Text = attempts[0].ToString() + " (" + Math.Round(successes[0] / attempts[0], 3) * 100 + "%)";
            else labelGA.Text = "0 (0)";
            labelGC.Text = Math.Round(cumulativeCost[0]).ToString();
            labelGF.Text = majorFails[0].ToString();
            if (attempts[1] > 0) labelRA.Text = attempts[1].ToString() + " (" + Math.Round(successes[1] / attempts[1], 3) * 100 + "%)";
            else labelRA.Text = "0 (0)";
            labelRC.Text = Math.Round(cumulativeCost[1]).ToString();
            labelRF.Text = majorFails[1].ToString();
            if (attempts[2] > 0) labelAA.Text = attempts[2].ToString() + " (" + Math.Round(successes[2] / attempts[2], 3) * 100 + "%)";
            else labelAA.Text = "0 (0)";
            labelAC.Text = Math.Round(cumulativeCost[2]).ToString();
            labelAF.Text = majorFails[2].ToString();
            if (attempts[3] > 0) labelHA.Text = attempts[3].ToString() + " (" + Math.Round(successes[3] / attempts[3], 3) * 100 + "%)";
            else labelHA.Text = "0 (0)";
            labelHC.Text = Math.Round(cumulativeCost[3]).ToString();
            labelHF.Text = majorFails[3].ToString();
            if (attempts[4] > 0) labelUA.Text = attempts[4].ToString() + " (" + Math.Round(successes[4] / attempts[4], 3) * 100 + "%)";
            else labelUA.Text = "0 (0)";
            labelUC.Text = Math.Round(cumulativeCost[4]).ToString();
            labelUF.Text = majorFails[4].ToString();
            if (attempts[5] > 0) labelCA.Text = attempts[5].ToString() + " (" + Math.Round(successes[5] / attempts[5], 3) * 100 + "%)";
            else labelCA.Text = "0 (0)";
            labelCC.Text = Math.Round(cumulativeCost[5]).ToString();
            labelCF.Text = majorFails[5].ToString();
            if (attempts[6] > 0) labelDA.Text = attempts[6].ToString() + " (" + Math.Round(successes[6] / attempts[6] , 3) * 100 + "%)";
            else labelDA.Text = "0 (0)";
            labelDC.Text = Math.Round(cumulativeCost[6]).ToString();
            labelDF.Text = majorFails[6].ToString();
            if (attempts[7] > 0) labelEA.Text = attempts[7].ToString() + " (" + Math.Round(successes[7] / attempts[7], 3) * 100 + "%)";
            else labelEA.Text = "0 (0)";
            labelEC.Text = Math.Round(cumulativeCost[7]).ToString();
            labelEF.Text = majorFails[7].ToString();
            if (attempts[8] > 0) labelLA.Text = attempts[8].ToString() + " (" + Math.Round(successes[8] / attempts[8], 3) * 100 + "%)";
            else labelLA.Text = "0 (0)";
            labelLC.Text = Math.Round(cumulativeCost[8]).ToString();
            labelLF.Text = majorFails[8].ToString();
            if (attempts[9] > 0) labelMA.Text = attempts[9].ToString() + " (" + Math.Round(successes[9] / attempts[9], 3) * 100 + "%)";
            else labelMA.Text = "0 (0)";
            labelMC.Text = Math.Round(cumulativeCost[9]).ToString();
            labelMF.Text = majorFails[9].ToString();
            if (attempts[10] > 0) labelPA.Text = attempts[10].ToString() + " (" + Math.Round(successes[10] / attempts[10], 3) * 100 + "%)";
            else labelPA.Text = "0 (0)";
            labelPC.Text = Math.Round(cumulativeCost[10]).ToString();
            labelPF.Text = majorFails[10].ToString();
            if (attempts[5] > 0)labelGSUD.Text = (Math.Round((GSUD / iterations) / attempts[5], 3) * 100).ToString() + "%";
            if(attempts[6] > 0)labelGSCE.Text = (Math.Round((GSCE / iterations) / attempts[6], 3) * 100).ToString() + "%";
            if(attempts[7] > 0)labelGSDL.Text = (Math.Round((GSDL / iterations) / attempts[7], 3) * 100).ToString() + "%";
            if(attempts[8] > 0)labelGSEM.Text = (Math.Round((GSEM / iterations) / attempts[8], 3) * 100).ToString() + "%";
            if (attempts[9] > 0) labelGSLP.Text = (Math.Round((GSLP / iterations) / attempts[9], 3) * 100).ToString() + "%";
            labelMajor.Text = totalFail.ToString();
            labelCost.Text = totalCost.ToString();
            if (isDoneEnchanting)
            {
                buttonEnchant.Enabled = true;
                buttonEnchant.Text = "Perform Enchantments";
                isDoneEnchanting = false;
            }
        }
        
        private void checkBoxTrino_CheckedChanged(object sender, EventArgs e)
        {
            //Notify User of What Trino RNG is
            if (checkBoxTrino.Checked)
            {
                MessageBox.Show("Trino RNG is an adaptive RNG modifier that favors failures and gets worse with every success, only resetting after a failure. This is complete speculation and does not reflect published percentages, this is only to emulate how RNG appears to screw us over in AA.");
            }
        }

        //Function to get the odds of a specific regrade given the options checked
        public double getOdds(int grade, bool isResplend, bool isCharmed, int charmedGrade, bool isShip, int classification)
        {
            switch (grade)
            {
                case 0://Basic -> Grand
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)return 10000;
                    else{
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 6000;
                        }
                    }
                case 1://Grand -> Rare
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5) return 10000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 8000;
                                else return 4000;
                        }
                    }
                case 2://Rare -> Arcane
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5) return 6000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 10000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 6000;
                                else return 3000;
                        }
                    }
                case 3://Arcane -> Heroic
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5) return 6000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 6750;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 5000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 6500;
                                else return 3250;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 6000;
                                else return 3000;
                        }
                    }
                case 4://Heroic -> Unique
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5) return 6000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 6750;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000;
                                else return 5000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 6500;
                                else return 3250;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 5000;
                                else return 2500;
                        }
                    }
                case 5://Unique -> Celestial
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5) return 5000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 9460;
                                else return 4730;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 7000;
                                else return 3500;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 4560;
                                else return 2280;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 4000;
                                else return 2000;
                        }
                    }
                case 6://Celestial -> Divine
                    if (isShip && patch == 2.9) return 4000;
                    else if (isShip && patch == 3.5) return 5000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 8100;
                                else return 4050;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 7000;
                                else return 3500;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 3900;
                                else return 1950;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 2000;
                                else return 1000;
                        }
                    }
                case 7://Divine -> Epic
                    if (isShip && patch == 2.9) return 3000;
                    else if (isShip && patch == 3.5) return 4000;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 2700;
                                else return 1350;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 2000;
                                else return 1000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 1300;
                                else return 650;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 1500;
                                else return 750;
                        }
                    }
                case 8://Epic -> Legendary
                    if (isShip && patch == 2.9) return 3000;
                    else if (isShip && patch == 3.5) return 3500;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 2160;
                                else return 1080;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 1600;
                                else return 800;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 1040;
                                else return 520;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 1000;
                                else return 500;
                        }
                    }
                case 9://Legendary -> Mythic
                    if (isShip && patch == 2.9) return 2000;
                    else if (isShip && patch == 3.5) return 1750;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 820;
                                else return 410;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 600;
                                else return 300;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 400;
                                else return 200;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 500;
                                else return 250;
                        }
                    }
                case 10://Mythic -> Primordial
                    if (isShip && patch == 2.9) return 880;
                    else if (isShip && patch == 3.5) return 880;
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 540;
                                else return 270;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 400;
                                else return 200;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 260;
                                else return 130;
                            default://Should not happen
                                if (isCharmed && charmedGrade >= grade) return 0;
                                else return 0;
                        }
                    }
                default:
                    return 0;
            }
        }

        private void checkBoxTesting_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxTesting.Checked)
            {
                MessageBox.Show("This enables 3.5 values, keep in mind these values are only from the KR version and may not be the same when 3.5 hits NA or EU.");
                comboBoxTarget.Items.Add("Primordial");
                comboBoxClassification.SelectedIndex = 0;
                comboBoxClassification.Items.RemoveAt(3);
                patch = 3.5;
                if (!checkBoxShip.Checked)
                {
                    comboBoxClassification.Enabled = true;
                    label63.Enabled = true;
                }
            }
            else
            {
                comboBoxClassification.Enabled = false;
                comboBoxTarget.Items.Remove("Primordial");
                comboBoxTarget.SelectedIndex = 0;
                comboBoxClassification.Items.Add("**2.9**");
                comboBoxClassification.SelectedIndex = 3;
                patch = 2.9;
                label63.Enabled = false;
            }
        }

        private void buttonHelp_Click(object sender, EventArgs e)
        {
            //Opens the FAQ
            help.getHelp();
        }

        private void labelUpdates_Click(object sender, EventArgs e)
        {
            //Gets Updates
            updates.getUpdates();
        }

        private void comboBoxFamily_SelectedIndexChanged(object sender, EventArgs e)
        {
            //local variables
            //Get the crops for the selected family
            comboBoxCrop.Items.Clear();

            //Get the list of structures
            try
            {
                crops = yield.getCrops(comboBoxFamily.SelectedIndex);
                for (int count = 0; count < crops.Count; count++)
                {
                    comboBoxCrop.Items.Add(crops.ElementAt(count).name);
                }
                comboBoxCrop.SelectedIndex = 0;
                lastSelection = comboBoxFamily.SelectedIndex;
            }
            catch
            {
                comboBoxFamily.SelectedIndex = lastSelection;
                MessageBox.Show("The crop family you have selected has not been implemented yet.");
            }
            
        }

        private void comboBoxCrop_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Determine the state of the add crop button and the bundle checkbox
            buttonAddToCrop.Enabled = true;
            if (crops.ElementAt(comboBoxCrop.SelectedIndex).hasBundle)
            {
                checkBoxBundle.Enabled = true;
            }
            else
            {
                checkBoxBundle.Enabled = false;
                checkBoxBundle.Checked = false;
            }
        }

        private void buttonClearCrop_Click(object sender, EventArgs e)
        {
            //Clear the whole table
            dataGridViewCrops.Rows.Clear();
            labelGoldCrop.Text = "0";
            labelVocationCrop.Text = "0";
            totalGold = 0;
            totalVocation = 0;
        }

        private void buttonRemoveSelected_Click(object sender, EventArgs e)
        {
            //Remove selected row
            try
            {
                totalGold -= double.Parse(dataGridViewCrops.SelectedRows[0].Cells[5].Value.ToString());
                totalVocation -= int.Parse(dataGridViewCrops.SelectedRows[0].Cells[6].Value.ToString());
                labelGoldCrop.Text = totalGold.ToString();
                labelVocationCrop.Text = totalVocation.ToString();
                dataGridViewCrops.Rows.RemoveAt(dataGridViewCrops.SelectedRows[0].Index);
            }
            catch
            {
                //There shouldn't be any errors here
            }

        }

        private void buttonAddToCrop_Click(object sender, EventArgs e)
        {
            //Local variables
            int index = comboBoxCrop.SelectedIndex;
            int quantityPlanted = -1;
            int minYield, maxYield, byproduct, vocationCost;
            int seedMultiplyer = 0;
            double goldCost;

            string byProductString;

            //Generate Data to Add to the List

            string cropName = crops.ElementAt(index).name;
            
            int.TryParse(textBoxQuantity.Text, out quantityPlanted);
            if(quantityPlanted <= 0)
            {
                MessageBox.Show("That was an invalid quantity. Please try again.");
                return;
            }

            if (checkBoxBundle.Checked)
            {
                seedMultiplyer = 10;
                minYield = (20 + crops.ElementAt(index).minHarv - 1) * quantityPlanted;
                maxYield = (20 + crops.ElementAt(index).maxHarv + 1) * quantityPlanted;
                cropName += " Bundle";
            }
            else
            {
                seedMultiplyer = 1;
                minYield = crops.ElementAt(index).minHarv * quantityPlanted;
                maxYield = crops.ElementAt(index).maxHarv * quantityPlanted;
            }

            byproduct = 0;
            switch (crops.ElementAt(index).vocation)
            {
                case 0:
                    byproduct = ((minYield + maxYield) / 20) * 10; //10 byproduct per 10 crop on no vocation items
                    break;
                case 45:
                    byproduct = ((minYield + maxYield) / 20) * 20; //20 byproduct per 10 crop on no vocation items
                    break;
                case 90:
                    byproduct = ((minYield + maxYield) / 20) * 45; //45 byproduct per 10 crop on no vocation items
                    break;
            }
            
            switch (crops.ElementAt(index).family)
            {
                case 1:
                    byProductString = byproduct + " Dried Flowers";
                    break;
                case 2:
                    byProductString = byproduct + " Orchard Puree";
                    break;
                case 3:
                    byProductString = byproduct + " Ground Grain";
                    break;
                case 6:
                    byProductString = byproduct + " Medicinal Powder";
                    break;
                case 7:
                    byProductString = byproduct + " Medicinal Powder";
                    break;
                case 8:
                    byProductString = byproduct + " Chopped Produce";
                    break;
                default:
                    byProductString = "No byproduct";
                    break;
            }
            goldCost = (crops.ElementAt(index).silver / 100) * quantityPlanted * seedMultiplyer;
            vocationCost = (crops.ElementAt(index).vocation) * quantityPlanted * seedMultiplyer;
            totalGold += goldCost;
            totalVocation += vocationCost;
            
            dataGridViewCrops.Rows.Add(cropName, quantityPlanted, minYield, maxYield, byProductString, goldCost, vocationCost);
            labelGoldCrop.Text = totalGold.ToString();
            labelVocationCrop.Text = totalVocation.ToString();
        }

        //Runs when the thread is complete
        private void backgroundWorker_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshPage();
        }

        private void checkBoxShip_CheckedChanged(object sender, EventArgs e)
        {
            //Disables all irrelevent options with ship component regrading
            if (checkBoxShip.Checked)
            {
                checkBoxCharms.Enabled = false;
                checkBoxEvent.Enabled = false;
                checkBoxIsAnchorCelest.Enabled = false;
                checkBoxIsAnchorDivine.Enabled = false;
                checkBoxMist.Enabled = false;
                checkBoxResplend.Enabled = false;
                comboBoxCharmGrade.Enabled = false;
                label1.Enabled = false;
                checkBoxCharms.Checked = false;
                checkBoxEvent.Checked = false;
                checkBoxIsAnchorCelest.Checked = false;
                checkBoxIsAnchorDivine.Checked = false;
                checkBoxMist.Checked = false;
                checkBoxResplend.Checked = false;
                if(patch == 3.5){
                    label63.Enabled = false;
                    comboBoxClassification.Enabled = false;
                }
            }
            else
            {
                checkBoxCharms.Enabled = true;
                checkBoxEvent.Enabled = true;
                checkBoxIsAnchorCelest.Enabled = true;
                checkBoxIsAnchorDivine.Enabled = true;
                checkBoxMist.Enabled = true;
                checkBoxResplend.Enabled = true;
                if (patch == 3.5)
                {
                    label63.Enabled = true;
                    comboBoxClassification.Enabled = true;
                }
            }

        }

        //Updates the progress bar when progress has been reported by the thread
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        //Sets up the thread and starts the enchanting sequence
        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //local variables
            double scroll, rScroll, charm, enchant;
            int iterations;
            bool isShip = checkBoxShip.Checked;
            //Initialize global variables from UI elements
            scroll = double.Parse(textBoxStandardScroll.Text);
            rScroll = double.Parse(textBoxResplenScroll.Text);
            charm = double.Parse(textBoxCharm.Text);
            enchant = double.Parse(textBoxEnchant.Text);
            iterations = int.Parse(textBoxIterations.Text);
            //Start the enchanting function
            doEnchant(scroll, rScroll, charm, enchant, iterations, currentGrade, charmedGrade, targetGrade, isRegradeEvent, isShip);
            isDoneEnchanting = true;
        }
    }
}
