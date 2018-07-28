using System.Collections;
using UnityEngine;
using DefinitionChar;
using DefineDefaultAttack;

public class Weapon : MonoBehaviour
{
    private bool canAttack = true;
    private Transform parentsObject = null;

    private bool isRight = true;

    private CustomCharacterInfo.CHAR_TYPE currentPlayerType;


    ArrayList isHiitedList = new ArrayList();

    private PlayerWeaponData currentPlayerData;
    private PlayerAttackController attackController;

    private BoxCollider2D weaponBoxCollider2D = null;
    private SpriteRenderer renderObject = null;

    private PlayerDefaultBulletData currentBulletData;

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
        DefaultAttackData argDefaultData = new DefaultAttackData(
            currentPlayerData.m_fAttackSpeed,
            currentPlayerData.m_fWeaponAxisStart,
            currentPlayerData.m_fWeaponAxisEnd,
            isRight, 
            isHiitedList);
        ExtraAttackData argExtraData = new ExtraAttackData(currentBulletData.m_v2BulletSize);
        switch (currentPlayerType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                yield return StartCoroutine(attackController.AlligatoerAttack(
                    argDefaultData,
                    argExtraData));
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                yield return StartCoroutine(attackController.MagitionAttack(
                    argDefaultData,
                    argExtraData));
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                yield return StartCoroutine(attackController.DragonAttack(
                    argDefaultData,
                    argExtraData));
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                yield return StartCoroutine(attackController.HeroAttack(
                    argDefaultData,
                    argExtraData));
                break;
        }

        canAttack = true;
    }

    public void setAttackDirection(bool argIsRight)
    {
        isRight = argIsRight;
    }

    public void setParameter(PlayerWeaponData argPlayerData,PlayerDefaultBulletData argBulletData ,CustomCharacterInfo.CHAR_TYPE argType)
    {
        currentPlayerData = argPlayerData;
        currentBulletData = argBulletData;

        currentPlayerType = argType;

        attackController.SetAttackAxisInfo(new Vector3(0, 0, -currentPlayerData.m_fWeaponAxisStart), new Vector3(currentPlayerData.m_v2WeaponAxisPosition.x, currentPlayerData.m_v2WeaponAxisPosition.y, 0));
        attackController.SetEffectInfo(currentPlayerData.m_v3EffectPosition, currentPlayerData.m_v3EffectScale);
        attackController.SetEffectImage(currentPlayerData.m_sWaeponEffectPath);

        weaponBoxCollider2D.enabled = currentPlayerData.m_bIsEnableWeaponHit;
        weaponBoxCollider2D.size = currentPlayerData.m_v2WeaponColliderArea;
        weaponBoxCollider2D.offset = currentPlayerData.m_v2Weaponoffset;
        gameObject.transform.localPosition = currentPlayerData.m_v2WeaponPosition;

        renderObject.sprite = Resources.Load<Sprite>(currentPlayerData.m_sWaeponSpritePath);
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
