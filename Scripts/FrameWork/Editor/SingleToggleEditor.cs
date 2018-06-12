using UnityEditor;

namespace FrameWork.Editor
{
    [CustomEditor(typeof(SingleToggle))]
    public class SingleToggleEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            ShowUtil.ScriptTitle(target);
            var t = target as SingleToggle;
            ShowUtil.ObjectDraw(t.active,"active", (obj) => { t.active = obj;});
            ShowUtil.ObjectDraw(t.inactive,"inactive", (obj) => { t.inactive = obj;});
        }
    }
}