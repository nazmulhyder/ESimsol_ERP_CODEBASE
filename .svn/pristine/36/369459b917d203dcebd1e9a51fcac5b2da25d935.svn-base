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
    [Serializable]
    public class ExportLCAmendmentClauseService : MarshalByRefObject, IExportLCAmendmentClauseService
    {
        #region Private functions and declaration
        private ExportLCAmendmentClause MapObject(NullHandler oReader)
        {
            ExportLCAmendmentClause oExportLCAmendmentClause = new ExportLCAmendmentClause();
            oExportLCAmendmentClause.ExportLCAmendmentClauseID = oReader.GetInt32("ExportLCAmendmentClauseID");
            oExportLCAmendmentClause.ExportLCAmendRequestID = oReader.GetInt32("ExportLCAmendRequestID");
            oExportLCAmendmentClause.Clause = oReader.GetString("Clause");
            return oExportLCAmendmentClause;
        }

        private ExportLCAmendmentClause CreateObject(NullHandler oReader)
        {
            ExportLCAmendmentClause oExportLCAmendmentClause = new ExportLCAmendmentClause();
            oExportLCAmendmentClause=MapObject(oReader);
            return oExportLCAmendmentClause;
        }

        private List<ExportLCAmendmentClause> CreateObjects(IDataReader oReader)
        {
            List<ExportLCAmendmentClause> oExportLCAmendmentClauses = new List<ExportLCAmendmentClause>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLCAmendmentClause oItem = CreateObject(oHandler);
                oExportLCAmendmentClauses.Add(oItem);
            }
            return oExportLCAmendmentClauses;
        }
        #endregion

        #region Interface implementation
        public ExportLCAmendmentClauseService() { }



        public List<ExportLCAmendmentClause> Gets(int nExportLCAmendmentClauseID, Int64 nUserId)
        {
            List<ExportLCAmendmentClause> oExportLCAmendmentClauses = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCAmendmentClauseDA.Gets(tc, nExportLCAmendmentClauseID);
                oExportLCAmendmentClauses = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLCAmendmentClauses", e);
                #endregion
            }

            return oExportLCAmendmentClauses;
        }

        #endregion
    }


}
