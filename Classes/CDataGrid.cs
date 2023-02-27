using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ProductsClient.Classes
{
    public class CDataGrid : DataGrid
    {
        static CDataGrid()
        {
            ItemsSourceProperty.OverrideMetadata(typeof(CDataGrid), new FrameworkPropertyMetadata(null, null));
        }
    }
}
