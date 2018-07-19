using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineBulletModel;

//탄알의 위치는 시전자가 구현한다. 탄알은 움직이는 것만 고려하자.

public class BulletController : MonoBehaviour
{
    BulletModel.BulletData currentBulletData = null;
    
    private bool isEnd = false;
    private bool isStart = false;

    private Vector3 startPosition;
    private Transform currentTransForm;

    private int renderingIndex = 0;

    private SpriteRenderer renderObj = null;


    //효과가 끝나면서 추가로 다른애들도 맞는지?
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Tile")
        {
            isEnd = true;
        }
        else
        {
            if (isVailidTarget(other.gameObject.layer))
            {
                //유효한 타겟인가??
                Debug.Log("해당 타겟에 데미지 입히자");
                if (isEnd == false && currentBulletData.penetrate.penetrateCount > 0)
                {
                    currentBulletData.penetrate.penetrateCount--;
                }
                else
                {
                    isEnd = true;
                }
            }
        }
    }

    // Use this for initialization
    void Start()
    {
        renderObj = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStart)
        {
            //load Complete
            if (isEnd)
            {
                //탄알 날아가는게 끝났는지? 끝난상태면 애니메이션을 해주던 뭘해주던 함.
                StartCoroutine(EndAnimation());
            }
            else
            {
                calculateNextPosition();
                this.gameObject.transform.TransformPoint(currentTransForm.position);


                renderObj.sprite = currentBulletData.sheetingsprite[renderingIndex];
                renderingIndex++;
                if (renderingIndex == currentBulletData.sheetingsprite.Length)
                {
                    renderingIndex = 0;
                }

                //Check Sheeting area;

                float distance = Vector3.Distance(startPosition, currentTransForm.position);
                if (distance > currentBulletData.sheetingLength)
                {
                    isEnd = true;
                }
            }
        }
    }

    IEnumerator EndAnimation()
    {
        isStart = false;
        isEnd = false;

        float time = 0f;

        renderingIndex = 0;
        while (time < currentBulletData.disapearTiming)
        {
            time += Time.deltaTime / currentBulletData.disapearTiming;
            renderObj.sprite = currentBulletData.endSprite[renderingIndex];
            renderingIndex++;
            if (renderingIndex == currentBulletData.endSprite.Length)
            {
                renderingIndex = 0;
            }
            yield return null;
        }
        Destroy(this.gameObject);
    }


    private void calculateNextPosition()
    {
        Vector3 moveOffet = new Vector3();

        switch (currentBulletData.dir)
        {
            //오른쪽을 기준으로 함.
            case BulletModel.BULLET_DIRECTION.NONE:
                moveOffet.x = moveOffet.y = moveOffet.z = 0;
                break;
            case BulletModel.BULLET_DIRECTION.LEFT:
                moveOffet.x = -1;
                moveOffet.y = 0;
                break;
            case BulletModel.BULLET_DIRECTION.TOP:
                moveOffet.x = 0;
                moveOffet.y = 1;
                break;
            case BulletModel.BULLET_DIRECTION.RIGHT:
                moveOffet.x = 1;
                moveOffet.y = 0;
                break;
            case BulletModel.BULLET_DIRECTION.BOT:
                moveOffet.x = 0;
                moveOffet.y = -1;
                break;
        }

        switch (currentBulletData.motion)
        {
            case BulletModel.MOTION_TYPE.NONE:
                moveOffet.x *= 0.0f;
                moveOffet.y *= 0.0f;
                moveOffet.z *= 0.0f;
                break;
            case BulletModel.MOTION_TYPE.STRAIGHT:
                moveOffet.x *= currentBulletData.weight.x;
                moveOffet.y *= currentBulletData.weight.y;
                break;
            case BulletModel.MOTION_TYPE.CURVE:
                //moveOffet.x = moveOffet.y = 1.0f;
                //커브 미구현
                break;
        }
        currentTransForm.position += moveOffet;
    }

    private bool isVailidTarget(int hittedLayer)
    {
        foreach (int eachLayer in currentBulletData.tLayer)
        {
            if (eachLayer == hittedLayer)
            {
                return true;
            }
        }
        return false;
    }

    //Bullet 의 최소속성. 
    /*
     * 1. 방향성
     * 2. 움직임 타입
     * 3 & 4 활강 및 종료시 Sprite
     * 5. 관통 유무
     * 6. 타겟 Layer
     */
    public void setInitialize(BulletModel.BulletData argBulletData)
    {
        currentBulletData = argBulletData;

        //시작지점이 현재지점임
        startPosition = new Vector3(
            this.gameObject.transform.position.x,
            this.gameObject.transform.position.y,
            this.gameObject.transform.position.z);
        currentTransForm = this.gameObject.transform;
        //데이터 세팅이 끝난 후 설정
        isStart = true;
    }
}
