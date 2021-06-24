using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//버튼 리스너는 RegionManager에서 해준다.
//얘는 에지 이어주는거까지만 해줌.
public class RegionMaker : MonoBehaviour
{
    ExploreManager exploreManager;
    public RegionIngame regionIngame;
    public List<TileButtonClass> tileButtonList;
    [SerializeField]
    GameObject dumyPrefab;
    [SerializeField]
    LineRenderer dumyEdge;
    [SerializeField]
    Transform canvas;
    bool[] edgedBool;


    //기즈모 그리기위한 더미변수
    //bool draw = false;

    // Start is called before the first frame update

    private void Start()
    {
        exploreManager = ExploreManager.inst;
        //이지랄을 해놓은 이유는 Load한다음에 FInd가 먹지를 않음. Start에서 해줘야함.
        exploreManager.OnRegionLoaded(this);
    }

    //이거 ExploreManager에서 불러옴. OnRegionLoaded에서.
    public RegionIngame EdgeMaker(RegionIngame region,RegionProperty property)
    {
        if (region != null)
        {
            regionIngame = region;
            tileButtonList = regionIngame.tileButtonList;
            //이거 상수임. 포지션에 얼마나 곱해줄것인지
            //position set하기
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                tileButtonList[i].tileButtonObject = Instantiate(dumyPrefab, canvas);
                tileButtonList[i].tileButtonObject.SetActive(false);
                tileButtonList[i].tileButtonObject.GetComponent<RectTransform>().anchoredPosition
                    = new Vector2(tileButtonList[i].xPos, tileButtonList[i].yPos);
                tileButtonList[i].adjacentLineList.Clear();
            }
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                //이거는 에지 라인 만들어주는거.
                for (int j = 0; j < tileButtonList[i].adjacentTileList.Count; j++)
                {
                    Vector3[] arr = new Vector3[2];
                    arr[0] = tileButtonList[i].tileButtonObject.transform.position;
                    arr[1] = tileButtonList[i].adjacentTileList[j].adjacentTileButton.tileButtonObject.transform.position;
                    GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject, canvas);
                    tileButtonList[i].adjacentLineList.Add(edgeObj);
                    edgeObj.SetActive(false);
                    edgeObj.GetComponent<LineRenderer>().SetPositions(arr);
                }
            }
            tileButtonList[0].tileButtonObject.SetActive(true);
        }
        else
        {
            regionIngame = new RegionIngame(property,dumyPrefab, canvas);
            tileButtonList = regionIngame.tileButtonList;

            //TileButtonClass tileButtonClass;
            //tileButtonClass = new TileButtonClass(true);
            //tileButtonList.Add(tileButtonClass);

            float positionConstant = 300;
            //이거 상수임. 포지션에 얼마나 곱해줄것인지
            //position set하기
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                float x, y;

                if (i == 0)
                {
                    x = 0;
                    y = 0;
                }
                else
                {
                    if ((i - 1) / 3 == 0)
                    {
                        x = positionConstant;
                    }
                    else
                    {
                        x = ((i-1)/3 + (1 + Random.Range(0,TileButtonClass.maxCost))) * positionConstant;
                        //x = (tileButtonList[i - 3].xPos) + (1) * positionConstant;
                    }

                    //나중에 여기다가 상수 곱해줘야함.
                    y = (i % 3 - 1) * positionConstant;
                }
                tileButtonList[i].SetPosition(x, y);
                tileButtonList[i].tileButtonObject.GetComponent<RectTransform>().anchoredPosition
                    = new Vector2(tileButtonList[i].xPos, tileButtonList[i].yPos);
            }

            //모든 노드를 다 돌아본다. edgedBool기준 트래벌스하기.
            edgedBool = new bool[RegionIngame.tileNumber];
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                edgedBool[i] = false;
            }
            //이거는 트래벌스 아님. 랜덤생성
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                RecursionEdge(i, false);
            }

            //이거는 트래벌스. 빠진게 있으면 넣어주는거.
            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                RecursionEdge(i, true);
            }

            for (int i = 0; i < RegionIngame.tileNumber; i++)
            {
                tileButtonList[i].tileButtonObject.SetActive(false);
            }
            tileButtonList[0].tileButtonObject.SetActive(true);
        }

        //for (int i = 0; i < RegionIngame.tileNumber; i++)
        //{
        //    //이거는 에지 라인 만들어주는거.
        //    tileButtonList[i].tileButtonObject.SetActive(true);
        //    for (int j = 0; j < tileButtonList[i].adjacentTileList.Count; j++)
        //    {
        //        Vector3[] arr = new Vector3[2];
        //        arr[0] = tileButtonList[i].tileButtonObject.transform.position;
        //        arr[1] = tileButtonList[i].adjacentTileList[j].tileButtonObject.transform.position;
        //        GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject, canvas);
        //        edgeObj.SetActive(true);
        //        edgeObj.GetComponent<LineRenderer>().SetPositions(arr);
        //    }

        //}

        //이거 더미임. 스프라이트로 바꿔야함 나중에.
        for(int i = 0; i < RegionIngame.tileNumber; i++)
        {
            Image image = tileButtonList[i].tileButtonObject.GetComponent<Image>();
            int nowTypeIndex = (int)tileButtonList[i].tileClass.tileType;
            Color buttonColor;
            //Debug.Log(nowTypeIndex);
            switch (nowTypeIndex)
            {
                //StartTile, MedicineTile, StoreTile, StoryTile,
                //TreasureTile, TrapTile, BarrierTile, SpecialStoreTile
                case 0:
                    buttonColor = Color.white;
                    break;
                case 1:
                    buttonColor = Color.black;
                    break;
                case 2:
                    buttonColor = Color.red;
                    break;
                case 3:
                    buttonColor = Color.cyan;
                    break;
                case 4:
                    buttonColor = Color.blue;
                    break;
                case 5:
                case 6:
                    buttonColor = new Color(0.3f, 0.3f, 1);
                    break;
                case 7:
                    buttonColor = Color.green;
                    break;
                default:
                    buttonColor = new Color(0.5f,0.5f,0.5f);
                    break;
            }
            image.color = buttonColor;
        }


        //이걸 ExplorerManager에서 받아서 재탕할 때 씀.
        return regionIngame;

    }


    //에지 연결해주는건데... 그냥 그러려니 해...
    void RecursionEdge(int index, bool isTraverse)
    {
        int forLoopEndIndex;
        int forLoopStartIndex;
        //내 바로앞에있는 에지만 연결할 수 있게함.
        if (index == 0)
        {
            forLoopStartIndex = 0;
            forLoopEndIndex = 3;
        }
        else
        {
            forLoopStartIndex = ((index - 1) / 3)*3 + 1;
            forLoopEndIndex = forLoopStartIndex + 5;
            if (forLoopEndIndex > RegionIngame.tileNumber-1)
            {
                forLoopEndIndex = RegionIngame.tileNumber - 1;
            }
            //만약 트래벌스인데 에지가 없으면 그대로 연결해줌.
            
        }
        if (isTraverse)
        {
            if (!edgedBool[index])
            {
                //이게 트래벌스.
                int rand =0;
                bool goAhead = true;
                do
                {
                    if ((index - 1) / 3 == 0)
                    {
                        rand = 0;
                    }
                    else
                    {
                        //뒷라인에서땡겨줌.
                        rand = Random.Range(forLoopStartIndex - 3, forLoopStartIndex);
                    }
                    if ((index % 3 == 2 && rand % 3 == 0 && rand - index == 1))
                    {
                        goAhead = false;
                    }
                    else
                    {
                        if ((index % 3 == 0 && rand % 3 == 2 && index - rand == 1))
                        {
                            goAhead = false;
                        }
                        else
                        {
                            goAhead = true;
                        }
                        
                    }
                } while (!goAhead) ;


                    edgedBool[index] = true;
                Vector3[] arr = new Vector3[2];
                arr[0] = tileButtonList[rand].tileButtonObject.transform.position;
                arr[1] = tileButtonList[index].tileButtonObject.transform.position;
                GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject, canvas);
                edgeObj.SetActive(false);
                edgeObj.GetComponent<LineRenderer>().SetPositions(arr);

                      //이거1 번에서 3번 꽂는거 방지.



                tileButtonList[rand].SetEdge(tileButtonList[index], edgeObj);


            }
            return;
        }
        if (tileButtonList[index].tileClass.tileType == TileType.SpecialStoreTile)
        {
            return;
        }
        //이 위까지가 트래벌스.
        for (int i = forLoopStartIndex; i <= forLoopEndIndex; i++)
        {
            //이건 랜덤연결해주는거. t
            if (index == i)// || edgedBool[i])
            {
                continue;
            }

            bool continueOrNot;
            int randomRange;
            if (edgedBool[i])
            {
                randomRange = 4;
            }
            else
            {
                randomRange = 2;
            }
            if(Random.Range(0,randomRange) == 0)
            {
                continueOrNot = false;
            }
            else
            {
                continueOrNot = true;
            }
            if(index == 0 && i == forLoopEndIndex && !edgedBool[0])
            {
                //이게 0번이랑 연결 안됐을 떄 연결해주는거.
                edgedBool[i] = true;

                Vector3[] arr = new Vector3[2];
                arr[0] = tileButtonList[index].tileButtonObject.transform.position;
                arr[1] = tileButtonList[i].tileButtonObject.transform.position;
                GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject, canvas);
                edgeObj.SetActive(false);
                edgeObj.GetComponent<LineRenderer>().SetPositions(arr);

                tileButtonList[index].SetEdge(tileButtonList[i],edgeObj);
            }
            else
            {
                if (continueOrNot)// && !isTraverse)
                {
                    continue;
                }
                if (index == 0)
                {
                    edgedBool[0] = true;
                }

                //이거1 번에서 3번 꽂는거 방지.
                if ((index % 3 == 2 && i % 3 == 0 && i - index == 1))
                {
                    return;
                }

                if ((index % 3 == 0 && i % 3 == 2 && index - i == 1))
                {
                    return;
                }

                edgedBool[i] = true;

                Vector3[] arr = new Vector3[2];
                arr[0] = tileButtonList[index].tileButtonObject.transform.position;
                arr[1] = tileButtonList[i].tileButtonObject.transform.position;
                GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject, canvas);
                edgeObj.SetActive(false);
                edgeObj.GetComponent<LineRenderer>().SetPositions(arr);

                tileButtonList[index].SetEdge(tileButtonList[i],edgeObj);
                if (!edgedBool[index])
                {
                    RecursionEdge(index, true);
                }
            }

        }
    }

}
