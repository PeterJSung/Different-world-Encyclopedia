using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHpBar : MonoBehaviour {
    public GameObject BOSSHPBAR;
    public float FULL_HP_VALUE = 0;
    public float CURRENT_HP_VALUE = 0;

    // Use this for initialization
    void Start () {

        //test code 잘돼나ㅣ~?
        initHPValue(1000, 500);

        updateView(calculPercentage(-200));
    }

    // Update is called once per frame
    void Update()
    {
        //DecreaseHp();
        
    }

    //보스체력바 초기화
    //fullHP의 값은 해당 몬스터가 가진 총 체력량임
    //일반적으로 처음엔 풀게이지이므로 fullHP, currentHP가 동일한 값이면 됨
    //처음시작할때부터 어느정도 HP가 떨어진 상태를 표현하고 싶다면 두번째 파라미터 값을 조절할것
    public void initHPValue(float fullHP, float currentHP)
    {
        FULL_HP_VALUE = fullHP;
        CURRENT_HP_VALUE = currentHP;

        this.BOSSHPBAR.GetComponent<Image>().fillAmount = CURRENT_HP_VALUE / FULL_HP_VALUE;
    }

    //파라미터에 입력된 값이 총 몇퍼센트의 비중을 차지하는지 계산
    public float calculPercentage(float val)
    {
        //float tmp_val = val;
        //if (val < 0)
        //    tmp_val *= -1;

        return val / FULL_HP_VALUE;
    }

    public void updateView(float percentage)
    {
        this.BOSSHPBAR.GetComponent<Image>().fillAmount += percentage;
    }

    public void DecreaseHp()
    {
        if (BOSSHPBAR)
        {
            this.BOSSHPBAR.GetComponent<Image>().fillAmount -= 0.001f;
        }
    }


    public void setFullHP()
    {

    }


}
