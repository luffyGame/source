using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FrameWork
{
	[RequireComponent (typeof(Image))]
	public class FrameAnimation : MonoBehaviour
	{
		private Image image;
		private float deltaTime;
		private int index;
		public UiAtlasComp frames;
		public float step = 0.02f;

		void Awake ()
		{
			this.image = this.GetComponent<Image> ();
		}

		void Update ()
		{
			if (frames == null || frames.Count == 0)
				return;
		
			deltaTime += Time.deltaTime;
			if (deltaTime > step) {
				deltaTime = 0;
				index++;
				if (index == frames.Count)
					index = 0;
			
				this.image.sprite = frames [index];
				this.image.SetNativeSize ();
			}
		}
		public void Play()
		{
			index = 0;
			deltaTime = 0f;
			enabled = true;
		}
		public void Stop()
		{
			enabled = false;
		}
	}
}