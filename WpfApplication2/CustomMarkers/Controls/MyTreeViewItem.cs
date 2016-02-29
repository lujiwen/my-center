using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfApplication2.CustomMarkers.Controls
{
    public class MyTreeViewItem : TreeViewItem
    {
        Object _nodeObject;
        public Object NodeObject { get { return _nodeObject; } set { _nodeObject = value; } }
        public MyTreeViewItem(Object o)
        {
            this.NodeObject = o;
        }

    }
}
