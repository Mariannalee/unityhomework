using UnityEngine;

namespace James
{
    /// <summary>
    /// NPC資料
    /// </summary>
    [CreateAssetMenu(menuName = "James/NPC")]
    public class DataNPC : ScriptableObject
    {
        [Header("NPC要分析的句子")]
        public string[] sentences;
    }
}