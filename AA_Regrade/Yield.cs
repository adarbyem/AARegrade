using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AA_Regrade
{
    class Yield
    {
        //Crop Structure
        public struct crop
        {
            public string name;
            public double silver;
            public int vocation;
            public int minHarv;
            public int maxHarv;
            public int labor;
            public int family;
            public bool hasBundle;
            public void setValues(string n, double s, int v, int min, int max, int l, int f, bool b)
            {
                name = n;
                silver = s;
                vocation = v;
                minHarv = min;
                maxHarv = max;
                labor = l;
                family = f;
                hasBundle = b;
            }
        }

        public List<crop> aquafarm;
        public List<crop> flowers;
        public List<crop> fruitTrees;
        public List<crop> grains;
        public List<crop> livestock;
        public List<crop> logTrees;
        public List<crop> medicinalHerbs;
        public List<crop> medicinalTrees;
        public List<crop> produce;
        public List<crop> spices;

        //Method to generate crops
        public void generateCrops()
        {
            //Aquafarm Crops Declarations
            crop redCoral       = new crop();
            crop greenCoral     = new crop();
            crop orangeCoral    = new crop();
            crop yellowCoral    = new crop();
            crop antlerCoral    = new crop();
            crop whiteCoral     = new crop();
            crop blueCoral      = new crop();
            crop pearlOyster    = new crop();

            //Trees Declarations
            crop appleTree      = new crop();
            crop ashTree        = new crop();
            crop willowTree     = new crop();
            crop hornbeamTree   = new crop();
            crop oliveTree      = new crop();
            crop corkTree       = new crop();
            crop aspenTree      = new crop();
            crop grapeTree      = new crop();
            crop lemonTree      = new crop();
            crop figTree        = new crop();
            crop rubberTree     = new crop();
            crop bambooTree     = new crop();
            crop pineTree       = new crop();
            crop juniperTree    = new crop();
            crop firTree        = new crop();
            crop cedarTree      = new crop();
            crop yewTree        = new crop();
            crop pomTree        = new crop();
            crop cherryTree     = new crop();
            crop orangeTree     = new crop();
            crop moringaTree    = new crop();
            crop ginkgoTree     = new crop();
            crop mapleTree      = new crop();
            crop avacadoTree    = new crop();
            crop bayTree        = new crop();
            crop cypressTree    = new crop();
            crop oakTree        = new crop();
            crop ebonyTree      = new crop();
            crop chestnutTree   = new crop();
            crop camphorTree    = new crop();
            crop apricotTree    = new crop();
            crop bananaTree     = new crop();
            crop poplarTree     = new crop();
            crop jujubeTree     = new crop();
            crop beechTree      = new crop();
            crop spruceTree     = new crop();

            //Seeds Declarations
            crop barleySeed     = new crop();
            crop beanSeed       = new crop();
            crop carrotSeed     = new crop();
            crop chiliSeed      = new crop();
            crop cornSeed       = new crop();
            crop cucumberSeed   = new crop();
            crop garlicSeed     = new crop();
            crop milletSeed     = new crop();
            crop oatSeed        = new crop();
            crop onionSeed      = new crop();
            crop peanutSeed     = new crop();
            crop potatoSeed     = new crop();
            crop pumpkinSeed    = new crop();
            crop quinoaSeed     = new crop();
            crop riceSeed       = new crop();
            crop ryeSeed        = new crop();
            crop strawberrySeed = new crop();
            crop tomatoSeed     = new crop();
            crop wheatSeed      = new crop();
            crop yamSeed        = new crop();

            //Flowers Declarations
            crop azalea          = new crop();
            crop cactus          = new crop();
            crop clover          = new crop();
            crop cornflower      = new crop();
            crop cotton          = new crop();
            crop lotus           = new crop();
            crop narcissus       = new crop();
            crop rose            = new crop();

            //Medicinal Herbs Declarations
            crop aloe           = new crop();
            crop ginseng        = new crop();
            crop thistle        = new crop();
            crop mushroom       = new crop();

            //Spices Declarations
            crop iris           = new crop();
            crop lavender       = new crop();
            crop mint           = new crop();
            crop poppy          = new crop();
            crop saffron        = new crop();
            crop sunflower      = new crop();
            crop turmeric       = new crop();
            
            //Aqualfarm Crops Initialization
            redCoral.setValues("Red Coral Polyp", 7, 0, 3, 4, 2, 0, false);
            greenCoral.setValues("Green Coral Polyp", 7, 0, 3, 4, 2, 0, false);
            orangeCoral.setValues("Orange Plate Coral Polyp", 11, 0, 2, 4, 3, 0, false);
            yellowCoral.setValues("Yellow Plate Coral Polyp", 11, 0, 2, 4, 3, 0, false);
            antlerCoral.setValues("Antler Coral Polyp", 0, 45, 3, 4, 4, 0, false);
            whiteCoral.setValues("White Coral Polyp", 0, 45, 3, 4, 4, 0, false);
            blueCoral.setValues("Blue Coral Polyp", 0, 45, 3, 4, 4, 0, false);
            pearlOyster.setValues("Pearl Oyster", 0, 45, 2, 2, 5, 0, false);

            //Tree Crops Initialization
            appleTree.setValues("Apple Sapling", 11, 0, 3, 4, 10, 2, false);
            ashTree.setValues("Ash Sapling", 0, 300, 12, 12, 25, 5, false);
            willowTree.setValues("Willow Sapling", 0, 300, 6, 7, 10, 5, false);
            hornbeamTree.setValues("Hornbeam Sapling", 7, 0, 6, 8, 25, 5, false);
            oliveTree.setValues("Olive Sapling", 0, 225, 3, 4, 15, 2, false);
            corkTree.setValues("Cork Sapling", 4, 0, 5, 6, 25, 5, false);
            aspenTree.setValues("Aspen Sapling", 0, 225, 6, 7, 10, 5, false);
            grapeTree.setValues("Grapevine Sapling", 4, 0, 3, 4, 5, 2, false);
            lemonTree.setValues("Lemon Sapling", 11, 0, 3, 4, 10, 2, false);
            figTree.setValues("Fig Sapling", 11, 0, 3, 4, 10, 2, false);
            rubberTree.setValues("Rubber Sapling", 0, 300, 3, 6, 10, 5, false);
            bambooTree.setValues("Bamboo Sapling", 0, 255, 9, 10, 10, 5, false);
            pineTree.setValues("Pine Sapling", 0, 300, 13, 13, 20, 5, false);
            juniperTree.setValues("Juniper Sapling", 0, 225, 10, 10, 10, 5, false);
            firTree.setValues("Fir Sapling", 0, 225, 10, 10, 10, 5, false);
            cedarTree.setValues("Cedar Sapling", 16, 0, 7, 7, 10, 5, false);
            yewTree.setValues("Yew Sapling", 4, 0, 2, 4, 10, 5, false);
            pomTree.setValues("Pomegranate Sapling", 0, 225, 3, 4, 20, 2, false);
            cherryTree.setValues("Cherry Sapling", 0, 300, 3, 4, 25, 2, false);
            orangeTree.setValues("Orange Sapling", 0, 225, 3, 4, 20, 2, false);
            moringaTree.setValues("Moringa Sapling", 0, 300, 2, 4, 25, 2, false);
            ginkgoTree.setValues("Ginkgo Sapling", 0, 225, 4, 5, 15, 6, false);
            mapleTree.setValues("Maple Sapling", 0, 300, 8, 10, 10, 5, false);
            avacadoTree.setValues("Avacado Sapling", 4, 0, 2, 3, 10, 2, false);
            bayTree.setValues("Bay Sapling", 0, 225, 3, 4, 5, 6, false);
            cypressTree.setValues("Cypress Sapling", 20, 0, 12, 14, 25, 5, false);
            oakTree.setValues("Oak Sapling", 20, 0, 4, 4, 20, 2, false);
            ebonyTree.setValues("Ebony Sapling", 20, 0, 12, 14, 25, 5, false);
            chestnutTree.setValues("Chestnut Sapling", 20, 0, 9, 9, 25, 2, false);
            camphorTree.setValues("Camphor Sapling", 20, 0, 12, 15, 20, 5, false);
            apricotTree.setValues("Apricot Sapling", 20, 0, 6, 7, 25, 2, false);
            bananaTree.setValues("Banana Sapling", 16, 0, 2, 4, 15, 2, false);
            poplarTree.setValues("Poplar Sapling", 20, 0, 4, 5, 10, 5, false);
            jujubeTree.setValues("Jujube Sapling", 0, 300, 2, 4, 20, 2, false);
            beechTree.setValues("Beech Sapling", 20, 0, 4, 4, 20, 2, false);
            spruceTree.setValues("Spruce Sapling", 0, 225, 9, 10, 20, 5, false);

            //Seeds Initialization
            barleySeed.setValues("Barley Seed", 1.5, 0, 2, 4, 1, 3, true);
            beanSeed.setValues("Bean Seed", 0, 90, 3, 4, 3, 3, true);
            carrotSeed.setValues("Carrot Seed", 1.5, 0, 2, 4, 1, 8, true);
            chiliSeed.setValues("Chili Seed", 0, 90, 2, 4, 3, 8, true);
            cornSeed.setValues("Corn Seed", 1.5, 0, 2, 4, 1, 3, true);
            cucumberSeed.setValues("Cucumber Seed", 1.5, 0, 2, 4, 1, 8, true);
            garlicSeed.setValues("Garlic Seed", 1.5, 0, 2, 4, 2, 8, true);
            milletSeed.setValues("Millet Seed", 1.5, 0, 2, 4, 1, 3, true);
            oatSeed.setValues("Oat Seed", 0, 45, 2, 4, 2, 3, true);
            onionSeed.setValues("Onion Seed", 1.5, 0, 2, 4, 1, 8, true);
            peanutSeed.setValues("Peanut Seed", 0, 45, 2, 4, 2, 3, true);
            potatoSeed.setValues("Potato Seed", 1.5, 0, 2, 4, 1, 8, true);
            pumpkinSeed.setValues("Pumpkin Seed", 0, 45, 2, 4, 2, 8, true);
            quinoaSeed.setValues("Quinoa Seed", 0, 90, 3, 4, 3, 3, true);
            riceSeed.setValues("Rice Seed", 1.5, 0, 2, 4, 1, 3, true);
            ryeSeed.setValues("Rye Seed", 0, 45, 2, 4, 2, 3, true);
            strawberrySeed.setValues("Strawberry Seed", 0, 45, 2, 4, 2, 8, true);
            tomatoSeed.setValues("Tomato Seed", 1.5, 0, 2, 4, 2, 8, true);
            wheatSeed.setValues("Wheat Seed", 0, 45, 2, 4, 2, 3, true);
            yamSeed.setValues("Yam Seed", 0, 45, 2, 4, 2, 8, true);

            //Flowers Initialization
            azalea.setValues("Azalea Seed", 0, 45, 3, 4, 2, 1, true);
            cactus.setValues("Cactus Seed", 0, 90, 2, 4, 2, 1, true);
            clover.setValues("Clover Seed", 1.1, 0, 2, 4, 1, 1, true);
            cornflower.setValues("Cornflower Seed", 0, 45, 2, 4, 2, 1, true);
            cotton.setValues("Cotton Seed", 1.4, 0, 7, 10, 2, 1, true);
            lotus.setValues("Lotus Seed", 0, 45, 2, 4, 2, 1, true);
            narcissus.setValues("Narcissus Seed", 1.1, 0, 2, 4, 1, 1, true);
            rose.setValues("Rose Seed", 1.1, 0, 2, 4, 1, 1, true);

            //Medicinal Herbs Initialization
            aloe.setValues("Aloe Seed", 0, 45, 3, 4, 2, 7, true);
            ginseng.setValues("Cultivated Ginseng", 0, 90, 2, 4, 3, 7, true);
            thistle.setValues("Thistle Seed", 4, 0, 2, 4, 1, 7, true);
            mushroom.setValues("Mushroom Seed", 0, 45, 2, 4, 1, 7, true);

            //Spices Initialization
            iris.setValues("Iris Seed", 1.1, 0, 2, 4, 1, 9, true);
            lavender.setValues("Lavender Seed", 1.5, 0, 2, 4, 1, 9, true);
            mint.setValues("Mint Seed", 0, 45, 2, 4, 2, 9, true);
            poppy.setValues("Poppy Seed", 0, 90, 2, 4, 5, 9, true);
            saffron.setValues("Saffron Seed", 0, 90, 3, 4, 5, 9, true);
            sunflower.setValues("Sunflower Seed", 0, 45, 2, 4, 3, 9, true);
            turmeric.setValues("Termeric Seed", 0, 90, 2, 4, 3, 9, true);

            //Create lists of families
            aquafarm = new List<crop>();
            aquafarm.Add(redCoral);
            aquafarm.Add(greenCoral);
            aquafarm.Add(redCoral);
            aquafarm.Add(greenCoral);
            aquafarm.Add(orangeCoral);
            aquafarm.Add(yellowCoral);
            aquafarm.Add(antlerCoral);
            aquafarm.Add(whiteCoral);
            aquafarm.Add(blueCoral);
            aquafarm.Add(pearlOyster);
            
            flowers = new List<crop>();
            flowers.Add(azalea);
            flowers.Add(cactus);
            flowers.Add(clover);
            flowers.Add(cornflower);
            flowers.Add(cotton);
            flowers.Add(lotus);
            flowers.Add(narcissus);
            flowers.Add(rose);

            fruitTrees = new List<crop>();
            fruitTrees.Add(appleTree);
            fruitTrees.Add(oliveTree);
            fruitTrees.Add(grapeTree);
            fruitTrees.Add(lemonTree);
            fruitTrees.Add(figTree);
            fruitTrees.Add(pomTree);
            fruitTrees.Add(cherryTree);
            fruitTrees.Add(orangeTree);
            fruitTrees.Add(moringaTree);
            fruitTrees.Add(avacadoTree);
            fruitTrees.Add(oakTree);
            fruitTrees.Add(chestnutTree);
            fruitTrees.Add(apricotTree);
            fruitTrees.Add(bambooTree);
            fruitTrees.Add(jujubeTree);
            fruitTrees.Add(beechTree);

            grains = new List<crop>();
            grains.Add(barleySeed);
            grains.Add(beanSeed);
            grains.Add(cornSeed);
            grains.Add(milletSeed);
            grains.Add(oatSeed);
            grains.Add(peanutSeed);
            grains.Add(quinoaSeed);
            grains.Add(riceSeed);
            grains.Add(ryeSeed);
            grains.Add(wheatSeed);

            livestock = new List<crop>();

            logTrees = new List<crop>();
            logTrees.Add(ashTree);
            logTrees.Add(willowTree);
            logTrees.Add(hornbeamTree);
            logTrees.Add(corkTree);
            logTrees.Add(aspenTree);
            logTrees.Add(rubberTree);
            logTrees.Add(pineTree);
            logTrees.Add(juniperTree);
            logTrees.Add(firTree);
            logTrees.Add(cedarTree);
            logTrees.Add(yewTree);
            logTrees.Add(mapleTree);
            logTrees.Add(cypressTree);
            logTrees.Add(ebonyTree);
            logTrees.Add(camphorTree);
            logTrees.Add(poplarTree);
            logTrees.Add(spruceTree);

            medicinalTrees = new List<crop>();
            medicinalTrees.Add(ginkgoTree);
            medicinalTrees.Add(bayTree);

            medicinalHerbs = new List<crop>();
            medicinalHerbs.Add(aloe);
            medicinalHerbs.Add(ginseng);
            medicinalHerbs.Add(thistle);
            medicinalHerbs.Add(mushroom);

            produce = new List<crop>();
            produce.Add(carrotSeed);
            produce.Add(chiliSeed);
            produce.Add(cucumberSeed);
            produce.Add(garlicSeed);
            produce.Add(onionSeed);
            produce.Add(potatoSeed);
            produce.Add(pumpkinSeed);
            produce.Add(strawberrySeed);
            produce.Add(tomatoSeed);
            produce.Add(yamSeed);

            spices = new List<crop>();
            spices.Add(iris);
            spices.Add(lavender);
            spices.Add(mint);
            spices.Add(poppy);
            spices.Add(saffron);
            spices.Add(sunflower);
            spices.Add(turmeric);

        }

        public List<crop> getCrops(int family)
        {
            /*
                Aquafarm = 0
                Flowers = 1
                Fruit Trees = 2
                Grains = 3
                Livestock = 4
                Log Trees = 5 
                Medicinal Trees = 6
                Medicinal Herbs = 7
                Produce = 8
                Spices = 9
            */
            switch (family)
            {
                case 0:
                    return aquafarm;
                case 1:
                    return flowers;
                case 2:
                    return fruitTrees;
                case 3:
                    return grains;
                case 4:
                    return livestock;
                case 5:
                    return logTrees;
                case 6:
                    return medicinalTrees;
                case 7:
                    return medicinalHerbs;
                case 8:
                    return produce;
                case 9:
                    return spices;
            }
            return null;
        } 
    }
}
