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

    [Serializable]
    public class ImportLCService : MarshalByRefObject, IImportLCService
    {
        #region Private functions and declaration
        private ImportLC MapObject(NullHandler oReader)
        {
            ImportLC oImportLC = new ImportLC();
            oImportLC.ImportLCID= oReader.GetInt32("ImportLCID");
            oImportLC.ImportLCLogID = oReader.GetInt32("ImportLCLogID");
            oImportLC.AmendmentNo = oReader.GetInt32("AmendmentNo");
          
            oImportLC.ContractorID = oReader.GetInt32("ContractorID");
            oImportLC.BankBranchID_Nego = oReader.GetInt32("BankBranchID_Nego");
            oImportLC.InsuranceCompanyID = oReader.GetInt32("InsuranceCompanyID");
            oImportLC.InsuranceName = oReader.GetString("InsuranceName");
            oImportLC.Amount = oReader.GetDouble("Amount");
            oImportLC.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportLC.BBankRefNo = oReader.GetString("BBankRefNo");
            oImportLC.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportLC.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oImportLC.AckmentRecDate = oReader.GetDateTime("AckmentRecDate");
            oImportLC.ForwardDate = oReader.GetDateTime("ForwardDate");
            oImportLC.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oImportLC.ReceiveAllInvoices = oReader.GetBoolean("ReceiveAllInvoices");
            oImportLC.CCRate = oReader.GetDouble("CCRate");
            oImportLC.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportLC.LCPaymentType = (EnumLCPaymentType)oReader.GetInt32("LCPaymentType");
            oImportLC.LCPaymentTypeInt = oReader.GetInt32("LCPaymentType");
            oImportLC.LCCurrentStatus = (EnumLCCurrentStatus)oReader.GetInt32("LCCurrentStatus");
            oImportLC.LCCurrentStatusInt = oReader.GetInt32("LCCurrentStatus");
            oImportLC.ExpireDate = oReader.GetDateTime("ExpireDate");
            oImportLC.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportLC.LCCoverNoteNo = oReader.GetString("LCCoverNoteNo");
            oImportLC.CoverNoteDate = oReader.GetDateTime("CoverNoteDate");
            oImportLC.LCRequestDate = oReader.GetDateTime("LCRequestDate");
            oImportLC.LCANo = oReader.GetString("LCANo");
            oImportLC.FileNo = oReader.GetString("FileNo");
            oImportLC.ContractorName = oReader.GetString("ContractorName");
            oImportLC.CurrencyName = oReader.GetString("CurrencyName");
            oImportLC.Currency = oReader.GetString("Currency");
            oImportLC.BankName_Nego = oReader.GetString("BankName_Nego");
            oImportLC.BankAddress_Nego = oReader.GetString("BankAddress_Nego");
            oImportLC.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oImportLC.InsuranceCompanyName = oReader.GetString("InsuranceCompanyName");
            oImportLC.ReceivedByUserName = oReader.GetString("ReceivedByUserName");
            oImportLC.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oImportLC.IsPartShipmentAllow = oReader.GetBoolean("IsPartShipmentAllow");
            oImportLC.IsTransShipmentAllow = oReader.GetBoolean("IsTransShipmentAllow");
            oImportLC.LCTermID = oReader.GetInt32("LCTermID");
            oImportLC.LCTermID_Bene = oReader.GetInt32("LCTermID_Bene");
            oImportLC.LCTermsName = oReader.GetString("LCTermsName");
            oImportLC.LCTermsName_Bene = oReader.GetString("LCTermsName_Bene");
            oImportLC.PaymentInstructionInt = oReader.GetInt32("PaymentInstructionType");
            oImportLC.Amount_Invoice = oReader.GetDouble("Amount_Invoice");
            oImportLC.BankAccountID = oReader.GetInt32("BankAccountID");
            oImportLC.LCAppType = (EnumLCAppType)oReader.GetInt32("LCAppType");
            oImportLC.LCAppTypeInt = oReader.GetInt32("LCAppType");
            oImportLC.LCMargin = oReader.GetDouble("LCMargin");
            oImportLC.Tolerance = oReader.GetDouble("Tolerance");
            oImportLC.IsConfirmation = oReader.GetBoolean("IsConfirmation");
            oImportLC.BUID = oReader.GetInt32("BUID");
            oImportLC.BUName = oReader.GetString("BUName");
            oImportLC.BUShortName = oReader.GetString("BUShortName");
            oImportLC.ImportPIType = (EnumImportPIType)oReader.GetInt32("ImportPIType");
            oImportLC.LCChargeType = (EnumLCChargeType)oReader.GetInt32("LCChargeType");
            oImportLC.LCChargeTypeInt = oReader.GetInt32("LCChargeType");
            return oImportLC;
        }

        private ImportLC CreateObject(NullHandler oReader)
        {
            ImportLC oImportLC = new ImportLC();
            oImportLC=MapObject(oReader);
            return oImportLC;
        }
       

        private List<ImportLC> CreateObjects(IDataReader oReader)
        {
            List<ImportLC> oImportLCs = new List<ImportLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLC oItem = CreateObject(oHandler);
                oImportLCs.Add(oItem);
            }
            return oImportLCs;
        }
        #endregion

        #region Interface implementation
        public ImportLCService() { }
        #region New Version By Mamun on 02 March 2015
        public ImportLC Save(ImportLC oImportLC, Int64 nUserID)
        {
            double nAmount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportLCDetail> oImportLCDetails = new List<ImportLCDetail>();
                ImportLCDetail oImportLCDetail = new ImportLCDetail();
                oImportLCDetails = oImportLC.ImportLCDetails;
                string sImportLCDetailIDs = "";

                IDataReader reader;
                if (oImportLC.ImportLCID <= 0)
                {
                    reader = ImportLCDA.InsertUpdate(tc, oImportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportLCDA.InsertUpdate(tc, oImportLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = new ImportLC();
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();

                #region Import LC Detail
                foreach (ImportLCDetail oItem in oImportLCDetails)
                {
                    IDataReader readerImportLCDetail;
                    oItem.ImportLCID = oImportLC.ImportLCID;
                    nAmount = nAmount + oItem.Amount;
                    if (oItem.ImportLCDetailID <= 0)
                    {
                        readerImportLCDetail = ImportLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerImportLCDetail = ImportLCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetailImportLCDetail = new NullHandler(readerImportLCDetail);
                    if (readerImportLCDetail.Read())
                    {
                        sImportLCDetailIDs = sImportLCDetailIDs + oReaderDetailImportLCDetail.GetString("ImportLCDetailID") + ",";
                    }
                    readerImportLCDetail.Close();

                }
                oImportLC.Amount = nAmount;
                if (sImportLCDetailIDs.Length > 0)
                {
                    sImportLCDetailIDs = sImportLCDetailIDs.Remove(sImportLCDetailIDs.Length - 1, 1);
                }
                oImportLCDetail = new ImportLCDetail();
                oImportLCDetail.ImportLCID = oImportLC.ImportLCID;
                ImportLCDetailDA.Delete(tc, oImportLCDetail, EnumDBOperation.Delete, nUserID, sImportLCDetailIDs);
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportLC;

        }
        public ImportLC SaveLog(ImportLC oImportLC, Int64 nUserID)
        {
            double nAmount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ImportLCDetail> oImportLCDetails = new List<ImportLCDetail>();
                ImportLCDetail oImportLCDetail = new ImportLCDetail();
                oImportLCDetails = oImportLC.ImportLCDetails;
                string sImportLCDetailIDs = "";

                IDataReader reader;
                if (oImportLC.ImportLCLogID <= 0)
                {
                    reader = ImportLCDA.InsertUpdateLog(tc, oImportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportLCDA.InsertUpdateLog(tc, oImportLC, EnumDBOperation.Update, nUserID);
                }        
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = new ImportLC();
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();

                #region Import LC Detail
                foreach (ImportLCDetail oItem in oImportLCDetails)
                {
                    
                    IDataReader readerImportLCDetail;
                    oItem.ImportLCID = oImportLC.ImportLCID;
                    oItem.ImportLCLogID = oImportLC.ImportLCLogID;
                    nAmount = nAmount + oItem.Amount;
                    if (oItem.ImportLCDetailLogID <= 0)
                    {
                        readerImportLCDetail = ImportLCDetailDA.InsertUpdateLog(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerImportLCDetail = ImportLCDetailDA.InsertUpdateLog(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetailImportLCDetail = new NullHandler(readerImportLCDetail);
                    if (readerImportLCDetail.Read())
                    {
                        sImportLCDetailIDs = sImportLCDetailIDs + oReaderDetailImportLCDetail.GetString("ImportLCDetailLogID") + ",";
                    }
                    readerImportLCDetail.Close();

                }
                if (sImportLCDetailIDs.Length > 0)
                {
                    sImportLCDetailIDs = sImportLCDetailIDs.Remove(sImportLCDetailIDs.Length - 1, 1);
                }
                oImportLC.Amount = nAmount;
                oImportLCDetail = new ImportLCDetail();
                oImportLCDetail.ImportLCID = oImportLC.ImportLCID;
                oImportLCDetail.ImportLCLogID = oImportLC.ImportLCLogID;
                ImportLCDetailDA.DeleteLog(tc, oImportLCDetail, EnumDBOperation.Delete, nUserID, sImportLCDetailIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportLC;

        }
        public ImportLC RequestConfirm(ImportLC oImportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                reader = ImportLCDA.RequestConfirm(tc, oImportLC, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();
              
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportLC;
        }
        public String Delete(ImportLC oImportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportLCDA.Delete(tc, oImportLC, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }

        #region Update Only For LC Open
        public ImportLC UpdateForLCOpen(ImportLC oImportLC, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                //IDataReader reader = ImportLCDA.UpdateForLCOpen(tc,oImportLC, nUserID);
                reader = ImportLCDA.UpdateImportLC(tc, oImportLC, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLC = new ImportLC();
                oImportLC.ErrorMessage = e.Message;

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ImportLC", e);
                #endregion
            }

            return oImportLC;
        }
        public ImportLC UpdateImportLC_FileNo(ImportLC oImportLC, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader;
                //IDataReader reader = ImportLCDA.UpdateForLCOpen(tc,oImportLC, nUserID);
                reader = ImportLCDA.UpdateImportLC_FileNo(tc, oImportLC, EnumDBOperation.Update, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportLC.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get ImportLC", e);
                #endregion
            }

            return oImportLC;
        }
     
        #endregion

     

        #endregion

        #region Retrive Information

        public ImportLC Get(int id, Int64 nUserID)
        {
            ImportLC oImportLC = new ImportLC();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLC", e);
                #endregion
            }

            return oImportLC;
        }
        public ImportLC GetLog(int nImportLCID, Int64 nUserID)
        {
            ImportLC oImportLC = new ImportLC();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCDA.GetLog(tc, nImportLCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLC", e);
                #endregion
            }

            return oImportLC;
        }

        public List<ImportLC> GetsByStatus(string sLCCurrentStatus, int nBUID, Int64 nUserID)
        {
            List<ImportLC> oImportLCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDA.GetsByStatus(tc, sLCCurrentStatus, nBUID);
                oImportLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCs", e);
                #endregion
            }

            return oImportLCs;
        }
        public List<ImportLC> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportLC> oImportLCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDA.Gets(tc, sSQL);
                oImportLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCs", e);
                #endregion
            }

            return oImportLCs;
        }

        public ImportLC Save_UpdateStatus(ImportLC oImportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oImportLC.ImportLCID <= 0)
                {
                    reader = ImportLCDA.InsertUpdateLCStatus(tc, oImportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportLCDA.InsertUpdateLCStatus(tc, oImportLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLC = new ImportLC();
                    oImportLC = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportLC = new ImportLC();
                oImportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportLC;

        }
        #endregion

        #endregion
    }

}
