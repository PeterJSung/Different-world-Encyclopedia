using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DefineBulletModel;

//탄알의 위치는 시전자가 구현한다. 탄알은 움직이는 것만 고려하자.

public class BulletController : MonoBehaviour
{
    BulletData currentBulletData = null;

    private bool isEnd = false;
    private bool isStart = false;

    private Vector3 startPosition;
    private Transform currentTransForm;

    private int renderingIndex = 0;

    private SpriteRenderer renderObj = null;

    private float time = 0;

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
                if (isEnd == false && currentBulletData.CanPenetrate())
                {
                    currentBulletData.DoPenetrate();
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
        if (currentBulletData != null && currentBulletData.GetStartSprite() != null)
        {
            StartCoroutine(StartAnimation());
        } else
        {
            isStart = true;
        }
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
                //Check Sheeting area;
                renderObj.sprite = currentBulletData.sheetingsprite[renderingIndex];
                renderingIndex++;
                if (renderingIndex == currentBulletData.sheetingsprite.Length)
                {
                    renderingIndex = 0;
                }                
                switch (currentBulletData.motion)
                {
                    case MOTION_TYPE.FLOAT:
                        time += Time.deltaTime;
                        if( time > currentBulletData.GetFloattingTimimg())
                        {
                            isEnd = true;
                        }
                        break;
                    case MOTION_TYPE.STRAIGHT:
                        calculateNextPosition();
                        this.gameObject.transform.TransformPoint(currentTransForm.position);

                        float distance = Vector3.Distance(startPosition, currentTransForm.position);
                        if (distance > currentBulletData.GetSheetingLength())
                        {
                            isEnd = true;
                        }
                        break;
                }


            }
        }
    }

    IEnumerator StartAnimation()
    {
        renderingIndex = 0;
        Sprite[] startSprite = currentBulletData.GetStartSprite();
        for(int i = 0; i < startSprite.Length; i++)
        {
            renderObj.sprite = startSprite[renderingIndex];
            renderingIndex++;
            yield return null;
        }
        isStart = true;
        renderingIndex = 0;
    }

    IEnumerator EndAnimation()
    {
        isStart = false;
        isEnd = false;

        float time = 0f;

        renderingIndex = 0;
        Sprite[] endSprite = currentBulletData.GetEndSprite();
        if (endSprite == null)
        {
            //End Sprite 가 없을경우 Aplpha 값 변경으로 사라진다/
            float sTransValue = 1.0f;
            float eTransValue = 0.0f;

            Color effectTransparent = new Color(1, 1, 1, sTransValue);

            while (time < currentBulletData.disapearTiming)
            {
                time += Time.deltaTime / currentBulletData.disapearTiming;

                effectTransparent.a = Mathf.Lerp(sTransValue, eTransValue, time);
                renderObj.material.color = effectTransparent;
                yield return null;
            }
        }
        else
        {
            if (currentBulletData.GetStartSprite() != null)
            {
                //End sprite 도 있고 strt Sprite 도 있는경우 대칭을위해 프레임단위로 랜더링ㅎ나다/.
                for (int i = 0; i < endSprite.Length; i++)
                {
                    renderObj.sprite = endSprite[renderingIndex];
                    renderingIndex++;
                    yield return null;
                }
            }
            else
            {
                while (time < currentBulletData.disapearTiming)
                {
                    time += Time.deltaTime / currentBulletData.disapearTiming;


                    renderObj.sprite = endSprite[renderingIndex];
                    renderingIndex++;
                    if (renderingIndex == endSprite.Length)
                    {
                        renderingIndex = 0;
                    }

                    yield return null;
                }
            }
        }
        Destroy(this.gameObject);
    }


    private void calculateNextPosition()
    {
        Vector3 moveOffet = new Vector3();

        switch (currentBulletData.GetBulletDirection())
        {
            //오른쪽을 기준으로 함.
            case BULLET_DIRECTION.NONE:
                moveOffet.x = moveOffet.y = moveOffet.z = 0;
                break;
            case BULLET_DIRECTION.LEFT:
                moveOffet.x = -1;
                moveOffet.y = 0;
                break;
            case BULLET_DIRECTION.TOP:
                moveOffet.x = 0;
                moveOffet.y = 1;
                break;
            case BULLET_DIRECTION.RIGHT:
                moveOffet.x = 1;
                moveOffet.y = 0;
                break;
            case BULLET_DIRECTION.BOT:
                moveOffet.x = 0;
                moveOffet.y = -1;
                break;
        }

        switch (currentBulletData.motion)
        {
            case MOTION_TYPE.NONE:
                moveOffet.x *= 0.0f;
                moveOffet.y *= 0.0f;
                moveOffet.z *= 0.0f;
                break;
            case MOTION_TYPE.STRAIGHT:
                moveOffet.x *= currentBulletData.GetShootingForce().x;
                moveOffet.y *= currentBulletData.GetShootingForce().y;
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
    public void setInitialize(BulletData argBulletData)
    {
        currentBulletData = argBulletData;

        //시작지점이 현재지점임
        startPosition = currentBulletData.startPosition;
        gameObject.transform.position = startPosition;
        currentTransForm = this.gameObject.transform;
        //데이터 세팅이 끝난 후 설정
        time = 0;
        
    }
}
