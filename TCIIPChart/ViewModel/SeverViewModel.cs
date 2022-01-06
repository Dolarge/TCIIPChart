using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using TCIIPChart.Model;

namespace TCIIPChart.ViewModel
{

    public interface IViewModel { }

    public class SeverViewModel : BindableBase, IViewModel
    {


        public SeverViewModel()
        {

            

        }
        
    }
}
