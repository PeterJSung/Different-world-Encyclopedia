using UnityEngine;
using System.Collections;
using System;

public class Playermove : MonoBehaviour
{
    public const float MOVE_WEIGHT = 0.04f;
    public const float DASH_MOVE_WEIGHT = 0.08f;
    Rigidbody2D rigid2D;
    float jumpForce = 400.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 3.1f;
    float dashForce = 800.0f;
    float time = 0f;
    float dashForce2 = 800.0f;
    float time2 = 0f;
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

        if (col.gameObject.tag == "Ground")
            isGrounded = true;
        jumpcount = 2;
    }


    void Update()
    {
        if (isGrounded)
        {
            // 점프한다
            if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)

            {
                this.rigid2D.AddForce(transform.up * this.jumpForce);
                jumpcount--;
            }
        }

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

