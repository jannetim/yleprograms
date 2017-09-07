using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Program : MonoBehaviour
{

    public GameObject InfoPanel { get; set; }
    public string Title { get; set; }
    public string Creator { get; set; }
    public string Description { get; set; }
    public string Duration { get; set; }
    public string Subject { get; set; }

    Dictionary<string, string> savedFields;

    void Awake()
    {
        savedFields = new Dictionary<string, string>();
        
    }
    // Use this for initialization
    void Start()
    {
        savedFields.Add("Title", Title);
        savedFields.Add("Creator", Creator);
        savedFields.Add("Description", Description);
        savedFields.Add("Duration", Duration);
        savedFields.Add("Subject", Subject);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowInfo()
    {
        Text[] infoFields = InfoPanel.GetComponentsInChildren<Text>();

        foreach (Text infoField in infoFields)
        {
            string value;
            savedFields.TryGetValue(infoField.name, out value);
            infoField.text = value;

            /*if (savedFields.ContainsKey(infoField.name))
            {
                infoField.text = savedFields[infoField.name];
            }*/
        }        
    }
}
