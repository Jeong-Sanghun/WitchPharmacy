using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
public class StoryParser
{
    enum ParseMode{
        Clamp,Switch,Route,DialogCharacterName, Dialog
    }

    public StoryParser()
    {

    }

    public ConversationDialogBundle LoadBundle(string bundleName,string languageDirectory,UILanguagePack languagePack)
    {
        string originText;

        ConversationDialogBundle gameData = new ConversationDialogBundle();

        string language = languageDirectory;
        string directory = "JsonData/";

        string appender1 = bundleName;
        StringBuilder builder = new StringBuilder(directory);
        builder.Append(language);
        builder.Append("StoryBundle");
        builder.Append(appender1);

        TextAsset jsonString = Resources.Load<TextAsset>(builder.ToString());
        originText = jsonString.text;

        gameData.bundleName = bundleName;

        builder = new StringBuilder();
        
        for (int i = 0; i < originText.Length; i++)
        {
            if(originText[i] == '<')
            {

            }
        }

        return gameData;
        //이 정보를 게임매니저나, 로딩으로 넘겨주는 것이당
    }
    
}
