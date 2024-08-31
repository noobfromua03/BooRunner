using System;
using UnityEngine;

namespace RandomGeneratorWithWeight
{
    //extend this class for more functionality
    [Serializable]
    public class ItemForRandom <T> : IItem
    {
        [SerializeField] private int _weight;

        [SerializeField] private T _item;

        public int GetWeight() => _weight;

        public T GetItem() => _item;

        public ItemForRandom<T> WithWeight(int weight)
        {
            _weight = weight;
            return this;
        }
        public ItemForRandom<T> WithItem(T item)
        {
            _item = item;
            return this;
        }
    }
}
