using UnityEngine;

namespace Morris
{
    /// <summary>
    /// NPC控制器
    /// </summary>
    public class NPCController1 : MonoBehaviour
    {
        //序列化欄位:將私人變數顯示在Unity屬性面板
        [SerializeField, Header("NPC資料")]
        private DataNPC1 dataNPC;
        [SerializeField, Header("動畫參數")]
        private string[] paramaters =
        {
            "觸發攻擊", "觸發跑步", "觸發走路", "觸發跳"
        };
        // Unity 的動畫控制系統
        private Animator ani;

        public DataNPC1 data => dataNPC;

        // 喚醒事件：播放遊戲時會執行一次
        private void Awake()
        {
            // 獲得 NPC 身上的動畫控制器
            ani = GetComponent<Animator>();
        }

        /// <summary>
        /// 播放動畫
        /// </summary>
        /// <param name="index">動畫參數編號</param>
        public void PlayAinmation(int index)
        {
            ani.SetTrigger(paramaters[index]);
        }
    }
}