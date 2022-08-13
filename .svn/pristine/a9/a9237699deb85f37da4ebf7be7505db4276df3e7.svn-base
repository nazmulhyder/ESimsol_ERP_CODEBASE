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
    public class VOrderService : MarshalByRefObject, IVOrderService
    {
        #region Private functions and declaration
        private VOrder MapObject(NullHandler oReader)
        {
            VOrder oVOrder = new VOrder();
            oVOrder.VOrderID = oReader.GetInt32("VOrderID");
            oVOrder.BUID = oReader.GetInt32("BUID");
            oVOrder.RefNo = oReader.GetString("RefNo");
            oVOrder.VOrderRefType = (EnumVOrderRefType)oReader.GetInt32("VOrderRefType");
            oVOrder.VOrderRefTypeInt = oReader.GetInt32("VOrderRefType");
            oVOrder.VOrderRefID = oReader.GetInt32("VOrderRefID");
            oVOrder.OrderNo = oReader.GetString("OrderNo");
            oVOrder.OrderDate = oReader.GetDateTime("OrderDate");
            oVOrder.SubledgerID = oReader.GetInt32("SubledgerID");
            oVOrder.Remarks = oReader.GetString("Remarks");
            oVOrder.SubledgerName = oReader.GetString("SubledgerName");
            oVOrder.BUName = oReader.GetString("BUName");
            oVOrder.BUCode = oReader.GetString("BUCode");
            oVOrder.VOrderRefNo = oReader.GetString("VOrderRefNo");
            return oVOrder;
        }
        private VOrder CreateObject(NullHandler oReader)
        {
            VOrder oVOrder = new VOrder();
            oVOrder = MapObject(oReader);
            return oVOrder;
        }
        private List<VOrder> CreateObjects(IDataReader oReader)
        {
            List<VOrder> oVOrder = new List<VOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VOrder oItem = CreateObject(oHandler);
                oVOrder.Add(oItem);
            }
            return oVOrder;
        }

        #endregion

        #region Interface implementation
        public VOrderService() { }
        public VOrder Save(VOrder oVOrder, Int64 nUserID)
        {
            TransactionContext tc = null;            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVOrder.VOrderID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VOrder, EnumRoleOperationType.Add);
                    reader = VOrderDA.InsertUpdate(tc, oVOrder, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VOrder, EnumRoleOperationType.Edit);
                    reader = VOrderDA.InsertUpdate(tc, oVOrder, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVOrder = new VOrder();
                    oVOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVOrder = new VOrder();
                oVOrder.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oVOrder;
        }        
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VOrder oVOrder = new VOrder();
                oVOrder.VOrderID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VOrder, EnumRoleOperationType.Delete);
                
                VOrderDA.Delete(tc, oVOrder, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VOrder. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
        public VOrder Get(int id, Int64 nUserId)
        {
            VOrder oAccountHead = new VOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VOrderDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VOrder", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<VOrder> Gets(Int64 nUserID)
        {
            List<VOrder> oVOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOrderDA.Gets(tc);
                oVOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOrder", e);
                #endregion
            }

            return oVOrder;
        }
        public List<VOrder> Gets(string sSQL, Int64 nUserID)
        {
            List<VOrder> oVOrder = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VOrderDA.Gets(tc, sSQL);
                oVOrder = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VOrder", e);
                #endregion
            }

            return oVOrder;
        }
        #endregion
    }   

    
}
