using System.Collections;
using System.Collections.Generic;
using DefinitionChar;
using UnityEngine;

namespace DefineSkill
{
    public struct SkillSummonData
    {

    }

    public struct DragonSkill
    {
        public float eachDuration;
        public float gapDuration;
        public float sparkYArea;
        public float laserYArea;
        //public Vector2 offset;
    }

    public struct Magition
    {
        public Vector2 startPostion;
        public Vector2 endPostion;

        public float handsDegree;

        public int handsCount;
        public float offsetRandomDistMax;
        public float offsetRandomDegMax;

        public Vector2 firstArea;
        public Vector2 lastArea;
    }

    public class SkillBulletData
    {
        public DragonSkill dragonSkill;
        public Magition magitionSkill;

        public SkillBulletData()
        {
            dragonSkill = new DragonSkill();
            magitionSkill = new Magition();
        }
    }

    public class SkillInfo
    {
        //for SummonSkill
        public SkillSummonData m_sSkillSummon;
        //for BulletSkill
        public SkillBulletData m_sSkillBullet;

        public SkillInfo()
        {
            m_sSkillSummon = new SkillSummonData();
            m_sSkillBullet = new SkillBulletData();
        }
    }

    public static class GlobalSkillInfo
    {

        public static Dictionary<GlobalCharacterInfo.CHAR_TYPE, SkillInfo> SKILL_GLOBAL_DEFAULT_DATA;
        static GlobalSkillInfo()
        {
            SKILL_GLOBAL_DEFAULT_DATA = new Dictionary<GlobalCharacterInfo.CHAR_TYPE, SkillInfo>();
            SKILL_GLOBAL_DEFAULT_DATA.Add(GlobalCharacterInfo.CHAR_TYPE.ALLIGATOR, generateDefaultSkillInfo(GlobalCharacterInfo.CHAR_TYPE.ALLIGATOR));
            SKILL_GLOBAL_DEFAULT_DATA.Add(GlobalCharacterInfo.CHAR_TYPE.MAGITION, generateDefaultSkillInfo(GlobalCharacterInfo.CHAR_TYPE.MAGITION));
            SKILL_GLOBAL_DEFAULT_DATA.Add(GlobalCharacterInfo.CHAR_TYPE.DRAGON, generateDefaultSkillInfo(GlobalCharacterInfo.CHAR_TYPE.DRAGON));
            SKILL_GLOBAL_DEFAULT_DATA.Add(GlobalCharacterInfo.CHAR_TYPE.HERO, generateDefaultSkillInfo(GlobalCharacterInfo.CHAR_TYPE.HERO));
        }


        private static SkillInfo generateDefaultSkillInfo(GlobalCharacterInfo.CHAR_TYPE arg)
        {
            SkillInfo ret = new SkillInfo();
            switch (arg)
            {
                case GlobalCharacterInfo.CHAR_TYPE.ALLIGATOR:
                    // Summon Data 만 설정한다.
                    break;
                case GlobalCharacterInfo.CHAR_TYPE.MAGITION:
                    ret.m_sSkillBullet.magitionSkill.startPostion = new Vector2(-3.0f,5.0f);
                    ret.m_sSkillBullet.magitionSkill.endPostion = new Vector2(-1.0f, 8.0f);

                    ret.m_sSkillBullet.magitionSkill.handsDegree = 125.0f;

                    ret.m_sSkillBullet.magitionSkill.handsCount = 10;
                    ret.m_sSkillBullet.magitionSkill.offsetRandomDistMax = 1;
                    ret.m_sSkillBullet.magitionSkill.offsetRandomDegMax = 10.0f;


                    ret.m_sSkillBullet.magitionSkill.firstArea = new Vector2(0.0f, 1.0f);
                    ret.m_sSkillBullet.magitionSkill.lastArea = new Vector2(15.0f, 1.0f);
                    break;
                case GlobalCharacterInfo.CHAR_TYPE.DRAGON:
                    ret.m_sSkillBullet.dragonSkill.sparkYArea = 0.1f;
                    ret.m_sSkillBullet.dragonSkill.laserYArea = 1.0f;
                    //ret.m_sSkillBullet.dragonSkill.offset = new Vector2(ret.m_sSkillBullet.dragonSkill.sparkArea.x / 2, 0.0f);
                    ret.m_sSkillBullet.dragonSkill.eachDuration = 100.0f;
                    ret.m_sSkillBullet.dragonSkill.gapDuration = 200.0f;
                    break;
                case GlobalCharacterInfo.CHAR_TYPE.HERO:
                    // Skill 이 없으므로 해당 데이터 설정하지 않는다.
                    break;
            }

            return ret;
        }
    }
}


