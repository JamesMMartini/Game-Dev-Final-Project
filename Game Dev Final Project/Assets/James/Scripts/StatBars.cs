using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatBars : MonoBehaviour
{
    public void UpdateBars(int hp, int maxHP)
    {
        Debug.Log(hp + " " + maxHP);

        // Set the health fill
        transform.Find("Health Bar").transform.Find("Health").GetComponent<Image>().fillAmount = ((float)hp) / ((float)maxHP);

        // Set the PP fill
    }
}
