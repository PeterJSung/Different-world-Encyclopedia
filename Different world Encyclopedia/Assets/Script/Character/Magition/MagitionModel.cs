using System.Collections;
using UnityEngine;

namespace MagitionActionModel
{
    public sealed class MagitionAttackModel
    {
        public Sprite[] sheetingSpriteStart;
        public Sprite[] sheetingSprite;
        public Sprite[] sheetingSpriteEnd;

        public ArrayList targetArray;

        public IsRight rightFunctionPointer;

        public float sheetingFrame; // ms
        public float appreanceFrame; // ms

        public float stretchYMin; // ms
        public float stretchYMax; // ms

        public float scale;

        public float floatingTime;

        public GameObject referencePosition;
    }

    public class MagitionSkillModel
    {
        public Sprite[] magicCircleSprite;
        public Sprite[] devilHandsSprite;

        public float circleRenderFrame;

        public ArrayList targetArray;

        public bool isRight;

        public Vector2 startPostion;
        public Vector2 endPostion;

        public float handsDegree;

        public int handsCount;
        public float offsetRandomDistMax;
        public float offsetRandomDegMax;

        public Vector2 firstArea;
        public Vector2 lastArea;
    }
}