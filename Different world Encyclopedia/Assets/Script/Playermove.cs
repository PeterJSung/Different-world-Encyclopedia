using UnityEngine;
using System.Collections;
using System;

public enum CharatorType
{
    ALLIGATOR, //악어야
    MAGITION, // 마법사
    DRAGON, // 드래곤
}

public enum CharatorStatus
{
    NULL = 0, // 아무것도 안함.
    DEAD = 1, //피격
    HIT = 2, //사망
    JUMP = 4, //점프
    ATTACK = 8, // 공격
    MOVE = 16, //이동
    DASH_MOVE = 32, // 대시
    SKILL = 64 //스킬
}

public class Playermove : MonoBehaviour
{
    //캐릭터는 7 Status 를 가지고 있음.
    //1. 사망 1순위
    //2. 피격 2순위
    //3. 점프 3순위
    //4. 공격 3순위
    //5. 이동 3순위
    //6. 대시 3순위
    //7. 스킬 3순위

    public CharatorType selectedCharactorType;
    public GameObject weaponObject;

    public CharatorType currentPlayerType
    {
        get
        {
            return selectedCharactorType;
        }
        set
        {
            selectedCharactorType = value;
        }
    }

    //GlobalObject
    Rigidbody2D rigid2D;

    //Charator Option
    private float MOVE_WEIGHT;
    private float DASH_MOVE_WEIGHT;
    private float JUMP_FORCE;

    private int jumpcount = 1;
    private int blinkcount = 1;
    private bool isGrounded = false;
    private CharatorStatus nowStatus = CharatorStatus.NULL;

    private float attackSpeed = 1.0f;

    MoveFlag moveValue;

    //Charactor Handler
    private Weapon weaponController = null;

    void Awake()
    {
        this.initialize();
    }

    void Start()
    {

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        this.CheckCollision(col);
    }


    void Update()
    {
        this.CheckAttack();
        this.CheckJump();
        this.CheckMove();

        this.DoingAction();
        this.RenderCharactor();
    }

    void RenderCharactor()
    {

    }

    void DoingAction()
    {
        if (this.IsStatus(CharatorStatus.DEAD))
        {
            //사망 시 1순위 이벤트
        }
        else
        {
            if (this.IsStatus(CharatorStatus.HIT))
            {
                //피격 시 2순위 이벤트
            }
            else
            {
                //나머지 3순위 이벤트 모두 동일
                if (this.IsStatus(CharatorStatus.MOVE) || this.IsStatus(CharatorStatus.DASH_MOVE))
                {
                    //이동
                    this.transform.Translate(moveValue.moveWeight, 0, 0);
                    bool isRight = this.moveValue.moveWeight > 0;
                    transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
                    weaponController.setAttackDirection(isRight);
                }

                if (this.IsStatus(CharatorStatus.JUMP))
                {
                    // 점프
                    this.rigid2D.AddForce(transform.up * JUMP_FORCE);
                    nowStatus &= ~CharatorStatus.JUMP;
                }

                if (this.IsStatus(CharatorStatus.ATTACK))
                {
                    // 공격
                    weaponController.AttackMotion();
                    nowStatus &= ~CharatorStatus.ATTACK;
                }

                if (this.IsStatus(CharatorStatus.SKILL))
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
            nowStatus |= CharatorStatus.ATTACK;
        }
    }

    void CheckJump()
    {
        if (isGrounded)
        {
            // 점프한다
            if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)
            {
                nowStatus |= CharatorStatus.JUMP;
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
                nowStatus |= CharatorStatus.DASH_MOVE;
                moveValue.moveWeight = DASH_MOVE_WEIGHT;
            }
            else
            {
                nowStatus |= CharatorStatus.MOVE;
                moveValue.moveWeight = MOVE_WEIGHT;
            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.RightArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveValue.prevValue == KeyCode.LeftArrow && Time.time - moveValue.tDown < 0.5f)
            {
                nowStatus |= CharatorStatus.DASH_MOVE;
                moveValue.moveWeight = -DASH_MOVE_WEIGHT;

            }
            else
            {
                nowStatus |= CharatorStatus.MOVE;
                moveValue.moveWeight = -MOVE_WEIGHT;

            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.LeftArrow;
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
            nowStatus &= ~CharatorStatus.MOVE;
            nowStatus &= ~CharatorStatus.DASH_MOVE;
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

    bool IsStatus(CharatorStatus argStat)
    {
        return (nowStatus & argStat) > 0;
    }

    void initialize()
    {
        //Object Initialize
        moveValue = new MoveFlag();
        this.rigid2D = GetComponent<Rigidbody2D>();
        jumpcount = 0;

        //CharatorData Initialize
        switch (currentPlayerType)
        {
            case CharatorType.ALLIGATOR:
                DASH_MOVE_WEIGHT = 0.08f;
                MOVE_WEIGHT = 0.04f;
                JUMP_FORCE = 400.0f;
                jumpcount = 2;
                blinkcount = 0;
                attackSpeed = 0.25f;
                break;
            case CharatorType.MAGITION:
                break;
            case CharatorType.DRAGON:
                break;
            default:
                //설정되지 않은 캐릭터면 ASSERT
                Debug.Assert(true);
                Debug.Assert(false);
                break;
        }

        if( weaponObject)
        {
            weaponController = weaponObject.GetComponent<Weapon>();
            weaponController.setParameter(attackSpeed, currentPlayerType);
        }

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
}

