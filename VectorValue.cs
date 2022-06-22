using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "MySOs/VectorValue")]
public class VectorValue : ScriptableObject, ISerializationCallbackReceiver
{
    public Vector2 initialValue;
    public Vector2 facing;
    public Vector2 defaultValue;

    public void OnAfterDeserialize(){
        initialValue = defaultValue;
    }
    public void OnBeforeSerialize(){

    }
}
