using GameFramework;
using GameFramework.ObjectPool;
using UnityEngine;

namespace _2048
{
    /// <summary>
    /// 瓦片
    /// </summary>
    public class TileItemObject : ObjectBase
    {
        public static TileItemObject Create(object target)
        {
            TileItemObject tileItemObject = ReferencePool.Acquire<TileItemObject>();
            tileItemObject.Initialize(target);
            return tileItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            SpriteRenderer spriteRenderer = (SpriteRenderer)Target;
            if (spriteRenderer == null)
                return;

            Object.Destroy(spriteRenderer.gameObject);
        }
    }
}