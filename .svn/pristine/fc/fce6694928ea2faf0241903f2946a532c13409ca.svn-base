using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 
namespace ESimSol.Services.Services
{
    public class ExportSummaryService : MarshalByRefObject, IExportSummaryService
    {      
        public ExportSummary GetsExportSummary(ExportSummary oExportSummary, Int64 nUserID)
        {
            TransactionContext tc = null;
            ExportSummary oTempExportSummary = new ExportSummary();
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportSummaryDA.GetsExportSummary(tc, oExportSummary);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[3]);
                oTempExportSummary.DataCarrier = oDataSet;
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempExportSummary = new ExportSummary();
                oTempExportSummary.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oTempExportSummary;
        }
    }
}
