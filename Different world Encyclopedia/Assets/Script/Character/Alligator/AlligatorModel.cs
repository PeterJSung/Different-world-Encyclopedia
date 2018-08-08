using System.Collections;
using UnityEngine;

namespace AlligatorActionModel {
    public sealed class AlligatorAttackModel
    {
        public Sprite[] sheetingSprite;
        public Sprite[] destroySprite;

        public ArrayList targetArray;

        public bool isRight;
        public float bulletSpeed;
        public float sheetingArea;
        public float frame; // ms
        public int endFrameMultiple; // ms
        public float scale;
    };

    public sealed class AlligatorSkillModel
    {
        enum SummonType
        {
            CANDLE,
            SWORD,
            WALL
        }
    };
};