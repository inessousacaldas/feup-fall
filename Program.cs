using System.Collections.Generic;
using System;

namespace PlatformGeneration
{

    class Library
    {
        public static float width = 79f;

        public Library()
        {
        }
    }

    class Platform
    {
        public static float height = 5;
        public static float width = 10;
        public float posX;
        public float posY;

        public Platform(float xPos, float yPos)
        {
            posX = xPos;
            posY = yPos;
        }

        public bool overlaps(Platform other)
        {
            if (posX < (other.posX + Platform.width) && (posX + Platform.width) > other.posX &&
                    posY < (other.posY + Platform.height) && (posY + Platform.height) > other.posY)
                return true;
            return false;
        }
    }

    class Player
    {
        public static float localRun = 0;
        public static float distanceRun = 0;
        public float largestWidth;
        public float largestHeight;

        public Player()
        {
        }
    }

    class PlatformManager /*: MonoBehaviour*/
    {
        Queue<Platform> platforms;
        public float recycleOffset;
        public float generatedOffset;

        public int destructibleRate;
        public int fixedRate;

        private Random rnd;
        private int maxIterations = 10;

        public PlatformManager()
        {
            destructibleRate = 10;
            fixedRate = 8;
        }


        public void Start()
        {
            platforms = new Queue<Platform>();
            Recycle(0);
        }

        public void Update()
        {
            if(Player.localRun > recycleOffset)
            {
                Recycle(Player.localRun);
                Player.localRun = 0;
            }
        }

        private Platform createNewPlatform()
        {
            bool validPlatform = false;
            int iterations = 0;
            Platform temp = new Platform(-1, 1);

            while (!validPlatform && iterations < maxIterations) {
                // position must be contained in limits
                float xPos = (float)rnd.NextDouble() * Library.width;
                float yPos = (float)rnd.NextDouble() * (generatedOffset - recycleOffset);

                temp = new Platform(xPos, yPos);

                //plataform cannot overlap other plataforms
                foreach (Platform p in platforms)
                {
                    if (temp.overlaps(p))
                    {
                        iterations++;
                        continue;
                    }
                }

                //plataform must leave space to player progress

                //if there are no problems
                return temp;
            }

            //return an invalid platform if number of iterations are superior to maximum
            return temp;
        }

        private void Recycle(float startPoint)
        {

            int nbDes = rnd.Next(0, destructibleRate + 1);
            int nbFixed = rnd.Next(0, fixedRate + 1);

            for (int i = 0; i < nbDes; i++)
            {
                
            }
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            PlatformManager pm = new PlatformManager();

        }
    }
}
