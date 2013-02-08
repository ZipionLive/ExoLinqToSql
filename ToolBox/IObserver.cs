using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox
{
    public interface IObserver<TEventArgs>
        where TEventArgs : EventArgs
    {
        void NotifyAction(object sender, TEventArgs e);
    }
}
