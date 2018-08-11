using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineDefaultAttack;

using AlligatorActionModel;
using MagitionActionModel;

public class PlayerAttackController : MonoBehaviour
{
    public GameObject weaponEffect;

    private Sprite[] alligatorSheetingObject = null;
    private Sprite[] alligatorEndObject = null;

    private Sprite[] magitionSheetingStart = null;
    private Sprite[] magitionSheetingHit = null;
    private Sprite[] magitionSheetingEnd = null;

    private SpriteRenderer renderEffect;
    private Color effectTransparent = new Color(1, 1, 1, 0);
    private Transform parentsObject = null;

    private GameObject alligatorFireBallPrefab;
    private GameObject magitionHuricaneBurnPrefab;

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

        alligatorFireBallPrefab = Resources.Load("Prefabs/Alligator/FireBall") as GameObject;
        magitionHuricaneBurnPrefab = Resources.Load("Prefabs/Magition/HurricaneBurn") as GameObject;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

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
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.rightFunctionPointer(), _defaultData.hitList));

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
            GameObject firballObject = MonoBehaviour.Instantiate(alligatorFireBallPrefab) as GameObject;
            firballObject.transform.position = new Vector3(parentsObject.transform.position.x +
                (_defaultData.rightFunctionPointer() == true ? +0.2f : -0.2f),
                parentsObject.transform.position.y,
                parentsObject.transform.position.z);
            AlligatorAttackModel argModel = new AlligatorAttackModel();

            argModel.bulletSpeed = 0.06f; // 0.1 x move
            argModel.destroySprite = alligatorEndObject;
            argModel.frame = 10.0f; // ms
            argModel.endFrameMultiple = 3; // ms
            argModel.isRight = _defaultData.rightFunctionPointer();
            argModel.scale = 1.0f; // multiple Scale
            argModel.sheetingArea = 5.0f;
            argModel.sheetingSprite = alligatorSheetingObject;
            argModel.targetArray = new ArrayList();
            argModel.targetArray.Add(GlobalLayerMask.ENEMY_MASK);

            FireBallScript firballScript = firballObject.GetComponent<FireBallScript>();

            firballScript.SetParameter(argModel);
        }
    }

    public IEnumerator MagitionAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        GameObject huricaneBurnObject = MonoBehaviour.Instantiate(magitionHuricaneBurnPrefab) as GameObject;
        huricaneBurnObject.transform.position = new Vector3(parentsObject.transform.position.x +
            (_defaultData.rightFunctionPointer() == true ? +0.2f : -0.2f),
            parentsObject.transform.position.y,
            parentsObject.transform.position.z);
        MagitionAttackModel argModel = new MagitionAttackModel();

        argModel.appreanceFrame = 10.0f;
        argModel.floatingTime = _defaultData.m_fAttackSpeed * 0.6f;
        argModel.referencePosition = weaponEffect;
        argModel.rightFunctionPointer = _defaultData.rightFunctionPointer;
        argModel.scale = 1.0f; // 0.1 x move
        argModel.sheetingFrame = 10.0f;
        argModel.sheetingSprite = magitionSheetingHit;
        argModel.sheetingSpriteEnd = magitionSheetingEnd;
        argModel.sheetingSpriteStart = magitionSheetingStart;
        argModel.stretchYMax = 0.4f;
        argModel.stretchYMin = 0.1f;
        argModel.targetArray = new ArrayList();
        argModel.targetArray.Add(GlobalLayerMask.ENEMY_MASK);

        HurricaneBurn firballScript = huricaneBurnObject.GetComponent<HurricaneBurn>();

        firballScript.SetParameter(argModel);
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.rightFunctionPointer(), _defaultData.hitList));
    }

    public IEnumerator DragonAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.rightFunctionPointer(), _defaultData.hitList));

        if (_defaultData.hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }

    public IEnumerator HeroAttack(DefaultAttackData _defaultData, ExtraAttackData _extraData)
    {
        yield return StartCoroutine(SwingWeapon(_defaultData.m_fAttackSpeed, _defaultData.m_fWeaponAxisStart, _defaultData.m_fWeaponAxisEnd, _defaultData.rightFunctionPointer(), _defaultData.hitList));

        if (_defaultData.hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }
}
