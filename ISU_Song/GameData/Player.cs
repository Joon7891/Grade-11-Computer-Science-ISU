//Name: Joon Song
//File Name: Player.cs
//Project Name: ISU_Song
//Creation Date: December 20, 2017
//Modified Date: Janurary 18, 2018
//Description: Class for player object

namespace ISU_Song.GameData
{
    public class Player
    {
        //Variables to hold balance and name of player        
        public static int lives;
        public static int gold;
        public static int wood;
        public static int steel;
        public static string name = "";

        //Bool variable to hold if revive has been used
        public static bool reviveUsed = false;

        //Pre: None
        //Post: User resources and name are reset
        //Description: Subprogram to reset user resources and name
        public static void ResetPlayer()
        {
            //Resetting player resources and name
            lives = 200;
            wood = 300;
            steel = 300;
            name = "";

            //Resetting revive use to false
            reviveUsed = false;
        }
    }
}
