using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefineDefaultAttack
{
    public sealed class DefaultAttackData
    {
        public float m_fAttackSpeed;
        public float m_fWeaponAxisStart;
        public float m_fWeaponAxisEnd;

        public ArrayList hitList;

        public IsRight rightFunctionPointer;
        public DefaultAttackData(float _attackSpeed, float _weaponAxisStart, float _weaponAxisEnd, IsRight _rightFunctionPointer, ArrayList _hitList)
        {
            m_fAttackSpeed = _attackSpeed;
            m_fWeaponAxisStart = _weaponAxisStart;
            m_fWeaponAxisEnd = _weaponAxisEnd;
            hitList = _hitList;

            rightFunctionPointer = _rightFunctionPointer;
        }
    }

    public sealed class ExtraAttackData
    {
        public Vector2 m_v2BulletSize;

        public ExtraAttackData(Vector2 _argBulletSize)
        {
            m_v2BulletSize = _argBulletSize;
        }
    }
}