using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DefinitionChar;

public class CharacterSwitchEffect : MonoBehaviour
{

    public GameObject referenceCharacter;
    public GameObject specificEffect;

    private Playermove moveScript = null;

    private CapsuleCollider2D colliderController = null;
    private SpriteRenderer currentRenderer = null;
    private SpriteRenderer specificRenderer = null;
    private Rigidbody2D rigid2DController = null;
    private Animator animationController = null;


    private Sprite spriteAlligatorHit = null;
    private Sprite spriteMagitionHit = null;
    private Sprite spriteDragonHit = null;
    private Sprite spriteHeroHit = null;

    private Sprite[] spriteSmoke = null;

    private Sprite spriteDownLine = null;

    private bool isLand = false;
    private bool isEndAnimation = false;

    void Awake()
    {
        moveScript = referenceCharacter.GetComponent<Playermove>();

        colliderController = gameObject.GetComponent<CapsuleCollider2D>();
        currentRenderer = gameObject.GetComponent<SpriteRenderer>();
        rigid2DController = gameObject.GetComponent<Rigidbody2D>();
        animationController = gameObject.GetComponent<Animator>();

        specificRenderer = specificEffect.GetComponent<SpriteRenderer>();


        spriteAlligatorHit = Resources.LoadAll<Sprite>("Player/dkrdjdiH").Single(s => s.name == "dkrdjdiH_4");
        spriteMagitionHit = Resources.LoadAll<Sprite>("Player/wichH").Single(s => s.name == "wichH_4");
        spriteDragonHit = Resources.LoadAll<Sprite>("Player/DragonH").Single(s => s.name == "DragonH_4");
        spriteHeroHit = Resources.LoadAll<Sprite>("Player/HeroH").Single(s => s.name == "HeroH_4");

        spriteDownLine = Resources.Load<Sprite>("Character/Common/DownLine");
        spriteSmoke = Resources.LoadAll<Sprite>("Character/Common/LandingSmoke");
        TurnOffComponent();
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isLand)
        {
            specificEffect.transform.position = new Vector3(
           gameObject.transform.position.x,
           gameObject.transform.position.y - 0.2f,
           gameObject.transform.position.z);
        }
    }

    void TurnOffComponent()
    {
        colliderController.enabled = false;
        currentRenderer.enabled = false;
        rigid2DController.gravityScale = 0.0f;
        rigid2DController.isKinematic = true;
        animationController.enabled = false;
    }

    void TurnOnComponent()
    {
        colliderController.enabled = true;
        currentRenderer.enabled = true;
        rigid2DController.gravityScale = 2.0f;
        rigid2DController.isKinematic = false;
        animationController.enabled = true;
    }

    public void SetModeFalldown(CustomCharacterInfo.CHAR_TYPE originType, bool isRight)
    {
        TurnOnComponent();
        gameObject.transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
        gameObject.transform.localEulerAngles = new Vector3(0, 0, 0);
        specificRenderer.sprite = spriteDownLine;

        switch (originType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR: animationController.runtimeAnimatorController = Resources.Load("Anim/Player/dkrdjdiH_0") as RuntimeAnimatorController; break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION: animationController.runtimeAnimatorController = Resources.Load("Anim/Player/wichH_0") as RuntimeAnimatorController; break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON: animationController.runtimeAnimatorController = Resources.Load("Anim/Player/DragonH_0") as RuntimeAnimatorController; break;
            case CustomCharacterInfo.CHAR_TYPE.HERO: animationController.runtimeAnimatorController = Resources.Load("Anim/Player/HeroH_0") as RuntimeAnimatorController; break;
        }

        colliderController.size = moveScript.GetCollisionArea();
        isLand = false;

        //위치 제어
        gameObject.transform.position = new Vector3(
            referenceCharacter.transform.position.x,
            referenceCharacter.transform.position.y + 5.0f,
            referenceCharacter.transform.position.z);
    }

    public void SetModeDisappear(CustomCharacterInfo.CHAR_TYPE originType, bool isRight)
    {
        isEndAnimation = false;
        rigid2DController.velocity = new Vector2(0.0f, 0.0f);
        TurnOffComponent();

        gameObject.transform.localScale = new Vector3(isRight ? 1 : -1, 1, 1);
        currentRenderer.enabled = true;

        gameObject.transform.position = new Vector3(
            referenceCharacter.transform.position.x,
            referenceCharacter.transform.position.y,
            referenceCharacter.transform.position.z);
        switch (originType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR: currentRenderer.sprite = spriteAlligatorHit; break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION: currentRenderer.sprite = spriteMagitionHit; break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON: currentRenderer.sprite = spriteDragonHit; break;
            case CustomCharacterInfo.CHAR_TYPE.HERO: currentRenderer.sprite = spriteHeroHit; break;
        }
        specificEffect.transform.position = new Vector3(
            referenceCharacter.transform.position.x,
            referenceCharacter.transform.position.y + 0.2f,
            referenceCharacter.transform.position.z);
        StartCoroutine(SmokeAnimation());
        StartCoroutine(DisappearAnimation(isRight));
    }

    IEnumerator SmokeAnimation()
    {
        for (int i = 0; i < spriteSmoke.Length; i++)
        {
            specificRenderer.sprite = spriteSmoke[i];
            yield return new WaitForSeconds(0.1f);
        }
        specificRenderer.sprite = null;
    }

    IEnumerator DisappearAnimation(bool isRight)
    {
        const float totalMove = 2.0f;

        const int totalStep = 15;
        const float totalDuration = 1.0f;

        float eachSecond = totalDuration / totalStep;

        const float alpha = 0.5f;

        const int angleMultiple = 30;
        for (float i = 0; i <= totalMove; i += eachSecond)
        {
            float x = isRight ? -i : i;
            float y = -alpha * x * x;

            float zRot = i * angleMultiple * (isRight ? 1 : -1);
            gameObject.transform.position = new Vector3(
                referenceCharacter.transform.position.x + x,
                referenceCharacter.transform.position.y + y,
                referenceCharacter.transform.position.z);
            
            gameObject.transform.localEulerAngles = new Vector3(0,0, zRot);
            yield return null;
        }
        TurnOffComponent();
        isEndAnimation = true;
    }

    public bool IsEndAni()
    {
        return isEndAnimation;
    }

    public bool IsLanding()
    {
        return isLand;
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.gameObject.layer);
        if (col.gameObject.layer == GlobalLayerMask.ALIAS_MASK || col.gameObject.layer == GlobalLayerMask.TERRIAN_MASK)
        {
            isLand = true;
        }
    }
}
