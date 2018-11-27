//Name: Joon Song
//File Name: Knight.cs
//Project Name: ISU_Song
//Creation Date: December 19, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for knight object (Child Class to Enemy Class)

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
    class Knight : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.85f;
        private const int TOTAL_HEALTH = 300;
        private const int WEIGHT = 5;

        //Constructor for knight
        public Knight(double relativePos) : base(relativePos)
        {
            //Setting up rectangle and speed
            rect = new Rectangle(0, 0, 40, 40);
            speed = SPEED;

            //Setting health, weight, and loot
            totalHealth = TOTAL_HEALTH;
            currentHealth = TOTAL_HEALTH;
            weight = WEIGHT;
            lootGold = WEIGHT;
            lootWood = WEIGHT;
            lootSteel = WEIGHT;

            //Setting up animation components
            numFrames = 9;
            updateCountMax = 7;
            animationImg = EnemyResources.knightImg;

            //Calling subprogram to set up health bar
            HealthBarSetup();
        }
    }
}
