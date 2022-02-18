using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialRoomManager : MonoBehaviour
{
    GameManager gameManager;
    //[SerializeField]
    //CounterManager counterManager;
    //[SerializeField]
    //MedicineManager medicineManager;
    //[SerializeField]
    //CounterDialogManager counterDialogManager;
    //[SerializeField]
    //VisitorTriggerManager visitorTriggerManager;
    UILanguagePack languagePack;

    [SerializeField]
    TutorialRoomCounterManager tutorialManager;
    [SerializeField]
    GameObject cam;
    [SerializeField]
    GameObject roomUICanvas;
    [SerializeField]
    GameObject counterUICanvas;
    Vector3 cameraCounterPos;
    Vector3 cameraRoomPos;


    // Start is called before the first frame update
    void Start()
    {

        gameManager = GameManager.singleton;
        languagePack = gameManager.languagePack;
        //saveData = gameManager.saveData;
        //sceneManager = SceneManager.inst;
        cameraRoomPos = new Vector3(51.2f, 0, -10);
        cameraCounterPos = new Vector3(0, 0, -10);
    }

    //이거 카운터매니저에서 넘겨주는거
    public void VisitorVisits(RandomVisitorClass visitor)
    {

    }

    //카운터 다이얼로그 매니저에서 스타트 끝나면 불러옴

    //CookedMedicineManager에서 약을 트레이로 올릴 때도 사용함. 버튼에서도 사용함.
    //룸이 움직일 때 카메라 옮겨줌
    public void ToCounterButton(bool isMedicineOnTray)
    {
        //버튼에서 호출할때는 false, 트레이에올려서 호출할때는 true
        cam.transform.position = cameraCounterPos;
        roomUICanvas.SetActive(false);
        counterUICanvas.SetActive(true);
        //medicineManager.ToCounterButton(isMedicineOnTray);
    }

    //이거 카운터매니저에 넣을려 했는데 카운터매니저에서 변수선언하기 귀찮음.
    //카운터에서 조제실 오는 버튼.
    public void ToRoomButton()
    {
        if (tutorialManager.isGlowing[(int)ActionKeyword.ToRoomButtonGlow] == false)
        {
            return;
        }
        cam.transform.position = cameraRoomPos;
        roomUICanvas.SetActive(true);
        counterUICanvas.SetActive(false);
        //medicineManager.ToRoomButton();
    }

}
