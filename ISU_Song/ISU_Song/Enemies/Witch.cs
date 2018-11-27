//Name: Joon Song
//File Name: Witch.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for witch object (Child Class to Enemy Class)

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
    class Witch : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.65f;
        private const int TOTAL_HEALTH = 750;
        private const int WEIGHT = 10;

        //Variables for skeleton spawning
        private const int SPAWN_COUNT_MAX = 350;
        private int spawnCount = 0;

        //Constructor for witch class
        public Witch(double relativePos) : base(relativePos)
        {
            //Setting up rectangle and speed
            rect = new Rectangle(0, 0, 35, 40);
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
            animationImg = EnemyResources.witchImg;

            //Calling subprogram to set up health bar
            HealthBarSetup();
        }

        //Pre: None
        //Post: Various witch components are updated
        //Description: Subprogram to hold update logic for witch
        public override void Update()
        {
            //Calling base class update subprogram
            base.Update();

            //Adding skeleton and resetting spawn count, when spawn count reaches max
            if (spawnCount == SPAWN_COUNT_MAX)
            {
                Driver.GameLogic.currentWave.enemyList.Add(new Skeleton(relativePos));
                spawnCount = 0;
            }
            else
            {
                spawnCount++;
            }
        }
    }
}
