using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BFGround : MonoBehaviour {

    public GameObject BImage;
    public GameObject FImage;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerPrefs.GetInt("sNum") == 0)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/1-1", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/1-2", typeof(Sprite)) as Sprite;
          
        }
        if (PlayerPrefs.GetInt("sNum") == 1)
        {
            FImage.SetActive(true);
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/1背景", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/1前景", typeof(Sprite)) as Sprite;
           
        }
        if (PlayerPrefs.GetInt("sNum") == 2)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/2-1", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/2-2", typeof(Sprite)) as Sprite;

            
        }
        if (PlayerPrefs.GetInt("sNum") == 3)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/3-1", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/3-2", typeof(Sprite)) as Sprite;

           
        }
        if (PlayerPrefs.GetInt("sNum") == 4)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/4-1", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/4-2", typeof(Sprite)) as Sprite;

           
        }
        if (PlayerPrefs.GetInt("sNum") == 5)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/5-1", typeof(Sprite)) as Sprite;
            FImage.SetActive(false);

        }
        if (PlayerPrefs.GetInt("sNum") == 6)
        {

            FImage.SetActive(true);
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/6背景", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/6前景", typeof(Sprite)) as Sprite;

            
        }
        if (PlayerPrefs.GetInt("sNum") == 7)
        {
            FImage.SetActive(true);
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/7背景", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/7前景", typeof(Sprite)) as Sprite;
        }
        if (PlayerPrefs.GetInt("sNum") == 8)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/6", typeof(Sprite)) as Sprite;
            FImage.SetActive(false);
        }
        if (PlayerPrefs.GetInt("sNum") == 9)
        {
            BImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/第一盏路灯-1", typeof(Sprite)) as Sprite;
            FImage.GetComponent<Image>().sprite = Resources.Load("BGTexture/第一盏路灯-2", typeof(Sprite)) as Sprite;
        }
    }
}
