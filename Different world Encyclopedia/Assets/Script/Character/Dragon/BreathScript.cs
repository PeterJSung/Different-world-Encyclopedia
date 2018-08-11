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
                StartCoroutine(SparkAnimation());
                break;
            case BREATH_STATUS.SHEETITING_END:
                StartCoroutine(EndAnimation());
                break;
            case BREATH_STATUS.DESTROY:
                Destroy(this.gameObject);
                break;
        }
    }

    private IEnumerator SparkAnimation()
    {
        Sprite[] targetSprite = skillModel.sheetingSpriteStart;

        int maxLength = targetSprite.Length;
        float frameTime = skillModel.appreanceFrame / 1000;
        float gapTime = skillModel.gapFrame / 1000;
        int renderIndex = 0;

        float startYValue = skillModel.stretchYMin;
        
        colliderController.size = new Vector2(colliderController.size.x, startYValue);
        while (renderIndex < maxLength)
        {
            Debug.Log(renderIndex  + " " + targetSprite[renderIndex].name);
            rendererController.sprite = targetSprite[renderIndex];
            renderIndex++;
            yield return new WaitForSeconds(frameTime);
        }

        yield return new WaitForSeconds(gapTime);
        currentStat = BREATH_STATUS.SHEETITING_END;
    }

    private IEnumerator EndAnimation()
    {
        Sprite[] targetSprite = skillModel.sheetingSprite;

        int maxLength = targetSprite.Length;
        float frameTime = skillModel.sheetingFrame / 1000;
        int renderIndex = 0;

        float startYValue = skillModel.stretchYMax;

        colliderController.size = new Vector2(colliderController.size.x, startYValue);
        while (renderIndex < maxLength)
        {
            Debug.Log(renderIndex + " " + targetSprite[renderIndex].name);
            rendererController.sprite = targetSprite[renderIndex];
            renderIndex++;
            yield return new WaitForSeconds(frameTime);
        }
        endSkill = true;
        currentStat = BREATH_STATUS.DESTROY;
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
