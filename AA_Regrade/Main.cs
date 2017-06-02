//Coded by - Aaron Darby
//Kyrios: Felthas
// April 29th, 2017

using System;
using System.Linq;
using System.ComponentModel;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;


namespace AA_Regrade
{
    public partial class Main : Form
    {

        //Global Variables
        int currentGrade, charmedGrade, targetGrade, iterations, classification, totalVocation = 0, lastSelection = 0, selectedCharm = 0;
        int emulatorItemType = 0;// 1= Gear 2= Ship
        int emulatorItemGrade = 0;// 1= Basic ... 11=Mythic
        int emulatorScrollType = 0;// 1= Standard 2=Resplendent
        double emulatorOdds = 0;//getOdds will fill this
        int emulatorClass = 0;//1 = Easy, 2 = Normal, 3 = Difficult
        double emulatorCharmType = 1; //Multiplier (1 *no charm*, 1.5, 1.75, 2, 2.5)
        double[] attempts = new double[11];
        double[] successes = new double[11];
        double[] cumulativeCost = new double[11];
        double[] majorFails = new double[11];
        double patch = 3.5;
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
        bool emulatorIsAnchored = false;
        bool emulatorIsReady = false;
        
        
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
            comboBoxClassification.SelectedIndex = 0;
            comboBoxCharmMulitplier.SelectedIndex = 0;
            comboBoxClassification.Items.RemoveAt(3);
            comboBoxClassification.Enabled = true;
            checkBoxBundle.Enabled = false;
            checkBoxButcher.Enabled = false;
            buttonAddToCrop.Enabled = false;
            //Format image layers
            pictureBoxCharm.BringToFront();
            pictureBoxItem.BringToFront();
            pictureBoxScroll.BringToFront();
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
                selectedCharm = comboBoxCharmMulitplier.SelectedIndex;

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
        public void doEnchant(double scroll, double rScroll, double charm, double enchant, int iterations, int currentGrade, int charmedGrade, int targetGrade, bool isRegradeEvent, bool isShip, int selectedCharm)
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
             * 11 = Eternal
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
                    odds = getOdds(currentGrade, checkBoxResplend.Checked, getCharmed, charmedGrade, isShip, classification, selectedCharm);
                    if (odds > 10000) odds = 10000;//Can't have greater than 100% chance
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
        public double getOdds(int grade, bool isResplend, bool isCharmed, int charmedGrade, bool isShip, int classification, int selectedCharm)
        {
            double multiplier = 0;
            switch (selectedCharm)
            {
                case 0:
                    multiplier = 1.5;
                    break;
                case 1:
                    multiplier = 1.75;
                    break;
                case 2:
                    multiplier = 2.0;
                    break;
                case 3:
                    multiplier = 2.5;
                    break;
                default:
                    multiplier = 1.0;
                    break;
            }
            switch (grade)
            {
                case 0://Basic -> Grand
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 10000 * multiplier;
                        else return 10000;
                    }
                    else{
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 6000 * multiplier;
                                else return 6000;
                        }
                    }
                case 1://Grand -> Rare
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 10000 * multiplier;
                        else return 10000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 4000 * multiplier;
                                else return 4000;
                        }
                    }
                case 2://Rare -> Arcane
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 6000 * multiplier;
                        else return 10000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 10000 * multiplier;
                                else return 10000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 5000 * multiplier;
                                else return 5000;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 3000 * multiplier;
                                else return 3000;
                        }
                    }
                case 3://Arcane -> Heroic
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 6000 * multiplier;
                        else return 6000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 6750 * multiplier;
                                else return 6750;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 5000 * multiplier;
                                else return 5000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 3250 * multiplier;
                                else return 3250;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 3000 * multiplier;
                                else return 3000;
                        }
                    }
                case 4://Heroic -> Unique
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 6000 * multiplier;
                        else return 6000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 6750 * multiplier;
                                else return 6750;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 5000 * multiplier;
                                else return 5000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 3250 * multiplier;
                                else return 3250;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 2500 * multiplier;
                                else return 2500;
                        }
                    }
                case 5://Unique -> Celestial
                    if (isShip && patch == 2.9) return 5000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 5000 * multiplier;
                        else return 5000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 4730 * multiplier;
                                else return 4730;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 3500 * multiplier;
                                else return 3500;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 2280 * multiplier;
                                else return 2280;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 2000 * multiplier;
                                else return 2000;
                        }
                    }
                case 6://Celestial -> Divine
                    if (isShip && patch == 2.9) return 4000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 5000 * multiplier;
                        else return 5000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 4050 * multiplier;
                                else return 4050;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 3000 * multiplier;
                                else return 3000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 1950 * multiplier;
                                else return 1950;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 1000 * multiplier;
                                else return 1000;
                        }
                    }
                case 7://Divine -> Epic
                    if (isShip && patch == 2.9) return 3000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 4000 * multiplier;
                        else return 4000;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 1350 * multiplier;
                                else return 1350;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 1000 * multiplier;
                                else return 1000;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 650 * multiplier;
                                else return 650;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 750 * multiplier;
                                else return 750;
                        }
                    }
                case 8://Epic -> Legendary
                    if (isShip && patch == 2.9) return 3000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 3500 * multiplier;
                        else return 3500;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 1080 * multiplier;
                                else return 1080;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 800 * multiplier;
                                else return 800;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 520 * multiplier;
                                else return 520;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 500 * multiplier;
                                else return 500;
                        }
                    }
                case 9://Legendary -> Mythic
                    if (isShip && patch == 2.9) return 2000;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 1750 * multiplier;
                        else return 1750;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 410 * multiplier;
                                else return 410;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 300 * multiplier;
                                else return 300;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 200 * multiplier;
                                else return 200;
                            default://Should not happen with 3.5, these are 2.9 Values
                                if (isCharmed && charmedGrade >= grade) return 250 * multiplier;
                                else return 250;
                        }
                    }
                case 10://Mythic -> Eternal
                    if (isShip && patch == 2.9) return 880;
                    else if (isShip && patch == 3.5)
                    {
                        if (isCharmed) return 880 * multiplier;
                        else return 880;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                if (isCharmed && charmedGrade >= grade) return 270 * multiplier;
                                else return 270;
                            case 1://Normal
                                if (isCharmed && charmedGrade >= grade) return 200 * multiplier;
                                else return 200;
                            case 2://Difficult
                                if (isCharmed && charmedGrade >= grade) return 130 * multiplier;
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

        /*
        private void checkBoxTesting_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBoxTesting.Checked)
            {
                label63.Enabled = true;
                comboBoxClassification.Enabled = true;
                comboBoxTarget.Items.Add("Eternal");
                comboBoxClassification.SelectedIndex = 0;
                comboBoxClassification.Items.RemoveAt(3);
                patch = 3.5;
            }
            else
            {
                MessageBox.Show("This feature is only enabled while ArcheAge NA is still in 3.0. After 3.5, this feature will be permanently removed.");
                comboBoxClassification.Enabled = false;
                comboBoxTarget.Items.Remove("Eternal");
                comboBoxTarget.SelectedIndex = 0;
                comboBoxClassification.Items.Add("**3.0**");
                comboBoxClassification.SelectedIndex = 3;
                patch = 3.0;
                label63.Enabled = false;
            }
        }
        */

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
            if (crops.ElementAt(comboBoxCrop.SelectedIndex).canButcher)
            {
                checkBoxButcher.Enabled = true;
            }
            else
            {
                checkBoxButcher.Enabled = false;
                checkBoxButcher.Checked = false;
            }
        }

        private void buttonShip_Click(object sender, EventArgs e)
        {
            resetEmulator();//Resets the emulator
            emulatorItemType = 2;
            setGrade(2);
            setItem(emulatorItemType);
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

        private void buttonGear_Click(object sender, EventArgs e)
        {
            resetEmulator();
            emulatorItemType = 1;
            setGrade(1);
            setItem(emulatorItemType);
        }

        private void buttonGrade1_Click(object sender, EventArgs e)
        {
            resetCharm();
            if(emulatorItemType != 0)
            {
                setGrade(1);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(1);
            }
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

        private void buttonGrade2_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(2);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(2);
            }
        }

        private void buttonGrade3_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(3);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(3);
            }
        }

        private void buttonGrade4_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(4);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(4);
            }
        }

        private void buttonGrade5_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(5);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(5);
            }
        }

        private void buttonGrade6_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(6);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(6);
            }
        }

        private void buttonGrade7_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(7);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(7);
            }
        }

        private void buttonGrade8_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(8);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(8);
            }
        }

        private void buttonGrade9_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(9);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(9);
            }
        }

        private void buttonGrade10_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(10);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(10);
            }
        }

        private void buttonGrade11_Click(object sender, EventArgs e)
        {
            resetCharm();
            if (emulatorItemType != 0)
            {
                setGrade(11);
            }
            else
            {
                emulatorItemType = 1;
                setItem(emulatorItemType);
                setGrade(11);
            }
        }

        private void buttonStandard_Click(object sender, EventArgs e)
        {
            if(emulatorItemType != 0)
            {
                pictureBoxScroll.BackgroundImage = Properties.Resources.scroll_standard;
                pictureBoxScroll.Image = Properties.Resources.rare;
                emulatorScrollType = 1;
            }
            eval();
        }

        private void buttonResplendent_Click(object sender, EventArgs e)
        {
            if(emulatorItemType != 2 && emulatorItemType != 0)
            {
                pictureBoxScroll.BackgroundImage = Properties.Resources.scroll_resplend;
                pictureBoxScroll.Image = Properties.Resources.heroic;
                emulatorScrollType = 2;
            }
            eval();
        }

        private void buttonAddToCrop_Click(object sender, EventArgs e)
        {
            //Local variables
            int index = comboBoxCrop.SelectedIndex;
            int quantityPlanted = -1;
            int minYield, maxYield, byproduct = 0, vocationCost, byproductMultiplyer = 0;
            int seedMultiplyer = 0;
            double goldCost;

            string byProductString = "";

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

            if (checkBoxButcher.Checked)
            {
                switch (cropName)
                {
                    case "Chicken":
                        minYield = 4 * quantityPlanted;
                        maxYield = 6 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Duck":
                        minYield = 4 * quantityPlanted;
                        maxYield = 6 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Goose":
                        minYield = 4 * quantityPlanted;
                        maxYield = 6 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Turkey":
                        minYield = 4 * quantityPlanted;
                        maxYield = 6 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Pig":
                        byproductMultiplyer = 30;
                        break;
                    case "Sheep":
                        minYield = 16 * quantityPlanted;
                        maxYield = 20 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Goat":
                        minYield = 16 * quantityPlanted;
                        maxYield = 20 * quantityPlanted;
                        byproductMultiplyer = 30;
                        break;
                    case "Cow":
                        minYield = 16 * quantityPlanted;
                        maxYield = 20 * quantityPlanted;
                        byproductMultiplyer = 50;
                        break;
                    case "Yata":
                        minYield = 16 * quantityPlanted;
                        maxYield = 20 * quantityPlanted;
                        byproductMultiplyer = 50;
                        break;
                    case "Water Buffalo":
                        minYield = 16 * quantityPlanted;
                        maxYield = 20 * quantityPlanted;
                        byproductMultiplyer = 50;
                        break;
                }
                if(cropName != "Blizzard Bear")cropName += " (Meat)";
                byproduct = ((minYield + maxYield) / 20) * byproductMultiplyer;
            }

            if(crops.ElementAt(index).family != 4)byproduct = 0;
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
            if (!checkBoxButcher.Checked)
            {
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
            }
            else
            {
                if (cropName != "Blizzard Bear") byProductString = byproduct + " Trimmed Meat";
                else byProductString += byproduct + " Bear Fur";
            }
            
            goldCost = (crops.ElementAt(index).silver / 100) * quantityPlanted * seedMultiplyer;
            vocationCost = (crops.ElementAt(index).vocation) * quantityPlanted * seedMultiplyer;
            totalGold += goldCost;
            totalVocation += vocationCost;
            
            dataGridViewCrops.Rows.Add(cropName, quantityPlanted, minYield, maxYield, byProductString, goldCost, vocationCost);
            labelGoldCrop.Text = totalGold.ToString();
            labelVocationCrop.Text = totalVocation.ToString();
        }

        private void buttonBlue_Click(object sender, EventArgs e)
        {
            if(emulatorItemGrade == 5 && emulatorItemType != 0)
            {
                emulatorCharmType = 1.75;
                pictureBoxCharm.BackgroundImage = Properties.Resources.blue;
                pictureBoxCharm.Image = Properties.Resources.grand;
                emulatorIsAnchored = false;
            }
            eval();
        }

        //Runs when the thread is complete
        private void backgroundWorker_RunWorkerCompleted_1(object sender, RunWorkerCompletedEventArgs e)
        {
            refreshPage();
        }

        private void buttonGreen_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 4 && emulatorItemType != 0)
            {
                emulatorCharmType = 1.75;
                pictureBoxCharm.BackgroundImage = Properties.Resources.green;
                pictureBoxCharm.Image = Properties.Resources.basic;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonYellow_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 6 && emulatorItemType != 0)
            {
                emulatorCharmType = 1.5;
                pictureBoxCharm.BackgroundImage = Properties.Resources.yellow;
                pictureBoxCharm.Image = Properties.Resources.rare;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonRed_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade <= 6 && emulatorItemType != 0)
            {
                emulatorCharmType = 2;
                pictureBoxCharm.BackgroundImage = Properties.Resources.red;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonSupYellow_Click(object sender, EventArgs e)
        {
            if (emulatorItemType != 0 && emulatorItemGrade != 12)
            {
                emulatorCharmType = 1.5;
                pictureBoxCharm.BackgroundImage = Properties.Resources.sup_yellow;
                pictureBoxCharm.Image = Properties.Resources.rare;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonSupRed_Click(object sender, EventArgs e)
        {
            if (emulatorItemType != 0 && emulatorItemGrade != 12)
            {
                emulatorCharmType = 2;
                pictureBoxCharm.BackgroundImage = Properties.Resources.sup_red;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonCelestAnchor_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 7 && emulatorItemType != 0)
            {
                emulatorCharmType = 1;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.rare;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void buttonSilver_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade >= 7 && emulatorItemType != 0 && emulatorItemGrade != 12)
            {
                emulatorCharmType = 2.5;
                pictureBoxCharm.BackgroundImage = Properties.Resources.silver;
                pictureBoxCharm.Image = Properties.Resources.heroic;
                emulatorIsAnchored = false;
            }
            eval();
        }

        private void buttonCelestAnchor15_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 7 && emulatorItemType != 0)
            {
                emulatorCharmType = 1.5;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.rare;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void buttonCelestAnchor20_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 7 && emulatorItemType != 0)
            {
                emulatorCharmType = 2.0;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void buttonDivineAnchor_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 8 && emulatorItemType != 0)
            {
                emulatorCharmType = 1;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void buttonDivineAnchor15_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 8 && emulatorItemType != 0)
            {
                emulatorCharmType = 1.5;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void buttonDivineAnchor20_Click(object sender, EventArgs e)
        {
            if (emulatorItemGrade == 8 && emulatorItemType != 0)
            {
                emulatorCharmType = 2.0;
                pictureBoxCharm.BackgroundImage = Properties.Resources.anchor;
                pictureBoxCharm.Image = Properties.Resources.arcane;
                emulatorIsAnchored = true;
            }
            eval();
        }

        private void checkBoxShip_CheckedChanged(object sender, EventArgs e)
        {
            //Disables all irrelevent options with ship component regrading
            if (checkBoxShip.Checked)
            {
                checkBoxEvent.Enabled = false;
                checkBoxMist.Enabled = false;
                checkBoxResplend.Enabled = false;
                checkBoxIsAnchorCelest.Checked = false;
                checkBoxIsAnchorDivine.Checked = false;
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

        private void buttonEasy_Click(object sender, EventArgs e)
        {
            buttonNormal.Enabled = true;
            buttonDifficult.Enabled = true;
            buttonEasy.Enabled = false;
            emulatorClass = 0;
            if (emulatorItemType != 0) eval();
        }

        private void buttonNormal_Click(object sender, EventArgs e)
        {
            buttonNormal.Enabled = false;
            buttonDifficult.Enabled = true;
            buttonEasy.Enabled = true;
            emulatorClass = 1;
            if (emulatorItemType != 0) eval();
        }

        private void buttonDifficult_Click(object sender, EventArgs e)
        {
            buttonNormal.Enabled = true;
            buttonDifficult.Enabled = false;
            buttonEasy.Enabled = true;
            emulatorClass = 2;
            if (emulatorItemType != 0) eval();
        }

        private void pictureBoxCancel_Click(object sender, EventArgs e)
        {
            resetEmulator();
        }

        private void pictureBoxEnchant_Click(object sender, EventArgs e)
        {
            //local variables
            bool didFail = true;
            bool didDegrade = false;
            bool didBlowUp = false;
            int successType = 0;
            int previousGrade = emulatorItemGrade;

            if (emulatorIsReady)
            {
                Random rng = new Random();
                int roll = rng.Next(0, 10000);
                if(roll > emulatorOdds)
                {
                    Console.WriteLine("Fail" + " Roll: " + roll + " RNG: " + emulatorOdds);
                    //Do Fail Stuff
                    roll = rng.Next(0, 10000);
                    successType = 5;
                    if (emulatorItemGrade == 7 && !emulatorIsAnchored)
                    {
                        if(roll < 5000 && emulatorItemType == 1)
                        {
                            Console.WriteLine("Degrade: [Arcane] <-<- [Celestial]");
                            didDegrade = true;
                            if (!checkBoxKeepItems.Checked)
                            {
                                emulatorItemGrade = 4;
                                setGrade(4);
                                resetCharm();
                                resetScroll();
                            }
                            successType = 3;
                        }
                        else
                        {
                            Console.WriteLine("Major Failure: The Item Exploded in a Blinding Light");
                            didBlowUp = true;
                            successType = 4;
                        }
                    }
                    else if(emulatorItemGrade > 7 && !emulatorIsAnchored)
                    {
                        Console.WriteLine("Major Failure: The Item Exploded in a Blinding Light");
                        successType = 4;
                        didBlowUp = true;
                    }
                }
                else
                {
                    didFail = false;
                    if(roll < emulatorOdds * 0.2 && emulatorItemGrade != 11 && emulatorScrollType == 2)
                    {
                        Console.WriteLine("Great Success!" + " Roll: " + roll + " RNG: " + emulatorOdds);
                        emulatorItemGrade += 2;
                        setGrade(emulatorItemGrade);
                        successType = 2;
                    }
                    else
                    {
                        Console.WriteLine("Succeed" + " Roll: " + roll + " RNG: " + emulatorOdds);
                        emulatorItemGrade++;
                        setGrade(emulatorItemGrade);
                        successType = 1;
                    }
                }
                if(!didFail)resetCharm();
                if (emulatorItemGrade == 12 || !checkBoxKeepItems.Checked)
                {
                    resetScroll();
                    resetCharm();
                }
                eval();
                FormResult result = new FormResult();
                result.displayResult(successType, previousGrade, emulatorItemGrade, emulatorIsAnchored, emulatorItemType);
                if (!checkBoxKeepItems.Checked && didBlowUp)
                {
                    resetEmulator();
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
            doEnchant(scroll, rScroll, charm, enchant, iterations, currentGrade, charmedGrade, targetGrade, isRegradeEvent, isShip, selectedCharm);
            isDoneEnchanting = true;
        }

        //Sets the item icon in the regrade emulator
        public void setItem(int type)
        {
            switch (type)
            {
                case 1:
                    pictureBoxItem.BackgroundImage = Properties.Resources.gear;
                    break;
                case 2:
                    pictureBoxItem.BackgroundImage = Properties.Resources.ship;
                    break;
            }
            pictureBoxItem.Image = Properties.Resources.basic;
        }
        //Sets the item grade in the regrade emulator
        public void setGrade(int grade)
        {
            switch (grade)
            {
                case 1:
                    pictureBoxItem.Image = Properties.Resources.basic;
                    break;
                case 2:
                    pictureBoxItem.Image = Properties.Resources.grand;
                    break;
                case 3:
                    pictureBoxItem.Image = Properties.Resources.rare;
                    break;
                case 4:
                    pictureBoxItem.Image = Properties.Resources.arcane;
                    break;
                case 5:
                    pictureBoxItem.Image = Properties.Resources.heroic;
                    break;
                case 6:
                    pictureBoxItem.Image = Properties.Resources.unique;
                    break;
                case 7:
                    pictureBoxItem.Image = Properties.Resources.celestial;
                    break;
                case 8:
                    pictureBoxItem.Image = Properties.Resources.divine;
                    break;
                case 9:
                    pictureBoxItem.Image = Properties.Resources.epic;
                    break;
                case 10:
                    pictureBoxItem.Image = Properties.Resources.legendary;
                    break;
                case 11:
                    pictureBoxItem.Image = Properties.Resources.mythic;
                    break;
                case 12:
                    pictureBoxItem.Image = Properties.Resources.eternal;
                    break;
            }
            emulatorItemGrade = grade;
            eval();
        }
        //Resets the emulator
        public void resetEmulator()
        {
            emulatorItemType = 0;
            emulatorItemGrade = 0;
            emulatorScrollType = 0;
            emulatorCharmType = 1;
            pictureBoxCharm.BackgroundImage = Properties.Resources.charm_initial;
            pictureBoxCharm.Image = null;
            pictureBoxScroll.BackgroundImage = Properties.Resources.scroll_initial;
            pictureBoxScroll.Image = null;
            pictureBoxItem.BackgroundImage = Properties.Resources.item_initial;
            pictureBoxItem.Image = null;
            pictureBoxDegrade.Image = Properties.Resources.chance_blank;
            pictureBoxRipChance.Image = Properties.Resources.chance_blank;
            labelChancePercent.Text = "";
            pictureBoxEnchant.Image = Properties.Resources.button_enchant_disable;
            emulatorIsReady = false;
        }
        //resets the scroll
        public void resetScroll()
        {
            pictureBoxScroll.BackgroundImage = Properties.Resources.scroll_initial;
            pictureBoxScroll.Image = null;
            emulatorScrollType = 0;
        }
        //resets the emulator charm
        public void resetCharm()
        {
            emulatorCharmType = 1;
            pictureBoxCharm.BackgroundImage = Properties.Resources.charm_initial;
            pictureBoxCharm.Image = null;
            emulatorIsAnchored = false;
        }
        //evaluates the chances
        public void eval()
        {
            //Local Variables
            bool isShip = false;
            int selectedCharm = 0;

            pictureBoxRipChance.Image = Properties.Resources.no_chance1;
            pictureBoxDegrade.Image = Properties.Resources.no_chance1;
            if(emulatorItemGrade == 7 && emulatorItemType != 1)
            {
                pictureBoxRipChance.Image = Properties.Resources.yes_chance1;
                pictureBoxDegrade.Image = Properties.Resources.no_chance1;
            }
            else if(emulatorItemGrade == 7 && !emulatorIsAnchored)
            {
                pictureBoxRipChance.Image = Properties.Resources.yes_chance1;
                pictureBoxDegrade.Image = Properties.Resources.yes_chance1;
            }
            else if(emulatorItemGrade > 7 && !emulatorIsAnchored)
            {
                pictureBoxRipChance.Image = Properties.Resources.yes_chance1;
                pictureBoxDegrade.Image = Properties.Resources.no_chance1;
            }

            //Evaluate if it is a ship part
            if (emulatorItemType == 2) isShip = true;

            //Evaluate selected charm
            switch (emulatorCharmType.ToString())
            {
                case "1":
                    selectedCharm = 5;
                    break;
                case "1.5":
                    selectedCharm = 0;
                    break;
                case "1.75":
                    selectedCharm = 1;
                    break;
                case "2":
                    selectedCharm = 2;
                    break;
                case "2.5":
                    selectedCharm = 3;
                    break;
            }
            emulatorOdds = getOdds((emulatorItemGrade - 1), false, true, 15, isShip, emulatorClass, selectedCharm);
            if (emulatorOdds > 10000) emulatorOdds = 10000;
            labelChancePercent.Text = Math.Round(emulatorOdds / 100, 2).ToString() + "%";

            //Evaluate if enchantment is ready
            if(emulatorItemType != 0 && emulatorScrollType != 0 && emulatorItemGrade != 12)
            {
                pictureBoxEnchant.Image = Properties.Resources.button_enchant_enable;
                emulatorIsReady = true;
            }
            else
            {
                pictureBoxEnchant.Image = Properties.Resources.button_enchant_disable;
                emulatorIsReady = false;
            }
        }
    }
}
