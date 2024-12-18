using UnityEngine;

namespace Morris
{
    /// <summary>
    /// NPC���
    /// </summary>
    public class NPCController1 : MonoBehaviour
    {
        //�ǦC�����:�N�p�H�ܼ���ܦbUnity�ݩʭ��O
        [SerializeField, Header("NPC���")]
        private DataNPC1 dataNPC;
        [SerializeField, Header("�ʵe�Ѽ�")]
        private string[] paramaters =
        {
            "Ĳ�o����", "Ĳ�o�]�B", "Ĳ�o����", "Ĳ�o��"
        };
        // Unity ���ʵe����t��
        private Animator ani;

        public DataNPC1 data => dataNPC;

        // ����ƥ�G����C���ɷ|����@��
        private void Awake()
        {
            // ��o NPC ���W���ʵe���
            ani = GetComponent<Animator>();
        }

        /// <summary>
        /// ����ʵe
        /// </summary>
        /// <param name="index">�ʵe�Ѽƽs��</param>
        public void PlayAinmation(int index)
        {
            ani.SetTrigger(paramaters[index]);
        }
    }
}