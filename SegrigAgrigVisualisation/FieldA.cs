using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace SegrigAgrigVisualisation
{
    class FieldA
    {
        int grid_step;
        List<ParicleA> particles;
        int kX;
        int kY;
        int amnt;
        int[,] wall;
        public int countC1, countL1, countT1, countC2, countL2, countT2;

        public FieldA(int _w, int _h, int _g, int _amnt)
        {
            grid_step = _g;
            kX = _w / _g;
            kY = _h / _g;
            amnt = _amnt;
            particles = new List<ParicleA>();
            wall = new int[kX, kY];
            countC1 = countL1 = countT1 = countC2 = countL2 = countT2 = 0;

            for (var i = 0; i < amnt; ++i)
            {
                ParicleA part;
                bool f;
                do
                {
                    part = new ParicleA(kY);

                    f = particles.Any(p => p.X == part.X && p.Y == part.Y);
                } while (f);
                particles.Add(part);
            }

            for (int i = 0; i < kX/2; i++)
            {
                wall[i, kY/3] = 1;
                wall[i, (kY/3)*2] = 1;
            }

            for (int i = 0; i < (kX/3)*2; i++)
            {
                wall[i, 0] = 1;
                wall[i, kY - 1] = 1;
            }

            for (int i = kY / 3; i < (kY / 3) * 2+1; i++)
                wall[(kX/2), i] = 1;

            for (int i = 0; i < kY / 3; i++)
                wall[(kX/3)*2, i] = 1;

            for (int i = (kY / 3) * 2+1; i < kY; i++)
                wall[(kX / 3) * 2, i] = 1;

            for (int i = (kX / 3) * 2; i < kX; i++)
            {
                wall[i, kY/3] = 1;
                wall[i, (kY / 3)*2+1] = 1;
            }

            for (int i = kY / 3; i < (kY / 3) * 2 + 1; i++)
                wall[kX-1, i] = 1;
        }

        private Vector NextPos(ParicleA p)
        {
            var v = new Vector(p.X, p.Y);
            var oldV = new Vector(p.X, p.Y);
            if (p.Go())
            {
                var rand = new Random((int)DateTime.Now.Ticks);
                var m = (float)rand.NextDouble();
                if (0.6 >= m)
                {
                    if (p.type==0)
                        v.Y += 1;
                    else if (p.type==1)
                        v.Y -= 1;
                }
                if (feelWall(v))
                    v.Y = oldV.Y;
                if(v.Y==oldV.Y)
                    v.X += 1;
                if (feelWall(v))
                    v.X = oldV.X;
            }

            if (p.type == 0 && v.Y == kY / 2)
                p.type = 2;

            if (p.type == 1 && v.Y == kY / 2)
                p.type = 2;

            if (v.X == kX / 3 && !p.isCnt1)
            {
                if (v.Y < kY / 2)
                    countC1++;
                else
                    countL1++;
                p.isCnt1 = true;
            }

            if ((v.X == (kX / 3) * 2 + 2) && !p.isCnt2)
            {
                countC2++;
                p.isCnt2 = true;
            }

            return v;
        }

        private bool feelWall(Vector parts)
        {                   //true при попадании в стену
            if (wall[(int)parts.X, (int)parts.Y] == 1)
                return true;
            return false;

        }

        public void Move()
        {
            countT1 = countC1 + countL1;

            countT2 += countC2;
            countL2 = countC2;
            countC2 = 0;
            
            foreach (var t in particles)
            {
                var v = NextPos(t);
                if (v.X == kX) v.X = kX - 1 ;
                if (v.Y == -1) 
                    v.Y = 0;
                else if (v.Y == kY)
                    v.Y = kY-1;

                var f = particles.All(t1 => v.Y != t1.Y || v.X != t1.X);
                if (!f) continue;
                t.X = (int)v.X;
                t.Y = (int)v.Y;
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in particles)
            {
                Brush b;
                if(p.col < kY/2)
                    b = new SolidBrush(Color.Green);
                else
                    b = new SolidBrush(Color.Red);
                g.FillEllipse(b, p.X * grid_step + 2, p.Y * grid_step + 2, grid_step - 5, grid_step - 5);

            }
            for (int i = 0; i < kX; i++)
            {
                for (int j = 0; j < kY; j++)
                {
                    if (wall[i, j] == 1)
                    {
                        g.FillRectangle(Brushes.Black, i * grid_step, j * grid_step, grid_step, grid_step);
                    }
                }

            }
        }
    }
}
