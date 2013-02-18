using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace WindowsGame2
{
    class PlayerManager
    {
        private int currentPlayer;
        List<Character> charList;
        Game1 game;
        OpponentAI ai;
        GameTime gameTime;

        public PlayerManager(Game1 game)
        {
            currentPlayer = 1;
            this.game = game;
            charList = (List<Character>)game.Services.GetService(typeof(List<Character>));
            this.gameTime = (GameTime)game.Services.GetService(typeof(GameTime));
        }

        public void LoadAI(Game1 game)
        {
            ai = (OpponentAI)game.Services.GetService(typeof(OpponentAI));
        }

        public void EndTurn()
        {
            if (currentPlayer == 1) currentPlayer = 2;
            else currentPlayer = 1;

            foreach( Character character in charList )
            {
                character.HasAttacked = false;
                character.HasMoved = false;
            }

            if (currentPlayer == 2 && game.GetAiFlag() == 1)
            {
                ai.StartTurn();
            }
        }

        public int GetCurrentPlayer()
        {
            return currentPlayer;
        }
    }
}
