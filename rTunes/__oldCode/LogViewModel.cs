using iTunesWrapper;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using System.Timers;
using System;
using System.Web.UI.WebControls;
using System.Collections.ObjectModel;
using System.Threading;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace rTunes
{
    public class LogItem : NotifyBase
    {
        private string _Notification;

        public string Notification
        {
            get { return _Notification; }
            set { _Notification = value; Changed();  }
        }

        private LogItemImportance _Importance;

        public LogItemImportance Importance
        {
            get { return _Importance; }
            set { _Importance = value; Changed(); }
        }
    }

    public enum LogItemImportance
    {
        Height,
        Middle,
        Low,
    }

    public class AsyncObservableCollection<T> : ObservableCollection<T>
    {
        private SynchronizationContext _synchronizationContext = SynchronizationContext.Current;

        public AsyncObservableCollection() { }

        public AsyncObservableCollection(IEnumerable<T> list) : base(list) { }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the CollectionChanged event on the current thread
                RaiseCollectionChanged(e);
            }
            else
            {
                // Raises the CollectionChanged event on the creator thread
                _synchronizationContext.Send(RaiseCollectionChanged, e);
            }
        }

        private void RaiseCollectionChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnCollectionChanged((NotifyCollectionChangedEventArgs)param);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (SynchronizationContext.Current == _synchronizationContext)
            {
                // Execute the PropertyChanged event on the current thread
                RaisePropertyChanged(e);
            }
            else
            {
                // Raises the PropertyChanged event on the creator thread
                _synchronizationContext.Send(RaisePropertyChanged, e);
            }
        }

        private void RaisePropertyChanged(object param)
        {
            // We are in the creator thread, call the base implementation directly
            base.OnPropertyChanged((PropertyChangedEventArgs)param);
        }
    }
}
