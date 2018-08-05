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
        public Vector2 sparkArea;
        public Vector2 laserArea;
        public Vector2 offset;
    }

    public struct Magition
    {
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
                    ret.m_sSkillBullet.magitionSkill.firstArea = new Vector2(0.0f, 1.0f);
                    ret.m_sSkillBullet.magitionSkill.lastArea = new Vector2(15.0f, 1.0f);
                    break;
                case GlobalCharacterInfo.CHAR_TYPE.DRAGON:
                    ret.m_sSkillBullet.dragonSkill.sparkArea = new Vector2(10.0f, 0.2f);
                    ret.m_sSkillBullet.dragonSkill.laserArea = new Vector2(10.0f, 0.8f);
                    ret.m_sSkillBullet.dragonSkill.offset = new Vector2(ret.m_sSkillBullet.dragonSkill.sparkArea.x / 2, 0.0f);
                    ret.m_sSkillBullet.dragonSkill.eachDuration = 0.3f;
                    ret.m_sSkillBullet.dragonSkill.gapDuration = 0.1f;
                    break;
                case GlobalCharacterInfo.CHAR_TYPE.HERO:
                    // Skill 이 없으므로 해당 데이터 설정하지 않는다.
                    break;
            }
            
            return ret;
        }
    }
}


