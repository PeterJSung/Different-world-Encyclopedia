using System.Collections;
using UnityEngine;
using DefineBulletModel;
using DefinitionChar;

public class Weapon : MonoBehaviour
{

    public GameObject weaponEffect;

    private Renderer renderEffect;
    private Color effectTransparent = new Color(1, 1, 1, 0);

    private bool canAttack = true;
    private Transform parentsObject = null;
    
    private bool isRight = true;
    
    private CustomCharacterInfo.CHAR_TYPE currentPlayerType;

    
    ArrayList isHiitedList = new ArrayList();

    private PlayerWeaponData currentData;
    private PlayerAttackController attackController;

    private BoxCollider2D weaponBoxCollider2D = null;
    private SpriteRenderer renderObject = null;

    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("TRIGGERD");
        if (!canAttack)
        {
            //공격 도중에만 충돌 체크
            if(other.gameObject.layer == GlobalLayerMask.ENEMY_MASK)
            {
                isHiitedList.Add(other.gameObject);
            }
        }
    }
    // Use this for initialization
    void Awake()
    {
        this.InitializeWeapon();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(parentsObject.localEulerAngles);
    }

    //for Attach Event
    public void AttackMotion()
    {
        //밖에서 걸러지긴하지만 그냥 방어코드겸 넣어봄.
        if (canAttack)
        {
            StartCoroutine(AttackAnimation());
        }
    }


    //Color.a = 1 불투명
    //Color.a = 0 투명
    IEnumerator AttackAnimation()
    {
        isHiitedList.Clear();
        canAttack = false;
        float sRotValue = currentData.m_fWeaponAxisStart;
        float eRotValue = currentData.m_fWeaponAxisEnd;

        float halfValue = (eRotValue + sRotValue) / 2;

        float sTransValue = 0.0f;
        float eTransValue = 1.0f;
        float time = 0f;

        float currentStep = Mathf.Lerp(currentData.m_fWeaponAxisStart, currentData.m_fWeaponAxisEnd, time);

        Vector3 tempVector = new Vector3(0, 0, 0);

        while (currentStep < currentData.m_fWeaponAxisEnd)
        {
            time += Time.deltaTime / currentData.m_fAttackSpeed;

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

        if(isHiitedList.Count > 0)
        {
            //0 이상이면 무기에 맞는놈이있다.
            //Hit and 넉벡
            Debug.Log("HIT");
        } else
        {
            /*
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
            argData.sheetingsprite = sheetingObject;
            argData.endSprite = endObject;
            argData.weight = new Vector3(0.04f, 0.04f, 0.04f);
            argData.penetrate = new BulletModel.PenetrateData(0,100.0f);
            argData.disapearTiming = 0.7f;
            argData.sheetingLength = 5.0f;
            argData.tLayer = new ArrayList();
            argData.tLayer.Add(GlobalLayerMask.ENEMY_MASK);
            bController.setInitialize(argData);
            */
        }
        canAttack = true;
    }

    public void setAttackDirection(bool argIsRight)
    {
        isRight = argIsRight;
    }

    public void setParameter(PlayerWeaponData argData, CustomCharacterInfo.CHAR_TYPE argType)
    {
        currentData = argData;
        currentPlayerType = argType;
        parentsObject.transform.localEulerAngles = new Vector3(0, 0, -currentData.m_fWeaponAxisStart);

        weaponBoxCollider2D.enabled = currentData.isEnableWeaponHit;
        weaponBoxCollider2D.size = currentData.m_v2WeaponColliderArea;
        weaponBoxCollider2D.offset = currentData.m_v2Weaponoffset;
        gameObject.transform.localPosition = currentData.m_v2WeaponPosition;

        string path = "Weapon";
        switch (currentPlayerType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                path += "/Alligator/Merona";
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                path += "/Magition/Stick";
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                path += "/Dragon/Axe";
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                path += "/Hero/sword";
                break;
        }
        renderObject.sprite = Resources.Load<Sprite>(path);

    }

    public bool CanAttackMotion()
    {
        return canAttack;
    }

    void InitializeWeapon()
    {
        if (weaponEffect)
        {
            renderEffect = weaponEffect.GetComponent<Renderer>();
            effectTransparent.a = 0.0f;
            renderEffect.material.color = effectTransparent;
        }
        parentsObject = this.transform.parent.transform;
        attackController = gameObject.GetComponent<PlayerAttackController>();
        weaponBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        renderObject = gameObject.GetComponent<SpriteRenderer>();
    }
}
