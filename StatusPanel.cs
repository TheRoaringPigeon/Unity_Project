using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusPanel : MonoBehaviour
{
    public FloatValue playerHealth;
    public Image healthPool;
    // Start is called before the first frame update
    void Start()
    {
        UpdateHealth();
    }

    public void UpdateHealth(){
        healthPool.fillAmount = playerHealth.currentValue / playerHealth.initialValue;
    }
}
