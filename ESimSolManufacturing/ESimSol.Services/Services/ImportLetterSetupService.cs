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
    [Serializable]
    public class ImportLetterSetupService : MarshalByRefObject, IImportLetterSetupService
    {
        #region Private functions and declaration
        private ImportLetterSetup MapObject(NullHandler oReader)
        {
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            oImportLetterSetup.ImportLetterSetupID = oReader.GetInt32("ImportLetterSetupID");
            //oImportLetterSetup.CompanyID = oReader.GetInt32("CompanyID");
            //oImportLetterSetup.DocumentType = (EnumDocumentType)oReader.GetInt32("DocumentType");
            oImportLetterSetup.LetterTypeInt = oReader.GetInt32("LetterType");
            oImportLetterSetup.LetterType = (EnumImportLetterType)oReader.GetInt32("LetterType");
            oImportLetterSetup.IssueToType = oReader.GetInt32("IssueToType");

            oImportLetterSetup.BankBranchID = oReader.GetInt32("BankBranchID");
            oImportLetterSetup.HeaderType = oReader.GetInt32("HeaderType");

            oImportLetterSetup.LCAppType = (EnumLCAppType)oReader.GetInt32("LCAppType");
            oImportLetterSetup.LCAppTypeInt = oReader.GetInt32("LCAppType");
            oImportLetterSetup.LCPaymentType = (EnumLCPaymentType)oReader.GetInt32("LCPaymentType");
            oImportLetterSetup.LCPaymentTypeInt = oReader.GetInt32("LCPaymentType");
            oImportLetterSetup.ProductType = oReader.GetInt32("ProductType");
            oImportLetterSetup.IssueToType = oReader.GetInt32("IssueToType");
          
            oImportLetterSetup.LetterName = oReader.GetString("LetterName");
            oImportLetterSetup.RefNo = oReader.GetString("RefNo");
            oImportLetterSetup.To = oReader.GetString("To");

            oImportLetterSetup.ToName = oReader.GetString("ToName");
            oImportLetterSetup.Subject = oReader.GetString("Subject");
            oImportLetterSetup.SubjectTwo = oReader.GetString("SubjectTwo");
            oImportLetterSetup.DearSir = oReader.GetString("DearSir");
            oImportLetterSetup.Body1 = oReader.GetString("Body1");
            oImportLetterSetup.Body2 = oReader.GetString("Body2");
            oImportLetterSetup.Body3 = oReader.GetString("Body3");
            oImportLetterSetup.ThankingOne = oReader.GetString("ThankingOne");
            oImportLetterSetup.ThankingTwo = oReader.GetString("ThankingTwo");
            oImportLetterSetup.Authorize1 = oReader.GetString("Authorize1");
            oImportLetterSetup.Authorize2 = oReader.GetString("Authorize2");
            oImportLetterSetup.Authorize3 = oReader.GetString("Authorize3");

            oImportLetterSetup.IsPrintAddress = oReader.GetBoolean("IsPrintAddress");
            oImportLetterSetup.IsPrintDateCurrentDate = oReader.GetBoolean("IsPrintDateCurrentDate");
            oImportLetterSetup.IsPrintDateObject = oReader.GetBoolean("IsPrintDateObject");
            oImportLetterSetup.IsAutoRefNo = oReader.GetBoolean("IsAutoRefNo");
            oImportLetterSetup.Authorize1IsAuto = oReader.GetBoolean("Authorize1IsAuto");
            oImportLetterSetup.Authorize2IsAuto = oReader.GetBoolean("Authorize2IsAuto");
            oImportLetterSetup.Authorize3IsAuto = oReader.GetBoolean("Authorize3IsAuto");
            oImportLetterSetup.Activity = oReader.GetBoolean("Activity");

            oImportLetterSetup.BUName = oReader.GetString("BUName");
            oImportLetterSetup.BUID = oReader.GetInt32("BUID");
            oImportLetterSetup.For = oReader.GetString("For");
            oImportLetterSetup.ForName = oReader.GetString("ForName");
            oImportLetterSetup.To = oReader.GetString("To");

            oImportLetterSetup.SupplierName = oReader.GetString("SupplierName");
            oImportLetterSetup.PIBank = oReader.GetString("PIBank");
            oImportLetterSetup.LCNo = oReader.GetString("LCNo");
            oImportLetterSetup.LCValue = oReader.GetString("LCValue");
            oImportLetterSetup.InvoiceNo = oReader.GetString("InvoiceNo");
            oImportLetterSetup.InvoiceValue = oReader.GetString("InvoiceValue");
            oImportLetterSetup.Clause = oReader.GetString("Clause");
                        
            oImportLetterSetup.IsPrintProductName = oReader.GetBoolean("IsPrintProductName");
            oImportLetterSetup.IsPrintPINo = oReader.GetBoolean("IsPrintPINo");
            oImportLetterSetup.IsPrinTnC = oReader.GetBoolean("IsPrinTnC");
            oImportLetterSetup.IsPrintPIBankAddress = oReader.GetBoolean("IsPrintPIBankAddress");
            oImportLetterSetup.IsPrintMaturityDate = oReader.GetBoolean("IsPrintMaturityDate");
            oImportLetterSetup.IsPrintSupplierAddress = oReader.GetBoolean("IsPrintSupplierAddress");
            oImportLetterSetup.MasterLCNo = oReader.GetString("MasterLCNo");
            oImportLetterSetup.BLNo = oReader.GetString("BLNo");
            oImportLetterSetup.LCPayType = oReader.GetString("LCPayType");
            oImportLetterSetup.IsCalMaturityDate = oReader.GetBoolean("IsCalMaturityDate");
            
           

            return oImportLetterSetup;
        }

        private ImportLetterSetup CreateObject(NullHandler oReader)
        {
            ImportLetterSetup oImportLetterSetup = new ImportLetterSetup();
            oImportLetterSetup=MapObject(oReader);
            return oImportLetterSetup;
        }

        private List<ImportLetterSetup> CreateObjects(IDataReader oReader)
        {
            List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLetterSetup oItem = CreateObject(oHandler);
                oImportLetterSetups.Add(oItem);
            }
            return oImportLetterSetups;
        }
        #endregion

        #region Interface implementation
        public ImportLetterSetupService() { }

        public ImportLetterSetup Get(int nID, Int64 nUserID)
        {
            ImportLetterSetup oPLC = new ImportLetterSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        public ImportLetterSetup Get(int nLetterType, int nIssueToType, int nImportLCID, string sSQL, Int64 nUserID)
        {
            ImportLetterSetup oPLC = new ImportLetterSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.Get(tc, nLetterType, nIssueToType, nImportLCID, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        public ImportLetterSetup Get(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL,Int64 nUserID)
        {
            ImportLetterSetup oPLC = new ImportLetterSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.Get(tc, nLetterType, nIssueToType, nBUID, nImportLCID, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        public ImportLetterSetup GetBy(int nLetterType, int nIssueToType, int nBUID, int nImportLCID, string sSQL, Int64 nUserID)
        {
            ImportLetterSetup oPLC = new ImportLetterSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.GetBy(tc, nLetterType, nIssueToType, nBUID, nImportLCID, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        public ImportLetterSetup GetForIPR(int nLetterType, int nIssueToType, int nBUID, int nIPRID, string sSQL, Int64 nUserID)
        {
            ImportLetterSetup oPLC = new ImportLetterSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.GetForIPR(tc, nLetterType, nIssueToType, nBUID, nIPRID, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseLC", e);
                #endregion
            }

            return oPLC;
        }
        
        public ImportLetterSetup Save(ImportLetterSetup oImportLetterSetup, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);


                IDataReader reader;
                if (oImportLetterSetup.ImportLetterSetupID <= 0)
                {
                    reader = ImportLetterSetupDA.InsertUpdate(tc, oImportLetterSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportLetterSetupDA.InsertUpdate(tc, oImportLetterSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLetterSetup = new ImportLetterSetup();
                    oImportLetterSetup = CreateObject(oReader);
                }
                reader.Close();

                


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportLetterSetup = new ImportLetterSetup();
                oImportLetterSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportLetterSetup;

        }
        public ImportLetterSetup Activate(ImportLetterSetup oImportLetterSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLetterSetupDA.Activate(tc, oImportLetterSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLetterSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLetterSetup = new ImportLetterSetup();
                oImportLetterSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportLetterSetup;
        }
        public List<ImportLetterSetup> UpdateSequence(ImportLetterSetup oImportLetterSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ImportLetterSetup> oImportLetterSetups = new List<ImportLetterSetup>();
            oImportLetterSetups = oImportLetterSetup.ImportLetterSetups;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (ImportLetterSetup oItem in oImportLetterSetups)
                {
                    ImportLetterSetupDA.UpdateSequence(tc, oItem);
                }
                IDataReader reader = null;
                reader = ImportLetterSetupDA.Gets(tc);
                oImportLetterSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportLetterSetups = new List<ImportLetterSetup>();
                oImportLetterSetup = new ImportLetterSetup();
                oImportLetterSetup.ErrorMessage = e.Message;
                oImportLetterSetups.Add(oImportLetterSetup);
                #endregion
            }
            return oImportLetterSetups;
        }
        public List<ImportLetterSetup> Gets(bool bActivity, int nBUID, Int64 nUserId)
        {
            List<ImportLetterSetup> oImportLetterSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLetterSetupDA.Gets(tc, bActivity, nBUID);
                oImportLetterSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLetterSetups", e);
                #endregion
            }

            return oImportLetterSetups;
        }

        public List<ImportLetterSetup> BUWiseGets( int nBUID, Int64 nUserId)
        {
            List<ImportLetterSetup> oImportLetterSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLetterSetupDA.BUWiseGets(tc, nBUID);
                oImportLetterSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLetterSetups", e);
                #endregion
            }

            return oImportLetterSetups;
        }
        public List<ImportLetterSetup> Gets( Int64 nUserId)
        {
            List<ImportLetterSetup> oImportLetterSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLetterSetupDA.Gets(tc);
                oImportLetterSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLetterSetups", e);
                #endregion
            }

            return oImportLetterSetups;
        }

        public List<ImportLetterSetup> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportLetterSetup> oImportLetterSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLetterSetupDA.Gets(sSQL,tc);
                oImportLetterSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLetterSetups", e);
                #endregion
            }

            return oImportLetterSetups;
        }

        public string Delete(ImportLetterSetup oImportLetterSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);


                ImportLetterSetupDA.Delete(tc, oImportLetterSetup, EnumDBOperation.Delete, nUserId);
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

        #endregion
    }


}
