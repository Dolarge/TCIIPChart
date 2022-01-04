using Prism.Mvvm;
using System;

namespace TCIIPChart.Model
{
    public class MessageModel :BindableBase
    {
        private string _message;
        public string message 
        {
            get { return _message; }
            set 
            {
                SetProperty(ref _message, value); 
            }  
        }
        private DateTime _time;
        public DateTime Time
        {
            get { return _time; }
            set
            {
                SetProperty(ref _time, value);
            }
        }

    }
}
