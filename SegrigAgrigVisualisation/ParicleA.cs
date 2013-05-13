using System;

namespace SegrigAgrigVisualisation
{
    class ParicleA
    {
        public float p;
        public int X;
        public int Y;
        public int type;
        public bool isCnt1, isCnt2;
        public int col;


        public ParicleA(int height, float _p = 2)
        {
            var rand = new Random();
            if (_p == 2)
            {
                do
                {
                    p = (float) (rand).NextDouble();
                } while (p < 0.35);
            }
            else
                p = _p;

            type = rand.Next(3);
            X = rand.Next(5);
            Y = rand.Next(height/3);
            if (type != 1 && type != 0)
                type = 1;
            if (type == 1)
            {
                Y += (height / 3)*2+1;
            }
            isCnt1 = isCnt2 = false;
            col = Y;
        }

        public bool Go()
        {
            var rand = new Random();
            var m = (float)rand.NextDouble();
            return p >= m;
        }

    }
}
