using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinPanel : MonoBehaviour
{
    public Inventory playerInventory;
    [Header("'sprites' are the game objects")]
    public Image[] sprites;
    [Header("'nums' are the digit sprites")]
    public Sprite[] nums;
    private int totalCoins;
    private int[] digits;

    private void Start(){
        updateCoins();
    }
    //Update the banner's total coins by getting it from the playerInventory. Then I will loop through the 1's, 10's, and 100's and assign the correct sprite to that location.
    public void updateCoins(){
        totalCoins = playerInventory.numberOfCoins;
        digits = GetDigits(totalCoins);
        for (var i = 0; i < digits.Length; i++){
            sprites[i].sprite = nums[digits[i]];
        }
    }
    //Break the single value of "totalCoins" into 3 seperate digits for me to loop through
    private int[] GetDigits(int num){
        List<int> listOfInts = new List<int>();
        while(num > 0){
            listOfInts.Add(num % 10);
            num = num/10;
        }
        return listOfInts.ToArray();
    }
}
