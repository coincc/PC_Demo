using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ImagesManager : MonoBehaviour {

    public GameObject albumPanel;
    public GameObject PicsPanel;
    public GameObject picPrefab;
    public InputField currentPagesCountText_InputField;
    public Text allPagesCountText;


    //计算页数
    private int currentPageCount = 1;
    private int allPageCount = 0;
    //显示 当前页 照片
    private List<string> allPicsPaths = new List<string>();
    private void Start()
    {
        Hide(albumPanel);
    }
    // Update is called once per frame
    void Update () {
		
	}



    IEnumerator LoadImg(GameObject img,string Path)
    {
        WWW www = new WWW(Path);
        yield return www;

        img.GetComponent<RawImage>().texture = www.texture;
        img.GetComponent<PicsInfo>().PicPath = Path;
    }
    public void EnterAlbum()
    {
        
        string[] tempPicsPaths=Directory.GetFiles(Application.streamingAssetsPath + "/pictureupload");
        //装进list
        foreach (var picpath in tempPicsPaths)
        {
            if (picpath.Contains(".meta"))
            { continue; }           
            allPicsPaths.Add(picpath);          
        }
        print(allPicsPaths.Count);
       
        Show(albumPanel);
        ShowFirstPage();
        allPageCount = Mathf.CeilToInt(allPicsPaths.Count / 10f);

        RefreshPageCountShowPanel();

    }

    public void ShowFirstPage()
    {
        //显示图片
        if(allPicsPaths.Count<=10)
        {
            foreach (var picpath in allPicsPaths)
            {
                GameObject image = Instantiate(picPrefab, PicsPanel.transform) as GameObject;
                StartCoroutine(LoadImg(image, picpath));
            }
        }
        else
        {
            for(int i=0;i<10;i++)
            {
                GameObject image = Instantiate(picPrefab, PicsPanel.transform) as GameObject;
                StartCoroutine(LoadImg(image, allPicsPaths[(currentPageCount - 1) * 10 + i]));
            }
        }              
    }

    /// <summary>
    /// 更新 1/10 页面计数显示
    /// </summary>
    public void RefreshPageCountShowPanel()
    {
        currentPagesCountText_InputField.text =currentPageCount.ToString();
        allPagesCountText.text = ("/" + allPageCount).ToString();
    }

    public void ToPrePage()
    {
        if(currentPageCount>1)
        {
            int i = 0;
            foreach(Transform picTrans in PicsPanel.transform)
            {
                i++;
            }
            //实例化出上一页的10个图片面板
            for(int j=0;j<10-i;j++)
            {
                GameObject go = Instantiate(picPrefab, PicsPanel.transform) as GameObject;
            }
            currentPageCount--;
            //进行图片赋值


            // 11-13  huidao 1-10   index  = 0--9

            int k = 0;
            foreach(Transform pic in PicsPanel.transform)
            {

                StartCoroutine(LoadImg(pic.gameObject, allPicsPaths[(currentPageCount-1)*10+k]));
                k++;
            }

            RefreshPageCountShowPanel();

        }
    }

    public void ToNextPage()
    {
        //能点下一页的情况： 图片数肯定大于10张  意味着当前页肯定显示满了10张
        if(allPageCount>1&&(currentPageCount<allPageCount))
        {
            int nextPagesCount = 0;
            //剩下的不止一页，那下一页肯定显示10张
            if ((allPageCount-currentPageCount)>1)
            {
                nextPagesCount = 10;
                //切换 当前页显示++
                currentPageCount++;
                int k = 0;
                foreach (Transform pic in PicsPanel.transform)
                {
                    StartCoroutine(LoadImg(pic.gameObject, allPicsPaths[(currentPageCount - 1) * 10 + k]));
                    k++;
                }


            }
            else if((allPageCount - currentPageCount)==1)//余下只有一页
            {
                //剩余数量 1<= nextPagesCount <=10
                nextPagesCount = allPicsPaths.Count-currentPageCount*10;

                currentPageCount++;
                int k = 0;
                foreach (Transform pic in PicsPanel.transform)
                {
                    if((k+1)<=nextPagesCount)
                    {
                        StartCoroutine(LoadImg(pic.gameObject, allPicsPaths[(currentPageCount - 1) * 10 + k]));
                        k++;
                    }
                    else
                    {
                        Destroy(pic.gameObject);
                    }                  
                }
            }
            RefreshPageCountShowPanel();
        }
    }

    /// <summary>
    /// 手动输入 跳页
    /// </summary>
    public void ToAnyPage()
    {
        int inputPageNum = Convert.ToInt32(currentPagesCountText_InputField.text);
        //输入页码符合 跳页要求
        if(inputPageNum>=1&&inputPageNum<=allPageCount&&inputPageNum!=currentPageCount)
        {
            int allPicsNum = allPicsPaths.Count;
            //1:1-10 2:11-20 3:21-30 4:31-32
            int inputPageToEndPicsNum = allPicsNum - (inputPageNum - 1) * 10;//输入页码 到最后 的照片de 数量
            int inputPagePicsNum = 0;
            if(inputPageToEndPicsNum>0)
            {
                if(inputPageToEndPicsNum>10)
                {
                    inputPagePicsNum = 10;
                }
                else
                {
                    inputPagePicsNum = inputPageToEndPicsNum;
                }
            }
            //至此得到 输入页的应当显示的数量inputPagePicsNum  
            //         当前显示的数量PicsPanel.transform.childCount

            currentPageCount = inputPageNum;

            int PicsPanelChildCount = PicsPanel.transform.childCount;
            //然后控制 照片面板的数量

            //增加到目标数量
            if (inputPagePicsNum> PicsPanel.transform.childCount)
            {
                int m = inputPagePicsNum - PicsPanel.transform.childCount;
                for(int i=0;i<m;i++)
                {
                    Instantiate(picPrefab, PicsPanel.transform);
                }
            }
            else if(inputPagePicsNum < PicsPanel.transform.childCount)//减少到目标数量
            {
                int n =  PicsPanel.transform.childCount- inputPagePicsNum;
                for (int i = 0; i < n; i++)
                {
                    //删除最后那个
                    DestroyImmediate(PicsPanel.transform.GetChild(PicsPanel.transform.childCount - 1).gameObject);
                    print("删除");
                }
            }

            //图片加载赋值
            int k = 0;
            foreach (Transform pic in PicsPanel.transform)
            {
                if ((k + 1) <= inputPagePicsNum)
                {
                    StartCoroutine(LoadImg(pic.gameObject, allPicsPaths[(currentPageCount - 1) * 10 + k]));
                    k++;
                }
            }

                RefreshPageCountShowPanel();
        }
        else//输入不符合
        {
            RefreshPageCountShowPanel();
        }
    }

    public void ExitAlbum()
    {
        Hide(albumPanel);
        ClearAlbum();
        allPicsPaths = new List<string>();
    }
    public void ClearAlbum()
    {
        foreach(Transform childImg in PicsPanel.transform)
        {
            Destroy(childImg.gameObject);
        }
    }

    public void Hide(GameObject Panel)
    {
        Panel.GetComponent<CanvasGroup>().alpha = 0;
        Panel.GetComponent<CanvasGroup>().interactable = false;
        Panel.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }
    public void Show(GameObject Panel)
    {
        Panel.GetComponent<CanvasGroup>().alpha = 1;
        Panel.GetComponent<CanvasGroup>().interactable = true;
        Panel.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    
}
