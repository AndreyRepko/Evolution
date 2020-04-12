using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Evolution.Game.Model
{
    public class RandomTransformer
    {
        public static int GetChange()
        {
            var probability = new Dictionary<int, int>();
            probability[0] = 300;
            probability[1] = 50;
            probability[2] = 27;
            probability[3] = 17;
            probability[4] = 6;
            var funk = new int[9];
            int sum = 0;
            for (var i = 0; i < 9; i++)
            {
                var index = Math.Abs(4 - i);
                sum += probability[index];
                funk[i] = sum;
            }
            var random = RandomNumberGenerator.GetInt32(0, sum);
            for (var i = 0; i < 9; i++)
                if (funk[i] > random)
                {
                    return i - 4;
                }
            throw new InvalidOperationException();
        }
    }
}
