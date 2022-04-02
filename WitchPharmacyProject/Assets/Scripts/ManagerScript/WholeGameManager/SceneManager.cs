using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Text;
using System;
using UnityEngine.Rendering.PostProcessing;

public class SceneManager : MonoBehaviour // JH
{
 

    GameManager gameManager;
    public static SceneManager inst;
    //이전 씬의 네임. storyScene에서 써먹을라고 만듦......
    //public string lastSceneName;
    public string sceneParameter;
    //텍스트가 지금 쳐지고 있으면. 여러매니저에서 써먹을듯.
    public bool nowTexting;
    public int nowSaveIndex;
    public int nowIngameSceneIndex;
    public SceneWrapper sceneWrapper;
   

    void Awake()
    {
        if (inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        Screen.SetResolution(2560, 1440, false);
        gameManager = GameManager.singleton;
        nowTexting = false;
        
        
    }

    public void SetSceneWrapper(string languageDirectory)
    {
        JsonManager json = new JsonManager();
        sceneWrapper = json.ResourceDataLoadBeforeGame<SceneWrapper>("SceneWrapper", languageDirectory);
    }

    public IEnumerator InvokerCoroutine(float time, Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    // // Module // //
    GameObject linearMovingObj;

    public IEnumerator LoadTextOneByOne(string inputTextString, Text inputTextUI, float eachTime = 0.05f, bool canClickSkip = true){
        nowTexting = true;
        float miniTimer = 0f; //타이머
        float currentTargetNumber=0f; // 해당 Time에 출력을 목표로 하는 최소 글자 수
        int currentNumber=0; // 해당 Time에 출력중인 글자 수

        string processedString = inputTextString;
        List<Vector2Int> boldIndexList = new List<Vector2Int>();
        List<Vector2Int> speedChangeIndexList = new List<Vector2Int>();
        List<float> speedChangeValueList = new List<float>();
        while (processedString.Contains("^bold^"))
        {
            int boldStartIndex = processedString.IndexOf("^bold^");
            processedString =  processedString.Remove(boldStartIndex, "^bold^".Length);
            int boldEndIndex = processedString.IndexOf("^boldEnd^");
            processedString = processedString.Remove(boldEndIndex, "^boldEnd^".Length);
            Vector2Int boldVector = new Vector2Int(boldStartIndex, boldEndIndex);
            boldIndexList.Add(boldVector);
        }

        while (processedString.Contains("^speed^"))
        {

            int speedStartIndex = processedString.IndexOf("^speed^");
            processedString = processedString.Remove(speedStartIndex, "^speed^".Length);
            Debug.Log(processedString);
            StringBuilder speedBuilder = new StringBuilder();
            for(int i = speedStartIndex; i<processedString.Length; i++)
            {
                char nowChar = processedString[i];
                if(nowChar == '{')
                {
                    continue;
                }
                else if (nowChar == '}')
                {
                    break;
                }
                else
                {
                    speedBuilder.Append(nowChar);
                }
            }
            Debug.Log(speedBuilder.ToString());
            float speed = float.Parse(speedBuilder.ToString());

            processedString = processedString.Remove(speedStartIndex, speedBuilder.Length + 2);
            speedChangeValueList.Add(speed);

            int speedEndIndex = processedString.IndexOf("^speedEnd^");
            processedString = processedString.Remove(speedEndIndex, "^speedEnd^".Length);
            Vector2Int speedVector = new Vector2Int(speedStartIndex, speedEndIndex);
            speedChangeIndexList.Add(speedVector);
        }


        string displayedText="";
        StringBuilder builder = new StringBuilder(displayedText);
        while (currentTargetNumber < processedString.Length){
            bool bold = false;

            while (currentNumber < currentTargetNumber){ // 목표 글자수까지 출력
                if (boldIndexList.Count >= 1)
                {
                    if (currentNumber >= boldIndexList[0].x && currentNumber < boldIndexList[0].y)
                    {
                        builder.Append("<b>");
                        bold = true;
                    }
                    if (currentNumber == boldIndexList[0].y)
                    {
                        boldIndexList.RemoveAt(0);
                    }

                }
                //displayedText += inputTextString.Substring(currentNumber,1);
                builder.Append(processedString.Substring(currentNumber, 1));
                currentNumber++;
                if (bold == true)
                {
                    builder.Append("</b>");
                }
            }

            //inputTextUI.text = displayedText;
            inputTextUI.text = builder.ToString();
            yield return null;
            miniTimer += Time.deltaTime;
            float nowSpeed = eachTime;
            if(speedChangeIndexList.Count >= 1)
            {
                if (currentNumber >= speedChangeIndexList[0].x && currentNumber < speedChangeIndexList[0].y)
                {
                    nowSpeed /= speedChangeValueList[0];
                }
                if (currentNumber == speedChangeIndexList[0].y)
                {
                    speedChangeValueList.RemoveAt(0);
                    speedChangeIndexList.RemoveAt(0);
                }
            }

            currentTargetNumber = miniTimer/nowSpeed;
            if(Input.GetMouseButtonDown(0)&&canClickSkip){
                break;
            }
        }
        while (currentNumber < processedString.Length){ // 목표 글자수까지 출력
            bool bold = false;
            if (boldIndexList.Count >= 1)
            {
                if (currentNumber >= boldIndexList[0].x && currentNumber < boldIndexList[0].y)
                {
                    builder.Append("<b>");
                    bold = true;
                }
                if (currentNumber == boldIndexList[0].y)
                {
                    boldIndexList.RemoveAt(0);
                }

            }
            builder.Append(processedString.Substring(currentNumber, 1));
            currentNumber++;
            if (bold == true)
            {
                builder.Append("</b>");
            }
        }
        inputTextUI.text = builder.ToString();
        yield return null;
        nowTexting = false;
    }

    public IEnumerator ChangeScale_Object(GameObject i_Object, float size_Initial, float size_final, float i_Time)
    {
        i_Object.SetActive(true);
        Transform objTransform = i_Object.transform;
        float miniTimer = 0f;
        
        while (miniTimer < i_Time)
        {
            float sizeFloat= Mathf.Lerp(size_Initial, size_final, miniTimer / i_Time);
            Vector3 sizeVector = new Vector3(sizeFloat, sizeFloat, sizeFloat);
            objTransform.localScale = sizeVector;
            yield return null;
            miniTimer += Time.deltaTime;
        }
        objTransform.localScale = new Vector3(size_final, size_final, size_final);
        yield return null;
    }

    public IEnumerator ColorChange_Sprite(GameObject i_Object, float color_Initial, float color_Final, float i_Time)
    {
        i_Object.SetActive(true);
        SpriteRenderer i_image = i_Object.GetComponent<SpriteRenderer>();
        float miniTimer = 0f;
        Color initColor = new Color(color_Initial, color_Initial, color_Initial, 1);
        Color afterColor = new Color(color_Final, color_Final, color_Final, 1);
        while (miniTimer < i_Time)
        {
            i_image.color = Color.Lerp(initColor, afterColor, miniTimer / i_Time);
            yield return null;
            miniTimer += Time.deltaTime;
        }
        i_image.color = afterColor;
        yield return null;
    }

    public IEnumerator FadeModule_Sprite(GameObject i_Object, float i_Alpha_Initial, float i_Alpha_Final, float i_Time)
    {
        i_Object.SetActive(true);
        SpriteRenderer i_image = i_Object.GetComponent<SpriteRenderer>();
        float miniTimer = 0f;
        float newAlpha = i_Alpha_Initial;
        while (miniTimer < i_Time)
        {
            newAlpha = Mathf.Lerp(i_Alpha_Initial, i_Alpha_Final, miniTimer / i_Time);
            i_image.color = new Color(i_image.color.r, i_image.color.g, i_image.color.b, newAlpha);
            yield return null;
            miniTimer += Time.deltaTime;
        }
        i_image.color = new Color(i_image.color.r, i_image.color.g, i_image.color.b, i_Alpha_Final);
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
    }

    bool moveModuleLinearRunning = false;
    bool moveModuleInterrupt = false;
    public IEnumerator MoveModule_Linear(GameObject i_Object, Vector3 i_Vector, float i_Time)
    {
        float miniTimer = 0f;
        if (moveModuleLinearRunning == true && linearMovingObj == i_Object)
        {
            moveModuleInterrupt = true;
        }
        moveModuleLinearRunning = true;
        Vector3 newVector = new Vector3(0f, 0f, 0f);
        Vector3 origin = i_Object.transform.position;
        linearMovingObj = i_Object;
        float newX = 0f, newY = 0f, newZ = 0f;
        while (miniTimer < i_Time)
        {
            newX = Mathf.Lerp(origin.x, i_Vector.x, miniTimer / i_Time);
            newY = Mathf.Lerp(origin.y, i_Vector.y, miniTimer / i_Time);
            newZ = Mathf.Lerp(origin.z, i_Vector.z, miniTimer / i_Time);
            newVector = new Vector3(newX, newY, newZ);
            i_Object.transform.position = newVector;
            yield return null;
            if (moveModuleInterrupt)
            {
                moveModuleInterrupt = false;
                break;
            }
            miniTimer += Time.deltaTime;
        }
        if (linearMovingObj == i_Object)
        {
            i_Object.transform.position = i_Vector;
        }

        yield return null;
        moveModuleLinearRunning = false;
    }

    public IEnumerator ShakeModule(GameObject i_Object,float shakePowerX,float shakePowerY, float i_Time)
    {
        float miniTimer = 0f;
        if (moveModuleLinearRunning == true && linearMovingObj == i_Object)
        {
            moveModuleInterrupt = true;
        }
        moveModuleLinearRunning = true;
        Vector3 newVector = new Vector3(0f, 0f, 0f);
        Vector3 origin = i_Object.transform.position;
        linearMovingObj = i_Object;
        float newX = 0f, newY = 0f;
        
        while (miniTimer < i_Time)
        {
            newX = origin.x + UnityEngine.Random.Range(-shakePowerX, shakePowerX);
            newY = origin.y + UnityEngine.Random.Range(-shakePowerY, shakePowerY);
            
            newVector = new Vector3(newX, newY, origin.z);
            i_Object.transform.position = newVector;
            yield return null;

            miniTimer += Time.deltaTime;
        }
        i_Object.transform.position = origin;

        yield return null;
        moveModuleLinearRunning = false;
    }

    

    public IEnumerator VolumeModule(PostProcessVolume volume, bool finalOne, float time)
    {
        float timer = 0;
        int one;
        if (finalOne)
        {
            timer = 0;
            while (timer < 1)
            {
                volume.weight = timer;
                timer += Time.deltaTime / time;
                yield return null;
            }
            volume.weight = 1;
        }
        else
        {
            timer = 1;
            while (timer > 0)
            {
                volume.weight = timer;
                timer -= Time.deltaTime / time;
                yield return null;
            }
            volume.weight = 0;
        }

    }

    public IEnumerator MoveModuleRect_Linear(GameObject i_Object, Vector3 i_Vector, float i_Time)
    {
        float miniTimer = 0f;
        Vector2 newVector = new Vector2(0f, 0f);
        float newX = 0f, newY = 0f;
        RectTransform rect = i_Object.GetComponent<RectTransform>();
        Vector2 origin = rect.anchoredPosition;
        while (miniTimer < i_Time)
        {
            newX = Mathf.Lerp(origin.x, i_Vector.x, miniTimer / i_Time);
            newY = Mathf.Lerp(origin.y, i_Vector.y, miniTimer / i_Time);
            //newZ = Mathf.Lerp(rect.anchoredPosition.z, i_Vector.z, miniTimer / i_Time);
            newVector = new Vector2(newX, newY);
            rect.anchoredPosition = newVector;
            yield return null;
            miniTimer += Time.deltaTime;
        }
        rect.anchoredPosition = i_Vector;
        yield return null;
    }


    // 가속이동모듈 (움직일 오브젝트, 목표벡터값, 이동시간, 초기가속여부, 후기가속여부, 이동후제거)
    public IEnumerator MoveModule_Accel(GameObject i_Object, Vector3 i_Vector, float i_Time, bool i_firstAccel=true, bool i_lastAccel=true, bool i_afterDestroy = false){
        float miniTimer = 0f;
        float miniTimer_Accel = 0f;
        Vector3 initialVector = new Vector3(i_Object.transform.position.x,i_Object.transform.position.y,i_Object.transform.position.z);
        Vector3 newVector = new Vector3(0f,0f,0f);
        float newX=0f, newY=0f, newZ=0f;
        float timeDivision = 1f;
        if(i_firstAccel&&i_lastAccel)
            timeDivision = 2f;
        if(i_firstAccel){
            while(miniTimer<i_Time){
                miniTimer_Accel = (miniTimer/i_Time)*(miniTimer/i_Time); // ((miniTimer/i_Time)^2)
                newX= Mathf.Lerp(initialVector.x,i_Vector.x,miniTimer_Accel/timeDivision);
                newY= Mathf.Lerp(initialVector.y,i_Vector.y,miniTimer_Accel/timeDivision);
                newZ= Mathf.Lerp(initialVector.z,i_Vector.z,miniTimer_Accel/timeDivision);
                newVector = new Vector3(newX,newY,newZ);
                i_Object.transform.position = newVector;
                yield return null;
                miniTimer+=Time.deltaTime;
            }
        }
        miniTimer = i_Time;
        if(i_lastAccel){
            while(miniTimer>0f){
                miniTimer_Accel = (miniTimer/i_Time)*(miniTimer/i_Time); // ((miniTimer/i_Time)^2)
                newX= Mathf.Lerp(initialVector.x,i_Vector.x,1f - miniTimer_Accel/timeDivision);
                newY= Mathf.Lerp(initialVector.y,i_Vector.y,1f - miniTimer_Accel/timeDivision);
                newZ= Mathf.Lerp(initialVector.z,i_Vector.z,1f - miniTimer_Accel/timeDivision);
                newVector = new Vector3(newX,newY,newZ);
                i_Object.transform.position = newVector;
                yield return null;
                miniTimer-=Time.deltaTime;
            }
        }
        i_Object.transform.position = i_Vector;
        i_Object.SetActive(!i_afterDestroy);
        yield return null;
    }

    // 부드러운 가속모듈
    public IEnumerator MoveModule_Accel2(GameObject i_Object, Vector3 i_Vector, float i_Time){
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

    public IEnumerator AfterRunCoroutine(float t, IEnumerator i){
        yield return new WaitForSeconds(t);
        StartCoroutine(i);
    }


    // // Module // //

        //최초에 씬을 불러오기...
    public void VeryFirstStartLoad()
    {
        
        StartCoroutine(StartLoadingSceneCoroutine());
    }

    IEnumerator StartLoadingSceneCoroutine()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("StartLoadingScene");
        yield return null;
        Text loadText = GameObject.Find("LoadingText").GetComponent<Text>();
        gameManager = GameManager.singleton;
        //if(gameManager.saveData.nowSaveTime == SaveTime.DayStart)
        //{
        //    lastSceneName = "StoryScene";
        //}
        //else
        //{
        //    lastSceneName = "RoomCounterScene";
        //}
        //게임매니저가 여기서 로드할 때 생기거덩 그러면 게임매니저에서 씬매니저의 그 인덱스 번호를 가져가서 스타트에서 로드를 때려버림. 
        int nowSceneIndex = gameManager.saveData.nowSceneIndex;
        string nowScene = sceneWrapper.sceneArray[nowSceneIndex].sceneName;
        sceneParameter = sceneWrapper.sceneArray[nowSceneIndex].sceneParameter;
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(nowScene);

        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {

            timer += Time.deltaTime;
            loadText.text = op.progress.ToString();
            if (timer > 0.5f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
        //if (nowScene == "StoryScene")
        //{
        //    TabletManager.inst.TabletOpenButtonActive(false);
        //}
        //else
        //{
        //    TabletManager.inst.TabletOpenButtonActive(true);
        //}
    }

    //public void BossSceneLoad()
    //{
    //    StartCoroutine(BossSceneCoroutine());
    //}

    //IEnumerator BossSceneCoroutine()
    //{
    //    UnityEngine.SceneManagement.SceneManager.LoadScene("StartLoadingScene");
    //    yield return null;
    //    Text loadText = GameObject.Find("LoadingText").GetComponent<Text>();
    //    gameManager = GameManager.singleton;
    //    //게임매니저가 여기서 로드할 때 생기거덩 그러면 게임매니저에서 씬매니저의 그 인덱스 번호를 가져가서 스타트에서 로드를 때려버림. 
    //    int nowSceneIndex = gameManager.saveData.nowSceneIndex;
    //    string nowScene = sceneWrapper.sceneArray[nowSceneIndex].sceneName;
    //    sceneParameter = sceneWrapper.sceneArray[nowSceneIndex].storyName;
    //    AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("BossScene");

    //    op.allowSceneActivation = false;

    //    float timer = 0.0f;

    //    while (!op.isDone)
    //    {

    //        timer += Time.deltaTime;
    //        loadText.text = op.progress.ToString();
    //        if (timer > 0.5f)
    //        {
    //            op.allowSceneActivation = true;
    //        }
    //        yield return null;
    //    }
    //    if (gameManager.saveData.nextLoadSceneName == "StoryScene")
    //    {
    //        TabletManager.inst.TabletOpenButtonActive(false);
    //    }
    //    else
    //    {
    //        TabletManager.inst.TabletOpenButtonActive(true);
    //    }
    //}

    public void LoadNextScene()
    {

        if (sceneWrapper.sceneArray[gameManager.saveData.nowSceneIndex].saveTimeString != null)
        {
            gameManager.saveData.nowSceneIndex++;
            gameManager.ForceSaveButtonActive();
            return;
        }
        gameManager.saveData.nowSceneIndex++;



        int nowSceneIndex = gameManager.saveData.nowSceneIndex;
        if(sceneWrapper.sceneArray[nowSceneIndex].day != null)
        {
            int day = int.Parse(sceneWrapper.sceneArray[nowSceneIndex].day);
            gameManager.saveData.nowDay = day;
        }
        string nowScene = sceneWrapper.sceneArray[nowSceneIndex].sceneName;
        sceneParameter = sceneWrapper.sceneArray[nowSceneIndex].sceneParameter;
        StartCoroutine(LoadingSceneCoroutine(nowScene));
    }

    public void LoadNowScene()
    {
        int nowSceneIndex = gameManager.saveData.nowSceneIndex;

        string nowScene = sceneWrapper.sceneArray[nowSceneIndex].sceneName;
        sceneParameter = sceneWrapper.sceneArray[nowSceneIndex].sceneParameter;
        StartCoroutine(LoadingSceneCoroutine(nowScene));
    }

    public void LoadScene(string sceneName)
    {
        //lastSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        StartCoroutine(LoadingSceneCoroutine(sceneName));
    }

    //불러오기 씬.
    public void SaveDataLoadScene()
    {
        //lastSceneName = null;
        //gameManager.LoadJson(index);
        int nowSceneIndex = gameManager.saveData.nowSceneIndex;
        string nowScene = sceneWrapper.sceneArray[nowSceneIndex].sceneName;
        sceneParameter = sceneWrapper.sceneArray[nowSceneIndex].sceneParameter;
        StartCoroutine(LoadingSceneCoroutine(nowScene));
    }

    IEnumerator LoadingSceneCoroutine(string sceneName)
    {
        TabletManager.inst.TabletOpenButtonActive(false,true);
        UnityEngine.SceneManagement.SceneManager.LoadScene("LoadingScene");

        yield return null;
        Text loadText = GameObject.Find("LoadingText").GetComponent<Text>();
        AsyncOperation op = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);

        op.allowSceneActivation = false;

        float timer = 0.0f;

        while (!op.isDone)
        {

            timer += Time.deltaTime;
            loadText.text = op.progress.ToString();
            if (timer > 0.5f)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
        TabletManager.inst.TabletOpenButtonActive(true,true);




    }



    
}
