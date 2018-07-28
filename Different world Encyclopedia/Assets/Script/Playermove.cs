using UnityEngine;
using DefinitionChar;
using System.Collections;

public class Playermove : MonoBehaviour
{
    private CharaterInfo currentCharInfo;
    private PlayerSkill skillController = null;
    private PlayerMoveData m_stPlayerMove;
    private CustomCharacterInfo.CHAR_TYPE selectedCharacterType;
    public GameObject weaponObject;

    //GlobalObject
    private Rigidbody2D rigid2D;
    private CapsuleCollider2D capsuleCollider2D;
    //Charator Option
    private float MOVE_WEIGHT;
    private float DASH_MOVE_WEIGHT;
    private float JUMP_FORCE;

    private int jumpcount = 1;
    private bool isBlink = false;
    private bool isRavitate = false;
    private bool isGrounded = false;
    private CustomCharacterInfo.CHAR_STATUS nowStatus = CustomCharacterInfo.CHAR_STATUS.NULL;

    private MoveFlag moveValue;
    private Animator animator;

    //Charactor Handler
    private Weapon weaponController = null;

    private RavitateFlag raviValue;
    private GameObject blinkPrefab = null;

    void Awake()
    {
        capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();
        animator = this.GetComponent<Animator>();
        moveValue = new MoveFlag();
        raviValue = new RavitateFlag();
        this.rigid2D = GetComponent<Rigidbody2D>();
        weaponController = weaponObject.GetComponent<Weapon>();

        blinkPrefab = Resources.Load("Prefabs/BlinkObject") as GameObject;
    }

    void Start()
    {
        ReLoadingCharacter();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        CheckCollision(col);
    }


    void Update()
    {
        CheckAttack();
        CheckJump();
        CheckMove();
        CheckRavitate();
    }

    void FixedUpdate()
    {
        DoingAction();
    }
    void DoingAction()
    {
        if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.DEAD))
        {
            //사망 시 1순위 이벤트
        }
        else
        {
            if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.HIT))
            {
                //피격 시 2순위 이벤트
            }
            else
            {
                //나머지 3순위 이벤트 모두 동일
                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.MOVE) || this.IsStatus(CustomCharacterInfo.CHAR_STATUS.DASH_MOVE))
                {
                    bool isRight = this.moveValue.moveWeight > 0;
                    transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
                    //이동
                    if (isBlink && this.IsStatus(CustomCharacterInfo.CHAR_STATUS.DASH_MOVE))
                    {
                        RaycastHit2D rayHitData = Physics2D.Raycast(
                            new Vector2(transform.position.x, transform.position.y),
                            new Vector2(isRight ? 1 : -1, 0),
                            m_stPlayerMove.m_fBlinkDistance, 1 << GlobalLayerMask.TERRIAN_MASK);
                        bool canMaxBlink = rayHitData.collider ? false : true;
                        Vector3 startPosition = gameObject.transform.position;
                        Vector3 nextPosition = new Vector3(
                            canMaxBlink ?
                                startPosition.x + (isRight ? m_stPlayerMove.m_fBlinkDistance : -m_stPlayerMove.m_fBlinkDistance) :
                                rayHitData.point.x,
                            startPosition.y,
                            startPosition.z);

                        //Blink Action
                        this.transform.position = nextPosition;
                        //Clear Data Blink 직후 멈춰야함.
                        nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.DASH_MOVE;
                        moveValue.prevValue = KeyCode.None;
                        this.moveValue.moveWeight = 0;

                        StartCoroutine(BlinkAnimation(startPosition, nextPosition, isRight));
                    }
                    else
                    {
                        this.transform.Translate(moveValue.moveWeight, 0, 0);
                    }
                    weaponController.setAttackDirection(isRight);
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.JUMP))
                {
                    // 점프
                    this.rigid2D.AddForce(transform.up * JUMP_FORCE);
                    nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.JUMP;
                }

                if (isRavitate && IsStatus(CustomCharacterInfo.CHAR_STATUS.RAVITATE))
                {
                    float yVel = (rigid2D.velocity.y + Physics.gravity.y) * rigid2D.gravityScale;
                    //Howering
                    rigid2D.AddForce(new Vector2(0, -yVel), ForceMode2D.Force);
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.ATTACK))
                {
                    // 공격
                    weaponController.AttackMotion();
                    nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.ATTACK;
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.SKILL))
                {
                    skillController.ActionSkill(selectedCharacterType);
                    // 스킬
                }
            }
        }
    }

    void CheckAttack()
    {
        if ((Input.GetKeyDown(KeyCode.Z) || Input.GetKey(KeyCode.Z)) && weaponController.CanAttackMotion())
        {
            nowStatus |= CustomCharacterInfo.CHAR_STATUS.ATTACK;
        }
    }

    void CheckRavitate()
    {
        if (isRavitate)
        {
            if (Input.GetKey(KeyCode.X))
            {
                if (raviValue.canRavitate && Time.time - raviValue.tDown > 0.2f)
                {
                    nowStatus |= CustomCharacterInfo.CHAR_STATUS.RAVITATE;
                    raviValue.canRavitate = false;
                }
            }

            if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.RAVITATE) && Input.GetKeyUp(KeyCode.X))
            {
                nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.RAVITATE;
                raviValue.canRavitate = true;
            }
        }
    }

    void CheckJump()
    {

        // 점프한다
        if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)
        {
            Debug.Log("JUMP");
            isGrounded = false;
            nowStatus |= CustomCharacterInfo.CHAR_STATUS.JUMP;
            jumpcount--;
            raviValue.tDown = Time.time;
        }

    }

    void CheckMove()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (moveValue.prevValue == KeyCode.RightArrow && Time.time - moveValue.tDown < 0.5f)
            {
                nowStatus |= CustomCharacterInfo.CHAR_STATUS.DASH_MOVE;
                moveValue.moveWeight = DASH_MOVE_WEIGHT;
            }
            else
            {
                nowStatus |= CustomCharacterInfo.CHAR_STATUS.MOVE;
                moveValue.moveWeight = MOVE_WEIGHT;
            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.RightArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveValue.prevValue == KeyCode.LeftArrow && Time.time - moveValue.tDown < 0.5f)
            {
                nowStatus |= CustomCharacterInfo.CHAR_STATUS.DASH_MOVE;
                moveValue.moveWeight = -DASH_MOVE_WEIGHT;

            }
            else
            {
                nowStatus |= CustomCharacterInfo.CHAR_STATUS.MOVE;
                moveValue.moveWeight = -MOVE_WEIGHT;

            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.LeftArrow;
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.MOVE;
            nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.DASH_MOVE;
            moveValue.moveWeight = 0;
        }
    }

    void CheckCollision(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Tile":

                TileType nowTile = col.gameObject.GetComponent<TileScript>().SelectTileType;
                if (nowTile == TileType.GROUND ||
                    nowTile == TileType.FLOAT_GROUND)
                {
                    isGrounded = true;
                    jumpcount = m_stPlayerMove.m_iJumpCount;
                }
                break;
            case "Monster":
                break;
            case "Trap":
                break;
        }
    }

    public bool IsPossibleCharaterChange()
    {
        //땅에있으면서 공격 가능할때 캐릭변경가능 공격도중 캐릭변경 ㄴㄴ해
        return isGrounded && weaponController.CanAttackMotion();
    }

    bool IsStatus(CustomCharacterInfo.CHAR_STATUS argStat)
    {
        return (nowStatus & argStat) > 0;
    }

    void ReLoadingCharacter()
    {
        CustomCharacterInfo.CHAR_GLOBAL_DEFAULT_DATA.TryGetValue(selectedCharacterType, out currentCharInfo);
        m_stPlayerMove = currentCharInfo.m_sPlayerMove;
        CharaterBaseDataInitialize();
        GraphicDataInitialize();
        WeaponDataInitialize();
    }

    void CharaterBaseDataInitialize()
    {
        //Object Initialize
        DASH_MOVE_WEIGHT = m_stPlayerMove.m_fDashMoveWeight;
        MOVE_WEIGHT = m_stPlayerMove.m_fMoveWeight;
        JUMP_FORCE = m_stPlayerMove.m_fJumpForce;
        jumpcount = m_stPlayerMove.m_iJumpCount;
        isBlink = m_stPlayerMove.m_bIsBlink;
        isRavitate = m_stPlayerMove.m_bIsEnableRavitate;
    }

    void WeaponDataInitialize()
    {
        if (weaponObject)
        {
            weaponController.setParameter(currentCharInfo.m_sPlaerWeapon, currentCharInfo.m_sPlayerDefaultBullet, selectedCharacterType);
        }
    }

    void GraphicDataInitialize()
    {
        animator.runtimeAnimatorController = Resources.Load(m_stPlayerMove.m_sPlayerSpritePath) as RuntimeAnimatorController;
        capsuleCollider2D.size = m_stPlayerMove.m_v2CharacterColliderArea;
    }


    public class RavitateFlag
    {
        public float tDown;
        public bool canRavitate;
        public RavitateFlag()
        {
            tDown = Time.time;
            canRavitate = true;
        }
    }

    public class MoveFlag
    {
        public float tDown;
        public KeyCode prevValue;
        public float moveWeight;
        public MoveFlag()
        {
            prevValue = 0;
            tDown = Time.time;
            moveWeight = 0.0f;
        }
    }

    public void setCharacterType(CustomCharacterInfo.CHAR_TYPE argType)
    {
        selectedCharacterType = argType;
        ReLoadingCharacter();
    }

    private IEnumerator BlinkAnimation(Vector3 startPosition, Vector3 nextPosition, bool isRight)
    {
        //잔상 단계는 총 5개 최종 위치를 포함한 총 포지션은 6개로 잡는다
        //잔상 1 처음위치(0/5)  Alpha 20% 
        //잔상 2 다음1(1/5) Alpha 40% 
        //잔상 3 다음2(2/5) Alpha 60% 
        //잔상 4 다음3(3/5) Alpha 80% 
        //잔상 5 다음4(4/5) Alpha 100% 
        //현재캐릭터(5/5) 종료위치

        float eachDuration = m_stPlayerMove.m_fBlinkDuration / m_stPlayerMove.m_iBlinkStep;
        float xMove = (nextPosition.x - startPosition.x) / m_stPlayerMove.m_iBlinkStep;
        Color setColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        for (int i = 0; i < m_stPlayerMove.m_iBlinkStep; i++)
        {
            GameObject blinkObject = MonoBehaviour.Instantiate(blinkPrefab) as GameObject;
            setColor.a = (float)(i + 1) / m_stPlayerMove.m_iBlinkStep;
            blinkObject.transform.position = new Vector3(startPosition.x + (xMove * i), startPosition.y, startPosition.z);
            blinkObject.transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
            blinkObject.GetComponent<SpriteRenderer>().material.color = setColor;
            yield return null;//new WaitForSeconds(eachDuration);
            Object.Destroy(blinkObject, eachDuration);
        }
    }
}
