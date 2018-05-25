using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ZXing;
using ZXing.QrCode;
using System;
using ZXing.Common;
using ZXing.Rendering;

public class WebCameraManager : MonoBehaviour
{

    public string DeviceName;
    public Vector2 CameraSize;
    public Camera FgCamera;
    public Camera BgCamera;
    public Camera CCamer;
    public GameObject UI;
    public GameObject UI2;
    public float CameraFPS;  //在计算机图像领域中，“FPS”是词组“Frames Per Second”的缩写。“Frames Per Second”在计算机图像范畴内被翻译为：“每秒传输帧数”。更确切的解释，就是“每秒中填充图像的帧数(帧/秒)“。


    WebCamTexture _webCameraTexture; //接收返回的图片数据 
                                     // public GameObject Plane;//作为显示摄像头的面板
    public UnityEngine.UI.RawImage image;
    public UnityEngine.UI.RawImage previewimage;

    public Texture2D colorPicker;
    public Texture2D whiteSquare;
    private Color guiColor;

    public int colorPickerSize;

    public Texture2D encoded;

    private string SaveName;//保存图片的名字

    public GameObject panel;
    public GameObject panel2;

    public GameObject skill;
    public GameObject backgroud;
    public float timer;
    private bool state = false;

    public Slider Screenshitx;
    public Slider Screenshity;
    public Slider Multipler;
    public Slider Range;
    public Slider HueRange;
    public Slider opacity;
    public Slider edgeSharpness;
    public Slider CropLeft;
    public Slider CropRight;
    public Slider CropTop;
    public Slider CropButtom;
    public Slider Saturation;


    public GameObject texture;
    public GameObject encode;

    public UChromaKey uck;
    public bool GUI_OPEN { set; get; }

    void Start()
    {
        guiColor = GUI.color;
        StartCoroutine("InitCameraCor");//开始协程，意思就是启动一个辅助的线程
        float r = PlayerPrefs.GetFloat("R", 0);
        float g = PlayerPrefs.GetFloat("G", 255);
        float b = PlayerPrefs.GetFloat("B", 0);
        uck.SelectedColor = new Color(r, g, b);
    }
    void OnGUI()
    {

        if (GUI_OPEN)
        {
            GUILayout.BeginVertical();
            GUILayout.Label("Key color selection:");
            GUI.color = uck.SelectedColor;
            GUILayout.Label(whiteSquare);
            GUI.color = guiColor;
            //Rect lastRect = GUILayoutUtility.GetLastRect();
            if (GUILayout.RepeatButton(colorPicker, "label", GUILayout.Width(colorPickerSize), GUILayout.Height(colorPickerSize)))
            {
                Vector2 pickpos = Event.current.mousePosition;
                float aaa = pickpos.x;
                float bbb = pickpos.y;
                int aaa2 = (int)(aaa * (colorPicker.width / (colorPickerSize + 0.0f)));
                int bbb2 = (int)((colorPickerSize - bbb) * (colorPicker.height / (colorPickerSize + 0.0f)));
                uck.SelectedColor = colorPicker.GetPixel(aaa2, bbb2);
                PlayerPrefs.SetFloat("R", uck.SelectedColor.r);
                PlayerPrefs.SetFloat("G", uck.SelectedColor.g);
                PlayerPrefs.SetFloat("B", uck.SelectedColor.b);

            }
            GUILayout.EndVertical();
        }


    }
    public void change()
    {
        sceneindex = PlayerPrefs.GetInt("sNum");
        Screenshitx.value = (float)SceneChange.scenebeans[sceneindex].ScreenShiftX;
        Screenshity.value = (float)SceneChange.scenebeans[sceneindex].ScreenShiftY;
        Multipler.value = (float)SceneChange.scenebeans[sceneindex].ScreenZ;
        CropLeft.value = (float)SceneChange.scenebeans[sceneindex].CropLeft;
        CropRight.value = (float)SceneChange.scenebeans[sceneindex].CropRight;
        CropTop.value = (float)SceneChange.scenebeans[sceneindex].CropTop;
        CropButtom.value = (float)SceneChange.scenebeans[sceneindex].CropButtom;
        Saturation.value = (float)SceneChange.scenebeans[sceneindex].Saturation;

    }
    void Update()
    {

        uck.Range = Range.value;
        uck.HueRange = HueRange.value;
        uck.opacity = opacity.value;
        uck.edgeSharpness = edgeSharpness.value;
        uck.uvShift.x = Screenshitx.value;
        uck.uvShift.y = Screenshity.value;
        uck.uvCoef.x = Multipler.value;
        uck.uvCoef.y = Multipler.value;
        uck.crop.x = CropLeft.value;
        uck.crop.y = CropRight.value;
        uck.crop.w = CropTop.value;
        uck.crop.z = CropButtom.value;
        uck.saturation = Saturation.value;
#if UNITY_EDITOR

        if (sceneindex>=0)
        {
            SceneChange.scenebeans[sceneindex].ScreenShiftX = double.Parse(Screenshitx.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].ScreenShiftY = double.Parse(Screenshity.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].ScreenZ = double.Parse(Multipler.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].CropLeft = double.Parse(CropLeft.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].CropRight = double.Parse(CropRight.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].CropTop = double.Parse(CropTop.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].CropButtom = double.Parse(CropButtom.value.ToString("0.000"));
            SceneChange.scenebeans[sceneindex].Saturation = double.Parse(Saturation.value.ToString("0.000"));
        }

#endif 
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (state)
        {
            //Debug.Log(timer);
            timer -= Time.deltaTime;
            skill.SetActive(true);
            //Debug.Log(timer);
            if (timer <= 2.0f)
            {

                skill.GetComponent<Image>().overrideSprite = Resources.Load("Texture/2", typeof(Sprite)) as Sprite;
            }
            if (timer <= 1.0f)
            {
                skill.GetComponent<Image>().overrideSprite = Resources.Load("Texture/1", typeof(Sprite)) as Sprite;
            }
            if (timer <= 0.0f)
            {
                skill.SetActive(false);
                backgroud.GetComponent<Image>().color = new Color(1, 1, 1, 120.0f / 255);
                skill.GetComponent<Image>().overrideSprite = Resources.Load("Texture/3", typeof(Sprite)) as Sprite;
            }
            if (timer <= -0.5f)
            {
                backgroud.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                state = false;
            }
        }
    }

    public void Back()
    {
        sceneindex = -1;

        image.texture = null;
        panel.SetActive(true);
        panel2.SetActive(false);
        CCamer.transform.position = new Vector3(0.51f, -0.25f, -14.1f);
        PlayCamera();
    }


    public void TakePhoto()
    {
        timer = 3;
        state = !state;
        panel2.SetActive(false);
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        Debug.Log("11111");

        yield return new WaitForSeconds(4f);
        if (_webCameraTexture != null)
        {
            Debug.Log("22222");

            if (_webCameraTexture.isPlaying)
            {
                Debug.Log("3333333");
                StopCamera();

            }
        }
    }

    public void NewPhoto()
    {
        image.texture = null;
        // previewimage.texture = null;
        UI2.SetActive(false);
        PlayCamera();
        File.Delete(Application.streamingAssetsPath + "/pictureupload/" + SaveName);
    }

    public void SaveTexture()
    {
        GetEncode("http://gzdl.chu-jiao.com/DianLi/" + SaveName);
        StartCoroutine(LoadImg(texture, Application.streamingAssetsPath + "/pictureupload/" + SaveName));
        StartCoroutine(LoadImg(encode, Application.streamingAssetsPath + "/encodedpicture/" + SaveName));
        image.texture = null;
        //previewimage.texture = null;
        PlayCamera();
    }
    IEnumerator LoadImg(GameObject img, string Path)
    {
        WWW www = new WWW(Path);
        yield return www;

        img.GetComponent<RawImage>().texture = www.texture;

    }


    public void PlayCamera()
    {
        //Plane.GetComponent<MeshRenderer>().enabled = true;
        _webCameraTexture.Play();
        image.gameObject.SetActive(false);
        //previewimage.texture = _webCameraTexture;

        //image.gameObject.SetActive(false);
    }


    public void StopCamera()
    {
        //Plane.GetComponent<MeshRenderer>().enabled = false;

        _webCameraTexture.Stop();

        StartCoroutine(_add());

    }
    Texture2D ss;
    IEnumerator _add()
    {

        image.gameObject.SetActive(true);
        UI.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        //var texturemix = ScreenshotCaptureCamera(CCamer);

        //var texturemix = AddWatermarkImage(ScreenshotCaptureCamera(BgCamera), ScreenshotCaptureCamera(CCamer), new Vector2(0, 0));
        yield return new WaitForSeconds(0.1f);
        string fileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        SaveName = fileName;

        //image.texture = (Texture)AddWatermarkImage(ss, ScreenshotCaptureCamera(FgCamera), Vector2.zero);
        string SavePath = Application.streamingAssetsPath + "/pictureupload/" + fileName;
        ScreenCapture.CaptureScreenshot(SavePath);
        yield return new WaitForSeconds(0.5f);

        WWW www = new WWW("file:///" + SavePath);
        yield return www;
        if (string.IsNullOrEmpty(www.error))
        {
            ss = www.texture;
        }
        else
        {
            Debug.LogError(SavePath);

            Debug.LogError(www.error);
        }
        // ss = AddWatermarkImage(texturemix, ScreenshotCaptureCamera(FgCamera), new Vector2(0, 0));
        if (ss == null)
        {
            Debug.Log("111111111");

        }
        else
        {
            Debug.Log("2222222222");

        }
        image.texture = ss;
        //byte[] b = ss.EncodeToPNG();
        ss = null;
        //string fileName = System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";

        //image.texture = (Texture)AddWatermarkImage(ss, ScreenshotCaptureCamera(FgCamera), Vector2.zero);
        UI.SetActive(true);
        UI2.SetActive(true);
        //string SavePath = Application.streamingAssetsPath+"/pictureupload/" + fileName;
        //if (!Directory.Exists(Application.streamingAssetsPath + "/pictureupload/"))
        //{
        //    Directory.CreateDirectory(Application.streamingAssetsPath + "/pictureupload/");
        //}
        //File.WriteAllBytes(SavePath, b);
    }

    public Texture2D ScreenshotCaptureCamera(Camera camera)
    {
        return ScreenshotCaptureCamera(camera, new Rect(0, 0, Screen.width, Screen.height));
    }

    public Texture2D ScreenshotCaptureCamera(Camera camera, Rect rect)
    {
        RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);
        camera.targetTexture = rt;
        camera.Render();

        RenderTexture.active = camera.targetTexture;
        Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
        screenShot.ReadPixels(rect, 0, 0);
        screenShot.Apply();
        RenderTexture.active = null;
        camera.targetTexture = null;
        GameObject.Destroy(rt);
        return screenShot;
    }
    /// <summary> 
    /// 初始化摄像头
    /// </summary> 
    public IEnumerator InitCameraCor()   //定义一个协程
    {
        yield return Application.RequestUserAuthorization(UserAuthorization.WebCam);
        if (Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            if (_webCameraTexture != null)
            {

                _webCameraTexture.Stop();
                _webCameraTexture = null;
            }

            WebCamDevice[] cameraDevices = WebCamTexture.devices;

            string deviceName = cameraDevices[0].name;
            uck.SourceType = UChromaKey.ChromaKeySource.Device;
            uck.DeviceName = deviceName;

            Debug.Log(deviceName);
            _webCameraTexture = uck.webCamTexture;
            //_webCameraTexture.Play();
            //           previewimage.texture = uck.webCamTexture;
            Debug.Log("_webCameraTexture");

            //Plane.GetComponent<Renderer>().material.mainTexture = _webCameraTexture;
            //Plane.transform.localScale = new Vector3(1, 1, 1);

        }
        yield return new WaitForSeconds(1f);

    }
    int sceneindex = -1;

    public Texture2D AddWatermarkImage(Texture2D screenShot, Texture2D waterMarker, Vector2 drawPoint)
    {
        if (drawPoint.x < 0 || drawPoint.x > 1)
        {
            Debug.Log("drawPoint.x not on screenShot");
        }
        else if (drawPoint.y < 0 || drawPoint.y > 1)
        {
            Debug.Log("drawPoint.y not on screenShot");
        }
        else if (drawPoint.x + waterMarker.width / screenShot.width > 1)
        {
            Debug.Log(waterMarker.width);
            Debug.Log(screenShot.width);
            Debug.Log("waterMareke.x not on screenShot");
        }
        else if (drawPoint.y + waterMarker.height / screenShot.height > 1)
        {
            Debug.Log("waterMareke.y not on screenShot");

        }
        else
        {
#if UNITY_ANDROID || UNITY_EDITOR || UNITY_STANDALONE




            for (int i = 0; i < waterMarker.width; i++)
            {
                for (int j = 0; j < waterMarker.height; j++)
                {
                    Color waterMarkerColor = waterMarker.GetPixel(i, j);
                    Color screenShotColor = screenShot.GetPixel(i, j);
                    screenShot.SetPixel(i, j, new Color(waterMarkerColor.a * waterMarkerColor.r + (1 - waterMarkerColor.a) * screenShotColor.r, waterMarkerColor.a * waterMarkerColor.g + (1 - waterMarkerColor.a) * screenShotColor.g, waterMarkerColor.a * waterMarkerColor.b + (1 - waterMarkerColor.a) * screenShotColor.b, 1));
                }
            }
#elif UNITY_IOS || UNITY_EDITOR_OSX

	            Color[] waterMarkerColor = waterMarker.GetPixels(0, 0, (int)waterMarker.width, (int)waterMarker.height);
	            screenShot.SetPixels((int)(drawPoint.x * screenShot.width), (int)(drawPoint.y * screenShot.height), (int)waterMarker.width, (int)waterMarker.height, waterMarkerColor);

#endif
            screenShot.Apply();

            return screenShot;
        }
        return null;
    }


    private void GetEncode(string url)
    {
        //设置二维码大小  
        encoded = new Texture2D(612, 612);
        //二维码边框  
        BitMatrix BIT;
        //设置二维码扫描结果  
        string name = url;

        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();

        //设置编码方式  
        hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

        BIT = new MultiFormatWriter().encode(name, BarcodeFormat.QR_CODE, 612, 612, hints);
        int width = BIT.Width;
        int height = BIT.Width;

        for (int x = 0; x < height; x++)
        {
            for (int y = 0; y < width; y++)
            {
                if (BIT[x, y])
                {
                    encoded.SetPixel(y, x, Color.black);
                }
                else
                {
                    encoded.SetPixel(y, x, Color.white);
                }

            }
        }
        encoded.Apply();
        byte[] bytes = encoded.EncodeToPNG();//把二维码转成byte数组，然后进行输出保存为png图片就可以保存下来生成好的二维码  
        if (!Directory.Exists(Application.streamingAssetsPath + "/encodedpicture/"))//创建生成目录，如果不存在则创建目录  
        {
            Directory.CreateDirectory(Application.streamingAssetsPath + "/encodedpicture/");
        }
        string fileName = Application.streamingAssetsPath + "/encodedpicture/" + SaveName;
        System.IO.File.WriteAllBytes(fileName, bytes);
    }
}
