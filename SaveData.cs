using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float playerHealth;
    public string sceneName;
    public Inventory playerInventory;
    public Vector2 playerPosition;
    
    public List<BoolValueStruct> bools = new List<BoolValueStruct>();

    [System.Serializable]
    public struct BoolValueStruct{
        public int bv_ID;
        public bool bv_value;
        public string bv_type;
    }
    
    public string ToJson(){
        return JsonUtility.ToJson(this);
    }
    public void LoadFromJson(string a_Json){
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(List<SaveData.BoolValueStruct> boolValues);
}
