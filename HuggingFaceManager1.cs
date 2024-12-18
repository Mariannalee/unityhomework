using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Morris
{
    /// <summary>
    /// 模型管理器
    /// </summary>
    public class HuggingFaceManager1 : MonoBehaviour
    {
        private string url = "https://api-inference.huggingface.co/models/sentence-transformers/all-MiniLM-L6-v2";
        private string key = "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";

        private TMP_InputField inputField;
        private string prompt;
        private string role = "你是一位機器人";
        private string[] npcSentences;

        [SerializeField, Header("NPC 物件")]
        private NPCController1 npc;

        //喚醒事件:遊戲播放後會執行一次
        private void Awake()
        {
            //尋找場景上名稱為 輸入欄位 的物件並存放到inputField的變數內
            inputField = GameObject.Find("輸入欄位").GetComponent<TMP_InputField>();
            //當玩家結束編輯輸入欄位時會執行 PlayerInput 方法
            inputField.onEndEdit.AddListener(PlayerInput);
            //獲得 NPC 要分析的語句
            npcSentences = npc.data.sentences;
        }

        private void PlayerInput(string input)
        {
            print ($"<color=#3f3>{input}</color>");
            prompt=input;
            // 啟動協同程序 獲得結果
            StartCoroutine(GetResult());
        }
        private IEnumerator GetResult()
        {
            var inputs = new 
            {
               source_sentence = prompt,
               sentence = npcSentences
            };

            //將資料轉為json以及上傳的byte[]格式
            string json = JsonConvert.SerializeObject(inputs);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            //透過 POST 將資料傳遞到模型伺服器並設定標題
            UnityWebRequest request = new UnityWebRequest (url,"Post");
            request.uploadHandler = new UploadHandlerRaw (postData);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + key);

            yield return request.SendWebRequest ();

            if (request.result != UnityWebRequest.Result.Success)
            {
                print($"<color=#f33>要求失敗：{request.error}<color>");
            }
            else
            {
                string responseText = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<List<float>>(responseText);

                print($"<color=#3f3>分數：{responseText}<color>");

                if (response != null && response.Count >0)
                { 
                    int best = response.Select((value, index) => new
                    {
                        Value = value, Index = index
                    }).OrderByDescending(x  => x.Value).First().Index;

                    print($"<color=#37f>最佳答案：{npcSentences[best]}<color>");

                    npc.PlayAinmation(best);
                }
            }
        }    

    }
}