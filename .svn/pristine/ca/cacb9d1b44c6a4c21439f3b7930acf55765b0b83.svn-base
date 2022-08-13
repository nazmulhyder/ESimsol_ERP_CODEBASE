using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ICS.Core.Utility
{
    public class NodeItem
    {
        public NodeItem()
        {
            _sKey = "";
            _sCaption = "";
            _sParentKey = "";
            _bVisible = false;
            _sForm = "";
            _sParameterType = "";
            _sParameterValue = "";
        }

        private int _nHashCode;
        public int Index
        {
            get { return _nHashCode; }
            set { _nHashCode = value; }
        }
        private string _sKey;
        public string Key
        {
            get { return _sKey; }
            set { _sKey = value; }
        }
        private string _sParentKey;
        public string ParentKey
        {
            get { return _sParentKey; }
            set { _sParentKey = value; }
        }
        private bool _bVisible;
        public bool Visible
        {
            get { return _bVisible; }
            set { _bVisible = value; }
        }
        private string _sCaption;
        public string Caption
        {
            get { return _sCaption; }
            set { _sCaption = value; }
        }
        private string _sForm;
        public string Form
        {
            get { return _sForm; }
            set { _sForm = value; }
        }
        private string _sParameterType;
        public string ParameterType
        {
            get { return _sParameterType; }
            set { _sParameterType = value; }
        }
        private string _sParameterValue;
        public string ParameterValue
        {
            get { return _sParameterValue; }
            set { _sParameterValue = value; }
        }

    }


    public class HierarchyTree : CollectionBase
    {
        private Stack _oKeyStack;
        public HierarchyTree()
        {
            _oKeyStack = new Stack();
        }

        public void Add(HierarchyTree oMenuTree)
        {
            foreach (NodeItem oMenuItem in oMenuTree)
            {
                InnerList.Add(oMenuItem);
            }
        }

        private string GetCurrentParent()
        {
            if (_oKeyStack.Count > 0)
            {
                return (string)_oKeyStack.Peek();
            }
            else
            {
                return null;
            }
        }

        private void PushParent(string sKey)
        {
            _oKeyStack.Push(sKey);
        }

        private void PopParent()
        {
            _oKeyStack.Pop();
        }

        public NodeItem this[int i]
        {
            get { return (NodeItem)InnerList[i]; }
            set { InnerList[i] = value; }
        }

        public NodeItem this[string sKey]
        {
            get
            {
                if (!Exists(sKey)) return null;
                return (NodeItem)InnerList[GetIndex(sKey)];
            }
        }
        private int GetIndex(string sKey)
        {
            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (this[i].Key == sKey)
                {
                    return i;
                }
            }
            return 0;
        }

        public string GetKey(int nHashCode)
        {
            int i;
            for (i = 0; i < this.Count; i++)
            {
                if (this[i].Index == nHashCode)
                {
                    return this[i].Key;
                }
            }
            return "";
        }

        private bool Exists(string sKey)
        {
            return !(GetIndex(sKey) == 0);
        }

        public void BeginGroup(string sKey, string sCaption, bool bVisible)
        {
            string sParentKey;
            if (!Exists(sKey))
            {
                NodeItem oItem = new NodeItem();
                sParentKey = GetCurrentParent();

                oItem.Key = sKey;
                oItem.ParentKey = sParentKey;
                oItem.Caption = sCaption;
                oItem.Visible = bVisible;

                if (sParentKey != null)
                {
                    if (!this[(int)GetIndex(sParentKey)].Visible)
                    {
                        oItem.Visible = false;
                    }
                }
                InnerList.Add(oItem);
            }
            PushParent(sKey);
        }

        public void MenuItem(string sKey, string sCaption, bool bVisible)
        {
            string sParentKey;

            if (!Exists(sKey))
            {
                NodeItem oItem = new NodeItem();

                sParentKey = GetCurrentParent();

                oItem.Key = sKey;
                oItem.ParentKey = sParentKey;
                oItem.Caption = sCaption;
                oItem.Visible = bVisible;

                if (sParentKey != null)
                {
                    if (!this[GetIndex(sParentKey)].Visible)
                    {
                        oItem.Visible = false;
                    }
                }

                InnerList.Add(oItem);
            }
        }

        public void EndGroup()
        {
            PopParent();
        }

        public void Add(NodeItem oMenuItem)
        {
            InnerList.Add(oMenuItem);
        }

        public HierarchyTree SubMenuTree(string sParentKey)
        {
            HierarchyTree oSubTree = new HierarchyTree();
            foreach (NodeItem oItem in this)
            {
                if (oItem.ParentKey == sParentKey)
                {
                    oSubTree.Add(oItem);
                }
            }

            return oSubTree;
        }
    }
}