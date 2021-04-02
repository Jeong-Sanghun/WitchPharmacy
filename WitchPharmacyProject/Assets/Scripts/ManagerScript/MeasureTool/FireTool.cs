using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTool : MeasureTool
{
    [SerializeField]
    GameObject crystalBallObject;
    [SerializeField]
    GameObject dustPrefab;

    GameObject touchedObject;               //터치한 오브젝트
    RaycastHit2D hit;                         //터치를 위한 raycastHit
    public Camera cam;                      //레이캐스트를 위한 카메라.


    // Start is called before the first frame update
    void Start()
    {
        float x, y,p;
        for(int i = 0; i < 50; i++)
        {
            GameObject dust = Instantiate(dustPrefab, crystalBallObject.transform);
            x = Random.Range(-2.75f, 2.75f);
            y = Random.Range(0.53f-Mathf.Sqrt(7.5625f-x*x),0.53f+ Mathf.Sqrt(7.5625f - x * x));
            dust.transform.localPosition = new Vector3(x, y, 0);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition); //마우스 좌클릭으로 마우스의 위치에서 Ray를 쏘아 오브젝트를 감지
            if (hit = Physics2D.Raycast(mousePos, Vector2.zero))
            {
                touchedObject = hit.collider.gameObject;
                if (touchedObject.CompareTag("Dust"))
                {
                    touchedObject.SetActive(false);
                }
            }
        }
    }
}
