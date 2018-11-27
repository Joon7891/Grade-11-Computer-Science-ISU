//Name: Joon Song
//File Name: Fairy.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for fairy object (Child Class to Enemy Class)

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
    class Fairy : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.6f;
        private const int TOTAL_HEALTH = 250;
        private const int WEIGHT = 5;

        //Various constants and variables for fairy healing ability
        private const float HEAL_COUNT_MAX = 0.2f;
        private double healCount = 0;
        private const int HEAL_AMOUNT = 30;
        private GUI.Circle healCircle = new GUI.Circle(new Vector2(0, 0), 40, Color.White);

        //Constructor for fairy class
        public Fairy(double relativePos) : base(relativePos)
        {
            //Setting up new rectangle and speed
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
            animationImg = EnemyResources.fairyImg;
        }

        //Pre: None
        //Post: Various fairy object updates are made
        //Description: Subprogram to hold update logic for fairy object
        public override void Update()
        {
            //Calling base class update subprogram 
            base.Update();

            //Updating heal circle
            healCircle.center = centerLoc;
            healCircle.Update();

            //Updating heal time
            healCount += Driver.Main.globalDeltaTime * Driver.GameLogic.currentSpeed;

            //Updating health for all troops around fairy, if it is time to heal
            if(healCount > HEAL_COUNT_MAX)
            {
                for(int i = 0; i < Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList).Count; i++)
                {
                    Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList)[i].currentHealth += HEAL_AMOUNT;

                    //If health of any troops exceeds max, reset it so it is exactly max
                    if (Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList)[i].currentHealth >
                        Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList)[i].totalHealth)
                    {
                        Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList)[i].currentHealth = 
                            (int)Driver.CollisionDetection.QuadTree(healCircle, Driver.GameLogic.currentWave.enemyList)[i].totalHealth;
                    }
                }
                
                //Resetting heal count
                healCount = 0;
            }
        }
    }
}
