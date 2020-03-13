using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamagePercentRed : MonoBehaviour
{
    public Text redtxt;

    public void SetStartingPercent(int percent)
    {
        redtxt.text = (percent.ToString()) + "%";
    }
    public void SetPercent(int percentage)
    {
        redtxt.text = (percentage.ToString()) + "%";
    }
}
