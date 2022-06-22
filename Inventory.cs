using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/Inventory")]
[System.Serializable]
public class Inventory : ScriptableObject
{
    public Item currentItem;
    public List<Item> items = new List<Item>();
    public int numberOfKeys;
    public int numberOfCoins;

    public void AddItem(Item item){
        currentItem = item;
        if (item.isKey){
            numberOfKeys++;
        }else if (item.isUnique){
            if (!items.Contains(item)){
                items.Add(item);
            }
        }
        else{
            var index = items.IndexOf(item);
            if (index == -1){
                items.Add(item);
            }else{
                items[index].numberHeld++;
            }
        }
    }
    public void GetCoins(int value){
        if (numberOfCoins + value > 999){
            numberOfCoins = 999;
        }else{
            numberOfCoins += value;
        }
    }
    public bool HasItem(string itemName){
        for (int i = 0; i < items.Count; i++){
            if (items[i].itemName == itemName){
                return true;
            }
        }
        return false;
    }
    public void Reset(){
        currentItem = null;
        items.Clear();
        numberOfKeys = 0;
        numberOfCoins = 0;
    }
}
