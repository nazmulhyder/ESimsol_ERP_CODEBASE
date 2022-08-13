using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class WUSubContractFabricReceiveService : MarshalByRefObject, IWUSubContractFabricReceiveService
    {
        #region Private functions and declaration
        private WUSubContractFabricReceive MapObject(NullHandler oReader)
        {
            WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            oWUSubContractFabricReceive.WUSubContractFabricReceiveID = oReader.GetInt32("WUSubContractFabricReceiveID");
            oWUSubContractFabricReceive.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContractFabricReceive.ReceiveNo = oReader.GetString("ReceiveNo");
            oWUSubContractFabricReceive.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oWUSubContractFabricReceive.PartyChallanNo = oReader.GetString("PartyChallanNo");
            oWUSubContractFabricReceive.ReceiveStoreID = oReader.GetInt32("ReceiveStoreID");
            oWUSubContractFabricReceive.CompositionID = oReader.GetInt32("CompositionID");
            oWUSubContractFabricReceive.Construction = oReader.GetString("Construction");
            oWUSubContractFabricReceive.LotID = oReader.GetInt32("LotID");
            oWUSubContractFabricReceive.NewLotNo = oReader.GetString("NewLotNo");
            oWUSubContractFabricReceive.MunitID = oReader.GetInt32("MunitID");
            oWUSubContractFabricReceive.ReceivedQty = oReader.GetDouble("ReceivedQty");
            oWUSubContractFabricReceive.RollNo = oReader.GetInt32("RollNo");
            oWUSubContractFabricReceive.ProcessLossQty = oReader.GetDouble("ProcessLossQty");
            oWUSubContractFabricReceive.Remarks = oReader.GetString("Remarks");
            oWUSubContractFabricReceive.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oWUSubContractFabricReceive.DBUserID = oReader.GetInt32("DBUserID");
            oWUSubContractFabricReceive.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oWUSubContractFabricReceive.ApprovedByName = oReader.GetString("ApprovedByName");
            oWUSubContractFabricReceive.EntyUserName = oReader.GetString("EntyUserName");
            oWUSubContractFabricReceive.JobNo = oReader.GetString("JobNo");
            oWUSubContractFabricReceive.ContractDate = oReader.GetDateTime("ContractDate");
            oWUSubContractFabricReceive.SupplierName = oReader.GetString("SupplierName");
            oWUSubContractFabricReceive.SupplierCPName = oReader.GetString("SupplierCPName");
            oWUSubContractFabricReceive.BuyerName = oReader.GetString("BuyerName");
            oWUSubContractFabricReceive.StyleNo = oReader.GetString("StyleNo");
            oWUSubContractFabricReceive.CompositionName = oReader.GetString("CompositionName");
            oWUSubContractFabricReceive.SubContractConstruction = oReader.GetString("SubContractConstruction");
            oWUSubContractFabricReceive.YetToRcvQty = oReader.GetDouble("YetToRcvQty");
            oWUSubContractFabricReceive.MUSymbol = oReader.GetString("MUSymbol");
            oWUSubContractFabricReceive.StoreName = oReader.GetString("StoreName");
            oWUSubContractFabricReceive.BUID = oReader.GetInt32("BUID");
            oWUSubContractFabricReceive.LotNo = oReader.GetString("LotNo");
            return oWUSubContractFabricReceive;
        }

        private WUSubContractFabricReceive CreateObject(NullHandler oReader)
        {
            WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            oWUSubContractFabricReceive = MapObject(oReader);
            return oWUSubContractFabricReceive;
        }

        private List<WUSubContractFabricReceive> CreateObjects(IDataReader oReader)
        {
            List<WUSubContractFabricReceive> oWUSubContractFabricReceive = new List<WUSubContractFabricReceive>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContractFabricReceive oItem = CreateObject(oHandler);
                oWUSubContractFabricReceive.Add(oItem);
            }
            return oWUSubContractFabricReceive;
        }

        #endregion

        #region Interface implementation
        public WUSubContractFabricReceiveService() { }

        public WUSubContractFabricReceive Save(WUSubContractFabricReceive oWUSubContractFabricReceive, Int64 nUserID)
        {
            TransactionContext tc = null;
            oWUSubContractFabricReceive.ErrorMessage = "";
            
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oWUSubContractFabricReceive.WUSubContractFabricReceiveID <= 0)
                {
                    reader = WUSubContractFabricReceiveDA.InsertUpdate(tc, oWUSubContractFabricReceive, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = WUSubContractFabricReceiveDA.InsertUpdate(tc, oWUSubContractFabricReceive, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                    oWUSubContractFabricReceive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save WUSubContractFabricReceive. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContractFabricReceive;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                oWUSubContractFabricReceive.WUSubContractFabricReceiveID = id;
                WUSubContractFabricReceiveDA.Delete(tc, oWUSubContractFabricReceive, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public WUSubContractFabricReceive Approve(WUSubContractFabricReceive oWUSubContractFabricReceive, Int64 nUserId)
        {
            TransactionContext tc = null;
            oWUSubContractFabricReceive.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WUSubContractFabricReceive, EnumRoleOperationType.Approved);
                reader = WUSubContractFabricReceiveDA.Approve(tc, oWUSubContractFabricReceive, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                    oWUSubContractFabricReceive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Approve WUSubContractFabricReceive. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContractFabricReceive;
        }

        public WUSubContractFabricReceive Get(int id, Int64 nUserId)
        {
            WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WUSubContractFabricReceiveDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractFabricReceive = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WUSubContractFabricReceive", e);
                #endregion
            }
            return oWUSubContractFabricReceive;
        }

        public List<WUSubContractFabricReceive> Gets(string sSQL, int nCurrentUserID)
        {
            List<WUSubContractFabricReceive> oWUSubContractFabricReceive = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractFabricReceiveDA.Gets(tc, sSQL);
                oWUSubContractFabricReceive = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContractFabricReceive", e);
                #endregion
            }

            return oWUSubContractFabricReceive;
        }

        #endregion
    }
}