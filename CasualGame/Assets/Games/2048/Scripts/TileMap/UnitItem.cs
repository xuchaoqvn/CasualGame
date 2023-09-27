using UnityEngine;

namespace _2048
{
    /// <summary>
    /// 单位
    /// </summary>
    public class UnitItem : MonoBehaviour
    {
        #region Field
        /// <summary>
        /// 索引
        /// </summary>
        [SerializeField]
        [Header("索引")]
        private int m_Index;

        /// <summary>
        /// 类型
        /// </summary>
        [SerializeField]
        [Header("类型")]
        private int m_Type;

        /// <summary>
        /// 是否锁定
        /// </summary>
        private bool m_Lock;
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置索引
        /// </summary>
        /// <value>索引</value>
        public int Index
        {
            get => this.m_Index;
            set => this.m_Index = value;
        }

        /// <summary>
        /// 获取或设置类型
        /// </summary>
        /// <value>类型</value>
        public int Type
        {
            get => this.m_Type;
            set => this.m_Type = value;
        }

        /// <summary>
        /// 获取或设置是否锁定
        /// </summary>
        /// <value>是否锁定</value>
        public bool Lock
        {
            get => this.m_Lock;
            set => this.m_Lock = value;
        }
        #endregion

        #region Function
        /// <summary>
        /// 能否合并
        /// </summary>
        /// <param name="left">左单位</param>
        /// <param name="right">右单位</param>
        /// <returns>能否合并</returns>
        public static bool CanMerge(UnitItem left, UnitItem right)
        {
            return left.Type == right.Type && !left.Lock && !right.Lock;
        }
        #endregion
    }
}