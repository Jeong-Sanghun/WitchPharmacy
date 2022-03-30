using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using System;

public class BlurManager : MonoBehaviour
{
    [SerializeField]
    GameObject backGround;
    [SerializeField]
    PostProcessProfile postProcessProfile;
    
    DepthOfField depthOfField;

    const int blurLayer = 8;
    const int defaultLayer = 0;
    bool runToggle = false;
    // Start is called before the first frame update
    void Start()
    {
        //backGround.layer =0;
        depthOfField = postProcessProfile.GetSetting<DepthOfField>();
    }

    public void OnBlur(bool blur)
    {
        StartCoroutine(BlurCoroutine(blur,null));
    }

    public void OnBlur(bool blur, Action afterAction)
    {
        StartCoroutine(BlurCoroutine(blur,afterAction));
    }
    bool running = false;

    public void ChangeLayer(bool blur,GameObject obj)
    {
        int targetLayer;
        if (blur)
        {
            targetLayer = blurLayer;
        }
        else
        {
            targetLayer = defaultLayer;
        }

        RecursiveLayerChange(targetLayer, obj.transform);
    }

    void RecursiveLayerChange(int layer, Transform obj)
    {
        obj.gameObject.layer = layer;
        if (obj.childCount == 0)
        {
            return;
        }
        else
        {
            for(int i = 0; i < obj.childCount; i++)
            {
                RecursiveLayerChange(layer, obj.GetChild(i));
            }
        }
    }

    IEnumerator BlurCoroutine(bool blur, Action afterAction)
    {
        runToggle = !runToggle;
        bool nowRunToggle = runToggle;
        float lerp;
        float hit;
        if (blur == true)
        {
            lerp = 4;
            hit = 0.5f;

            while (lerp >= hit)
            {
                if (nowRunToggle != runToggle)
                {
                    break;
                }
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
                if (nowRunToggle != runToggle)
                {
                    break;
                }
                lerp += Time.deltaTime * 4;
                depthOfField.focusDistance.value = lerp;

                yield return null;
            }

        }
        if (nowRunToggle == runToggle)
        {
            depthOfField.focusDistance.value = hit;
        }

        if(afterAction != null)
        {
            afterAction();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
