using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace KID
{
    /// <summary>
    /// 模型管理器：芸琤老師提供的 uCRL 版本
    /// </summary>
    public class ModelManager : MonoBehaviour
    {
        /// <summary>
        /// 模型連結
        /// </summary>
        private string url = "https://g.ubitus.ai/v1/chat/completions";
        /// <summary>
        /// 模型金鑰
        /// </summary>
        private string key = "d4pHv5n2G3q2vkVMPen3vFMfUJic4huKiQbvMmGLWUVIr/ptUuGnsCx/zVdYmVtdrGPO9//2h8Fbp6HkSQ0/oA==";
        /// <summary>
        /// 角色設定
        /// </summary>
        private string role = "你是一個工業時代機器人";

        [SerializeField, Header("文字結果")]
        private TMP_Text textResult;

        #region 模型處理區域
        [SerializeField, Header("輸入欄位")]
        private TMP_InputField inputField;

        private string prompt;

        // 喚醒事件：遊戲播放後會執行一次
        private void Awake()
        {
            // 當玩家結束編輯輸入欄位時會執行 PlayerInput 方法
            inputField.onEndEdit.AddListener(PlayerInput);
        }

        private void PlayerInput(string input)
        {
            print($"<color=#3f3>{input}</color>");
            prompt = input;
            // 啟動協同程序 獲得結果
            StartCoroutine(GetResult());
        }

        private IEnumerator GetResult()
        {
            var data = new
            {
                model = "llama-3.1-70b",
                messages = new[]
                {
                    new
                    {
                        name = "user",
                        content = prompt,
                        role = this.role
                    }
                },
                stop = new string[] { "<|eot_id|>", "<|end_of_text|>" },
                frequency_penalty = 0,
                max_tokens = 100,
                temperature = 0.2,
                top_p = 0.5,
                top_k = 20,
                stream = false
            };

            string json = JsonConvert.SerializeObject(data);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            UnityWebRequest request = new UnityWebRequest(url, "POST");
            request.uploadHandler = new UploadHandlerRaw(postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest();


            if (request.result != UnityWebRequest.Result.Success)
            {
                print($"<color=#f11>{request.error}</color>");
            }
            else
            {
                print($"<color=#39f>{request.result}</color>");

                string reponseText = request.downloadHandler.text;
                JObject jObject = JObject.Parse(reponseText);
                string content = jObject["choices"][0]["message"]["content"].ToString();

                print($"<color=#f96>結果：{content}</color>");
                Interaction(content);
            }
        }
        #endregion

        /// <summary>
        /// 互動
        /// </summary>
        /// <param name="response">這是模型回傳的文字結果</param>
        private void Interaction(string response)
        {
            textResult.text = response;
        }
    }
}
