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
    public class WUSubContractService : MarshalByRefObject, IWUSubContractService
    {
        #region Private functions and declaration
        private WUSubContract MapObject(NullHandler oReader)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract.WUSubContractID = oReader.GetInt32("WUSubContractID");
            oWUSubContract.BUID = oReader.GetInt32("BUID");
            oWUSubContract.JobNo =  oReader.GetString("JobNo");
            oWUSubContract.Version = oReader.GetInt32("Version");
            oWUSubContract.ContractDate = oReader.GetDateTime("ContractDate");
            oWUSubContract.SupplierID = oReader.GetInt32("SupplierID");
            oWUSubContract.ContractPersonID = oReader.GetInt32("ContractPersonID");
            oWUSubContract.ContractBy = oReader.GetInt32("ContractBy");
            oWUSubContract.ContractStatus = (EnumWUSubContractStatus)oReader.GetInt32("ContractStatus");
            oWUSubContract.ContractStatusInt = oReader.GetInt32("ContractStatus");
            oWUSubContract.YarnChallanStatus = (EnumWUYarnChallanStatus)oReader.GetInt32("YarnChallanStatus");
            oWUSubContract.YarnChallanStatusInt = oReader.GetInt32("YarnChallanStatus");
            oWUSubContract.FabricRcvStatus = (EnumWUFabricRcvStatus)oReader.GetInt32("FabricRcvStatus");
            oWUSubContract.FabricRcvStatusInt = oReader.GetInt32("FabricRcvStatus");
            oWUSubContract.PaymentMode = (EnumInvoicePaymentMode)oReader.GetInt32("PaymentMode");
            oWUSubContract.PaymentModeInt = oReader.GetInt32("PaymentMode");
            oWUSubContract.SONo =  oReader.GetString("SONo");
            oWUSubContract.BuyerID = oReader.GetInt32("BuyerID");
            oWUSubContract.StyleNo =  oReader.GetString("StyleNo");
            oWUSubContract.OrderType = (EnumWUOrderType)oReader.GetInt32("OrderType");
            oWUSubContract.OrderTypeInt = oReader.GetInt32("OrderType");
            oWUSubContract.FabricTypeID = oReader.GetInt32("FabricTypeID");
            oWUSubContract.CompositionID = oReader.GetInt32("CompositionID");
            oWUSubContract.Construction =  oReader.GetString("Construction");
            oWUSubContract.GrayWidth =  oReader.GetString("GrayWidth");
            oWUSubContract.GrayPick = oReader.GetInt32("GrayPick");
            oWUSubContract.ReedSpace =  oReader.GetString("ReedSpace");
            oWUSubContract.TotalEnds = oReader.GetInt32("TotalEnds");
            oWUSubContract.ReedCount =  oReader.GetString("ReedCount");
            oWUSubContract.WeaveDesignID = oReader.GetInt32("WeaveDesignID");
            oWUSubContract.WSCWorkType = (EnumWSCWorkType)oReader.GetInt32("WSCWorkType");
            oWUSubContract.WSCWorkTypeInt = oReader.GetInt32("WSCWorkType");
            oWUSubContract.OrderQty = oReader.GetDouble("OrderQty");
            oWUSubContract.MUnitID = oReader.GetInt32("MUnitID");
            oWUSubContract.Rate = oReader.GetDouble("Rate");
            oWUSubContract.RatePerPick = oReader.GetDouble("RatePerPick");
            oWUSubContract.TotalAmount = oReader.GetDouble("TotalAmount");
            oWUSubContract.CRate = oReader.GetDouble("CRate");
            oWUSubContract.CurrencyID = oReader.GetInt32("CurrencyID");
            oWUSubContract.Transportation = (EnumTransportation)oReader.GetInt32("Transportation");
            oWUSubContract.TransportationInt = oReader.GetInt32("Transportation");
            oWUSubContract.ProdStartDate = oReader.GetDateTime("ProdStartDate");
            oWUSubContract.ProdStartComments =  oReader.GetString("ProdStartComments");
            oWUSubContract.ProdCompleteDate = oReader.GetDateTime("ProdCompleteDate");
            oWUSubContract.Remarks =  oReader.GetString("Remarks");
            oWUSubContract.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oWUSubContract.DBUserID = oReader.GetInt32("DBUserID");
            oWUSubContract.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oWUSubContract.BUName =  oReader.GetString("BUName");
            oWUSubContract.BUShortName =  oReader.GetString("BUShortName");
            oWUSubContract.SupplierName =  oReader.GetString("SupplierName");
            oWUSubContract.SupplierAddress = oReader.GetString("SupplierAddress");
            oWUSubContract.BuyerName =  oReader.GetString("BuyerName");
            oWUSubContract.SupplierCPName =  oReader.GetString("SupplierCPName");
            oWUSubContract.SupplierCPDesignation = oReader.GetString("SupplierCPDesignation");
            oWUSubContract.CUSymbol =  oReader.GetString("CUSymbol");
            oWUSubContract.ContractByName =  oReader.GetString("ContractByName");
            oWUSubContract.FabricTypeName =  oReader.GetString("FabricTypeName");
            oWUSubContract.CompositionCode = oReader.GetString("CompositionCode");
            oWUSubContract.CompositionName = oReader.GetString("CompositionName");
            oWUSubContract.WeaveDesignName =  oReader.GetString("WeaveDesignName");
            oWUSubContract.MUSymbol =  oReader.GetString("MUSymbol");
            oWUSubContract.ApprovedByName =  oReader.GetString("ApprovedByName");
            oWUSubContract.EntyUserName =  oReader.GetString("EntyUserName");
            oWUSubContract.EmpDesignation = oReader.GetString("EmpDesignation");
            oWUSubContract.YetToRcvQty = oReader.GetDouble("YetToRcvQty");
            oWUSubContract.WUSubContractLogID = oReader.GetInt32("WUSubContractLogID");
            oWUSubContract.BaseCurrencyID = oReader.GetInt32("BaseCurrencyID");
            oWUSubContract.BCSymbol = oReader.GetString("BCSymbol");
            return oWUSubContract;
        }

        private WUSubContract CreateObject(NullHandler oReader)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract = MapObject(oReader);
            return oWUSubContract;
        }

        private List<WUSubContract> CreateObjects(IDataReader oReader)
        {
            List<WUSubContract> oWUSubContract = new List<WUSubContract>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUSubContract oItem = CreateObject(oHandler);
                oWUSubContract.Add(oItem);
            }
            return oWUSubContract;
        }

        #endregion

        #region Interface implementation
        public WUSubContractService() { }
        public WUSubContract Save(WUSubContract oWUSubContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            oWUSubContract.ErrorMessage = "";

            string sWUSubContractYarnConsumptionIDs = "";
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
            WUSubContractYarnConsumption oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
            oWUSubContractYarnConsumptions = oWUSubContract.WUSubContractYarnConsumptions;

            string sWUSubContractTermsConditionIDs = "";
            List<WUSubContractTermsCondition> oWUSubContractTermsConditions = new List<WUSubContractTermsCondition>();
            WUSubContractTermsCondition oWUSubContractTermsCondition = new WUSubContractTermsCondition();
            oWUSubContractTermsConditions = oWUSubContract.WUSubContractTermsConditions;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oWUSubContract.WUSubContractID <= 0)
                {
                    reader = WUSubContractDA.InsertUpdate(tc, oWUSubContract, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = WUSubContractDA.InsertUpdate(tc, oWUSubContract, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
                }
                reader.Close();

                #region WUSubContractYarnConsumption

                foreach (WUSubContractYarnConsumption oItem in oWUSubContractYarnConsumptions)
                {
                    IDataReader readerdetail;
                    oItem.WUSubContractID = oWUSubContract.WUSubContractID;
                    if (oItem.WUSubContractYarnConsumptionID <= 0)
                    {
                        readerdetail = WUSubContractYarnConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = WUSubContractYarnConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sWUSubContractYarnConsumptionIDs = sWUSubContractYarnConsumptionIDs + oReaderDetail.GetString("WUSubContractYarnConsumptionID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sWUSubContractYarnConsumptionIDs.Length > 0)
                {
                    sWUSubContractYarnConsumptionIDs = sWUSubContractYarnConsumptionIDs.Remove(sWUSubContractYarnConsumptionIDs.Length - 1, 1);
                }
                oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
                oWUSubContractYarnConsumption.WUSubContractID = oWUSubContract.WUSubContractID;
                WUSubContractYarnConsumptionDA.Delete(tc, oWUSubContractYarnConsumption, EnumDBOperation.Delete, nUserID, sWUSubContractYarnConsumptionIDs);

                #endregion

                #region WUSubContractTermsCondition

                foreach (WUSubContractTermsCondition oItem in oWUSubContractTermsConditions)
                {
                    IDataReader readerdetail;
                    oItem.WUSubContractID = oWUSubContract.WUSubContractID;
                    if (oItem.WUSubContractTermsConditionID <= 0)
                    {
                        readerdetail = WUSubContractTermsConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = WUSubContractTermsConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sWUSubContractTermsConditionIDs = sWUSubContractTermsConditionIDs + oReaderDetail.GetString("WUSubContractTermsConditionID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sWUSubContractTermsConditionIDs.Length > 0)
                {
                    sWUSubContractTermsConditionIDs = sWUSubContractTermsConditionIDs.Remove(sWUSubContractTermsConditionIDs.Length - 1, 1);
                }
                oWUSubContractTermsCondition = new WUSubContractTermsCondition();
                oWUSubContractTermsCondition.WUSubContractID = oWUSubContract.WUSubContractID;
                WUSubContractTermsConditionDA.Delete(tc, oWUSubContractTermsCondition, EnumDBOperation.Delete, nUserID, sWUSubContractTermsConditionIDs);

                #endregion

                #region Get WUSubContract

                reader = WUSubContractDA.Get(tc, oWUSubContract.WUSubContractID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save WUSubContract. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContract;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                WUSubContract oWUSubContract = new WUSubContract();
                oWUSubContract.WUSubContractID = id;
                WUSubContractDA.Delete(tc, oWUSubContract, EnumDBOperation.Delete, nUserId);
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

        public WUSubContract Approve(WUSubContract oWUSubContract, Int64 nUserId)
        {
            TransactionContext tc = null;
            oWUSubContract.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.WUSubContract, EnumRoleOperationType.Approved);
                reader = WUSubContractDA.Approve(tc, oWUSubContract, EnumDBOperation.Approval, nUserId);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Approve WUSubContract. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContract;
        }

        public WUSubContract FinishYarnChallan(WUSubContract oWUSubContract, Int64 nUserId)
        {
            TransactionContext tc = null;
            oWUSubContract.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                WUSubContractDA.FinishYarnChallan(tc, oWUSubContract, nUserId);
                
                IDataReader reader;
                reader = WUSubContractDA.Get(tc, oWUSubContract.WUSubContractID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Approve WUSubContract. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContract;
        }

        public WUSubContract AcceptRevise(WUSubContract oWUSubContract, Int64 nUserID)
        {
            TransactionContext tc = null;
            oWUSubContract.ErrorMessage = "";

            string sWUSubContractYarnConsumptionIDs = "";
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
            WUSubContractYarnConsumption oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
            oWUSubContractYarnConsumptions = oWUSubContract.WUSubContractYarnConsumptions;

            string sWUSubContractTermsConditionIDs = "";
            List<WUSubContractTermsCondition> oWUSubContractTermsConditions = new List<WUSubContractTermsCondition>();
            WUSubContractTermsCondition oWUSubContractTermsCondition = new WUSubContractTermsCondition();
            oWUSubContractTermsConditions = oWUSubContract.WUSubContractTermsConditions;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.WUSubContract, EnumRoleOperationType.Revise);
                reader = WUSubContractDA.AcceptRevise(tc, oWUSubContract, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
                }
                reader.Close();

                #region WUSubContractYarnConsumption

                foreach (WUSubContractYarnConsumption oItem in oWUSubContractYarnConsumptions)
                {
                    IDataReader readerdetail;
                    oItem.WUSubContractID = oWUSubContract.WUSubContractID;
                    if (oItem.WUSubContractYarnConsumptionID <= 0)
                    {
                        readerdetail = WUSubContractYarnConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = WUSubContractYarnConsumptionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sWUSubContractYarnConsumptionIDs = sWUSubContractYarnConsumptionIDs + oReaderDetail.GetString("WUSubContractYarnConsumptionID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sWUSubContractYarnConsumptionIDs.Length > 0)
                {
                    sWUSubContractYarnConsumptionIDs = sWUSubContractYarnConsumptionIDs.Remove(sWUSubContractYarnConsumptionIDs.Length - 1, 1);
                }
                oWUSubContractYarnConsumption = new WUSubContractYarnConsumption();
                oWUSubContractYarnConsumption.WUSubContractID = oWUSubContract.WUSubContractID;
                WUSubContractYarnConsumptionDA.Delete(tc, oWUSubContractYarnConsumption, EnumDBOperation.Delete, nUserID, sWUSubContractYarnConsumptionIDs);

                #endregion

                #region WUSubContractTermsCondition

                foreach (WUSubContractTermsCondition oItem in oWUSubContractTermsConditions)
                {
                    IDataReader readerdetail;
                    oItem.WUSubContractID = oWUSubContract.WUSubContractID;
                    if (oItem.WUSubContractTermsConditionID <= 0)
                    {
                        readerdetail = WUSubContractTermsConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerdetail = WUSubContractTermsConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sWUSubContractTermsConditionIDs = sWUSubContractTermsConditionIDs + oReaderDetail.GetString("WUSubContractTermsConditionID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sWUSubContractTermsConditionIDs.Length > 0)
                {
                    sWUSubContractTermsConditionIDs = sWUSubContractTermsConditionIDs.Remove(sWUSubContractTermsConditionIDs.Length - 1, 1);
                }
                oWUSubContractTermsCondition = new WUSubContractTermsCondition();
                oWUSubContractTermsCondition.WUSubContractID = oWUSubContract.WUSubContractID;
                WUSubContractTermsConditionDA.Delete(tc, oWUSubContractTermsCondition, EnumDBOperation.Delete, nUserID, sWUSubContractTermsConditionIDs);

                #endregion

                #region Get WUSubContract

                reader = WUSubContractDA.Get(tc, oWUSubContract.WUSubContractID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = new WUSubContract();
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save WUSubContract. Because of " + e.Message, e);
                #endregion
            }
            return oWUSubContract;
        }

        public WUSubContract Get(int id, Int64 nUserId)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WUSubContractDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WUSubContract", e);
                #endregion
            }
            return oWUSubContract;
        }

        public WUSubContract GetRevise(int id, Int64 nUserId)
        {
            WUSubContract oWUSubContract = new WUSubContract();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = WUSubContractDA.GetRevise(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWUSubContract = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get WUSubContract", e);
                #endregion
            }
            return oWUSubContract;
        }

        public List<WUSubContract> Gets(int bid, Int64 nUserID)
        {
            List<WUSubContract> oWUSubContracts = new List<WUSubContract>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractDA.Gets(tc, bid);
                oWUSubContracts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContract", e);
                #endregion
            }
            return oWUSubContracts;
        }

        public List<WUSubContract> Get(string sSQL, int nCurrentUserID)
        {
            List<WUSubContract> oWUSubContract = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractDA.Get(tc, sSQL);
                oWUSubContract = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContract", e);
                #endregion
            }

            return oWUSubContract;
        }

        public List<WUSubContract> GetsPrint(string sSQL, int nCurrentUserID)
        {
            List<WUSubContract> oWUSubContract = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUSubContractDA.GetsPrint(tc, sSQL);
                oWUSubContract = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WUSubContract", e);
                #endregion
            }

            return oWUSubContract;
        }

        #endregion
    }   
}