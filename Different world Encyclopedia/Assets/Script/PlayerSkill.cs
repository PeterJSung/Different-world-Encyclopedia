using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinitionChar;

public class PlayerSkill : MonoBehaviour
{
    Sprite[] magitionSheetingSprite = null;
    Sprite[] magitionEndSprite = null;
    
    void Awake()
    {

    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ActionSkill(CustomCharacterInfo.CHAR_TYPE argType)
    {
        switch (argType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                AlligatorSkill();
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                MagitionSkill();
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                DragonSkill();
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                HeroSkill();
                break;
        }
    }

    private void AlligatorSkill()
    {

    }

    private void MagitionSkill()
    {

    }

    private void DragonSkill()
    {

    }

    private void HeroSkill()
    {

    }
}
