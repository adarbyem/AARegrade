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
            public bool canButcher;
            public void setValues(string n, double s, int v, int min, int max, int l, int f, bool b, bool butcher)
            {
                name = n;
                silver = s;
                vocation = v;
                minHarv = min;
                maxHarv = max;
                labor = l;
                family = f;
                hasBundle = b;
                canButcher = butcher;
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

            //Livestock Declarations
            crop chicken        = new crop();
            crop duck           = new crop();
            crop goose          = new crop();
            crop turkey         = new crop();
            crop pig            = new crop();
            crop sheep          = new crop();
            crop goat           = new crop();
            crop cow            = new crop();
            crop buffalo        = new crop();
            crop yata           = new crop();
            crop bear           = new crop();

            //Livestock Initialization
            chicken.setValues("Chicken", 10, 0, 2, 4, 5, 4, false, true);
            duck.setValues("Duck", 10, 0, 2, 4, 5, 4, false, true);
            goose.setValues("Goose", 10, 0, 2, 4, 5, 4, false, true);
            turkey.setValues("Turkey", 10, 0, 4, 6, 15, 4, false, true);
            pig.setValues("Pig", 50, 0, 16, 20, 20, 4, false, true);
            sheep.setValues("Sheep", 50, 0, 17, 19, 10, 4, false, true);
            goat.setValues("Goat", 50, 0, 7, 9, 10, 4, false, true);
            cow.setValues("Cow", 0, 450, 7, 9, 25, 4, false, true);
            buffalo.setValues("Water Buffalo", 0, 450, 16, 20, 25, 4, false, true);
            yata.setValues("Yata", 0, 1125, 7, 9, 15, 4, false, true);
            bear.setValues("Blizzard Bear", 0, 1125, 8, 10, 25, 4, false, true);

            //Aqualfarm Crops Initialization
            redCoral.setValues("Red Coral Polyp", 7, 0, 3, 4, 2, 0, false, false);
            greenCoral.setValues("Green Coral Polyp", 7, 0, 3, 4, 2, 0, false, false);
            orangeCoral.setValues("Orange Plate Coral Polyp", 11, 0, 2, 4, 3, 0, false, false);
            yellowCoral.setValues("Yellow Plate Coral Polyp", 11, 0, 2, 4, 3, 0, false, false);
            antlerCoral.setValues("Antler Coral Polyp", 0, 45, 3, 4, 4, 0, false, false);
            whiteCoral.setValues("White Coral Polyp", 0, 45, 3, 4, 4, 0, false, false);
            blueCoral.setValues("Blue Coral Polyp", 0, 45, 3, 4, 4, 0, false, false);
            pearlOyster.setValues("Pearl Oyster", 0, 45, 2, 2, 5, 0, false, false);

            //Tree Crops Initialization
            appleTree.setValues("Apple Sapling", 11, 0, 3, 4, 10, 2, false, false);
            ashTree.setValues("Ash Sapling", 0, 300, 12, 12, 25, 5, false, false);
            willowTree.setValues("Willow Sapling", 0, 300, 6, 7, 10, 5, false, false);
            hornbeamTree.setValues("Hornbeam Sapling", 7, 0, 6, 8, 25, 5, false, false);
            oliveTree.setValues("Olive Sapling", 0, 225, 3, 4, 15, 2, false, false);
            corkTree.setValues("Cork Sapling", 4, 0, 5, 6, 25, 5, false, false);
            aspenTree.setValues("Aspen Sapling", 0, 225, 6, 7, 10, 5, false, false);
            grapeTree.setValues("Grapevine Sapling", 4, 0, 3, 4, 5, 2, false, false);
            lemonTree.setValues("Lemon Sapling", 11, 0, 3, 4, 10, 2, false, false);
            figTree.setValues("Fig Sapling", 11, 0, 3, 4, 10, 2, false, false);
            rubberTree.setValues("Rubber Sapling", 0, 300, 3, 6, 10, 5, false, false);
            bambooTree.setValues("Bamboo Sapling", 0, 255, 9, 10, 10, 5, false, false);
            pineTree.setValues("Pine Sapling", 0, 300, 13, 13, 20, 5, false, false);
            juniperTree.setValues("Juniper Sapling", 0, 225, 10, 10, 10, 5, false, false);
            firTree.setValues("Fir Sapling", 0, 225, 10, 10, 10, 5, false, false);
            cedarTree.setValues("Cedar Sapling", 16, 0, 7, 7, 10, 5, false, false);
            yewTree.setValues("Yew Sapling", 4, 0, 2, 4, 10, 5, false, false);
            pomTree.setValues("Pomegranate Sapling", 0, 225, 3, 4, 20, 2, false, false);
            cherryTree.setValues("Cherry Sapling", 0, 300, 3, 4, 25, 2, false, false);
            orangeTree.setValues("Orange Sapling", 0, 225, 3, 4, 20, 2, false, false);
            moringaTree.setValues("Moringa Sapling", 0, 300, 2, 4, 25, 2, false, false);
            ginkgoTree.setValues("Ginkgo Sapling", 0, 225, 4, 5, 15, 6, false, false);
            mapleTree.setValues("Maple Sapling", 0, 300, 8, 10, 10, 5, false, false);
            avacadoTree.setValues("Avacado Sapling", 4, 0, 2, 3, 10, 2, false, false);
            bayTree.setValues("Bay Sapling", 0, 225, 3, 4, 5, 6, false, false);
            cypressTree.setValues("Cypress Sapling", 20, 0, 12, 14, 25, 5, false, false);
            oakTree.setValues("Oak Sapling", 20, 0, 4, 4, 20, 2, false, false);
            ebonyTree.setValues("Ebony Sapling", 20, 0, 12, 14, 25, 5, false, false);
            chestnutTree.setValues("Chestnut Sapling", 20, 0, 9, 9, 25, 2, false, false);
            camphorTree.setValues("Camphor Sapling", 20, 0, 12, 15, 20, 5, false, false);
            apricotTree.setValues("Apricot Sapling", 20, 0, 6, 7, 25, 2, false, false);
            bananaTree.setValues("Banana Sapling", 16, 0, 2, 4, 15, 2, false, false);
            poplarTree.setValues("Poplar Sapling", 20, 0, 4, 5, 10, 5, false, false);
            jujubeTree.setValues("Jujube Sapling", 0, 300, 2, 4, 20, 2, false, false);
            beechTree.setValues("Beech Sapling", 20, 0, 4, 4, 20, 2, false, false);
            spruceTree.setValues("Spruce Sapling", 0, 225, 9, 10, 20, 5, false, false);

            //Seeds Initialization
            barleySeed.setValues("Barley Seed", 1.5, 0, 2, 4, 1, 3, true, false);
            beanSeed.setValues("Bean Seed", 0, 90, 3, 4, 3, 3, true, false);
            carrotSeed.setValues("Carrot Seed", 1.5, 0, 2, 4, 1, 8, true, false);
            chiliSeed.setValues("Chili Seed", 0, 90, 2, 4, 3, 8, true, false);
            cornSeed.setValues("Corn Seed", 1.5, 0, 2, 4, 1, 3, true, false);
            cucumberSeed.setValues("Cucumber Seed", 1.5, 0, 2, 4, 1, 8, true, false);
            garlicSeed.setValues("Garlic Seed", 1.5, 0, 2, 4, 2, 8, true, false);
            milletSeed.setValues("Millet Seed", 1.5, 0, 2, 4, 1, 3, true, false);
            oatSeed.setValues("Oat Seed", 0, 45, 2, 4, 2, 3, true, false);
            onionSeed.setValues("Onion Seed", 1.5, 0, 2, 4, 1, 8, true, false);
            peanutSeed.setValues("Peanut Seed", 0, 45, 2, 4, 2, 3, true, false);
            potatoSeed.setValues("Potato Seed", 1.5, 0, 2, 4, 1, 8, true, false);
            pumpkinSeed.setValues("Pumpkin Seed", 0, 45, 2, 4, 2, 8, true, false);
            quinoaSeed.setValues("Quinoa Seed", 0, 90, 3, 4, 3, 3, true, false);
            riceSeed.setValues("Rice Seed", 1.5, 0, 2, 4, 1, 3, true, false);
            ryeSeed.setValues("Rye Seed", 0, 45, 2, 4, 2, 3, true, false);
            strawberrySeed.setValues("Strawberry Seed", 0, 45, 2, 4, 2, 8, true, false);
            tomatoSeed.setValues("Tomato Seed", 1.5, 0, 2, 4, 2, 8, true, false);
            wheatSeed.setValues("Wheat Seed", 0, 45, 2, 4, 2, 3, true, false);
            yamSeed.setValues("Yam Seed", 0, 45, 2, 4, 2, 8, true, false);

            //Flowers Initialization
            azalea.setValues("Azalea Seed", 0, 45, 3, 4, 2, 1, true, false);
            cactus.setValues("Cactus Seed", 0, 90, 2, 4, 2, 1, true, false);
            clover.setValues("Clover Seed", 1.1, 0, 2, 4, 1, 1, true, false);
            cornflower.setValues("Cornflower Seed", 0, 45, 2, 4, 2, 1, true, false);
            cotton.setValues("Cotton Seed", 1.4, 0, 7, 10, 2, 1, true, false);
            lotus.setValues("Lotus Seed", 0, 45, 2, 4, 2, 1, true, false);
            narcissus.setValues("Narcissus Seed", 1.1, 0, 2, 4, 1, 1, true, false);
            rose.setValues("Rose Seed", 1.1, 0, 2, 4, 1, 1, true, false);

            //Medicinal Herbs Initialization
            aloe.setValues("Aloe Seed", 0, 45, 3, 4, 2, 7, true, false);
            ginseng.setValues("Cultivated Ginseng", 0, 90, 2, 4, 3, 7, true, false);
            thistle.setValues("Thistle Seed", 4, 0, 2, 4, 1, 7, true, false);
            mushroom.setValues("Mushroom Seed", 0, 45, 2, 4, 1, 7, true, false);

            //Spices Initialization
            iris.setValues("Iris Seed", 1.1, 0, 2, 4, 1, 9, true, false);
            lavender.setValues("Lavender Seed", 1.5, 0, 2, 4, 1, 9, true, false);
            mint.setValues("Mint Seed", 0, 45, 2, 4, 2, 9, true, false);
            poppy.setValues("Poppy Seed", 0, 90, 2, 4, 5, 9, true, false);
            saffron.setValues("Saffron Seed", 0, 90, 3, 4, 5, 9, true, false);
            sunflower.setValues("Sunflower Seed", 0, 45, 2, 4, 3, 9, true, false);
            turmeric.setValues("Termeric Seed", 0, 90, 2, 4, 3, 9, true, false);

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
            livestock.Add(chicken);
            livestock.Add(duck);
            livestock.Add(goose);
            livestock.Add(turkey);
            livestock.Add(pig);
            livestock.Add(sheep);
            livestock.Add(goat);
            livestock.Add(cow);
            livestock.Add(buffalo);
            livestock.Add(yata);
            livestock.Add(bear);

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
