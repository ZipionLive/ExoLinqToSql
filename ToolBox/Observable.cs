using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ToolBox
{
    class Observable<TEventArgs> : IObservable<TEventArgs>
        where TEventArgs : EventArgs
    {
        public event EventHandler<TEventArgs> Notify;

        public void Register(IObserver<TEventArgs> Observer)
        {
            Notify += new EventHandler<TEventArgs>(Observer.NotifyAction);
        }

        public void Unregister(IObserver<TEventArgs> Observer)
        {
            Notify -= Observer.NotifyAction;
        }

        protected void RaiseNotify(TEventArgs e)
        {
            if (Notify != null)
            {
                Notify(this, e);
            }
        }
    }
}
