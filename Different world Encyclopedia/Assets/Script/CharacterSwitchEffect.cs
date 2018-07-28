using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DefinitionChar;

public class CharacterSwitchEffect : MonoBehaviour
{

    public GameObject referenceCharacter;

    private Playermove moveScript = null;

    private CapsuleCollider2D colliderController = null;
    private SpriteRenderer rendererController = null;
    private Rigidbody2D rigid2DController = null;
    private Animator animationController = null;


    private Sprite spriteAlligatorHit = null;
    private Sprite spriteMagitionHit = null;
    private Sprite spriteDragonHit = null;
    private Sprite spriteHeroHit = null;

    private bool isLand = false;

    void Awake()
    {
        moveScript = referenceCharacter.GetComponent<Playermove>();

        colliderController = gameObject.GetComponent<CapsuleCollider2D>();
        rendererController = gameObject.GetComponent<SpriteRenderer>();
        rigid2DController = gameObject.GetComponent<Rigidbody2D>();
        animationController = gameObject.GetComponent<Animator>();
        TurnOffComponent();

        spriteAlligatorHit = Resources.LoadAll<Sprite>("Player/dkrdjdiH").Single(s => s.name == "dkrdjdiH_4");
        spriteMagitionHit = Resources.LoadAll<Sprite>("Player/wichH").Single(s => s.name == "wichH_4");
        spriteDragonHit = Resources.LoadAll<Sprite>("Player/DragonH").Single(s => s.name == "DragonH_4");
        spriteHeroHit = Resources.LoadAll<Sprite>("Player/HeroH").Single(s => s.name == "HeroH_4");
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void TurnOffComponent()
    {
        colliderController.enabled = false;
        rendererController.enabled = false;
        rigid2DController.isKinematic = true;
        animationController.enabled = false;
    }

    void TurnOnComponent()
    {
        colliderController.enabled = true;
        rendererController.enabled = true;
        rigid2DController.isKinematic = false;
        animationController.enabled = true;
    }

    public void SetModeFalldown(CustomCharacterInfo.CHAR_TYPE originType)
    {
        TurnOnComponent();

        animationController.runtimeAnimatorController = Resources.Load(moveScript.GetanimatorPath()) as RuntimeAnimatorController;
        colliderController.size = moveScript.GetCollisionArea();
        isLand = false;

        gameObject.transform.position = new Vector3(
            referenceCharacter.transform.position.x,
            referenceCharacter.transform.position.y + 5.0f,
            referenceCharacter.transform.position.z);
    }

    public void SetModeDisappear(CustomCharacterInfo.CHAR_TYPE originType)
    {
        animationController.runtimeAnimatorController = null;
        colliderController.enabled = false;
        rigid2DController.isKinematic = false;
        animationController.enabled = false;

        switch (originType)
        {
            case CustomCharacterInfo.CHAR_TYPE.ALLIGATOR: rendererController.sprite = spriteAlligatorHit; break;
            case CustomCharacterInfo.CHAR_TYPE.MAGITION: rendererController.sprite = spriteMagitionHit; break;
            case CustomCharacterInfo.CHAR_TYPE.DRAGON: rendererController.sprite = spriteDragonHit; break;
            case CustomCharacterInfo.CHAR_TYPE.HERO: rendererController.sprite = spriteHeroHit; break;
        }
    }

    public bool IsLanding()
    {
        return isLand;
    }


    void CheckCollision(Collision2D col)
    {
        switch (col.gameObject.tag)
        {
            case "Tile":
                TileType nowTile = col.gameObject.GetComponent<TileScript>().SelectTileType;
                if (nowTile == TileType.GROUND ||
                    nowTile == TileType.FLOAT_GROUND)
                {
                    //땅에 도착할 시 Ground 도착
                    isLand = true;
                }
                break;
        }
    }
}
