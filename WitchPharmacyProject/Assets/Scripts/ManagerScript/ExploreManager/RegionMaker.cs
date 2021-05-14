using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionMaker : MonoBehaviour
{
    RegionIngame regionIngame;
    List<TileButtonClass> tileButtonList;
    public GameObject dumyPrefab;
    bool draw = false;

    // Start is called before the first frame update
    void Start()
    {
        int nowEdgeNodeIndex = 1;
        int nowMainNodeIndex = 0;
        int maxNodeNumber = 10;
        regionIngame = new RegionIngame(dumyPrefab,new GameObject().transform);
        tileButtonList = regionIngame.tileButtonList;
        TileButtonClass tileButtonClass;
        tileButtonClass = new TileButtonClass(true);
        tileButtonList.Add(tileButtonClass);

        /*
        for (int i = 1; i < maxNodeNumber; i++)
        {
            GameObject dumy = Instantiate(dumyPrefab);
            tileButtonClass = new TileButtonClass(dumyPrefab, null, false);
            tileButtonList.Add(tileButtonClass);
        }
        */
        while(nowMainNodeIndex < maxNodeNumber)
        {
            int startRand = Random.Range(1, 4);
            for (int j = 0; j < startRand; j++)
            {
                if(nowEdgeNodeIndex >= maxNodeNumber)
                {
                    break;
                }
                int rand = Random.Range(0, 4);
                int cost = Random.Range(1, 5);
                bool allEdged = true;
                for(int k = 0; k < 4; k++)
                {
                    if(!tileButtonList[nowMainNodeIndex].adjacentBoolArray[k])
                        allEdged = false;
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
                tileButtonList[nowMainNodeIndex].SetEdge(tileButtonList[nowEdgeNodeIndex],cost, (Edge)rand);
                tileButtonList[nowMainNodeIndex].tileButtonObject.SetActive(true);
                tileButtonList[nowEdgeNodeIndex].tileButtonObject.SetActive(true);
                tileButtonList[nowMainNodeIndex].tileButtonObject.GetComponentInChildren<TextMesh>().text = nowMainNodeIndex.ToString() + tileButtonList[nowMainNodeIndex].tileClass;
                tileButtonList[nowEdgeNodeIndex].tileButtonObject.GetComponentInChildren<TextMesh>().text = nowEdgeNodeIndex.ToString() + tileButtonList[nowEdgeNodeIndex].tileClass;
                if (tileButtonList[nowEdgeNodeIndex].tileButtonObject.transform.position == Vector3.zero)
                {
                    Vector3 pos;
                    switch ((Edge)rand)
                    {
                        case Edge.Left:
                            pos = new Vector3(-1, 0, 0);
                            break;
                        case Edge.Right:
                            pos = new Vector3(1, 0, 0);
                            break;
                        case Edge.Up:
                            pos = new Vector3(0, 1, 0);
                            break;
                        case Edge.Down:
                            pos = new Vector3(0, -1, 0);
                            break;
                        default:
                            pos = new Vector3(-1, 0, 0);
                            break;
                    }
                    pos = pos * cost;
                    tileButtonList[nowEdgeNodeIndex].tileButtonObject.transform.position = tileButtonList[nowMainNodeIndex].tileButtonObject.transform.position + pos;
                }
               
                //Gizmos.color = Color.blue;
                //Gizmos.DrawLine(tileButtonList[nowEdgeNodeIndex].tileButtonObject.transform.position, tileButtonList[nowMainNodeIndex].tileButtonObject.transform.position);
                nowEdgeNodeIndex++;
            }
            nowMainNodeIndex++;
            nowEdgeNodeIndex = nowMainNodeIndex+1;
        }
        draw = true;
    }

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
