using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP : MonoBehaviour
{
    GameObject MPGAGE;


    // Use this for initialization
    void Start()
    {
        this.MPGAGE = GameObject.Find("MPGAGE");
    }

    // Update is called once per frame
    public void DecreaseMp()
    {
        this.MPGAGE.GetComponent<Image>().fillAmount -= 0.1f;

    }
}