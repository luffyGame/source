using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
    //声音代理，对声音进行管理
    public class SoundAgent : SingleBehavior<SoundAgent>
    {
        #region Variables
        public GameObject sound3dTemp;
        protected SoundPlayer m_bg = null;
        protected int count = 0;
        protected List<SoundPlayer> m_used3d = new List<SoundPlayer>();
        protected List<SoundPlayer> m_unUsed3d = new List<SoundPlayer>();
        protected List<SoundPlayer> m_used2d = new List<SoundPlayer>();
        protected List<SoundPlayer> m_unUsed2d = new List<SoundPlayer>();
        private Transform trans;
        public Transform Trans { get { if (null == trans) trans = transform; return trans; } }
        #endregion
        // Use this for initialization
        public override void OnInit()
        {
            m_bg = GetPlayer(false,false);
        }
        #region Private Methdo
        public SoundPlayer GetPlayer(bool autoRecycle,bool is3d)
        {
            if (is3d)
                return GetPlayer3d(autoRecycle);
            else
                return GetPlayer2d(autoRecycle);
        }
        public SoundPlayer GetPlayer3d(bool autoRecycle)
        {
            SoundPlayer player;
            if (m_unUsed3d.Count > 0)
            {
                player = m_unUsed3d[0];
                m_unUsed3d.RemoveAt(0);
            }
            else
            {
                GameObject go = Object.Instantiate<GameObject>(sound3dTemp);
                go.name = (++count).ToString();
                player = go.AddComponent<SoundPlayer>();
                player.Source = go.GetComponent<AudioSource>();
                player.is3d = true;
            }
            player.AutoRecycle = autoRecycle;
            m_used3d.Add(player);
            return player;
        }
        private SoundPlayer GetPlayer2d(bool autoRecycle)
        {
            SoundPlayer player;
            if (m_unUsed2d.Count > 0)
            {
                player = m_unUsed2d[0];
                m_unUsed2d.RemoveAt(0);
            }
            else
            {
                GameObject go = new GameObject((++count).ToString());
                player = go.AddComponent<SoundPlayer>();
                player.Source = go.AddComponent<AudioSource>();
                player.Trans.parent = Trans;
                player.is3d = false;
            }
            player.AutoRecycle = autoRecycle;
            m_used2d.Add(player);
            return player;
        }
        #endregion
        #region Public Method
        public void PlayBG(string assetPath, bool loop, float volume = 1f)
        {
            if (null != assetPath)
            {
                m_bg.Play(assetPath, volume, 1f, loop);
            }
        }
        public void StopBG()
        {
            m_bg.ShutDown(false);
        }
        public void BgVolume(float volume)
        {
            m_bg.Volume = volume;
        }
        public SoundPlayer PlaySound(string assetPath,bool loop, float volume = 1f)
        {
            SoundPlayer player = GetPlayer(true,false);
            player.Play(assetPath, volume, 1f, loop);
            return player;
        }
        public SoundPlayer PlaySound(string assetPath, bool loop,Vector3 pos,float volume = 1f)
        {
            SoundPlayer player = GetPlayer(true, true);
            player.Trans.position = pos;
            player.Play(assetPath, volume, 1f, loop);
            return player;
        }
        public void UpdateVolume(float volume)
        {
            for (int i = 0; i < m_used3d.Count; ++i)
                m_used3d[i].Volume = volume;
            for (int i = 0; i < m_used2d.Count; ++i)
                m_used2d[i].Volume = volume;
        }
        public void UnusePlayer(SoundPlayer player)
        {
            if (player.is3d)
            {
                m_used3d.Remove(player);
                m_unUsed3d.Add(player);
                player.Parent = Trans;
            }
            else
            {
                m_used2d.Remove(player);
                m_unUsed2d.Add(player);
            }
        }
        public virtual void StopAllSoundEffect()
        {
            for (int i = m_used2d.Count - 1; i >= 0; --i)
            {
                if (m_used2d[i].AutoRecycle)
                    m_used2d[i].ShutDown();
            }
            for (int i = m_used3d.Count - 1; i >= 0; --i)
            {
                if (m_used3d[i].AutoRecycle)
                    m_used3d[i].ShutDown();
            }
        }
        #endregion
    }
}