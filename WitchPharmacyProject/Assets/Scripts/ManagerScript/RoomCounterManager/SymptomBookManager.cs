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

    List<SymptomBookBundle> bookBundleList;
    SymptomBookBundle nowBundle;
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
        bookBundleList = new List<SymptomBookBundle>();
        for(int i = 0; i < gameManager.saveData.symptomBookList.Count; i++)
        {
            SymptomBookBundle bundle = gameManager.LoadSymptomBookBundle(gameManager.saveData.symptomBookList[i]);
            bookBundleList.Add(bundle);
        }
        for(int j = 0; j < bookBundleList.Count; j++)
        {
            nowBundle = bookBundleList[j];
            for (int i = 0; i < bookBundleList[j].oneSymptomBookList.Count; i++)
            {
                MakePages(bookBundleList[j].oneSymptomBookList[i]);
            }
            isNewPage = true;
            for (int i = 0; i < bookBundleList[j].twoSymptomBookList.Count; i++)
            {
                MakePages(bookBundleList[j].twoSymptomBookList[i]);
            }
            isNewPage = true;
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
        prefabImage.sprite = book.LoadImage(nowBundle.symptomString);

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
