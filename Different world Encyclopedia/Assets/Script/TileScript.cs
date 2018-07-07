using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour, ITile {
    public TileType SelectTileType;
    public float SelectHarzardValue;

    TileType ITile.tileType
    {
        get {
            return SelectTileType;
        }
        set {
            SelectTileType = value;
        }
    }
    float ITile.harzardValue {
        get
        {
            return SelectHarzardValue;
        }
        set
        {
            SelectHarzardValue = value;
        }
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
