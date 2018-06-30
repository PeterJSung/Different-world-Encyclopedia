using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCubeScript : MonoBehaviour {

    Vector3Int rotateVec;
	// Use this for initialization
	void Start () {
        rotateVec.x = rotateVec.y = rotateVec.z = 0;
	}
	
	// Update is called once per frame
	void Update () {
        rotateVec.x++;
        rotateVec.y++;
        rotateVec.z++;
        transform.rotation = Quaternion.Euler(rotateVec.x, rotateVec.y, rotateVec.z);
    }
}
