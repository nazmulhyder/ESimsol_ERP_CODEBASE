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
    public class ExportUDDetailService : MarshalByRefObject, IExportUDDetailService
    {
        #region Private functions and declaration

        private ExportUDDetail MapObject(NullHandler oReader)
        {
            ExportUDDetail oExportUDDetail = new ExportUDDetail();
            oExportUDDetail.ExportUDDetailID = oReader.GetInt32("ExportUDDetailID");
            oExportUDDetail.ExportUDID = oReader.GetInt32("ExportUDID");
            oExportUDDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportUDDetail.LCDate = oReader.GetDateTime("LCDate");
            oExportUDDetail.LCReceiveDate = oReader.GetDateTime("LCReceiveDate");
            oExportUDDetail.IssueDate = oReader.GetDateTime("IssueDate");
            oExportUDDetail.Amount = oReader.GetDouble("Amount");
            oExportUDDetail.Qty = oReader.GetDouble("Qty");
            oExportUDDetail.ANo = oReader.GetInt32("ANo");
            oExportUDDetail.PINo = oReader.GetString("PINo");

            return oExportUDDetail;
        }

        private ExportUDDetail CreateObject(NullHandler oReader)
        {
            ExportUDDetail oExportUDDetail = new ExportUDDetail();
            oExportUDDetail = MapObject(oReader);
            return oExportUDDetail;
        }

        private List<ExportUDDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportUDDetail> oExportUDDetail = new List<ExportUDDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportUDDetail oItem = CreateObject(oHandler);
                oExportUDDetail.Add(oItem);
            }
            return oExportUDDetail;
        }

        #endregion

        #region Interface implementation


        public ExportUDDetail Get(int id, Int64 nUserId)
        {
            ExportUDDetail oExportUDDetail = new ExportUDDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportUDDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportUDDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportUDDetail", e);
                #endregion
            }
            return oExportUDDetail;
        }

        public List<ExportUDDetail> Gets(int nExportUDID, Int64 nUserID)
        {
            List<ExportUDDetail> oExportUDDetails = new List<ExportUDDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUDDetailDA.Gets(tc, nExportUDID);
                oExportUDDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExportUDDetail oExportUDDetail = new ExportUDDetail();
                oExportUDDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportUDDetails;
        }

        public List<ExportUDDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportUDDetail> oExportUDDetails = new List<ExportUDDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportUDDetailDA.Gets(tc, sSQL);
                oExportUDDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) 
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportUDDetail", e);
                #endregion
            }
            return oExportUDDetails;
        }

        #endregion
    }

}
