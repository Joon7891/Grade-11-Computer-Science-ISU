//Name: Joon Song
//File Name: Dragon.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for dragon object (Child Class to Enemy Class)

using Microsoft.Xna.Framework;

namespace ISU_Song.Enemies
{
    class Dragon : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.25f;
        private const int TOTAL_HEALTH = 5000;
        private const int WEIGHT = 50;

        //Constructor for dragon class
        public Dragon(double relativePos) : base(relativePos)
        {
            //Setting up rectangle and speed
            rect = new Rectangle(0, 0, 45, 45);
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
            updateCountMax = 15;
            animationImg = EnemyResources.dragonImg;

            //Calling subprogram to set up health bar
            HealthBarSetup();
        }
    }
}
