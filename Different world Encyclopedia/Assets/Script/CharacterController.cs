using UnityEngine;
using DefinitionChar;
using CustomLib;
using System;

public class CharacterController : MonoBehaviour {

    public GameObject charObj;
    private CustomCharacterInfo.CHAR_TYPE currentType;
    private Playermove moveScript;
    private CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE> characterQueue;

    private bool isCharChangeCoolTime = true;
    // Use this for initialization
    void Awake()
    {
        characterQueue = new CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE>();
        currentType = CustomCharacterInfo.CHAR_TYPE.ALLIGATOR;
        moveScript = charObj.GetComponent<Playermove>();
        

        foreach (CustomCharacterInfo.CHAR_TYPE charType in (CustomCharacterInfo.CHAR_TYPE[])Enum.GetValues(typeof(CustomCharacterInfo.CHAR_TYPE)))
        {
            characterQueue.push(charType);
        }
    }

    void Start () {
        moveScript.setCharacterType(currentType);
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
