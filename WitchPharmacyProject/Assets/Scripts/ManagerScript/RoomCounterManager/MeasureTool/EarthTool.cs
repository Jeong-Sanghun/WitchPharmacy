using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthTool : MeasureTool
{ 
    class Glyph
    {
        public int glyphType;
        public GameObject obj;
        public Rigidbody2D rigid;
        public SpriteRenderer renderer;
        public bool corrected;
        public Sprite sprite;

        public Glyph()
        {
            corrected = false;
        }
    }

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.
    [SerializeField]
    Text symptomText;
    [SerializeField]
    GameObject glyphPrefab;
    [SerializeField]
    Transform glyphParent;
    [SerializeField]
    SpriteRenderer correctSprite;

    Glyph[] glyphArray;
    Sprite[] spriteArray;
    int correctGlyphType;
    //맞는 글리프 타입 0 1 2 3
    int chosenGlyphIndex;
    const int glyphNumber = 12;
    //글리프 타입은 4개
    float moveTimer = 0;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        spriteArray = new Sprite[4];
        ExplainLoad();
    }

    

    // Update is called once per frame
    void Update()
    {
        if(measureEnd == false && measureStarted == true)
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
                if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
                {
                    touchedObject = hit.collider.gameObject;
                    if (touchedObject.CompareTag("Glyph") &&  chosenGlyphIndex == -1)
                    {
                        for(int i = 0; i < glyphArray.Length; i++)
                        {
                            if(touchedObject == glyphArray[i].obj)
                            {
                                Debug.Log(i);
                                chosenGlyphIndex = i;
                            }
                        }
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
                if (chosenGlyphIndex != -1)
                {
                    glyphArray[chosenGlyphIndex].obj.transform.position = new Vector3(mousePos.x, mousePos.y, 0);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (chosenGlyphIndex != -1)
                {
                    if (glyphArray[chosenGlyphIndex].obj.transform.position.magnitude < 1.5)
                    {
                        if (glyphArray[chosenGlyphIndex].glyphType == correctGlyphType)
                        {
                            glyphArray[chosenGlyphIndex].obj.SetActive(false);
                            glyphArray[chosenGlyphIndex].corrected = true;
                            bool correct = true;
                            for (int i = 0; i < glyphArray.Length; i++)
                            {
                                if (glyphArray[i].glyphType != correctGlyphType)
                                {
                                    continue;
                                }
                                if (glyphArray[i].corrected == false)
                                {
                                    correct = false;
                                    break;
                                }
                            }
                            if (correct == true)
                            {
                                MeasureEnd();
                            }

                        }
                        else
                        {
                            glyphArray[chosenGlyphIndex].rigid.AddForce((glyphArray[chosenGlyphIndex].obj.transform.position - Vector3.zero).normalized * 20, ForceMode2D.Impulse);
                        }
                    }
                   

                    chosenGlyphIndex = -1;
                }
            }
            moveTimer += Time.deltaTime;
            if(moveTimer >= 2)
            {
                moveTimer = 0;
                MoveGlyphs();
            }
            BoundGlyphs();
            
        }
    }

    public override void ToolActive(bool active)
    {
        base.ToolActive(active);
        measureStarted = true;

        if(measureEnd == false)
        {
            if (isAuto == true)
            {

                MeasureEnd();
            }
            else
            {
                if (active == true)
                {
                    MoveGlyphs();
                }
            }
        }
 
        StartCoroutine(SceneManager.inst.MoveModule_Linear(toolObject, Vector3.zero, 2));
    }

    public override void OnNewVisitor(int symptomNum, int index, bool auto)
    {
        base.OnNewVisitor(symptomNum, index, auto);
        symptomText.gameObject.SetActive(false);
        if (isAuto)
        {
            MeasureEnd();
        }
        else
        {
            GenerateGlyphs();
            MoveGlyphs();
        }
    }

    void MoveGlyphs()
    {
        for(int i = 0; i < glyphArray.Length; i++)
        {
            if(i == chosenGlyphIndex)
            {
                glyphArray[i].rigid.velocity = Vector2.zero;
            }
            glyphArray[i].rigid.velocity = (new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f))).normalized;
        }
    }

    void BoundGlyphs()
    {
        for (int i = 0; i < glyphArray.Length; i++)
        {
            float x = glyphArray[i].obj.transform.position.x;
            float y = glyphArray[i].obj.transform.position.y;
            if (glyphArray[i].obj.transform.position.x > 9)
            {
                glyphArray[i].obj.transform.position = new Vector3(9, y, 0);
            }
            else if (glyphArray[i].obj.transform.position.x < -9)
            {
                glyphArray[i].obj.transform.position = new Vector3(-9, y, 0);
            }
            if(glyphArray[i].obj.transform.position.y > 7)
            {
                glyphArray[i].obj.transform.position = new Vector3(x, 7, 0);
            }
            else if (glyphArray[i].obj.transform.position.y < -7)
            {
                glyphArray[i].obj.transform.position = new Vector3(x, -7, 0);
            }

        }
    }

    void GenerateGlyphs()
    {
        glyphArray = new Glyph[glyphNumber];
        correctGlyphType = Random.Range(0, 4);
        if(spriteArray[correctGlyphType] == null)
        {
            spriteArray[correctGlyphType] = Resources.Load<Sprite>("Glyph/" + correctGlyphType.ToString());
        }
        correctSprite.sprite = spriteArray[correctGlyphType];
        for (int i = 0; i < glyphNumber; i++)
        {
            glyphArray[i] = new Glyph();
            GameObject inst = Instantiate(glyphPrefab, glyphParent);
            inst.SetActive(true);
            if(i >= glyphNumber / 2)
            {
                inst.transform.localPosition = new Vector3(-7, i % (glyphNumber / 2) * (-2) + 5);
            }
            else
            {
                inst.transform.localPosition = new Vector3(7, i % (glyphNumber / 2) * (-2) + 5);
            }
            
            glyphArray[i].obj = inst;
            glyphArray[i].rigid = inst.GetComponent<Rigidbody2D>();
            glyphArray[i].renderer = inst.GetComponent<SpriteRenderer>();
            glyphArray[i].glyphType = i % 4;
            if(spriteArray[i%4] == null)
            {
                spriteArray[i % 4] = Resources.Load<Sprite>("Glyph/" + (i % 4).ToString());
            }
            glyphArray[i].renderer.sprite = spriteArray[i % 4];
            glyphArray[i].sprite = spriteArray[i % 4];
             
        }
    }

    protected override void ExplainLoad()
    {
        base.ExplainLoad();
        explainData = gameManager.jsonManager.ResourceDataLoad<MeasureToolExplain>("MeasureToolExplain/Earth");
        ExplainSet();
    }

    protected override void MeasureEnd()
    {
        base.MeasureEnd();
        if(glyphArray != null)
        {
            for (int i = 0; i < glyphArray.Length; i++)
            {
                glyphArray[i].obj.SetActive(false);
            }
        }

        symptomText.gameObject.SetActive(true);
        symptomText.text = symptomNumber.ToString();
    }
}
