using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelsManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public GameObject ScanPanel;
    public void Hide()
    {
        ScanPanel.GetComponent<CanvasGroup>().alpha = 0;
        ScanPanel.GetComponent<CanvasGroup>().interactable = false;
        ScanPanel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void Show()
    {
        ScanPanel.GetComponent<CanvasGroup>().alpha = 1;
        ScanPanel.GetComponent<CanvasGroup>().interactable = true;
        ScanPanel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

}
