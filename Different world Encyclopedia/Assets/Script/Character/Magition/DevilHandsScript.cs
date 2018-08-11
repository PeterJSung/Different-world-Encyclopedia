using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagitionActionModel;

public class DevilHandsScript : MonoBehaviour
{
    MagitionSkillModel skillModel;

    SpriteRenderer rendererController = null;
    CapsuleCollider2D colliderController = null;

    bool duringAnimation = false;
    enum DEVIL_HANDS_STATUS
    {
        NONE = 0, //NOT READY,
        STRETCH_STATUS = 1,
        DESTROY = 2 // END
    }

    DEVIL_HANDS_STATUS currentStat = DEVIL_HANDS_STATUS.NONE;

    void Awake()
    {
        rendererController = gameObject.GetComponent<SpriteRenderer>();
        colliderController = gameObject.GetComponent<CapsuleCollider2D>();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch (currentStat)
        {
            case DEVIL_HANDS_STATUS.STRETCH_STATUS:
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
        */
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
}
