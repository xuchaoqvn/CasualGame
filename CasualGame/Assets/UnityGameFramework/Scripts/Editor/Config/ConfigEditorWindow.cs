using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UnityGameFramework.Extension.Editor
{
    internal class ConfigData
    {
        /// <summary>
        /// Key
        /// </summary>
        private string m_Key;

        /// <summary>
        /// 字符串
        /// </summary>
        private string m_Value;

        public ConfigData()
        {

        }


        public string Key
        {
            get => this.m_Key;
            set => this.m_Key = value;
        }

        public string Value
        {
            get => this.m_Value;
            set => m_Value = value;
        }
    }

    /// <summary>
    /// 配置编辑窗口
    /// </summary>
    public class ConfigEditorWindow : EditorWindow
    {
        #region Field
        /// <summary>
        /// 配置数据
        /// </summary>
        private List<ConfigData> m_ConfigDatas;

        /// <summary>
        /// 排序列表
        /// </summary>
        private ReorderableList m_ReorderableList;

        /// <summary>
        /// Foldout
        /// </summary>
        private HashSet<int> m_Foldout = new HashSet<int>();

        /// <summary>
        /// 初始滚动位置
        /// </summary>
        private Vector2 m_ScrollPosition;

        /// <summary>
        /// 保存路径
        /// </summary>
        private string m_SavePath;
        #endregion

        #region Function
        /// <summary>
        /// 新建数据表
        /// </summary>
        [MenuItem("Game Framework/Config/New ConfigAsset", false, 50)]
        private static void NewConfigAsset()
        {
            ConfigEditorWindow configEditorWindow = ScriptableObject.CreateInstance<ConfigEditorWindow>();
            configEditorWindow.titleContent = new GUIContent("New ConfigAsset");
            configEditorWindow.Init(string.Empty);
            configEditorWindow.Show();
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        [MenuItem("Game Framework/Config/Load ConfigAsset", false, 51)]
        private static void LoadConfigAsset()
        {
            string assetPath = EditorUtility.OpenFilePanel("Load ConfigAsset", Application.dataPath, "bytes");
            if (string.IsNullOrEmpty(assetPath))
                return;

            ConfigEditorWindow configEditorWindow = ScriptableObject.CreateInstance<ConfigEditorWindow>();
            configEditorWindow.titleContent = new GUIContent("New ConfigAsset");
            configEditorWindow.Init(assetPath);
            configEditorWindow.Show();
        }

        internal void Init(string configPath)
        {
            this.m_SavePath = configPath;
            this.m_ConfigDatas = new List<ConfigData>();
            //新建配置文件
            if (string.IsNullOrEmpty(this.m_SavePath))
                this.NewConfig();
            //修改配置文件
            else
                this.LoadConfig();

            this.m_ReorderableList = new ReorderableList(this.m_ConfigDatas, typeof(IList), true, true, true, true);
            this.m_ReorderableList.headerHeight = 20.0f;
            this.m_ReorderableList.drawHeaderCallback += this.OnDrawHeader;
            this.m_ReorderableList.onAddCallback += this.OnConfigAdd;
            this.m_ReorderableList.onRemoveCallback += this.OnConfigRemove;
            this.m_ReorderableList.drawElementCallback += this.OnConfigDrawElement;
            this.m_ReorderableList.drawNoneElementCallback += this.OnConfigDrawNoneElement;
            this.m_ReorderableList.elementHeightCallback += this.OnConfigElementHeight;
        }

        /// <summary>
        /// 新建配置文件
        /// </summary>
        private void NewConfig()
        {
            this.m_ConfigDatas.Clear();
        }

        /// <summary>
        /// 修改配置文件
        /// </summary>
        private void LoadConfig()
        {
            if (!this.m_SavePath.EndsWith(".bytes"))
            {
                Debug.LogError("File format error.");
                return;
            }

            byte[] data = System.IO.File.ReadAllBytes(this.m_SavePath);
            using (MemoryStream memoryStream = new MemoryStream(data, 0, data.Length, false))
            {
                using (BinaryReader binaryReader = new BinaryReader(memoryStream, Encoding.UTF8))
                {
                    while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
                    {
                        string configName = binaryReader.ReadString();
                        string configValue = binaryReader.ReadString();

                        bool boolValue = false;
                        bool.TryParse(configValue, out boolValue);

                        int intValue = 0;
                        int.TryParse(configValue, out intValue);

                        float floatValue = 0f;
                        float.TryParse(configValue, out floatValue);

                        ConfigData configData = new ConfigData();
                        configData.Key = configName;
                        configData.Value = configValue;
                        this.m_ConfigDatas.Add(configData);
                    }
                }
            }
        }

        /// <summary>
        /// 保存配置
        /// </summary>
        private void SaveConfig()
        {
            using (FileStream fileStream = new FileStream(this.m_SavePath, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream, Encoding.UTF8))
                {
                    for (int i = 0; i < this.m_ConfigDatas.Count; i++)
                    {
                        ConfigData configData = this.m_ConfigDatas[i];
                        if (string.IsNullOrEmpty(configData.Key) || string.IsNullOrEmpty(configData.Value))
                            continue;
                        binaryWriter.Write(configData.Key);
                        binaryWriter.Write(configData.Value);
                    }
                }
            }
        }

        private void OnGUI()
        {
            this.m_ScrollPosition = EditorGUILayout.BeginScrollView(this.m_ScrollPosition);
            this.m_ReorderableList.DoLayoutList();
            EditorGUILayout.BeginHorizontal();
            {
                GUILayout.Label(string.Empty);
                if (GUILayout.Button("保存", GUILayout.Width(100f), GUILayout.Height(25.0f)))
                {
                    if (string.IsNullOrEmpty(this.m_SavePath))
                        this.m_SavePath = EditorUtility.SaveFilePanel("Save Config", string.Empty, "NewConfig.bytes", "bytes");
                    this.SaveConfig();
                }
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        /// <summary>
        /// 当绘制表头是
        /// </summary>
        /// <param name="rect"></param>
        private void OnDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, $"ConfigPath:{this.m_SavePath}");
        }

        /// <summary>
        /// 当增加配置时
        /// </summary>
        /// <param name="list">可排序列表</param>
        private void OnConfigAdd(ReorderableList list)
        {
            this.m_ReorderableList.list.Add(new ConfigData());
        }

        /// <summary>
        /// 当移除配置时
        /// </summary>
        /// <param name="list">可排序列表</param>
        private void OnConfigRemove(ReorderableList list)
        {
            this.m_ReorderableList.list.RemoveAt(list.index);
        }

        /// <summary>
        /// 当绘制配置元素时
        /// </summary>
        /// <param name="rect">绘制区域</param>
        /// <param name="index">元素索引</param>
        /// <param name="isActive">是否激活</param>
        /// <param name="isFocused">是否聚焦</param>
        private void OnConfigDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            ConfigData configData = (ConfigData)this.m_ReorderableList.list[index];
            int configHashCode = configData.GetHashCode();

            bool constFoldout = this.m_Foldout.Contains(configHashCode);
            string foldoutContent = string.IsNullOrEmpty(configData.Key) || string.IsNullOrEmpty(configData.Value) ? "Invalid ConfigData" : configData.Key;
            if (constFoldout != EditorGUI.Foldout(new Rect(rect.x, rect.y, rect.width, 20.0f), constFoldout, foldoutContent))
            {
                constFoldout = !constFoldout;
                if (constFoldout)
                    this.m_Foldout.Add(configHashCode);
                else
                    this.m_Foldout.Remove(configHashCode);
            }

            if (constFoldout)
            {
                //水平偏移15
                rect.x += 15.0f;
                //垂直偏移20
                rect.y += 20.0f;
                //总宽度减少15
                rect.width -= 15.0f;
                //总高度减少25（20+5）
                rect.height -= 25.0f;

                rect.width -= (30.0f + 5.0f + 40.0f);

                Rect keyLableRect = new Rect(rect.x, rect.y, 30.0f, 20.0f);
                Rect keyRect = new Rect(keyLableRect.x + keyLableRect.width, rect.y, rect.width * 0.5f, 20.0f);
                Rect valueLableRect = new Rect(keyRect.x + keyRect.width + 5.0f, rect.y, 40.0f, 20.0f);
                Rect valueRect = new Rect(valueLableRect.x + valueLableRect.width, rect.y, rect.width * 0.5f, 20.0f);
                EditorGUI.LabelField(keyLableRect, "Key:");
                string key = EditorGUI.TextField(keyRect, configData.Key);
                if (this.m_ConfigDatas.Exists(config => config.Key == key) && key != configData.Key)
                    Debug.LogError($"An item with the same key has already been added. Key: {key}");
                else
                    configData.Key = key;
                EditorGUI.LabelField(valueLableRect, "Value:");
                configData.Value = EditorGUI.TextField(valueRect, configData.Value);
            }
        }

        /// <summary>
        /// 当绘制无配置时
        /// </summary>
        /// <param name="rect">绘制区域</param>
        private void OnConfigDrawNoneElement(Rect rect)
        {
            EditorGUI.LabelField(rect, "当前无配置。。。");
        }

        /// <summary>
        /// 绘制配置高度
        /// </summary>
        /// <param name="index">元素索引</param>
        /// <returns>高度</returns>
        private float OnConfigElementHeight(int index)
        {
            object configData = this.m_ReorderableList.list[index];
            int configDataHashCode = configData.GetHashCode();

            bool configDataFoldout = this.m_Foldout.Contains(configDataHashCode);
            return configDataFoldout ? 40.0f : 20.0f;
        }
        #endregion
    }
}