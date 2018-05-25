using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class PicsInfo : MonoBehaviour {
    public string PicPath;
    public Transform ScanPanel;
    private Transform canvas;
    private Transform bigPic;
    private Transform TwoDCodePic;
    private void Start()
    {
        this.canvas = GameObject.Find("Panel 1").transform;
        ScanPanel = canvas.transform.Find("ScanBg");
        bigPic = ScanPanel.transform.Find("PicImage");
        TwoDCodePic = ScanPanel.transform.Find("2DCodeImage");
    }
    public void Show2DCodesPanel()
    {
        var cgp = ScanPanel.GetComponent<CanvasGroup>();
        cgp.alpha = 1;
        cgp.blocksRaycasts = true;
        cgp.interactable = true;
        Get2DCodes();
    }

    public void Hide2DCodesPanel()
    {
        var cgp = ScanPanel.GetComponent<CanvasGroup>();
        cgp.alpha = 0;
        cgp.blocksRaycasts = false;
        cgp.interactable = false;
    }

    IEnumerator LoadImg(GameObject img, string Path)
    {
        WWW www = new WWW(Path);
        yield return www;

        img.GetComponent<RawImage>().texture = www.texture;
        
    }
    public void Get2DCodes()
    {
        //print("父级目录" + Path.GetDirectoryName(PicPath));
        string picName = PicPath.Replace(Path.GetDirectoryName(PicPath)+@"\", "");
        //print("picname:" + picName);
        //var twoDcodePicPath = Application.dataPath + @"\2DCodes\" + picName;
        //print(twoDcodePicPath);
        StartCoroutine(LoadImg(bigPic.gameObject, Application.streamingAssetsPath + "/pictureupload/"+ picName));
        StartCoroutine(LoadImg(TwoDCodePic.gameObject, Application.streamingAssetsPath + "/encodedpicture/"+ picName));
    }


}
