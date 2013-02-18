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
    class SpiritPriest : Character
    {
        Character championCommander;
        SpriteBatch spriteBatch;
        Texture2D[] walk;
        Texture2D[] attack;
        Texture2D[] heal;
        Texture2D walkScaled;
        ContentManager content;
        BattleMap map;
        Vector2 movePosition = new Vector2();
        Vector2 attackPos = new Vector2(50, 200);
        GameStateManager gameStateManager;
        int k;
        float damage;
        int experienceGain;
        

        public SpiritPriest(Game game, int xLoc, int yLoc, int playerIndex)
            : base(game, xLoc, yLoc)
        {
            
            walk = new Texture2D[2];
            attack = new Texture2D[2];
            heal = new Texture2D[3];
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            /*Initialize();
            LoadContent();*/
            this.Move(xLoc, yLoc);
            PlayerIndex = playerIndex;
            HasMoved = true;
            HasAttacked = true;
            MoveDistance = 2;
            HealthPoints = 15;
            CurrentHealth = HealthPoints;
            Strength = 6;
            PDefense = 8;
            MDefense = 14;
            Dexterity = 8;
            Intelligence = 14;
            AttackRange = 3;
            Alive = true;
            CharType = "mercenary";
            Cost = 150;
            Attacked = 0;
            championCommander = GetChampionCommander((List<Character>)game.Services.GetService(typeof(List<Character>)), this);
            Level = 1;
            Experience = 0;
        }

        public override void Initialize()
        {   
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            content = GetContentManager();
            content.RootDirectory = "Content\\Priest";
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
                walkScaled = content.Load<Texture2D>("walk_scaled_0");
            }
            else
            {
                walk[0] = content.Load<Texture2D>("walk_0_2");
                walk[1] = content.Load<Texture2D>("walk_1_2");
                attack[0] = content.Load<Texture2D>("attack_0_2");
                attack[1] = content.Load<Texture2D>("attack_1_2");
                walkScaled = content.Load<Texture2D>("walk_scaled_2");
            }

            heal[0] = content.Load<Texture2D>("heal_0");
            heal[1] = content.Load<Texture2D>("heal_1");
            heal[2] = content.Load<Texture2D>("heal_2");
            
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (attackPos.X < 4500)
            {
                attackPos.X += (float).043 * attackPos.X;
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
                
                if (attackPos.X <= 300)
                {
                    spriteBatch.Draw(attack[0], new Vector2(50, 250), Color.White);
                }
                else if(attackPos.X > 300 && attackPos.X < 535)
                {
                    spriteBatch.Draw(attack[1],  new Vector2(50, 250), Color.White);
                    spriteBatch.Draw(heal[0], new Vector2(600, 250), Color.White);
                }
                else if (attackPos.X >= 535 && attackPos.X < 1000)
                {
                    spriteBatch.Draw(attack[1], new Vector2(50, 250), Color.White);
                    spriteBatch.Draw(heal[1], new Vector2(600, 250), Color.White);

                    spriteBatch.DrawString(spriteFont, "+" + (int)damage, new Vector2(630, 200), Color.Green);
                    spriteBatch.DrawString(spriteFont, "Exp: +" + experienceGain, new Vector2(50, 150), Color.Gold);
                    
                }
                else if (attackPos.X >= 100 && attackPos.X < 4500)
                {  
                    attackPos.X = 700;
                    spriteBatch.Draw(attack[1], new Vector2(50, 250), Color.White);
                    spriteBatch.Draw(heal[2], new Vector2(600, 250), Color.White);

                    spriteBatch.DrawString(spriteFont, "+" + (int)damage, new Vector2(630, 200), Color.Green);
                    spriteBatch.DrawString(spriteFont, "Exp: +" + experienceGain, new Vector2(50, 150), Color.Gold);

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
            

            damage = ((float)Intelligence / (float)(Intelligence)) * (float)Intelligence + attacked.Intelligence;
            attacked.CurrentHealth += (int)damage;
            experienceGain = (attacked.Level - Level + 1) * 20;

            Experience += experienceGain;
            if (Experience >= 100) LevelUp();
            
        }

        public override void LevelUp()
        {
            base.LevelUp();

            Level++;
            HealthPoints += 5;
            CurrentHealth = HealthPoints;
            Strength += 1;
            Dexterity += 1;
            Intelligence += 7;
            PDefense += 1;
            MDefense += 1;
            Experience = 0;
        }
    }
}
