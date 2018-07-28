using System.Collections;
using System.Collections.Generic;
using DefineBulletModel;
using UnityEngine;
using DefineDefaultAttack;

public class PlayerAttackController : MonoBehaviour {

    Sprite[] alligatorSheetingObject = null;
    Sprite[] alligatorEndObject = null;

    Sprite[] magitionSheetingStart = null;
    Sprite[] magitionSheetingHit = null;
    Sprite[] magitionSheetingEnd = null;


    public GameObject weaponEffect;

    private SpriteRenderer renderEffect;
    private Color effectTransparent = new Color(1, 1, 1, 0);
    private Transform parentsObject = null;

    private GameObject prefbObject;

    void Awake()
    {
        if (weaponEffect)
        {
            renderEffect = weaponEffect.GetComponent<SpriteRenderer>();
            effectTransparent.a = 0.0f;
            renderEffect.material.color = effectTransparent;
        }
        parentsObject = this.transform.parent.transform;

        //악어야 공격 Sheeting & End Sprite Load
        alligatorSheetingObject = Resources.LoadAll<Sprite>("Character/Alligator/AttackSheeting");
        alligatorEndObject = Resources.LoadAll<Sprite>("Character/Alligator/AttackEnd");

        //마법사 공격 sheetingStart Hit End 2가지로 나눔 불러옴.
        magitionSheetingStart = Resources.LoadAll<Sprite>("Character/Magition/AttackSheetingStart");
        magitionSheetingHit = Resources.LoadAll<Sprite>("Character/Magition/AttackSheetingHit");
        magitionSheetingEnd = Resources.LoadAll<Sprite>("Character/Magition/AttackSheetingEnd");

        prefbObject = Resources.Load("Prefabs/BulletDefaultAttack") as GameObject;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetAttackAxisInfo(Vector3 argLocalEulerAngles, Vector3 argLocalPosition)
    {
        parentsObject.transform.localEulerAngles = argLocalEulerAngles;
        parentsObject.transform.localPosition = argLocalPosition;
    }

    public void SetEffectInfo(Vector3 argLocalPosition, Vector3 argLocalScale)
    {
        weaponEffect.transform.localPosition = argLocalPosition;
        weaponEffect.transform.localScale = argLocalScale;
    }

    public void SetEffectImage(string path)
    {
        renderEffect.sprite = Resources.Load<Sprite>(path);
    }

    //Color.a = 1 불투명
    //Color.a = 0 투명
    private IEnumerator SwingWeapon(float attackDuration, float startRot, float endRot, bool isRight, ArrayList hitList)
    {
        hitList.Clear();
        float sRotValue = startRot;
        float eRotValue = endRot;

        float halfValue = (eRotValue + sRotValue) / 2;

        float sTransValue = 0.0f;
        float eTransValue = 1.0f;
        float time = 0f;

        float currentStep = Mathf.Lerp(startRot, endRot, time);

        Vector3 tempVector = new Vector3(0, 0, 0);

        while (currentStep < endRot)
        {
            time += Time.deltaTime / attackDuration;

            currentStep = Mathf.Lerp(sRotValue, eRotValue, time);
            tempVector.z = -currentStep;
            // half step 까지는 0->1로
            // 나머지 half step 은 1->0으로

            if (halfValue > currentStep)
            {
                effectTransparent.a = Mathf.Lerp(sTransValue, eTransValue, time * 2);
                //절반정도까지 왔을때 1까지 상승값
            }
            else
            {
                effectTransparent.a = 1.0f - Mathf.Lerp(-eTransValue, eTransValue, time);
            }

            parentsObject.localEulerAngles = tempVector;
            renderEffect.material.color = effectTransparent;
            yield return null;
        }
        //초기화
        tempVector.z = -sRotValue;
        parentsObject.localEulerAngles = tempVector;
        effectTransparent.a = 0.0f;
        renderEffect.material.color = effectTransparent;
    }

    public IEnumerator AlligatoerAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.isRight, _defaultData.hitList));
        
        if (_defaultData.hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
        else
        {
            //Only For Test
            //무기에 맞는놈이 없으므로 파이어볼
            // Resources/Prefabs/Bullet.prefab 로드
            GameObject bullet = MonoBehaviour.Instantiate(prefbObject) as GameObject;
            bullet.GetComponent<CapsuleCollider2D>().size = _extraData.m_v2BulletSize;
            // 실제 인스턴스 생성. GameObject name의 기본값은 Bullet (clone)
            bullet.transform.position = new Vector3(parentsObject.transform.position.x +
                (_defaultData.isRight == true? +0.2f : -0.2f),
                parentsObject.transform.position.y,
                parentsObject.transform.position.z);
            bullet.transform.localScale = new Vector3(_defaultData.isRight ? bullet.transform.localScale.x : -bullet.transform.localScale.x,
                bullet.transform.localScale.y,
                bullet.transform.localScale.z);
            BulletController bController = bullet.GetComponent<BulletController>();
            BulletDataStrightType argData = new BulletDataStrightType();
            argData.dir = (_defaultData.isRight ? BULLET_DIRECTION.RIGHT : BULLET_DIRECTION.LEFT);
            argData.motion = MOTION_TYPE.STRAIGHT;
            argData.sheetingsprite = alligatorSheetingObject;
            argData.endSprite = alligatorEndObject;
            argData.shootingForce = new Vector2(0.04f,0.04f);

            argData.startPosition = new Vector3(
                bullet.transform.position.x,
                weaponEffect.transform.position.y,
                bullet.transform.position.z);

            argData.penetrateCount = 1;
            argData.disapearTiming = 0.7f;
            argData.sheetingLength = 5.0f;
            argData.tLayer = new ArrayList();
            argData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);
            bController.setInitialize(argData);
        }
    }

    public IEnumerator MagitionAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        
        GameObject bullet = MonoBehaviour.Instantiate(prefbObject) as GameObject;
        bullet.GetComponent<CapsuleCollider2D>().size = _extraData.m_v2BulletSize;
        // 실제 인스턴스 생성. GameObject name의 기본값은 Bullet (clone)
        bullet.transform.position = new Vector3(parentsObject.transform.position.x +
            (_defaultData.isRight == true ? +0.2f : -0.2f),
            parentsObject.transform.position.y,
            parentsObject.transform.position.z);
        bullet.transform.localScale = new Vector3(_defaultData.isRight ? bullet.transform.localScale.x : -bullet.transform.localScale.x,
            bullet.transform.localScale.y,
            bullet.transform.localScale.z);
        BulletController bController = bullet.GetComponent<BulletController>();
        BulletDataFloatType argData = new BulletDataFloatType();
        argData.floatTiming = _defaultData.m_fAttackSpeed * 0.8f;
        argData.motion = MOTION_TYPE.FLOAT;
        argData.startprite = magitionSheetingStart;
        argData.sheetingsprite = magitionSheetingHit;
        argData.endSprite = magitionSheetingEnd;
        argData.disapearTiming = 0.5f;

        argData.startPosition = new Vector3(
            bullet.transform.position.x + (_defaultData.isRight ? +0.4f : -0.4f),
            weaponEffect.transform.position.y,
            bullet.transform.position.z);

        argData.tLayer = new ArrayList();
        argData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);
        bController.setInitialize(argData);

        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.isRight, _defaultData.hitList));
    }

    public IEnumerator DragonAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.isRight, _defaultData.hitList));

        if (_defaultData.hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }

    public IEnumerator HeroAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.isRight, _defaultData.hitList));

        if (_defaultData.hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }
}
