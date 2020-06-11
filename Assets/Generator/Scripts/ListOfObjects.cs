using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemList
{
    public GameObject item;
    public int randomMinimum;
    public int randomMaximum;
    public ItemList(GameObject item, int randomMinimum, int randomMaximum)
    {
        this.item = item;
        this.randomMinimum = randomMinimum;
        this.randomMaximum = randomMaximum;
    }
}

[System.Serializable]
public class EnemyList
{
    public GameObject enemy;
    public int count;
    public EnemyList(GameObject enemy, int count)
    {
        this.enemy = enemy;
        this.count = count;
    }
}

