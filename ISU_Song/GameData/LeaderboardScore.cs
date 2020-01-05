//Name: Joon Song
//File Name: LeaderboardScore.cs
//Project Name: ISU_Song
//Creation Date: November 30, 2017
//Modified Date: November 30, 2017
//Description: Class for leaderboard score object

namespace ISU_Song.GameData
{
    public class LeaderboardScore
    {
        //Variables for name and score        
        public string name;
        public int score;

        //Constructor for leaderboard object, setting name and score values
        public LeaderboardScore(string newName, int newScore)
        {
            name = newName;
            score = newScore;
        }
    }
}
