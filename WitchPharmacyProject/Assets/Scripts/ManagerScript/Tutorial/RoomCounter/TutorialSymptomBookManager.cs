using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSymptomBookManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] TutorialRoomCounterManager tutorialManager;
    [SerializeField] GameObject bookCanvas;
    [SerializeField] Transform pageBundleParent;
    [SerializeField] GameObject pageBundlePrefab;
    [SerializeField] GameObject onePagePrefab;
    [SerializeField] Text prefabTitleText;
    [SerializeField] Text prefabExplainText;
    [SerializeField] Image prefabImage;
    [SerializeField] Image prefabGlyphImage;
    Transform effectIconGlowParent;


    List<SymptomBookBundle> bookBundleList;
    SymptomBookBundle nowBundle;
    GameObject nowPageBundle;
    bool isNewPage;
    int nowPageIndex;
    List<GameObject> pageBundleList;
    // Start is called before the first frame update
    void Start()
    {
        int nowPages = 0;
        gameManager = GameManager.singleton;
        //연구할 때 이걸 받아와야함.
        isNewPage = true;
        nowPageIndex = 0;
        pageBundleList = new List<GameObject>();
        bookBundleList = new List<SymptomBookBundle>();
        SymptomBookBundle bundle = gameManager.LoadSymptomBookBundle("water+");
        bookBundleList.Add(bundle);
        bundle = gameManager.LoadSymptomBookBundle("water-");
        bookBundleList.Add(bundle);
        for (int j = 0; j < bookBundleList.Count; j++)
        {
            nowPages += bookBundleList[j].oneSymptomBookList.Count + bookBundleList[j].twoSymptomBookList.Count;
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
        if (isNewPage == true)
        {
            pageBundleInst = Instantiate(pageBundlePrefab, pageBundleParent);
            if (pageBundleList.Count == 0)
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

    public void ChangePage()
    {
        if(tutorialManager.isGlowing[(int)ActionKeyword.RightPageGlow] == false)
        {
            return;
        }
        tutorialManager.isGlowing[(int)ActionKeyword.RightPageGlow] = true;
        pageBundleList[nowPageIndex].SetActive(false);
        pageBundleList[nowPageIndex + 1].SetActive(true);
        effectIconGlowParent = pageBundleList[nowPageIndex + 1].transform.GetChild(1).GetChild(0).GetChild(3);
        tutorialManager.SetEffectIconParent(effectIconGlowParent);
        nowPageIndex++;
    }

    public void BookCanvasActive(bool active)
    {
        if(active == true)
        {
            if(tutorialManager.isGlowing[(int)ActionKeyword.BookGlow] == false)
            {
                return;
            }
        }
        else
        {
            if (tutorialManager.isGlowing[(int)ActionKeyword.ExitGlow] == false)
            {
                return;
            }
        }
        bookCanvas.SetActive(active);
    }

}
