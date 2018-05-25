using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.IO;
using System;

public class Test : MonoBehaviour {
    public Image image;
    public Sprite ss;
    private string[] PicturePath;
	// Use this for initialization
	void Start () {
        //ss = Resources.Load("Texture/head",typeof(Sprite)) as Sprite;
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void LoadSprite()
    {
        GetPicture(Application.streamingAssetsPath + "/Picture");
        //image.GetComponent<Image>().overrideSprite = ss;
    }
    public void GetPicture(string path)
    {
        Debug.Log(path);
        //PicturePath = new DirectoryInfo(path).GetFiles();
        foreach (var item in new DirectoryInfo(path).GetFiles())
        {
            Debug.Log(item);
        }
    }
}
