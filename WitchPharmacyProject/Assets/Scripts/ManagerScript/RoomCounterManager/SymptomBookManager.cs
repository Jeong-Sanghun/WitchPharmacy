using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymptomBookManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] GameObject bookCanvas;
    [SerializeField] Transform pageBundleParent;
    [SerializeField] GameObject pageBundlePrefab;
    [SerializeField] GameObject onePagePrefab;
    [SerializeField] Text prefabTitleText;
    [SerializeField] Text prefabExplainText;
    [SerializeField] Image prefabImage;

    SymptomBookBundle debugBookBundle;

    GameObject nowPageBundle;
    bool isNewPage;
    int nowPageIndex;

    List<GameObject> pageBundleList;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.singleTon;
        //연구할 때 이걸 받아와야함.
        isNewPage = true;
        nowPageIndex = 0;
        pageBundleList = new List<GameObject>();
        debugBookBundle = gameManager.LoadSymptomBookBundle("water+");

        for(int i = 0; i < debugBookBundle.oneSymptomBookList.Count; i++)
        {
            MakePages(debugBookBundle.oneSymptomBookList[i]);
        }
        isNewPage = true;
        for (int i = 0; i < debugBookBundle.twoSymptomBookList.Count; i++)
        {
            MakePages(debugBookBundle.twoSymptomBookList[i]);
        }
    }

    void MakePages(SymptomBook book)
    {

        GameObject pageBundleInst;
        if(isNewPage == true)
        {
            pageBundleInst = Instantiate(pageBundlePrefab, pageBundleParent);
            if(pageBundleList.Count == 0)
            {
                pageBundleInst.SetActive(true);
            }
            else
            {
                pageBundleInst.SetActive(false);
            }
            
            pageBundleList.Add(pageBundleInst);
            
            nowPageBundle = pageBundleInst;
           
        }
        else
        {
            pageBundleInst = nowPageBundle;
        }
        Transform leftPage = pageBundleInst.transform.GetChild(0);
        Transform rightPage = pageBundleInst.transform.GetChild(1);
        prefabTitleText.text = book.title;
        prefabExplainText.text = book.explain;
        prefabImage.sprite = book.LoadImage(debugBookBundle.symptomString);

        GameObject pageInst;
        if (isNewPage)
        {
            pageInst = Instantiate(onePagePrefab, leftPage);
            isNewPage = false;
        }
        else
        {
            pageInst = Instantiate(onePagePrefab, rightPage);
            isNewPage = true;
        }
        pageInst.SetActive(true);
        pageInst.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
    }

    public void ChangePage(bool left)
    {
        Debug.Log("페이지체인지 되냐");
        if (left)
        {
            if(nowPageIndex <= 0)
            {
                return;
            }
            pageBundleList[nowPageIndex].SetActive(false);
            pageBundleList[nowPageIndex - 1].SetActive(true);
            nowPageIndex--;
        }
        else
        {
            if(nowPageIndex >= pageBundleList.Count-1)
            {
                return;
            }
            pageBundleList[nowPageIndex].SetActive(false);
            pageBundleList[nowPageIndex + 1].SetActive(true);
            nowPageIndex++;
        }
    }

    public void BookCanvasActive(bool active)
    {
        bookCanvas.SetActive(active);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
