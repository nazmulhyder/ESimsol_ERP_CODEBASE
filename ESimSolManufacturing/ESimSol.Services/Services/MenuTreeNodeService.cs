using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using System.ServiceModel;
using System.Runtime.Serialization;
using ICS.Base.Client.BOFoundation;
using ICS.Base.DataAccess;
using ICS.Base.Utility;
using ICS.Base.FrameWork;


namespace ESimSol.Services.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class MenuTreeNodeService : MarshalByRefObject, IMenuTreeNodeService
    {
        #region Private functions and declaration
        private void MapObject(MenuTreeNode oMenuTreeNode, NullHandler oReader)
        {
            BusinessObject.Factory.SetID(oMenuTreeNode, new ID(oReader.GetInt32("MenuTreeNodeID")));
            oMenuTreeNode.ParentID = oReader.GetInt32("ParentID");
            oMenuTreeNode.MenuName = oReader.GetString("MenuName");
            oMenuTreeNode.LinkText = oReader.GetString("LinkText");
            oMenuTreeNode.ActionName = oReader.GetString("ActionName");
            oMenuTreeNode.ActionName = oReader.GetString("ActionName");
            BusinessObject.Factory.SetObjectState(oMenuTreeNode, ObjectState.Saved);
        }

        private MenuTreeNode CreateObject(NullHandler oReader)
        {
            MenuTreeNode oMenuTreeNode = new MenuTreeNode();
            MapObject(oMenuTreeNode, oReader);
            return oMenuTreeNode;
        }

        private IList<MenuTreeNode> CreateObjects(IDataReader oReader)
        {
            IList<MenuTreeNode> oMenuTreeNodes = new List<MenuTreeNode>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MenuTreeNode oItem = CreateObject(oHandler);
                oMenuTreeNodes.Add(oItem);
            }
            return oMenuTreeNodes;
        }
        #endregion

        #region Interface implementation
        public MenuTreeNodeService() { }

        public ID Save(MenuTreeNode oMenuTreeNode)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                if (oMenuTreeNode.IsNew)
                {
                    BusinessObject.Factory.SetID(oMenuTreeNode, new ID(MenuTreeNodeDA.GetNewID(tc)));
                    MenuTreeNodeDA.Insert(tc, oMenuTreeNode);
                }
                else
                {
                    MenuTreeNodeDA.Update(tc, oMenuTreeNode);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oMenuTreeNode, ObjectState.Saved);
            }
            catch (Exception e)
            {
                if (tc != null) tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save MenuTreeNode", e);
            }
            return oMenuTreeNode.ID;
        }
        public void Delete(ID oID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MenuTreeNodeDA.Delete(tc, oID.ToInt64);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException(e.Message, e);
                #endregion
            }
        }
        public MenuTreeNode Get(ID id)
        {
            MenuTreeNode oMenuTreeNode = new MenuTreeNode();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MenuTreeNodeDA.Get(tc, id.ToInt64);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMenuTreeNode = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MenuTreeNode", e);
                #endregion
            }

            return oMenuTreeNode;
        }
        public IList<MenuTreeNode> Gets()
        {
            IList<MenuTreeNode> oMenuTreeNodes = new List<MenuTreeNode>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MenuTreeNodeDA.Gets(tc);
                oMenuTreeNodes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MenuTreeNodes", e);
                #endregion
            }

            return oMenuTreeNodes;
        }
        #endregion
    }
}
