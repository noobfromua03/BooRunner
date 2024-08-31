using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomGeneratorWithWeight;

public class NumbersGenerator : MonoBehaviour
{
    [SerializeField] int countOfNumbers = 15;

    //int items for random
    [SerializeField] List<ItemForRandom<int>> numbersForRandom;

    //GameObject items for random
    [SerializeField] List<ItemForRandom<GameObject>>GOForRandom;

    void Start()
    {
        print("items:");

        //get items 
        for (int i = 0; i < countOfNumbers; ++i)
            print(GetItemWithWeight.GetItem(numbersForRandom));

        print("-----------------------------------------------");
        print("indexes:");

        //get items index
        for (int i = 0; i < countOfNumbers; ++i)
            print(GetItemWithWeight.GetIndex(numbersForRandom));
    }
}
