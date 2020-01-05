//Name: Joon Song
//File Name: CollisionDetection.cs
//Project Name: ISU_Song
//Creation Date: December 18, 2017
//Modified Date: Janurary 21, 2018
//Description: Class to hold collision detection subprograms and logic

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ISU_Song.Driver
{
    public class CollisionDetection
    {
        //Pre: 'newCircle', the search circle, 'newEnemyList', the list of enemies to check for collision,
        //Post: Returns a list of enemies within the range
        //Description: Subprogram to determine all enemies within the range of a circle
        public static List<Enemies.Enemy> QuadTree(GUI.Circle newCircle, List<Enemies.Enemy> newEnemyList)
        {
            //List of enemies to return and quadrant of enemies
            List<Enemies.Enemy> enemyReturnList = new List<Enemies.Enemy>();
            int circleQuadrant;

            //Initial min, max and midpoint coordinates of x and y
            int xMin = 650;
            int xMax = 0;
            int xMid;
            int yMin = 550;
            int yMax = 0;
            int yMid;

            //Determining max, min, and mid of enemy x and y coordinates
            for (int i = 0; i < newEnemyList.Count; i++)
            {
                //If newEnemy center x is less than current x min, set new x min
                if (newEnemyList[i].centerLoc.X < xMin)
                {
                    xMin = (int)newEnemyList[i].centerLoc.X;
                }

                //If newEnemy center x is greater than current x max, set new x max
                if (newEnemyList[i].centerLoc.X > xMax)
                {
                    xMax = (int)newEnemyList[i].centerLoc.X;
                }

                //If newEnemy center y is less than current y min, set new y min
                if (newEnemyList[i].centerLoc.Y < yMin)
                {
                    yMin = (int)newEnemyList[i].centerLoc.Y;
                }

                //If newEnemy center y is greater than current y max, set new y max
                if (newEnemyList[i].centerLoc.Y > yMax)
                {
                    yMax = (int)newEnemyList[i].centerLoc.Y;
                }
            }
            xMid = (int)((xMax + xMin) / 2.0);
            yMid = (int)((yMax + yMin) / 2.0);

            //Determining quadrant of quadtree circle
            if (newCircle.center.X > xMid)
            {
                if (newCircle.center.Y > yMid)
                {
                    circleQuadrant = 3;
                }
                else
                {
                    circleQuadrant = 2;
                }
            }
            else
            {
                if (newCircle.center.Y > yMid)
                {
                    circleQuadrant = 4;
                }
                else
                {
                    circleQuadrant = 1;
                }
            }

            //If center of quadtree is inside of circle
            if (Math.Abs(newCircle.center.X - xMid) >= newCircle.radius &&
                Math.Abs(newCircle.center.Y - yMid) >= newCircle.radius)
            {
                //If enemy is inside circle's quadrant, add enemy
                for (int i = 0; i < newEnemyList.Count; i++)
                {
                    if (circleQuadrant == 1 && newEnemyList[i].centerLoc.X <= xMid && newEnemyList[i].centerLoc.Y <= yMid)
                    {
                        enemyReturnList.Add(newEnemyList[i]);
                    }
                    else if (circleQuadrant == 2 && newEnemyList[i].centerLoc.X >= xMid && newEnemyList[i].centerLoc.Y <= yMid)
                    {
                        enemyReturnList.Add(newEnemyList[i]);
                    }
                    else if (circleQuadrant == 3 && newEnemyList[i].centerLoc.X >= xMid && newEnemyList[i].centerLoc.Y >= yMid)
                    {
                        enemyReturnList.Add(newEnemyList[i]);
                    }
                    else if (circleQuadrant == 4 && newEnemyList[i].centerLoc.X <= xMid && newEnemyList[i].centerLoc.Y >= yMid)
                    {
                        enemyReturnList.Add(newEnemyList[i]);
                    }
                }

                //Performing appropriate return, depending on how many enemies are left
                if (enemyReturnList.Count == 0)
                {
                    //If list is empty, return empty list
                    return enemyReturnList;
                }
                else if (enemyReturnList.Count == 1)
                {
                    //If list has one enemy, return appropriate list depending on if enemy is within range
                    if (PointToCircle(newCircle, enemyReturnList[0].centerLoc))
                    {
                        return enemyReturnList;
                    }
                    else
                    {
                        return new List<Enemies.Enemy>();
                    }

                }
                else if (enemyReturnList.Count == 2)
                {
                    //If list has two enemies, check of they are within range
                    for (int i = 0; i < 2; i++)
                    {
                        if (PointToCircle(newCircle, enemyReturnList[i].centerLoc))
                        {
                            enemyReturnList.Add(enemyReturnList[i]);
                        }
                    }
                    enemyReturnList.RemoveRange(0, 2);

                    //Returning enemy list
                    return enemyReturnList;
                }
                else
                {
                    //Calling recursive case
                    return QuadTree(newCircle, new List<Enemies.Enemy>(enemyReturnList));
                }
            }
            else
            {
                //Checking to see if enemy is inside circle
                for (int i = 0; i < newEnemyList.Count; i++)
                {
                    if (PointToCircle(newCircle, newEnemyList[i].centerLoc))
                    {
                        enemyReturnList.Add(newEnemyList[i]);
                    }
                }

                //Returning enemy list
                return enemyReturnList;
            }
        }
        
        //Pre: 'newRect', the rectangle to check for collision, 'newPoint', the point to check for collision
        //Post: Returns true or false, depending on if the point is inside the rectangle
        //Description: Subprogram to check if a point is inside a rectangle
        public static bool PointToRectangle(Rectangle newRect, Vector2 newPoint)
        {
            //If x and y coordinats of the point are inside the rectangle, return true
            if (newPoint.X >= newRect.Left &&
                newPoint.X <= newRect.Right &&
                newPoint.Y >= newRect.Top &&
                newPoint.Y <= newRect.Bottom)
            {
                return true;
            }

            //Otherwise, return false
            return false;
        }

        //Pre: 'newCircle', the circle to check for collision, 'newPoint', the point to check for collision
        //Post: Returns true or false, depending on whether the point is inside circle
        //Description: Subprogram to check if an enemy is inside a tower's range
        public static bool PointToCircle(GUI.Circle newCircle, Vector2 newPoint)
        {
            //If enemy is inside circle, return true
            if (Math.Pow((newCircle.center.X - newPoint.X), 2) + Math.Pow((newCircle.center.Y - newPoint.Y), 2) <= newCircle.radius * newCircle.radius)
            {
                return true;
            }
            
            //Otherwise, return false
            return false;
        }
    }
}
