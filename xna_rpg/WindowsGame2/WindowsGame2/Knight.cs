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
    class Knight : Character
    {
        Character championCommander;
        SpriteBatch spriteBatch;
        Texture2D[] walk;
        Texture2D[] attack;
        Texture2D walkScaled;
        ContentManager content;
        BattleMap map;
        private Vector2 movePosition = new Vector2();
        Vector2 attackPos = new Vector2(1, 200);
        GameStateManager gameStateManager;
        int k;
        float damage;
        int experienceGain;
        

        public Knight(Game game, int xLoc, int yLoc, int playerIndex)
            : base(game, xLoc, yLoc)
        {
            
            walk = new Texture2D[2];
            attack = new Texture2D[4];
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            /*Initialize();
            LoadContent();*/
            this.Move(xLoc, yLoc);
            PlayerIndex = playerIndex;
            HasMoved = true;
            HasAttacked = true;
            MoveDistance = 3;
            HealthPoints = 25;
            CurrentHealth = HealthPoints;
            Strength = 10;
            PDefense = 10;
            Dexterity = 10;
            AttackRange = 1;
            Alive = true;
            CharType = "mercenary";
            Cost = 100;
            Attacked = 0;
            championCommander = GetChampionCommander((List<Character>)game.Services.GetService(typeof(List<Character>)), this);
            Level = 1;
            Experience = 0;
        }

        public override void Initialize()
        {   
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            content = GetContentManager();
            content.RootDirectory = "Content\\Knight";
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
                walkScaled = content.Load<Texture2D>("walk_0_scaled");
            }
            else
            {
                walk[0] = content.Load<Texture2D>("walk_0_2");
                walk[1] = content.Load<Texture2D>("walk_1_2");
                attack[0] = content.Load<Texture2D>("attack_0_2");
                attack[1] = content.Load<Texture2D>("attack_1_2");
                attack[2] = content.Load<Texture2D>("attack_2_2");
                attack[3] = content.Load<Texture2D>("attack_3_2");
                walkScaled = content.Load<Texture2D>("walk_0_scaled_2");
            }
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attackPos.X < 4500)
            {
                attackPos.X += (float).103 * attackPos.X;
            }
            if (position.X != movePosition.X)
            {
                if (position.X < movePosition.X) position.X += 2;
                if (position.X > movePosition.X) position.X -= 2;
            }
            if (position.Y != movePosition.Y)
            {
                if (position.Y < movePosition.Y) position.Y += 2;
                if (position.Y > movePosition.Y) position.Y -= 2;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameStateManager.State == GameState.playing )
            {
                if (Alive == true && playerManager.GetCurrentPlayer() == PlayerIndex)
                {
                    int time = gameTime.TotalGameTime.Milliseconds;

                    if (time % 500 == 0)
                    {
                        if (k == 0) k = 1;
                        else k = 0;
                    }

                    spriteBatch.Draw(walk[k], position, Color.White);
                    attackPos.X = 1;
                }
                else if(Alive == false)
                {
                    //draw nothing
                }
                else
                {
                    spriteBatch.Draw(walk[0], position, Color.White);
                }
            }

            if(gameStateManager.State == GameState.combat && (Attacking == 1 || Attacked == 1))
            {
                DrawCombat(this.Attacked, attackPos);
            }
            
            spriteBatch.End();
            base.Draw(gameTime);
        }

        public override void Move(int x, int y)
        {
            map.GetSquare((int)position.Y / 60, (int)position.X / 60).setCurrentChar(null);
            map.GetSquare(y, x).setCurrentChar(this);
            movePosition.X = x * 60 + 12;
            movePosition.Y = y * 60 + 27;
            HasMoved = true;
        }

        private void DrawCombat(int attacked, Vector2 attackPos)
        {
            
            if (attacked == 0)
            {
                
                if (attackPos.X <= 515)
                {
                    spriteBatch.Draw(attack[0], attackPos, Color.White);
                }
                else if(attackPos.X > 515 && attackPos.X < 535)
                {
                    spriteBatch.Draw(attack[1], attackPos, Color.White);
                }
                else if (attackPos.X >= 535 && attackPos.X < 900)
                {
                    spriteBatch.Draw(attack[2], attackPos, Color.White);
                    if (damage > 0)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + (int)damage, new Vector2(630, 200), Color.Red);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, "Miss", new Vector2(630, 200), Color.Red);
                        spriteBatch.DrawString(spriteFont, "Exp: +" + experienceGain, new Vector2(700, 150), Color.Gold);
                    }
                }
                else if (attackPos.X >= 900 && attackPos.X < 4500)
                {  
                    attackPos.X = 700;
                    spriteBatch.Draw(attack[3], attackPos, Color.White);
                    if (damage > 0)
                    {
                        spriteBatch.DrawString(spriteFont, "-" + (int)damage, new Vector2(630, 200), Color.Red);
                        spriteBatch.DrawString(spriteFont, "Exp: +" + experienceGain, new Vector2(700, 150), Color.Gold);
                    }
                    else
                    {
                        spriteBatch.DrawString(spriteFont, "Miss", new Vector2(630, 200), Color.Red);
                    }
                }
                else
                {
                    gameStateManager.State = GameState.combatOver;
                    damage = 0;
                }
            }
            else
            {
                spriteBatch.Draw(walkScaled, new Vector2(625, 260), Color.White);
            }
        }

        public override void Attack(int x, int y)
        {
            Character attacked = map.GetSquare(x, y).getCurrentChar();
            
            if(1*(100-attacked.Dexterity) > RandomNumber(1,100) + Dexterity)
            {
                damage = ((float)Strength / (float)(Strength + attacked.PDefense)) * (float)Strength;
                attacked.CurrentHealth -= (int)damage;
                experienceGain = (Math.Abs(attacked.Level - Level) + 1) * 20;
                
                if (attacked.CurrentHealth <= 0)
                {
                    map.GetSquare(x, y).setCurrentChar(null);
                    attacked.Alive = false;
                    championCommander.Gold += attacked.Cost;
                    experienceGain = (Math.Abs(attacked.Level - Level) + 1) * 40;
                }

                Experience += experienceGain;
                if (Experience >= 100) LevelUp();
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Level++;
            HealthPoints += 10;
            CurrentHealth = HealthPoints;
            Strength += 2;
            Dexterity += 1;
            Intelligence += 1;
            PDefense += 4;
            MDefense += 2;
            Experience = 0;
        }
    }
}
