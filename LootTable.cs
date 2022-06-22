using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Loot{
    public PowerUp thisLoot;
    public int lootChance;
}
[CreateAssetMenu(menuName = "MySOs/LootTable")]
public class LootTable : ScriptableObject
{
    public Loot[] loots;

    public PowerUp LootPowerUp(){

        int cumProb = 0;
        int currentProb = Random.Range(0, 100);
        for (int i = 0; i < loots.Length; i++){
            cumProb += loots[i].lootChance;
            if (currentProb <= cumProb){
                return loots[i].thisLoot;
            }
        }
        return null;
    }
}
