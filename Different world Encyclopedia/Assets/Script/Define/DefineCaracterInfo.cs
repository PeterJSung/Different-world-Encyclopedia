using System.Collections.Generic;
using UnityEngine;

namespace DefinitionChar
{
    public class PlayerDefaultBulletData
    {
        public Vector2 m_v2BulletSize;
        //need Extra Data
    }

    public struct PlayerWeaponData
    {
        public float m_fWeaponAxisStart;
        public float m_fWeaponAxisEnd;

        public Vector2 m_v2WeaponColliderArea;
        public Vector2 m_v2Weaponoffset;

        public Vector2 m_v2WeaponPosition;
        public Vector2 m_v2WeaponAxisPosition;

        public Vector3 m_v3EffectPosition;
        public Vector3 m_v3EffectScale;

        public float m_fAttackSpeed;

        public bool m_bIsEnableWeaponHit;

        public string m_sWaeponSpritePath;
        public string m_sWaeponEffectPath;
    }

    public struct PlayerMoveData
    {
        public Vector2 m_v2CharacterColliderArea;

        public float m_fDashMoveWeight;
        public float m_fMoveWeight;

        public float m_fJumpForce;
        public int m_iJumpCount;
        public bool m_bIsBlink;

        public bool m_bIsEnableRavitate;

        public string m_sPlayerSpritePath;
    }

    public class CharaterInfo
    {
        //for weapon
        public PlayerWeaponData m_sPlaerWeapon;
        //for PlayerMove
        public PlayerMoveData m_sPlayerMove;
        //for default attack bullet
        public PlayerDefaultBulletData m_sPlayerDefaultBullet;

        public CharaterInfo()
        {
            m_sPlaerWeapon = new PlayerWeaponData();
            m_sPlayerMove = new PlayerMoveData();
            m_sPlayerDefaultBullet = new PlayerDefaultBulletData();
        }
    }

    public static class CustomCharacterInfo
    {
        public enum CHAR_TYPE
        {
            ALLIGATOR, //악어야
            MAGITION, // 마법사
            DRAGON, // 드래곤
            HERO //용사
        }

        //캐릭터는 7 Status 를 가지고 있음.
        //1. 사망 1순위
        //2. 피격 2순위
        //3. 점프 3순위
        //4. 공격 3순위
        //5. 이동 3순위
        //6. 대시 3순위
        //7. 스킬 3순위
        public enum CHAR_STATUS
        {
            NULL = 0, // 아무것도 안함.
            DEAD = 1, //피격
            HIT = 2, //사망
            JUMP = 4, //점프
            RAVITATE = 8, //Ravitate 마법사전용
            ATTACK = 16, // 공격
            MOVE = 32, //이동
            DASH_MOVE = 64, // 대시
            SKILL = 128 //스킬
        }

        public static Dictionary<CHAR_TYPE, CharaterInfo> CHAR_GLOBAL_DEFAULT_DATA;

        static CustomCharacterInfo()
        {
            CHAR_GLOBAL_DEFAULT_DATA = new Dictionary<CHAR_TYPE, CharaterInfo>();
            CHAR_GLOBAL_DEFAULT_DATA.Add(CHAR_TYPE.ALLIGATOR, generateDefaultCharInfo(CHAR_TYPE.ALLIGATOR));
            CHAR_GLOBAL_DEFAULT_DATA.Add(CHAR_TYPE.MAGITION, generateDefaultCharInfo(CHAR_TYPE.MAGITION));
            CHAR_GLOBAL_DEFAULT_DATA.Add(CHAR_TYPE.DRAGON, generateDefaultCharInfo(CHAR_TYPE.DRAGON));
            CHAR_GLOBAL_DEFAULT_DATA.Add(CHAR_TYPE.HERO, generateDefaultCharInfo(CHAR_TYPE.HERO));
        }

        private static CharaterInfo generateDefaultCharInfo(CHAR_TYPE arg)
        {
            CharaterInfo ret = new CharaterInfo();
            switch (arg)
            {
                case CHAR_TYPE.ALLIGATOR:
                    ret.m_sPlaerWeapon.m_fWeaponAxisStart = 30.0f;
                    ret.m_sPlaerWeapon.m_fWeaponAxisEnd = 130.0f;

                    ret.m_sPlaerWeapon.m_v2WeaponColliderArea = new Vector2(0.12f, 0.4f);
                    ret.m_sPlaerWeapon.m_v2Weaponoffset = new Vector2(0.0f, 0.0f);

                    ret.m_sPlaerWeapon.m_v3EffectPosition = new Vector3(0.4f, 0.08f, 0.0f);
                    ret.m_sPlaerWeapon.m_v3EffectScale = new Vector3(1.0f, 1.9f, 1.0f);

                    ret.m_sPlaerWeapon.m_v2WeaponPosition = new Vector2(-0.015f, 0.18f);
                    ret.m_sPlaerWeapon.m_v2WeaponAxisPosition = new Vector2(-0.015f, -0.015f);

                    ret.m_sPlaerWeapon.m_fAttackSpeed = 0.40f;
                    ret.m_sPlaerWeapon.m_bIsEnableWeaponHit = true;
                    ret.m_sPlaerWeapon.m_sWaeponSpritePath = "Weapon/Alligator/Merona";
                    ret.m_sPlaerWeapon.m_sWaeponEffectPath = "Weapon/Common/Swing";

                    ret.m_sPlayerMove.m_fJumpForce = 400.0f;
                    ret.m_sPlayerMove.m_v2CharacterColliderArea = new Vector2(0.45f, 0.58f);
                    ret.m_sPlayerMove.m_iJumpCount = 2;
                    ret.m_sPlayerMove.m_bIsBlink = false;
                    ret.m_sPlayerMove.m_bIsEnableRavitate = false;
                    ret.m_sPlayerMove.m_sPlayerSpritePath = "Anim/Player/dkrdjdiH_0";

                    ret.m_sPlayerDefaultBullet.m_v2BulletSize = new Vector2(0.2f, 0.2f);
                    break;
                case CHAR_TYPE.MAGITION:
                    ret.m_sPlaerWeapon.m_fWeaponAxisStart = 0.0f;
                    ret.m_sPlaerWeapon.m_fWeaponAxisEnd = 45.0f;

                    ret.m_sPlaerWeapon.m_v2WeaponColliderArea = new Vector2(0.0f, 0.0f);
                    ret.m_sPlaerWeapon.m_v2Weaponoffset = new Vector2(0.0f, 0.0f);

                    ret.m_sPlaerWeapon.m_v3EffectPosition = new Vector3(0.1f, 0.0f, 0.0f);
                    ret.m_sPlaerWeapon.m_v3EffectScale = new Vector3(0.5f, 1.0f, 1.0f);

                    ret.m_sPlaerWeapon.m_v2WeaponPosition = new Vector2(0.03f, 0.0f);
                    ret.m_sPlaerWeapon.m_v2WeaponAxisPosition = new Vector2(-0.015f, -0.07f);

                    ret.m_sPlaerWeapon.m_fAttackSpeed = 1.2f;
                    ret.m_sPlaerWeapon.m_bIsEnableWeaponHit = false;
                    ret.m_sPlaerWeapon.m_sWaeponSpritePath = "Weapon/Magition/Stick";
                    ret.m_sPlaerWeapon.m_sWaeponEffectPath = "Weapon/Common/MagicFildRed";

                    ret.m_sPlayerMove.m_fJumpForce = 300.0f;
                    ret.m_sPlayerMove.m_v2CharacterColliderArea = new Vector2(0.45f, 0.70f);
                    ret.m_sPlayerMove.m_iJumpCount = 1;
                    ret.m_sPlayerMove.m_bIsBlink = false;
                    ret.m_sPlayerMove.m_bIsEnableRavitate = true;
                    ret.m_sPlayerMove.m_sPlayerSpritePath = "Anim/Player/wichH_0";

                    ret.m_sPlayerDefaultBullet.m_v2BulletSize = new Vector2(0.75f, 0.4f);
                    break;
                case CHAR_TYPE.DRAGON:
                    ret.m_sPlaerWeapon.m_fWeaponAxisStart = 10.0f;
                    ret.m_sPlaerWeapon.m_fWeaponAxisEnd = 120.0f;

                    ret.m_sPlaerWeapon.m_v2WeaponColliderArea = new Vector2(0.35f, 0.55f);
                    ret.m_sPlaerWeapon.m_v2Weaponoffset = new Vector2(0.0f, 0.05f);

                    ret.m_sPlaerWeapon.m_v3EffectPosition = new Vector3(0.5f, 0.08f, 0.0f);
                    ret.m_sPlaerWeapon.m_v3EffectScale = new Vector3(1.5f, 2.0f, 1.0f);

                    ret.m_sPlaerWeapon.m_v2WeaponPosition = new Vector2(0.02f, 0.15f);
                    ret.m_sPlaerWeapon.m_v2WeaponAxisPosition = new Vector2(-0.03f, -0.07f);

                    ret.m_sPlaerWeapon.m_fAttackSpeed = 0.6f;
                    ret.m_sPlaerWeapon.m_bIsEnableWeaponHit = true;
                    ret.m_sPlaerWeapon.m_sWaeponSpritePath = "Weapon/Dragon/Axe";
                    ret.m_sPlaerWeapon.m_sWaeponEffectPath = "Weapon/Common/Swing";

                    ret.m_sPlayerMove.m_fJumpForce = 400.0f;
                    ret.m_sPlayerMove.m_v2CharacterColliderArea = new Vector2(0.45f, 0.70f);
                    ret.m_sPlayerMove.m_iJumpCount = 1;
                    ret.m_sPlayerMove.m_bIsBlink = true;
                    ret.m_sPlayerMove.m_bIsEnableRavitate = false;
                    ret.m_sPlayerMove.m_sPlayerSpritePath = "Anim/Player/DragonH_0";
                    break;
                case CHAR_TYPE.HERO:
                    ret.m_sPlaerWeapon.m_fWeaponAxisStart = 20.0f;
                    ret.m_sPlaerWeapon.m_fWeaponAxisEnd = 130.0f;

                    ret.m_sPlaerWeapon.m_v2WeaponColliderArea = new Vector2(0.1f, 0.45f);
                    ret.m_sPlaerWeapon.m_v2Weaponoffset = new Vector2(0.0f, 0.05f);

                    ret.m_sPlaerWeapon.m_v3EffectPosition = new Vector3(0.5f, 0.08f, 0.0f);
                    ret.m_sPlaerWeapon.m_v3EffectScale = new Vector3(0.75f, 2.2f, 1.0f);

                    ret.m_sPlaerWeapon.m_v2WeaponPosition = new Vector2(0.0f, 0.25f);
                    ret.m_sPlaerWeapon.m_v2WeaponAxisPosition = new Vector2(-0.015f, -0.11f);

                    ret.m_sPlaerWeapon.m_fAttackSpeed = 0.6f;
                    ret.m_sPlaerWeapon.m_bIsEnableWeaponHit = true;
                    ret.m_sPlaerWeapon.m_sWaeponSpritePath = "Weapon/Hero/sword";
                    ret.m_sPlaerWeapon.m_sWaeponEffectPath = "Weapon/Common/Swing";

                    ret.m_sPlayerMove.m_fJumpForce = 400.0f;
                    ret.m_sPlayerMove.m_v2CharacterColliderArea = new Vector2(0.45f, 0.75f);
                    ret.m_sPlayerMove.m_iJumpCount = 1;
                    ret.m_sPlayerMove.m_bIsBlink = false;
                    ret.m_sPlayerMove.m_bIsEnableRavitate = false;
                    ret.m_sPlayerMove.m_sPlayerSpritePath = "Anim/Player/HeroH_0";
                    break;
            }
            ret.m_sPlayerMove.m_fDashMoveWeight = 0.06f;
            ret.m_sPlayerMove.m_fMoveWeight = 0.03f;
            
            return ret;
        }

    }
}
