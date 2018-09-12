using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagitionActionModel;

public class DevilHandsScript : MonoBehaviour
{
    MagitionSkillModel skillModel;

    SpriteRenderer rendererCircleController = null;
    SpriteRenderer rendererHandsController = null;
    CapsuleCollider2D colliderController = null;

    bool duringAnimation = false;

    public GameObject circleObject;
    public GameObject handsObject;
    enum DEVIL_HANDS_STATUS
    {
        NONE = 0, //NOT READY,
        STRETCH_STATUS = 1,
        DESTROY = 2 // END
    }

    DEVIL_HANDS_STATUS currentStat = DEVIL_HANDS_STATUS.NONE;

    void Awake()
    {
        rendererCircleController = circleObject.GetComponent<SpriteRenderer>();
        rendererHandsController = handsObject.GetComponent<SpriteRenderer>();
        colliderController = handsObject.GetComponent<CapsuleCollider2D>();
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
            case DEVIL_HANDS_STATUS.STRETCH_STATUS:
                if (!duringAnimation)
                {
                    StartCoroutine(StretchAnimation());
                }
                break;
            case DEVIL_HANDS_STATUS.DESTROY:
                if (!duringAnimation)
                {
                    StartCoroutine(DisappearAnimation());
                }
                break;
        }
    }

    IEnumerator StretchAnimation()
    {
        int renderIndex = 0;
        int maxIndex = skillModel.devilHandsSprite.Length;
        float renderTiming = skillModel.handsRenderFrame / 1000;
        while (true)
        {
            rendererHandsController.sprite = skillModel.devilHandsSprite[renderIndex];

            yield return new WaitForSeconds(renderTiming);
            renderIndex++;
            if (maxIndex == renderIndex)
            {
                renderIndex = 0;
            }
        }
    }

    IEnumerator DisappearAnimation()
    {
        yield return null;
    }

    IEnumerator CircleAnimation()
    {
        int renderIndex = 0;
        int maxIndex = skillModel.magicCircleSprite.Length;
        float renderTiming = skillModel.circleRenderFrame / 1000;
        while (true)
        {
            rendererCircleController.sprite = skillModel.magicCircleSprite[renderIndex];
            
            yield return new WaitForSeconds(renderTiming);
            renderIndex++;
            if(maxIndex == renderIndex)
            {
                renderIndex = 0;
            }
        }
    }

    private bool isVailidTarget(int hittedLayer)
    {
        foreach (int eachLayer in skillModel.targetArray)
        {
            if (eachLayer == hittedLayer)
            {
                return true;
            }
        }
        return false;
    }

    public void SetParameter(MagitionSkillModel _argModel)
    {
        skillModel = _argModel;
        StartCoroutine(CircleAnimation());
        currentStat = DEVIL_HANDS_STATUS.STRETCH_STATUS;
    }
}
