using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class ObjLocator : SingleBehavior<ObjLocator>
    {
        #region Var
        public Transform spriteRoot;
        public Transform sceneItemRoot;
        public Transform otherRoot;
        public Transform effectRoot;
        public Transform furnitureRoot;

        private Dictionary<int,Effect> effects = new Dictionary<int, Effect>();//用于存储完全托管的特效
        #endregion

        public override void OnInit()
        {
            base.OnInit();
            DontDestroyOnLoad(gameObject);
        }

        public void LoadSprite(string res, Action<Sprite> onLoaded)
        {
            Sprite sprite = new Sprite(res);
            sprite.Load(() =>
            {
                sprite.SetParent(spriteRoot);
                onLoaded(sprite);
            });
        }
        public void LoadSceneItem(string res, Action<SceneItem> onLoaded)
        {
            SceneItem sceneItem = new SceneItem(res);
            sceneItem.Load(() =>
            {
                sceneItem.SetParent(sceneItemRoot);
                onLoaded(sceneItem);
            });
        }

        public void LoadFurnitureItem(string res,int layer, Action<FurnitureItem> onLoaded)
        {
            FurnitureItem furnitureItem = new FurnitureItem(res,layer);
            furnitureItem.Load(() =>
            {
                //sceneitems
                furnitureItem.SetParent(furnitureRoot);
                onLoaded(furnitureItem);
            });
        }

        public void LoadBasicModel(string res,AssetType assetType, Action<BasicModel> onLoaded)
        {
            BasicModel basicModel = new BasicModel(res,assetType);
            basicModel.Load(() =>
            {
                basicModel.SetParent(otherRoot);
                onLoaded(basicModel);
            });
        }

        public void AddManagedEffect(Effect effect)
        {
            effect.SetParent(effectRoot,true);
            effects.Add(effect.oid,effect);
        }

        public void RemoveManagedEffect(Effect effect)
        {
            effects.Remove(effect.oid);
            effect.Release();
        }
    }
}