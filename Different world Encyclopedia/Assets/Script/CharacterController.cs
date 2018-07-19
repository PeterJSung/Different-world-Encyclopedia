using UnityEngine;
using DefinitionChar;
using CustomLib;
using System;

public class CharacterController : MonoBehaviour {

    public GameObject charObj;
    private CaracterInfo.CHAR_TYPE currentType;
    private Playermove moveScript;
    private CustomCircularQueue<CaracterInfo.CHAR_TYPE> characterQueue;

    private bool isCharChangeCoolTime = true;
    // Use this for initialization
    void Awake()
    {
        characterQueue = new CustomCircularQueue<CaracterInfo.CHAR_TYPE>();
        currentType = CaracterInfo.CHAR_TYPE.ALLIGATOR;
        moveScript = charObj.GetComponent<Playermove>();
        moveScript.setCharacterType(currentType);

        foreach (CaracterInfo.CHAR_TYPE charType in (CaracterInfo.CHAR_TYPE[])Enum.GetValues(typeof(CaracterInfo.CHAR_TYPE)))
        {
            characterQueue.push(charType);
        }
    }

    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (isCharChangeCoolTime)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //A를 누를 시
                characterQueue.goLeft();
                moveScript.setCharacterType(characterQueue.getVaule());
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                //S를 누를 시
                characterQueue.goRight();
                moveScript.setCharacterType(characterQueue.getVaule());
            }
        }
    }
}
