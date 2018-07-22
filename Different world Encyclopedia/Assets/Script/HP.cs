using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public GameObject HPGAGE;


    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        DecreaseHp();
    }

    // Update is called once per frame
    public void DecreaseHp()
    {
        if (HPGAGE)
        {
            this.HPGAGE.GetComponent<Image>().fillAmount -= 0.003f;
        }
    }
}
