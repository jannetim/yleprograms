using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System;
using System.Text.RegularExpressions;

public class QueryHandler : MonoBehaviour
{

    public InputField Input;
    public ScrollRect ScrollRect;
    public GameObject ProgramButton;
    public Scrollbar ScrollbarVertical;
    public GameObject InfoPanel;
    public InfoPanelController InfoPanelController;
    string baseUrl;
    string auth;
    string queryText;
    string limit;
    string type;
    int offset;



    // Use this for initialization
    void Start()
    {
        baseUrl = "https://external.api.yle.fi/v1/programs/items.json";
        auth = "?app_id=7f376c3d&app_key=2fc2125e68cfddf83376af66ea6a588c";
        limit = "&limit=10";
        type = "&type=program";
        offset = 0;
        queryText = "";
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Scrolling()
    {
        if (ScrollbarVertical.value == 0 && ScrollbarVertical.size != 1)
        {
            StartCoroutine(GetPrograms());
            offset += 10;
        }
    }

    public void Search()
    {
        if (!string.IsNullOrEmpty(Input.text))
        {
            InfoPanelController.CleanContent();
            queryText = "&q=" + Input.text;
            StartCoroutine(GetPrograms());
        }
    }

    IEnumerator GetPrograms()
    {
        UnityWebRequest www = UnityWebRequest.Get(baseUrl + auth + limit + type + queryText);

        // www.Send() is getting deprecated
        yield return www.Send();

        // use with Unity 2017.2.0 ->
        //yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
            string res = www.downloadHandler.text;
            JSONObject j = new JSONObject(res);
            GetFields(j);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
    }


    void GetFields(JSONObject j)
    {
        try
        {
            int i = j.keys.IndexOf("data");
            JSONObject data = j.list[i];
            foreach (JSONObject obj in data.list)
            {
                if (obj.keys.IndexOf("title") > -1)
                {
                    data = obj.list[obj.keys.IndexOf("title")];
                    if (data.keys.IndexOf("fi") > -1)
                    {
                        i = data.keys.IndexOf("fi");
                        GameObject PButton = Instantiate(ProgramButton, ScrollRect.content);
                        Button button = PButton.GetComponent<Button>();
                        Program program = PButton.GetComponent<Program>();
                        button.GetComponentInChildren<Text>().text = data.list[i].str;
                        program.Title = data.list[i].str;
                        button.transform.SetParent(ScrollRect.content);
                        button.onClick.AddListener(InfoPanelController.ShowInfoPanel);
                        program.InfoPanel = InfoPanel;

                        if (obj.keys.IndexOf("creator") > -1)
                        {
                            data = obj.list[obj.keys.IndexOf("creator")];
                            if (data.Count > 0 && data[0].keys.IndexOf("name") > -1)
                            {
                                i = data[0].keys.IndexOf("name");
                                program.Creator = data[0].list[i].str;
                            }
                        }
                        if (obj.keys.IndexOf("description") > -1)
                        {
                            data = obj.list[obj.keys.IndexOf("description")];
                            if (data.keys.IndexOf("fi") > -1)
                            {
                                String description = data.list[data.keys.IndexOf("fi")].str;
                                description = Regex.Unescape(description);
                                program.Description = description;
                            }
                        }
                        if (obj.keys.IndexOf("duration") > -1)
                        {
                            string duration = obj.list[obj.keys.IndexOf("duration")].str;
                            duration = Regex.Replace(duration.Substring(2, duration.Length - 2), "[A-Z]", "$0 ").ToLower();

                            program.Duration = duration;

                        }
                        if (obj.keys.IndexOf("subject") > -1)
                        {
                            data = obj.list[obj.keys.IndexOf("subject")];
                            if (data.Count > 0 && data[0].keys.IndexOf("title") > -1)
                            {
                                i = data[0].keys.IndexOf("title");
                                data = data[0].list[i];
                                if (data.keys.IndexOf("fi") > -1)
                                {
                                    program.Subject = data.list[data.keys.IndexOf("fi")].str;                                    
                                }
                            }
                        }
                        if (obj.keys.IndexOf("partOfSeries") > -1)
                        {
                            data = obj.list[obj.keys.IndexOf("partOfSeries")];
                            if (data.keys.IndexOf("description") > -1)
                            {
                                data = data.list[obj.keys.IndexOf("description")];
                                if (data.keys.IndexOf("fi") > -1)
                                {
                                    string series = data.list[data.keys.IndexOf("fi")].str;
                                    series = Regex.Unescape(series);
                                    program.Series = series;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("No Finnish title available, skipping program");
                    }
                }
            }
        }
        catch (NullReferenceException e)
        {
            Debug.Log(e);
        }
    }
}


