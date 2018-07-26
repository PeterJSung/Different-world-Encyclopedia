using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BossHpBar : MonoBehaviour {
    public GameObject BOSSHPBAR;
    public float FULL_HP_VALUE = 0;
    public float CURRENT_HP_VALUE = 0;
    public float TARGET_HP_VALUE = 0;
    //public float CURRENT_HP_PERCENTAGE = 1;
    public bool IS_GAGE_ANIMATION_ON = false;
    public bool IS_DEMAGE = false;

    // Use this for initialization
    void Start () {

        //test code 잘돼나ㅣ~?
        initHPValue(1000, 1000);

        //updateView(calculPercentage(-200));
        updateGage(-400);
    }

    // Update is called once per frame
    void Update()
    {
        gageController();
        
    }

    //보스체력바 초기화
    //fullHP의 값은 해당 몬스터가 가진 총 체력량임
    //일반적으로 처음엔 풀게이지이므로 fullHP, currentHP가 동일한 값이면 됨
    //처음시작할때부터 어느정도 HP가 떨어진 상태를 표현하고 싶다면 두번째 파라미터 값을 조절할것
    public void initHPValue(float fullHP, float currentHP)
    {
        FULL_HP_VALUE = fullHP;
        CURRENT_HP_VALUE = currentHP;
        //CURRENT_HP_PERCENTAGE = currentHP / fullHP;

        this.BOSSHPBAR.GetComponent<Image>().fillAmount = CURRENT_HP_VALUE / FULL_HP_VALUE;
    }

    //게이지바가 가고자하는 체력 목표값을 설정하고
    //파라미터에 입력된 값이 총 몇퍼센트의 비중을 차지하는지 계산
    public void updateGage(float val)
    {
        TARGET_HP_VALUE = CURRENT_HP_VALUE + val;
        if (val > 0)
            IS_DEMAGE = false;
        else
            IS_DEMAGE = true;

        IS_GAGE_ANIMATION_ON = true;

        //return val / FULL_HP_VALUE;
    }

    public void gageController()
    {
        if (IS_GAGE_ANIMATION_ON)
        {
            if(IS_DEMAGE)//데미지일 경우
            {
                if (TARGET_HP_VALUE / FULL_HP_VALUE <= this.BOSSHPBAR.GetComponent<Image>().fillAmount)
                    this.BOSSHPBAR.GetComponent<Image>().fillAmount -= 0.01f;
                else
                    IS_GAGE_ANIMATION_ON = false;
            }
            else  //힐일 경우
            {
                if(TARGET_HP_VALUE / FULL_HP_VALUE >= this.BOSSHPBAR.GetComponent<Image>().fillAmount)
                   this.BOSSHPBAR.GetComponent<Image>().fillAmount += 0.01f;
                else
                    IS_GAGE_ANIMATION_ON = false;
            }
            
        }
    }

    


}
