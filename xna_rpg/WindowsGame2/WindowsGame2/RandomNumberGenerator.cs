using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsGame2
{
    class RandomNumberGenerator
    {
        Random random;

        public RandomNumberGenerator()
        {
            random = new Random();
        }


        public int RandomNumber(int low, int high)
        {
            return random.Next(low, high);
        }
    }
}
