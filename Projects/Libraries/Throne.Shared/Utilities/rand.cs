using System;

namespace Throne.Framework.Utilities
{
    public class rand
    {
        private static int _staticSeed;
        private int _thisSeed;

        static rand()
        {
            _staticSeed = Environment.TickCount;
        }

        public rand(int seed)
        {
            _thisSeed = seed;
        }

        public int Next()
        {
            _thisSeed *= 0x343fd;
            _thisSeed += 0x269Ec3;
            return (_thisSeed >> 0x10) & 0x7FFF;
        }

        public int Next(int min, int max)
        {
            return (Next()%(((max) + 1) - (min))) + (min);
        }

        public static void SRand(int seed)
        {
            _staticSeed = seed;
        }

        public static int SNext()
        {
            _staticSeed *= 0x343fd;
            _staticSeed += 0x269Ec3;
            return (_staticSeed >> 0x10) & 0x7FFF;
        }

        public static int SNext(int max)
        {
            return (SNext()%(max + 1));
        }

        //Inclusive
        /// <summary>
        ///     INCLUSIVE LOL
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int SNext(int min, int max)
        {
            return (SNext()%(((max) + 1) - (min))) + (min);
        }
    }
}