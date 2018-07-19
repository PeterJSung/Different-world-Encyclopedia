namespace DefinitionChar
{
    public static class CaracterInfo
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
            ATTACK = 8, // 공격
            MOVE = 16, //이동
            DASH_MOVE = 32, // 대시
            SKILL = 64 //스킬
        }


    }
}
