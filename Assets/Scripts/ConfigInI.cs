using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
 
public static class ConfigInI
{
    static Dictionary<string, string> datas = new Dictionary<string, string>();
    static string[] data;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        string path = Application.dataPath + "config.ini";
        Debug.Log("==================>streamingAssetsPath: " + path);
        if(!File.Exists(path))
        {
            Debug.Log("==================>Create config.ini");
            File.Create(path);
            if (!File.Exists(path))
            {
                Debug.Log("<color=red>Error: </color>Create config.ini file failed.");
            }
        }

        string contents = File.ReadAllText(path);
        data = contents.Split('\n');
        for (int i = 0; i < data.Length; i++)
        {
            if (!data[i].Contains("::") && data[i].Contains("="))
            {
                string[] content = data[i].Split('=');
                if (!datas.ContainsKey(content[0].Trim()))
                {
                    datas.Add(content[0].Trim(), content[1].Trim());
                }
                else
                {
                    Debug.Log("duplicate keys exist:" + content[0]);
                }
            }
        }
    }

    public static string GetValue(string key)
    {
        if (datas.ContainsKey(key))
        {
            return datas[key];
        }
        else
        {
            Debug.Log("Illegal key");
            return "";
        }
    }

    public static void SetConfig(string key, string value)
    {
        string path = Directory.GetCurrentDirectory() + "/config.ini";
        StreamReader reader = new StreamReader(path, Encoding.GetEncoding("GB2312"));
        string content = reader.ReadLine();
        int i = 0;
        string[] stringToWriteLine = new string[data.Length];
        while (null != content)
        {
            if (!content.Contains("::") && content.Contains("=") && content.Contains(key))
            {
                datas[key] = value;
                stringToWriteLine[i] = key + " = " + datas[key];
            }
            else
            {
                stringToWriteLine[i] = content;
            }
            i++;
            content = reader.ReadLine();
        }
        reader.Close();

        StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("GB2312"));
        foreach (var item in stringToWriteLine)
        {
            writer.WriteLine(item);
        }
        writer.Flush();
        writer.Close();
    }
}
