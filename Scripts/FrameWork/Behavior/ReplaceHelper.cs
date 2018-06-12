using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class ReplaceHelper : MonoBehaviour
    {
        public EReplacePart id;
        public GameObject newPart;
        public SkinnedMeshRenderer ski;

        [ContextMenu("change")]
        private void Modify()
        {
            Replacement rep = GetComponent<Replacement>();
            GameObject part = Instantiate(newPart);
            rep.Replace((int)id,part);
        }
        [ContextMenu("clear")]
        private void Clear()
        {
            Replacement rep = GetComponent<Replacement>();
            rep.ClearBones();
            GameObject part = rep.ReplaceToDefault((int)id);
            if (Application.isPlaying)
                Destroy(part);
            else
            {
                DestroyImmediate(part);
            }
        }

        [ContextMenu("replace")]
        private void Replace()
        {
            Replacement rep = GetComponent<Replacement>();
            rep.ReplaceBones(ski);
        }
    }
}