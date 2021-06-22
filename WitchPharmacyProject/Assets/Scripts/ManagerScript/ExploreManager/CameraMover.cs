using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    const float speed = 1;
    const float acceleration = 1;
    [SerializeField]
    GameObject cameraObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    Vector3 deltaMousepos;
    Vector3 lastMousepos;
    // Update is called once per frame
    void Update()
    {
        
        //if (Input.touchCount==1)
        //{
        //    Touch touch = Input.GetTouch(0);
        //    Debug.Log("터치");
        //    if(touch.phase == TouchPhase.Moved)
        //    {
        //        cameraObject.transform.localPosition =
        //            cameraObject.transform.localPosition + new Vector3(touch.deltaPosition.x * speed, 0,0);
        //    }
        //}
        if (Input.GetMouseButtonDown(0))
        {
            lastMousepos = Input.mousePosition;
        }
        if (Input.GetMouseButton(0))
        {
            deltaMousepos = lastMousepos - Input.mousePosition;
            if (deltaMousepos.x > 10 || deltaMousepos.x<-10)
            {
                cameraObject.transform.localPosition =
                   cameraObject.transform.localPosition + new Vector3(deltaMousepos.x/100 * speed, 0, 0);
                if (cameraObject.transform.localPosition.x > 17)
                {
                    cameraObject.transform.localPosition = new Vector3(17, 0, -10);
                }
                else if (cameraObject.transform.localPosition.x < 0)
                {
                    cameraObject.transform.localPosition = new Vector3(0, 0, -10);
                }

            }
            lastMousepos = Input.mousePosition;
        }
    }

    
}
