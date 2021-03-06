﻿//Coded by - Aaron Darby
//Kyrios: Felthas
//June 5th, 2017

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
        int currentGrade, targetGrade, iterations, classification, totalVocation = 0, lastSelection = 0, selectedCharm = 0;
        int emulatorItemType = 0;// 1= Gear 2= Ship
        int emulatorItemGrade = 0;// 1= Basic ... 11=Mythic
        int emulatorScrollType = 0;// 1= Standard 2=Resplendent
        int[,] gradeOptions = new int[12, 2];
        double emulatorOdds = 0;//getOdds will fill this
        int emulatorClass = 0;//1 = Easy, 2 = Normal, 3 = Difficult
        double emulatorCharmType = 1; //Multiplier (1 *no charm*, 1.5, 1.75, 2, 2.5)
        double[] attempts = new double[11];
        double[] successes = new double[11];
        double[] cumulativeCost = new double[11];
        double[] majorFails = new double[11];
        double[] prices = new double[16];
        double patch = 3.5;
        double totalCost = 0;
        double totalFail = 0;
        double currentEnchantCost = 0;
        double GSUD = 0;
        double GSCE = 0;
        double GSDL = 0;
        double GSEM = 0;
        double GSLP = 0;
        double totalGold = 0;
        double charmCost = 0;
        double scrollCost = 0;
        bool isRegradeEvent = false;
        bool isDoneEnchanting = true;
        bool emulatorIsAnchored = false;
        bool emulatorIsReady = false;
        bool checkGold = false;
        
        //Global Object
        Help help = new Help();
        Updates updates = new Updates();
        Yield yield = new Yield();
        Costs costs = new Costs();
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
            comboBoxBasicCharm.SelectedIndex = 0;
            comboBoxBasicScroll.SelectedIndex = 0;
            comboBoxGrandCharm.SelectedIndex = 0;
            comboBoxGrandScroll.SelectedIndex = 0;
            comboBoxArcaneCharm.SelectedIndex = 0;
            comboBoxArcaneScroll.SelectedIndex = 0;
            comboBoxRareCharm.SelectedIndex = 0;
            comboBoxRareScroll.SelectedIndex = 0;
            comboBoxHeroicCharm.SelectedIndex = 0;
            comboBoxHeroicScroll.SelectedIndex = 0;
            comboBoxUniqueCharm.SelectedIndex = 0;
            comboBoxUniqueScroll.SelectedIndex = 0;
            comboBoxCelestialCharm.SelectedIndex = 0;
            comboBoxCelestialScroll.SelectedIndex = 0;
            comboBoxDivineCharm.SelectedIndex = 0;
            comboBoxDivineScroll.SelectedIndex = 0;
            comboBoxEpicCharm.SelectedIndex = 0;
            comboBoxEpicScroll.SelectedIndex = 0;
            comboBoxLegendaryCharm.SelectedIndex = 0;
            comboBoxLegendaryScroll.SelectedIndex = 0;
            comboBoxMythicCharm.SelectedIndex = 0;
            comboBoxMythicScroll.SelectedIndex = 0;
            yield.generateCrops();
            crops = new List<Yield.crop>();
            //Initialize and disable
            comboBoxClassification.SelectedIndex = 0;
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
            //Setup Grade Options
            gradeOptions[0, 0] = comboBoxBasicScroll.SelectedIndex;
            gradeOptions[0, 1] = comboBoxBasicCharm.SelectedIndex;
            gradeOptions[1, 0] = comboBoxGrandScroll.SelectedIndex;
            gradeOptions[1, 1] = comboBoxGrandCharm.SelectedIndex;
            gradeOptions[2, 0] = comboBoxRareScroll.SelectedIndex;
            gradeOptions[2, 1] = comboBoxRareCharm.SelectedIndex;
            gradeOptions[3, 0] = comboBoxArcaneScroll.SelectedIndex;
            gradeOptions[3, 1] = comboBoxArcaneCharm.SelectedIndex;
            gradeOptions[4, 0] = comboBoxHeroicScroll.SelectedIndex;
            gradeOptions[4, 1] = comboBoxHeroicCharm.SelectedIndex;
            gradeOptions[5, 0] = comboBoxUniqueScroll.SelectedIndex;
            gradeOptions[5, 1] = comboBoxUniqueCharm.SelectedIndex;
            gradeOptions[6, 0] = comboBoxCelestialScroll.SelectedIndex;
            gradeOptions[6, 1] = comboBoxCelestialCharm.SelectedIndex;
            gradeOptions[7, 0] = comboBoxDivineScroll.SelectedIndex;
            gradeOptions[7, 1] = comboBoxDivineCharm.SelectedIndex;
            gradeOptions[8, 0] = comboBoxEpicScroll.SelectedIndex;
            gradeOptions[8, 1] = comboBoxEpicCharm.SelectedIndex;
            gradeOptions[9, 0] = comboBoxLegendaryScroll.SelectedIndex;
            gradeOptions[9, 1] = comboBoxLegendaryCharm.SelectedIndex;
            gradeOptions[10, 0] = comboBoxMythicScroll.SelectedIndex;
            gradeOptions[10, 1] = comboBoxMythicCharm.SelectedIndex;
            //Local Variables
            double scroll;

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
                scroll = costs.regScrollCost;
                iterations = int.Parse(textBoxIterations.Text);
                isRegradeEvent = checkBoxEvent.Checked;

                //Setup the Progress Bar
                progressBar.Maximum = iterations;
                progressBar.Value = 0;

                //Step 3: Call Enchanting Process
                isDoneEnchanting = false;
                currentGrade = comboBoxGrade.SelectedIndex;
                targetGrade = comboBoxTarget.SelectedIndex;
                classification = comboBoxClassification.SelectedIndex;
                buttonEnchant.Enabled = false;
                buttonEnchant.Text = "Enchanting...";

                //Initialize Global Variables
                GSUD = 0;
                GSCE = 0;
                GSDL = 0;
                GSEM = 0;
                GSLP = 0;
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
            int iteration = 0;
            bool isValid = false;
            //Validate Entries in the Textboxes, they should correctly parse as double values
            try
            {
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
        public void doEnchant(int iterations, int currentGrade, int targetGrade, bool isRegradeEvent, bool isShip, int selectedCharm, double[] prices, int[,] gradeOptions)
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
            double odds = 0;
            double roll = 0;
            double sessionCost = 0;
            double trino = 1; 
            double miscModifier = 0;
            double enchantCost = 0;
            int initialGrade = currentGrade;
            int charmSelection = 0;

            //Initialize Global Variables
            
            //Begin Loop
            for (int count = 1; count <= iterations; count++)
            {
                currentGrade = initialGrade; //We don't want to modify the inital grade
                enchantCost = prices[15];
                trino = 1; //Reset the Trino RNG modifyer

                //Begin enchanting an item until it reaches the target grade
                
                while (!isDone)
                {
                    //Step 1: Get Odds of Success and increment the attempt count

                    //Set the Charm Multiplier
                    if (currentGrade == 0)
                    {
                        if (gradeOptions[0, 1] == 0) charmSelection = -1;
                        if (gradeOptions[0, 1] == 1) charmSelection = 0;
                        if (gradeOptions[0, 1] == 2) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 1)
                    {
                        if (gradeOptions[1, 1] == 0) charmSelection = -1;
                        if (gradeOptions[1, 1] == 1) charmSelection = 0;
                        if (gradeOptions[1, 1] == 2) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 2)
                    {
                        if (gradeOptions[2, 1] == 0) charmSelection = -1;
                        if (gradeOptions[2, 1] == 1) charmSelection = 0;
                        if (gradeOptions[2, 1] == 2) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 3)
                    {
                        if (gradeOptions[3, 1] == 0) charmSelection = -1;
                        if (gradeOptions[3, 1] == 1) charmSelection = 1;
                        if (gradeOptions[3, 1] == 2) charmSelection = 0;
                        if (gradeOptions[3, 1] == 3) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 4)
                    {
                        if (gradeOptions[4, 1] == 0) charmSelection = -1;
                        if (gradeOptions[4, 1] == 1) charmSelection = 1;
                        if (gradeOptions[4, 1] == 2) charmSelection = 0;
                        if (gradeOptions[4, 1] == 3) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 5)
                    {
                        if (gradeOptions[5, 1] == 0) charmSelection = -1;
                        if (gradeOptions[5, 1] == 1) charmSelection = 0;
                        if (gradeOptions[5, 1] == 2) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 6)
                    {
                        if (gradeOptions[6, 1] == 0) charmSelection = -1;
                        if (gradeOptions[6, 1] == 1) charmSelection = 3;
                        if (gradeOptions[6, 1] == 2) charmSelection = 0;
                        if (gradeOptions[6, 1] == 3) charmSelection = 2;
                        if (gradeOptions[6, 1] == 4) charmSelection = -1;
                        if (gradeOptions[6, 1] == 5) charmSelection = 0;
                        if (gradeOptions[6, 1] == 6) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 7)
                    {
                        if (gradeOptions[7, 1] == 0) charmSelection = -1;
                        if (gradeOptions[7, 1] == 1) charmSelection = 3;
                        if (gradeOptions[7, 1] == 2) charmSelection = 0;
                        if (gradeOptions[7, 1] == 3) charmSelection = 2;
                        if (gradeOptions[7, 1] == 4) charmSelection = -1;
                        if (gradeOptions[7, 1] == 5) charmSelection = 0;
                        if (gradeOptions[7, 1] == 6) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 8)
                    {
                        if (gradeOptions[8, 1] == 0) charmSelection = -1;
                        if (gradeOptions[8, 1] == 1) charmSelection = 3;
                        if (gradeOptions[8, 1] == 2) charmSelection = 0;
                        if (gradeOptions[8, 1] == 3) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 9)
                    {
                        if (gradeOptions[9, 1] == 0) charmSelection = -1;
                        if (gradeOptions[9, 1] == 1) charmSelection = 3;
                        if (gradeOptions[9, 1] == 2) charmSelection = 0;
                        if (gradeOptions[9, 1] == 3) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    if (currentGrade == 10)
                    {
                        if (gradeOptions[10, 1] == 0) charmSelection = -1;
                        if (gradeOptions[10, 1] == 1) charmSelection = 3;
                        if (gradeOptions[10, 1] == 2) charmSelection = 0;
                        if (gradeOptions[10, 1] == 3) charmSelection = 2;
                        selectedCharm = charmSelection;
                    }
                    //---//
                    odds = getOdds(currentGrade, isShip, classification, selectedCharm);
                    if (odds > 10000) odds = 10000;//Can't have greater than 100% chance
                    attempts[currentGrade]++;
                    //Step 2: Apply Statistics
                    //Add the cost of a scrolls
                    if (gradeOptions[currentGrade, 0] == 1) sessionCost += prices[14];
                    else sessionCost += prices[13];
                    //Add the enchantment cost
                    sessionCost += enchantCost;
                    //Add the charm cost if applicable
                    sessionCost += getCharmCost(currentGrade);
                    cumulativeCost[currentGrade] += sessionCost;

                    //Step 3: Roll the Enchantment
                    roll = rng.Next(1, 10000) * (trino + miscModifier);
                    //Add modifier for a regrade event (Using modifiers from May 2017 Event)
                    if (isRegradeEvent && currentGrade <= 5) odds *= 1.5;
                    //Check for Success
                    if (roll <= odds)
                    {
                        //Success
                        successes[currentGrade]++;
                        trino += trinoRNG();
                        currentGrade++;
                        enchantCost += (enchantCost * .35); //This is a rough estimate to account for increasing enchanting costs
                        sessionCost = 0;
                        //Check for Great Success if using a resplendent
                        if (!isShip && gradeOptions[currentGrade, 0] == 1)
                        {
                            if (roll <= odds * .25)
                            {
                                //Great Success
                                trino += trinoRNG();
                                //Increment the Great Success count for the appropriate grade
                                if (currentGrade == 5) GSUD++;
                                if (currentGrade == 6) GSCE++;
                                if (currentGrade == 7) GSDL++;
                                if (currentGrade == 8) GSEM++;
                                if (currentGrade == 9) GSLP++;
                                currentGrade++;
                                enchantCost += (enchantCost * .35);
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
                            enchantCost = prices[15];
                            trino = 1;
                        }

                        //Roll for Celestial break or downgrade
                        if (!isShip && currentGrade == 6 && majorFail(gradeOptions[6,1], isRegradeEvent))
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = prices[15];
                            trino = 1;
                            
                        }
                        
                        else if(!isShip && currentGrade == 6 && gradeOptions[6,1] < 4)
                        {
                            //Downgrade Item
                            enchantCost = prices[15];
                            currentGrade = 3; //Downgrade
                            if (currentGrade > initialGrade)
                            {
                                for (int a = 3; a > initialGrade; a--)
                                {
                                    enchantCost *= 1.35;
                                }
                            }
                            else if (currentGrade < initialGrade)
                            {
                                for (int a = 3; a < initialGrade; a++)
                                {
                                    enchantCost /= 1.35;
                                }
                            }
                        }

                        //Break Divine if Not Anchored
                        if(currentGrade == 7 && !isShip && gradeOptions[7,1] < 4)
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = prices[15];
                            trino = 1;
                        }
                        //Break Epic and Above
                        else if (currentGrade > 7 && !isShip)
                        {
                            majorFails[currentGrade]++;
                            currentGrade = initialGrade;
                            enchantCost = prices[15];
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
        public bool majorFail(int charmSelection, bool regradeEvent)
        {
            //local variables
            bool anchored = false;
            Random rng = new Random();

            if (charmSelection >= 4 && charmSelection <= 6) anchored = true;
            if (anchored || regradeEvent) return false;
            else if (rng.Next(1, 10000) < 5000) return false;
            else return true;
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
            labelGC.Text = Math.Round(cumulativeCost[0], 1).ToString();
            labelGF.Text = majorFails[0].ToString();
            if (attempts[1] > 0) labelRA.Text = attempts[1].ToString() + " (" + Math.Round(successes[1] / attempts[1], 3) * 100 + "%)";
            else labelRA.Text = "0 (0)";
            labelRC.Text = Math.Round(cumulativeCost[1], 1).ToString();
            labelRF.Text = majorFails[1].ToString();
            if (attempts[2] > 0) labelAA.Text = attempts[2].ToString() + " (" + Math.Round(successes[2] / attempts[2], 3) * 100 + "%)";
            else labelAA.Text = "0 (0)";
            labelAC.Text = Math.Round(cumulativeCost[2], 1).ToString();
            labelAF.Text = majorFails[2].ToString();
            if (attempts[3] > 0) labelHA.Text = attempts[3].ToString() + " (" + Math.Round(successes[3] / attempts[3], 3) * 100 + "%)";
            else labelHA.Text = "0 (0)";
            labelHC.Text = Math.Round(cumulativeCost[3], 1).ToString();
            labelHF.Text = majorFails[3].ToString();
            if (attempts[4] > 0) labelUA.Text = attempts[4].ToString() + " (" + Math.Round(successes[4] / attempts[4], 3) * 100 + "%)";
            else labelUA.Text = "0 (0)";
            labelUC.Text = Math.Round(cumulativeCost[4], 1).ToString();
            labelUF.Text = majorFails[4].ToString();
            if (attempts[5] > 0) labelCA.Text = attempts[5].ToString() + " (" + Math.Round(successes[5] / attempts[5], 3) * 100 + "%)";
            else labelCA.Text = "0 (0)";
            labelCC.Text = Math.Round(cumulativeCost[5], 1).ToString();
            labelCF.Text = majorFails[5].ToString();
            if (attempts[6] > 0) labelDA.Text = attempts[6].ToString() + " (" + Math.Round(successes[6] / attempts[6] , 3) * 100 + "%)";
            else labelDA.Text = "0 (0)";
            labelDC.Text = Math.Round(cumulativeCost[6], 1).ToString();
            labelDF.Text = majorFails[6].ToString();
            if (attempts[7] > 0) labelEA.Text = attempts[7].ToString() + " (" + Math.Round(successes[7] / attempts[7], 3) * 100 + "%)";
            else labelEA.Text = "0 (0)";
            labelEC.Text = Math.Round(cumulativeCost[7], 1).ToString();
            labelEF.Text = majorFails[7].ToString();
            if (attempts[8] > 0) labelLA.Text = attempts[8].ToString() + " (" + Math.Round(successes[8] / attempts[8], 3) * 100 + "%)";
            else labelLA.Text = "0 (0)";
            labelLC.Text = Math.Round(cumulativeCost[8], 1).ToString();
            labelLF.Text = majorFails[8].ToString();
            if (attempts[9] > 0) labelMA.Text = attempts[9].ToString() + " (" + Math.Round(successes[9] / attempts[9], 3) * 100 + "%)";
            else labelMA.Text = "0 (0)";
            labelMC.Text = Math.Round(cumulativeCost[9], 1).ToString();
            labelMF.Text = majorFails[9].ToString();
            if (attempts[10] > 0) labelPA.Text = attempts[10].ToString() + " (" + Math.Round(successes[10] / attempts[10], 3) * 100 + "%)";
            else labelPA.Text = "0 (0)";
            labelPC.Text = Math.Round(cumulativeCost[10], 1).ToString();
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
        public double getOdds(int grade, bool isShip, int classification, int selectedCharm)
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
                    if (isShip && patch == 3.5)
                    {
                        return 10000 * multiplier;
                    }
                    else{
                        switch (classification)
                        {
                            case 0://Easy
                                return 10000 * multiplier;
                            case 1://Normal
                                return 10000 * multiplier;
                            case 2://Difficult
                                return 10000 * multiplier;
                            default://Should not happen
                                return 10000 * multiplier;
                        }
                    }
                case 1://Grand -> Rare
                    if (isShip && patch == 3.5)
                    {
                        return 10000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 10000 * multiplier;
                            case 1://Normal
                                return 10000 * multiplier;
                            case 2://Difficult
                                return 10000 * multiplier;
                            default://Should not happen
                                return 4000 *  multiplier;
                        }
                    }
                case 2://Rare -> Arcane
                    if (isShip && patch == 3.5)
                    {
                        return 6000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 10000 * multiplier;
                            case 1://Normal
                                return 10000 * multiplier;
                            case 2://Difficult
                                return 5000 * multiplier;
                            default://Should not happen
                                return 3000 * multiplier;
                        }
                    }
                case 3://Arcane -> Heroic
                    if (isShip && patch == 3.5)
                    {
                        return 6000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 6750 * multiplier;
                            case 1://Normal
                                return 5000 * multiplier;
                            case 2://Difficult
                                return 3250 * multiplier;
                            default://Should not happen
                            return 3000;
                        }
                    }
                case 4://Heroic -> Unique
                    if (isShip && patch == 3.5)
                    {
                        return 6000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 6750 * multiplier;
                            case 1://Normal
                                return 5000 * multiplier;
                            case 2://Difficult
                                return 3250 * multiplier;
                            default://Should not happen
                                return 2500 * multiplier;
                        }
                    }
                case 5://Unique -> Celestial
                    if (isShip && patch == 3.5)
                    {
                        return 5000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 4730 * multiplier;
                            case 1://Normal
                                return 3500 * multiplier;
                            case 2://Difficult
                                return 2280 * multiplier;
                            default://Should not happen
                                return 2000 * multiplier;
                        }
                    }
                case 6://Celestial -> Divine
                    if (isShip && patch == 3.5)
                    {
                        return 5000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 4050 * multiplier;
                            case 1://Normal
                                return 3000 * multiplier;
                            case 2://Difficult
                                return 1950 * multiplier;
                            default://Should not happen
                                return 1000 * multiplier;
                        }
                    }
                case 7://Divine -> Epic
                    if (isShip && patch == 3.5)
                    {
                        return 4000 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 1350 * multiplier;
                            case 1://Normal
                                return 1000 * multiplier;
                            case 2://Difficult
                                return 650 * multiplier;
                            default://Should not happen
                                return 750 * multiplier;
                        }
                    }
                case 8://Epic -> Legendary
                    if (isShip && patch == 3.5)
                    {
                        return 3500 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 1080 * multiplier;
                            case 1://Normal
                                return 800 * multiplier;
                            case 2://Difficult
                                return 520 * multiplier;
                            default://Should not happen
                                return 500 * multiplier;
                        }
                    }
                case 9://Legendary -> Mythic
                    if (isShip && patch == 3.5)
                    {
                        return 1750 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 410 * multiplier;
                            case 1://Normal
                                return 300 * multiplier;
                            case 2://Difficult
                                return 200 * multiplier;
                            default://Should not happen
                                return 250 * multiplier;
                        }
                    }
                case 10://Mythic -> Eternal
                    if (isShip && patch == 2.9) return 880;
                    else if (isShip && patch == 3.5)
                    {
                        return 880 * multiplier;
                    }
                    else
                    {
                        switch (classification)
                        {
                            case 0://Easy
                                return 270 * multiplier;
                            case 1://Normal
                                return 200 * multiplier;
                            case 2://Difficult
                                return 130 * multiplier;
                            default://Should not happen
                                return 0;
                        }
                    }
                default:
                    return 0;
            }
        }
        
        public double getCharmCost(int currentGrade)
        {
            //Returns the cost of the item based on the selection made in the main window
            switch (currentGrade)
            {
                case 0:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[3];
                    else return 0;
                case 1:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[3];
                    else return 0;
                case 2:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[3];
                    else return 0;
                case 3:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[0];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[3];
                    else return 0;
                case 4:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[1];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[3];
                    else return 0;
                case 5:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[2];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[3];
                    else return 0;
                case 6:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[6];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[4];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[5];
                    else if (gradeOptions[currentGrade, 1] == 4) return prices[7];
                    else if (gradeOptions[currentGrade, 1] == 5) return prices[8];
                    else if (gradeOptions[currentGrade, 1] == 6) return prices[9];
                    else return 0;
                case 7:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[6];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[4];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[5];
                    else if (gradeOptions[currentGrade, 1] == 4) return prices[10];
                    else if (gradeOptions[currentGrade, 1] == 5) return prices[11];
                    else if (gradeOptions[currentGrade, 1] == 6) return prices[12];
                    else return 0;
                case 8:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[6];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[4];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[5];
                    else return 0;
                case 9:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[6];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[4];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[5];
                    else return 0;
                case 10:
                    if (gradeOptions[currentGrade, 1] == 1) return prices[6];
                    else if (gradeOptions[currentGrade, 1] == 2) return prices[4];
                    else if (gradeOptions[currentGrade, 1] == 3) return prices[5];
                    else return 0;
                default:
                    return 0;
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
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
            if (checkBox1.Checked) setEnchCost(costs.enchantCost, false);
        }

        private void buttonStandard_Click(object sender, EventArgs e)
        {
            if(emulatorItemType != 0)
            {
                updateScrollCost(costs.regScrollCost);
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
                updateScrollCost(costs.resplendScrollCost);
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
                updateCharmCost(costs.blueCharmCost);
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
                updateCharmCost(costs.greenCharmCost);
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
                updateCharmCost(costs.yellowCharmCost);
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
                updateCharmCost(costs.redCharmCost);
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
                updateCharmCost(costs.supYellowCharmCost);
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
                updateCharmCost(costs.supRedCharmCost);
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
                updateCharmCost(costs.celestCharm10Cost);
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
                updateCharmCost(costs.silverCharmCost);
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
                updateCharmCost(costs.celestCharm15Cost);
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
                updateCharmCost(costs.celestCharm20Cost);
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
                updateCharmCost(costs.divineCharm10Cost);
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
                updateCharmCost(costs.divineCharm15Cost);
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
                updateCharmCost(costs.divineCharm20Cost);
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
                if(patch == 3.5){
                    label63.Enabled = false;
                    comboBoxClassification.Enabled = false;

                    //Reset scrolls to standard and disable the boxes
                    comboBoxBasicScroll.SelectedIndex = 0;
                    comboBoxGrandScroll.SelectedIndex = 0;
                    comboBoxArcaneScroll.SelectedIndex = 0;
                    comboBoxRareScroll.SelectedIndex = 0;
                    comboBoxHeroicScroll.SelectedIndex = 0;
                    comboBoxUniqueScroll.SelectedIndex = 0;
                    comboBoxCelestialScroll.SelectedIndex = 0;
                    comboBoxDivineScroll.SelectedIndex = 0;
                    comboBoxEpicScroll.SelectedIndex = 0;
                    comboBoxLegendaryScroll.SelectedIndex = 0;
                    comboBoxMythicScroll.SelectedIndex = 0;

                    comboBoxBasicScroll.Enabled     = false;
                    comboBoxGrandScroll.Enabled     = false;
                    comboBoxArcaneScroll.Enabled    = false;
                    comboBoxRareScroll.Enabled      = false;
                    comboBoxHeroicScroll.Enabled    = false;
                    comboBoxUniqueScroll.Enabled    = false;
                    comboBoxCelestialScroll.Enabled = false;
                    comboBoxDivineScroll.Enabled    = false;
                    comboBoxEpicScroll.Enabled      = false;
                    comboBoxLegendaryScroll.Enabled = false;
                    comboBoxMythicScroll.Enabled    = false;
                }
            }
            else
            {
                checkBoxEvent.Enabled = true;
                if (patch == 3.5)
                {
                    label63.Enabled = true;
                    comboBoxClassification.Enabled = true;
                    comboBoxBasicScroll.Enabled     = true;
                    comboBoxGrandScroll.Enabled     = true;
                    comboBoxArcaneScroll.Enabled    = true;
                    comboBoxRareScroll.Enabled      = true;
                    comboBoxHeroicScroll.Enabled    = true;
                    comboBoxUniqueScroll.Enabled    = true;
                    comboBoxCelestialScroll.Enabled = true;
                    comboBoxDivineScroll.Enabled    = true;
                    comboBoxEpicScroll.Enabled      = true;
                    comboBoxLegendaryScroll.Enabled = true;
                    comboBoxMythicScroll.Enabled    = true;
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
                costs.budget = totalGold -= currentEnchantCost;
                refreshGold();
                Random rng = new Random();
                int roll = rng.Next(0, 10000);
                if(roll > emulatorOdds)
                {
                    //Do Fail Stuff
                    roll = rng.Next(0, 10000);
                    successType = 5;
                    if (emulatorItemGrade == 7 && !emulatorIsAnchored)
                    {
                        if(roll < 5000 && emulatorItemType == 1)
                        {
                            didDegrade = true;
                            if (!checkBoxKeepItems.Checked)
                            {
                                emulatorItemGrade = 4;
                                setGrade(4);
                                resetCharm();
                                resetScroll();
                                currentEnchantCost /= 1.55;
                                currentEnchantCost /= 1.55;
                                currentEnchantCost /= 1.55;

                            }
                            successType = 3;
                        }
                        else
                        {
                            didBlowUp = true;
                            successType = 4;
                        }
                    }
                    else if(emulatorItemGrade > 7 && !emulatorIsAnchored)
                    {
                        successType = 4;
                        didBlowUp = true;
                    }
                }
                else
                {
                    didFail = false;
                    if(roll < emulatorOdds * 0.2 && emulatorItemGrade != 11 && emulatorScrollType == 2)
                    {
                        emulatorItemGrade += 2;
                        setGrade(emulatorItemGrade);
                        successType = 2;
                    }
                    else
                    {
                        emulatorItemGrade++;
                        setGrade(emulatorItemGrade);
                        successType = 1;
                    }
                }
                if (!didFail)
                {
                    resetCharm();
                    currentEnchantCost -= scrollCost;
                    scrollCost = 0;
                    setEnchCost(costs.enchantCost, true);
                    if (successType == 2) setEnchCost(costs.enchantCost, true);
                }
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
                    setEnchCost(0, false);
                }
            }
        }

        //Updates the progress bar when progress has been reported by the thread
        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
        }

        private void buttonSetCosts_Click(object sender, EventArgs e)
        {
            costs.ShowDialog();
        }

        //Sets up the thread and starts the enchanting sequence
        public void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //local variables
            
            
            int iterations;
            bool isShip = checkBoxShip.Checked;
            //Initialize global variables from UI elements
            prices[0] = costs.greenCharmCost;
            prices[1] = costs.blueCharmCost;
            prices[2] = costs.yellowCharmCost;
            prices[3] = costs.redCharmCost;
            prices[4] = costs.supYellowCharmCost;
            prices[5] = costs.supRedCharmCost;
            prices[6] = costs.silverCharmCost;
            prices[7] = costs.celestCharm10Cost;
            prices[8] = costs.celestCharm15Cost;
            prices[9] = costs.celestCharm20Cost;
            prices[10] = costs.divineCharm10Cost;
            prices[11] = costs.divineCharm15Cost;
            prices[12] = costs.divineCharm20Cost;
            prices[13] = costs.regScrollCost;
            prices[14] = costs.resplendScrollCost;
            prices[15] = costs.enchantCost;
            
            iterations = int.Parse(textBoxIterations.Text);
            //Start the enchanting function
            doEnchant(iterations, currentGrade, targetGrade, isRegradeEvent, isShip, selectedCharm, prices, gradeOptions);
            isDoneEnchanting = true;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                panelCurrentGold.Visible = true;
                checkGold = true;
                buttonRefresh.Visible = true;
                buttonSetCost2.Visible = true;
                panelEnchCost.Visible = true;
                checkBoxKeepItems.Checked = false;
                checkBoxKeepItems.Enabled = false;
                MessageBox.Show("The Keep Items/Scrolls feature will be disabled for this mode. Set your budget and item costs by pressing the \"Set Costs\" button.");
            }
            else
            {
                panelCurrentGold.Visible = false;
                checkGold = false;
                buttonRefresh.Visible = false;
                buttonSetCost2.Visible = false;
                panelEnchCost.Visible = false;
                checkBoxKeepItems.Enabled = true;
            }
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

        private void buttonSetCost2_Click(object sender, EventArgs e)
        {
            costs.ShowDialog();
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

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            totalGold = costs.budget;
            refreshGold();
            eval();
        }

        private void pictureBoxCharm_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                resetCharm();
                eval();
            };
        }

        private void pictureBoxScroll_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                resetScroll();
                eval();
            }
        }

        private void pictureBoxItem_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                resetEmulator();
                setEnchCost(0, false);
            }
        }

        //Resets the emulator
        public void resetEmulator()
        {
            emulatorItemType = 0;
            emulatorItemGrade = 0;
            emulatorScrollType = 0;
            emulatorCharmType = 1;
            currentEnchantCost = 0;
            scrollCost = 0;
            charmCost = 0;
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
            setEnchCost(0, false);
        }

        //resets the scroll
        public void resetScroll()
        {
            pictureBoxScroll.BackgroundImage = Properties.Resources.scroll_initial;
            pictureBoxScroll.Image = null;
            emulatorScrollType = 0;
            currentEnchantCost -= scrollCost;
            setEnchCost(currentEnchantCost, false);
            scrollCost = 0;
        }
        //resets the emulator charm
        public void resetCharm()
        {
            emulatorCharmType = 1;
            pictureBoxCharm.BackgroundImage = Properties.Resources.charm_initial;
            pictureBoxCharm.Image = null;
            emulatorIsAnchored = false;
            currentEnchantCost -= charmCost;
            setEnchCost(currentEnchantCost, false);
            charmCost = 0;
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
            emulatorOdds = getOdds((emulatorItemGrade - 1), isShip, emulatorClass, selectedCharm);
            if (emulatorOdds > 10000) emulatorOdds = 10000;
            labelChancePercent.Text = Math.Round(emulatorOdds / 100, 2).ToString() + "%";

            //Evaluate if enchantment is ready
            if(checkGold && currentEnchantCost > totalGold)
            {
                pictureBoxEnchant.Image = Properties.Resources.button_enchant_disable;
                emulatorIsReady = false;
            }
            else if(emulatorItemType != 0 && emulatorScrollType != 0 && emulatorItemGrade != 12)
            {
                pictureBoxEnchant.Image = Properties.Resources.button_enchant_enable;
                emulatorIsReady = true;
            }
            else
            {
                pictureBoxEnchant.Image = Properties.Resources.button_enchant_disable;
                emulatorIsReady = false;
            }
            refreshGold();
        }

        public void setEnchCost(double cost, bool upgrade)
        {
            if (upgrade)
            {
                if(emulatorItemType == 1)currentEnchantCost = currentEnchantCost * 1.55;
            }
            else
            {
                currentEnchantCost = cost;
            }
            double remainder = Math.Round(currentEnchantCost, 4);
            Console.WriteLine(remainder);
            double gold = (int)Math.Floor(currentEnchantCost);
            remainder -= gold;
            double silver = (int)Math.Floor(remainder * 100);
            remainder -= (silver / 100);
            double copper = (int)Math.Ceiling(remainder * 10000);
            remainder -= copper / 10000;
            if(silver == 100)
            {
                silver = 0;
                gold++;
            }
            if(copper == 100)
            {
                copper = 0;
                silver++;
            }
            labelEnchGold.Text = gold.ToString();
            labelEnchSilver.Text = silver.ToString();
            labelEnchCopper.Text = copper.ToString();
        }

        public void refreshGold()
        {
            totalGold = costs.budget;
            double remainder = totalGold;
            double gold = (int)Math.Floor(totalGold);
            remainder -= gold;
            double silver = (int)Math.Floor(remainder * 100);
            remainder -= (silver / 100);
            double copper = (int)Math.Floor(remainder * 10000);
            remainder -= copper / 10000;
            if (silver == 100)
            {
                silver = 0;
                gold++;
            }
            if (copper == 100)
            {
                copper = 0;
                silver++;
            }
            labelGold.Text = gold.ToString();
            labelSilver.Text = silver.ToString();
            labelCopper.Text = copper.ToString();
        }

        public void updateScrollCost(double cost)
        {
            currentEnchantCost -= scrollCost;
            scrollCost = cost;
            currentEnchantCost += scrollCost;
            setEnchCost(currentEnchantCost, false);
        }
        
        public void updateCharmCost(double cost)
        {
            currentEnchantCost -= charmCost;
            charmCost = cost;
            currentEnchantCost += charmCost;
            setEnchCost(currentEnchantCost, false);
        }
    }
}
