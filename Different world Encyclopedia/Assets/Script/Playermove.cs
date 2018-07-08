using UnityEngine;
using System.Collections;
using System;

public enum CharatorType
{
    ALLIGATOR, //악어야
    MAGITION, // 마법사
    DRAON, // 드래곤
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

public class Playermove : MonoBehaviour
{
    //캐릭터는 7 Status 를 가지고 있음.
    //1. 피격
    //2. 사망
    //3. 사망
    //4. 공격
    //5. 이동
    //6. 대시
    //7. 스킬

    public CharatorType selectedCharactorType;

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

    public float MOVE_WEIGHT;
    public float DASH_MOVE_WEIGHT;
    Rigidbody2D rigid2D;
    public float JUMP_FORCE;
   
    public int jumpcount = 2;
    public int blinkcount = 1;
    public bool isGrounded = false;


    MoveFlag moveValue;

    void Awake()
    {

    }

    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        CheckCollision(col);
    }


    void Update()
    {
        
        CheckJump();
        CheckMove();


        this.transform.Translate(moveValue.moveWeight, 0, 0);

        

        // 캐릭터 스프라이트 반전
        if (moveValue.moveWeight > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveValue.moveWeight < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void CheckJump()
    {
        if (isGrounded)
        {
            // 점프한다
            if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)

            {
                this.rigid2D.AddForce(transform.up * JUMP_FORCE);
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
                moveValue.moveWeight = DASH_MOVE_WEIGHT;
            }
            else
            {
                moveValue.moveWeight = MOVE_WEIGHT;
            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.RightArrow;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (moveValue.prevValue == KeyCode.LeftArrow && Time.time - moveValue.tDown < 0.5f)
            {
                moveValue.moveWeight = -DASH_MOVE_WEIGHT;
            }
            else
            {
                moveValue.moveWeight = -MOVE_WEIGHT;

            }
            moveValue.tDown = Time.time;
            moveValue.prevValue = KeyCode.LeftArrow;
        }
        else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow))
        {
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
                
                MOVE_WEIGHT = 0.08f;
                JUMP_FORCE = 400.0f;
                jumpcount = 2;
                blinkcount = 0;
                break;
            case CharatorType.MAGITION:
                break;
            case CharatorType.DRAON:
                break;
            default:
                //설정되지 않은 캐릭터면 ASSERT
                Debug.Assert(true);
                Debug.Assert(false);
                break;
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

}

