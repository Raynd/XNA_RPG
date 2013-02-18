using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace WindowsGame2
{
    class BuyScreen : MenuScreen
    {
        InputAction buyKnight;
        InputAction buyBarbarian;
        InputAction buyRanger;
        InputAction buyMage;
        InputAction buyPriest;

        GameStateManager gameStateManager;
        Cursor cursor;
        Character champion;
        PlayerManager playerManager;
        Game game;
        BattleMap map;
        List<Character> charList;
        ScreenManager screenManager;

        public BuyScreen(Game game, Character character)
            : base("Hire a Mercenary")
        {
            

            cursor = (Cursor)game.Services.GetService(typeof(Cursor));
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            charList = (List<Character>)game.Services.GetService(typeof(List<Character>));
            screenManager = (ScreenManager)game.Services.GetService(typeof(ScreenManager));
            champion = character;
            this.game = game;

            MenuEntry buyKnight = new MenuEntry("Hire Knight, Cost: 100");
            MenuEntry buyRanger = new MenuEntry("Hire Ranger, Cost: 100");
            MenuEntry buyBarbarian = new MenuEntry("Hire Barbarian, Cost: 100");
            MenuEntry buyMage = new MenuEntry("Hire Mage, Cost: 150");
            MenuEntry buyPriest = new MenuEntry("Hire Spirit Priest, Cost: 150");
            MenuEntry cancel = new MenuEntry("Cancel");
            gameStateManager = (GameStateManager)game.Services.GetService(typeof(GameStateManager));

            gameStateManager.State = GameState.buying;
            // Hook up menu event handlers.
            buyKnight.Selected += buyKnightSelected;
            buyRanger.Selected += buyRangerSelected;
            buyBarbarian.Selected += buyBarbarianSelected;
            buyMage.Selected += buyMageSelected;
            buyPriest.Selected += buyPriestSelected;
            cancel.Selected += cancelSelected;

            // Add entries to the menu.
            MenuEntries.Add(buyKnight);
            MenuEntries.Add(buyRanger);
            MenuEntries.Add(buyBarbarian);
            MenuEntries.Add(buyMage);
            MenuEntries.Add(buyPriest);
            MenuEntries.Add(cancel);

            /*if (aiFlag == true)
            {
                BuyRandom();
            }*/
        }


        void cancelSelected(object sender, PlayerIndexEventArgs e)
        {
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }

        public void buyKnightSelected(object sender, PlayerIndexEventArgs e)
        {
            Character newKnight;
            Vector2 champPos = champion.getPosition();
            champPos.X = champPos.X / 60;
            champPos.Y = champPos.Y / 60;

            if ((int)champPos.Y + 1 < 10 && map.GetSquare((int)champPos.Y + 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newKnight = new Knight(game, (int)champPos.X, (int)champPos.Y + 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newKnight);
                charList.Add(newKnight);
                champion.Gold -= newKnight.Cost;
            }
            else if ((int)champPos.X + 1 < 10 && map.GetSquare((int)champPos.Y, (int)champPos.X + 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newKnight = new Knight(game, (int)champPos.X + 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newKnight);
                charList.Add(newKnight);
                champion.Gold -= newKnight.Cost;
            }
            else if ((int)champPos.Y - 1 >= 0 && map.GetSquare((int)champPos.Y - 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newKnight = new Knight(game, (int)champPos.X, (int)champPos.Y-1, playerManager.GetCurrentPlayer());
                game.Components.Add(newKnight);
                charList.Add(newKnight);
                champion.Gold -= newKnight.Cost;
            }
            else if ((int)champPos.X - 1 >= 0 && map.GetSquare((int)champPos.Y, (int)champPos.X - 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newKnight = new Knight(game, (int)champPos.X - 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newKnight);
                charList.Add(newKnight);
                champion.Gold -= newKnight.Cost;
            }
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }

        public void buyRangerSelected(object sender, PlayerIndexEventArgs e)
        {
            Character newRanger;
            Vector2 champPos = champion.getPosition();
            champPos.X = champPos.X / 60;
            champPos.Y = champPos.Y / 60;

            if ((int)champPos.Y + 1 < 10 && map.GetSquare((int)champPos.Y + 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newRanger = new Ranger(game, (int)champPos.X, (int)champPos.Y + 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newRanger);
                charList.Add(newRanger);
                champion.Gold -= newRanger.Cost;
            }
            else if ((int)champPos.X + 1 < 10 && map.GetSquare((int)champPos.Y, (int)champPos.X + 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newRanger = new Ranger(game, (int)champPos.X + 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newRanger);
                charList.Add(newRanger);
                champion.Gold -= newRanger.Cost;
            }
            else if ((int)champPos.Y - 1 >= 0 &&map.GetSquare((int)champPos.Y - 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newRanger = new Ranger(game, (int)champPos.X, (int)champPos.Y - 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newRanger);
                charList.Add(newRanger);
                champion.Gold -= newRanger.Cost;
            }
            else if ((int)champPos.X - 1 >= 0 && map.GetSquare((int)champPos.Y, (int)champPos.X - 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newRanger = new Ranger(game, (int)champPos.X - 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newRanger);
                charList.Add(newRanger);
                champion.Gold -= newRanger.Cost;
            }
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }

        public void buyBarbarianSelected(object sender, PlayerIndexEventArgs e)
        {
            Character newBarbarian;
            Vector2 champPos = champion.getPosition();
            champPos.X = champPos.X / 60;
            champPos.Y = champPos.Y / 60;

            if ((int)champPos.Y + 1 < 10 && map.GetSquare((int)champPos.Y + 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newBarbarian = new Barbarian(game, (int)champPos.X, (int)champPos.Y + 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newBarbarian);
                charList.Add(newBarbarian);
                champion.Gold = champion.Gold - newBarbarian.Cost;
            }
            else if ((int)champPos.X + 1 < 10 && map.GetSquare((int)champPos.Y, (int)champPos.X + 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newBarbarian = new Barbarian(game, (int)champPos.X + 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newBarbarian);
                charList.Add(newBarbarian);
                champion.Gold = champion.Gold - newBarbarian.Cost;
            }
            else if ((int)champPos.Y - 1 >= 0 && map.GetSquare((int)champPos.Y - 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newBarbarian = new Barbarian(game, (int)champPos.X, (int)champPos.Y - 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newBarbarian);
                charList.Add(newBarbarian);
                champion.Gold = champion.Gold - newBarbarian.Cost;
            }
            else if ((int)champPos.X - 1 >= 0 && map.GetSquare((int)champPos.Y, (int)champPos.X - 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newBarbarian = new Barbarian(game, (int)champPos.X - 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newBarbarian);
                charList.Add(newBarbarian);
                champion.Gold = champion.Gold - newBarbarian.Cost;
            }
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }

        public void buyMageSelected(object sender, PlayerIndexEventArgs e)
        {
            Character newMage;
            Vector2 champPos = champion.getPosition();
            champPos.X = champPos.X / 60;
            champPos.Y = champPos.Y / 60;

            if (map.GetSquare((int)champPos.Y + 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newMage = new Mage(game, (int)champPos.X, (int)champPos.Y + 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newMage);
                charList.Add(newMage);
                champion.Gold -= newMage.Cost;
            }
            else if (map.GetSquare((int)champPos.Y, (int)champPos.X + 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newMage = new Mage(game, (int)champPos.X + 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newMage);
                charList.Add(newMage);
                champion.Gold -= newMage.Cost;
            }
            else if (map.GetSquare((int)champPos.Y - 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newMage = new Mage(game, (int)champPos.X, (int)champPos.Y - 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newMage);
                charList.Add(newMage);
                champion.Gold -= newMage.Cost;
            }
            else if (map.GetSquare((int)champPos.Y, (int)champPos.X - 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newMage = new Mage(game, (int)champPos.X - 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newMage);
                charList.Add(newMage);
                champion.Gold -= newMage.Cost;
            }
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }

        public void buyPriestSelected(object sender, PlayerIndexEventArgs e)
        {
            Character newPriest;
            Vector2 champPos = champion.getPosition();
            champPos.X = champPos.X / 60;
            champPos.Y = champPos.Y / 60;

            if (map.GetSquare((int)champPos.Y + 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newPriest = new SpiritPriest(game, (int)champPos.X, (int)champPos.Y + 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newPriest);
                charList.Add(newPriest);
                champion.Gold -= newPriest.Cost;
            }
            else if (map.GetSquare((int)champPos.Y, (int)champPos.X + 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newPriest = new SpiritPriest(game, (int)champPos.X + 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newPriest);
                charList.Add(newPriest);
                champion.Gold -= newPriest.Cost;
            }
            else if (map.GetSquare((int)champPos.Y - 1, (int)champPos.X).getCurrentChar() == null && champion.Gold >= 100)
            {
                newPriest = new SpiritPriest(game, (int)champPos.X, (int)champPos.Y - 1, playerManager.GetCurrentPlayer());
                game.Components.Add(newPriest);
                charList.Add(newPriest);
                champion.Gold -= newPriest.Cost;
            }
            else if (map.GetSquare((int)champPos.Y, (int)champPos.X - 1).getCurrentChar() == null && champion.Gold >= 100)
            {
                newPriest = new SpiritPriest(game, (int)champPos.X - 1, (int)champPos.Y, playerManager.GetCurrentPlayer());
                game.Components.Add(newPriest);
                charList.Add(newPriest);
                champion.Gold -= newPriest.Cost;
            }
            gameStateManager.State = GameState.playing;
            screenManager.RemoveScreen(this);
        }
        
    }
}
