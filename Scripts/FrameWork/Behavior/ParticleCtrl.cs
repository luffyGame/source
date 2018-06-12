using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
	/// <summary>
	/// 一个部分有效的粒子运行时缩放
	/// </summary>
	public class ParticleCtrl : MonoBehaviour
	{
		#region Variables
		public ParticleSystem[] pss = null;
		public ParticleEmitter[] pes = null;
		public ParticleSetDecal[] decals = null;
        public float scaleRate = 1f;
		private Transform cachedTrans = null;
		public Transform CachedTrans {get{if(null==cachedTrans) cachedTrans = transform; return cachedTrans;}}
        private GameObject cachedGo = null;
        public GameObject CachedGo { get { if (null == cachedGo) cachedGo = gameObject; return cachedGo; } }
		#endregion
		[ContextMenu("Execute")]
		void Execute()
		{
			//执行缩放，并将新的值作为原始值
			Init();
			_Scale(scaleRate,1f);
			scaleRate = 1f;
		}
		#region Public Method
		public static void Attach(GameObject psGo)
		{
			List<ParticleSystem> listPss = new List<ParticleSystem>();
			Utils.GetComponent<ParticleSystem>(psGo,ref listPss);
			List<ParticleEmitter> listPes = new List<ParticleEmitter>();
			Utils.GetComponent<ParticleEmitter>(psGo,ref listPes);
			if(listPss.Count == 0&&listPes.Count == 0)
				return;
			ParticleCtrl scaler = psGo.GetComponent<ParticleCtrl>();
			if(null==scaler)
				scaler = psGo.AddComponent<ParticleCtrl>();
			if(listPss.Count>0)
			{
				scaler.pss = new ParticleSystem[listPss.Count];
                for (int i = 0; i < listPss.Count; ++i)
                {
                    scaler.pss[i] = listPss[i];
                    scaler.pss[i].playOnAwake = false;
                }
			}
			else
				scaler.pss = null;
			
			if(listPes.Count>0)
			{
				scaler.pes = new ParticleEmitter[listPes.Count];
                for (int i = 0; i < listPes.Count; ++i)
                {
                    scaler.pes[i] = listPes[i];
                }
			}
			else
				scaler.pes = null;
			
			List<ParticleSetDecal> listPsd = new List<ParticleSetDecal>();
			Utils.GetComponent<ParticleSetDecal>(psGo,ref listPsd);
			if (listPsd.Count > 0)
				scaler.decals = listPsd.ToArray();
			else
				scaler.decals = null;
		}
		public void Scale(float rate)
		{
			if(Mathf.Abs(rate - scaleRate)<0.00001f)
				return;
			CachedTrans.SetScale(new Vector3(rate,rate,rate));
			/*Transform parent = CachedTrans.parent;
			float parentScale = null == parent?1f:parent.lossyScale.x;
			float selfScale = scaleRate/parentScale;
			CachedTrans.localScale = new Vector3(selfScale,selfScale,selfScale);*/
			_Scale(rate,scaleRate);
			scaleRate = rate;
		}
		
		public void Play()
		{
			if(null!=pss)
			{
                if(null != decals)
                {
	                Transform coliTrans = Global.Instance.TerrainTransform;
                    for(int i=0;i<decals.Length;++i)
	                    decals[i].SetCollision(coliTrans);
                }

				for(int i=0;i<pss.Length;++i)
				{
					ParticleSystem ps = pss[i];
					if(null!=ps)
						ps.Play();
				}
			}
		}
		public void Pause()
		{
			if(null!=pss)
			{
				for(int i=0;i<pss.Length;++i)
				{
					ParticleSystem ps = pss[i];
					if(null!=ps)
						ps.Pause();
				}
			}
		}
		public void Stop()
		{
			if(null!=pss)
			{
				for(int i=0;i<pss.Length;++i)
				{
					ParticleSystem ps = pss[i];
					if(null!=ps)
						ps.Stop();
				}
			}
			Clear();
		}
		#endregion
		#region Private Method
		private void Init()
		{
			pss = GetComponentsInChildren<ParticleSystem>();
			pes = GetComponentsInChildren<ParticleEmitter>();
		}

		private void _Scale(float newRate,float oldRate)
		{
			float finalRate = newRate/oldRate;
			if(null!=pss)
			{
				for(int i=0;i<pss.Length;++i)
				{
					ParticleSystem ps = pss[i];
					if(null!=ps)
					{
						ps.startSize *= finalRate;
						ps.startSpeed *= finalRate;
					}
				}
			}
			if(null!=pes)
			{
				for(int i=0;i<pes.Length;++i)
				{
					ParticleEmitter pe = pes[i];
					if(null!=pe)
					{
						pe.minSize *= finalRate;
						pe.maxSize *= finalRate;
					}
				}
			}
		}
		private void Clear()
		{
			if(null!=pes)
			{
				for(int i=0;i<pes.Length;++i)
				{
					ParticleEmitter pe = pes[i];
					if(null!=pe)
						pe.ClearParticles();
				}
			}
			if(null != decals)
			{
				for(int i=0;i<decals.Length;++i)
					decals[i].SetCollision(null);
			}
		}
		#endregion
	}
}

