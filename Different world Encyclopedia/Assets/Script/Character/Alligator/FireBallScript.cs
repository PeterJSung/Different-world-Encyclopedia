using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AlligatorActionModel;


public class FireBallScript : MonoBehaviour
{

    AlligatorAttackModel attackModel = null;
    SpriteRenderer rendererController = null;

    Vector3 startPosition;
    Vector3 currentPostion;

    enum FIREBALL_STATUS
    {
        NONE = 0, //NOT READY,
        RENDER_INIT = 1, //Need Rendering Init
        SHEETING = 2, // SHEETING
        SHEETING_END = 3, // SHEETING_END
        DESTROY = 4 // END
    }

    FIREBALL_STATUS currentStat = FIREBALL_STATUS.NONE;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && other.gameObject.tag == "Tile")
        {
            currentStat = FIREBALL_STATUS.SHEETING_END;
        }
        else
        {
            if (!other.isTrigger && isVailidTarget(other.gameObject.layer))
            {
                //유효한 타겟인가??
                //추후 데미지 파라미터는 따로 설정함 Penetrate Option 추가 가능.
                Debug.Log("해당 타겟에 데미지 입히자");
                currentStat = FIREBALL_STATUS.SHEETING_END;
            }
        }
    }

    private bool isVailidTarget(int hittedLayer)
    {
        foreach (int eachLayer in attackModel.targetArray)
        {
            if (eachLayer == hittedLayer)
            {
                return true;
            }
        }
        return false;
    }

    void Awake()
    {
        rendererController = gameObject.GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        switch (currentStat)
        {
            case FIREBALL_STATUS.RENDER_INIT:
                StartCoroutine(SheetingAnimation());

                break;
            case FIREBALL_STATUS.SHEETING:
                gameObject.transform.position = new Vector3(
                    gameObject.transform.position.x + ((attackModel.isRight ? 1 : -1) * attackModel.bulletSpeed),
                    gameObject.transform.position.y,
                    gameObject.transform.position.z);
                //다음 위치로 이동.

                currentPostion = gameObject.transform.position;

                float distance = Vector3.Distance(startPosition, currentPostion);
                if (distance > attackModel.sheetingArea)
                {
                    currentStat = FIREBALL_STATUS.SHEETING_END;
                }
                //날라간 거리 체크
                break;
            case FIREBALL_STATUS.SHEETING_END:
                StartCoroutine(EndAnimation());

                break;
            case FIREBALL_STATUS.DESTROY:
                Destroy(this.gameObject);
                break;
        }
    }


    private IEnumerator SheetingAnimation()
    {
        currentStat = FIREBALL_STATUS.SHEETING;

        int maxLength = attackModel.sheetingSprite.Length;
        float frameTime = attackModel.frame / 1000;
        int renderIndex = 0;
        while (currentStat == FIREBALL_STATUS.SHEETING)
        {
            while(renderIndex < maxLength)
            {
                rendererController.sprite = attackModel.sheetingSprite[renderIndex];
                renderIndex++;
                yield return new WaitForSecondsRealtime(frameTime);
            }
            renderIndex = 0;
        }
    }

    private IEnumerator EndAnimation()
    {
        int maxLength = attackModel.destroySprite.Length;
        float frameTime = attackModel.frame / 1000;

        for (int multipleIndex = 0; multipleIndex < attackModel.endFrameMultiple; multipleIndex++)
        {
            for (int renderIndex = 0; renderIndex < maxLength; renderIndex++)
            {
                rendererController.sprite = attackModel.destroySprite[renderIndex];
                yield return new WaitForSecondsRealtime(frameTime);
            }
        }
        
        currentStat = FIREBALL_STATUS.DESTROY;
    }

    public void SetParameter(AlligatorAttackModel _argModel)
    {
        attackModel = _argModel;
        currentStat = FIREBALL_STATUS.RENDER_INIT;

        gameObject.transform.localScale = new Vector3(
            gameObject.transform.localScale.x * attackModel.scale * (attackModel.isRight ? 1 : -1),
            gameObject.transform.localScale.y * attackModel.scale,
            gameObject.transform.localScale.z);

        startPosition = currentPostion = gameObject.transform.position;
    }
}
