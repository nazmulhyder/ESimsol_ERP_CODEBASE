using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class ImportLCReqByExBillService : MarshalByRefObject, IImportLCReqByExBillService
    {
        #region Private functions and declaration

        private ImportLCReqByExBill MapObject(NullHandler oReader)
        {
            ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
            oImportLCReqByExBill.ImportLCReqByExBillID = oReader.GetInt32("ImportLCReqByExBillID");
            oImportLCReqByExBill.ImportLCID = oReader.GetInt32("ImportLCID");
            oImportLCReqByExBill.AmendmentNo = oReader.GetInt32("AmendmentNo");
            oImportLCReqByExBill.BankAccountID = oReader.GetInt32("BankAccountID");
            oImportLCReqByExBill.Amount = oReader.GetDouble("Amount ");
            oImportLCReqByExBill.MarginLien = oReader.GetDouble("MarginLien");
            oImportLCReqByExBill.MarginCash = oReader.GetDouble("MarginCash");
            oImportLCReqByExBill.MarginSCF = oReader.GetDouble("MarginSCF");
            oImportLCReqByExBill.MarginLienP = oReader.GetDouble("MarginLienP");
            oImportLCReqByExBill.MarginCashP = oReader.GetDouble("MarginCashP");
            oImportLCReqByExBill.MarginSCFP = oReader.GetDouble("MarginSCFP");
            oImportLCReqByExBill.Enclosed = oReader.GetString("Enclosed");
            oImportLCReqByExBill.Note = oReader.GetString("Note");
            oImportLCReqByExBill.IssueDate = oReader.GetDateTime("IssueDate");

            return oImportLCReqByExBill;
        }

        private ImportLCReqByExBill CreateObject(NullHandler oReader)
        {
            ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
            oImportLCReqByExBill = MapObject(oReader);
            return oImportLCReqByExBill;
        }

        private List<ImportLCReqByExBill> CreateObjects(IDataReader oReader)
        {
            List<ImportLCReqByExBill> oImportLCReqByExBill = new List<ImportLCReqByExBill>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCReqByExBill oItem = CreateObject(oHandler);
                oImportLCReqByExBill.Add(oItem);
            }
            return oImportLCReqByExBill;
        }

        #endregion

        #region Interface implementation
        public ImportLCReqByExBill Save(ImportLCReqByExBill oImportLCReqByExBill, Int64 nUserID)
        {
            ImportLCReqByExBillDetail oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
            ImportLCReqByExBill oUG = new ImportLCReqByExBill();
            oUG = oImportLCReqByExBill;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region ImportLCReqByExBill
                IDataReader reader;
                if (oImportLCReqByExBill.ImportLCReqByExBillID <= 0)
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ImportLCReqByExBill, EnumRoleOperationType.Add);
                    reader = ImportLCReqByExBillDA.InsertUpdate(tc, oImportLCReqByExBill, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    //AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ImportLCReqByExBill, EnumRoleOperationType.Edit);
                    reader = ImportLCReqByExBillDA.InsertUpdate(tc, oImportLCReqByExBill, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCReqByExBill = new ImportLCReqByExBill();
                    oImportLCReqByExBill = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region ImportLCReqByExBillDetail

                if (oImportLCReqByExBill.ImportLCReqByExBillID > 0)
                {
                    string sImportLCReqByExBillDetailIDs = "";
                    if (oUG.ImportLCReqByExBillDetails.Count > 0)
                    {
                        IDataReader readerdetail;
                        foreach (ImportLCReqByExBillDetail oDRD in oUG.ImportLCReqByExBillDetails)
                        {
                            oDRD.ImportLCReqByExBillID = oImportLCReqByExBill.ImportLCReqByExBillID;
                            if (oDRD.ImportLCReqByExBillDetailID <= 0)
                            {
                                readerdetail = ImportLCReqByExBillDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ImportLCReqByExBillDetailDA.InsertUpdate(tc, oDRD, EnumDBOperation.Update, nUserID, "");

                            }
                            NullHandler oReaderDevRecapdetail = new NullHandler(readerdetail);
                            int nImportLCReqByExBillDetailID = 0;
                            if (readerdetail.Read())
                            {
                                nImportLCReqByExBillDetailID = oReaderDevRecapdetail.GetInt32("ImportLCReqByExBillDetailID");
                                sImportLCReqByExBillDetailIDs = sImportLCReqByExBillDetailIDs + oReaderDevRecapdetail.GetString("ImportLCReqByExBillDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                    }
                    if (sImportLCReqByExBillDetailIDs.Length > 0)
                    {
                        sImportLCReqByExBillDetailIDs = sImportLCReqByExBillDetailIDs.Remove(sImportLCReqByExBillDetailIDs.Length - 1, 1);
                    }
                    oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
                    oImportLCReqByExBillDetail.ImportLCReqByExBillID = oImportLCReqByExBill.ImportLCReqByExBillID;
                    ImportLCReqByExBillDetailDA.Delete(tc, oImportLCReqByExBillDetail, EnumDBOperation.Delete, nUserID, sImportLCReqByExBillDetailIDs);
                }

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oImportLCReqByExBill = new ImportLCReqByExBill();
                    oImportLCReqByExBill.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oImportLCReqByExBill;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
                oImportLCReqByExBill.ImportLCReqByExBillID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ImportLCReqByExBill, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ImportLCReqByExBill", id);
                ImportLCReqByExBillDA.Delete(tc, oImportLCReqByExBill, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ImportLCReqByExBill Get(int id, Int64 nUserId)
        {
            ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportLCReqByExBillDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCReqByExBill = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLCReqByExBill", e);
                #endregion
            }
            return oImportLCReqByExBill;
        }

        public ImportLCReqByExBill GetByLC(int id, Int64 nUserId)
        {
            ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportLCReqByExBillDA.GetByLC(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCReqByExBill = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLCReqByExBill", e);
                #endregion
            }
            return oImportLCReqByExBill;
        }

        public List<ImportLCReqByExBill> Gets(Int64 nUserID)
        {
            List<ImportLCReqByExBill> oImportLCReqByExBills = new List<ImportLCReqByExBill>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportLCReqByExBillDA.Gets(tc);
                oImportLCReqByExBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ImportLCReqByExBill oImportLCReqByExBill = new ImportLCReqByExBill();
                oImportLCReqByExBill.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oImportLCReqByExBills;
        }

        public List<ImportLCReqByExBill> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportLCReqByExBill> oImportLCReqByExBills = new List<ImportLCReqByExBill>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportLCReqByExBillDA.Gets(tc, sSQL);
                oImportLCReqByExBills = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCReqByExBill", e);
                #endregion
            }
            return oImportLCReqByExBills;
        }

        #endregion
    }

}
