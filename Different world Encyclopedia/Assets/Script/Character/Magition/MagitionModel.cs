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

    }
}