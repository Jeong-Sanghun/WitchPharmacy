using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Coffee.UIExtensions;

public class AirTool : MeasureTool
{
    [SerializeField]
    Text symptomTextDumy;
    [SerializeField]
    UIEffect textPixelEffect;
    [SerializeField]
    RectTransform handleRect;
    const float radius = 150;
    Vector2 startPos = new Vector2(-150, 0);
    Vector2 targetPos;
    Vector3 screenPos;
    bool endCoroutineRunning = false;
    bool touched = false;
    bool touchedDuringCoroutine = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        screenPos = new Vector3(Screen.width / 2, Screen.height / 2,0);
        base.Start();
        targetPos = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * radius;
        ExplainLoad();
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void HandleDrag()
    {
        if(!isAuto && !measureEnd && measureStarted)
        {
            Vector2 mousePos = Input.mousePosition - screenPos;
            Vector2 handlePos = mousePos.normalized * radius;
            handleRect.anchoredPosition = handlePos;
            textPixelEffect.effectFactor = 0.8f + 0.2f * ((targetPos - handlePos).magnitude) / (radius * 2);
            touched = true;
            if (endCoroutineRunning == true)
            {
                touchedDuringCoroutine = true;
            }
        }

    }

    public void HandleCheck()
    {
        if (!isAuto && !measureEnd && measureStarted)
        {
            touched = false;
            if (endCoroutineRunning == false)
            {
                touchedDuringCoroutine = false;
            }
            if (textPixelEffect.effectFactor < 0.85f)
            {
                StartCoroutine(EndCoroutine());
            }
        }
        
    }

    IEnumerator EndCoroutine()
    {
        if(endCoroutineRunning == false)
        {
            endCoroutineRunning = true;
            yield return new WaitForSeconds(1f);
            if (!touched && !touchedDuringCoroutine)
            {
                if (textPixelEffect.effectFactor < 0.85f)
                {
                    textPixelEffect.effectFactor = 0;
                    MeasureEnd();
                }

            }
            endCoroutineRunning = false;
        }
        
    }

    public override void ToolActive(bool active)
    {
        base.ToolActive(active);
        measureStarted = true;
        if (isAuto == true && measureEnd == false)
        {

        }
        //StartCoroutine(SceneManager.inst.MoveModule_Linear(toolObject, Vector3.zero, 2));
    }

    public override void OnNewVisitor(int symptomNum, int index, bool auto)
    {
        base.OnNewVisitor(symptomNum, index, auto);
        symptomTextDumy.text = symptomNumber.ToString();
        if (!isAuto)
        {
            float targetX = Random.Range(-1f, 1f);
            float targetY = Random.Range(-1f, 1f);
            float handleX = targetX * (-1) + Random.Range(-0.4f,0.4f);
            float handleY = targetY * (-1) + Random.Range(-0.4f, 0.4f);
            targetPos = (new Vector2(targetX, targetY)).normalized * radius;
            handleRect.anchoredPosition = (new Vector2(handleX,handleY )).normalized * radius;
            textPixelEffect.effectFactor = 0.8f + 0.2f * ((targetPos - handleRect.anchoredPosition).magnitude) / (radius * 2);
        }
        else
        {
            Debug.Log("오토");
            targetPos = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * radius;
            handleRect.anchoredPosition = targetPos;

            MeasureEnd();
        }
    }
    protected override void ExplainLoad()
    {
        base.ExplainLoad();
        explainData = gameManager.jsonManager.ResourceDataLoad<MeasureToolExplain>("MeasureToolExplain/Air");
        ExplainSet();
    }

    protected override void MeasureEnd()
    {
        base.MeasureEnd();
        symptomTextDumy.fontSize = 150;
        textPixelEffect.effectFactor = 0;
        symptomTextDumy.gameObject.SetActive(true);
    }
}
