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
    class Barbarian : Character
    {
        private Character championCommander;
        int experienceGain;
        SpriteBatch spriteBatch;
        Texture2D[] walk;
        Texture2D[] attack;
        Texture2D wind;
        Texture2D walkScaled;
        ContentManager content;
        BattleMap map;
        Vector2 movePosition = new Vector2();
        Vector2 attackPos = new Vector2(1, 250);
        Vector2 windPos1 = new Vector2();
        Vector2 windPos2 = new Vector2();
        Vector2 windPos3 = new Vector2();
        GameStateManager gameStateManager;
        int k;
        float damage;

        public Barbarian(Game game, int xLoc, int yLoc, int playerIndex)
            : base(game, xLoc, yLoc)
        {
            walk = new Texture2D[2];
            attack = new Texture2D[3];
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            this.Move(xLoc, yLoc);
            PlayerIndex = playerIndex;
            HasMoved = true;
            HasAttacked = true;
            MoveDistance = 4;
            HealthPoints = 20;
            CurrentHealth = HealthPoints;
            Strength = 15;
            PDefense = 7;
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
            content.RootDirectory = "Content\\Barbarian";
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
                walkScaled = content.Load<Texture2D>("walk_scaled");
            }
            else
            {
                walk[0] = content.Load<Texture2D>("walk_0_2");
                walk[1] = content.Load<Texture2D>("walk_1_2");
                attack[0] = content.Load<Texture2D>("attack_0_2");
                attack[1] = content.Load<Texture2D>("attack_1_2");
                attack[2] = content.Load<Texture2D>("attack_2_2");
                walkScaled = content.Load<Texture2D>("walk_scaled_2");
            }
            wind = content.Load<Texture2D>("wind");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attackPos.X < 4500)
            {
                attackPos.X += (float).103 * attackPos.X;
                windPos1.X = attackPos.X;
                windPos1.Y += (float).103 * windPos1.Y;
                windPos2.X = attackPos.X;
                windPos2.Y -= (float).103 * windPos2.Y;
                windPos3.X = attackPos.X;
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

                    spriteBatch.Draw(walk[k], position, Color.White);
                    attackPos.X = 1;
                }
                else if (Alive == false)
                {
                    //draw nothing
                }
                else
                {
                    spriteBatch.Draw(walk[0], position, Color.White);
                }
            }

            if (gameStateManager.State == GameState.combat && (Attacking == 1 || Attacked == 1))
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
                    windPos1.Y = 250;
                    windPos2.Y = 250;
                    windPos3.Y = 250;
                }
                else if(attackPos.X > 515 && attackPos.X < 535)
                {
                    spriteBatch.Draw(attack[1], attackPos, Color.White);
                    spriteBatch.Draw(wind, windPos1, Color.White);
                    spriteBatch.Draw(wind, windPos2, Color.White);
                    spriteBatch.Draw(wind, windPos3, Color.White);
                    
                }
                else if (attackPos.X >= 535 && attackPos.X < 700)
                {
                    attackPos.X = 500;
                    spriteBatch.Draw(attack[1], attackPos, Color.White);
                    spriteBatch.Draw(wind, windPos1, Color.White);
                    spriteBatch.Draw(wind, windPos2, Color.White);
                    spriteBatch.Draw(wind, windPos3, Color.White);
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
                else if (attackPos.X >= 700 && attackPos.X < 4500)
                {
                    attackPos.X = 500;
                    spriteBatch.Draw(attack[2], attackPos, Color.White);
                    spriteBatch.Draw(wind, windPos1, Color.White);
                    spriteBatch.Draw(wind, windPos2, Color.White);
                    spriteBatch.Draw(wind, windPos3, Color.White);
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
                spriteBatch.Draw(walkScaled, new Vector2(625, 260), Color.White);
            }
        }

        public override void Attack(int x, int y)
        {
            Character attacked1 = map.GetSquare(x, y).getCurrentChar();
            Character attacked2 = null;
            Character attacked3 = null;

            if ((map.GetSquare(x + 1, y).getCurrentChar() != null && map.GetSquare(x + 1, y).getCurrentChar() == this) ||
                (map.GetSquare(x - 1, y).getCurrentChar() != null && map.GetSquare(x - 1, y).getCurrentChar() == this))
            {
                if (map.GetSquare(x, y - 1).getCurrentChar() != null) attacked2 = map.GetSquare(x, y - 1).getCurrentChar();
                if (map.GetSquare(x, y + 1).getCurrentChar() != null) attacked3 = map.GetSquare(x, y - 1).getCurrentChar();
            }

            if ((map.GetSquare(x, y + 1).getCurrentChar() != null && map.GetSquare(x, y + 1).getCurrentChar() == this) ||
                (map.GetSquare(x, y - 1).getCurrentChar() != null && map.GetSquare(x, y - 1).getCurrentChar() == this))
            {
                if (map.GetSquare(x - 1, y).getCurrentChar() != null) attacked2 = map.GetSquare(x - 1, y).getCurrentChar();
                if (map.GetSquare(x + 1, y).getCurrentChar() != null) attacked3 = map.GetSquare(x + 1, y).getCurrentChar();
            }

            if (attacked2 != null) attacked2.Attacked = 1;
            if (attacked3 != null) attacked3.Attacked = 1;
            
            
            if(1*(100-attacked1.Dexterity) > RandomNumber(1,100) + Dexterity)
            {
                damage = ((float)Strength / (float)(Strength + attacked1.PDefense)) * (float)Strength;
                attacked1.CurrentHealth -= (int)damage;
                experienceGain = (Math.Abs(attacked1.Level - Level) + 1) * 20;
                if (attacked2 != null) attacked2.CurrentHealth -= (int)(damage * .3);
                if (attacked3 != null) attacked3.CurrentHealth -= (int)(damage * .3);
                
                if (attacked1.CurrentHealth <= 0)
                {
                    map.GetSquare(x, y).setCurrentChar(null);
                    attacked1.Alive = false;
                    championCommander.Gold += attacked1.Cost;
                    experienceGain = (Math.Abs(attacked1.Level - Level) + 1) * 40;
                }
                if (attacked2 != null && attacked2.CurrentHealth <= 0)
                {
                    Vector2 attacked2Pos = attacked2.getPosition();
                    map.GetSquare((int)attacked2Pos.X/60, (int)attacked2Pos.Y/60).setCurrentChar(null);
                    attacked2.Alive = false;
                    championCommander.Gold += attacked2.Cost;
                }
                if (attacked3 != null && attacked3.CurrentHealth <= 0)
                {
                    Vector2 attacked3Pos = attacked3.getPosition();
                    map.GetSquare((int)attacked3Pos.X / 60, (int)attacked3Pos.Y / 60).setCurrentChar(null);
                    attacked3.Alive = false;
                    championCommander.Gold += attacked1.Cost;
                }

                Experience += experienceGain;
                if (Experience >= 100) LevelUp();
            }
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Level++;
            HealthPoints += 4;
            CurrentHealth = HealthPoints;
            Strength += 5;
            Dexterity += 2;
            Intelligence += 1;
            PDefense += 2;
            MDefense += 1;
            Experience = 0;
        }
    }
}
