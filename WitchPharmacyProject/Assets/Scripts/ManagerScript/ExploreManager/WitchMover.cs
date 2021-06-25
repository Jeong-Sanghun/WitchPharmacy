using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class WitchMover : MonoBehaviour
{

    //이동하자마자 열어줘야돼서 그럼
    [SerializeField]
    RegionManager regionManager;

    SceneManager sceneManager;
    //타임키퍼 역할을 해줘야함.
    ExploreManager exploreManager;
    RegionIngame regionIngame;
    [SerializeField]
    RectTransform witchRect;
    [SerializeField]
    Text timeText;
    List<int> pathIndex;
    List<TileButtonClass> tileButtonList;
    List<AStar> oNodeList;
    List<AStar> cNodeList;
    [SerializeField]
    GameObject tileOpenButton;
    bool moveCoroutineRunning;
    int nowWitchIndex;

    //RegionManager에서 타일매니저로 넘겨줌
    public TileButtonClass nowTileButton;

    [System.Serializable]
    struct AStar
    {
        public float fScore;
        public float gScore;
        public float hScore;
        public int parentNode;
        public int nodeID;
        
    }
    void Start()
    {
        moveCoroutineRunning = false;
        nowWitchIndex = 0;
        sceneManager = SceneManager.inst;
        exploreManager = ExploreManager.inst;
        regionIngame = exploreManager.GetRegionIngame();
        tileButtonList = regionIngame.tileButtonList;
        //버튼 이어주는거.
        for(int i = 0; i < regionIngame.tileButtonList.Count; i++)
        {
            int delegateIndex = i;
            Button button = regionIngame.tileButtonList[i].tileButtonObject.GetComponent<Button>();
            button.onClick.AddListener(() => OnTileButton(delegateIndex));
        }
        witchRect.transform.SetAsLastSibling();
        //타임코스트는 텍스트 업뎃해줄라고.
        TimeCost(0);
        //이거는 디버그할때 오류나길래
        nowTileButton = tileButtonList[0];

        //0번 어웨어해야함.
        AwareTile(tileButtonList[0]);

        //이거는 regionIngame이 exploreManager에서 저장되어잇으니까 그거 aware되어있는건 aware해야됨.
        for(int i = 1; i < tileButtonList.Count; i++)
        {
            if (tileButtonList[i].awared)
            {
                AwareTile(tileButtonList[i]);
            }
        }
    }


    public void OnTileButton(int index)
    {
        if (moveCoroutineRunning)
        {
            return;
        }
        if(tileButtonList[index] == nowTileButton)
        {
            return;
        }
        StartCoroutine(WitchMoveCor(index));
    }

    IEnumerator WitchMoveCor(int targetIndex)
    {
        moveCoroutineRunning = true;
        tileOpenButton.SetActive(false);

        //List<TileButtonClass> adjacentList = tileButtonList[nowWitchIndex].adjacentTileList;
        //List<float> costList = tileButtonList[nowWitchIndex].adjacentCostList;
        FindPath(nowWitchIndex, targetIndex);

        for (int i= 0; i < pathIndex.Count-1; i++)
        {
            float timer = 0;
            float cost =0;
            Vector2 startPos = new Vector2(tileButtonList[pathIndex[i]].xPos, tileButtonList[pathIndex[i]].yPos);
            Vector2 endPos = new Vector2(tileButtonList[pathIndex[i + 1]].xPos, tileButtonList[pathIndex[i + 1]].yPos);
            cost = Mathf.Sqrt(Vector2.SqrMagnitude(startPos - endPos));
            while (timer < 1)
            {
                timer += Time.deltaTime * 300/cost;
                witchRect.anchoredPosition = Vector2.Lerp(startPos, endPos, timer);
                //중간에 들어서면 보여주기

                yield return null;
            }
            TimeCost(cost);
        }
        nowWitchIndex = targetIndex;
        moveCoroutineRunning = false;

        //if(tileButtonList[targetIndex].tileClass.tileType != TileType.StartTile)
        //{
        //    tileOpenButton.SetActive(true);
        //}

        nowTileButton = tileButtonList[targetIndex];
        //if (tileButtonList[targetIndex].tileClass.tileType == TileType.MedicineTile)
        //{
        //    tileOpenButton.SetActive(true);
        //}
        TileOpen();

    }
    

    //A*알고리즘 뭐 읽을필요없음.
    void FindPath(int startIndex, int targetIndex)
    {
        //미확정
        oNodeList = new List<AStar>();
        //확정
        cNodeList = new List<AStar>();
        bool solved = false;
        AStar startNode = new AStar();
        startNode.nodeID = startIndex;
        startNode.parentNode = -1;
        cNodeList.Add(startNode);
        TileButtonClass nowTile = tileButtonList[startIndex];
        TileButtonClass targetTile = tileButtonList[targetIndex];
        while (!solved)
        {
            for (int i = 0; i < nowTile.adjacentTileList.Count; i++)
            {
                //이거는 열어본 타일만 가려고.
                if(nowTile.adjacentTileList[i].isOpened == false)
                {
                    continue;
                }
                TileButtonClass nextTile = nowTile.adjacentTileList[i].adjacentTileButton;
                bool cont = false;
                for(int j = 0; j < cNodeList.Count; j++)
                {
                    if(nextTile.tileClass.index == cNodeList[j].nodeID)
                    {
                        cont = true;
                        break;
                    }
                }
                if (cont)
                    continue;

                bool included = false;
                AStar node = new AStar();


                node.nodeID = nextTile.tileClass.index;
                node.gScore = nowTile.adjacentCostList[i];
                node.hScore = Mathf.Sqrt((nextTile.xPos - targetTile.xPos) * (nextTile.xPos - targetTile.xPos)
                    + (nextTile.yPos - targetTile.yPos) * (nextTile.yPos - targetTile.yPos));
                node.fScore = node.gScore + node.hScore;
                node.parentNode = nowTile.tileClass.index;

                for(int j = 0; j < oNodeList.Count; j++)
                {
                    if(node.nodeID == oNodeList[j].nodeID)
                    {
                        if(node.fScore < oNodeList[j].fScore)
                        {
                            oNodeList.RemoveAt(j);
                            oNodeList.Add(node);
                            included = true;
                            break;
                        }
                    }
                }
                if (!included && node.nodeID != startIndex)
                {
                    oNodeList.Add(node);
                }
            }

            float minimum = oNodeList[0].fScore;
            int minimumIndex = 0;
            for(int i = 1; i < oNodeList.Count; i++)
            {
                if(minimum > oNodeList[i].fScore)
                {
                    minimum = oNodeList[i].fScore;
                    minimumIndex = i;
                }
            }
            nowTile = tileButtonList[oNodeList[minimumIndex].nodeID];
            if(oNodeList[minimumIndex].nodeID == targetIndex)
            {
                solved = true;
                cNodeList.Add(oNodeList[minimumIndex]);
            }
            else
            {
                if (nowTile.adjacentTileList.Count > 1)
                {
                    cNodeList.Add(oNodeList[minimumIndex]);
                }
            }
            oNodeList.RemoveAt(minimumIndex);

        }
        List<AStar> stack = new List<AStar>();
        pathIndex = new List<int>();
        solved = false;
        stack.Add(cNodeList[cNodeList.Count - 1]);
        while (!solved)
        {
            for (int i = 0; i < cNodeList.Count; i++)
            {
                if (cNodeList[i].nodeID == stack[stack.Count - 1].parentNode)
                {
                    stack.Add(cNodeList[i]);
                }
                if (stack[stack.Count - 1].nodeID == startIndex)
                {
                    solved = true;
                }
            }

        }


        for (int i = stack.Count - 1; i >= 0; i--)
        {
            pathIndex.Add(stack[i].nodeID);
        }


    }



    void TimeCost(float cost)
    {
        //시간 넣어주는거.
        cost = cost - cost % 60;
        exploreManager.TimeChange(cost);

    }

    public void TileOpen()
    {
        TileButtonClass nowTile = tileButtonList[nowWitchIndex];
        if (!nowTile.opened)
        {
            regionManager.OnTileOpenButton();
        }
        else
        {
            if (nowTile.tileClass.tileType == TileType.StartTile)
            {
                tileOpenButton.SetActive(false);
            }
            else
            {
                tileOpenButton.SetActive(true);
            }
        }
        
    }

    //전장의 안개 걷히는거
    //이거 regionManager에서 관리해줘야함
    public void AwareTile(TileButtonClass nextTile)
    {
        //if (nextTile.awared)
        //{
        //    return;
        //}
        nextTile.awared = true;
        for(int i = 0; i < nextTile.adjacentTileList.Count; i++)
        {

            for(int j = 0; j < nextTile.adjacentTileList[i].adjacentTileButton.adjacentTileList.Count; j++)
            {
                TileButtonAdjacent tile = nextTile.adjacentTileList[i].adjacentTileButton.adjacentTileList[j];
                if(tile.isOpened == true)
                {
                    continue;
                }
                if(tile.adjacentTileButton.tileClass.index == nextTile.tileClass.index)
                {
                    tile.isOpened = true;
                }
            }
            nextTile.adjacentTileList[i].isOpened = true;
            if (!nextTile.adjacentTileList[i].adjacentTileButton.tileButtonObject.activeSelf)
            {
                nextTile.adjacentTileList[i].adjacentTileButton.tileButtonObject.SetActive(true);
                StartCoroutine(sceneManager.FadeModule_Image(nextTile.adjacentTileList[i].adjacentTileButton.tileButtonObject, 0, 1, 600 / nextTile.adjacentCostList[i]));

            }
            nextTile.adjacentLineList[i].SetActive(true);

            
        }
        
    }


    private void Update()
    {
        //디버그용
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AwareTile(nowTileButton);
        }
        
    }

}
