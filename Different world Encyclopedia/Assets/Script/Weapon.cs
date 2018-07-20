using System.Collections;
using UnityEngine;
using DefinitionChar;

public class Weapon : MonoBehaviour
{
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
        if (!canAttack)
        {
            //공격 도중에만 충돌 체크
            if (other.gameObject.layer == GlobalLayerMask.ENEMY_MASK)
            {
                isHiitedList.Add(other.gameObject);
            }
        }
    }
    // Use this for initialization
    void Awake()
    {
        InitializeWeapon();
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



    IEnumerator AttackAnimation()
    {
        canAttack = false;

        switch (currentPlayerType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                yield return StartCoroutine(attackController.AlligatoerAttack(
            currentData.m_fAttackSpeed,
            currentData.m_fWeaponAxisStart,
            currentData.m_fWeaponAxisEnd,
            isRight,
            isHiitedList));
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                break;
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

        attackController.SetAttackInfo(new Vector3(0, 0, -currentData.m_fWeaponAxisStart), new Vector3(currentData.m_v2WeaponAxisPosition.x, currentData.m_v2WeaponAxisPosition.y, 0));
        weaponBoxCollider2D.enabled = currentData.m_bIsEnableWeaponHit;
        weaponBoxCollider2D.size = currentData.m_v2WeaponColliderArea;
        weaponBoxCollider2D.offset = currentData.m_v2Weaponoffset;
        gameObject.transform.localPosition = currentData.m_v2WeaponPosition;
        
        renderObject.sprite = Resources.Load<Sprite>(currentData.m_sWaeponSpritePath);
    }

    public bool CanAttackMotion()
    {
        return canAttack;
    }

    void InitializeWeapon()
    {
        parentsObject = this.transform.parent.transform;
        attackController = gameObject.GetComponent<PlayerAttackController>();
        weaponBoxCollider2D = gameObject.GetComponent<BoxCollider2D>();
        renderObject = gameObject.GetComponent<SpriteRenderer>();
    }
}
