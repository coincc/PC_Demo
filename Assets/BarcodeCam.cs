using UnityEngine;
using System.Collections;
using ZXing;
using ZXing.QrCode;
using System;
using ZXing.Common;
using ZXing.Rendering;
using System.Collections.Generic;
using System.IO;

public class BarcodeCam : MonoBehaviour
{
    public Texture2D encoded;



    void Start()
    {
        //设置二维码大小  
        encoded = new Texture2D(612, 612);
        //二维码边框  
        BitMatrix BIT;
        //设置二维码扫描结果  
        string name = "http://www.baidu.com";

        Dictionary<EncodeHintType, object> hints = new Dictionary<EncodeHintType, object>();

        //设置编码方式  
        hints.Add(EncodeHintType.CHARACTER_SET, "UTF-8");

        BIT = new MultiFormatWriter().encode(name, BarcodeFormat.QR_CODE, 512, 512, hints);
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
        if (!Directory.Exists("D:/upload/encodedpicture/"))//创建生成目录，如果不存在则创建目录  
        {
            Directory.CreateDirectory("D:/upload/encodedpicture/");
        }
        string fileName = "D:/upload/encodedpicture/" + 111 + ".png";
        System.IO.File.WriteAllBytes(fileName, bytes);
    }

}