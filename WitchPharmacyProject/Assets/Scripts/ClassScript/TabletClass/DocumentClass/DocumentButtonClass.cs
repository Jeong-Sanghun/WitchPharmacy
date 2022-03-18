using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;
using UnityEngine.EventSystems;

//여백의 미 버튼
public class DocumentButtonClass
{
    
    public int index;
    public DocumentBundle bundle;
    public OwningDocumentClass owningDocumentClass;
    public GameObject buttonObject;
    public GameObject documentCanvas;
    public Transform scrollContent;
    public Image documentImage;
    public Text documentText;
    public Text gainedRegionText;
    public DocumentCondition documentCondition;
    public GameObject imageOpenCanvas;
    public Image imageOpenImage;
    public Text imageOpenText;
    public Text popupText;
    public GameObject popupObject;
    public bool isOpened;
    //public List<GameObject> highlightButtonList;
    //public List<GameObject> highlightPopupList;
    List<Highlight> highlightList;


    public DocumentButtonClass(int _index,DocumentCondition condition, DocumentBundle doc,OwningDocumentClass ownDoc, GameObject buttonObj)
    {
        owningDocumentClass = ownDoc;
        index = _index;
        bundle = doc;
        buttonObject = buttonObj;
        documentCondition = condition;
        Text buttonText = buttonObject.transform.GetChild(1).GetChild(0).GetComponent<Text>();
        buttonText.text = condition.ingameName;
        highlightList = new List<Highlight>();
    }


    public void SetupDocument(GameObject canvas, UILanguagePack languagePack, GameObject highlightButtonPref, GameObject highlightPopupPref, GameObject imageOpenPrefab)
    {
        isOpened = true;
        documentCanvas = canvas;
        scrollContent = documentCanvas.transform.GetChild(1).GetChild(0).GetChild(0);
        //여기서 겟 차일드 지랄지랄 해주면 된다.
        documentImage = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(0).GetComponent<Image>();
        documentText = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
        gainedRegionText = canvas.transform.GetChild(1).GetChild(0).GetChild(0).GetChild(2).GetComponent<Text>();
        popupObject = canvas.transform.GetChild(1).GetChild(0).GetChild(1).gameObject;
        popupText = popupObject.transform.GetChild(0).GetComponent<Text>();
        if (documentCondition.printSprite == true)
        {
            documentImage.sprite = bundle.LoadSprite();

            EventTrigger trigger = documentImage.GetComponent<EventTrigger>();

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { OpenImage(); });
            trigger.triggers.Add(entry);

            imageOpenCanvas = GameObject.Instantiate(imageOpenPrefab, TabletManager.inst.transform);
            imageOpenCanvas.SetActive(false);
            imageOpenImage = imageOpenCanvas.transform.GetChild(1).GetComponent<Image>();
            imageOpenText = imageOpenCanvas.transform.GetChild(2).GetComponent<Text>();
            imageOpenImage.sprite = bundle.LoadSprite();
            imageOpenText.text = documentCondition.ingameName;

            EventTrigger shutdown = imageOpenCanvas.transform.GetChild(3).GetComponent<EventTrigger>();
            EventTrigger.Entry entry1 = new EventTrigger.Entry();
            entry1.eventID = EventTriggerType.PointerUp;
            entry1.callback.AddListener((data) => { ShutImage(); });
            shutdown.triggers.Add(entry1);

        }
        else
        {
            documentImage.gameObject.SetActive(false);
            documentText.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -280);
        }

        EventTrigger popupTrigger = popupObject.GetComponent<EventTrigger>();

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = EventTriggerType.PointerUp;
        entry2.callback.AddListener((data) => { CloseHighlight((PointerEventData)data); });
        popupTrigger.triggers.Add(entry2);

        MakeHighlight(highlightButtonPref,highlightPopupPref);
        gainedRegionText.text = languagePack.documentGainedRegion + languagePack.exploreRegionNameArray[(int)owningDocumentClass.gainedRegion];
    }

    void OpenImage()
    {
        imageOpenCanvas.SetActive(true);
        
    }

    void ShutImage()
    {
        imageOpenCanvas.SetActive(false);
    }

    void MakeHighlight(GameObject buttonPrefab, GameObject popupPrefab)
    {

        List<string> highlightedText = new List<string>();
        List<string> highlightPopupDocument = new List<string>();
        StringBuilder documentBuilder = new StringBuilder();
        StringBuilder builder = new StringBuilder();
        bool nowButton = false;
        bool nowPopup = false;
        for (int i = 0; i < bundle.document.Length; i++)
        {
            if (nowButton == true || nowPopup == true)
            {
                builder.Append(bundle.document[i]);
            }
            if (bundle.document[i] == '$')
            {
                if (nowButton == true)
                {

                    builder.Remove(builder.Length - 1, 1);
                    highlightedText.Add(builder.ToString());
                    Debug.Log("버튼 " +builder.ToString());
                    highlightList[highlightList.Count-1].endIndex = documentBuilder.Length;
                    builder.Clear();
                    
                    nowButton = false;
                }
                else
                {
                    nowButton = true;
                    Highlight index = new Highlight();
                    index.startIndex = documentBuilder.Length;
                    highlightList.Add(index);
                }

            }
            else if (bundle.document[i] == '*')
            {
                if(nowPopup == true)
                {
                    builder.Remove(builder.Length - 1, 1);
                    highlightPopupDocument.Add(builder.ToString());
                    Debug.Log("팝업 " + builder.ToString());
                    highlightList[highlightList.Count - 1].popupString = builder.ToString();
                    builder.Clear();
                    nowPopup = false;
                }
                else
                {
                    nowPopup = true;
                }
            }
            else if (!nowPopup)
            {
                documentBuilder.Append(bundle.document[i]);
            }

        }

        documentText.text = documentBuilder.ToString();
        TextGenerator textGen = documentText.cachedTextGenerator;
        //Debug.Log(generator.
        //TextGenerator textGen = new TextGenerator(documentBuilder.Length);
        Vector2 extents = documentText.gameObject.GetComponent<RectTransform>().rect.size;
        textGen.Populate(documentBuilder.ToString(), documentText.GetGenerationSettings(extents));

        if(documentCondition.printSprite == true)
        {
            scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 2 * documentText.fontSize + 1100 - textGen.lines[textGen.lineCount - 1].topY);
        }
        else
        {
            scrollContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 2 * documentText.fontSize + 280 - textGen.lines[textGen.lineCount - 1].topY);
        }
        

        for (int i = 0; i < highlightList.Count; i++)
        {
            Debug.Log("스타트 " + highlightList[i].startIndex + "엔드 " + highlightList[i].endIndex);
            //다른줄일때.
            if (textGen.characters[highlightList[i].endIndex].cursorPos.y != textGen.characters[highlightList[i].startIndex].cursorPos.y)
            {
                GameObject inst = GameObject.Instantiate(buttonPrefab, documentText.transform);
                inst.SetActive(true);
                RectTransform rect = inst.GetComponent<RectTransform>();
                rect.anchoredPosition = textGen.characters[highlightList[i].startIndex].cursorPos;
                bool solved = false;
                int nowLine = 0;
                //int startLine = 0;
                List<GameObject> instList = new List<GameObject>();
                for (int j = 0; j < textGen.lineCount; j++)
                {
                    int lineStartIndex = textGen.lines[j].startCharIdx;
                    if (textGen.characters[lineStartIndex].cursorPos.y == textGen.characters[highlightList[i].startIndex].cursorPos.y)
                    {
                        //startLine = j;
                        Debug.Log(nowLine);
                        nowLine = j;
                        break;
                    }
                }
                float xSize = textGen.rectExtents.width / 2 - textGen.characters[highlightList[i].startIndex].cursorPos.x;
                rect.sizeDelta = new Vector2(xSize, documentText.fontSize);
                inst.transform.SetParent(scrollContent);
                inst.transform.SetAsFirstSibling();
                instList.Add(inst);
                while (solved == false)
                {
                    //스타트라인은 해결해줬으니까 다음라인으로 넘어가서 판정해줌. 다음라인이랑 엔드인덱스랑 같은줄인지.
                    nowLine++;
                    if(i ==0)
                    {
                        Debug.Log(nowLine);
                    }
                    if(nowLine >= textGen.lineCount)
                    {
                        break;
                    }
                    int lineStartIndex = textGen.lines[nowLine].startCharIdx;
                    if(textGen.characters[lineStartIndex].cursorPos.y == textGen.characters[highlightList[i].endIndex].cursorPos.y)
                    {
                        //같은줄이면 끝이지. 
                        solved = true;

                        inst = GameObject.Instantiate(buttonPrefab, documentText.transform);
                        inst.SetActive(true);
                        rect = inst.GetComponent<RectTransform>();
                        rect.anchoredPosition = textGen.characters[lineStartIndex].cursorPos;
                        xSize = textGen.characters[highlightList[i].endIndex].cursorPos.x - textGen.characters[lineStartIndex].cursorPos.x;
                        rect.sizeDelta = new Vector2(xSize, documentText.fontSize);
                        inst.transform.SetParent(scrollContent);
                        inst.transform.SetAsFirstSibling();
                        instList.Add(inst);
                        break;
                    }
                    else
                    {
                        //다른줄이면 끝까지 돌려야함.
                        solved = false;
                        inst = GameObject.Instantiate(buttonPrefab, documentText.transform);
                        inst.SetActive(true);
                        rect = inst.GetComponent<RectTransform>();
                        rect.anchoredPosition = textGen.characters[lineStartIndex].cursorPos;
                        xSize = textGen.rectExtents.width;
                        rect.sizeDelta = new Vector2(xSize, documentText.fontSize);
                        inst.transform.SetParent(scrollContent);
                        inst.transform.SetAsFirstSibling();
                        instList.Add(inst);
                    }


                }
                for (int j = 0; j < instList.Count;j++)
                {
                    EventTrigger trigger = instList[j].GetComponent<EventTrigger>();

                    EventTrigger.Entry entry1 = new EventTrigger.Entry();
                    entry1.eventID = EventTriggerType.PointerUp;
                    int dele = i;
                    entry1.callback.AddListener((data) => { OpenHighlight((PointerEventData)data,dele); });
                    trigger.triggers.Add(entry1);
                }
            }
            else
            {
                //같은줄일때.
                GameObject inst = GameObject.Instantiate(buttonPrefab, documentText.transform);
                inst.SetActive(true);
                RectTransform rect = inst.GetComponent<RectTransform>();
                rect.anchoredPosition = textGen.characters[highlightList[i].startIndex].cursorPos;

                float xSize = textGen.characters[highlightList[i].endIndex].cursorPos.x - textGen.characters[highlightList[i].startIndex].cursorPos.x;
                rect.sizeDelta = new Vector2(xSize, documentText.fontSize);
                inst.transform.SetParent(scrollContent);
                inst.transform.SetAsFirstSibling();

                EventTrigger trigger = inst.GetComponent<EventTrigger>();

                EventTrigger.Entry entry1 = new EventTrigger.Entry();
                entry1.eventID = EventTriggerType.PointerUp;
                int dele = i;
                entry1.callback.AddListener((data) => { OpenHighlight((PointerEventData)data, dele); });
                trigger.triggers.Add(entry1);
            }
            //popupObject = popupPrefab;
 



        }


        

        //Debug.Log(info[0].cursorPos);
        //Debug.Log(newLine);
        //Debug.Log(myLine);
        //Debug.Log(textGen.characterCount);
        //Debug.Log(textGen.vertexCount);




        return;


    }

    void OpenHighlight(PointerEventData data, int index)
    {
        popupText.text = highlightList[index].popupString;
        popupObject.SetActive(true);
        RectTransform popupRect = popupObject.GetComponent<RectTransform>();
        //popupRect.anchoredPosition = textGen.characters[highlightList[i].startIndex].cursorPos;
        //highlightList[i].popupObject = popup;
        TextGenerator popupGen = popupText.cachedTextGenerator;
        //Debug.Log(generator.
        //TextGenerator textGen = new TextGenerator(documentBuilder.Length);
        Vector2 popupExtents = popupText.gameObject.GetComponent<RectTransform>().rect.size;
        popupGen.Populate(highlightList[index].popupString, popupText.GetGenerationSettings(popupExtents));
        Debug.Log(popupGen.lines[popupGen.lineCount - 1].topY);
        if ((-1) * popupGen.lines[popupGen.lineCount - 1].topY + 4 * popupText.fontSize > 300)
        {
            popupRect.sizeDelta = new Vector2(popupRect.sizeDelta.x, (-1) * popupGen.lines[popupGen.lineCount - 1].topY + 4 * popupText.fontSize);
        }
        else
        {
            popupRect.sizeDelta = new Vector2(popupRect.sizeDelta.x, 300);
        }
        
    }

    //나가기 누를때 꺼줘야돼서 다큐멘트 매니저에서 불러옴
    public void CloseHighlight(PointerEventData data)
    {
        popupObject.SetActive(false);
    }

    public void ActiveDocument(bool active)
    {
        documentCanvas.SetActive(active);
    }

    
}
