using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreButtonManager : MonoBehaviour
{
    ExploreManager exploreManager;
    // Start is called before the first frame update
    void Start()
    {
        exploreManager = ExploreManager.inst;
    }

    public void OnButtonLoad(int index)
    {
        exploreManager = ExploreManager.inst;
        exploreManager.OnRegionLoad(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
