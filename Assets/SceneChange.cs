using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LitJson;
using System.IO;
public class SceneChange : MonoBehaviour {

    public GameObject panel;
    public GameObject panel2;
    // Use this for initialization
    void Start () {
        SavePath = Application.streamingAssetsPath + "/sceneconfig/config.json";
        loadSceneConfigToJson();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void ButtonNum()
    {
        PlayerPrefs.SetInt("sNum",0);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum1()
    {
        PlayerPrefs.SetInt("sNum", 1);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum2()
    {
        PlayerPrefs.SetInt("sNum", 2);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum3()
    {
        PlayerPrefs.SetInt("sNum", 3);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum4()
    {
        PlayerPrefs.SetInt("sNum", 4);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum5()
    {
        PlayerPrefs.SetInt("sNum", 5);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum6()
    {
        PlayerPrefs.SetInt("sNum", 6);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum7()
    {
        PlayerPrefs.SetInt("sNum", 7);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum8()
    {
        PlayerPrefs.SetInt("sNum", 8);
        panel.SetActive(false);
        panel2.SetActive(true);
    }
    public void ButtonNum9()
    {
        PlayerPrefs.SetInt("sNum", 9);
        panel.SetActive(false);
        panel2.SetActive(true);
    }

    public void saveSceneConfigToJson() {


        string json = JsonMapper.ToJson(scenebeans);
       
        if (!Directory.Exists(Application.streamingAssetsPath + "/sceneconfig/"))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/sceneconfig/");
        }
        File.WriteAllBytes(SavePath,System.Text.Encoding.UTF8.GetBytes(json));
    }
    string SavePath="";
    public static scenebean[] scenebeans = new scenebean[10];
    public void loadSceneConfigToJson()
    {
        using (var f = File.Open(SavePath,FileMode.Open)) {
            byte[] bs = new byte[f.Length];
            f.Read(bs, 0, bs.Length);
            scenebeans = JsonMapper.ToObject<scenebean[]>(System.Text.Encoding.UTF8.GetString(bs));
        }
    }
}


public class scenebean {

    public int index = 0;
    public double ScreenShiftX = 0;
    public double ScreenShiftY = 0;
    public double ScreenZ = 1;
    public double CropRight = 0;
    public double CropLeft = 0;
    public double CropTop = 0;
    public double CropButtom = 0;
    public double Saturation = 0;


}