using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using GameFramework.ObjectPool;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace _2048
{
    /// <summary>
    /// 2048组件
    /// </summary>
    public partial class _2048Component : GameFrameworkComponent
    {
        #region Field
        /// <summary>
        /// 宽
        /// </summary>
        private int m_Width;

        /// <summary>
        /// 高
        /// </summary>
        private int m_Height;
        #endregion

        #region Property
        /// <summary>
        /// 获取或设置宽度
        /// </summary>
        /// <value>宽度</value>
        public int Width
        {
            get => this.m_Width;
            set => this.m_Width = value;
        }

        /// <summary>
        /// 获取或设置高度
        /// </summary>
        /// <value>高度</value>
        public int height
        {
            get => this.m_Height;
            set => this.m_Height = value;
        }

        /// <summary>
        /// 获取地图单位大小
        /// </summary>
        public int Size => this.m_Width * this.m_Height;
        #endregion
    }

    /// <summary>
    /// 2048组件
    /// </summary>
    public partial class _2048Component : GameFrameworkComponent
    {
        #region Field
        /// <summary>
        /// 间隔
        /// </summary>
        private float m_MapDelta = 5.0f;

        /// <summary>
        /// 地图开始坐标
        /// </summary>
        private Vector2 m_StartPosition;

        /// <summary>
        /// 精灵
        /// </summary>
        private Sprite m_Sprite;

        /// <summary>
        /// 地图根节点
        /// </summary>
        [SerializeField]
        private Transform m_TileInstanceRoot = null;

        /// <summary>
        /// 地图单位对象池容量
        /// </summary>
        [SerializeField]
        private int m_MapItemCapacity = 16;

        /// <summary>
        /// 地图单位对象池
        /// </summary>
        private IObjectPool<TileItemObject> m_MapItemObjectPool = null;

        /// <summary>
        /// 激活的地图单位
        /// </summary>
        private List<SpriteRenderer> m_ActiveMapItems = null;

        #endregion

        #region Function
        /// <summary>
        /// 生成地图
        /// </summary>
        public void CreatMap(int width, int height)
        {
            if (this.m_Width == width && this.m_Height == height)
                return;

            this.m_Width = width;
            this.m_Height = height;

            float delta = this.m_MapDelta;
            float halfDelta = this.m_MapDelta * 0.5f;

            float startX = this.m_Width % 2 == 0 ? this.m_Width / 2 * delta - halfDelta : this.m_Width / 2 * delta;
            float startY = this.m_Height % 2 == 0 ? this.m_Height / 2 * delta - halfDelta : this.m_Height / 2 * delta;

            this.m_StartPosition = new Vector2(startX * -1, startY * -1);
            Color cellColor = new Color(205.0f / 255.0f, 193.0f / 255.0f, 180.0f / 255.0f, 1.0f);

            for (int i = 0; i < this.m_Height; i++)
            {
                for (int j = 0; j < this.m_Width; j++)
                {
                    float x = this.m_StartPosition.x + j * delta;
                    float y = this.m_StartPosition.y + i * delta;
                    Vector2 spawnPosition = new Vector2(x, y);

                    SpriteRenderer renderer = this.SpawnMapItem();
                    renderer.gameObject.name = $"{i} {j}";
                    renderer.color = cellColor;
                    renderer.sortingOrder = 1;
                    renderer.transform.SetParent(this.m_TileInstanceRoot);
                    renderer.transform.localPosition = spawnPosition;
                }
            }
            SpriteRenderer rootRenderer = this.m_TileInstanceRoot.GetComponent<SpriteRenderer>();
            rootRenderer.drawMode = SpriteDrawMode.Tiled;
            float sizeX = this.m_Width * delta;
            float sizeY = this.m_Height * delta;
            rootRenderer.size = new Vector2(sizeX, sizeY);
        }

        /// <summary>
        /// 生成地图单位
        /// </summary>
        /// <returns>精灵</returns>
        private SpriteRenderer SpawnMapItem()
        {
            SpriteRenderer spriteRenderer = null;
            TileItemObject tileItemObject = this.m_MapItemObjectPool.Spawn();
            if (tileItemObject == null)
            {
                GameObject go = new GameObject();
                spriteRenderer = go.AddComponent<SpriteRenderer>();
                spriteRenderer.sprite = this.m_Sprite;
                Transform transform = spriteRenderer.transform;
                transform.SetParent(this.m_TileInstanceRoot);
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;
                spriteRenderer.sortingOrder = 1;
                this.m_MapItemObjectPool.Register(TileItemObject.Create(spriteRenderer), true);
            }
            else
                spriteRenderer = (SpriteRenderer)tileItemObject.Target;

            this.m_ActiveMapItems.Add(spriteRenderer);
            return spriteRenderer;
        }

        /// <summary>
        /// 由索引获取坐标
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns>坐标</returns>
        public Vector2 IndexToPosition(int index)
        {
            float xIndex = index % this.m_Width;
            float yIndex = index / this.m_Width;

            float x = this.m_StartPosition.x + xIndex * this.m_MapDelta;
            float y = this.m_StartPosition.y + yIndex * this.m_MapDelta;
            Vector2 position = new Vector2(x, y);
            return position;
        }
        #endregion
    }

    public partial class _2048Component : GameFrameworkComponent
    {
        #region Field
        /// <summary>
        /// 精灵
        /// </summary>
        private GameObject m_UnitPrefab;

        /// <summary>
        /// 单位根节点
        /// </summary>
        [SerializeField]
        private Transform m_UnitInstanceRoot = null;

        /// <summary>
        /// 单位对象池容量
        /// </summary>
        [SerializeField]
        private int m_UnitItemCapacity = 16;

        /// <summary>
        /// 单位对象池
        /// </summary>
        private IObjectPool<UnitItemObject> m_UnitItemObjectPool = null;

        /// <summary>
        /// 激活的单位
        /// </summary>
        private Dictionary<int, UnitItem> m_ActiveUnitItems = null;

        /// <summary>
        /// 动画时长
        /// </summary>
        private float m_TweenTime = 0.25f;
        #endregion

        #region Property
        /// <summary>
        /// 激活单位的数量
        /// </summary>
        public int UnitItemCount => this.m_ActiveUnitItems.Count;
        #endregion

        /// <summary>
        /// 是否是空格子
        /// </summary>
        /// <param name="index">索引</param>
        /// <returns></returns>
        public bool IsEmptyCell(int index) => !this.m_ActiveUnitItems.ContainsKey(index);

        /// <summary>
        /// 是否有空格子
        /// </summary>
        private bool HasEmptyCell()
        {
            int size = this.m_Width * this.m_Height;
            return this.m_ActiveUnitItems.Count < size;
        }

        /// <summary>
        /// 生成单位
        /// </summary>
        /// <param name="num">单位数量</param>
        public void SpawnItem(int num = 1)
        {
            for (int i = 0; i < num; i++)
            {
                if (!this.HasEmptyCell())
                    return;
                UnitItem unitItem = this.ShowUnitItem();
                int index = this.RandomIndex();
                unitItem.transform.position = this.IndexToPosition(index);
                unitItem.Index = index;
            }
        }

        /// <summary>
        /// 随机一个空格子位置
        /// </summary>
        /// <returns>空格子索引</returns>
        private int RandomIndex()
        {
            int size = this.m_Width * this.m_Height;

            int index = Random.Range(0, size);
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
        /// 尝试获取左边单位
        /// </summary>
        /// <param name="index">单位索引</param>
        /// <param name="unitItem">左边单位</param>
        /// <returns>是否获取到</returns>
        public bool TryGetLeftUnit(int index, out UnitItem unitItem)
        {
            unitItem = null;

            int leftIndex = index--;

            int row = index / this.m_Width;
            int leftRow = leftIndex / this.m_Width;
            if (leftRow != row)
                return false;
            if (!this.m_ActiveUnitItems.ContainsKey(leftIndex))
                return false;

            unitItem = this.m_ActiveUnitItems[leftIndex];
            return true;
        }

        /// <summary>
        /// 尝试获取右边单位
        /// </summary>
        /// <param name="index">单位索引</param>
        /// <param name="unitItem">右边单位</param>
        /// <returns>是否获取到</returns>
        public bool TryGetRightUnit(int index, out UnitItem unitItem)
        {
            unitItem = null;

            int rightIndex = index++;

            int row = index / this.m_Width;
            int rightRow = rightIndex / this.m_Width;
            if (rightRow != row)
                return false;
            if (!this.m_ActiveUnitItems.ContainsKey(rightIndex))
                return false;

            unitItem = this.m_ActiveUnitItems[rightIndex];
            return true;
        }

        /// <summary>
        /// 尝试获取上方单位
        /// </summary>
        /// <param name="index">单位索引</param>
        /// <param name="unitItem">上方单位</param>
        /// <returns>是否获取到</returns>
        public bool TryGetUpUnit(int index, out UnitItem unitItem)
        {
            unitItem = null;

            int upIndex = index + this.m_Width;

            int row = index / this.m_Width;
            int upRow = upIndex / this.m_Width;
            if (upRow >= this.m_Width * this.m_Height || upRow - row != 1)
                return false;
            if (!this.m_ActiveUnitItems.ContainsKey(upIndex))
                return false;

            unitItem = this.m_ActiveUnitItems[upIndex];
            return true;
        }

        /// <summary>
        /// 尝试获取下方单位
        /// </summary>
        /// <param name="index">单位索引</param>
        /// <param name="unitItem">下方单位</param>
        /// <returns>是否获取到</returns>
        public bool TryGetDownUnit(int index, out UnitItem unitItem)
        {
            unitItem = null;

            int downIndex = index - this.m_Width;

            int row = index / this.m_Width;
            int downRow = downIndex / this.m_Width;
            if (downRow < 0 || row - downRow != 1)
                return false;
            if (!this.m_ActiveUnitItems.ContainsKey(downIndex))
                return false;

            unitItem = this.m_ActiveUnitItems[downIndex];
            return true;
        }

        /// <summary>
        /// 检查游戏是否结束
        /// </summary>
        /// <returns>是否结束</returns>
        public bool CheckGameOver()
        {
            foreach (KeyValuePair<int, UnitItem> item in this.m_ActiveUnitItems)
            {
                int index = item.Value.Index;
                UnitItem rightUnit;
                this.TryGetRightUnit(index, out rightUnit);
                UnitItem upUnit;
                this.TryGetUpUnit(index, out upUnit);
                if (rightUnit != null && UnitItem.CanMerge(item.Value, rightUnit))
                    return false;
                if (upUnit != null && UnitItem.CanMerge(item.Value, upUnit))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 显示瓦片
        /// </summary>
        /// <returns>精灵瓦片</returns>
        public UnitItem ShowUnitItem()
        {
            UnitItem unitItem = null;
            UnitItemObject unitItemObject = this.m_UnitItemObjectPool.Spawn();
            if (unitItemObject == null)
            {
                unitItem = Object.Instantiate<GameObject>(this.m_UnitPrefab).GetComponent<UnitItem>();
                Transform transform = unitItem.GetComponent<Transform>();
                transform.SetParent(this.m_UnitInstanceRoot);
                transform.localPosition = Vector3.one;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;

                this.m_UnitItemObjectPool.Register(UnitItemObject.Create(unitItem), true);
            }
            else
                unitItem = (UnitItem)unitItemObject.Target;

            this.m_ActiveUnitItems.Add(unitItem.Index, unitItem);
            return unitItem;
        }

        /// <summary>
        /// 隐藏单位
        /// </summary>
        /// <param name="unitItem">单位</param>
        public void HideUnitItem(UnitItem unitItem)
        {
            this.m_ActiveUnitItems.Remove(unitItem.Index);
            this.m_UnitItemObjectPool.Unspawn(unitItem);
        }

        /// <summary>
        /// 关闭所有单位
        /// </summary>
        public void HideAllUnitItem()
        {
            foreach (KeyValuePair<int, UnitItem> item in this.m_ActiveUnitItems)
                this.m_UnitItemObjectPool.Unspawn(item.Value);
            this.m_ActiveUnitItems.Clear();
        }

        /// <summary>
        /// 尝试向上移动棋盘可移动棋子
        /// </summary>
        public void TryMoveUp()
        {
            int index = this.Size - this.m_Width - 1;
            int deltaIndex = -1;
            int moveDelta = this.m_Width;

            while (index >= 0)
            {
                if (this.m_ActiveUnitItems.ContainsKey(index))
                    this.MoveUnitItem(this.m_ActiveUnitItems[index], -1, this.Size, moveDelta);

                index += deltaIndex;
            }
        }

        /// <summary>
        /// 尝试向右移动棋盘可移动棋子
        /// </summary>
        public void TryMoveRight()
        {
            int index = this.Size - 2;
            int deltaIndex = -1;
            int moveDelta = 1;

            while (index >= 0)
            {
                if (index % this.m_Width != this.m_Width - 1)
                {
                    if (this.m_ActiveUnitItems.ContainsKey(index))
                    {
                        int row = index / this.m_Width;
                        int min = row * this.m_Height - Width - 1;
                        int max = (row + 1) * this.m_Width;
                        this.MoveUnitItem(this.m_ActiveUnitItems[index], min, max, moveDelta);
                    }
                }

                index += deltaIndex;
            }
        }

        /// <summary>
        /// 尝试向下移动棋盘可移动棋子
        /// </summary>
        public void TryMoveDown()
        {
            int index = this.m_Width;
            int deltaIndex = 1;
            int moveDelta = -this.m_Width;
            while (index < this.Size)
            {
                if (this.m_ActiveUnitItems.ContainsKey(index))
                    this.MoveUnitItem(this.m_ActiveUnitItems[index], -1, this.Size, moveDelta);

                index += deltaIndex;
            }
        }

        /// <summary>
        /// 尝试向左移动棋盘可移动棋子
        /// </summary>
        public void TryMoveLeft()
        {
            int index = 1;
            int deltaIndex = 1;
            int moveDelta = -1;
            while (index < this.Size)
            {
                if (index % this.m_Width != 0)
                {
                    if (this.m_ActiveUnitItems.ContainsKey(index))
                    {
                        int row = index / this.m_Width;
                        int min = row * this.m_Width - 1;
                        int max = (row + 1) * this.m_Width;
                        this.MoveUnitItem(this.m_ActiveUnitItems[index], min, max, moveDelta);
                    }
                }

                index += deltaIndex;
            }
        }

        /// <summary>
        /// 移动单位
        /// </summary>
        /// <param name="unitItem">单位</param>
        /// <param name="indexDelta">移动间隔</param>
        private void MoveUnitItem(UnitItem unitItem, int minIndex, int maxIndex, int indexDelta)
        {
            int index = unitItem.Index + indexDelta;
            while (index > minIndex && index < maxIndex)
            {
                if (!this.IsEmptyCell(index))
                {
                    UnitItem left = this.m_ActiveUnitItems[index];
                    if (this.CanMerge(left, unitItem))
                    {
                        this.Merge(left, unitItem);
                        return;
                    }
                    //当前非空格子，不在继续查询是否可以合并
                    break;
                }
                index += indexDelta;
            }

            index -= indexDelta;
            //位置相同
            if (index == unitItem.Index)
                return;

            unitItem.Index = index;
            this.TweenUnitItem(unitItem, this.IndexToPosition(index), false);
        }

        /// <summary>
        /// 能否合并棋子
        /// </summary>
        /// <param name="left">左棋子</param>
        /// <param name="right">右棋子</param>
        /// <returns>是否是相同的棋子</returns>
        private bool CanMerge(UnitItem left, UnitItem right)
        {
            return left.Type == right.Type && !left.Lock && !right.Lock;
        }

        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="left">左棋子</param>
        /// <param name="right">右棋子</param>
        private void Merge(UnitItem left, UnitItem right)
        {
            left.Type *= 2;
            left.GetComponentInChildren<TMPro.TextMeshPro>().text = left.Type.ToString();

            this.m_ActiveUnitItems.Remove(right.Index);
            this.TweenUnitItem(right, this.IndexToPosition(left.Index));
        }

        /// <summary>
        /// 单位动画
        /// </summary>
        /// <param name="unitItem">单位</param>
        /// <param name="targetPosition">目标位置</param>
        /// <param name="hide">是否隐藏</param>
        private void TweenUnitItem(UnitItem unitItem, Vector2 targetPosition, bool hide = true)
        {
            Tween tween = unitItem.transform.DOMove(targetPosition, this.m_TweenTime)
            .SetAutoKill(true)
            .OnComplete(() =>
            {
                unitItem.transform.position = targetPosition;
                if (hide)
                    this.HideUnitItem(unitItem);
            });
        }
    }
}