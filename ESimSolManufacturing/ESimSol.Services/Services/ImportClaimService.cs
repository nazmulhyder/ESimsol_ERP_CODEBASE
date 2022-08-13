using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ImportClaimService : MarshalByRefObject, IImportClaimService
    {
        #region Private functions and declaration
        private ImportClaim MapObject(NullHandler oReader)
        {
            ImportClaim oImportClaim = new ImportClaim();
            oImportClaim.ImportClaimID = oReader.GetInt32("ImportClaimID");
            oImportClaim.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportClaim.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            oImportClaim.ImportInvoiceQty = oReader.GetDouble("ImportInvoiceQty");
            oImportClaim.ImportInvoiceAmount = oReader.GetDouble("ImportInvoiceAmount");
            oImportClaim.Amount = oReader.GetDouble("Amount");
            oImportClaim.ImportInvoiceDate = oReader.GetDateTime("ImportInvoiceDate");
            oImportClaim.IssueDate = oReader.GetDateTime("IssueDate");
            oImportClaim.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportClaim.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportClaim.ImportLCDate = oReader.GetDateTime("ImportLCDate");
            oImportClaim.ContractorName = oReader.GetString("ContractorName");
            oImportClaim.Currency = oReader.GetString("Currency");
            oImportClaim.ClaimNo = oReader.GetString("ClaimNo");
            oImportClaim.ClaimReasonID = oReader.GetInt32("ClaimReasonID");
            oImportClaim.Note = oReader.GetString("Note");
            oImportClaim.SettleBy = oReader.GetInt32("SettleBy");
            oImportClaim.RequestBy = oReader.GetInt32("RequestBy");
            oImportClaim.RequestDate = oReader.GetDateTime("RequestDate");
            oImportClaim.ApproveBy = oReader.GetInt32("ApproveBy");
            oImportClaim.ApproveDate = oReader.GetDateTime("ApproveDate");
            return oImportClaim;
        }

        private ImportClaim CreateObject(NullHandler oReader)
        {
            ImportClaim oImportClaim = new ImportClaim();
            oImportClaim = MapObject(oReader);
            return oImportClaim;
        }

        private List<ImportClaim> CreateObjects(IDataReader oReader)
        {
            List<ImportClaim> oImportClaims = new List<ImportClaim>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportClaim oItem = CreateObject(oHandler);
                oImportClaims.Add(oItem);
            }
            return oImportClaims;
        }

        #endregion

        #region Interface implementation
        public ImportClaimService() { }

        public ImportClaim Save(ImportClaim oImportClaim, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportClaimDetail> oImportClaimDetails = new List<ImportClaimDetail>();
                oImportClaimDetails = oImportClaim.ImportClaimDetails;
                string sImportClaimDetailIDS = "";
              
                IDataReader reader;
                if (oImportClaim.ImportClaimID <= 0)
                {
                    reader = ImportClaimDA.InsertUpdate(tc, oImportClaim, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportClaimDA.InsertUpdate(tc, oImportClaim, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaim = CreateObject(oReader);
                }
                reader.Close();

                #region ImportClaim Part

                foreach (ImportClaimDetail oItem in oImportClaimDetails)
                {
                    IDataReader readerdetail;
                    oItem.ImportClaimID = oImportClaim.ImportClaimID;
                    if (oItem.ImportClaimDetailID <= 0)
                    {
                        readerdetail = ImportClaimDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = ImportClaimDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sImportClaimDetailIDS = sImportClaimDetailIDS + oReaderDetail.GetString("ImportClaimDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                ImportClaimDetail oImportClaimDetail = new ImportClaimDetail();
                oImportClaimDetail.ImportClaimID = oImportClaim.ImportClaimID;
                if (sImportClaimDetailIDS.Length > 0)
                {
                    sImportClaimDetailIDS = sImportClaimDetailIDS.Remove(sImportClaimDetailIDS.Length - 1, 1);
                }
                ImportClaimDetailDA.Delete(tc, oImportClaimDetail, EnumDBOperation.Delete, nUserID, sImportClaimDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oImportClaim.ErrorMessage = Message;
                #endregion
            }

            return oImportClaim;

        }
        
        public String Delete(ImportClaim oImportClaim, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportClaimDA.Delete(tc, oImportClaim, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                return Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ImportClaim Get(int id, Int64 nUserId)
        {
            ImportClaim oImportClaim = new ImportClaim();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportClaimDA.Get(id,tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaim = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportClaim;
        }
        public List<ImportClaim> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportClaim> oImportClaim = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportClaimDA.Gets(tc,sSQL);
                oImportClaim = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportClaim", e);
                #endregion
            }
            return oImportClaim;
        }
        public List<ImportClaim> Gets(Int64 nUserId)
        {
            List<ImportClaim> oImportClaims = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportClaimDA.Gets(tc);
                oImportClaims = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oImportClaims;
        }

        public ImportClaim Request(ImportClaim oImportClaim, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportClaimDA.Request(tc, oImportClaim, EnumDBOperation.Request, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaim = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportClaim = new ImportClaim();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oImportClaim.ErrorMessage = Message;
                #endregion
            }
            return oImportClaim;
        }

        public ImportClaim Approve(ImportClaim oImportClaim, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportClaimDA.Approve(tc, oImportClaim, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaim = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportClaim = new ImportClaim();
                oImportClaim.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportClaim;
        }
        #endregion
    }
}