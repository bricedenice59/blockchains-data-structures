using System;

namespace BlockchainDataStructures.RadixDataStructure.Utils
{
    public static class ListUtilsExtension
    {   
        public static List<T> Shuffle<T>(this List<T> input)
        {
            var array = input.ToArray();
            FisherYatesShuffle(array);
            return array.ToList();
        }

        //https://stackoverflow.com/questions/9557883/random-slot-algorithm
        private static void FisherYatesShuffle<T>(T[] array)
        {
            Random r = new Random();
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = r.Next(0, i + 1);
                T temp = array[j];
                array[j] = array[i];
                array[i] = temp;
            }
        }
    }
}
