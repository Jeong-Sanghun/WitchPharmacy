using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class SceneManager : MonoBehaviour // JH
{
    
    public GameManager GameManagerScript;


    // // Module // //

    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.05f){
        float miniTimer = 0f; //타이머
        float currentTargetNumber=0f; // 해당 Time에 출력을 목표로 하는 최소 글자 수
        int currentNumber=0; // 해당 Time에 출력중인 글자 수
        string displayedText="";
        StringBuilder builder = new StringBuilder(displayedText);
        while (currentTargetNumber < inputTextString.Length){
            while (currentNumber < currentTargetNumber){ // 목표 글자수까지 출력
                //displayedText += inputTextString.Substring(currentNumber,1);
                builder.Append(inputTextString.Substring(currentNumber, 1));
                currentNumber++;
            }
            //inputTextUI.text = displayedText;
            inputTextUI.text = builder.ToString();
            yield return null;
            miniTimer += Time.deltaTime;
            currentTargetNumber = miniTimer/eachTime;
        }
        yield return null;
    }

     public IEnumerator FadeModule_Image(GameObject i_Object, float i_Alpha_Initial, float i_Alpha_Final, float i_Time){
        i_Object.SetActive(true);
        Image i_image = i_Object.GetComponent<Image>();
        float miniTimer=0f;
        float newAlpha = i_Alpha_Initial;
        while(miniTimer<i_Time){
            newAlpha = Mathf.Lerp(i_Alpha_Initial,i_Alpha_Final,miniTimer/i_Time);
            i_image.color = new Color(i_image.color.r,i_image.color.g,i_image.color.b,newAlpha);
            yield return null;
            miniTimer+=Time.deltaTime;
        }
        i_image.color = new Color(i_image.color.r,i_image.color.g,i_image.color.b,i_Alpha_Final);
        yield return null;
    }

    public IEnumerator FadeModule_Image(GameObject i_Object, float i_Alpha_Initial, float i_Alpha_Final, float i_Time, bool afterActive){
        i_Object.SetActive(true);
        Image i_image = i_Object.GetComponent<Image>();
        float miniTimer=0f;
        float newAlpha = i_Alpha_Initial;
        while(miniTimer<i_Time){
            newAlpha = Mathf.Lerp(i_Alpha_Initial,i_Alpha_Final,miniTimer/i_Time);
            i_image.color = new Color(i_image.color.r,i_image.color.g,i_image.color.b,newAlpha);
            yield return null;
            miniTimer+=Time.deltaTime;
        }
        i_image.color = new Color(i_image.color.r,i_image.color.g,i_image.color.b,i_Alpha_Final);
        yield return null;
        i_Object.SetActive(afterActive);
    }

    public IEnumerator FadeModule_Text(Text i_Object, float i_Alpha_Initial, float i_Alpha_Final, float i_Time){
        //i_Object.SetActive(true);
        //Image i_image = i_Object.GetComponent<Image>();
        float miniTimer=0f;
        float newAlpha = i_Alpha_Initial;
        while(miniTimer<i_Time){
            newAlpha = Mathf.Lerp(i_Alpha_Initial,i_Alpha_Final,miniTimer/i_Time);
            i_Object.color = new Color(i_Object.color.r,i_Object.color.g,i_Object.color.b,newAlpha);
            yield return null;
            miniTimer+=Time.deltaTime;
        }
        i_Object.color = new Color(i_Object.color.r,i_Object.color.g,i_Object.color.b,i_Alpha_Final);
        yield return null;
    }

    public IEnumerator MoveModule_Linear(GameObject i_Object, Vector3 i_Vector, float i_Time){
        float miniTimer = 0f;
        Vector3 newVector = new Vector3(0f,0f,0f);
        float newX=0f, newY=0f, newZ=0f;
        while(miniTimer<i_Time){
            newX= Mathf.Lerp(i_Object.transform.position.x,i_Vector.x,miniTimer/i_Time);
            newY= Mathf.Lerp(i_Object.transform.position.y,i_Vector.y,miniTimer/i_Time);
            newZ= Mathf.Lerp(i_Object.transform.position.z,i_Vector.z,miniTimer/i_Time);
            newVector = new Vector3(newX,newY,newZ);
            i_Object.transform.position = newVector;
            yield return null;
            miniTimer+=Time.deltaTime;
        }
        i_Object.transform.position = i_Vector;
        yield return null;
    }

    public IEnumerator MoveModule_Accel(GameObject i_Object, Vector3 i_Vector, float i_Time){
        float miniTimer = 0f;
        float miniTimer_Accel = 0f;
        Vector3 newVector = new Vector3(0f,0f,0f);
        float newX=0f, newY=0f, newZ=0f;
        while(miniTimer<i_Time){
            miniTimer_Accel = (miniTimer/i_Time)*(miniTimer/i_Time); // ((miniTimer/i_Time)^2)
            newX= Mathf.Lerp(i_Object.transform.position.x,i_Vector.x,miniTimer_Accel);
            newY= Mathf.Lerp(i_Object.transform.position.y,i_Vector.y,miniTimer_Accel);
            newZ= Mathf.Lerp(i_Object.transform.position.z,i_Vector.z,miniTimer_Accel);
            newVector = new Vector3(newX,newY,newZ);
            i_Object.transform.position = newVector;
            yield return null;
            miniTimer+=Time.deltaTime;
        }
        i_Object.transform.position = i_Vector;
        yield return null;
    }



    // // Module // //



    void Start()
    {
       
    }

    
    void Update()
    {
        
    }
}
