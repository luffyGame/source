using UnityEngine;

namespace Game.Test
{
    public class TestAni : MonoBehaviour
    {
        public Animation anim;
        public string ani1;
        public string ani2;
        private float ani1Time;
        private float ani2Time;
        private float totalTime;
        private bool isPlayed;
        private float time;
        private bool isPlayAni2;
        private float startTime;

        private void Awake()
        {
            ani1Time = anim[ani1].length;
            ani2Time = anim[ani2].length;
            totalTime = ani1Time + ani2Time;
        }
        [ContextMenu("play")]
        public void Play()
        {
            startTime = Time.realtimeSinceStartup;
            isPlayed = true;
            time = 0f;
            anim.Play(ani1);
            
        }

        private void Update()
        {
            if (isPlayed)
            {
                time += Time.deltaTime;
                if (time >= ani1Time - 0.3f)
                {
                    time = 0f;
                    isPlayAni2 = true;
                    Debug.Log("play " + ani1);
                    if (anim.IsPlaying(ani1))
                    {
                        anim.Rewind(ani1);
                    }
                    else
                        anim.CrossFade(ani1,0.3f);
                }

                if (time >= totalTime)
                {
                    
                }
            }
        }

        public void Stop()
        {
            float costTime = Time.realtimeSinceStartup - startTime;
            Debug.Log("real " + costTime);
            Debug.Log("cal " + totalTime);
        }
    }
}