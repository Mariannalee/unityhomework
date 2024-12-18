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
    /// �ҫ��޲z��
    /// </summary>
   public class Modelmanager1 : MonoBehaviour
   {
        /// <summary>
        /// �ҫ��q��
        /// </summary>
        private string url = "https://g.ubitus.ai/v1/chat/completions";
        /// <summary>
        /// �ҫ����_
        /// </summary>
        private string key = "Bearer d4pHv5n2G3q2vkVMPen3vFMfUJic4huKiQbvMmGLWUVIr/ptUuGnsCx/zVdYmVtdrGPO9//2h8Fbp6HkSQ0/oA==";
        /// <summary>
        /// ����]�w
        /// </summary>
        private TMP_InputField inputField;
        private string prompt;
        private string role = "�A�O�@������H";

        [SerializeField, Header("��r���G")]
        private TMP_Text textResult;

        //����ƥ�:�C�������|����@��
        private void Awake()
        {
            //�M������W�W�٬� ��J��� ������æs���inputField���ܼƤ�
            inputField = GameObject.Find("��J���").GetComponent<TMP_InputField>();
            //���a�����s���J���ɷ|���� PlayerInput ��k
            inputField.onEndEdit.AddListener(PlayerInput);
        }
        private void PlayerInput(string input)
        {
            print ($"<color=#3f3>{input}</color>");
            prompt=input;
            // �Ұʨ�P�{�� ��o���G
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

            //�N����ର json �H�ΤW�Ǫ� byte[] �榡
            string json = JsonUtility.ToJson(data);
            byte[] postData = Encoding.UTF8.GetBytes(json);
            //�z�L POST �N��ƶǻ���ҫ����A���ó]�w���D
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
        /// ����
        /// </summary>
        /// <param name="response">�o�O�ҫ��Ǧ^����r���G</param>
        private void Interacton(string response)
        {
            // �N�ҫ���X���G�L�b Unity ����r�����W
            textResult.text = response;
        }
    }
}