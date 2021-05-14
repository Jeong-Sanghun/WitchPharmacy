using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegionMaker : MonoBehaviour
{
    public RegionIngame regionIngame;
    List<TileButtonClass> tileButtonList;
    [SerializeField]
    GameObject dumyPrefab;
    [SerializeField]
    LineRenderer dumyEdge;
    [SerializeField]
    Transform canvas;

    
    //기즈모 그리기위한 더미변수
    bool draw = false;

    // Start is called before the first frame update
    void Start()
    {

        EdgeMaker();

    }

    void EdgeMaker()
    {

        int nowEdgeNodeIndex = 1;
        int nowMainNodeIndex = 0;
        int maxNodeNumber = RegionIngame.tileNumber;
        regionIngame = new RegionIngame(dumyPrefab, canvas);
        tileButtonList = regionIngame.tileButtonList;
        TileButtonClass tileButtonClass;
        tileButtonClass = new TileButtonClass(true);
        tileButtonList.Add(tileButtonClass);

        //모든 노드를 다 돌아본다.
        while (nowMainNodeIndex < maxNodeNumber)
        {
            //그 노드에서 몇 개가 뻗어나갈 것인지 체크.
            int startRand = Random.Range(1, TileButtonClass.edgeNumber+1) - tileButtonList[nowMainNodeIndex].nowEdgeNumber;
            if(nowMainNodeIndex == 0)
            {
                startRand = 8;
            }
            for (int j = 0; j < startRand; j++)
            {
                if (nowEdgeNodeIndex >= maxNodeNumber)
                {
                    break;
                }
                int rand = Random.Range(0, TileButtonClass.edgeNumber);
                int cost = Random.Range(1, TileButtonClass.maxCost+1);
                bool allEdged = true;
                for (int k = 0; k < 4; k++)
                {
                    if (!tileButtonList[nowMainNodeIndex].adjacentBoolArray[k])
                    {
                        allEdged = false;
                        break;
                    }
                        
                }
                if (allEdged)
                {
                    break;
                }
                if (tileButtonList[nowMainNodeIndex].adjacentBoolArray[rand])
                {
                    j--;
                    continue;
                }
                tileButtonList[nowMainNodeIndex].SetEdge(tileButtonList[nowEdgeNodeIndex], cost, (Edge)rand);
                tileButtonList[nowMainNodeIndex].tileButtonObject.SetActive(true);
                tileButtonList[nowEdgeNodeIndex].tileButtonObject.SetActive(true);
                tileButtonList[nowMainNodeIndex].tileButtonObject.GetComponentInChildren<Text>().text = nowMainNodeIndex.ToString() + tileButtonList[nowMainNodeIndex].tileClass;
                tileButtonList[nowEdgeNodeIndex].tileButtonObject.GetComponentInChildren<Text>().text = nowEdgeNodeIndex.ToString() + tileButtonList[nowEdgeNodeIndex].tileClass;
                if (tileButtonList[nowEdgeNodeIndex].tileButtonObject.GetComponent<RectTransform>().anchoredPosition == Vector2.zero)
                {
                    Vector2 pos;
                    switch ((Edge)rand)
                    {
                        case Edge.Left:
                            pos = new Vector3(-1, 0);
                            break;
                        case Edge.Right:
                            pos = new Vector3(1, 0);
                            break;
                        case Edge.Up:
                            pos = new Vector3(0, 1);
                            break;
                        case Edge.Down:
                            pos = new Vector3(0, -1);
                            break;
                        case Edge.UpLeft:
                            pos = new Vector3(-0.5f, 0.5f);
                            break;
                        case Edge.DownRight:
                            pos = new Vector3(0.5f, -0.5f);
                            break;
                        case Edge.UpRight:
                            pos = new Vector3(0.5f, 0.5f);
                            break;
                        case Edge.DownLeft:
                            pos = new Vector3(-0.5f, -0.5f);
                            break;
                        default:
                            pos = new Vector3(-1, 0);
                            break;
                    }
                    pos = pos * cost * 100;
                    tileButtonList[nowEdgeNodeIndex].tileButtonObject.GetComponent<RectTransform>().anchoredPosition = tileButtonList[nowMainNodeIndex].tileButtonObject.GetComponent<RectTransform>().anchoredPosition + pos;
                }
                Vector3[] arr = new Vector3[2];
                arr[0] = tileButtonList[nowMainNodeIndex].tileButtonObject.transform.position;
                arr[1] = tileButtonList[nowEdgeNodeIndex].tileButtonObject.transform.position;
                GameObject edgeObj = GameObject.Instantiate(dumyEdge.gameObject);
                edgeObj.SetActive(true);
                edgeObj.GetComponent<LineRenderer>().SetPositions(arr);
                
                nowEdgeNodeIndex++;
            }
            nowMainNodeIndex++;
            nowEdgeNodeIndex = nowMainNodeIndex + 1;
        }
        draw = true;
    }

    /*
    void OnDrawGizmos()
    {
        if (draw)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            for(int i = 0; i < 10; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(tileButtonList[i].adjacentBoolArray[j] == false)
                    {
                        continue;
                    }
                    Gizmos.DrawLine(tileButtonList[i].tileButtonObject.transform.position, tileButtonList[i].adjacentTileArray[j].tileButtonObject.transform.position) ;
                }
                
            }

        }
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}
