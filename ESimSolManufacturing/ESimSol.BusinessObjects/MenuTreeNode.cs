using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Base.Client.BOFoundation;
using ICS.Base.Client.ServiceVessel;
using System.Reflection;
using System.Reflection.Emit;

namespace ESimSol.BusinessObjects
{

    #region MenuTreeNode
    [KnownType(typeof(BusinessObject))]
    [KnownType(typeof(ID))]
    [DataContract]
    public class MenuTreeNode : BusinessObject
    {
        public MenuTreeNode() { }

        #region Properties
        [DataMember]
        public int ParentID { get; set; }

        [DataMember]
        public string MenuName { get; set; }

        [DataMember]
        public string LinkText { get; set; }

        [DataMember]
        public string ActionName { get; set; }

        [DataMember]
        public string ControllerName { get; set; }

        [DataMember]
        public MenuTreeNode Parent;

        #region SubItems
        private MenuTreeNodes _oSubItems = new MenuTreeNodes();
        public MenuTreeNodes SubItems
        {
            get { return _oSubItems; }
            set { _oSubItems = value; }
        }
        #endregion

        #region SubNodes
        private List<MenuTreeNode> _oSubNodes = new List<MenuTreeNode>();
        public List<MenuTreeNode> SubNodes
        {
            get { return _oSubNodes; }
            set { _oSubNodes = value; }
        }
        #endregion
        
        #region IsViewed
        private bool _bIsViewed = false;
        public bool IsViewed
        {
            get { return _bIsViewed; }
            set { _bIsViewed = value; }
        }
        #endregion

        #endregion

        #region Functions
        public MenuTreeNode Get(int nMenuTreeNodeID)
        {
            return (MenuTreeNode)ICSWCFServiceClient.CallMethod(ServiceType, "Get", new ID(nMenuTreeNodeID));
        }
        public static List<MenuTreeNode> Gets()
        {
            return (List<MenuTreeNode>)ICSWCFServiceClient.CallMethod(ServiceType, "Gets");
        }
        public ID Save()
        {
            return (ID)ICSWCFServiceClient.CallMethod(ServiceType, "Save", this);
        }

        #endregion

        #region Non DB Function
        public static List<MenuTreeNode> SubMenuTree(int nParentID, List<MenuTreeNode> oMenuTreeNodes)
        {
            List<MenuTreeNode> oSubTree = new List<MenuTreeNode>();
            foreach (MenuTreeNode oItem in oMenuTreeNodes)
            {
                if (oItem.ParentID == nParentID)
                {
                    oSubTree.Add(oItem);
                }
            }
            return oSubTree;
        }
        #endregion

        #region ServiceFactory
        internal static Type ServiceType
        {
            get
            {
                return typeof(IMenuTreeNodeService);
            }
        }

        #endregion
    }
    #endregion

    #region MenuTreeNodes
    public class MenuTreeNodes : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(MenuTreeNode oItem)
        {
            base.AddItem(oItem);
        }
        public void Remove(MenuTreeNode oItem)
        {
            base.RemoveItem(oItem);
        }
        public MenuTreeNode this[int index]
        {
            get { return (MenuTreeNode)GetItem(index); }
        }
        public int GetIndex(int nID)
        {
            return base.GetIndex(new ID(nID));
        }
        #endregion

        //[OperationContract]
        //public static MenuTreeNodes Gets()
        //{
        //    return MenuTreeNode.Service.Gets();
        //}


        #region Non DB Function
        public MenuTreeNodes SubMenuTree(int nParentID)
        {
            MenuTreeNodes oSubTree = new MenuTreeNodes();
            foreach (MenuTreeNode oItem in this)
            {
                if (oItem.ParentID == nParentID)
                {
                    oSubTree.Add(oItem);
                }
            }
            return oSubTree;
        }
        #endregion
    }
    #endregion

    #region IMenuTreeNode interface
    [ServiceContract]
    public interface IMenuTreeNodeService
    {
        [OperationContract]
        MenuTreeNode Get(ID id);
        [OperationContract]
        IList<MenuTreeNode> Gets();
        [OperationContract]
        void Delete(ID oID);
        [OperationContract]
        ID Save(MenuTreeNode oMenuTreeNode);
    }
    #endregion
}
