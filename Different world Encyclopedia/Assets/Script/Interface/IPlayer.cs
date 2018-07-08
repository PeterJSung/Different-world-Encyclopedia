using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CharatorType
{
    ALLIGATOR, //악어야
    MAGITION, // 마법사
    DRAON, // 드래곤
}

public abstract IPlayer
{
    CharatorType PlayerType = 0;
    float MOVE_WEIGHT { get; set; }
    float DASH_MOVE_WEIGHT { get; set; }
    float JUMP_FORCE { get; set; }
}
