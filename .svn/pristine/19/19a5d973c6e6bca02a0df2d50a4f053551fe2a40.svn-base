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
    public class WUSubContractYarnChallanService : MarshalByRefObject, IWUSubContractYarnChallanService
    {
        #region Private functions and declaration
        private WUSubContractYarnChallan MapObject(NullHandler oReader)
        {
            WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            oWUSubContractYarnChallan.WUSubContractYarnChallanID = oReader.GetInt32("WUSubContractYarnChallanID");
            oWUSubContractYarnChallan.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContractYarnChallan.ChallanNo = oReader.GetString("ChallanNo");
            oWUSubContractYarnChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
            oWUSubContractYarnChallan.TruckNo = oReader.GetString("TruckNo");
            oWUSubContractYarnChallan.DriverName = oReader.GetString("DriverName");
            oWUSubContractYarnChallan.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oWUSubContractYarnChallan.Remarks = oReader.GetString("Remarks");
            oWUSubContractYarnChallan.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oWUSubContractYarnChallan.DBUserID = oReader.GetInt32("DBUserID");
            oWUSubContractYarnChallan.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oWUSubContractYarnChallan.ApprovedByName = oReader.GetString("ApprovedByName");
            oWUSubContractYarnChallan.EntyUserName = oReader.GetString("EntyUserName");
            oWUSubContractYarnChallan.JobNo = oReader.GetString("JobNo");
            oWUSubContractYarnChallan.ContractDate = oReader.GetDateTime("ContractDate");
            oWUSubContractYarnChallan.SupplierName = oReader.GetString("SupplierName");
            oWUSubContractYarnChallan.Construction = oReader.GetString("Construction");
            oWUSubContractYarnChallan.BuyerName = oReader.GetString("BuyerName");
            oWUSubContractYarnChallan.OrderQty = oReader.GetDouble("OrderQty");
            oWUSubContractYarnChallan.MUSymbol = oReader.GetString("MUSymbol");
            return oWUSubContractYarnChallan;
        }

        private WUSubContractYarnChallan CreateObject(NullHandler oReader)
        {
            WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            oWUSubContractYarnChallan = MapObject(oReader);
            return oWUSubContractYarnChallan;
        }

        private List<WUSubContractYarnChallan> CreateObjects(IDataReader oReader)
        {
            List<WUSubContractYarnChallan> oWUSubContractYarnChallan = new List<WUSubContractYarnChallan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContractYarnChallan oItem = CreateObject(oHandler);
                oWUSubContractYarnChallan.Add(oItem);
            }
            return oWUSubContractYarnChallan;
        }

        #endregion

        #region Interface implementation
        public WUSubContractYarnChallanService() { }

        public WUSubContractYarnChallan Save(WUSubContractYarnChallan oWUSubContractYarnChallan, Int64 nUserID)
        {
            TransactionContext tc = null;
            oWUSubContractYarnChallan.ErrorMessage = "";

            string sWUSubContractYarnChallanDetailIDs = "";
            List<WUSubContractYarnChallanDetail> oWUSubContractYarnChallanDetails = new List<WUSubContractYarnChallanDetail>();
            WUSubContractYarnChallanDetail oWUSubContractYarnChallanDetail = new WUSubContractYarnChallanDetail();
            oWUSubContractYarnChallanDetails = oWUSubContractYarnChallan.WUSubContractYarnChallanDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oWUSubContractYarnChallan.WUSubContractYarnChallanID <= 0)
                {
                    reader = WUSubContractYarnChallanDA.InsertUpdate(tc, oWUSubContractYarnChallan, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = WUSubContractYarnChallanDA.InsertUpdate(tc, oWUSubContractYarnChallan, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                    oWUSubContractYarnChallan = CreateObject(oReader);
                }
                reader.Close();

                #region WUSubContractYarnChallanDetail

                foreach (WUSubContractYarnChallanDetail oItem in oWUSubContractYarnChallanDetails)
                {
                    IDataReader readerdetail;
                    oItem.WUSubContractYarnChallanID = oWUSubContractYarnChallan.WUSubContractYarnChallanID;
                    if (oItem.WUSubContractYarnChallanDetailID <= 0)
                    {
                        readerdetail = WUSubContractYarnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = WUSubContractYarnChallanDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sWUSubContractYarnChallanDetailIDs = sWUSubContractYarnChallanDetailIDs + oReaderDetail.GetString("WUSubContractYarnChallanDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sWUSubContractYarnChallanDetailIDs.Length > 0)
                {
                    sWUSubContractYarnChallanDetailIDs = sWUSubContractYarnChallanDetailIDs.Remove(sWUSubContractYarnChallanDetailIDs.Length - 1, 1);
                }
                oWUSubContractYarnChallanDetail = new WUSubContractYarnChallanDetail();
                oWUSubContractYarnChallanDetail.WUSubContractYarnChallanID = oWUSubContractYarnChallan.WUSubContractYarnChallanID;
                WUSubContractYarnChallanDetailDA.Delete(tc, oWUSubContractYarnChallanDetail, EnumDBOperation.Delete, nUserID, sWUSubContractYarnChallanDetailIDs);

                #endregion

                #region Get WUSubContractYarnChallan

                reader = WUSubContractYarnChallanDA.Get(tc, oWUSubContractYarnChallan.WUSubContractYarnChallanID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                    oWUSubContractYarnChallan = CreateObject(oReader);
                }
                reader.Close();

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save WUSubContractYarnChallan. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContractYarnChallan;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                oWUSubContractYarnChallan.WUSubContractYarnChallanID = id;
                WUSubContractYarnChallanDA.Delete(tc, oWUSubContractYarnChallan, EnumDBOperation.Delete, nUserId);
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

        public WUSubContractYarnChallan Approve(WUSubContractYarnChallan oWUSubContractYarnChallan, Int64 nUserId)
        {
            TransactionContext tc = null;
            oWUSubContractYarnChallan.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WUSubContractYarnChallan, EnumRoleOperationType.Approved);
                reader = WUSubContractYarnChallanDA.Approve(tc, oWUSubContractYarnChallan, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                    oWUSubContractYarnChallan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                oWUSubContractYarnChallan.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Approve WUSubContractYarnChallan. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContractYarnChallan;
        }

        public WUSubContractYarnChallan Get(int id, Int64 nUserId)
        {
            WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WUSubContractYarnChallanDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContractYarnChallan = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WUSubContractYarnChallan", e);
                #endregion
            }
            return oWUSubContractYarnChallan;
        }

        public List<WUSubContractYarnChallan> Gets(string sSQL, int nCurrentUserID)
        {
            List<WUSubContractYarnChallan> oWUSubContractYarnChallan = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractYarnChallanDA.Gets(tc, sSQL);
                oWUSubContractYarnChallan = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContractYarnChallan", e);
                #endregion
            }

            return oWUSubContractYarnChallan;
        }

        #endregion
    }
}