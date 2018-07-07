using UnityEngine;
using System.Collections;
using System;

public class Playermove : MonoBehaviour
{
    public const float MOVE_WEIGHT = 0.04f;
    public const float DASH_MOVE_WEIGHT = 0.08f;
    Rigidbody2D rigid2D;
    public const float JUMP_FORCE = 400.0f;
   
    public int jumpcount = 2;
    public bool isGrounded = false;

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

    MoveFlag moveValue;

    void Start()
    {
        moveValue = new MoveFlag();
        this.rigid2D = GetComponent<Rigidbody2D>();
        jumpcount = 0;
    }

    private void OnCollisionEnter2D(Collision2D col)

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


    void Update()
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

