using System;
using System.Collections.Generic;
using System.Linq;

namespace RandomGeneratorWithWeight
{
    static class GetItemWithWeight
    {
        static Random rnd = new Random();

        class Segment<T>
        {
            public int begin;
            public int end;
            public T item;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> type of needed value: int, GAmeObject etc </typeparam>
        /// <param name="items">List of items typeof ItemForRandom</param>
        /// <returns>return random item from list. item depends on weight value</returns>
        public static T GetItem<T>(List<ItemForRandom<T>> items)
        {
            if (items.Count == 0)
                throw new Exception("List is empty!");

            return items[GetIndex(items)].GetItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> type of needed value: int, GAmeObject etc </typeparam>
        /// <param name="items">List of items typeof ItemForRandom</param>
        /// <returns>return random item from list. item depends on weight value</returns>
        public static (T, int) GetItemWithIndex<T>(List<ItemForRandom<T>> items)
        {
            if (items.Count == 0)
                throw new Exception("List is empty!");

            var index = GetIndex(items);
            return (items[index].GetItem(), index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> type of needed value: int, GAmeObject etc </typeparam>
        /// <param name="items">List of items typeof ItemForRandom</param>
        /// <returns>return random item from list. item depends on weight value</returns>
        public static T GetItem<T>(ItemForRandom<T>[] items)
        {
            return GetItem(items.OfType<ItemForRandom<T>>().ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> type of needed value: int, GAmeObject etc </typeparam>
        /// <param name="items">List of items typeof ItemForRandom</param>
        /// <returns>return random item from list. item depends on weight value</returns>
        public static int GetIndex<T>(List<ItemForRandom<T>> items)
        {
            var segments = SetSegmentsForItems(items);

            int randomValue = rnd.Next(segments[segments.Count - 1].end);

            return segments.FindIndex(element => element.begin <= randomValue && element.end > randomValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"> type of needed value: int, GAmeObject etc </typeparam>
        /// <param name="items">List of items typeof ItemForRandom</param>
        /// <returns>return random item from list. item depends on weight value</returns>
        public static int GetIndex<T>(ItemForRandom<T>[] items)
        {
            return GetIndex(items.OfType<ItemForRandom<T>>().ToList());
        }

        static List<Segment<T>> SetSegmentsForItems<T>(List<ItemForRandom<T>> items)
        {
            List<Segment<T>> segments = new List<Segment<T>>()
            {
                new Segment<T> { begin = 0, end = items[0].GetWeight(), item = items[0].GetItem() }
            };


            for (int i = 1; i < items.Count; ++i)
            {
                segments.Add(new Segment<T>());

                segments[i].begin = segments[i - 1].end;
                segments[i].end = segments[i].begin + items[i].GetWeight();

                segments[i].item = items[i].GetItem();
            }

            return segments;
        }
    }
}
