using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSaveManager : MonoBehaviour, ISaveable
{
    [Header("FilePath")]
    public string zoneName;
    public string sceneName;
    [Header("Saveable Objects")]
    public BoolValue[] doors;
    public BoolValue[] chests;
    public BoolValue[] other;
    [Header("Player Stuff")]
    public VectorValue playerPosition;
    public Inventory playerInventory;
    public FloatValue playerHealth;
    [Header("Continue")]
    public BoolValue continueBool, FirstTimeInZone;
    public MySignal playerShouldMove, getPlayerPosition;
    public BoolValue[] gameSavers;

    private string SaveLocation;
    private string SceneFolder;
    private string GameSaverBools;
    private string GameSaverFolder;
    
    private void OnEnable(){
        SaveLocation = Application.persistentDataPath + "\\" + zoneName + "\\" + sceneName + "\\" + "SaveData.dat";
        SceneFolder = Application.persistentDataPath + "\\" + zoneName + "\\" + sceneName;
        GameSaverFolder = Application.persistentDataPath + "\\" + "GameSavers";
        GameSaverBools = Application.persistentDataPath + "\\" + "GameSavers" + "\\" + "SaveData.dat";
        System.IO.Directory.CreateDirectory(GameSaverFolder);
        System.IO.Directory.CreateDirectory(SceneFolder);
        if (FirstTimeInZone != null){
            UpdateGameSaverBool();
        }
        if (doors != null && doors.Length > 0 || chests != null && chests.Length > 0 || other != null && other.Length > 0){
            
            if (File.Exists(SaveLocation) && !FirstTimeInZone.initialValue){
                Load();
            }else if (FirstTimeInZone.initialValue){
                Reset();
                FirstTimeInZone.initialValue = false;
            }
        }
        if (continueBool.initialValue){
            StartCoroutine(MovePlayerCo());
        }
        if (FirstTimeInZone != null){
            FirstTimeInZone.initialValue = false;
        }
    }
    private void UpdateGameSaverBool(){
        if (File.Exists(GameSaverBools)){
            if (FileManager.LoadFromFile(GameSaverBools, out var json)){
                SaveData saveFile = new SaveData();
                SaveData newSaveFile = new SaveData();
                saveFile.LoadFromJson(json);
                foreach (SaveData.BoolValueStruct bvs in saveFile.bools){
                    if (bvs.bv_ID == FirstTimeInZone.myID){
                        FirstTimeInZone.initialValue = bvs.bv_value;
                        break;
                    }
                }
                foreach (SaveData.BoolValueStruct bvs in saveFile.bools){
                    if (bvs.bv_ID == FirstTimeInZone.myID){
                        SaveData.BoolValueStruct newBVS = new SaveData.BoolValueStruct{
                            bv_ID = bvs.bv_ID,
                            bv_type = bvs.bv_type,
                            bv_value = false,
                        };
                        Debug.Log("CHanged");
                        newSaveFile.bools.Add(newBVS);
                    }else{
                        SaveData.BoolValueStruct newBVS = new SaveData.BoolValueStruct{
                            bv_ID = bvs.bv_ID,
                            bv_type = bvs.bv_type,
                            bv_value = bvs.bv_value
                        };
                        newSaveFile.bools.Add(newBVS);
                    }
                }
                if (FileManager.WriteToFile(GameSaverBools, newSaveFile.ToJson())){
                    //Debug.Log("Save was successful.");
                }else{
                    Debug.Log("Something went wrong trying to update GameSavers.");
                }
            }else{
                Debug.Log("Couldn't Load from Json.");
            }
        }else{
            Debug.Log(GameSaverBools + " doesn't exist.");
        }
    }
    public void NewGame(){
        SaveData saveFile = new SaveData();
        foreach (BoolValue b in gameSavers){
            SaveData.BoolValueStruct bvs = new SaveData.BoolValueStruct{
                bv_ID = b.myID,
                bv_value = b.defaultValue,
                bv_type = b.boolType,
            };
            saveFile.bools.Add(bvs);
        }
        if (FileManager.WriteToFile(GameSaverBools, saveFile.ToJson())){
            //Debug.Log("Save was successful.");
        }else{
            Debug.Log("Something went wrong trying to save GameSavers.");
        }
    }
    private IEnumerator MovePlayerCo(){
        yield return new WaitForSeconds(0.1f);
        playerShouldMove.Raise();
        continueBool.initialValue = false;
    }
    private void OnDisable(){
        if (doors != null && doors.Length > 0 || chests != null && chests.Length > 0 || other != null && other.Length > 0){
            Save();
        }
    }
    public void Continue(){
        string PlayerSave = Application.persistentDataPath + "\\" + "LastState" + "\\" + "Player.dat";
        if (File.Exists(PlayerSave)){
            if (FileManager.LoadFromFile(PlayerSave, out var json)){
                SaveData saveFile = new SaveData();
                saveFile.LoadFromJson(json);
                this.playerPosition.initialValue = saveFile.playerPosition;
                this.playerInventory = saveFile.playerInventory;
                this.playerHealth.initialValue = saveFile.playerHealth;
                continueBool.initialValue = true;
                SceneManager.LoadScene(saveFile.sceneName);
            }
            else{
                Debug.Log("Player Save data not loaded.");
            }
        }else{
            Debug.Log("Player Save data does not exist.");
        }
    }
    public void SaveAndQuit(){
        Save();
        getPlayerPosition.Raise();
        string PlayerSave = Application.persistentDataPath + "\\" + "LastState" + "\\" + "Player.dat";
        string PlayerFolder = Application.persistentDataPath + "\\" + "LastState";
        System.IO.Directory.CreateDirectory(PlayerFolder);
        SaveData saveFile = new SaveData{
            playerHealth = this.playerHealth.initialValue,
            sceneName = this.sceneName,
            playerInventory = this.playerInventory,
            playerPosition = this.playerPosition.initialValue
        };
        if (FileManager.WriteToFile(PlayerSave, saveFile.ToJson())){
            Debug.Log("Player data saved");
        }else{
            Debug.Log("Player data was not saved");
        }
        SceneManager.LoadScene("StartMenu");
    }
    public void Save(){
        SaveData saveFile = new SaveData();
        PopulateSaveData(saveFile);
        if (FileManager.WriteToFile(SaveLocation, saveFile.ToJson())){
            //Debug.Log("Save was successful.");
        }else{
            Debug.Log("Something went wrong trying to save.");
        }
    }
    public void Load(){
        if (FileManager.LoadFromFile(SaveLocation, out var json)){
            SaveData saveFile = new SaveData();
            saveFile.LoadFromJson(json);
            LoadFromSaveData(saveFile.bools);
            //Debug.Log("Load was successful");
        }
        else{
            Debug.Log("Didn't Load");
        }
    }
    public void Reset(){
        Debug.Log("reset");
        foreach (BoolValue b in doors){
            b.Reset();
        }
        foreach (BoolValue b in chests){
            b.Reset();
        }
        foreach (BoolValue b in other){
            b.Reset();
        }
    }
    public void PopulateSaveData(SaveData a_SaveData){
        foreach (BoolValue b in doors){
            SaveData.BoolValueStruct bvs = new SaveData.BoolValueStruct{
                bv_ID = b.myID,
                bv_value = b.initialValue,
                bv_type = b.boolType,
            };
            a_SaveData.bools.Add(bvs);
        }
        foreach (BoolValue b in chests){
            SaveData.BoolValueStruct bvs = new SaveData.BoolValueStruct{
                bv_ID = b.myID,
                bv_value = b.initialValue,
                bv_type = b.boolType,
            };
            a_SaveData.bools.Add(bvs);
        }
        foreach (BoolValue b in other){
            SaveData.BoolValueStruct bvs = new SaveData.BoolValueStruct{
                bv_ID = b.myID,
                bv_value = b.initialValue,
                bv_type = b.boolType,
            };
            a_SaveData.bools.Add(bvs);
        }
    }
    public void LoadFromSaveData(List<SaveData.BoolValueStruct> boolValues){
        foreach(BoolValue b in doors){
            foreach (SaveData.BoolValueStruct bvs in boolValues){
                if (b.myID == bvs.bv_ID && b.boolType == bvs.bv_type){
                    b.initialValue = bvs.bv_value;
                    break;
                }
            }
        }
        foreach(BoolValue b in chests){
            foreach (SaveData.BoolValueStruct bvs in boolValues){
                if (b.myID == bvs.bv_ID && b.boolType == bvs.bv_type){
                    b.initialValue = bvs.bv_value;
                    break;
                }
            }
        }
        foreach(BoolValue b in other){
            foreach (SaveData.BoolValueStruct bvs in boolValues){
                if (b.myID == bvs.bv_ID && b.boolType == bvs.bv_type){
                    b.initialValue = bvs.bv_value;
                    break;
                }
            }
        }
    }






    
    // private void Save(string type, ScriptableObject obj, int i){
    //     FileStream file = File.Create(Application.persistentDataPath + "\\" + zoneName + "\\" + type + string.Format("/{0}.dat", i));
    //     BinaryFormatter binary = new BinaryFormatter();
    //     var json = JsonUtility.ToJson(obj);
    //     binary.Serialize(file, json);
    //     file.Close();
    // }
    // private void Load(string type, int i){
    //     if (File.Exists(Application.persistentDataPath + "\\" + zoneName + "\\" + type + string.Format("/{0}.dat", i))){
    //         FileStream file = File.Open(Application.persistentDataPath + "\\" + zoneName + "\\" + type + string.Format("/{0}.dat", i), FileMode.Open);
    //         BinaryFormatter binary = new BinaryFormatter();
    //         switch (type)
    //         {
    //             case "doors":
    //             JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), doors[i]);
    //                 break;
    //             case "switches":
    //             JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), switches[i]);
    //                 break;
    //             case "chests":
    //             JsonUtility.FromJsonOverwrite((string)binary.Deserialize(file), chests[i]);
    //                 break;
    //             default:
    //                 break;
    //         }
    //         file.Close();
    //     }
    // }
    // private void Delete(string type, int i){
    //     if (File.Exists(Application.persistentDataPath + "\\" + zoneName + "\\" + type + string.Format("/{0}.dat", i))){
    //         File.Delete(Application.persistentDataPath + "\\" + zoneName + "\\" + type + string.Format("/{0}.dat", i));
    //     }
    // }
}
