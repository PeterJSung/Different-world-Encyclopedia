using System.Collections;
using System.Collections.Generic;
using DefineBulletModel;
using UnityEngine;
using DefineDefaultAttack;

public class PlayerAttackController : MonoBehaviour {

    Sprite[] alligatorSheetingObject = null;
    Sprite[] alligatorEndObject = null;

    Sprite[] magitionSheetingObject = null;


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
        alligatorSheetingObject = Resources.LoadAll<Sprite>("Weapon/Alligator/AttackSheeting");
        alligatorEndObject = Resources.LoadAll<Sprite>("Weapon/Alligator/AttackEnd");

        //마법사 공격 sheeting 불러옴.
        magitionSheetingObject = Resources.LoadAll<Sprite>("Weapon/Magition/AttackSheeting");

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
            BulletModel.BulletData argData = new BulletModel.BulletData();
            argData.dir = (_defaultData.isRight ? BulletModel.BULLET_DIRECTION.RIGHT : BulletModel.BULLET_DIRECTION.LEFT);
            argData.motion = BulletModel.MOTION_TYPE.STRAIGHT;
            argData.sheetingsprite = alligatorSheetingObject;
            argData.endSprite = alligatorEndObject;
            argData.weight = new Vector3(0.04f, 0.04f, 0.04f);
            argData.penetrate = new BulletModel.PenetrateData(0,100.0f);
            argData.disapearTiming = 0.7f;
            argData.sheetingLength = 5.0f;
            argData.tLayer = new ArrayList();
            argData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);
            bController.setInitialize(argData);
        }
    }

    public IEnumerator MagitionAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.isRight, _defaultData.hitList));

        yield return new WaitForSeconds(_defaultData.m_fAttackSpeed / 2);

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
        BulletModel.BulletData argData = new BulletModel.BulletData();
        argData.dir = (_defaultData.isRight ? BulletModel.BULLET_DIRECTION.RIGHT : BulletModel.BULLET_DIRECTION.LEFT);
        argData.motion = BulletModel.MOTION_TYPE.STRAIGHT;
        argData.sheetingsprite = alligatorSheetingObject;
        argData.endSprite = alligatorEndObject;
        argData.weight = new Vector3(0.04f, 0.04f, 0.04f);
        argData.penetrate = new BulletModel.PenetrateData(1, 100.0f);
        argData.disapearTiming = 0.7f;
        argData.sheetingLength = 5.0f;
        argData.tLayer = new ArrayList();
        argData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);
        bController.setInitialize(argData);
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
