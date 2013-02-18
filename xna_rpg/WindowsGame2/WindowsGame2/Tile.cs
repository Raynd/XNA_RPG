using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2
{
    class Tile
    {
        private string terrain;
        private Character currentChar = null;
        private bool isSelected = false;
        private bool isMovable = false;
        private bool isAttackable = false;

        public bool IsAttackable
        {
            get { return isAttackable; }
            set { isAttackable = value; }
        }

        public bool IsMovable
        { 
            get{ return isMovable; }
            set{ isMovable = value; }
        }

        public Tile(string terrainType)
        {
            terrain = terrainType;
        }

        public void setCurrentChar(Character character)
        {
            currentChar = character;
        }

        public Character getCurrentChar()
        {
            return currentChar;
        }

        public string getTerrainType()
        {
            return terrain;
        }

        public void TileSelect()
        {
            isSelected = true;
        }

        public void TileDeselect()
        {
            isSelected = false;
        }

        public bool isTileSelected()
        {
            return isSelected;
        }
    }
}
