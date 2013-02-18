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
    class Character : DrawableGameComponent
    {

        protected Vector2 position;
        protected SpriteFont spriteFont;
        RandomNumberGenerator random;
        ContentManager content;
        BattleMap map;
        Boolean isAlive;
        Boolean moving = false;
        protected PlayerManager playerManager;
        int level;
        int playerIndex;
        int Maxhealth;
        int currentHealth;
        int strength;
        int dexterity;
        int intelligence;
        int pDefense;
        int mDefense;
        int moveDistance;
        int attackRange;
        int initiative;
        int attacked = 0;
        int attacking = 0;
        int cost;
        int gold;
        int exp;
        Boolean hasMoved;
        Boolean hasAttacked;
        string charType;

        public Boolean Moving
        {
            get { return moving; }
            set { moving = value; }
        }

        public int Experience
        {
            get { return exp; }
            set { exp = value; }
        }

        public int Level
        {
            get { return level; }
            set { level = value; }
        }
        public int Gold
        {
            get { return gold; }
            set { gold = value; }
        }

        public int Cost
        {
            get { return cost; }
            set { cost = value; }
        }

        public string CharType
        {
            get { return charType; }
            set { charType = value; }
        }

        public Boolean HasMoved
        {
            get { return hasMoved; }
            set { hasMoved = value; }
        }

        public Boolean HasAttacked
        {
            get { return hasAttacked; }
            set { hasAttacked = value; }
        }

        public int PlayerIndex
        {
            get { return playerIndex; }
            set { playerIndex = value; }
        }

        public int AttackRange
        {
            get { return attackRange; }
            set { attackRange = value; }
        }

        public Boolean Alive
        {
            get { return isAlive; }
            set { isAlive = value; }
        }

        public int HealthPoints
        {
            get { return Maxhealth; }
            set { Maxhealth = value; }
        }

        public int CurrentHealth
        {
            get { return currentHealth; }
            set { currentHealth = value; }
        }

        public int Attacking
        {
            get { return attacking; }
            set { attacking = value; }
        }

        public int Attacked
        {
            get { return attacked; }
            set { attacked = value; }
        }

        public int Strength
        {
            get { return strength; }
            set { strength = value; }
        }
        public int Dexterity
        {
            get { return dexterity; }
            set { dexterity = value; }
        }
        public int Intelligence
        {
            get { return intelligence; }
            set { intelligence = value; }
        }
        public int PDefense
        {
            get { return pDefense; }
            set { pDefense = value; }
        }
        public int MDefense
        {
            get { return mDefense; }
            set { mDefense = value; }
        }
        public int Initiative
        {
            get { return initiative; }
            set { initiative = value; }
        }
        public int MoveDistance
        {
            get { return moveDistance; }
            set { moveDistance = value; }
        }


        public Character(Game game, int xLoc, int yLoc)
            : base(game)
        {
            position = new Vector2(xLoc*60+12, yLoc*60+27);
            content = new ContentManager(game.Services, "Content");
            map = (BattleMap)game.Services.GetService(typeof(BattleMap));
            playerManager = (PlayerManager)game.Services.GetService(typeof(PlayerManager));
            currentHealth = Maxhealth;
            random = new RandomNumberGenerator();
        }

        public ContentManager GetContentManager()
        {
            return content;
        }

        public virtual void Move(int x, int y)
        {
            
        }

        public virtual void Attack(int x, int y)
        {

        }

        public Vector2 getPosition()
        {
            return position;
        }

        protected int RandomNumber(int min, int max)
        {
            return random.RandomNumber(min, max);
        }

        public virtual void BuyMercenary()
        {

        }

        public virtual void LevelUp()
        {

        }

        protected Character GetChampionCommander(List<Character> charList, Character thisChar)
        {
            foreach (Character character in charList)
            {
                if (character.PlayerIndex == thisChar.PlayerIndex && character.CharType == "champion")
                {
                    Character championCommander = character;
                    return championCommander;
                }
            }
            return null;
        }
    }
}
