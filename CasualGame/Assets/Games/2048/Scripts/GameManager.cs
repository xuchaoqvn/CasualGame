using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    #region Field
    /// <summary>
    /// 棋子集
    /// </summary>
    private SortedList<int, ChessPieces> m_ChessPiecess = new SortedList<int, ChessPieces>();

    /// <summary>
    /// 棋盘
    /// </summary>
    [SerializeField]
    [Header("棋盘")]
    private Chessboard m_Chessboard;

    /// <summary>
    /// 棋子预制体
    /// </summary>
    [SerializeField]
    [Header("棋子预制体")]
    private GameObject m_Prefab;

    private List<Tween> m_Tweens = new List<Tween>();

    /// <summary>
    /// 动画时长
    /// </summary>
    private float m_TweenTime = 0.15f;
    #endregion

    #region Property
    /// <summary>
    /// 是否有空格子
    /// </summary>
    private bool HasEmptyCell => this.m_ChessPiecess.Count < this.m_Chessboard.Size;
    #endregion

    private void Start()
    {
        this.m_Chessboard.InitChessboard();
        this.SpawnChessPieces(2);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        UnityEngine.InputSystem.Keyboard keyboard = UnityEngine.InputSystem.Keyboard.current;
        if (keyboard.wKey.wasPressedThisFrame)
            this.OnInputAction(0);
        else if (keyboard.dKey.wasPressedThisFrame)
            this.OnInputAction(1);
        else if (keyboard.sKey.wasPressedThisFrame)
            this.OnInputAction(2);
        else if (keyboard.aKey.wasPressedThisFrame)
            this.OnInputAction(3);
        //else if (Input.GetKeyDown(KeyCode.Q))
        //    this.SpawnChessPieces(1);
    }

    #region Function
    /// <summary>
    /// 生成棋子
    /// </summary>
    /// <param name="num"></param>
    private void SpawnChessPieces(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (!this.HasEmptyCell)
                return;
            GameObject go = Object.Instantiate<GameObject>(this.m_Prefab);
            int index = this.RandomIndex();
            go.transform.position = this.m_Chessboard.IndexToPosition(index);
            ChessPieces chessPieces = go.GetComponent<ChessPieces>();
            chessPieces.Index = index;
            this.m_ChessPiecess.Add(index, chessPieces);
        }
    }

    /// <summary>
    /// 是否是空格子
    /// </summary>
    /// <param name="index">索引</param>
    /// <returns></returns>
    private bool IsEmptyCell(int index) => !this.m_ChessPiecess.ContainsKey(index);

    /// <summary>
    /// 随机一个空格子位置
    /// </summary>
    /// <returns>空格子索引</returns>
    private int RandomIndex()
    {
        int size = this.m_Chessboard.Size;

        int index = Random.Range(0, this.m_Chessboard.Size);
        int startIndex = index;
        while (!this.IsEmptyCell(index))
        {
            index = (index + 1) % size;

            if (index == startIndex)
                throw new System.Exception("No Has Empty Cell...");
        }

        return index;
    }

    /// <summary>
    /// 当有输入动作时
    /// </summary>
    /// <param name="dir">方向</param>
    public void OnInputAction(int dir)
    {
        //上
        if (dir == 0)
            this.TryMoveUp(this.m_Chessboard.Size - this.m_Chessboard.Width - 1, -1, this.m_Chessboard.Width);
        //右
        else if (dir == 1)
            this.TryMoveRight(this.m_Chessboard.Size - 2, -1, 1);
        //下
        else if (dir == 2)
            this.TryMoveDown(this.m_Chessboard.Width, 1, -this.m_Chessboard.Width);
        //左
        else if (dir == 3)
            this.TryMoveLeft(1, 1, -1);

        if (this.m_Tweens.Count > 0)
        {
            this.m_Tweens.Clear();
            this.SpawnChessPieces(1);
        }
    }

    /// <summary>
    /// 尝试向上移动棋盘可移动棋子
    /// </summary>
    /// <param name="startIndex">起始棋子索引位置</param>
    /// <param name="deltaIndex">索引间隔</param>
    /// <param name="moveDelta">棋子移动索引间隔</param>
    private void TryMoveUp(int startIndex, int deltaIndex, int moveDelta)
    {
        int index = startIndex;
        while (index >= 0)
        {
            if (this.m_ChessPiecess.ContainsKey(index))
                this.MoveChessPieces(this.m_ChessPiecess[index], -1, this.m_Chessboard.Size, moveDelta);

            index += deltaIndex;
        }
    }

    /// <summary>
    /// 尝试向右移动棋盘可移动棋子
    /// </summary>
    /// <param name="startIndex">起始棋子索引位置</param>
    /// <param name="deltaIndex">索引间隔</param>
    /// <param name="moveDelta">棋子移动索引间隔</param>
    private void TryMoveRight(int startIndex, int deltaIndex, int moveDelta)
    {
        int index = startIndex;
        while (index >= 0)
        {
            if (index % this.m_Chessboard.Width != this.m_Chessboard.Width - 1)
            {
                if (this.m_ChessPiecess.ContainsKey(index))
                {
                    int row = index / this.m_Chessboard.Width;
                    int min = row * this.m_Chessboard.Width - 1;
                    int max = (row + 1) * this.m_Chessboard.Width;
                    this.MoveChessPieces(this.m_ChessPiecess[index], min, max, moveDelta);
                }
            }

            index += deltaIndex;
        }
    }

    /// <summary>
    /// 尝试向下移动棋盘可移动棋子
    /// </summary>
    /// <param name="startIndex">起始棋子索引位置</param>
    /// <param name="deltaIndex">索引间隔</param>
    /// <param name="moveDelta">棋子移动索引间隔</param>
    private void TryMoveDown(int startIndex, int deltaIndex, int moveDelta)
    {
        int index = startIndex;
        while (index < this.m_Chessboard.Size)
        {
            if (this.m_ChessPiecess.ContainsKey(index))
                this.MoveChessPieces(this.m_ChessPiecess[index], -1, this.m_Chessboard.Size, moveDelta);

            index += deltaIndex;
        }
    }

    /// <summary>
    /// 尝试向左移动棋盘可移动棋子
    /// </summary>
    /// <param name="startIndex">起始棋子索引位置</param>
    /// <param name="deltaIndex">索引间隔</param>
    /// <param name="moveDelta">棋子移动索引间隔</param>
    private void TryMoveLeft(int startIndex, int deltaIndex, int moveDelta)
    {
        int index = startIndex;
        while (index < this.m_Chessboard.Size)
        {
            if (index % this.m_Chessboard.Width != 0)
            {
                if (this.m_ChessPiecess.ContainsKey(index))
                {
                    int row = index / this.m_Chessboard.Width;
                    int min = row * this.m_Chessboard.Width - 1;
                    int max = (row + 1) * this.m_Chessboard.Width;
                    this.MoveChessPieces(this.m_ChessPiecess[index], min, max, moveDelta);
                }
            }

            index += deltaIndex;
        }
    }

    /// <summary>
    /// 移动棋子
    /// </summary>
    /// <param name="chessPieces">棋子</param>
    /// <param name="indexDelta">移动间隔</param>
    private void MoveChessPieces(ChessPieces chessPieces, int minIndex, int maxIndex, int indexDelta)
    {
        int index = chessPieces.Index + indexDelta;
        while (index > minIndex && index < maxIndex)
        {
            if (!this.IsEmptyCell(index))
            {
                ChessPieces left = this.m_ChessPiecess[index];
                if (this.CanMerge(left, chessPieces))
                {
                    this.Merge(left, chessPieces);
                    return;
                }
                //当前非空格子，不在继续查询是否可以合并
                break;
            }
            index += indexDelta;
        }

        index -= indexDelta;
        //位置相同
        if (index == chessPieces.Index)
            return;
        this.m_ChessPiecess.Remove(chessPieces.Index);
        chessPieces.Index = index;
        this.m_ChessPiecess.Add(index, chessPieces);
        this.TweenChessPieces(chessPieces, this.m_Chessboard.IndexToPosition(index), false);
    }

    /// <summary>
    /// 能否合并棋子
    /// </summary>
    /// <param name="left">左棋子</param>
    /// <param name="right">右棋子</param>
    /// <returns>是否是相同的棋子</returns>
    private bool CanMerge(ChessPieces left, ChessPieces right)
    {
        return left.Type == right.Type && !left.Lock && !right.Lock;
    }

    /// <summary>
    /// 合并
    /// </summary>
    /// <param name="left">左棋子</param>
    /// <param name="right">右棋子</param>
    private void Merge(ChessPieces left, ChessPieces right)
    {
        left.Type *= 2;
        left.GetComponentInChildren<TMPro.TextMeshPro>().text = left.Type.ToString();

        this.m_ChessPiecess.Remove(right.Index);
        this.TweenChessPieces(right, this.m_Chessboard.IndexToPosition(left.Index));
    }

    /// <summary>
    /// 棋子动画
    /// </summary>
    /// <param name="chessPieces">棋子</param>
    /// <param name="targetPosition">目标位置</param>
    /// <param name="destroy">销毁</param>
    private void TweenChessPieces(ChessPieces chessPieces, Vector2 targetPosition, bool destroy = true)
    {
        Tween tween = chessPieces.transform.DOMove(targetPosition, this.m_TweenTime)
        .SetAutoKill(true)
        .OnComplete(() =>
        {
            chessPieces.transform.position = targetPosition;
            if (destroy)
                Object.Destroy(chessPieces.gameObject);
        });

        this.m_Tweens.Add(tween);
    }

    /// <summary>
    /// 检查是否游戏
    /// </summary>
    /// <returns></returns>
    private bool CheckGameOver()
    {
        //if (this.m_ChessPiecess.Count == this.m_Chessboard.Size)
        //    return true;

        foreach (KeyValuePair<int, ChessPieces> item in this.m_ChessPiecess)
        {
            int upIndex = item.Key + this.m_Chessboard.Width;
            int rightIndex = item.Key + 1;
            int downIndex = item.Key - this.m_Chessboard.Width; ;
            int leftIndex = item.Key - 1;
            ChessPieces up = null;
            ChessPieces right = null;
            ChessPieces down = null;
            ChessPieces left = null;

            if (upIndex > this.m_Chessboard.Size || !this.m_ChessPiecess.ContainsKey(upIndex))
                up = null;
            if (rightIndex / this.m_Chessboard.Width != item.Key / this.m_Chessboard.Width || !this.m_ChessPiecess.ContainsKey(rightIndex))
                right = null;
            if (downIndex < 0 || !this.m_ChessPiecess.ContainsKey(downIndex))
                down = null;
            if (leftIndex / this.m_Chessboard.Width != item.Key / this.m_Chessboard.Width || !this.m_ChessPiecess.ContainsKey(leftIndex))
                left = null;

            if (up != null && this.CanMerge(up, item.Value))
                return false;
            if (right != null && this.CanMerge(right, item.Value))
                return false;
            if (down != null && this.CanMerge(down, item.Value))
                return false;
            if (left != null && this.CanMerge(left, item.Value))
                return false;
        }
        return true;
    }
    #endregion
}
