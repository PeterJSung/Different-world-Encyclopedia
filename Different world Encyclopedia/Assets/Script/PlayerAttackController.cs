using System.Collections;
using System.Collections.Generic;
using DefineBulletModel;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour {

    Sprite[] alligatorSheetingObject = null;
    Sprite[] alligatorEndObject = null;

    Sprite[] magitionSheetingObject = null;


    public GameObject weaponEffect;

    private SpriteRenderer renderEffect;
    private Color effectTransparent = new Color(1, 1, 1, 0);
    private Transform parentsObject = null;

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

    //Color.a = 1 불투명
    //Color.a = 0 투명
    public IEnumerator AlligatoerAttack(float attackDuration, float startRot,float endRot,bool isRight, ArrayList hitList)
    {
        yield return StartCoroutine(SwingWeapon(attackDuration, startRot, endRot, isRight, hitList));
        
        if (hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
        else
        {
            
            //Only For Test
            //무기에 맞는놈이 없으므로 파이어볼
            Debug.Log("Fire Ball");
            GameObject prefab = Resources.Load("Prefabs/BulletFireBall") as GameObject;
            // Resources/Prefabs/Bullet.prefab 로드
            GameObject bullet = MonoBehaviour.Instantiate(prefab) as GameObject;
            // 실제 인스턴스 생성. GameObject name의 기본값은 Bullet (clone)
            bullet.transform.position = new Vector3(parentsObject.transform.position.x +
                (isRight == true? +0.2f : -0.2f),
                parentsObject.transform.position.y,
                parentsObject.transform.position.z);
            bullet.transform.localScale = new Vector3(isRight ? bullet.transform.localScale.x : -bullet.transform.localScale.x,
                bullet.transform.localScale.y,
                bullet.transform.localScale.z);
            BulletController bController = bullet.GetComponent<BulletController>();
            BulletModel.BulletData argData = new BulletModel.BulletData();
            argData.dir = (isRight ? BulletModel.BULLET_DIRECTION.RIGHT : BulletModel.BULLET_DIRECTION.LEFT);
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

    public IEnumerator MagitionAttack(float attackDuration, float startRot, float endRot, bool isRight, ArrayList hitList)
    {
        yield return StartCoroutine(SwingWeapon(attackDuration, startRot, endRot, isRight, hitList));
    }

    public IEnumerator DragonAttack(float attackDuration, float startRot, float endRot, bool isRight, ArrayList hitList)
    {
        yield return StartCoroutine(SwingWeapon(attackDuration, startRot, endRot, isRight, hitList));

        if (hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }

    public IEnumerator HeroAttack(float attackDuration, float startRot, float endRot, bool isRight, ArrayList hitList)
    {
        yield return StartCoroutine(SwingWeapon(attackDuration, startRot, endRot, isRight, hitList));

        if (hitList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        }
    }
}
