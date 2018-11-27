//Name: Joon Song
//File Name: Wizard.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for wizard object (Child Class to Enemy Class)

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
    class Wizard : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.7f;
        private const int TOTAL_HEALTH = 500;
        private const int WEIGHT = 8;
        
        //Constructor for wiazrd class
        public Wizard(double relativePos) : base(relativePos)
        {
            //Setting up rectangle and speed
            rect = new Rectangle(0, 0, 38, 38);
            speed = SPEED;
            
            //Setting health, weight, and loot
            totalHealth = TOTAL_HEALTH;
            currentHealth = TOTAL_HEALTH;
            weight = WEIGHT;
            lootGold = WEIGHT;
            lootWood = WEIGHT;
            lootSteel = WEIGHT;

            //Setting up animation components
            numFrames = 16;
            updateCountMax = 4;
            animationImg = EnemyResources.wizardImg;

            //Calling subprogram to setup health bar
            HealthBarSetup();
        }
    }
}
