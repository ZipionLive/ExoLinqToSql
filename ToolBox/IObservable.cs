using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox
{
    public interface IObservable<TEventArgs>
        where TEventArgs : EventArgs
    {
        event EventHandler<TEventArgs> Notify;

        void Register(IObserver<TEventArgs> Observer);
        void Unregister(IObserver<TEventArgs> Observer);
    }
}
