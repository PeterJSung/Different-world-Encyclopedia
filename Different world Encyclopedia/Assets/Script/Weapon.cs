using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

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

    private CharatorType currentPlayerType;

    Object[] sheetingObject = null;
    Object[] endObject = null;

    ArrayList isHiitedList = new ArrayList();
    private void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log("TRIGGERD");
        if (!canAttack)
        {
            //공격 도중에만 충돌 체크
            if(other.gameObject.layer == GlobalLayerMask.ENEMY_MASK)
            {
                //
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
        Debug.Log("START MOTION");
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
            //무기에 맞는놈이 없으므로 파이어볼
            Debug.Log("Fire Ball");
        }

        canAttack = true;
        Debug.Log("END MOTION");
    }

    public void setAttackDirection(bool argIsRight)
    {
        isRight = argIsRight;
    }

    public void setParameter(float argSpeed, CharatorType argType)
    {
        string sheetingDiectory = "";
        string bulletEndDirectory = "";

        attackSpeed = argSpeed;
        currentPlayerType = argType;

        string assetDirectory = "Weapon";
        switch (currentPlayerType)
        {
            case CharatorType.ALLIGATOR:
                assetDirectory += "/Alligator";
                break;
            case CharatorType.MAGITION:
                assetDirectory += "/Magition";
                break;
            case CharatorType.DRAGON:
                assetDirectory += "/Dragon";
                break;
            default:
                Debug.Assert(true);
                Debug.Assert(false);
                break;
        }
        sheetingDiectory = assetDirectory + "/AttackSheeting";
        bulletEndDirectory = assetDirectory + "/AttackEnd";

        sheetingObject = Resources.LoadAll(sheetingDiectory, typeof(Texture2D));
        endObject = Resources.LoadAll(bulletEndDirectory, typeof(Texture2D));
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
