using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Morris
{
    /// <summary>
    /// 模型管理器
    /// </summary>
   public class Modelmanager1 : MonoBehaviour
   {
        /// <summary>
        /// 模型通路
        /// </summary>
        private string url = "https://g.ubitus.ai/v1/chat/completions";
        /// <summary>
        /// 模型金鑰
        /// </summary>
        private string key = "Bearer d4pHv5n2G3q2vkVMPen3vFMfUJic4huKiQbvMmGLWUVIr/ptUuGnsCx/zVdYmVtdrGPO9//2h8Fbp6HkSQ0/oA==";
        /// <summary>
        /// 角色設定
        /// </summary>
        private TMP_InputField inputField;
        private string prompt;
        private string role = "你是一位機器人";

        [SerializeField, Header("文字結果")]
        private TMP_Text textResult;

        //喚醒事件:遊戲播放後會執行一次
        private void Awake()
        {
            //尋找場景上名稱為 輸入欄位 的物件並存放到inputField的變數內
            inputField = GameObject.Find("輸入欄位").GetComponent<TMP_InputField>();
            //當玩家結束編輯輸入欄位時會執行 PlayerInput 方法
            inputField.onEndEdit.AddListener(PlayerInput);
        }
        private void PlayerInput(string input)
        {
            print ($"<color=#3f3>{input}</color>");
            prompt=input;
            // 啟動協同程序 獲得結果
            StartCoroutine(GetResult() );
        }
        private IEnumerator GetResult()
        {
            var data= new 
            {
               model = "11ama-3.1-70b",
               massages = new
               {
                   name = "user",
                   content = prompt,
                   role = this.role
               },
               stop = new string[] { "<|eot.id|>", "<|end_of_text" },
               freqency_penalty = 0,
               max_tokens = 2000,
               temperature = 0.2,
               top_p = 0.5,
               top_k = 20,
               stream = false
            };

            //將資料轉為 json 以及上傳的 byte[] 格式
            string json = JsonUtility.ToJson(data);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            //透過 POST 將資料傳遞到模型伺服器並設定標題
            UnityWebRequest request = new UnityWebRequest (url,"Post");
            request.uploadHandler = new UploadHandlerRaw (postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest ();

            print(request.result);
            print (request.error);
        }    
        
        /// <summary>
        /// 互動
        /// </summary>
        /// <param name="response">這是模型傳回的文字結果</param>
        private void Interacton(string response)
        {
            // 將模型輸出結果印在 Unity 的文字介面上
            textResult.text = response;
        }
    }
}