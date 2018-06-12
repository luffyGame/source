using System.Collections;
using System.Collections.Generic;

namespace FrameWork.MVVM
{
    public interface IView
    {
        string Name { get; set; }
        void Open();
        void Close();
    }
}