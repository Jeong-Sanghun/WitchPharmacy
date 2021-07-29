using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterTool : MeasureTool
{

    [SerializeField]
    Text symptomTextDumy;
    [SerializeField]
    RectTransform barRect;
    float nowGauge;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

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
               
                barRect.sizeDelta = new Vector2(nowGauge, barRect.sizeDelta.y);
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
        StartCoroutine(SceneManager.inst.MoveModule_Linear(toolObject, Vector3.zero, 2));
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
            barRect.sizeDelta = new Vector2(nowGauge, barRect.sizeDelta.y);
        }
    }


    public override void OnNewVisitor(int symptomNum, int index, bool auto)
    {
        base.OnNewVisitor(symptomNum, index, auto);

        if (!isAuto)
        {
            nowGauge = 0;
            barRect.sizeDelta = new Vector2(nowGauge, barRect.sizeDelta.y);
            symptomTextDumy.gameObject.SetActive(false);
        }
        else
        {
            nowGauge = 1000;
            barRect.sizeDelta = new Vector2(nowGauge, barRect.sizeDelta.y);
            MeasureEnd();
        }

    }

    protected override void MeasureEnd()
    {
        base.MeasureEnd();
        symptomTextDumy.gameObject.SetActive(true);
        symptomTextDumy.text = symptomNumber.ToString();
    }
}
