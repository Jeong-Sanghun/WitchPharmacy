using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeScriptModule : MonoBehaviour //JH 22.05.16
{
    protected SceneManager sceneManager;
    [SerializeField]
    bool startingFadeIn;  // bool 체크하면 해당 오브젝트 SetActive(True) 될때마다 자동 FadeIn
    [SerializeField]
    float startingFadeInTime = 1f;
    [SerializeField]
    bool isButton;  // bool 체크하면 자식 오브젝트의 Text 제어
    //public bool endingFadeOut;
    Coroutine onLoadCoroutine1;
    Coroutine onLoadCoroutine2;


    public void StartingFadeIn(float t = 1f){ // OnEnable시 실행, 오브젝트 SetActive(true) 시 페이드인 효과
        if(this.gameObject.activeSelf==false) // 오브젝트 꺼져있으면 켜주고
            this.gameObject.SetActive(true);
        if(this.gameObject.GetComponent<Text>()!=null) // Text나 Image FadeIn 모듈 실행
            onLoadCoroutine1 = StartCoroutine(sceneManager.FadeModule_Text(this.gameObject.GetComponent<Text>(), 0f, 1f, t));
        if(isButton && this.gameObject.GetComponentInChildren<Text>()!=null) // Text나 Image FadeIn 모듈 실행
            onLoadCoroutine1 = StartCoroutine(sceneManager.FadeModule_Text(this.gameObject.GetComponentInChildren<Text>(), 0f, 1f, t));
        if(this.gameObject.GetComponent<Image>()!=null)
            onLoadCoroutine2 = StartCoroutine(sceneManager.FadeModule_Image(this.gameObject, 0f, 1f, t));
    }

    public void EndingFadeOut(float t = 1f){ // 페이드아웃 코루틴 호출 함수
        StartCoroutine(EndingFadeOutC(t));
    }

    private IEnumerator EndingFadeOutC(float t){ //페이드 아웃 후 SetActive False 예약
        if(this.gameObject.GetComponent<Text>()!=null) // Text나 Image FadeOut 모듈 실행
            yield return (onLoadCoroutine1 = StartCoroutine(sceneManager.FadeModule_Text(this.gameObject.GetComponent<Text>(), 1f, 0f, t)));
        else if(this.gameObject.GetComponent<Image>()!=null)
            yield return (onLoadCoroutine2 = StartCoroutine(sceneManager.FadeModule_Image(this.gameObject, 1f, 0f, t)));
        this.gameObject.SetActive(false); //오브젝트 끔
    }

    public void StopFade(){ // 페이드 코루틴 외부에서 멈출때 사용
        OnDisable();
    }

    void Awake()
    {
        sceneManager = SceneManager.inst;
    }

    private void OnEnable() {
        if (startingFadeIn) // bool 체크하면 해당 오브젝트 SetActive(True) 될때마다 자동 FadeIn
            StartingFadeIn(startingFadeInTime);
    }

    private void OnDisable() { // 오브젝트 비활성화 시 코루틴 정지
        if(onLoadCoroutine1!=null)
            StopCoroutine(onLoadCoroutine1);
        if(onLoadCoroutine2!=null)
            StopCoroutine(onLoadCoroutine2);
    }


}
