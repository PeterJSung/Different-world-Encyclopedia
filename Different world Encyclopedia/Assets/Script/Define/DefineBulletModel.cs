using System.Collections;
using UnityEngine;

namespace DefineBulletModel
{
    public static class BulletModel
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
            NONE, // 움직이지 않음.
            STRAIGHT, // 직선으로 던짐
            CURVE // 포물선
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

        public class BulletData
        {
            public BULLET_DIRECTION dir;
            public MOTION_TYPE motion;
            public Sprite[] sheetingsprite;
            public Sprite[] endSprite;
            public PenetrateData penetrate;
            public ArrayList tLayer;
            public float disapearTiming;
            public float sheetingLength;
            public Vector3 weight;
            //x = xmove 
            //y = ymove 
            //z = curveWeight
        }
    }

}
