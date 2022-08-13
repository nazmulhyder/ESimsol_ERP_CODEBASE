using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region Menu

    public class Menu : BusinessObject
    {
        public Menu()
        {
            MenuID = 0;
            ParentID = 0;
            MenuName = "";
            ActionName = "";
            ControllerName = "";
            CallingPerameterValue = "";
            CallingPerameterType = "";
            MenuSequence = 0;
            BUID = 0;
            BUName = "";
            IsWithBU = false;
            IsActive = true;
            ApplicationType = EnumApplicationType.WebApplication;
            ModuleName = EnumModuleName.None;
            MenuLevel = 0;
            PermittedOperation = "";
        }

        #region Properties

        public int MenuID { get; set; }
        public int ParentID { get; set; }
        public int BUID { get; set; }
        public string BUName { get; set; }
        public string MenuName { get; set; }
        public bool IsWithBU { get; set; }
        public bool IsActive { get; set; }
        public string ActionName { get; set; }

        public string ControllerName { get; set; }

        public string CallingPerameterValue { get; set; }

        public string CallingPerameterType { get; set; }

        public EnumApplicationType ApplicationType { get; set; }
        public int MenuSequence { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public int MenuLevel { get; set; }
        public string ErrorMessage { get; set; }

        #region Derived Property
        public IEnumerable<Menu> ChildNodes { get; set; }
        public Menu Parent { get; set; }

        public bool IsChild { get; set; }
        public string PermittedOperation { get; set; }
        public bool IsSibling { get; set; }

        public string DisplayMessage { get; set; }
        public List<Menu> Menus { get; set; }
        public int[] Permissions { get; set; }
        public string IsWithBUInString { get { if (this.IsWithBU)return "Yes"; else return "No"; } }
        public string ModuleNameText
        {
            get
            {
                return EnumObject.jGet(this.ModuleName);
            }
        }
        #endregion

        #endregion

        #region Functions

        public static List<Menu> Gets(int nEnumApplicationType, long nUserID) // Enumtype value not pass with wcf service thats way pass int valu convert from enum
        {
            return Menu.Service.Gets(nEnumApplicationType, nUserID);
        }

        public static List<Menu> Gets(long nUserID)
        {
            return Menu.Service.Gets(nUserID);
        }

        public static List<Menu> Gets(string sSQL, long nUserID)
        {
            return Menu.Service.Gets(sSQL, nUserID);
        }
        public Menu Get(int id, long nUserID)
        {
            return Menu.Service.Get(id, nUserID);
        }
        public Menu Save(long nUserID)
        {

            return Menu.Service.Save(this, nUserID);
        }
        public Menu RefreshSequence(long nUserID)
        {
            return Menu.Service.RefreshSequence(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return Menu.Service.Delete(id, nUserID);
        }
        public static Menu Get(string ActionName, string ControllerName, long nUserID)
        {
            return Menu.Service.Get(ActionName, ControllerName , nUserID);
        }
        #region Tree Menu

        private User _oUser;
        private List<Menu> _oMenuList = new List<Menu>();
        public Menu PrepareTree(List<Menu> oMenuList, User oUser)
        {
            _oUser = oUser;
            _oMenuList = oMenuList;

            Menu oRoot = new Menu();

            foreach (Menu oItem in _oMenuList)
            {
                if (oItem.ParentID == 0)
                {
                    oRoot = oItem;
                    break;
                }
            }
            this.AddChildNodes(ref oRoot);

            return oRoot;
        }

        private void AddChildNodes(ref Menu oRoot)
        {
            IEnumerable<Menu> oChildNodes;
            oChildNodes = this.GetChildNodes(oRoot.MenuID);
            oRoot.ChildNodes = oChildNodes;

            foreach (Menu oItem in oChildNodes)
            {
                Menu oTemp = oItem;
                AddChildNodes(ref oTemp);
            }
        }

        public IEnumerable<Menu> GetChildNodes(int nParentID)
        {
            List<Menu> oReturnItems = new List<Menu>();
            foreach (Menu oItem in _oMenuList)
            {
                if (oItem.ParentID == nParentID)
                {
                    if ((_oUser.IsSuperUser) || _oUser.IsPermitted(oItem.MenuID))
                    {
                        oReturnItems.Add(oItem);
                    }
                }
            }
            return oReturnItems;
        }
        #endregion
        #endregion

        #region ServiceFactory

        internal static IMenuService Service
        {
            get { return (IMenuService)Services.Factory.CreateService(typeof(IMenuService)); }
        }

        #endregion
    }
    #endregion

    #region Menus
    public class Menus : IndexedBusinessObjects
    {
        #region Collection Class Methods
        public void Add(Menu item)
        {
            base.AddItem(item);
        }
        public void Remove(Menu item)
        {
            base.RemoveItem(item);
        }
        public Menu this[int index]
        {
            get { return (Menu)GetItem(index); }
        }
        public int GetIndex(int id)
        {
            return base.GetIndex(new ID(id));
        }
        #endregion
    }
    #endregion

    #region IMenuService interface

    public interface IMenuService
    {

        Menu Get(int id, Int64 nUserID);

        List<Menu> Gets(int nEnumApplicationType, Int64 nUserID);

        string Delete(int id, Int64 nUserID);

        List<Menu> Gets(Int64 nUserID);

        List<Menu> Gets(string sSQL, Int64 nUserID);

        Menu Save(Menu oMenu, Int64 nUserID);
        Menu RefreshSequence(Menu oMenu, long nUserID);

        Menu Get(string ActionName, string ControllerName, Int64 nUserID);
    }
    #endregion

    #region TMenu
    public class TMenu
    {
        public TMenu()
        {
            id = 0;
            text = "";
            state = "open";
            parentid = 0;
            ControllerName = "";
            BUName = "";
            IsWithBU = false;
            BUID = 0;
            ActionName = "";
            IsActive = true;
            ModuleName = EnumModuleName.None;
            ActivityInString = "";
            MenuLevel = 0;
        }
        public int id { get; set; }                 //: node id, which is important to load remote data
        public string text { get; set; }            //: node text to show
        public string state { get; set; }           //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string ControllerName { get; set; }  //: node state, 'open' or 'closed', default is 'open'. When set to 'closed', the node have children nodes and will load them from remote site        
        public string ActionName { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public string attributes { get; set; }      //: custom attributes can be added to a node // attributes set a sting that hold action name & controller namr seperated by '~'
        public int parentid { get; set; }
        public string BUName { get; set; }
        public bool IsWithBU { get; set; }
        public bool IsActive { get; set; }
        public EnumModuleName ModuleName { get; set; }
        public string ActivityInString { get; set; }
        public int BUID { get; set; }
        public IEnumerable<TMenu> children { get; set; }//: an array nodes defines some children nodes
        public Company Company { get; set; }
        public List<TMenu> TMenus { get; set; }
        public int MenuLevel { get; set; }
    }
    #endregion
}
