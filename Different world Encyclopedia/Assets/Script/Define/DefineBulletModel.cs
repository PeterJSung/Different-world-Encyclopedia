using System.Collections;
using UnityEngine;

namespace DefineBulletModel
{
    //Bullet 객체 방향
    public enum BULLET_DIRECTION
    {
        NONE, // 방향없음.
        LEFT, //왼쪽
        TOP, //위쪽
        RIGHT, //오른쪽
        BOT // 아래쪽
    };

    //Bullet 객체 움직입 타입
    public enum MOTION_TYPE
    {
        NONE, // 정의되지 않음.
        FLOAT, // 떠있음.
        STRAIGHT // 직선으로 던짐
    };

    //관통력 구현.
    public class PenetrateData
    {
        public static int INFINITE_PENETRATE = 10000;
        public int penetrateCount = 0;
        public float maxSheetingArea = 0;

        public PenetrateData(int argPenetrateCount, float argMaxSheetingArea)
        {
            penetrateCount = argPenetrateCount;
            maxSheetingArea = argMaxSheetingArea;
        }
    }

    public abstract class BulletData
    {
        public MOTION_TYPE motion = MOTION_TYPE.NONE;
        public Sprite[] sheetingsprite = null;
        public Vector3 startPosition;
        public float disapearTiming;
        public ArrayList tLayer = null;
        public IsRight rightCheckFunction;

        //For Interface
        //Float
        public bool GetIsRight() { return rightCheckFunction == null ? false : rightCheckFunction(); }

        public virtual GameObject GetStandardPosition() { return null; }
        public virtual float GetFloattingTimimg() { return 0; }

        //Straight
        public virtual BULLET_DIRECTION GetBulletDirection() { return BULLET_DIRECTION.NONE; }
        public virtual float GetSheetingLength(){ return 0.0f; }
        public virtual Vector2 GetShootingForce() { return new Vector2(); }
        
        public virtual bool CanPenetrate() { return true; }
        public virtual void DoPenetrate() {  }

        public virtual Sprite[] GetStartSprite() { return null; }
        public virtual Sprite[] GetSheetingSprite() { return sheetingsprite; }
        public virtual Sprite[] GetEndSprite() { return null;  }

    }

    public class BulletDataFloatType : BulletData
    {
        public Sprite[] startprite = null;
        public float floatTiming;
        public Sprite[] endSprite = null;
        public GameObject standardPosition = null;
        public override float GetFloattingTimimg()
        {
            return floatTiming;
        }

        public override Sprite[] GetStartSprite()
        {
            return startprite;
        }

        public override Sprite[] GetEndSprite()
        {
            return endSprite;
        }

        public override GameObject GetStandardPosition()
        {
            return standardPosition;
        }
    }

    public class BulletDataStrightType : BulletData
    {
        public BULLET_DIRECTION dir = BULLET_DIRECTION.NONE;

        public float sheetingLength;

        public Vector2 shootingForce;
        
        public int penetrateCount;

        public Sprite[] endSprite = null;

        public override BULLET_DIRECTION GetBulletDirection()
        {
            return dir;
        }

        public override float GetSheetingLength()
        {
            return sheetingLength;
        }

        public override Vector2 GetShootingForce()
        {
            return shootingForce;
        }

        public override Sprite[] GetEndSprite()
        {
            return endSprite;
        }

        public override bool CanPenetrate()
        {
            return penetrateCount > 0;
        }

        public override void DoPenetrate()
        {
            penetrateCount--;
        }
    }
}
