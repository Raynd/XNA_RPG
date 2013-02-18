using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{
    class Cursor : DrawableGameComponent
    {
        private Vector2 pos;
        Texture2D square;
        Character selectedCharacter;
        Character movingCharacter;
        Character attackingCharacter;
        Character attackedCharacter;
        Tile currentTile;
        Texture2D cursor;
        ContentManager content;
        SpriteBatch spriteBatch;
        BattleMap map;
        GamePadState currentButtonState;
        KeyboardState currentKeyState;
        GamePadState oldButtonState;
        KeyboardState oldKeyState;
        ScreenManager screenManager;
        GameStateManager gameStateManager;
        PlayerManager playerManager;
        Game game;
        int buttonFlag = 0;
        int moveFlag = 0;
        int attackFlag = 0;

        public Cursor(Game game)
            : base(game)
        {
            pos = new Vector2(20, 0);
            this.game = game;
            Initialize();
            LoadContent();
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            currentTile = SelectTile(0, 0);
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
        }

        public override void Initialize()
        {
            content = new ContentManager(Game.Services);
            content.RootDirectory = "Content";
            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));
            base.Initialize();
        }

        protected override void LoadContent()
        {
            cursor = content.Load<Texture2D>("cursor");
            square = content.Load<Texture2D>("empty_square");
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (gameStateManager.State == GameState.playing)
            {
                currentButtonState = GamePad.GetState(PlayerIndex.One);
                currentKeyState = Keyboard.GetState(PlayerIndex.One);

                if ((currentButtonState.DPad.Left == ButtonState.Pressed || currentButtonState.ThumbSticks.Left.X < 0 || currentKeyState.IsKeyDown(Keys.Left)))
                {
                    currentTile.TileDeselect();
                    pos.X -= 60;

                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.DPad.Left == ButtonState.Pressed && oldButtonState.DPad.Left == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Left) && oldKeyState.IsKeyDown(Keys.Left)))
                    {
                        pos.X += 60;
                    }
                    else
                    {
                        buttonFlag = 0;
                    }

                    if (pos.X < 0) pos.X = 20;
                    currentTile = SelectTile((int)pos.Y / 60, (int)pos.X / 60);
                }

                if ((currentButtonState.DPad.Right == ButtonState.Pressed || currentButtonState.ThumbSticks.Left.X > 0 || currentKeyState.IsKeyDown(Keys.Right)))
                {
                    currentTile.TileDeselect();
                    pos.X += 60;

                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.DPad.Right == ButtonState.Pressed && oldButtonState.DPad.Right == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Right) && oldKeyState.IsKeyDown(Keys.Right)))
                    {
                        pos.X -= 60;
                    }
                    else
                    {
                        buttonFlag = 0;
                    }

                    if ((int)pos.X / 60 > map.getWidth() - 1) pos.X -= 60;
                    currentTile = SelectTile((int)pos.Y / 60, (int)pos.X / 60);
                }

                if ((currentButtonState.DPad.Up == ButtonState.Pressed || currentButtonState.ThumbSticks.Left.Y > 0 || currentKeyState.IsKeyDown(Keys.Up)))
                {
                    currentTile.TileDeselect();
                    pos.Y -= 60;

                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.DPad.Up == ButtonState.Pressed && oldButtonState.DPad.Up == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Up) && oldKeyState.IsKeyDown(Keys.Up)))
                    {
                        pos.Y += 60;
                    }
                    else
                    {
                        buttonFlag = 0;
                    }

                    if (pos.Y < 0) pos.Y = 0;
                    currentTile = SelectTile((int)pos.Y / 60, (int)pos.X / 60);
                }

                if ((currentButtonState.DPad.Down == ButtonState.Pressed || currentButtonState.ThumbSticks.Left.Y < 0 || currentKeyState.IsKeyDown(Keys.Down)))
                {
                    currentTile.TileDeselect();
                    pos.Y += 60;

                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.DPad.Down == ButtonState.Pressed && oldButtonState.DPad.Down == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Down) && oldKeyState.IsKeyDown(Keys.Down)))
                    {
                        pos.Y -= 60;
                    }
                    else
                    {
                        buttonFlag = 0;
                    }

                    if ((int)pos.Y / 60 > map.getHeight() - 1) pos.Y -= 60;
                    currentTile = SelectTile((int)pos.Y / 60, (int)pos.X / 60);
                }

                if ((currentButtonState.Buttons.A == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.Space)))
                {
                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.Buttons.A == ButtonState.Pressed && oldButtonState.Buttons.A == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyDown(Keys.Space)))
                    {
                        //Do Nothing
                        buttonFlag = 0;
                    }
                    else
                    {
                        selectedCharacter = map.GetSquare((int)pos.Y / 60, (int)pos.X / 60).getCurrentChar();
                        if (selectedCharacter == null && moveFlag == 0 && attackFlag == 0)
                        {
                            screenManager.AddScreen(new PopupScreen(game, null), null);
                            buttonFlag = 1;
                        }
                        if (selectedCharacter != null && moveFlag == 0 && attackFlag == 0 && selectedCharacter.PlayerIndex == playerManager.GetCurrentPlayer())
                        {
                            screenManager.AddScreen(new PopupScreen(game, selectedCharacter), null);
                            buttonFlag = 1;
                        }

                        if (moveFlag == 1 && buttonFlag == 1 && map.GetSquare((int)pos.Y / 60, (int)pos.X / 60).getCurrentChar() == null )
                        {
                            if(map.GetSquare((int)pos.X / 60, (int)pos.Y / 60).IsMovable)
                            {
                                movingCharacter.Move((int)pos.X / 60, (int)pos.Y / 60);
                                ClearBoard();
                            }
                        }

                        if (attackFlag == 1 && buttonFlag == 1 && map.GetSquare((int)pos.Y / 60, (int)pos.X / 60).getCurrentChar() != null)
                        {
                            if (map.GetSquare((int)pos.X / 60, (int)pos.Y / 60).IsAttackable)
                            {
                                attackedCharacter = map.GetSquare((int)pos.Y / 60, (int)pos.X / 60).getCurrentChar();
                                attackedCharacter.Attacked = 1;
                                attackingCharacter.Attacking = 1;
                                attackingCharacter.Attack((int)pos.Y / 60, (int)pos.X / 60);

                                screenManager.AddScreen(new BattleScreen(game, attackingCharacter, attackedCharacter), null);
                                gameStateManager.State = GameState.combat;

                                ClearBoard();
                            }
                        }
                    }
                }

                if ((currentButtonState.Buttons.B == ButtonState.Pressed || currentKeyState.IsKeyDown(Keys.B)))
                {
                    if (buttonFlag == 0)
                    {
                        buttonFlag = 1;
                    }
                    if ((currentButtonState.Buttons.A == ButtonState.Pressed && oldButtonState.Buttons.A == ButtonState.Pressed) || (currentKeyState.IsKeyDown(Keys.Space) && oldKeyState.IsKeyDown(Keys.Space)))
                    {
                        //Do Nothing
                        buttonFlag = 0;
                    }
                    if (moveFlag == 1 && buttonFlag == 1)
                    {
                        movingCharacter.HasMoved = false;
                        movingCharacter = null;
                        selectedCharacter = null;
                        moveFlag = 0;
                        buttonFlag = 0;

                         for (int i = 0; i < map.getHeight(); i++)
                         {
                            for (int j = 0; j < map.getWidth(); j++)
                            {
                                    map.GetSquare(i, j).IsMovable = false;
                            }
                         }
                    }
                    if (attackFlag == 1 && buttonFlag == 1)
                    {
                        attackingCharacter.HasAttacked = false;
                        attackingCharacter = null;
                        attackFlag = 0;
                        buttonFlag = 0;
                        
                        for (int i = 0; i < map.getHeight(); i++)
                        {
                            for (int j = 0; j < map.getWidth(); j++)
                            {
                                map.GetSquare(i, j).IsAttackable = false;
                            }
                        }
                    }
                }

                oldButtonState = currentButtonState;
                oldKeyState = currentKeyState;
            }
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();

            if (gameStateManager.State == GameState.playing)
            {
                spriteBatch.Draw(cursor, pos, Color.White);
            }

            if (moveFlag == 1 || attackFlag == 1)
            {
                for (int i = 0; i < map.getHeight(); i++)
                {
                    for (int j = 0; j < map.getWidth(); j++)
                    {
                        //Console.WriteLine((int)pos.Y/60 +" , " + (int)pos.X/60);
                        if (map.GetSquare(i, j).IsMovable == true)
                        {
                            spriteBatch.Draw(square, new Vector2(i*60,j*60+20), new Color(0,250,0,5));
                        }

                        if (map.GetSquare(i, j).IsAttackable == true)
                        {
                            spriteBatch.Draw(square, new Vector2(i * 60, j * 60 + 20), new Color(250, 0, 5, 5));
                        }
                    }
                }
            }
            
            spriteBatch.End();
        }

        public Tile SelectTile(int x, int y)
        {
            map.GetSquare(x, y).TileSelect();
            return map.GetSquare(x, y);
        }

        public void MoveHelper(Character character)
        {
            moveFlag = 1;
            movingCharacter = character;

            Vector2 charPos = movingCharacter.getPosition();

            for (int i = 0; i < map.getHeight(); i++)
            {
                for (int j = 0; j < map.getWidth(); j++)
                {
                    if (((Math.Abs((int)charPos.Y / 60 - j)) + (Math.Abs((int)charPos.X / 60 - i))) <= movingCharacter.MoveDistance
                        && map.GetSquare(j,i).getCurrentChar() == null)
                    {
                        map.GetSquare(i, j).IsMovable = true;
                    }
                }
            }

        }

        public void AttackHelper(Character character)
        {
            attackFlag = 1;
            attackingCharacter = character;

            Vector2 charPos = attackingCharacter.getPosition();

            for (int i = 0; i < map.getHeight(); i++)
            {
                for (int j = 0; j < map.getWidth(); j++)
                {

                    if (((Math.Abs((int)charPos.Y / 60 - j)) + (Math.Abs((int)charPos.X / 60 - i))) <= attackingCharacter.AttackRange)
                    {
                        map.GetSquare(i, j).IsAttackable = true;
                    }
                }
            }
        }

        public void ClearBoard()
        {
            moveFlag = 0;
            buttonFlag = 0;
            attackFlag = 0;
            movingCharacter = null;
            attackFlag = 0;
            attackingCharacter = null;
            attackedCharacter = null;
            selectedCharacter = null;

            for (int i = 0; i < map.getHeight(); i++)
            {
                for (int j = 0; j < map.getWidth(); j++)
                {
                    map.GetSquare(i, j).IsMovable = false;
                    map.GetSquare(i, j).IsAttackable = false;
                }
            }

        }

    }
}
