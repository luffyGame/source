using System;
using System.Security.Permissions;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class FurnitureItem : ObjBase
    {
        #region Var
        public string res { get; private set; }
        public override ObjType objType
        {
            get { return ObjType.O_BUILD; }
        }

        private FurnitureInfo theInfo;

        public FurnitureInfo TheInfo
        {
            get { return theInfo; }
        }

        public override ObjInfo info
        {
            get { return theInfo; }
        }

        public bool IsBuilding
        {
            set{if(null!=theInfo) theInfo.SetBuilding(value);}
        }

        public int Direction
        {
            get { return (int)( null == theInfo ?  FurnitureDirect.UNDEFINED : theInfo.direction); }
            set
            {
                SetFurnitureDir(value);
            }
        }
        public int PosIndex { get; set; }
        private int layer;
        public int Layer
        {
            get { return layer;}
            set { layer = value;
                if (null != theInfo) theInfo.layer = layer;
            }
        }

        public bool IsFurniture {
            get { return null == theInfo ? false : theInfo.furnitureType == FurnitureType.furniture; }
        }

        #endregion

        #region Public Method
        public FurnitureItem(string res,int layer)
        {
            this.res = res;
            this.layer = layer;
        }

        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(AssetType.HOME_BUILD, res);
            rootModel.Load(OnLoaded + done);
        }

        public void RotateClockwise()
        {
            int newDir = (Direction + 1) % (int) FurnitureDirect.COUNT;
            if(!CanRotateTo(newDir))
                return;
            Direction = newDir;
        }
        #endregion

        #region Private Method

        private void OnLoaded()
        {
            theInfo = rootModel.GetComponent<FurnitureInfo>();
            theInfo.layer = layer;
            this.InfoRef();
        }

        private void SetFurnitureDir(int dir)
        {
            theInfo.direction = (FurnitureDirect)dir;
            this.SetRot(Quaternion.Euler(0, (dir - 1) * 90, 0));
        }

        public void SetPosAligned(Vector3 pos)
        {
            if (theInfo != null)
            {
                if (theInfo.cellWidth % 2 == 0)
                    pos.x += Const.CELL_SIZE / 2;

                if (theInfo.cellHeight % 2 == 0)
                    pos.y += Const.CELL_SIZE / 2;
            }
            SetPos(pos);
        }

        private bool CanRotateTo(int newDir)
        {
            if (theInfo.direction == FurnitureDirect.UNDEFINED)
                return false;
            return true;
        }
        #endregion
    }
}