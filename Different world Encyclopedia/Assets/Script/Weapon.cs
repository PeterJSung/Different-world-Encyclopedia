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

    private const float START_ROTATE = 30.0f;
    private const float END_ROTATE = 130.0f;

    private bool isRight = true;
    private float attackSpeed = 1.0f;

    private CaracterInfo.CHAR_TYPE currentPlayerType;

    Sprite[] sheetingObject = null;
    Sprite[] endObject = null;

    ArrayList isHiitedList = new ArrayList();
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
            StartCoroutine(AttackAnimation(attackSpeed));
        }
    }


    //Color.a = 1 불투명
    //Color.a = 0 투명
    IEnumerator AttackAnimation(float totalduration)
    {
        isHiitedList.Clear();
        canAttack = false;
        float sRotValue = START_ROTATE;
        float eRotValue = END_ROTATE;

        float halfValue = (eRotValue + sRotValue) / 2;

        float sTransValue = 0.0f;
        float eTransValue = 1.0f;
        float time = 0f;

        float currentStep = Mathf.Lerp(START_ROTATE, END_ROTATE, time);

        Vector3 tempVector = new Vector3(0, 0, 0);

        while (currentStep < END_ROTATE)
        {
            time += Time.deltaTime / totalduration;

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
        }
        canAttack = true;
    }

    public void setAttackDirection(bool argIsRight)
    {
        isRight = argIsRight;
    }

    public void setParameter(float argSpeed, CaracterInfo.CHAR_TYPE argType)
    {
        string sheetingDiectory = "";
        string bulletEndDirectory = "";

        attackSpeed = argSpeed;
        currentPlayerType = argType;

        string assetDirectory = "Weapon";
        switch (currentPlayerType)
        {
            case CaracterInfo.CHAR_TYPE.ALLIGATOR:
                assetDirectory += "/Alligator";
                break;
            case CaracterInfo.CHAR_TYPE.MAGITION:
                assetDirectory += "/Magition";
                break;
            case CaracterInfo.CHAR_TYPE.DRAGON:
                assetDirectory += "/Dragon";
                break;
            default:
                Debug.Assert(true);
                Debug.Assert(false);
                break;
        }
        sheetingDiectory = assetDirectory + "/AttackSheeting";
        bulletEndDirectory = assetDirectory + "/AttackEnd";

        sheetingObject = Resources.LoadAll<Sprite>(sheetingDiectory);
        endObject = Resources.LoadAll<Sprite>(bulletEndDirectory);
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

    }
}
