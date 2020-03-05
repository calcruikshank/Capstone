using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DamagePercentBlue : MonoBehaviour
{
    public Text txt;

    public void SetStartingPercent(int percent)
    {

    }
    public void SetPercent(int percentage)
    {
        txt.text = (percentage.ToString()) + "%";
    }
}
