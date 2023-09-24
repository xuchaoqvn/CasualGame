using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Text;

public class NewBehaviourScript : MonoBehaviour
{
    private const string API_KEY = "Q0Hn4czmkvVklYiXCMIeHKbi";

    private const string SECRET_KEY = "fYWamTLQ5etnW0lV9cIecUNHo1qFcwK0";

    private string RequestURL = "https://aip.baidubce.com/rpc/2.0/ernievilg/v1/txt2imgv2";

    private string m_Access_Token = "24.c2c1972ab358d867cee1c2ffae8e5e74.2592000.1697699924.282335-39605613";

    [SerializeField]
    private Texture2D m_Texture2D;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        /*
        System.Text.Encoding.Default.GetBytes("");
        byte[] data = System.IO.File.ReadAllBytes($"{Application.dataPath}/DefaultConfig.bytes");
        using (MemoryStream memoryStream = new MemoryStream(data, 0, data.Length, false))
        {
            using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
            {
                while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                {
                    string configName = binaryReader.ReadString();
                    string configValue = binaryReader.ReadString();
                    Debug.Log($"{configName}:{configValue}");
                }
            }
        }
        //string content = System.Text.Encoding.Default.GetString(data);
        */
        string path = $"{Application.dataPath}/2048Config.bytes";
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
            {
                binaryWriter.Write("2048.ScreenWidth");
                binaryWriter.Write("1080");
                binaryWriter.Write("2048.ScreenHeight");
                binaryWriter.Write("1920");
                binaryWriter.Write("2048.Fullscreen");
                binaryWriter.Write("False");
                binaryWriter.Write("2048.CameraSize");
                binaryWriter.Write("30");
            }
        }
    }

    /*
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        private IEnumerator Start()
        {
            //yield return this.GetAccessToken();

            Debug.Log("00");

            JObject jObject = new JObject();
            JProperty prompt = new JProperty("prompt", "睡莲");
            JProperty version = new JProperty("version", "v2");
            JProperty width = new JProperty("width", 1024);
            JProperty height = new JProperty("height", 1024);
            JProperty image_num = new JProperty("image_num", 1);
            JProperty image = new JProperty("image", Convert.ToBase64String(this.m_Texture2D.EncodeToJPG()));
            JProperty change_degree = new JProperty("change_degree", 5);
            jObject.Add(prompt);
            jObject.Add(version);
            jObject.Add(width);
            jObject.Add(height);
            jObject.Add(image_num);
            jObject.Add(image);
            jObject.Add(change_degree);

            string postData = jObject.ToString();


            Debug.Log("11");
            UnityWebRequest uwr = new UnityWebRequest($"https://aip.baidubce.com/rpc/2.0/ernievilg/v1/txt2imgv2?access_token={this.m_Access_Token}", "POST");
            uwr.SetRequestHeader("Content-Type", "application/json");
            uwr.SetRequestHeader("Accept", "application/json");
            byte[] postBytes = System.Text.Encoding.UTF8.GetBytes(postData);
            uwr.uploadHandler = new UploadHandlerRaw(postBytes);
            uwr.downloadHandler = new DownloadHandlerBuffer();

            yield return uwr.SendWebRequest();

            if (uwr.isHttpError || uwr.isNetworkError)
            {
                Debug.LogError(uwr.error);
                yield break;
            }

            Debug.Log(uwr.downloadHandler.text);
        }
    */

    /**
        * 使用 AK，SK 生成鉴权签名（Access Token）
        * @return 鉴权签名信息（Access Token）
        */
    private IEnumerator GetAccessToken()
    {
        WWWForm wwwForm = new WWWForm();
        wwwForm.AddField("grant_type", "client_credentials");
        wwwForm.AddField("client_id", API_KEY);
        wwwForm.AddField("client_secret", SECRET_KEY);
        UnityWebRequest uwr = UnityWebRequest.Post("https://aip.baidubce.com/oauth/2.0/token", wwwForm);

        yield return uwr.SendWebRequest();

        if (uwr.isHttpError || uwr.isNetworkError)
        {
            Debug.LogError(uwr.error);
            yield break;
        }

        JObject o = JObject.Parse(uwr.downloadHandler.text);

        this.m_Access_Token = o["access_token"].ToString();
    }
}