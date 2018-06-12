using UnityEngine;

namespace Game
{
    public class FurnitureCollider : ObjCollider
    {
        private bool isBuilding;
        public void SetBuilding(bool isBuilding)
        {
            if(this.isBuilding == isBuilding)
                return;
            this.isBuilding = isBuilding;
            collider.isTrigger = isBuilding;
        }
        public void OnTriggerEnter(Collider other)
        {
            if(!isBuilding)
                return;
            var furnitureCollider = other.GetComponent<FurnitureCollider>();
            if (furnitureCollider != null)
            {
                FurnitureInfo fi = furnitureCollider.info as FurnitureInfo;
                if (fi != null)
                {
                    BuilderHelper.Instance.AddColliderFurniture(fi);
                }
            }
            else
            {
                BuilderHelper.Instance.ObstacleCollider++;
                Debug.Log("Collider Furniture is:" + other.name);
            }

            BuilderHelper.Instance.TriggerStateChanged = true;
        }

        private void OnTriggerExit(Collider other)
        {
            if(!isBuilding)
                return;
            var furnitureCollider = other.GetComponent<FurnitureCollider>();
            if (furnitureCollider != null)
            {
                FurnitureInfo fi = furnitureCollider.info as FurnitureInfo;
                if (fi != null)
                {
                    BuilderHelper.Instance.RemoveColliderFurniture(fi);
                }
            }
            else
            {
                BuilderHelper.Instance.ObstacleCollider--;
                Debug.Log("Collider Furniture is:" + other.name);
            }
            BuilderHelper.Instance.TriggerStateChanged = true;
        }
    }
}