//Name: Joon Song
//File Name: ResourceTower.cs
//Project Name: ISU_Song
//Creation Date: December 23, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for resource tower object (Sub-Parent Class)

using System;
using Microsoft.Xna.Framework;

namespace ISU_Song.Towers
{
    public abstract class ResourceTower : Tower
    {
        //Constant to hold initial and total yield
        public const int DEFAULT_YIELD = 15;
        public const int DEFAULT_SPEED = 10;
        protected int yieldRate;
        protected int totalYield = 0;

        //Constructor for resource tower class
        public ResourceTower(int x, int y) : base(x, y)
        {
        }

        //Overloaded constructor for resouce tower tower class, used for IO
        public ResourceTower(int x, int y, int propLevel1, int propLevel2, int total) : base(x, y, propLevel1, propLevel2, total)
        {
        }

        //Pre: None
        //Post: Adds the produced resources to player wallet
        //Description: Subprogram to add produced resources to player wallet
        public virtual void AddResources()
        {
        }

        //Pre: None
        //Post: Defense tower details are drawn
        //Description: Subprogram to hold draw logic for tower details
        public override void DrawDetails()
        {
            //Calling base version of subprogram
            base.DrawDetails();

            //Drawing defense tower specific information
            Driver.Main.spriteBatch.DrawString(infoText, "Amount:", infoLoc[1], Color.Red);
            Driver.Main.spriteBatch.DrawString(infoText, "Level: " + propertyLevel[0], infoLoc[2], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Value: " + propertyValue[0], infoLoc[3], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Speed:", infoLoc[4], Color.Blue);
            Driver.Main.spriteBatch.DrawString(infoText, "Level: " + propertyLevel[1], infoLoc[5], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Value: " + propertyValue[1], infoLoc[6], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Yield Rate: " + yieldRate, infoLoc[7], Color.Black);
            Driver.Main.spriteBatch.DrawString(infoText, "Total Yield: " + totalYield, infoLoc[8], Color.Red);
        }

        //Pre: None
        //Post: Various resource tower upgrade updates are made
        //Description: Subprogram to hold update logic for upgrades
        public override void UpgradeLogic()
        {
            //Calling base upgrade logic subprogram
            base.UpgradeLogic();

            //Updating yield amount
            yieldRate = propertyValue[0] * propertyValue[1];
        }

        //Pre: None
        //Post: Upgrades the amount of production
        //Description: Subprogram to upgrade production amount
        public override void Upgrade1()
        {
            //Upgrading production amount
            propertyValue[0] = (int)(Math.Round(propertyValue[0] * 1.25));
            propertyLevel[0]++;
        }

        //Pre: None
        //Post: Upgrades the rate of production
        //Description: Subprogram to upgrade production rate
        public override void Upgrade2()
        {
            //Upgrading production rate
            propertyValue[1] = (int)(Math.Round(propertyValue[1] * 1.25));
            propertyLevel[1]++;
        }

        //Pre: None
        //Post: Resource tower's information is written as a string
        //Description: Subprogram to condense the resouce tower's information into a string
        public override string GetInformation()
        {
            //Initializing and setting return string
            string returnString;
            returnString = towerType + "," + GetCoordinates() + "," + propertyLevel[0] + "," + propertyValue[1] + "," + totalYield;

            //Returning tower information as a string
            return returnString;
        }
    }
}
