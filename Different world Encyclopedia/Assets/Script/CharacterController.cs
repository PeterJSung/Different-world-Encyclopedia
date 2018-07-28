using UnityEngine;
using DefinitionChar;
using CustomLib;
using System;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public GameObject characterObject;
    public GameObject characterSwitchEffectObject;
    private CustomCharacterInfo.CHAR_TYPE currentType;
    private Playermove moveScript;
    private CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE> characterQueue;

    private CharacterSwitchEffect effectScript;

    private bool isCharChangeCoolTime = true;
    // Use this for initialization

    void Awake()
    {
        characterQueue = new CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE>();
        currentType = CustomCharacterInfo.CHAR_TYPE.ALLIGATOR;
        moveScript = characterObject.GetComponent<Playermove>();
        effectScript = characterSwitchEffectObject.GetComponent<CharacterSwitchEffect>();

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

        if (isCharChangeCoolTime && moveScript.IsPossibleCharaterChange())
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                //A를 누를 시
                CustomCharacterInfo.CHAR_TYPE origin = characterQueue.getVaule();
                characterQueue.goLeft();
                moveScript.setCharacterType(characterQueue.getVaule());

                CustomCharacterInfo.CHAR_TYPE next = characterQueue.getVaule();
                //StartCoroutine(SwitchAnimation(origin, next));
            } else if (Input.GetKeyDown(KeyCode.S))
            {
                //S를 누를 시
                CustomCharacterInfo.CHAR_TYPE origin = characterQueue.getVaule();
                characterQueue.goRight();
                CustomCharacterInfo.CHAR_TYPE next = characterQueue.getVaule();
                StartCoroutine(SwitchAnimation(origin, next));
            }
        }
    }

    private IEnumerator SwitchAnimation(CustomCharacterInfo.CHAR_TYPE originType, CustomCharacterInfo.CHAR_TYPE switchType)
    {
        //모든 설정 초기화.
        moveScript.ResetCharacterInfo();
        //Hold & Imune 설정
        moveScript.SetHold();
        moveScript.SetInvInvincibility();

        // 우선 스프라이트가 위에서 설정되서 내려온다
        // Effect Script On 변경할 캐릭터가 내려와야함.

        effectScript.SetModeFalldown(switchType);
        
        //캐릭터 공중에 올려놓는다.
        
        //캐릭터가 지면에 닿을동안 대기.
        yield return new WaitUntil(() => effectScript.IsLanding() == true);
        Debug.Log("IsLanding Here");
        //닿으면 연기 Effect 설정.

        moveScript.IsRight();
        yield return null;

        //캐릭터 변경.
        moveScript.setCharacterType(switchType);

        //연기 effect 와 동시에 캐릭터 왼쪽 오른쪽 여부 판단하여 애니메이션 작동



        //모든 애니메이션 동작 완료 시 해당 다시 Release
        moveScript.ReleaseHold();
        moveScript.ReleaseInvincibility();
    }
}
