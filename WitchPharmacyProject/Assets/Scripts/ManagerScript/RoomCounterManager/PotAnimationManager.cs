using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Coffee.UIExtensions;

public class PotAnimationManager : MonoBehaviour
{
    [SerializeField]
    Animator potWorldAnimation;
    [SerializeField]
    Animator potUIAnimation;
    // Start is called before the first frame update
    [SerializeField]
    UIGradient gradient;
    [SerializeField]
    UIGradient bottleGradient;
    [SerializeField]
    GameObject stopPotFront;
    [SerializeField]
    GameObject movingPotFront;
    [SerializeField]
    Color[] symptomColorArray;


    public void PotWorldAnimation(bool active)
    {
        potWorldAnimation.SetBool("isCooking", active);
        
    }

    public void PotUIAnimation(bool active)
    {
        potUIAnimation.SetBool("Boil", active);
        stopPotFront.SetActive(!active);
        movingPotFront.SetActive(active);
    }

    public void PotDropAnimation()
    {
        potUIAnimation.SetTrigger("Drop");
    }

    public void SetPotColor(Symptom symptom, int symptomNumber,int index)
    {
        //Texture2D tex = sprite.texture;
        //Color32[] texColors = sprite.texture.GetPixels32();

        //int total = texColors.Length;

        //float r = 0;
        //float g = 0;
        //float b = 0;
        //float totalWithoutAlpha = total;

        //for (int i = 0; i < total; i++)
        //{
        //    if (texColors[i].a == 0)
        //    {
        //        totalWithoutAlpha--;
        //        continue;
        //    }
        //    r += texColors[i].r;

        //    g += texColors[i].g;

        //    b += texColors[i].b;

        //}

        //Color32 color = new Color32((byte)(r*1.2f / totalWithoutAlpha), (byte)(g*1.2f / totalWithoutAlpha), (byte)(b*1.2f / totalWithoutAlpha), 255);
        Color color;
        int arrayIndex = (int)symptom * 2;
        if(symptomNumber == 2)
        {
            arrayIndex++;
        }
        color = symptomColorArray[arrayIndex];
        switch (index)
        {
            case 0:
                bottleGradient.color1 = color;
                gradient.color1 = color;
                break;
            case 1:
                bottleGradient.color2 = color;
                gradient.color2 = color;
                break;
            case 2:
                bottleGradient.color3 = color;
                gradient.color3 = color;
                break;
        }
    }

    public void UnSetPotColor(int index,bool bottle)
    {
        switch (index)
        {
            case 0:
                if(bottle)
                    bottleGradient.color1 = Color.white;
                gradient.color1 = Color.white;
                break;
            case 1:
                if (bottle)
                    bottleGradient.color2 = Color.white;
                gradient.color2 = Color.white;
                break;
            case 2:
                if (bottle)
                    bottleGradient.color3 = Color.white;
                gradient.color3 = Color.white;
                break;
        }
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
