using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

public class SpecialVisitorClass
{
    public GameObject visitorObject;

    public SpecialVisitorClass(GameObject parent,string characterName)
    {
        string path = "RandomCharacter/";
        Transform visitorParent = parent.transform;
        GameObject visitor = new GameObject();
        visitor.transform.SetParent(visitorParent);
        visitorObject = visitor;


        StringBuilder builder = new StringBuilder(path);
        builder.Append(characterName);
        GameObject lily = Resources.Load<GameObject>(builder.ToString());
        GameObject part = GameObject.Instantiate(lily, visitor.transform);
        part.transform.position = new Vector3(0, -11.91f, -1);
    }
}
