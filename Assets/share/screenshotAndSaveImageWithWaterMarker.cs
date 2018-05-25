using UnityEngine;
using System.Collections;
using System.IO;
using System;
namespace ARBrowser{
	public class screenshotAndSaveImageWithWaterMarker //: MonoBehaviour
	{

		static screenshotAndSaveImageWithWaterMarker ssaswwm_instance;
		static readonly object padlock = new object ();

		private screenshotAndSaveImageWithWaterMarker ()
		{

		}

		public static screenshotAndSaveImageWithWaterMarker GetInstance ()
		{
			if (ssaswwm_instance == null) {
				lock (padlock) {
					if (ssaswwm_instance == null) {
						ssaswwm_instance = new screenshotAndSaveImageWithWaterMarker ();
					}
				}
			}
			return ssaswwm_instance;
		}

		public Texture2D ScreenshotCaptureCamera (RenderTexture renderTexture)
		{
			Texture2D screenShot = new Texture2D (renderTexture.width, renderTexture.height);
			RenderTexture.active = renderTexture;
			screenShot.ReadPixels (new Rect (0, 0, renderTexture.width, renderTexture.height), 0, 0);
			screenShot.Apply ();
			RenderTexture.active = null;
			return screenShot;
		}


		//public Texture2D ScreenshotCaptureCamera(Camera CaptureCamera1)
		//{
		//    return ScreenshotCaptureCamera(CaptureCamera1, Camera.main, new Rect(0, 0, Screen.width, Screen.height));
		//}

		//public Texture2D ScreenshotCaptureCamera(Camera CaptureCamera1, Camera CaptureCamera2, Rect rect)
		//{
		//    RenderTexture rt = new RenderTexture((int)rect.width, (int)rect.height, 0);

		//    CaptureCamera1.targetTexture = rt;
		//    CaptureCamera1.Render();
		//    CaptureCamera2.targetTexture = rt;
		//    CaptureCamera2.Render();

		//    RenderTexture.active = rt;
		//    Texture2D screenShot = new Texture2D((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		//    screenShot.ReadPixels(rect, 0, 0,true);
		//   // screenShot.Apply();

		//    CaptureCamera1.targetTexture = null;
		//    CaptureCamera2.targetTexture = null;
		//    RenderTexture.active = null;
		//    GameObject.Destroy(rt);
		//    return screenShot;
		//}

		public Texture2D AddWatermarkImage (Texture2D screenShot, Texture2D waterMarker, Vector2 drawPoint)
		{
			if (drawPoint.x < 0 || drawPoint.x > 1) {
				Debug.Log ("drawPoint.x not on screenShot");
			} else if (drawPoint.y < 0 || drawPoint.y > 1) {
				Debug.Log ("drawPoint.y not on screenShot");
			} else if (drawPoint.x + waterMarker.width / screenShot.width > 1) {
				Debug.Log ("waterMareke.x not on screenShot");
			} else if (drawPoint.y + waterMarker.height / screenShot.height > 1) {
				Debug.Log ("waterMareke.y not on screenShot");

			} else
	        {

	#if UNITY_ANDROID || UNITY_EDITOR
	            for (int i = 0; i < waterMarker.height; i++)
	            {
	                for (int j = 0; j < waterMarker.width; j++)
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

		public string chooseImageOfFilename ()
		{
			return chooseImageOfFilename ("", "");
		}

		public string chooseImageOfFilename (string filePath, string fileName)
		{
			fileName = fileName.Equals ("") ? System.Convert.ToInt64 ((System.DateTime.UtcNow - new System.DateTime (1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString () + "Screenshot.png" : fileName;
			filePath = filePath.Equals ("") ? Application.temporaryCachePath+"/sharecache/" : filePath;
			if (Application.platform.Equals (RuntimePlatform.Android)) {
				if (!Directory.Exists (filePath)) {
					Directory.CreateDirectory (filePath);
				}
				return filePath + fileName;
				//  return Application.persistentDataPath + "/" + fileTimeName;
			} else if (Application.platform.Equals (RuntimePlatform.IPhonePlayer)) {
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                return filePath + fileName;
			} else {
				return Application.dataPath + "/" + fileName;
			}
		}

		public string saveImageWithTexture2D (Texture2D image, string filenPathName)
		{
			if (filenPathName != null) {
				byte[] bytes = image.EncodeToPNG ();
				System.IO.File.WriteAllBytes (filenPathName, bytes);
			}
			Debug.Log ("The Image save filename is " + filenPathName);
			return filenPathName;
		}
	}
}
