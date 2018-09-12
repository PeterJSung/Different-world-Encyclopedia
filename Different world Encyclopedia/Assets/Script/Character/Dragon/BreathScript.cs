using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonActionModel;

public class BreathScript : MonoBehaviour
{
    DragonSkillModel skillModel;

    SpriteRenderer rendererController = null;
    CapsuleCollider2D colliderController = null;
    // Use this for initialization

    bool duringAnimation = false;

    enum BREATH_STATUS
    {
        NONE = 0, //NOT READY,
        SHEETITING_START = 1, //Need Rendering Init
        SHEETITING_END = 2, // SHEETING_END
        DESTROY = 3 // END
    }

    BREATH_STATUS currentStat = BREATH_STATUS.NONE;

    private bool endSkill = false;
    void Awake()
    {
        rendererController = gameObject.GetComponent<SpriteRenderer>();
        colliderController = gameObject.GetComponent<CapsuleCollider2D>();
    }

    void Start()
    {

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

    // Update is called once per frame
    void Update()
    {
        switch (currentStat)
        {
            case BREATH_STATUS.SHEETITING_START:
                if (!duringAnimation)
                {
                    StartCoroutine(SparkAnimation());
                }
                break;
            case BREATH_STATUS.SHEETITING_END:
                if (!duringAnimation)
                {
                    StartCoroutine(LaserAnimation());
                }
                break;
            case BREATH_STATUS.DESTROY:
                endSkill = true;
                break;
        }
    }

    private IEnumerator SparkAnimation()
    {
        duringAnimation = true;
        Sprite[] targetSprite = skillModel.sheetingSpriteStart;

        int maxLength = targetSprite.Length;
        float frameTime = skillModel.appreanceFrame / 1000;
        float gapTime = skillModel.gapFrame / 1000;
        int renderIndex = 0;

        float startXValue = skillModel.stretchXMin;

        
        colliderController.size = new Vector2(startXValue, colliderController.size.y);
        colliderController.offset = new Vector2(-(skillModel.stretchXMax - colliderController.size.x) / 2, colliderController.offset.y);
        while (renderIndex < maxLength)
        {
            rendererController.sprite = targetSprite[renderIndex];
            renderIndex++;
            yield return new WaitForSeconds(frameTime);
        }
        rendererController.sprite = null;
        yield return new WaitForSeconds(gapTime);
        currentStat = BREATH_STATUS.SHEETITING_END;
        duringAnimation = false;
    }

    private IEnumerator LaserAnimation()
    {
        duringAnimation = true;
        Sprite[] targetSprite = skillModel.sheetingSprite;
        int maxLength = targetSprite.Length;
        float frameTime = skillModel.sheetingFrame / 1000;
        int renderIndex = 0;

        float startXValue = skillModel.stretchXMax;

        colliderController.size = new Vector2(startXValue, colliderController.size.y);
        colliderController.offset = new Vector2(0.0f , colliderController.offset.y);
        while (renderIndex < maxLength)
        {
            rendererController.sprite = targetSprite[renderIndex];
            renderIndex++;
            yield return new WaitForSeconds(frameTime);
        }
        currentStat = BREATH_STATUS.DESTROY;
        duringAnimation = false;
    }

    public bool EndSkill()
    {
        return endSkill;
    }

    public void SetParameter(DragonSkillModel _argModel)
    {
        skillModel = _argModel;
        currentStat = BREATH_STATUS.SHEETITING_START;
    }
}
