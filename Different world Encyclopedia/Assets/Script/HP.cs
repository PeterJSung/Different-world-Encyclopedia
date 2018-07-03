using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour {
    GameObject HPGAGE;
    

	// Use this for initialization
	void Start () {
        this.HPGAGE = GameObject.Find("HPGAGE");
	}
	
	// Update is called once per frame
	public void DecreaseHp() {
        this.HPGAGE.GetComponent<Image>().fillAmount -= 0.1f;
		
	}
}
