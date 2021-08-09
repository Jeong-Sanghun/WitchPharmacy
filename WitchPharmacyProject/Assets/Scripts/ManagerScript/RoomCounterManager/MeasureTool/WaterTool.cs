using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterTool : MeasureTool
{
    [SerializeField]
    GameObject movingTool;
    [SerializeField]
    Text symptomTextDumy;
    [SerializeField]
    GameObject symptomObj;
    //[SerializeField]
    //RectTransform barRect;
    [SerializeField]
    Image barImage;
    float nowGauge;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ExplainLoad();
    }


    // Update is called once per frame
    void Update()
    {
        if(isAuto == false && measureStarted == true && measureEnd == false)
        {
            if (nowGauge >= 0)
            {
                nowGauge -= Time.deltaTime * 30;
                if (nowGauge < 0)
                {
                    nowGauge = 0;
                }
                barImage.fillAmount = nowGauge / 1000;
                //barRect.sizeDelta = new Vector2(nowGauge, barRect.sizeDelta.y);
            }
        }
    }

    public override void ToolActive(bool active)
    {
        base.ToolActive(active);
        measureStarted = true;
        if (isAuto == true && measureEnd == false)
        {
            nowGauge = 1000;
        }
        StartCoroutine(SceneManager.inst.MoveModuleRect_Linear(movingTool, Vector3.zero, 2));
    }

    public void OnTouch()
    {
        if(isAuto == false && measureEnd == false && measureStarted ==true)
        {
            nowGauge += 100f;
           
            if (nowGauge >= 1000)
            {
                nowGauge = 1000;
                MeasureEnd();
            }
            barImage.fillAmount = nowGauge / 1000;
        }
    }


    public override void OnNewVisitor(int symptomNum, int index, bool auto)
    {
        base.OnNewVisitor(symptomNum, index, auto);

        if (!isAuto)
        {
            nowGauge = 0;
            barImage.fillAmount = nowGauge / 1000;
            symptomObj.SetActive(false);
        }
        else
        {
            nowGauge = 1000;
            barImage.fillAmount = nowGauge / 1000;
            MeasureEnd();
        }

    }
    protected override void ExplainLoad()
    {
        base.ExplainLoad();
        explainData = gameManager.jsonManager.ResourceDataLoad<MeasureToolExplain>("MeasureToolExplain/Water");
        ExplainSet();
    }

    protected override void MeasureEnd()
    {
        base.MeasureEnd();
        symptomObj.SetActive(true);
        symptomTextDumy.text = symptomNumber.ToString();
    }
}
