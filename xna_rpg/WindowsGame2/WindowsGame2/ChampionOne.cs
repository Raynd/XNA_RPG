using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace WindowsGame2
{
    class ChampionOne : Character
    {

        int gold;
        float damage;
        private int k;
        Game game;
        SpriteBatch spriteBatch;
        Texture2D[] walk;
        Texture2D[] attack;
        Texture2D walkScaled;
        ContentManager content;
        BattleMap map;
        GameStateManager gameStateManager;
        ScreenManager screenManager;
        Vector2 attackPos = new Vector2(1, 250);
        Vector2 movePosition = new Vector2();
        int experienceGain;


        public ChampionOne(Game1 game, int xLoc, int yLoc, int playerIndex)
            : base(game, xLoc, yLoc)
        {
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            this.game = game;
            walk = new Texture2D[2];
            attack = new Texture2D[5];
            PlayerIndex = playerIndex;

            Initialize();
            LoadContent();

            this.Move(xLoc, yLoc);
            PlayerIndex = playerIndex;
            HasMoved = false;
            HasAttacked = false;
            Alive = true;
            CharType = "champion";
            MoveDistance = 4;
            HealthPoints = 50;
            Strength = 20;
            PDefense = 15;
            MDefense = 15;
            Dexterity = 10;
            AttackRange = 1;
            CurrentHealth = HealthPoints;
            Gold = 500;
            Level = 2;
            Experience = 0;
            
        }

        public override void Initialize()
        {
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            content = GetContentManager();
            content.RootDirectory = "Content\\ChampionOne";
            spriteFont = content.Load<SpriteFont>(@"..\\Arial");
            base.Initialize();
        }

        protected override void LoadContent()
        {
            if (PlayerIndex == 1)
            {
                walk[0] = content.Load<Texture2D>("walk_0");
                walk[1] = content.Load<Texture2D>("walk_1");
                attack[0] = content.Load<Texture2D>("attack_0");
                attack[1] = content.Load<Texture2D>("attack_1");
                attack[2] = content.Load<Texture2D>("attack_2");
                attack[3] = content.Load<Texture2D>("attack_3");
                attack[4] = content.Load<Texture2D>("attack_4");
                walkScaled = content.Load<Texture2D>("walk_scaled");
            }
            else
            {
                walk[0] = content.Load<Texture2D>("walk_0_2");
                walk[1] = content.Load<Texture2D>("walk_1_2");
                attack[0] = content.Load<Texture2D>("attack_0_2");
                attack[1] = content.Load<Texture2D>("attack_1_2");
                attack[2] = content.Load<Texture2D>("attack_2_2");
                attack[3] = content.Load<Texture2D>("attack_3_2");
                attack[4] = content.Load<Texture2D>("attack_4_2");
                walkScaled = content.Load<Texture2D>("walk_scaled_2");
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attackPos.X < 2500)
            {
                attackPos.X += (float).103 * attackPos.X;
            }
            if (position.X != movePosition.X)
            {
                if (position.X < movePosition.X) movePosition.X -= 2;
                if (position.X > movePosition.X) movePosition.X += 2;
            }
            if (position.Y != movePosition.Y)
            {
                if (position.Y < movePosition.Y) movePosition.Y -= 2;
                if (position.Y > movePosition.Y) movePosition.Y += 2;
            }
            if (position.Y == movePosition.Y && position.X == movePosition.X)
            {
                Moving = false;
            }

            if (Alive == false)
            {
                gameStateManager.State = GameState.gameOver;
            }
        }

        public override void Draw(GameTime gameTime)
        {
             spriteBatch.Begin();

             if (gameStateManager.State == GameState.playing)
             {
                 if (Alive == true && playerManager.GetCurrentPlayer() == PlayerIndex)
                 {
                     int time = gameTime.TotalGameTime.Milliseconds;

                     if (time % 500 == 0)
                     {
                         if (k == 0) k = 1;
                         else k = 0;
                     }

                     spriteBatch.Draw(walk[k], movePosition, Color.White);
                     attackPos.X = 1;
                 }
                 else if (Alive == false)
                 {
                     //draw nothing
                 }
                 else
                 {
                     spriteBatch.Draw(walk[0], movePosition, Color.White);
                 }
             }

             if (gameStateManager.State == GameState.combat && (Attacking == 1 || Attacked == 1))
             {
                 DrawCombat(this.Attacked, attackPos);
             }

             spriteBatch.End();
        }

        public override void Move(int x, int y)
        {
            movePosition.X = position.X;
            movePosition.Y = position.Y;
            map.GetSquare((int)position.Y / 60, (int)position.X / 60).setCurrentChar(null);
            map.GetSquare(y, x).setCurrentChar(this);
            position.X = x * 60 + 12;
            position.Y = y * 60 + 27;
            Moving = true;
            HasMoved = true;
        }

        public override void BuyMercenary()
        {
            Vector2 currentPos = new Vector2((int)position.X / 60, (int)position.Y / 60);
            gameStateManager.State = GameState.buying;

            if (map.GetSquare((int)currentPos.Y + 1, (int)currentPos.X).getCurrentChar() != null)
            {
                if (map.GetSquare((int)currentPos.Y + 1, (int)currentPos.X).getCurrentChar().CharType == "camp")
                {
                    screenManager.AddScreen(new BuyScreen(game, this), null);
                }
            }
            if (map.GetSquare((int)currentPos.Y, (int)currentPos.X + 1) .getCurrentChar() != null)
            {
                if (map.GetSquare((int)(currentPos.Y), (int)currentPos.X + 1).getCurrentChar().CharType == "camp")
                {
                    screenManager.AddScreen(new BuyScreen(game, this), null);
                }
            }
            if (map.GetSquare((int)currentPos.Y - 1, (int)currentPos.X).getCurrentChar() != null)
            {
                if (map.GetSquare((int)(currentPos.Y - 1), (int)currentPos.X).getCurrentChar().CharType == "camp")
                {
                    screenManager.AddScreen(new BuyScreen(game, this), null);
                }
            }
            if (map.GetSquare((int)currentPos.Y, (int)currentPos.X - 1).getCurrentChar() != null)
            {
                if (map.GetSquare((int)(currentPos.Y), (int)currentPos.X - 1).getCurrentChar().CharType == "camp")
                {
                    screenManager.AddScreen(new BuyScreen(game, this), null);
                }
            }
        }

        private void DrawCombat(int attacked, Vector2 attackPos)
        {

            if (attacked == 0)
            {
                if (attackPos.X <= 450)
                {
                    spriteBatch.Draw(attack[0], attackPos, Color.White);
                }
                else if (attackPos.X > 450 && attackPos.X < 500)
                {
                    spriteBatch.Draw(attack[1], attackPos, Color.White);
                }
                else if (attackPos.X >= 500 && attackPos.X < 535)
                {
                    spriteBatch.Draw(attack[2], attackPos, Color.White);
                    //spriteBatch.DrawString(spriteFont, "-" + (int)damage, new Vector2(630, 200), Color.Red);
                }
                else if (attackPos.X >= 335 && attackPos.X < 550)
                {
                    spriteBatch.Draw(attack[3], attackPos, Color.White);
                    if (damage > 0)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + (int)damage, new Vector2(630, 200), Color.Red);
                        spriteBatch.DrawString(spriteFont, "EXP: +" + experienceGain, new Vector2(500, 200), Color.Gold);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, "Miss", new Vector2(630, 200), Color.Red);
                    }
                }
                else if (attackPos.X >= 550 && attackPos.X < 1500)
                {
                    attackPos.X = 550;
                    spriteBatch.Draw(attack[4], attackPos, Color.White);
                    if (damage > 0)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + (int)damage, new Vector2(630, 200), Color.Red);
                        spriteBatch.DrawString(spriteFont, "EXP: +" + experienceGain, new Vector2(500, 200), Color.Gold);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, "Miss", new Vector2(630, 200), Color.Red);
                    }
                }
                else
                {
                    gameStateManager.State = GameState.combatOver;
                }
            }
            else
            {
                spriteBatch.Draw(walkScaled, new Vector2(600, 260), Color.White);
            }
        }

        public override void Attack(int x, int y)
        {
            Character attacked = map.GetSquare(x, y).getCurrentChar();

            if (1 * (100 - attacked.Dexterity) > RandomNumber(1, 100) + Dexterity)
            {
                damage = ((float)Strength / (float)(Strength + attacked.PDefense)) * (float)Strength;
                attacked.CurrentHealth -= (int)damage;
                experienceGain = (Math.Abs(attacked.Level - Level) + 1) * 20;

                if (attacked.CurrentHealth <= 0)
                {
                    map.GetSquare(x, y).setCurrentChar(null);
                    attacked.Alive = false;
                    Gold += attacked.Cost;
                    experienceGain = (Math.Abs(attacked.Level - Level) + 1) * 40;
                }

                Experience += experienceGain;
                if (Experience >= 100) LevelUp();
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Strength += 3;
            HealthPoints += 5;
            CurrentHealth = HealthPoints;
            Dexterity += 3;
            Intelligence += 1;
            PDefense += 2;
            MDefense += 1;
            Experience = 0;
            Level++;
        }
    }
}
