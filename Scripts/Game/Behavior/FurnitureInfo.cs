using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class FurnitureInfo : ObjInfo
    {
        #region Var Set
        public FurnitureType furnitureType;
        public FurnitureDirect direction;//当前朝向
        public int cellWidth = 1;
        public int cellHeight = 1;
        
        public FurnitureCollider furnitureCollider;
        public UseableCollider[] usableColliders;
        #endregion
        public int layer;
        public int furnitureSpace
        {
            get { return cellWidth * cellHeight; }
        }
        //detail
        public Renderer[] renders;

        public override ObjCollider objCollider
        {
            get { return furnitureCollider; }
        }

        private void Awake()
        {
            renders = GetComponentsInChildren<Renderer>();
        }

        public void SetState(bool isPlaced, bool canPlaced)
        {
            //if (renders == null) renders = GetComponentsInChildren<Renderer>();
            if (!isPlaced && renders != null)
            {
                for(int i = 0; i < renders.Length; i++)
                {
                    renders[i].material.shader = BuilderHelper.Instance.placingShader;
                    renders[i].material.color = canPlaced ? Color.green : Color.red;
                }
            }
            else
            {
                //已建成的若canPlace=false则为置灰状态； canPlace=true则为正常状态
                for (int i = 0; i < renders.Length; i++)
                {
                    renders[i].material.shader = BuilderHelper.Instance.placedShader;
                    renders[i].material.color = canPlaced ? Color.white : Color.blue;
                }
            }
        }

        public void SetBuilding(bool isBuilding)
        {
            if(null!=furnitureCollider)
                furnitureCollider.SetBuilding(isBuilding);
            OnBuilding(isBuilding);
        }

        protected virtual void OnBuilding(bool isBuilding) { }
    }
}