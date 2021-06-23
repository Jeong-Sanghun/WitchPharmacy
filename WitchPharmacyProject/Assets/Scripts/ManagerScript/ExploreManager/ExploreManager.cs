using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text;

//지역탐색에 들어간 후 지역을 고를 때 나옴.
public class ExploreManager : MonoBehaviour
{
    const int regionQuantity = 10;
    public static ExploreManager inst;
    GameManager gameManager;
    SceneManager sceneManager;
    RegionMaker regionMaker;
    RegionPropertyWrapper regionPropertyWrapper;
    RegionIngame[] regionIngameArray;
    //bool[] visitedRegionArray;
    int nowIndex;

    //이거 타일매니저로 넘겨줘야되는데 regionManager에서 타일매니저로 넘겨줌
    public RegionProperty nowProperty;
    public RegionIngame nowRegionIngame;

    //이거 단위 초임.
    public float nowTime;

    Text timeText;
    
    // Start is called before the first frame update
    void Awake()
    {
        if(inst == null)
        {
            inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        sceneManager = SceneManager.inst;
        gameManager = GameManager.singleTon;
        regionPropertyWrapper = gameManager.regionPropertyWrapper;

        //이거도 더미. 나중에는 제이슨으로 받아올건데 지금은 임시로 램덤.
        //int roundRobin;

        //for (int j = 0; j < regionQuantity; j++)
        //{
        //    regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[0] = 1;
        //    for (int k = 1; k < 8; k++)
        //    {
        //        regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[k] = 0;
        //    }
        //}


        //for (int j = 0; j < regionQuantity; j++)
        //{
        //    for (int i = 1; i < RegionIngame.tileNumber; i++)
        //    {
        //        roundRobin = Random.Range(1, 8);
        //        regionPropertyWrapper.regionPropertyArray[j].tileTypeArray[roundRobin]++;
        //    }
        //}
        
        regionIngameArray = new RegionIngame[regionQuantity];
        //이거는 굳이 필요없을지도..
        //visitedRegionArray = new bool[regionQuantity];
        //for(int i = 0; i < regionQuantity; i++)
        //{
        //    visitedRegionArray[i] = false;
        //}

    }

    //버튼 눌렸을 때 ExploreButtonManager에서 불러옴.
    public void OnRegionLoad(int index)
    {
        nowIndex = index;
        nowProperty = regionPropertyWrapper.regionPropertyArray[nowIndex];
        nowRegionIngame = regionIngameArray[nowIndex];
        sceneManager.LoadScene("RegionScene");

    }

    //지역 로드하면 만들어줘야댐. 이거 RegionMaker에서 Start로 불러옴.
    public void OnRegionLoaded(RegionMaker _regionMaker)
    {
        regionMaker = _regionMaker;
        timeText = GameObject.Find("TimeText").GetComponent<Text>();
        if (regionIngameArray[nowIndex] == null)
        {
            regionIngameArray[nowIndex] = regionMaker.EdgeMaker(null, regionPropertyWrapper.regionPropertyArray[nowIndex]);
        }
        else
        {
            regionMaker.EdgeMaker(regionIngameArray[nowIndex], regionPropertyWrapper.regionPropertyArray[nowIndex]);
        }

    }

    //이거 버튼누를때마다 불러옴.
    //이거 단위 초다.
    public void TimeChange(float plusTime)
    {
        nowTime += plusTime;

        int hour = (int)nowTime / 3600;
        int minute = ((int)nowTime % 3600) / 60;
        StringBuilder builder = new StringBuilder(hour.ToString());
        builder.Append("시");
        builder.Append(minute.ToString());
        builder.Append("분");
        timeText.text = builder.ToString();

    }

    //이거 RegionManager에서 불러옴.
    public RegionIngame GetRegionIngame()
    {
        return regionIngameArray[nowIndex];
    }

}
