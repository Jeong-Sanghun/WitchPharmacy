using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class BlurManager : MonoBehaviour
{
    [SerializeField]
    GameObject backGround;
    [SerializeField]
    PostProcessProfile postProcessProfile;
    
    DepthOfField depthOfField;

    const int blurLayer = 8;
    const int defaultLayer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //backGround.layer =0;
        depthOfField = postProcessProfile.GetSetting<DepthOfField>();
    }

    public void OnBlur(bool blur)
    {
        StartCoroutine(BlurCoroutine(blur));
    }
    bool running = false;

    IEnumerator BlurCoroutine(bool blur)
    {
        if(running == true)
        {
            Debug.LogError("좆ㅅ돼따");
            
        }
        else
        {
            running = true;
            float lerp;
            float hit;
            if (blur == true)
            {
                lerp = 4;
                hit = 0.5f;

                while (lerp >= hit)
                {
                    lerp -= Time.deltaTime * 4;
                    depthOfField.focusDistance.value = lerp;
                    yield return null;
                }
            }
            else
            {
                lerp = 0.5f;
                hit = 4;
                while (lerp <= hit)
                {
                    lerp += Time.deltaTime * 4;
                    depthOfField.focusDistance.value = lerp;
                    yield return null;
                }

            }
            depthOfField.focusDistance.value = hit;
            running = false;
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
