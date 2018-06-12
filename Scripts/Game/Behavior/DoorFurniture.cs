using System.Collections;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class DoorFurniture : FurnitureInfo
    {
        public DoorController doorCtrl;

        protected override void OnBuilding(bool isBuilding)
        {
            if (doorCtrl != null)
            {
                doorCtrl.SetColliderActive(!isBuilding);
            }
        }
    }
}
