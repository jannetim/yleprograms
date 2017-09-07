using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoPanelController : MonoBehaviour {

    public GameObject InfoPanel; 

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void CleanContent()
    {
        if (InfoPanel.activeInHierarchy)
        {
            StartCoroutine(FadeOutInfoPanel());
        }
        GameObject[] programButtons = GameObject.FindGameObjectsWithTag("ProgramButton");
        foreach (GameObject programButton in programButtons)
        {
            Destroy(programButton);
        }
    }

    IEnumerator FadeOutInfoPanel()
    {
        int panelAlpha = (int)InfoPanel.GetComponent<CanvasGroup>().alpha * 100;
        for (int i = panelAlpha; i > 0; i -= 5)
        {
            float f = i / 100f;
            InfoPanel.GetComponent<CanvasGroup>().alpha = f;
            yield return null;
        }
        InfoPanel.SetActive(false);
    }

    public void ShowInfoPanel()
    {
        if (!InfoPanel.activeInHierarchy)
        {
            InfoPanel.SetActive(true);
        }
        StartCoroutine(FadeInInfoPanel());
        LayoutRebuilder.ForceRebuildLayoutImmediate(InfoPanel.GetComponent<RectTransform>());
    }

    IEnumerator FadeInInfoPanel()
    {
        float panelAlpha = InfoPanel.GetComponent<CanvasGroup>().alpha;
        for (int i = 20; i <= 100; i += 5)
        {
            float f = i / 100f;
            InfoPanel.GetComponent<CanvasGroup>().alpha = f;
            yield return null;
        }
    }
}
