using UnityEngine;
using DefinitionChar;
using CustomLib;
using System;
using System.Collections;
using System.Linq;

public class CharacterController : MonoBehaviour {

    public GameObject characterObject;
    public GameObject characterSwitchEffectObject;
    private SpriteRenderer switchEffectRenderer;
    private CustomCharacterInfo.CHAR_TYPE currentType;
    private Playermove moveScript;
    private CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE> characterQueue;

    private bool isCharChangeCoolTime = true;
    // Use this for initialization

    private Sprite spriteAlligatorHit = null;
    private Sprite spriteMagitionHit = null;
    private Sprite spriteDragonHit = null;
    private Sprite spriteHeroHit = null;
    void Awake()
    {
        characterQueue = new CustomCircularQueue<CustomCharacterInfo.CHAR_TYPE>();
        currentType = CustomCharacterInfo.CHAR_TYPE.ALLIGATOR;
        moveScript = characterObject.GetComponent<Playermove>();
        switchEffectRenderer = characterSwitchEffectObject.GetComponent<SpriteRenderer>();

        characterSwitchEffectObject.SetActive(false);

        foreach (CustomCharacterInfo.CHAR_TYPE charType in (CustomCharacterInfo.CHAR_TYPE[])Enum.GetValues(typeof(CustomCharacterInfo.CHAR_TYPE)))
        {
            characterQueue.push(charType);
        }
        
        spriteAlligatorHit = Resources.LoadAll<Sprite>("Player/dkrdjdiH").Single(s => s.name == "dkrdjdiH_4");
        spriteMagitionHit = Resources.LoadAll<Sprite>("Player/wichH").Single(s => s.name == "wichH_4");
        spriteDragonHit = Resources.LoadAll<Sprite>("Player/DragonH").Single(s => s.name == "DragonH_4");
        spriteHeroHit = Resources.LoadAll<Sprite>("Player/HeroH").Single(s => s.name == "HeroH_4");
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
                CustomCharacterInfo.CHAR_TYPE next = characterQueue.getVaule();
                StartCoroutine(SwitchAnimation(origin, next));
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


        //이전 캐릭터 스프라이트 정위치.
        characterSwitchEffectObject.SetActive(true);
        characterSwitchEffectObject.transform.position = characterObject.transform.position;

        switch (originType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:   switchEffectRenderer.sprite = spriteAlligatorHit;   break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:    switchEffectRenderer.sprite = spriteMagitionHit;    break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:      switchEffectRenderer.sprite = spriteDragonHit;      break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:        switchEffectRenderer.sprite = spriteHeroHit;        break;
        }

        //캐릭터 변경.
        moveScript.setCharacterType(switchType);

        //캐릭터 공중에 올려놓는다.
        characterObject.transform.Translate(0.0f, 5.0f,0.0f);

        //캐릭터가 지면에 닿을동안 대기.
        while (false)
        {
            yield return null;
        }

        //닿으면 연기 Effect 설정.

        //연기 effect 와 동시에 캐릭터 왼쪽 오른쪽 여부 판단하여 애니메이션 작동
        moveScript.IsRight();
        yield return null;

        //모든 애니메이션 동작 완료 시 해당 다시 Release
        moveScript.ReleaseHold();
        moveScript.ReleaseInvincibility();
        characterSwitchEffectObject.SetActive(false);
    }
}
