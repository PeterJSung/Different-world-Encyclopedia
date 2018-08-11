using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefinitionChar;
using DefineSkill;

using DragonActionModel;

public class PlayerSkill : MonoBehaviour
{
    private Sprite[] magitionMagicCircleSrite = null;
    private Sprite[] magitionHandsSprite = null;

    private Sprite[] dragonSparkSprite = null;
    private Sprite[] dragonLaserSprite = null;

    private bool duringSkill = false;

    private Playermove moveScript = null;
    private SkillInfo defaultSkillInfo = null;

    private GameObject dragonBreathPrefab;
    private GameObject magitionDevilHandsPrefab;

    void Awake()
    {
        magitionMagicCircleSrite = Resources.LoadAll<Sprite>("Character/Magition/SkillDevillHand/Circle");
        magitionHandsSprite = Resources.LoadAll<Sprite>("Character/Magition/SkillDevillHand/Hands");

        dragonSparkSprite = Resources.LoadAll<Sprite>("Character/Dragon/SkillLaserShoot/Spark");
        dragonLaserSprite = Resources.LoadAll<Sprite>("Character/Dragon/SkillLaserShoot/Laser");

        moveScript = gameObject.GetComponent<Playermove>();

        dragonBreathPrefab = Resources.Load("Prefabs/Dragon/Breath") as GameObject;
        magitionDevilHandsPrefab = Resources.Load("Prefabs/Magition/DevilHands") as GameObject;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool GetNoSkillAction()
    {
        return duringSkill == false;
    }

    public void ActionSkill(GlobalCharacterInfo.CHAR_TYPE currentType)
    {
        GlobalSkillInfo.SKILL_GLOBAL_DEFAULT_DATA.TryGetValue(currentType, out defaultSkillInfo);

        duringSkill = true;
        moveScript.SetInvInvincibility();
        moveScript.SetHold();
        switch (currentType)
        {
            case GlobalCharacterInfo.CHAR_TYPE.ALLIGATOR:
                StartCoroutine(AlligatorSkill());
                break;
            case GlobalCharacterInfo.CHAR_TYPE.MAGITION:
                StartCoroutine(MagitionSkill());
                break;
            case GlobalCharacterInfo.CHAR_TYPE.DRAGON:
                StartCoroutine(DragonSkill());
                break;
            case GlobalCharacterInfo.CHAR_TYPE.HERO:
                StartCoroutine(HeroSkill());
                break;
        }
    }

    private IEnumerator AlligatorSkill()
    {
        yield return null;
        moveScript.ReleaseInvincibility();
        moveScript.ReleaseHold();
        duringSkill = false;
    }

    private IEnumerator MagitionSkill()
    {
        GameObject magicHandsObject = MonoBehaviour.Instantiate(magitionDevilHandsPrefab) as GameObject;

        yield return null;
        moveScript.ReleaseInvincibility();
        moveScript.ReleaseHold();
        duringSkill = false;
    }

    private IEnumerator DragonSkill()
    {
        GameObject breathObject = MonoBehaviour.Instantiate(dragonBreathPrefab) as GameObject;
        DragonSkillModel skillModel = new DragonSkillModel();
        skillModel.appreanceFrame = defaultSkillInfo.m_sSkillBullet.dragonSkill.eachDuration;
        skillModel.gapFrame = defaultSkillInfo.m_sSkillBullet.dragonSkill.gapDuration;
        skillModel.isRight = moveScript.IsRight();
        skillModel.sheetingFrame = defaultSkillInfo.m_sSkillBullet.dragonSkill.eachDuration;
        skillModel.sheetingSprite = dragonLaserSprite;
        skillModel.sheetingSpriteStart = dragonSparkSprite;
        skillModel.stretchYMax = defaultSkillInfo.m_sSkillBullet.dragonSkill.laserYArea;
        skillModel.stretchYMin = defaultSkillInfo.m_sSkillBullet.dragonSkill.sparkYArea;
        skillModel.targetArray = new ArrayList();
        skillModel.targetArray.Add(GlobalLayerMask.ENEMY_MASK); ;

        
        float offsetValue = breathObject.GetComponent<CapsuleCollider2D>().size.x / 2 * breathObject.transform.localScale.x;
        float charSizeX = moveScript.GetCurrentSize().x;

        breathObject.transform.position = new Vector3(
            moveScript.GetCurrentPostion().x + (moveScript.IsRight()? offsetValue : -offsetValue) + charSizeX,
            moveScript.GetCurrentPostion().y,
            moveScript.GetCurrentPostion().z);

        breathObject.transform.localScale = new Vector3(moveScript.IsRight() ? 1 : -1, 1, 1);

        BreathScript breathScript = breathObject.GetComponent<BreathScript>();
        breathScript.SetParameter(skillModel);
        yield return new WaitUntil(() => breathScript.EndSkill() == true);
        Destroy(breathObject);
       
        moveScript.ReleaseInvincibility();
        moveScript.ReleaseHold();
        duringSkill = false;
    }

    private IEnumerator HeroSkill()
    {
        yield return null;
        moveScript.ReleaseInvincibility();
        moveScript.ReleaseHold();
        duringSkill = false;
    }
}
