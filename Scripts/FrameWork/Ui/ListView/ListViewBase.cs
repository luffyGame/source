using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FrameWork
{
    [Serializable]
    public class ListViewBaseEvent : UnityEvent<int, ListViewItem>
    {
    }
}
