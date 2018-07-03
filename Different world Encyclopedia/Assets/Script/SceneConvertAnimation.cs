using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneConvertAnimation : MonoBehaviour
{
    public float animTime = 2f;

    public Color fadeInColor = new Color(1, 1, 1);
    public Color fadeOutColor = new Color(1, 1, 1);
    public bool isFadeIn = true;

    private Image animObject;

    private float start = 0f;
    private float end = 0f;
    private float time = 0f;

    private bool isPlaying = false;
    public delegate void ExcuteNextScene();
    //없다가 생기면   fade In
    //있다가 없어지면 fade Out


    //이미지 기준이 아닌 화면기준으로생각
    //화면이 생기려면   이미지가없어져야함 Fade In 이미지는 불투명-> 투명
    //화면이 없어질려면 이미지가 생겨야함 Fade Out 이미지는 투명->불투명
    void Awake()
    {
        animObject = GetComponent<Image>();

        if (isFadeIn)
        {
            StartConvertAnimation(true);
        }
        else
        {
            animObject.color = new Color(1, 1, 1, 0);
        }
    }

    public void StartConvertAnimation(bool isFadeIn, ExcuteNextScene callbackFunction = null)
    {
        if (isPlaying == true)
        {
            return;
        }
        StartCoroutine(FadeAnimation(isFadeIn, callbackFunction));
    }


    //Color.a = 1 불투명
    //Color.a = 0 투명
    IEnumerator FadeAnimation(bool isFadeIn, ExcuteNextScene callbackFunction)
    {
        isPlaying = true;

        Color color;
        if (isFadeIn)
        {
            color = fadeInColor;
            start = 1f;
            end = 0f;
        }
        else
        {
            color = fadeOutColor;
            start = 0f;
            end = 1f;
        }
       
        
        time = 0f;
        color.a = Mathf.Lerp(start, end, time);

        while (isFadeIn? color.a > 0f : color.a < 1f)
        {
            time += Time.deltaTime / animTime;

            color.a = Mathf.Lerp(start, end, time);
            animObject.color = color;
            yield return null;
        }
        isPlaying = false;
        if(callbackFunction != null)
        {
            callbackFunction();
        }
    }
}
