using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class SoundPlayer : MonoBehaviour
    {
        private string assetPath;
        private AudioSource source;
        public AudioSource Source { set { source = value; } get { return source; } }
        private IEnumerator closeCoroutine;
        private Transform trans;
        private ulong assetCbId;
        private bool autoRecycle;
        private bool isPlaying;
        public bool AutoRecycle { set { autoRecycle = value; } get { return autoRecycle; } }
        public float Volume { set { source.volume = value; } }
        public Transform Trans { get { if (null == trans) trans = transform; return trans; } }
        public Transform Parent
        {
            get { return Trans.parent; }
            set { Trans.SetParent(value); Trans.localPosition = Vector3.zero; }
        }
        public bool is3d { get; set; }
        #region Public Method
        public void Play(string path,float volume, float pitch, bool loop)
        {
            ShutDown(false);
            isPlaying = true;
            assetPath = ResObjUtil.GetObjPath(EObjType.SOUND, path);
            assetCbId = BundleMgr.Instance.GetAsset(assetPath, (asset, cbId) =>
            {
                Play((AudioClip)asset, volume, pitch, loop);
                assetCbId = 0;
            });
        }
        public void ShutDown(bool recycle = true)
        {
            isPlaying = false;
            if (null != closeCoroutine)
            {
                StopCoroutine(closeCoroutine);
                closeCoroutine = null;
            }
            Release(recycle);
        }
        public void GetReady(string path, float volume, float pitch, bool loop)
        {
            ShutDown(false);
            isPlaying = false;
            assetPath = ResObjUtil.GetObjPath(EObjType.SOUND, path);
            assetCbId = BundleMgr.Instance.GetAsset(assetPath, (asset, cbId) =>
            {
                Play((AudioClip)asset, volume, pitch, loop);
                assetCbId = 0;
            });
        }
        public void Stop()
        {
            if (isPlaying)
            {
                isPlaying = false;
                if (source.clip != null)
                    source.Stop();
            }
        }
        public void Play()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                if (source.clip != null)
                    source.Play();
            }
        }
        #endregion
        #region Private Method
        private void Play(AudioClip clip, float volume, float pitch, bool loop)
        {
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.loop = loop;
            if (isPlaying)
            {
                source.Play();
                if (!loop)
                {
                    DelayClose(clip.length);
                }
            }
        }
        private void Release(bool recycle)
        {
            if (source.clip != null)
            {
                source.Stop();
                source.clip = null;
            }
            if(assetCbId>0)
            {
                BundleMgr.Instance.CancelUngotAsset(assetCbId);
                assetCbId = 0;
            }
            if (null != assetPath)
            {
                BundleMgr.Instance.ReleaseAsset(assetPath, false);
                assetPath = null;
            }
            if (recycle && autoRecycle)
                SoundAgent.Instance.UnusePlayer(this);
        }

        private void DelayClose(float delay)
        {
            closeCoroutine = Utils.DelayAction(delay, () =>
            {
                Release(true);
                closeCoroutine = null;
            });
            StartCoroutine(closeCoroutine);
        }
        #endregion
    }
}
