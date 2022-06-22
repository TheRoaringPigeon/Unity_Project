using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/Item")]
[System.Serializable]
public class Item : ScriptableObject
{
    public Sprite itemSprite;
    public string itemName;
    public string itemDescription;
    public bool isKey;
    public bool isUnique;
    public int numberHeld;
}
