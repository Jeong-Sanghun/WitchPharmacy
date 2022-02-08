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
    [SerializeField]
    Transform handleParentTransform;
    [SerializeField]
    GameObject movingTool;
    const float radius = 150;
    Vector2 startPos = new Vector2(-150, 0);
    Vector2 targetPos;
    Vector3 screenPos;
    Vector2 handleParentPos;
    float targetAngle;

    bool endCoroutineRunning = false;
    bool touched = false;
    bool touchedDuringCoroutine = false;
    // Start is called before the first frame update
    protected override void Start()
    {
        screenPos = new Vector3(Screen.width / 2, Screen.height / 2,0);
        base.Start();
        targetPos = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * radius;
        targetAngle = Random.Range(-90f, 180f);
        handleParentPos = handleParentTransform.GetComponent<RectTransform>().anchoredPosition;
        ExplainLoad();
        symptomTextDumy.fontSize = 150;
    }


    // Update is called once per frame
    void Update()
    {

    }

    int touchState = 0;
    float lastAngle;
    public void HandleDrag()
    {
        if(!isAuto && !measureEnd && measureStarted)
        {
            Vector2 mousePos = Input.mousePosition - screenPos - (new Vector3(400,0,0));
            Vector2 handleVector = (mousePos - handleParentPos);
            float angle = Vector2.SignedAngle(Vector2.left,handleVector);

            bool outOfBounds = false;
            if(touchState == -1)
            {
                if((angle<-90 && angle > -180) || (angle<180 && angle>0) || (angle<0 && angle>-85))
                {
                    outOfBounds = true;
                }
            }

            if(touchState == 1)
            {
                if ((angle < -90 && angle > -180) || (angle> -90 && angle < 0) || (angle < 175 && angle >0 ))
                {
                    outOfBounds = true;
                }
            }
            if (outOfBounds == true)
            {
                return;
            }

            if (outOfBounds == false)
            {
                if (angle > -90 && angle < 180)
                {
                    touchState = 0;
                }
                else if (angle < -90 && lastAngle > -90 && lastAngle < 0)
                {
                    touchState = -1;
                }
                else if (angle > -180 && lastAngle > 0 && lastAngle < 180)
                {
                    touchState = 1;
                }
            }

            if(touchState == -1)
            {
                angle = -90;
            }
            else if(touchState == 1)
            {
                angle = 180;
            }
            handleParentTransform.localRotation = Quaternion.Euler(0, 0, angle);

            //Vector2 handlePos = mousePos.normalized * radius;
            //handleRect.anchoredPosition = handlePos;
            //textPixelEffect.effectFactor = 0.8f + 0.2f * ((targetPos - handlePos).magnitude) / (radius * 2);
            textPixelEffect.effectFactor = 0.9f + 0.1f * Mathf.Abs((targetAngle - angle)) / 180;
            touched = true;
            if (endCoroutineRunning == true)
            {
                touchedDuringCoroutine = true;
            }
            lastAngle = angle;
        }

    }

    public void HandleCheck()
    {
        touchState = 0;
        if (!isAuto && !measureEnd && measureStarted)
        {
            touched = false;
            if (endCoroutineRunning == false)
            {
                touchedDuringCoroutine = false;
            }
            if (textPixelEffect.effectFactor < 0.93f)
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
                if (textPixelEffect.effectFactor < 0.93f)
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
        StartCoroutine(SceneManager.inst.MoveModuleRect_Linear(movingTool, new Vector3(400,0,0),0.5f));
    }

    public override void OnNewVisitor(int symptomNum, int index, bool auto)
    {
        base.OnNewVisitor(symptomNum, index, auto);
        symptomTextDumy.text = symptomNumber.ToString();
        if (!isAuto)
        {
            //float targetX = Random.Range(-1f, 1f);
            //float targetY = Random.Range(-1f, 1f);
            //float handleX = targetX * (-1) + Random.Range(-0.4f,0.4f);
            //float handleY = targetY * (-1) + Random.Range(-0.4f, 0.4f);
            //targetPos = (new Vector2(targetX, targetY)).normalized * radius;
            targetAngle = Random.Range(-90f, 180f);
            //handleRect.anchoredPosition = (new Vector2(handleX,handleY )).normalized * radius;
            float angle = targetAngle * (-1) + Random.Range(-20f, 20f);
            if (angle < -90 && angle > -180)
            {
                angle = -90;
            }
            handleParentTransform.localEulerAngles = new Vector3(0, 0, angle);
            //textPixelEffect.effectFactor = 0.8f + 0.2f * ((targetPos - handleRect.anchoredPosition).magnitude) / (radius * 2);
            textPixelEffect.effectFactor = 0.9f + 0.1f * Mathf.Abs((targetAngle - angle)) / 180;
        }
        else
        {
            Debug.Log("오토");
            targetPos = (new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * radius;
            targetAngle = Random.Range(-90f, 180f);
            //handleRect.anchoredPosition = targetPos;
            handleParentTransform.localRotation = Quaternion.Euler(0, 0, targetAngle);

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
