using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour
{
    public Image convertImage;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (convertImage)
            {
                convertImage.GetComponent<SceneConvertAnimation>().StartConvertAnimation(false, NextScene);
            }
        }
    }

    void NextScene()
    {
        SceneManager.LoadScene("Stage0");
    }
}
