//Name: Joon Song
//File Name: Prince.cs
//Project Name: ISU_Song
//Creation Date: Janurary 6, 2018
//Modified Date: Janurary 18, 2018
//Description: Class for prince object (Child Class to Enemy Class)

using Microsoft.Xna.Framework;

namespace ISU_Song.Enemies
{
    class Prince : Enemy
    {
        //Constants for various enemy type specific data
        private const float SPEED = 0.95f;
        private const int TOTAL_HEALTH = 700;
        private const int WEIGHT = 10;

        //Constructor for prince
        public Prince(double relativePos) : base(relativePos)
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
            numFrames = 4;
            updateCountMax = 10;
            animationImg = EnemyResources.princeImg;

            //Calling subprogram to set up health bar
            HealthBarSetup();
        }
    }
}
