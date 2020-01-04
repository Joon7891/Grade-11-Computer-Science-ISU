﻿//Name: Joon Song
//File Name: Skeleton.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for skeleton object (Child Class to Enemy Class)

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace ISU_Song.Enemies
{
    class Skeleton : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.9f;
        private const int TOTAL_HEALTH = 15;
        private const int WEIGHT = 1;

        //Constructor for skeleton class
        public Skeleton(double relativePos) : base(relativePos)
        {
            //Setting up rectangle and speed
            rect = new Rectangle(0, 0, 20, 30);
            speed = SPEED;

            //Setting health, weight, and loot
            totalHealth = TOTAL_HEALTH;
            currentHealth = TOTAL_HEALTH;
            weight = WEIGHT;
            lootGold = WEIGHT;
            lootWood = WEIGHT;
            lootSteel = WEIGHT;

            //Setting up animation components
            numFrames = 4;
            updateCountMax = 12;
            animationImg = EnemyResources.skeletonImg;

            //Calling subprogram to set up health bar
            HealthBarSetup();
        }
    }
}
