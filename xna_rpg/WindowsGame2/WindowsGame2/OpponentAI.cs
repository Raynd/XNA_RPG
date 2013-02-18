using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{
    class OpponentAI : GameComponent
    {
        PlayerManager playerManager;
        BattleMap map;
        List<Character> charList;
        Cursor cursor;
        Game game;
        ScreenManager screenManager;
        GameStateManager gameStateManager;
        GameTime gameTime;
        int i = 0;
        
        public OpponentAI(Game game)
            : base(game)
        {
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            charList = (List<Character>)game.Services.GetService(typeof(List<Character>));
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            this.game = game;
        }

        double lastMove;
        public override void Update(GameTime gameTime)
        {
            this.gameTime = gameTime;
            base.Update(gameTime);

            int count = charList.Count;
            
            
            Character character = charList[i];
            if (playerManager.GetCurrentPlayer() == 2)
            {
                if (charList[i].PlayerIndex != 2 || character.Alive == false)
                {
                    i++;
                }
                else if (charList[i].PlayerIndex == 2 && character.Alive == true)// && gameTime.TotalGameTime.TotalSeconds % 2 == 0)
                {
                    if (gameTime.TotalGameTime.TotalSeconds - lastMove > 3)
                    {
                        lastMove = gameTime.TotalGameTime.TotalSeconds;
                        GetMove(character, gameTime);
                        i++;
                    }
                }


                if (i == count - 1)
                {
                    i = 0;
                    playerManager.EndTurn();
                }
            }
        }

        public void LoadCursor(Game game)
        {
            cursor = (Cursor)game.Services.GetService(typeof(Cursor));
        }

        public void StartTurn()
        {
            
            
            /*for(int i = 0; i<charList.Count; i++)
            {
                Character character = charList[i];

                if(character.PlayerIndex == 2 && character.Alive == true)
                {
                    GetMove(character);
                }
            }*/
            
        }

        public void GetMove(Character character, GameTime gameTime)
        {
            if (character.CharType == "champion")
            {
                GetChampionMove(character);
            }
            else
            {
                GetMercenaryMove(character);
            }
        }

        public void GetChampionMove(Character character)
        {
            if (character.Gold >= 100)
            {
                MoveToCamp(character);
            }
            else
            {
                MoveToEnemy(character);
            }
        }

        public void GetMercenaryMove(Character character)
        {
            MoveToEnemy(character);
            AttackEnemy(character);
        }

        private void AttackEnemy(Character character)
        {
            Character lowHealth = null;

            if (character.HasAttacked == false)
            {
                cursor.AttackHelper(character);
                for (int i = 0; i < map.getHeight(); i++)
                {
                    for (int j = 0; j < map.getWidth(); j++)
                    {
                        if (map.GetSquare(j, i).IsAttackable && map.GetSquare(j, i).getCurrentChar() != null)
                        {
                            if(character.GetType() == typeof(SpiritPriest))
                            {
                                if (map.GetSquare(j, i).getCurrentChar().PlayerIndex == 2)
                                {
                                    Character temp = map.GetSquare(j, i).getCurrentChar();

                                    if (lowHealth == null)
                                    {
                                        lowHealth = temp;
                                    }
                                    else if (temp.CurrentHealth < lowHealth.CurrentHealth)
                                    {
                                        lowHealth = temp;
                                    }
                                }
                            }
                            
                            else if (map.GetSquare(j, i).getCurrentChar().PlayerIndex == 1)
                            {
                                Character temp = map.GetSquare(j, i).getCurrentChar();

                                if (lowHealth == null)
                                {
                                    lowHealth = temp;
                                }
                                else if (temp.CurrentHealth < lowHealth.CurrentHealth)
                                {
                                    lowHealth = temp;
                                }
                            }
                        }
                    }
                }

                cursor.ClearBoard();

                if (lowHealth != null)
                {
                    gameStateManager.State = GameState.combat;
                    character.Attack((int)lowHealth.getPosition().Y / 60, (int)lowHealth.getPosition().X / 60);
                    character.HasAttacked = true;
                    character.Attacking = 1;
                    lowHealth.Attacked = 1;
                    screenManager.AddScreen(new BattleScreen(game, character, lowHealth), null);
                }
            }
        }

        private void MoveToEnemy(Character character)
        {
            Character closestEnemy =  charList[0];
            int minDistance = (int)Math.Sqrt(((int)character.getPosition().Y / 60 - (int)closestEnemy.getPosition().Y / 60) * ((int)character.getPosition().Y / 60 - (int)closestEnemy.getPosition().Y / 60) - ((int)character.getPosition().X / 60 - (int)closestEnemy.getPosition().X / 60) * ((int)character.getPosition().X / 60 - (int)closestEnemy.getPosition().X) / 60);

            for (int i = 0; i < map.getHeight(); i++)
            {
                for (int j = 0; j < map.getWidth(); j++)
                {
                    if (map.GetSquare(i, j).getCurrentChar() != null && map.GetSquare(i, j).getCurrentChar().PlayerIndex == 1)
                    {
                        Character temp = map.GetSquare(i, j).getCurrentChar();
                        int distance = (int)Math.Sqrt(((int)character.getPosition().Y / 60 - (int)temp.getPosition().Y / 60) * ((int)character.getPosition().Y / 60 - (int)temp.getPosition().Y / 60) - ((int)character.getPosition().X / 60 - (int)temp.getPosition().X / 60) * ((int)character.getPosition().X / 60 - (int)temp.getPosition().X / 60));

                        if (distance < minDistance)
                        {
                            closestEnemy = temp;
                            minDistance = distance;
                        }
                    }
                }
            }

            if (character.HasMoved == false)
            {
                cursor.MoveHelper(character);
                int distance = 9000;
                Vector2 toMove = new Vector2();

                for (int i = 0; i < map.getHeight(); i++)
                {
                    for (int j = 0; j < map.getWidth(); j++)
                    {
                        if (map.GetSquare(i, j).IsMovable == true)
                        {
                            int distanceToEnemy = (int)Math.Sqrt(Math.Pow((int)j - ((int)closestEnemy.getPosition().Y / 60), 2) + Math.Pow((int)i - ((int)closestEnemy.getPosition().X / 60), 2));

                            if (distanceToEnemy < distance)
                            {
                                if(distanceToEnemy >= 1)
                                {
                                    distance = distanceToEnemy;
                                    toMove.X = i;
                                    toMove.Y = j;
                                }
                            }
                        }
                    }
                }
                character.Move((int)toMove.X, (int)toMove.Y);
                character.HasMoved = true;

                cursor.ClearBoard();
            }
        }

        private void MoveToCamp(Character character)
        {
            Vector2 campPosition;
            Vector2 minCampPosition = new Vector2(); ;
            int minCampDistance = 9000;
            Character nearbyCamp;

            foreach (Character camp in charList)
            {
                if (camp.CharType == "camp")
                {
                    campPosition = camp.getPosition();
                    campPosition.X = (int)campPosition.X / 60;
                    campPosition.Y = (int)campPosition.Y / 60;

                    if ((int)Math.Sqrt((campPosition.X * campPosition.X) + (campPosition.Y * campPosition.Y)) < minCampDistance)
                    {
                        minCampPosition = campPosition;
                        nearbyCamp = camp;
                    }    
                }
            }

            cursor.MoveHelper(character);

            int minDistance = 9000;
            Vector2 toMove = new Vector2();
            for (int i = 0; i < map.getHeight(); i++)
            {
                for (int j = 0; j < map.getWidth(); j++)
                {
                    if (map.GetSquare(i, j).IsMovable == true)
                    {
                        int distanceToCamp = (int)Math.Sqrt(Math.Abs((i - minCampPosition.Y) * (i - minCampPosition.Y)) - Math.Abs((j - minCampPosition.X) * (j - minCampPosition.X)));

                        if (distanceToCamp < minDistance)
                        {
                            minDistance = distanceToCamp;
                            toMove.X = j;
                            toMove.Y = i;
                        }
                    }
                }
            }

            if (character.HasMoved == false)
            {
                character.Move((int)toMove.Y, (int)toMove.X);
                character.HasMoved = true;

                //map.GetSquare((int)character.getPosition().Y / 60, (int)character.getPosition().X / 60).TileDeselect();
                cursor.ClearBoard();

                if ((int)(character.getPosition().X / 60) + 1 < 10 && map.GetSquare((int)(character.getPosition().Y / 60), (int)(character.getPosition().X / 60) + 1).getCurrentChar() != null)
                {
                    if (map.GetSquare((int)(character.getPosition().Y / 60), (int)(character.getPosition().X / 60) + 1).getCurrentChar().CharType == "camp")
                    {
                        BuyMercenary(character);
                    }
                }
                if ((int)(character.getPosition().Y / 60) - 1 >= 0 && map.GetSquare((int)(character.getPosition().Y / 60) - 1, (int)(character.getPosition().X / 60)).getCurrentChar() != null)
                {
                    if (map.GetSquare((int)(character.getPosition().Y / 60) - 1, (int)(character.getPosition().X / 60)).getCurrentChar().CharType == "camp")
                    {
                        BuyMercenary(character);
                    }
                }
                if ((int)(character.getPosition().X / 60) - 1 >= 0 && map.GetSquare((int)(character.getPosition().Y / 60), (int)(character.getPosition().X / 60) - 1).getCurrentChar() != null)
                {
                    if (map.GetSquare((int)(character.getPosition().Y / 60), (int)(character.getPosition().X / 60) - 1).getCurrentChar().CharType == "camp")
                    {
                        BuyMercenary(character);
                    }
                }
                if ((character.getPosition().Y / 60) + 1 < 10 && map.GetSquare((int)(character.getPosition().Y / 60) + 1, (int)(character.getPosition().X / 60)).getCurrentChar() != null)
                {
                    if (map.GetSquare((int)(character.getPosition().Y / 60) + 1, (int)(character.getPosition().X / 60)).getCurrentChar().CharType == "camp")
                    {
                        BuyMercenary(character);
                    }
                }
            }
        }

        private void BuyMercenary(Character character)
        {
            for (int i = 0; i < 2; i++)
            {
                BuyRandom(character);
            }
        }

        void BuyRandom(Character champion)
        {
            Random random = new Random();
            int num;

            if (champion.Gold >= 150) num = random.Next(0, 4);
            else num = random.Next(0, 2);
            
            switch (num)
            {
                case 0:
                    new BuyScreen(game, champion).buyKnightSelected(null, null);
                    break;
                case 1:
                    new BuyScreen(game, champion).buyRangerSelected(null, null);
                    break;
                case 2:
                    new BuyScreen(game, champion).buyBarbarianSelected(null, null);
                    break;
                case 3:
                    new BuyScreen(game, champion).buyMageSelected(null, null);
                    break;
                case 4:
                    new BuyScreen(game, champion).buyPriestSelected(null, null);
                    break;
            }
        }
    }
}
