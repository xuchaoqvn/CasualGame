using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditorInternal;
using System.IO;
using System.Text;
using System;
using System.Reflection;
using UnityEditor;

namespace UnityGameFramework.Extension.Editor.DataTableTools
{
    /// <summary>
    /// 数据表工具
    /// </summary>
    public class DataTableUtility
    {
        /// <summary>
        /// 获取中间位置
        /// </summary>
        /// <param name="windowWidth">窗体宽度</param>
        /// <param name="windowHeight">窗体高度</param>
        /// <returns></returns>
        public static Vector2 GetMiddlePosition(float windowWidth, float windowHeight)
        {
            return new Vector2(Screen.currentResolution.width / 2 - windowWidth / 2, Screen.currentResolution.height / 2 - windowHeight / 2);
        }

        /// <summary>
        /// 获取中间位置
        /// </summary>
        /// <param name="windowSize"></param>
        /// <returns></returns>
        public static Vector2 GetMiddlePosition(Vector2 windowSize)
        {
            return new Vector2(Screen.currentResolution.width / 2 - windowSize.x / 2, Screen.currentResolution.height / 2 - windowSize.y / 2);
        }

        /// <summary>
        /// 数据表模板
        /// </summary>
        public static List<DataTableRowData> DataTableTemplate = new List<DataTableRowData>
        {
            new DataTableRowData()
            {
                Data = new List<string>()
                {
                    "#","配置",string.Empty,string.Empty
                }
            },
            new DataTableRowData()
            {
                Data = new List<string>()
                {
                    "#","ID",string.Empty,string.Empty
                }
            },
            new DataTableRowData()
            {
                Data = new List<string>()
                {
                    "#",string.Empty,string.Empty,string.Empty
                }
            },
            new DataTableRowData()
            {
                Data = new List<string>()
                {
                    string.Empty, "0",string.Empty,string.Empty
                }
            },
        };

        /// <summary>
        /// 保存表格文件
        /// </summary>
        /// <param name="path">保存文件路径</param>
        /// <param name="data">数据信息</param>
        /// <returns>保存是否成功</returns>
        public static bool SaveDataTableFile(string path, List<DataTableRowData> data, Encoding encoding)
        {
            string dataTablePath = path;
            if (string.IsNullOrEmpty(path))
                dataTablePath = EditorUtility.SaveFilePanel("保存文件", "", "template.txt", "txt");
            using (StreamWriter sw = new StreamWriter(dataTablePath, false, encoding))
            {
                for (int i = 0; i < data.Count; i++)
                {
                    for (int j = 0; j < data[i].Data.Count; j++)
                    {
                        sw.Write(data[i].Data[j]);

                        if (j != data[i].Data.Count - 1)
                        {
                            sw.Write("\t");
                        }
                    }

                    if (i != data.Count - 1)
                    {
                        sw.WriteLine();
                    }
                }
            }

            //EditorUtility.DisplayDialog("提示", "保存成功!", "ojbk");

            AssetDatabase.Refresh();

            return true;
        }

        /// <summary>
        /// 加载数据表文件
        /// </summary>
        /// <param name="dataTablePath">数据表路径</param>
        /// <param name="encoding">编码</param>
        /// <param name="row">行</param>
        /// <param name="col">列</param>
        /// <returns>数据表集合</returns>
        public static List<DataTableRowData> LoadDataTableFile(string dataTablePath, Encoding encoding, out Vector2 RowCol)
        {
            RowCol = Vector2.zero;
            if (File.Exists(dataTablePath) == false)
            {
                EditorUtility.DisplayDialog("提示", "文件路径不存在", "确定");
                return null;
            }

            List<DataTableRowData> data = new List<DataTableRowData>();
            using (StreamReader sr = new StreamReader(dataTablePath, encoding))
            {
                while (sr.EndOfStream == false)
                {
                    // UTF8Encoding utf8 = new UTF8Encoding();
                    string line = sr.ReadLine();
                    string[] splited = line.Split('\t');
                    DataTableRowData roeData = new DataTableRowData();

                    for (int i = 0; i < splited.Length; i++)
                        roeData.Data.Add(splited[i]);

                    if (splited.Length > RowCol.y)
                        RowCol.y = splited.Length;

                    data.Add(roeData);
                    RowCol.x++;
                }
            }

            return data;
        }
    }

    /// <summary>
    /// 数据表行数据
    /// </summary>
    [Serializable]
    public class DataTableRowData
    {
        /// <summary>
        /// 数据
        /// </summary>
        public List<string> Data { get; set; }

        public DataTableRowData()
        {
            this.Data = new List<string>();
        }
    }

    public static class DataTableEditor
    {
        /// <summary>
        /// 新建数据表
        /// </summary>
        [MenuItem("Game Framework/DataTable/New DataTable", false, 50)]
        private static void NewDataTable()
        {
            DataTableEditor.OpenDataTableEditorWindow(string.Empty);
        }

        /// <summary>
        /// 加载数据表
        /// </summary>
        [MenuItem("Game Framework/DataTable/Load DataTable", false, 51)]
        private static void LoadDataTable()
        {
            string filePath = EditorUtility.OpenFilePanel("加载数据表格文件", Application.dataPath, "txt");
            if (!string.IsNullOrEmpty(filePath))
                DataTableEditor.OpenDataTableEditorWindow(filePath);
            else
                Debug.Log("取消选择数据表");
        }

        /// <summary>
        /// 打开数据表编辑窗口
        /// </summary>
        /// <param name="path">数据表路径</param>
        public static void OpenDataTableEditorWindow(string path)
        {
            FileInfo fileInfo;
            string dataTableName = string.Empty;
            if (!string.IsNullOrEmpty(path))
            {
                fileInfo = new FileInfo(path);
                dataTableName = fileInfo.Name;
            }
            DataTableEditorWindow dataTableEditingWindow;
#if UNITY_2019_1_OR_NEWER
            dataTableEditingWindow = EditorWindowUtility.CreateWindow<DataTableEditorWindow>(dataTableName);
#else
            dataTableEditingWindow = EditorWindowUtility.CreateWindow<DataTableEditingWindow>(dataTableName);
#endif
            dataTableEditingWindow.Init(path);
            dataTableEditingWindow.Show();
        }
    }

    /// <summary>
    /// 数据表编辑窗口
    /// </summary>
    public class DataTableEditorWindow : EditorWindow
    {
        #region Field
        /// <summary>
        /// 原数据表行数据
        /// </summary>
        private List<DataTableRowData> m_RowDatas;

        /// <summary>
        /// 缓存数据表行数据
        /// </summary>
        private List<DataTableRowData> m_RowDatasTemp;

#if UNITY_2020_1_OR_NEWER
        private UnityInternalBridge.ReorderableList reorderableList;
#else
        /// <summary>
        /// 可重新排序的列表框
        /// </summary>
        private ReorderableList m_ReorderableList;
#endif

        /// <summary>
        /// 数据表文件路径
        /// </summary>
        private string m_DataTablePath;

        /// <summary>
        /// 是否高亮模式
        /// </summary>
        public int m_LightMode = 0;

        /// <summary>
        /// 主题
        /// </summary>
        public string Theme = "LODCameraLine";

        /// <summary>
        /// 初始滚动位置
        /// </summary>
        private Vector2 m_ScrollViewPos;

        /// <summary>
        /// 源行列
        /// </summary>
        private Vector2 m_OriginalRowCol;

        /// <summary>
        /// 现行列
        /// </summary>
        private Vector2 m_CurrentRowCol;
        #endregion

        /// <summary>
        /// 初始化窗口
        /// </summary>
        /// <param name="dataTablePath">数据表路径</param>
        public void Init(string dataTablePath)
        {
            this.m_DataTablePath = dataTablePath;
            //新建数据表
            if (string.IsNullOrEmpty(dataTablePath))
            {
                this.m_RowDatas = DataTableUtility.DataTableTemplate;
                this.m_OriginalRowCol = Vector2.one * 4;
            }
            else
                this.m_RowDatas = DataTableUtility.LoadDataTableFile(this.m_DataTablePath, Encoding.UTF8, out this.m_OriginalRowCol);
            this.m_CurrentRowCol = this.m_OriginalRowCol;

            if (this.m_RowDatas == null)
                return;

            this.m_RowDatasTemp = new List<DataTableRowData>();
            for (int i = 0; i < this.m_RowDatas.Count; i++)
            {
                DataTableRowData data = new DataTableRowData();
                for (int j = 0; j < this.m_RowDatas[i].Data.Count; j++)
                    data.Data.Add(m_RowDatas[i].Data[j]);

                this.m_RowDatasTemp.Add(data);
            }

            this.m_LightMode = EditorPrefs.GetInt("DataTableEditor_" + Application.productName + "_LightMode", 0);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("box", GUILayout.Height(position.height));
            {
                this.m_ScrollViewPos = GUILayout.BeginScrollView(this.m_ScrollViewPos);
                {
                    if (this.m_RowDatasTemp == null || this.m_RowDatasTemp.Count == 0)
                    {
                        this.Close();
                        GUILayout.EndScrollView();
                        EditorGUILayout.EndVertical();
                        return;
                    }

                    if (m_LightMode == 0)
                        Theme = "ScriptText";
                    else if (m_LightMode == 1)
                        Theme = "PreferencesSectionBox";

                    if (this.m_ReorderableList == null)
                    {
#if UNITY_2020_1_OR_NEWER
                reorderableList = new UnityInternalBridge.ReorderableList(RowDatas, typeof(List<DataTableRowData>), true, false, true,true);
#else
                        this.m_ReorderableList = new ReorderableList(this.m_RowDatasTemp, typeof(List<DataTableRowData>), true, false, true, true);
#endif
                        //当绘制元素时
                        this.m_ReorderableList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
                        {
                            for (int i = 0; i < this.m_RowDatasTemp[index].Data.Count; i++)
                            {
                                rect.width = (this.position.width - 20) / this.m_RowDatasTemp[index].Data.Count;
                                rect.x = rect.width * i + 20;
                                this.m_RowDatasTemp[index].Data[i] = EditorGUI.TextField(rect, string.Empty, this.m_RowDatasTemp[index].Data[i], this.Theme);
                            }
                        };

                        //当点击添加时
                        this.m_ReorderableList.onAddCallback = list =>
                        {
                            int result = EditorUtility.DisplayDialogComplex("提示", "添加 行 或 列", "行", "取消", "列");

                            //行
                            if (result == 0)
                            {
                                DataTableRowData dataTableRowData = new DataTableRowData();
                                dataTableRowData.Data = new List<string>();
                                for (int i = 0; i < this.m_RowDatasTemp[0].Data.Count; i++)
                                    dataTableRowData.Data.Add(string.Empty);
                                this.m_RowDatasTemp.Add(dataTableRowData);
                                this.m_CurrentRowCol.x++;
                            }
                            //列
                            else if (result == 2)
                            {
                                for (int i = 0; i < this.m_RowDatasTemp.Count; i++)
                                    this.m_RowDatasTemp[i].Data.Add(string.Empty);
                                this.m_CurrentRowCol.y++;
                            }
                            //取消
                            else if (result == 1)
                            {
                                //Debug.Log("取消添加。");
                            }
                        };

                        //当点击移除时
                        this.m_ReorderableList.onRemoveCallback = list =>
                        {
                            int result = EditorUtility.DisplayDialogComplex("提示", "移除 行 或 列", "行", "取消", "列");

                            //行
                            if (result == 0)
                            {
                                this.m_RowDatasTemp.RemoveAt(list.index);
                                this.m_CurrentRowCol.x--;
                            }
                            //列
                            else if (result == 2)
                            {
                                for (int i = 0; i < this.m_RowDatasTemp.Count; i++)
                                    this.m_RowDatasTemp[i].Data.RemoveAt(this.m_RowDatasTemp[i].Data.Count - 1);
                                this.m_CurrentRowCol.y--;
                            }
                            //取消
                            else if (result == 1)
                            {
                                //Debug.Log("取消删除");
                            }
                        };

                        //当绘制表头时
                        this.m_ReorderableList.drawHeaderCallback = rect =>
                        {
                            rect.x += 3;
                            EditorGUI.LabelField(rect, $"文件路径:{this.m_DataTablePath}");
                            rect.x = rect.width - 70;
                            EditorGUI.LabelField(rect, "高亮模式");
                            rect.x = rect.width - 15;

                            this.m_LightMode = EditorGUI.Toggle(rect, this.m_LightMode == 0 ? true : false) ? 0 : 1;
                            EditorPrefs.SetInt("DataTableEditor_" + Application.productName + "_LightMode", this.m_LightMode);
                        };
                    }
                    this.m_ReorderableList.DoLayoutList();

                    GUILayout.Space(5f);
                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayout.Label(string.Empty);
                        if (GUILayout.Button("保存", GUILayout.Width(100f), GUILayout.Height(25)))
                        {
                            if (this.IsEdited())
                            {
                                DataTableUtility.SaveDataTableFile(this.m_DataTablePath, this.m_RowDatasTemp, Encoding.UTF8);
                                this.m_OriginalRowCol = this.m_CurrentRowCol;
                                for (int i = 0; i < this.m_RowDatasTemp.Count; i++)
                                {
                                    for (int j = 0; j < this.m_RowDatasTemp[i].Data.Count; j++)
                                        this.m_RowDatas[i].Data[j] = this.m_RowDatasTemp[i].Data[j];
                                }
                            }
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                }
                GUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 设置数据被修改
        /// </summary>
        private bool IsEdited()
        {
            if (this.m_CurrentRowCol != this.m_OriginalRowCol)
                return true;

            for (int i = 0; i < this.m_OriginalRowCol.x; i++)
            {
                for (int j = 0; j < this.m_OriginalRowCol.y; j++)
                {
                    if (this.m_RowDatas[i].Data[j] != this.m_RowDatasTemp[i].Data[j])
                        return true;
                }
            }
            return false;
        }

        private void OnDisable()
        {
            if (!this.IsEdited())
                return;

            if (EditorUtility.DisplayDialog("提示", "你已经对表格进行了修改，是否需要保存？", "是", "否"))
                DataTableUtility.SaveDataTableFile(this.m_DataTablePath, this.m_RowDatasTemp, Encoding.UTF8);
        }
    }
}