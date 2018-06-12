using UnityEngine;

namespace Game.Test
{
    public class TestAniCross : MonoBehaviour
    {
        public Animation anim;
        public string ani1;
        public string ani2;
        public float fade = 0.25f;
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
            isPlayed = true;
            time = 0f;
            anim.Play(ani1);
        }

        private void Update()
        {
            if (isPlayed)
            {
                time += Time.deltaTime;
                if (time >= ani1Time*(1-fade))
                {
                    isPlayed = false;
                    //if(fade>=0f)
                        anim.CrossFade(ani2,fade);
                    //else
                    //{
                    //    anim.Play(ani2);
                    //}
                }
            }
        }
    }
}