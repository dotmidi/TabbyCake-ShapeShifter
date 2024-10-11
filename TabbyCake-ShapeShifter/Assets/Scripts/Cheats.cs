using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cheats : MonoBehaviour
{
    private int clickedCount = 0;

    public void Clicked()
    {
        if (++clickedCount >= 5)
        {
            clickedCount = 0;
            if (PlayerPrefs.GetInt("Cheats", 0) == 0)
            {
                PlayerPrefs.SetInt("Cheats", 1);
                PlayerPrefs.Save();
            }
        }
    }

    public void CheckCheats()
    {
        int cheatsEnabled = PlayerPrefs.GetInt("Cheats", 0);
        // Debug.Log("Cheats value: " + cheatsEnabled);
    }
}
