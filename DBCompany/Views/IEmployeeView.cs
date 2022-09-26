using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DBCompany.Views
{
    public interface IEmployeeView
    {
        Employee Employee { get;}
        BindingSource Binding{ get; set; }

        event EventHandler AddEvent;
        event EventHandler EditEvent;
        event EventHandler DeleteEvent;
        event EventHandler UpdateEvent;

        void ShowMessageNotExistId();
    }
}
