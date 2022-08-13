using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class MenuService : MarshalByRefObject, IMenuService
    {
        #region Private functions and declaration
        private Menu MapObject(NullHandler oReader)
        {
            Menu oMenu = new Menu();
            oMenu.MenuID = oReader.GetInt32("MenuID");
            oMenu.ParentID = oReader.GetInt32("ParentID");
            oMenu.BUID = oReader.GetInt32("BUID");
            oMenu.MenuName = oReader.GetString("MenuName");
            oMenu.BUName = oReader.GetString("BUName");
            oMenu.ActionName = oReader.GetString("ActionName");
            oMenu.ControllerName = oReader.GetString("ControllerName");
            oMenu.CallingPerameterValue = oReader.GetString("CallingPerameterValue");
            oMenu.CallingPerameterType = oReader.GetString("CallingPerameterType");
            oMenu.ApplicationType = (EnumApplicationType)oReader.GetInt32("ApplicationType");
            oMenu.MenuSequence = oReader.GetInt32("MenuSequence");
            oMenu.IsWithBU = oReader.GetBoolean("IsWithBU");
            oMenu.IsActive = oReader.GetBoolean("IsActive");
            oMenu.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oMenu.MenuLevel = oReader.GetInt32("MenuLevel");

            return oMenu;
        }

        private Menu CreateObject(NullHandler oReader)
        {
            Menu oMenu = new Menu();
            oMenu = MapObject(oReader);
            return oMenu;
        }

        private List<Menu> CreateObjects(IDataReader oReader)
        {
            List<Menu> oMenus = new List<Menu>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Menu oItem = CreateObject(oHandler);
                oMenus.Add(oItem);
            }
            return oMenus;
        }

        #endregion

        #region Interface implementation
        public MenuService() { }

        public Menu Save(Menu oMenu, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMenu.MenuID <= 0)
                {
                    reader = MenuDA.InsertUpdate(tc, oMenu, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MenuDA.InsertUpdate(tc, oMenu, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMenu = new Menu();
                    oMenu = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                #endregion
            }
            return oMenu;
        }

        public Menu RefreshSequence(Menu oMenu, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oMenu.Menus.Count > 0)
                {
                    foreach (Menu oItem in oMenu.Menus)
                    {
                        if (oItem.MenuID > 0 && oItem.MenuSequence > 0)
                        {
                            MenuDA.UpdateSequence(tc, oItem);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                #endregion
            }
            return oMenu;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Menu oMenu = new Menu();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Menu, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Menu", id);
                oMenu.MenuID = id;
                MenuDA.Delete(tc, oMenu, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data Delete Successfully";
        }

        public Menu Get(int id, Int64 nUserId)
        {
            Menu oMenu = new Menu();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MenuDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMenu = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                #endregion
            }

            return oMenu;
        }

        public List<Menu> Gets(int nEnumApplicationType, Int64 nUserID)
        {
            List<Menu> oMenus = new List<Menu>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MenuDA.Gets(tc, nEnumApplicationType);
                oMenus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                Menu oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                oMenus.Add(oMenu);
                #endregion
            }

            return oMenus;
        }

        public List<Menu> Gets(Int64 nUserID)
        {
            List<Menu> oMenus = new List<Menu>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MenuDA.Gets(tc);
                oMenus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                Menu oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                oMenus.Add(oMenu);
                #endregion
            }

            return oMenus;
        }

        public List<Menu> Gets(string sSQL, Int64 nUserID)
        {
            List<Menu> oMenus = new List<Menu>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MenuDA.Gets(tc, sSQL);
                oMenus = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                Menu oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                oMenus.Add(oMenu);
                #endregion
            }

            return oMenus;
        }


        public Menu Get(string ActionName, string ControllerName, Int64 nUserID)
        {
            Menu oMenu = new Menu();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();


                IDataReader reader = MenuDA.Get(tc, ActionName, ControllerName);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMenu = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMenu = new Menu();
                oMenu.ErrorMessage = e.Message;
                #endregion
            }

            return oMenu;
        }

       
        #endregion
    }
}
