using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;
    [Header("Location of player and where he looks")]
    public Vector2 positionToLoad;
    public Vector2 facing;
    [Header("ScriptObj player uses on 'Awake()'")]
    public VectorValue playerPosition;
    [Header("HUD obj")]
    public GameObject fadePanel;
    
    public virtual void OnTriggerEnter2D(Collider2D other){
        if (other.CompareTag("Player") && !other.isTrigger){
            playerPosition.initialValue = positionToLoad;
            playerPosition.facing = facing;
            fadePanel.GetComponent<Animator>().SetTrigger("Fade");
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
