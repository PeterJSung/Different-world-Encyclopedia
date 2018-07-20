using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour {

    Sprite[] alligatorSheetingObject = null;
    Sprite[] alligatorEndObject = null;

    Sprite[] magitionSheetingObject = null;

    void Awake()
    {
        alligatorSheetingObject = Resources.LoadAll<Sprite>("Weapon/Alligator/AttackSheeting");
        alligatorEndObject = Resources.LoadAll<Sprite>("Weapon/Alligator/AttackEnd");

        //마법사 공격 sheeting 불러옴.
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator AlligatoerAttack()
    {
        yield return null;
    }
}
