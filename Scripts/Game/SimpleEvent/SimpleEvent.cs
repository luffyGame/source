/* @Description 动画回调的简单事件
 * @Auhtor SunShubin
 * @2018-06-16
 */
using UnityEngine;
using UnityEngine.Events;

namespace Game.SimpleEvent
{
	public class SimpleEvent : MonoBehaviour
	{
		public UnityEvent callBack;
		private void SetGameObjectDisable()
		{
			Debug.Log(callBack);
			if (callBack != null)
			{
				callBack.Invoke();
				Debug.Log("call back");
			}
			gameObject.SetActive(false);
		}
	}
}
