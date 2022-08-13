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
    public class DUHardWindingReportService : MarshalByRefObject, IDUHardWindingReportService
    {
        #region Private functions and declaration

        private DUHardWindingReport MapObject(NullHandler oReader)
        {
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            oDUHardWindingReport.StartDate = oReader.GetDateTime("StartDate");
            oDUHardWindingReport.EndDate = oReader.GetDateTime("EndDate");
            oDUHardWindingReport.QtyHWIn = oReader.GetDouble("QtyHWIn");
            oDUHardWindingReport.QtyHWOut = oReader.GetDouble("QtyHWOut");
            oDUHardWindingReport.QtyReHWIn = oReader.GetDouble("QtyReHWIn");
            oDUHardWindingReport.QtyReHWOut = oReader.GetDouble("QtyReHWOut");
            oDUHardWindingReport.QtyGreige = oReader.GetDouble("QtyGreige");
            oDUHardWindingReport.Qty_LO = oReader.GetDouble("Qty_LO");
            oDUHardWindingReport.BeamCom = oReader.GetDouble("BeamCom");
            oDUHardWindingReport.BeamTF = oReader.GetDouble("BeamTF");
            oDUHardWindingReport.Warping = oReader.GetDouble("Warping");
            oDUHardWindingReport.BeamStock = oReader.GetDouble("BeamStock");

            return oDUHardWindingReport;
        }

        private DUHardWindingReport CreateObject(NullHandler oReader)
        {
            DUHardWindingReport oDUHardWindingReport = new DUHardWindingReport();
            oDUHardWindingReport = MapObject(oReader);
            return oDUHardWindingReport;
        }

        private List<DUHardWindingReport> CreateObjects(IDataReader oReader)
        {
            List<DUHardWindingReport> oDUHardWindingReport = new List<DUHardWindingReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUHardWindingReport oItem = CreateObject(oHandler);
                oDUHardWindingReport.Add(oItem);
            }
            return oDUHardWindingReport;
        }

        #endregion

        #region Interface implementation

        public List<DUHardWindingReport> Gets(DUHardWindingReport oDUHardWindingReport, Int64 nUserID)
        {
            List<DUHardWindingReport> oDUHardWindingReports = new List<DUHardWindingReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUHardWindingReportDA.Gets(tc, oDUHardWindingReport);
                oDUHardWindingReports = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUHardWindingReport", e);
                #endregion
            }
            return oDUHardWindingReports;
        }

        #endregion
    }

}
