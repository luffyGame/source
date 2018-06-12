using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class PlacementManager : MonoBehaviour
    {
        private static readonly int[] nearOffset = new int[]{0, 1, -1};
        private Transform cachedTrans;
        private Transform CachedTrans {get
        {
            if (null == cachedTrans) cachedTrans = transform;
            return cachedTrans;
        }}
        //每个格子上放置的物体
        public class Cell
        {
            public int index;
            public List<FurnitureItem> furnitures;
            public Cell(int index)
            {
                this.index = index;
                furnitures = new List<FurnitureItem>();
            }

            public bool HasFurniture
            {
                get { return null != furnitures && furnitures.Count > 0; }
            }

            public int FurnitureCount
            {
                get { return null != furnitures ? furnitures.Count : 0; }
            }
        }

        private Dictionary<int,Cell> cells = new Dictionary<int, Cell>();

        private int halfWidth
        {
            get { return width / 2; }
        }
        private int halfHeight
        {
            get { return height / 2; }
        }
        public int width;
        public int height;
        
        private Vector4 border;
        private float y;

        void Start()
        {
            Vector3 center = CachedTrans.position;
            border = new Vector4(center.x - halfWidth * Const.CELL_SIZE, 
                center.z - halfHeight * Const.CELL_SIZE,
                center.x + halfWidth * Const.CELL_SIZE, 
                center.z + halfHeight * Const.CELL_SIZE);
            y = center.y;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            var tempPos = CachedTrans.position;
            var leftDown = new Vector3(tempPos.x - this.halfWidth * Const.CELL_SIZE, tempPos.y + 0.03f, tempPos.z - this.halfHeight * Const.CELL_SIZE);
            var rightDown = new Vector3(tempPos.x + this.halfWidth * Const.CELL_SIZE, tempPos.y + 0.03f, tempPos.z - this.halfHeight * Const.CELL_SIZE);
            var leftUp = new Vector3(tempPos.x - this.halfWidth * Const.CELL_SIZE, tempPos.y + 0.03f, tempPos.z + this.halfHeight * Const.CELL_SIZE);
            var rightUp = new Vector3(tempPos.x + this.halfWidth * Const.CELL_SIZE, tempPos.y + 0.03f, tempPos.z + this.halfHeight * Const.CELL_SIZE);

            Gizmos.DrawLine(leftDown, rightDown);
            Gizmos.DrawLine(leftDown, leftUp);
            Gizmos.DrawLine(rightDown, rightUp);
            Gizmos.DrawLine(leftUp, rightUp);

            for (int i = 0; i < width; i++)
            {
                Gizmos.DrawLine(new Vector3(leftDown.x + Const.CELL_SIZE * i, leftDown.y, leftDown.z), new Vector3(leftUp.x + Const.CELL_SIZE * i, leftUp.y, leftUp.z));
            }

            for (int j = 0; j < height; j++)
            {
                Gizmos.DrawLine(new Vector3(leftDown.x, leftDown.y, leftDown.z + Const.CELL_SIZE * j), new Vector3(rightDown.x, rightDown.y, rightDown.z + Const.CELL_SIZE * j));
            }

        }

        #region 获取格子索引
        public int GetNearestWallIndex(FurnitureItem lastBuilding, out FurnitureDirect dir)
        {
            int x, y;
            GetIndex2D(lastBuilding.PosIndex, out x, out y);
            dir = lastBuilding.TheInfo.direction;
            switch (dir)
            {
                case FurnitureDirect.east:
                    if(y == 0 || !HasFurniture(x,y-1))
                        dir = FurnitureDirect.sourth;
                    else
                        y--;
                    break;
                case FurnitureDirect.sourth:
                    if(x == 0 || !HasFurniture(x-1,y))
                        dir = FurnitureDirect.west;
                    else
                        x--;
                    break;
                case FurnitureDirect.west:
                    if (y == height-1 || !HasFurniture(x,y+1))
                        dir = FurnitureDirect.north;
                    else
                        y++;
                    break;
                case FurnitureDirect.north:
                    if (x == width-1 || !HasFurniture(x+1,y))
                        dir = FurnitureDirect.east;
                    else
                        x++;
                    break;
            }

            return y*width+x;
        }

        private int GetNearestIndex(Vector3 pos)
        {
            int x, z;
            GetPosIndex(pos, out x, out z);
            return GetNearestIndex(x, z);
        }

        public int GetNearestIndex(int index)
        {
            int x, z;
            GetIndex2D(index,out x,out z);
            return GetNearestIndex(x, z);
        }

        private int GetNearestIndex(int x, int z)
        {
            for (int i = 0; i < nearOffset.Length; i++)
            {
                int newx = x + nearOffset[i];
                if (newx >= 0 && newx < width)
                {
                    for (int j = 0; j < nearOffset.Length; j++)
                    {
                        int newZ = z + nearOffset[j];
                        if (newZ >= 0 && newZ < height)
                        {
                            if (!HasFurniture(newx,newZ))
                            {
                                return newZ*height+newx;
                            }
                        }
                    }
                }
            }

            return z * height + x;
        }

        public Vector3 GetIndexPos(int index)
        {
            float x = border.x + (index % width + 0.5f) * Const.CELL_SIZE;
            float z = border.y + (index / width + 0.5f) * Const.CELL_SIZE;
            return new Vector3(x, y, z);
        }

        public int GetPosIndex(Vector3 pos)
        {
            int xIndex,zIndex;
            GetPosIndex(pos, out xIndex, out zIndex);
            return  zIndex* width + xIndex;
        }

        private void GetPosIndex(Vector3 pos,out int xIndex,out int zIndex)
        {
            xIndex = Mathf.Clamp((int)((pos.x - border.x) / Const.CELL_SIZE),0,width-1);
            zIndex = Mathf.Clamp((int)((pos.z - border.y) / Const.CELL_SIZE),0,height-1);
        }

        private void GetIndex2D(int index,out int xIndex,out int zIndex)
        {
            xIndex = index % width;
            zIndex = index / width;
        }
        #endregion

        #region 建筑管理
        public void PlaceItem(FurnitureItem fi)
        {
            int index = fi.PosIndex;
            Cell cell = null;
            if (cells.ContainsKey(index))
            {
                cell = cells[index];
            }
            else
            {
                cell = new Cell(index);
                cells.Add(index,cell);
            }
            cell.furnitures.Add(fi);
        }

        public void ReplacePlaceItem(FurnitureItem orign, FurnitureItem target)
        {
            int index = orign.PosIndex;
            Cell cell = null;
            if (cells.ContainsKey(index))
            {
                cell = cells[index];
                cell.furnitures.Remove(orign);
                cell.furnitures.Add(target);
            }
        }

        public void RemovePlaceItem(FurnitureItem fi)
        {
            int posIndex = fi.PosIndex;
            if(cells.ContainsKey(posIndex))
            {
                Cell cell = cells[posIndex];
                cell.furnitures.Remove(fi);
            }
        }

        public int FurnitureCount(int posIndex)
        {
            if(cells.ContainsKey(posIndex))
            {
                return cells[posIndex].FurnitureCount;
            }
            return 0;
        }

        private bool HasFurniture(int xIndex, int zIndex)
        {
            int index = zIndex * width + xIndex;
            if (cells.ContainsKey(index))
            {
                return cells[index].HasFurniture;
            }

            return false;
        }
        
        #endregion
        #region Other
        public FurnitureDirect GetTargetWallDir(int index, Vector3 hitPos)
        {
            Vector3 algendPos = GetIndexPos(index);
            return (FurnitureDirect)compareWallDir(hitPos, algendPos);
        }

        public int GetDefaultIndex()
        {
            Ray ray = Global.Instance.MainCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100f, Const.LAYER_BUILD_GROUND_MASK))
            {
                return GetNearestIndex(hit.point);
            }

            return halfHeight * width + halfWidth;
        }
        private int compareWallDir(Vector3 pos, Vector3 centerPos)
        {
            var offset = new Vector2(pos.x - centerPos.x, pos.z - centerPos.z);
            if (offset.x > 0)
            {
                if (Mathf.Abs(offset.x) - Mathf.Abs(offset.y) >= 0)
                {
                    return 0;
                }
                else if (offset.y > 0)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (Mathf.Abs(offset.x) - Mathf.Abs(offset.y) >= 0)
                {
                    return 2;
                }
                else if (offset.y > 0)
                {
                    return 3;
                }
                else
                {
                    return 1;
                }
            }

        }
        #endregion
    }
}