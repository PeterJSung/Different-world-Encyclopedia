using System.Collections;
using UnityEngine;

namespace DragonActionModel
{
    public sealed class DragonSkillModel
    {
        public Sprite[] sheetingSpriteStart;
        public Sprite[] sheetingSprite;

        public ArrayList targetArray;

        public bool isRight;

        public float sheetingFrame; // ms
        public float appreanceFrame; // ms

        public float gapFrame; // ms

        public float stretchYMin; // ms
        public float stretchYMax; // ms
    }
}