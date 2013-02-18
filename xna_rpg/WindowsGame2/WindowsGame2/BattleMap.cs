using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace WindowsGame2
{

    class BattleMap
    {
        private Tile[,] map;
        private int height;
        private int width;
        RandomNumberGenerator random;

        public BattleMap(Game game, int x, int y)
        {
            height = x;
            width = y;
            map = new Tile[x,y];
            random = new RandomNumberGenerator();
        }

        public void RandomMap()
        {
            int mapTheme = random.RandomNumber(1, 1);

            switch (mapTheme)
            {
                case 1: //Grassland
                    for (int i = 0; i < height; i++)
                    {
                        for (int j = 0; j < width; j++)
                        {
                            if (random.RandomNumber(1, 100) >= 30)
                            {
                                map[i, j] = new Tile("grass");
                            }
                            else
                            {
                                map[i, j] = new Tile("dirt");
                            }
                        }
                    }
                    break;
            }
        }

        public Tile GetSquare(int x, int y)
        {
            return map[x, y];
        }

        public int getWidth()
        {
            return width;
        }

        public int getHeight()
        {
            return height;
        }

        public void SetMovable(int x, int y, bool flag)
        {
            GetSquare(x, y).IsMovable = flag;
        }
    }
}
