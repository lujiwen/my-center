using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace WpfApplication2.CustomMarkers.Controls
{
    class MyCheckBox : CheckBox
    {
        public enum CheckBoxType{ cabTyp , deviceType , buildingType,unknow };
        public CheckBoxType type ;
        private int id;
        public int CheckBoxIDs { get { return id; } set { id = value; } }
        private MyTreeViewItem _treeNode;
        private Object _nodeObject;
        public Object NodeObject { get { return _nodeObject; } set { _nodeObject = value; } }

        public MyTreeViewItem TreeNode { get { return _treeNode; } set { _treeNode = value; } }

        public MyCheckBox(CheckBoxType t, MyTreeViewItem treeNode )
        {
            VerticalAlignment = System.Windows.VerticalAlignment.Center;
            HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            type = t;
            this._nodeObject = treeNode.NodeObject; 
            _treeNode = treeNode;
        }
    }
}
