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
    public class ImportClaimDetailService : MarshalByRefObject, IImportClaimDetailService
    {
        #region Private functions and declaration
        private ImportClaimDetail MapObject(NullHandler oReader)
        {
            ImportClaimDetail oImportClaimDetail = new ImportClaimDetail();
            oImportClaimDetail.ImportClaimDetailID = oReader.GetInt32("ImportClaimDetailID");
            oImportClaimDetail.ProductID = oReader.GetInt32("ProductID");
            oImportClaimDetail.ProductName = oReader.GetString("ProductName");
            oImportClaimDetail.MUnit = oReader.GetString("MUnit");
            oImportClaimDetail.Qty = oReader.GetDouble("Qty");
            oImportClaimDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportClaimDetail.Note = oReader.GetString("Note");
            return oImportClaimDetail;
        }

        private ImportClaimDetail CreateObject(NullHandler oReader)
        {
            ImportClaimDetail oImportClaimDetail = new ImportClaimDetail();
            oImportClaimDetail = MapObject(oReader);
            return oImportClaimDetail;
        }

        private List<ImportClaimDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportClaimDetail> oImportClaimDetails = new List<ImportClaimDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportClaimDetail oItem = CreateObject(oHandler);
                oImportClaimDetails.Add(oItem);
            }
            return oImportClaimDetails;
        }

        #endregion

        #region Interface implementation
        public ImportClaimDetailService() { }


        public ImportClaimDetail Save(ImportClaimDetail oImportClaimDetail, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region ImportClaimDetail
                IDataReader reader;
                if (oImportClaimDetail.ImportClaimDetailID <= 0)
                {
                    reader = ImportClaimDetailDA.InsertUpdate(tc, oImportClaimDetail, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ImportClaimDetailDA.InsertUpdate(tc, oImportClaimDetail, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaimDetail = new ImportClaimDetail();
                    oImportClaimDetail = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportClaimDetail = new ImportClaimDetail();
                oImportClaimDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportClaimDetail;
        }

        public String Delete(ImportClaimDetail oImportClaimDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportClaimDetailDA.Delete(tc, oImportClaimDetail, EnumDBOperation.Delete, nUserID);
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
        public ImportClaimDetail Get(int id, Int64 nUserId)
        {
            ImportClaimDetail oImportClaimDetail = new ImportClaimDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportClaimDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportClaimDetail = CreateObject(oReader);
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

            return oImportClaimDetail;
        }

        public List<ImportClaimDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportClaimDetail> oImportClaimDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportClaimDetailDA.Gets(sSQL, tc);
                oImportClaimDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportClaimDetail", e);
                #endregion
            }
            return oImportClaimDetail;
        }
        
        public List<ImportClaimDetail> Gets(Int64 nUserId)
        {
            List<ImportClaimDetail> oImportClaimDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportClaimDetailDA.Gets(tc);
                oImportClaimDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Claim Details", e);
                #endregion
            }

            return oImportClaimDetails;
        }

        public List<ImportClaimDetail> Gets(int nImportInvoiceID, Int64 nUserId)
        {
            List<ImportClaimDetail> oImportClaimDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportClaimDetailDA.Gets(nImportInvoiceID, tc);
                oImportClaimDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import Invoice Products", e);
                #endregion
            }
            return oImportClaimDetails;
        }

        #endregion
    }
}