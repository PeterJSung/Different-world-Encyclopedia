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

    void Awake()
    {
        magitionMagicCircleSrite = Resources.LoadAll<Sprite>("Character/Magition/SkillDevillHand/Circle");
        magitionHandsSprite = Resources.LoadAll<Sprite>("Character/Magition/SkillDevillHand/Hands");

        dragonSparkSprite = Resources.LoadAll<Sprite>("Character/Dragon/SkillLaserShoot/Spark");
        dragonLaserSprite = Resources.LoadAll<Sprite>("Character/Dragon/SkillLaserShoot/Laser");

        moveScript = gameObject.GetComponent<Playermove>();

        dragonBreathPrefab = Resources.Load("Prefabs/Dragon/Breath") as GameObject;
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


        BreathScript firballScript = breathObject.GetComponent<BreathScript>();
        firballScript.SetParameter(skillModel);
        yield return new WaitForSeconds(1.0f);
        /*
        // 이번 공격 형식은 모두 Float 형식임.

        BulletDataFloatType floatData = new BulletDataFloatType();
        floatData.tLayer = new ArrayList();
        floatData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);

        floatData.motion = MOTION_TYPE.FLOAT;
        floatData.rightCheckFunction = moveScript.IsRight;
        


        // Spark 공격
        GameObject bulletObject = MonoBehaviour.Instantiate(prefab) as GameObject;
        bulletObject.GetComponent<CapsuleCollider2D>().size = defaultSkillInfo.m_sSkillBullet.dragonSkill.sparkArea;
        bulletObject.GetComponent<CapsuleCollider2D>().offset = defaultSkillInfo.m_sSkillBullet.dragonSkill.offset;

        bulletObject.transform.position = new Vector3(gameObject.transform.position.x + (moveScript.IsRight() == true ? +0.2f : -0.2f),
            gameObject.transform.position.y, 
            gameObject.transform.position.z);
        bulletObject.transform.localScale = new Vector3(moveScript.IsRight() ? bulletObject.transform.localScale.x : -bulletObject.transform.localScale.x,
            bulletObject.transform.localScale.y,
            bulletObject.transform.localScale.z);



        SpriteRenderer renderObj = bulletObject.GetComponent<SpriteRenderer>();
        for (int i = 0; i < dragonSparkSprite.Length; i++)
        {
            renderObj.sprite = dragonSparkSprite[i];
            yield return new WaitForSeconds(defaultSkillInfo.m_sSkillBullet.dragonSkill.eachDuration);
        }
        Destroy(bulletObject);
        yield return new WaitForSeconds(defaultSkillInfo.m_sSkillBullet.dragonSkill.gapDuration);

        bulletObject = MonoBehaviour.Instantiate(prefab) as GameObject;
        bulletObject.GetComponent<CapsuleCollider2D>().size = defaultSkillInfo.m_sSkillBullet.dragonSkill.laserArea;
        bulletObject.GetComponent<CapsuleCollider2D>().offset = defaultSkillInfo.m_sSkillBullet.dragonSkill.offset;

        bulletObject.transform.position = new Vector3(gameObject.transform.position.x + (moveScript.IsRight() == true ? +0.2f : -0.2f),
            gameObject.transform.position.y,
            gameObject.transform.position.z);
        bulletObject.transform.localScale = new Vector3(moveScript.IsRight() ? bulletObject.transform.localScale.x : -bulletObject.transform.localScale.x,
            bulletObject.transform.localScale.y,
            bulletObject.transform.localScale.z);

        for (int i = 0; i < dragonLaserSprite.Length; i++)
        {
            renderObj.sprite = dragonLaserSprite[i];
            yield return new WaitForSeconds(defaultSkillInfo.m_sSkillBullet.dragonSkill.eachDuration);
        }
        Destroy(bulletObject);

        yield return new WaitForSeconds(defaultSkillInfo.m_sSkillBullet.dragonSkill.gapDuration);
        */

        yield return null;
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
