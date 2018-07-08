using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    GROUND,//땅
    FLOAT_GROUND,
    WALL,
};

public interface ITile {
    TileType tileType { get; set; }
    float harzardValue { get; set; }
}
