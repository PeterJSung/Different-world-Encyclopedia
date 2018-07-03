using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MP : MonoBehaviour
{
    public GameObject MPGAGE;


    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        DecreaseMp();
    }

    // Update is called once per frame
    public void DecreaseMp()
    {
        if (MPGAGE)
        {
            this.MPGAGE.GetComponent<Image>().fillAmount -= 0.001f;
        }
    }
}