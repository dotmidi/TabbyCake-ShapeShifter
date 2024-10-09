using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private int clickedCount = 0;

    // this is attached to a button, so it will be called when the button is clicked
    public void Clicked()
    {
        clickedCount++;
        Debug.Log("Clicked " + clickedCount + " times");
        if (clickedCount >= 5)
        {
            clickedCount = 0;
            // set playerprefs "cheat" to 1, make the playerprefs if it doesn't exist
            PlayerPrefs.SetInt("Cheats", 1);
            PlayerPrefs.Save(); // Ensure PlayerPrefs changes are saved
            Debug.Log("Cheats enabled");
        }
    }

    public void CheckCheats()
    {
        int cheatsEnabled = PlayerPrefs.GetInt("Cheats", 0);
        Debug.Log("Cheats value: " + cheatsEnabled);
    }
}
