using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    List<VisitorClass> visitorList;
    SymptomDialog dialog;
    public Text textComponent;

    void Start()
    {
        visitorList = new List<VisitorClass>();
        dialog = new SymptomDialog();
        
        for(int i = 0; i< 6; i++)
        {
            visitorList.Add(new VisitorClass(dialog));
        }
    }


    int index = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            VisitorClass visitor = new VisitorClass(dialog);
            textComponent.text = visitor.fullDialog;
        }
    }
}
