using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chessboard : MonoBehaviour
{
    #region Field
    /// <summary>
    /// 棋盘行数
    /// </summary>
    [SerializeField]
    [Header("棋盘行数")]
    [Range(3, 8)]
    private int m_Row = 3;

    /// <summary>
    /// 棋盘列数
    /// </summary>
    [SerializeField]
    [Header("棋盘列数")]
    [Range(3, 6)]
    private int m_Col = 3;

    /// <summary>
    /// 棋盘格精灵
    /// </summary>
    [SerializeField]
    [Header("棋盘格精灵")]
    private Sprite m_CellSprite;

    /// <summary>
    /// 棋盘格开始坐标
    /// </summary>
    [SerializeField]
    private Vector2 m_StartPosition;

    private float m_Delta = 5.0f;
    #endregion

    #region Property
    /// <summary>
    /// 宽
    /// </summary>
    public int Width => this.m_Col;

    /// <summary>
    /// 高
    /// </summary>
    public int Height => this.m_Row;

    /// <summary>
    /// 棋盘大小
    /// </summary>
    public int Size => this.m_Row * this.m_Col;
    #endregion

    #region Function
    /// <summary>
    /// 初始化棋盘
    /// </summary>
    public void InitChessboard()
    {
        float delta = this.m_Delta;
        float halfDelta = this.m_Delta * 0.5f;

        float startX = this.m_Col % 2 == 0 ? this.m_Col / 2 * delta - halfDelta : this.m_Col / 2 * delta;
        float startY = this.m_Row % 2 == 0 ? this.m_Row / 2 * delta - halfDelta : this.m_Row / 2 * delta;

        this.m_StartPosition = new Vector2(startX * -1, startY * -1);
        Color cellColor = new Color(205.0f / 255.0f, 193.0f / 255.0f, 180.0f / 255.0f, 1.0f);

        for (int i = 0; i < this.m_Row; i++)
        {
            for (int j = 0; j < this.m_Col; j++)
            {
                float x = this.m_StartPosition.x + j * delta;
                float y = this.m_StartPosition.y + i * delta;
                Vector2 spawnPosition = new Vector2(x, y);

                GameObject go = new GameObject($"{i} {j}", typeof(SpriteRenderer));
                SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
                renderer.sprite = this.m_CellSprite;
                renderer.color = cellColor;
                renderer.sortingOrder = 1;
                renderer.transform.SetParent(this.transform);
                renderer.transform.localPosition = spawnPosition;
            }
        }

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        float sizeX = this.m_Col * delta;
        float sizeY = this.m_Row * delta;
        spriteRenderer.size = new Vector2(sizeX, sizeY);
    }

    /// <summary>
    /// 由索引获取坐标
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns>坐标</returns>
    public Vector2 IndexToPosition(int index)
    {
        float xIndex = index % this.m_Col;
        float yIndex = index / this.m_Col;

        float x = this.m_StartPosition.x + xIndex * this.m_Delta;
        float y = this.m_StartPosition.y + yIndex * this.m_Delta;
        Vector2 position = new Vector2(x, y);
        return position;
    }

    //public int PositionToIndex(Vector2 position)
    //{

    //}
    #endregion
}
