using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Diplom;
using Diplom.Utility;

namespace Diplom.ViewModels
{
    public class Navigation:BaseViewModel   
    {
        private object _currentView;
        private string _test;
      
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged();
                }
            }
        }
     


        public Navigation()
        {
           
           _currentView= new LoginViewModel(this);
        }
    }
}
