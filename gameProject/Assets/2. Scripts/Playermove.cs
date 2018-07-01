using UnityEngine;
using System.Collections;

public class Playermove : MonoBehaviour
{
    Rigidbody2D rigid2D;
    Animator animator;
    float jumpForce = 680.0f;
    float walkForce = 30.0f;
    float maxWalkSpeed = 5.0f;
    public int jumpcount = 2;
    public bool isGrounded = false;

    void Start()
    {
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
            // 점프한다
            if (Input.GetKeyDown(KeyCode.X) && jumpcount > 0)
      
        {
            this.rigid2D.AddForce(transform.up * this.jumpForce);
               jumpcount--;
        }
           
        // 좌우이동
        int key = 0;
        if (Input.GetKey(KeyCode.RightArrow)) key = 1;
        if (Input.GetKey(KeyCode.LeftArrow)) key = -1;

        // 플레이어 속도
        float speedx = Mathf.Abs(this.rigid2D.velocity.x);

        // 스피드 제한
        if (speedx < this.maxWalkSpeed)
        {
            this.rigid2D.AddForce(transform.right * key * this.walkForce);
        }

        // 반전대책
        if (key != 0)
        {
            transform.localScale = new Vector3(key, 1, 1);
        }

   

    }
}
