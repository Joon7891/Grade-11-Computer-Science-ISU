//Name: Joon Song
//File Name: EnemyResources.cs
//Project Name: ISU_Song
//Creation Date: Janurary 7, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for enemy object resources

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Enemies
{
    class EnemyResources
    {
        //2D Arrays of Texture2D to hold enemy images    
        public static Texture2D[,] knightImg = new Texture2D[4, 9];
        public static Texture2D[,] archerImg = new Texture2D[4, 12];
        public static Texture2D[,] banditImg = new Texture2D[4, 7];
        public static Texture2D[,] skeletonImg = new Texture2D[4, 4];
        public static Texture2D[,] goblinImg = new Texture2D[4, 7];
        public static Texture2D[,] princeImg = new Texture2D[4, 4];
        public static Texture2D[,] wizardImg = new Texture2D[4, 16];
        public static Texture2D[,] fairyImg = new Texture2D[4, 4];
        public static Texture2D[,] witchImg = new Texture2D[4, 4];
        public static Texture2D[,] dragonImg = new Texture2D[4, 4];

        //Pre: 'Content', the content manager required to load content
        //Post: Various enemy components are loaded and setup
        //Description: Subprogram to load and setup various enemy components 
        public static void Load(ContentManager Content)
        {
            //Importing knight images
            for(int i = 0; i < 9; i++)
            {
                knightImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Knight/Up/knightUp" + (i + 1));
                knightImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Knight/Right/knightRight" + (i + 1));
                knightImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Knight/Down/knightDown" + (i + 1));
                knightImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Knight/Left/knightLeft" + (i + 1));
            }

            //Importing archer images
            for(int i = 0; i < 12; i++)
            {
                archerImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Archer/Up/archerUp" + (i + 1));
                archerImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Archer/Right/archerRight" + (i + 1));
                archerImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Archer/Down/archerDown" + (i + 1));
                archerImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Archer/Left/archerLeft" + (i + 1));
            }

            //Importing bandit images
            for (int i = 0; i < 7; i++)
            {
                banditImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Bandit/Up/banditUp" + (i + 1));
                banditImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Bandit/Right/banditRight" + (i + 1));
                banditImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Bandit/Down/banditDown" + (i + 1));
                banditImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Bandit/Left/banditLeft" + (i + 1));
            }

            //Importing skeleton images
            for (int i = 0; i < 4; i++)
            {
                skeletonImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Skeleton/Up/skeletonUp" + (i + 1));
                skeletonImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Skeleton/Right/skeletonRight" + (i + 1));
                skeletonImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Skeleton/Down/skeletonDown" + (i + 1));
                skeletonImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Skeleton/Left/skeletonLeft" + (i + 1));
            }

            //Importing goblin images
            for (int i = 0; i < 7; i++)
            {
                goblinImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Goblin/Up/goblinUp" + (i + 1));
                goblinImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Goblin/Right/goblinRight" + (i + 1));
                goblinImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Goblin/Down/goblinDown" + (i + 1));
                goblinImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Goblin/Left/goblinLeft" + (i + 1));
            }

            //Importing prince images
            for(int i = 0; i < 4; i++)
            {
                princeImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Prince/Up/princeUp" + (i + 1));
                princeImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Prince/Right/princeRight" + (i + 1));
                princeImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Prince/Down/princeDown" + (i + 1));
                princeImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Prince/Left/princeLeft" + (i + 1));
            }

            //Importing wizard images
            for(int i = 0; i < 16; i++)
            {
                wizardImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Wizard/Up/wizardUp" + (i + 1));
                wizardImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Wizard/Right/wizardRight" + (i + 1));
                wizardImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Wizard/Down/wizardDown" + (i + 1));
                wizardImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Wizard/Left/wizardLeft" + (i + 1));
            }

            //Importing fairy images
            for (int i = 0; i < 4; i++)
            {
                fairyImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Fairy/Up/fairyUp" + (i + 1));
                fairyImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Fairy/Right/fairyRight" + (i + 1));
                fairyImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Fairy/Down/fairyDown" + (i + 1));
                fairyImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Fairy/Left/fairyLeft" + (i + 1));
            }

            //Importing witch images
            for (int i = 0; i < 4; i++)
            {
                witchImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Witch/Up/witchUp" + (i + 1));
                witchImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Witch/Right/witchRight" + (i + 1));
                witchImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Witch/Down/witchDown" + (i + 1));
                witchImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Witch/Left/witchLeft" + (i + 1));
            }

            //Importing dragon images
            for (int i = 0; i < 4; i++)
            {
                dragonImg[0, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Dragon/Up/dragonUp" + (i + 1));
                dragonImg[1, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Dragon/Right/dragonRight" + (i + 1));
                dragonImg[2, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Dragon/Down/dragonDown" + (i + 1));
                dragonImg[3, i] = Content.Load<Texture2D>("Images/Sprites/Enemies/Dragon/Left/dragonLeft" + (i + 1));
            }
        }
    }
}