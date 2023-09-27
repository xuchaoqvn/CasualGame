using UnityEngine;
using GameFramework;
using GameFramework.ObjectPool;

namespace _2048
{
    /// <summary>
    /// 单位物体
    /// </summary>
    public class UnitItemObject : ObjectBase
    {
        public static UnitItemObject Create(object target)
        {
            UnitItemObject unitItemObject = ReferencePool.Acquire<UnitItemObject>();
            unitItemObject.Initialize(target);
            return unitItemObject;
        }

        protected override void Release(bool isShutdown)
        {
            UnitItem unitItem = (UnitItem)Target;
            if (unitItem == null)
                return;

            Object.Destroy(unitItem.gameObject);
        }
    }
}