using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

public class RegionManager : MonoBehaviour
{
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
    [SerializeField]
    List<AStar> oNodeList;
    [SerializeField]
    List<AStar> cNodeList;
    bool moveCoroutineRunning;
    int nowWitchIndex;

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
        TimeCost(0);
    }

    public void OnBackButton()
    {
        sceneManager.LoadScene("ExploreScene");
    }

    public void OnTileButton(int index)
    {
        if (moveCoroutineRunning)
        {
            return;
        }
        StartCoroutine(WitchMoveCor(index));
    }

    IEnumerator WitchMoveCor(int targetIndex)
    {
        moveCoroutineRunning = true;

        //List<TileButtonClass> adjacentList = tileButtonList[nowWitchIndex].adjacentTileList;
        //List<float> costList = tileButtonList[nowWitchIndex].adjacentCostList;
        FindPath(nowWitchIndex, targetIndex);
        for (int i = 0; i < pathIndex.Count; i++)
            Debug.Log(pathIndex[i]);
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
                yield return null;
            }
            TimeCost(cost);
        }
        nowWitchIndex = targetIndex;
        moveCoroutineRunning = false;

    }
    

    //A*알고리즘
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
                bool notIncluded = false;
                AStar node = new AStar();
                TileButtonClass nextTile = nowTile.adjacentTileList[i];

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
                        }
                        else
                        {
                            notIncluded = true;
                        }
                    }
                }
                if (!notIncluded)
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

            cNodeList.Add(oNodeList[minimumIndex]);
            nowTile = tileButtonList[oNodeList[minimumIndex].nodeID];
            if(cNodeList[cNodeList.Count-1].nodeID == targetIndex)
            {
                solved = true;
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


    //bool FindPath(int startIndex, int targetIndex, List<int> pathIndex, List<TileButtonClass> nowAdjacentList)
    //{
    //    pathIndex.Add(startIndex);
    //    for (int i = 0; i < nowAdjacentList.Count; i++)
    //    {
    //        if (nowAdjacentList[i].tileClass.index == targetIndex)
    //        {
    //            pathIndex.Add(targetIndex);
    //            return true;
    //        }
    //    }

    //    for (int i = 0; i < nowAdjacentList.Count; i++)
    //    {
    //        bool isEndNode = false;
    //        for (int k = 0; k < pathIndex.Count; k++)
    //        {
    //            if (nowAdjacentList[i].tileClass.index == pathIndex[k])
    //            {
    //                isEndNode = true;
    //            }
    //        }
    //        if (!isEndNode)
    //        {
    //            bool solved = FindPath(nowAdjacentList[i].tileClass.index, targetIndex,
    //            pathIndex, nowAdjacentList[i].adjacentTileList);
    //            if (!solved)
    //            {
    //                pathIndex.Remove(nowAdjacentList[i].tileClass.index);
    //            }
    //            else
    //            {
    //                return true;
    //            }
    //        }
    //    }
    //    pathIndex.Remove(startIndex);
    //    return false;
    //}


    void TimeCost(float cost)
    {
        //시간 넣어주는거.
        cost = cost - cost % 60;
        exploreManager.TimeChange(cost);
        int hour = (int)exploreManager.nowTime / 3600;
        int minute = ((int)exploreManager.nowTime % 3600) / 60;
        StringBuilder builder = new StringBuilder(hour.ToString());
        builder.Append("시");
        builder.Append(minute.ToString());
        builder.Append("분");
        timeText.text = builder.ToString();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
