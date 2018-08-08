using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagitionActionModel;

public class HurricaneBurn : MonoBehaviour
{
    MagitionAttackModel attackModel;

    SpriteRenderer rendererController = null;
    CapsuleCollider2D colliderController = null;

    bool durationAnimation = false;
    enum BURRICANEBURN_STATUS
    {
        NONE = 0, //NOT READY,
        SHEETITING_START = 1, //Need Rendering Init
        SHEETITING = 2, // SHEETING
        SHEETITING_END = 3, // SHEETING_END
        DESTROY = 4 // END
    }

    BURRICANEBURN_STATUS currentStat = BURRICANEBURN_STATUS.NONE;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.isTrigger && isVailidTarget(other.gameObject.layer))
        {
            //유효한 타겟인가??
            //추후 데미지 파라미터는 따로 설정함 Penetrate Option 추가 가능.
            Debug.Log("해당 타겟에 데미지 입히자");
            //currentStat = FIREBALL_STATUS.SHEETING_END;
        }
    }

    void Awake()
    {
        rendererController = gameObject.GetComponent<SpriteRenderer>();
        colliderController = gameObject.GetComponent<CapsuleCollider2D>();
    }

    // Use this for initialization
    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        switch (currentStat)
        {
            case BURRICANEBURN_STATUS.SHEETITING_START:
                UpdateTransform();
                if (!durationAnimation)
                {
                    durationAnimation = true;
                    StartCoroutine(ApprearenceAnimation(true));
                }
                break;
            case BURRICANEBURN_STATUS.SHEETITING:
                UpdateTransform();
                if (!durationAnimation)
                {
                    durationAnimation = true;
                    StartCoroutine(SheetingAnimation());
                }
                break;
            case BURRICANEBURN_STATUS.SHEETITING_END:
                UpdateTransform();
                if (!durationAnimation)
                {
                    durationAnimation = true;
                    StartCoroutine(ApprearenceAnimation(false));
                }
                break;
            case BURRICANEBURN_STATUS.DESTROY:
                Destroy(this.gameObject);
                break;
        }
    }

    private void UpdateTransform()
    {
        Vector3 refPos = attackModel.referencePosition.transform.position;
        
        gameObject.transform.position = new Vector3(
            refPos.x + (attackModel.rightFunctionPointer() ? +0.6f : -0.6f),
            refPos.y,
            refPos.z);

        gameObject.transform.localScale = new Vector3(
            gameObject.transform.localScale.x * attackModel.scale * (attackModel.rightFunctionPointer() ? 1 : -1),
            gameObject.transform.localScale.y * attackModel.scale,
            gameObject.transform.localScale.z);
    }

    private IEnumerator ApprearenceAnimation(bool isStart)
    {
        Sprite[] targetSprite = isStart ? attackModel.sheetingSpriteStart : attackModel.sheetingSpriteEnd;

        int maxLength = targetSprite.Length;
        float frameTime = attackModel.appreanceFrame / 1000;
        int renderIndex = 0;

        float startYValue = isStart ? attackModel.stretchYMin : attackModel.stretchYMax;
        float endYValue = isStart ? attackModel.stretchYMax : attackModel.stretchYMin;

        float eachYStep = (endYValue - startYValue) / maxLength;

        float currentYValue = startYValue;
        colliderController.size = new Vector2(colliderController.size.x, startYValue);
        while (renderIndex < maxLength)
        {
            colliderController.size = new Vector2(colliderController.size.x, currentYValue);
            currentYValue += eachYStep;
            rendererController.sprite = targetSprite[renderIndex];
            renderIndex++;
            yield return new WaitForSecondsRealtime(frameTime);
        }
        colliderController.size = new Vector2(colliderController.size.x, endYValue);

        renderIndex = 0;
        if (isStart)
        {
            currentStat = BURRICANEBURN_STATUS.SHEETITING;
        }
        else
        {
            currentStat = BURRICANEBURN_STATUS.DESTROY;
        }
        durationAnimation = false;
    }

    private IEnumerator SheetingAnimation()
    {
        int maxLength = attackModel.sheetingSprite.Length;
        float frameTime = attackModel.sheetingFrame / 1000;
        int renderIndex = 0;
        float startTime = Time.time;
        while(Time.time < startTime + attackModel.floatingTime)
        {
            while (renderIndex < maxLength)
            {
                rendererController.sprite = attackModel.sheetingSprite[renderIndex];
                renderIndex++;
                yield return new WaitForSecondsRealtime(frameTime);
            }
            renderIndex = 0;
        }
        

        currentStat = BURRICANEBURN_STATUS.SHEETITING_END;
        durationAnimation = false;
    }

    public void SetParameter(MagitionAttackModel _argModel)
    {
        attackModel = _argModel;
        currentStat = BURRICANEBURN_STATUS.SHEETITING_START;
    }
}
