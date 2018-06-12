﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UObj = UnityEngine.Object;

namespace FrameWork
{
    public delegate void OnAssetGot(System.Object asset, ulong cbId);
    public class AssetCallback
    {
        private static ulong idLast = 0;
        private static Dictionary<ulong, AssetCallback> items = new Dictionary<ulong, AssetCallback>();
        public static AssetCallback Gen(OnAssetGot cb,string path)
        {
            AssetCallback acb = new AssetCallback(cb, path);
            items.Add(acb.Id, acb);
            return acb;
        }
        public static void Remove(ulong cbId)
        {
            if (items.ContainsKey(cbId))
                items.Remove(cbId);
        }
        public static AssetCallback Get(ulong cbId)
        {
            if (items.ContainsKey(cbId))
                return items[cbId];
            return null;
        }
        private ulong id;
        private string path;
        private OnAssetGot cb;
        public ulong Id { get { return id; } }
        public string Path { get { return path; } }
        private AssetCallback(OnAssetGot cb,string path)
        {
            id = ++idLast;
            this.cb = cb;
            this.path = path;
        }
        public void Do(System.Object obj)
        {
            if (null != cb)
                cb(obj, id);
        }
    }

    public delegate void OnGameObjGot(GameObject gameObj, ulong cbId);
    public class GoCallback
    {
        private static ulong idLast = 0;
        private static Dictionary<ulong, GoCallback> items = new Dictionary<ulong, GoCallback>();
        public static GoCallback Gen(OnGameObjGot cb, string path)
        {
            GoCallback acb = new GoCallback(cb, path);
            items.Add(acb.Id, acb);
            return acb;
        }
        public static void Remove(ulong cbId)
        {
            if (items.ContainsKey(cbId))
                items.Remove(cbId);
        }
        public static GoCallback Get(ulong cbId)
        {
            if (items.ContainsKey(cbId))
                return items[cbId];
            return null;
        }

        private ulong id;
        private string path;
        private OnGameObjGot cb;
        public OnGameObjGot Cb { set { cb = value; } }
        public ulong Id { get { return id; } }
        public string Path { get { return path; } }
        private ulong assetCbId = 0;
        public ulong AssetCbId
        {
            get { return assetCbId; }
            set { assetCbId = value; }
        }
        private GoCallback(OnGameObjGot cb,string path)
        {
            id = ++idLast;
            this.cb = cb;
            this.path = path;
        }
        public void Do(GameObject gameObj)
        {
            if (null != cb)
                cb(gameObj, id);
        }
        public void Cancel()
        {
            if(assetCbId>0)
            {
                BundleMgr.Instance.CancelUngotAsset(assetCbId);
                assetCbId = 0;
            }
        }
    }
}
