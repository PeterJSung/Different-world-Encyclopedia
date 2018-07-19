using UnityEngine;
using DefinitionChar;

public class Playermove : MonoBehaviour
{

    private CustomCharacterInfo.CHAR_TYPE selectedCharactorType;
    public GameObject weaponObject;

    //GlobalObject
    Rigidbody2D rigid2D;
    CapsuleCollider2D capsuleCollider2D;
    //Charator Option
    private float MOVE_WEIGHT;
    private float DASH_MOVE_WEIGHT;
    private float JUMP_FORCE;

    private int jumpcount = 1;
    private int blinkcount = 1;
    private bool isGrounded = false;
    private CustomCharacterInfo.CHAR_STATUS nowStatus = CustomCharacterInfo.CHAR_STATUS.NULL;

    private float attackSpeed = 1.0f;

    MoveFlag moveValue;
    Animator animator;

    //Charactor Handler
    private Weapon weaponController = null;

    void Awake()
    {
        capsuleCollider2D = this.GetComponent<CapsuleCollider2D>();
        animator = this.GetComponent<Animator>();
        moveValue = new MoveFlag();
        this.rigid2D = GetComponent<Rigidbody2D>();
        weaponController = weaponObject.GetComponent<Weapon>();

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
        this.CheckAttack();
        this.CheckJump();
        this.CheckMove();

        this.DoingAction();
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
                    //이동
                    this.transform.Translate(moveValue.moveWeight, 0, 0);
                    bool isRight = this.moveValue.moveWeight > 0;
                    transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
                    weaponController.setAttackDirection(isRight);
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.JUMP))
                {
                    // 점프
                    this.rigid2D.AddForce(transform.up * JUMP_FORCE);
                    nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.JUMP;
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.ATTACK))
                {
                    // 공격
                    weaponController.AttackMotion();
                    nowStatus &= ~CustomCharacterInfo.CHAR_STATUS.ATTACK;
                }

                if (this.IsStatus(CustomCharacterInfo.CHAR_STATUS.SKILL))
                {
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

    void CheckJump()
    {
        if (isGrounded)
        {
            // 점프한다
            if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)
            {
                nowStatus |= CustomCharacterInfo.CHAR_STATUS.JUMP;
                jumpcount--;
            }
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
                    jumpcount = 2;
                }
                break;
            case "Monster":
                break;
            case "Trap":
                break;
        }
    }

    bool IsStatus(CustomCharacterInfo.CHAR_STATUS argStat)
    {
        return (nowStatus & argStat) > 0;
    }

    void ReLoadingCharacter()
    {
        CharaterBaseDataInitialize();
        GraphicDataInitialize();
        WeaponDataInitialize();
    }

    void CharaterBaseDataInitialize()
    {
        //Object Initialize
        jumpcount = 0;

        //CharatorData Initialize
        switch (selectedCharactorType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                DASH_MOVE_WEIGHT = 0.08f;
                MOVE_WEIGHT = 0.04f;
                JUMP_FORCE = 400.0f;
                jumpcount = 2;
                blinkcount = 0;
                attackSpeed = 0.25f;
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                break;
            default:
                //설정되지 않은 캐릭터면 ASSERT
                Debug.Assert(true);
                Debug.Assert(false);
                break;
        }

        
    }

    void WeaponDataInitialize()
    {
        if (weaponObject)
        {
            weaponController.setParameter(attackSpeed, selectedCharactorType);
        }
    }

    void GraphicDataInitialize()
    {
        string path = "Anim";
        Vector2 resizePoint;
        CustomCharacterInfo.CHAR_COLLIDER_AREA.TryGetValue(selectedCharactorType, out resizePoint);
        switch (selectedCharactorType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR:
                path += "/dkrdjdiH_0";
                break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION:
                path += "/wichH_0";
                break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON:
                path += "/DragonH_0";
                break;
            case CustomCharacterInfo.CHAR_TYPE.HERO:
                break;
            default:
                //설정되지 않은 캐릭터면 ASSERT
                Debug.Assert(true);
                Debug.Assert(false);
                break;
        }


        animator.runtimeAnimatorController = Resources.Load(path) as RuntimeAnimatorController;
        capsuleCollider2D.size = resizePoint;
    }

    void DEBUG_KEY_FUNCTION()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Debug.Log("[RIGHT]KEY DOWN");
        }

        if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            Debug.Log("[RIGHT]KEY UP");
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            Debug.Log("[RIGHT]KEY PRESS");
        }


        //
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Debug.Log("[LEFT]KEY DOWN");
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            Debug.Log("[LEFT]KEY UP");
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Debug.Log("[LEFT]KEY PRESS");
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
        selectedCharactorType = argType;
        ReLoadingCharacter();
    }
}

