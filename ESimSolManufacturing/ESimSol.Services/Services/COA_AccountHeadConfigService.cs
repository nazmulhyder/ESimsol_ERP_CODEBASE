using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class COA_AccountHeadConfigService : MarshalByRefObject, ICOA_AccountHeadConfigService
    {
        #region Private functions and declaration
        private COA_AccountHeadConfig MapObject(NullHandler oReader)
        {
            COA_AccountHeadConfig oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
            oCOA_AccountHeadConfig.AccountHeadConfigID = oReader.GetInt32("AccountHeadConfigID");
            oCOA_AccountHeadConfig.AccountHeadID = oReader.GetInt32("AccountHeadID");            
            oCOA_AccountHeadConfig.IsCostCenterApply = oReader.GetBoolean("IsCostCenterApply");
            oCOA_AccountHeadConfig.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oCOA_AccountHeadConfig.IsInventoryApply = oReader.GetBoolean("IsInventoryApply");
            oCOA_AccountHeadConfig.IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");
            oCOA_AccountHeadConfig.IsVoucherReferenceApply = oReader.GetBoolean("IsVoucherReferenceApply");
            oCOA_AccountHeadConfig.CounterHeadID = oReader.GetInt32("CounterHeadID");
            oCOA_AccountHeadConfig.AccountHeadName = oReader.GetString("AccountHeadName");
            oCOA_AccountHeadConfig.AccountCode = oReader.GetString("AccountCode");
            oCOA_AccountHeadConfig.CounterHeadName = oReader.GetString("CounterHeadName");
            oCOA_AccountHeadConfig.CounterHeadCode = oReader.GetString("CounterHeadCode");
            return oCOA_AccountHeadConfig;
        }
        private COA_AccountHeadConfig CreateObject(NullHandler oReader)
        {
            COA_AccountHeadConfig oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
            oCOA_AccountHeadConfig = MapObject(oReader);
            return oCOA_AccountHeadConfig;
        }
        private List<COA_AccountHeadConfig> CreateObjects(IDataReader oReader)
        {
            List<COA_AccountHeadConfig> oCOA_AccountHeadConfig = new List<COA_AccountHeadConfig>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                COA_AccountHeadConfig oItem = CreateObject(oHandler);
                oCOA_AccountHeadConfig.Add(oItem);
            }
            return oCOA_AccountHeadConfig;
        }
        #endregion

        #region Interface implementation

        public COA_AccountHeadConfig Save(COA_AccountHeadConfig oCOA_AccountHeadConfig, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                List<AccountHeadConfigure> oAccHeadCostCenters = new List<AccountHeadConfigure>();
                List<AccountHeadConfigure> oAccHeadProductCategorys = new List<AccountHeadConfigure>();
                BusinessUnitWiseAccountHead oBusinessUnitWiseAccountHead = new BusinessUnitWiseAccountHead();
                List<BusinessUnitWiseAccountHead> oBusinessUnitWiseAccountHeads = new List<BusinessUnitWiseAccountHead>();
                oAccHeadCostCenters = oCOA_AccountHeadConfig.AccHeadCostCenters;
                oAccHeadProductCategorys = oCOA_AccountHeadConfig.AccHeadProductCategorys;
                oBusinessUnitWiseAccountHead.AccountHeadID = oCOA_AccountHeadConfig.AccountHeadID;
                oBusinessUnitWiseAccountHeads = oCOA_AccountHeadConfig.BOAHs;

                tc = TransactionContext.Begin(true);

                #region Insert Update 
                IDataReader reader;
                if (oCOA_AccountHeadConfig.AccountHeadConfigID <= 0)
                {
                    reader = COA_AccountHeadConfigDA.InsertUpdate(tc, oCOA_AccountHeadConfig, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = COA_AccountHeadConfigDA.InsertUpdate(tc, oCOA_AccountHeadConfig, EnumDBOperation.Update, nUserId);
                }
                reader.Close();
                #endregion

                #region Delete
                AccountHeadConfigure oAccountHeadConfigure = new AccountHeadConfigure();
                oAccountHeadConfigure.AccountHeadID = oCOA_AccountHeadConfig.AccountHeadID;                
                AccountHeadConfigureDA.Delete(tc, oAccountHeadConfigure, EnumDBOperation.Delete, nUserId);
                #endregion
                
                #region CCTs
                if (oAccHeadCostCenters != null)
                {
                    foreach (AccountHeadConfigure oItem in oAccHeadCostCenters)
                    {
                        IDataReader readerCCT;
                        oItem.ReferenceObjectType = EnumVoucherExplanationType.CostCenter;
                        oItem.AccountHeadID = oCOA_AccountHeadConfig.AccountHeadID;
                        
                        readerCCT = AccountHeadConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderCCT = new NullHandler(readerCCT);
                        readerCCT.Close();
                    }
                }
                #endregion

                #region IR
                if (oAccHeadProductCategorys != null)
                {
                    foreach (AccountHeadConfigure oItem in oAccHeadProductCategorys)
                    {
                        IDataReader readerCCT;
                        oItem.ReferenceObjectType = EnumVoucherExplanationType.InventoryReference;
                        oItem.AccountHeadID = oCOA_AccountHeadConfig.AccountHeadID;
                        
                        readerCCT = AccountHeadConfigureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderCCT = new NullHandler(readerCCT);
                        readerCCT.Close();
                    }
                }
               
                #endregion

                #region BUAH
                if (oBusinessUnitWiseAccountHeads != null)
                {
                    string sBusinessUnitIDs = "";
                    foreach (BusinessUnitWiseAccountHead oItem in oBusinessUnitWiseAccountHeads)
                    {
                        sBusinessUnitIDs = sBusinessUnitIDs + oItem.BusinessUnitID + ",";
                    }
                    if (sBusinessUnitIDs.Length > 0)
                    {
                        sBusinessUnitIDs = sBusinessUnitIDs.Remove(sBusinessUnitIDs.Length - 1, 1);
                    }
                    IDataReader readertnc;                    
                    readertnc = BusinessUnitWiseAccountHeadDA.IUDFromCOA(tc, oBusinessUnitWiseAccountHead, sBusinessUnitIDs, nUserId);                    
                    readertnc.Close();
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCOA_AccountHeadConfig.ErrorMessage = e.Message;
                #endregion
            }
            return oCOA_AccountHeadConfig;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                COA_AccountHeadConfig oCOA_AccountHeadConfig = new COA_AccountHeadConfig();
                oCOA_AccountHeadConfig.AccountHeadConfigID = id;
                COA_AccountHeadConfigDA.Delete(tc, oCOA_AccountHeadConfig, EnumDBOperation.Delete, nUserId);
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
            return "Data delete successfully";
        }

        public COA_AccountHeadConfig Get(int nAccountHeadID, int nUserId)
        {
            COA_AccountHeadConfig oAccountHead = new COA_AccountHeadConfig();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = COA_AccountHeadConfigDA.Get(tc, nAccountHeadID);
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
                throw new ServiceException("Failed to Get COA_AccountHeadConfig", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<COA_AccountHeadConfig> Gets(int nCompanyID, int nUserID)
        {
            List<COA_AccountHeadConfig> oCOA_AccountHeadConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COA_AccountHeadConfigDA.Gets(tc, nCompanyID);
                oCOA_AccountHeadConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COA_AccountHeadConfig", e);
                #endregion
            }

            return oCOA_AccountHeadConfigs;
        }
        public List<COA_AccountHeadConfig> Gets(string sSQL, int nUserID)
        {
            List<COA_AccountHeadConfig> oCOA_AccountHeadConfigs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = COA_AccountHeadConfigDA.Gets(tc, sSQL);
                oCOA_AccountHeadConfigs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COA_AccountHeadConfig", e);
                #endregion
            }

            return oCOA_AccountHeadConfigs;
        }
        #endregion
    }
}
