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
    public class ImportLCReqByExBillDetailService : MarshalByRefObject, IImportLCReqByExBillDetailService
    {
        #region Private functions and declaration

        private ImportLCReqByExBillDetail MapObject(NullHandler oReader)
        {
            ImportLCReqByExBillDetail oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
            oImportLCReqByExBillDetail.ImportLCReqByExBillDetailID = oReader.GetInt32("ImportLCReqByExBillDetailID");
            oImportLCReqByExBillDetail.ImportLCReqByExBillID = oReader.GetInt32("ImportLCReqByExBillID");
            oImportLCReqByExBillDetail.ExportBillID = oReader.GetInt32("ExportBillID");
            oImportLCReqByExBillDetail.ExportLCID = oReader.GetInt32("ExportLCID");
            oImportLCReqByExBillDetail.ExportLCNo = oReader.GetString("ExportLCNo");
            oImportLCReqByExBillDetail.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oImportLCReqByExBillDetail.LDBCNo = oReader.GetString("LDBCNo");
            oImportLCReqByExBillDetail.Amount = oReader.GetDouble("Amount");
            oImportLCReqByExBillDetail.MaturityDate = oReader.GetDateTime("MaturityDate");

            return oImportLCReqByExBillDetail;
        }

        private ImportLCReqByExBillDetail CreateObject(NullHandler oReader)
        {
            ImportLCReqByExBillDetail oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
            oImportLCReqByExBillDetail = MapObject(oReader);
            return oImportLCReqByExBillDetail;
        }

        private List<ImportLCReqByExBillDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportLCReqByExBillDetail> oImportLCReqByExBillDetail = new List<ImportLCReqByExBillDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCReqByExBillDetail oItem = CreateObject(oHandler);
                oImportLCReqByExBillDetail.Add(oItem);
            }
            return oImportLCReqByExBillDetail;
        }

        #endregion

        #region Interface implementation


        public ImportLCReqByExBillDetail Get(int id, Int64 nUserId)
        {
            ImportLCReqByExBillDetail oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ImportLCReqByExBillDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportLCReqByExBillDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportLCReqByExBillDetail", e);
                #endregion
            }
            return oImportLCReqByExBillDetail;
        }

        public List<ImportLCReqByExBillDetail> Gets(int nImportLCReqByExBillID, Int64 nUserID)
        {
            List<ImportLCReqByExBillDetail> oImportLCReqByExBillDetails = new List<ImportLCReqByExBillDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportLCReqByExBillDetailDA.Gets(tc, nImportLCReqByExBillID);
                oImportLCReqByExBillDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ImportLCReqByExBillDetail oImportLCReqByExBillDetail = new ImportLCReqByExBillDetail();
                oImportLCReqByExBillDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oImportLCReqByExBillDetails;
        }

        public List<ImportLCReqByExBillDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportLCReqByExBillDetail> oImportLCReqByExBillDetails = new List<ImportLCReqByExBillDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ImportLCReqByExBillDetailDA.Gets(tc, sSQL);
                oImportLCReqByExBillDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportLCReqByExBillDetail", e);
                #endregion
            }
            return oImportLCReqByExBillDetails;
        }

        #endregion
    }

}
