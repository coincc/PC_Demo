using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Text;

public class CopyUpdata : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void getURL()
    {
        DirectoryInfo s = new DirectoryInfo(Application.persistentDataPath);
        s = Directory.GetParent(Application.persistentDataPath);
        s = Directory.GetParent(s.ToString());
        s = Directory.GetParent(s.ToString());
        s = Directory.GetParent(s.ToString());
        Debug.Log(s);
        //LoadJson();
        CopyTo(Application.streamingAssetsPath+ "/7N/.qrsbox",s+ "/.qrsbox");
        //System.Diagnostics.Process.Start(Application.streamingAssetsPath + "/7N/qiniu/qrsbox/qrsbox.exe");
        Debug.Log(Application.streamingAssetsPath + "7N/qiniu/qrsbox/qrsbox.exe");
    }

    public void CopyTo(string path, string toPath)
    {
        //判断复制的文件是否存在
        if (!Directory.Exists(path))
        {
            return;
        }
        //创建目标路径下的文件夹
        if (Directory.Exists(toPath))
        {
            DirectoryInfo di = new DirectoryInfo(toPath);
            di.Delete(true);
        }
        Directory.CreateDirectory(toPath);
        string[] files = Directory.GetFiles(path);
        foreach (string formFileName in files)
        {
            string fileName = System.IO.Path.GetFileName(formFileName);
            string toFileName = System.IO.Path.Combine(toPath, fileName);
            File.Copy(formFileName, toFileName);
        }
        string[] fromDirs = Directory.GetDirectories(path);
        foreach (string fromDirName in fromDirs)
        {
            string dirName = System.IO.Path.GetFileName(fromDirName);
            string toDirName = System.IO.Path.Combine(toPath, dirName);
            CopyTo(fromDirName, toDirName);
        }
    }

    public void LoadJson()
    {
        string Json = File.ReadAllText(Application.streamingAssetsPath + "/7N/.qrsbox/qrsbox.conf");
        Debug.Log(Json);
        QnData obj = JsonUtility.FromJson<QnData>(Json);
        obj.tasks.src = (Application.streamingAssetsPath + "/pictureupload/").ToString().Replace("/","\\");
        Debug.Log(obj.tasks.src);
    }
    public static void Save(string information, string path)
    {
        //文件流信息
        StreamWriter sw;
        FileInfo t = new FileInfo(path);
        if (!t.Exists)
        {
            //如果此文件不存在则创建
            sw = t.CreateText();
        }
        else
        {
            //如果此文件存在则打开
            sw = t.AppendText();
        }
        //以行的形式写入信息
        Byte[] info = Encoding.UTF8.GetBytes(information);
        string str = Encoding.UTF8.GetString(info);
        sw.WriteLine(str);
        //关闭流
        sw.Close();
        //销毁流
        sw.Dispose();
    }

}

[Serializable]
public class QnData
{
    public refencenes tasks;
    public string access_key;
    public string secret_key;
    public int report_mode;
    public int debug_level;
}
[Serializable]
public class refencenes
{
    public string src;
    public string dest;
    public int deletable;
    public int syncdur;
}