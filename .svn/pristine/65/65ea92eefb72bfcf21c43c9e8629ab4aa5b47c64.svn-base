using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;

namespace ESimSol.Services.Services.ReportingService
{
    public class RptProductionMonthlyInspectionService : MarshalByRefObject, IRptProductionMonthlyInspectionService
    {
        #region Private functions and declaration
        private RptProductionMonthlyInspection MapObject(NullHandler oReader)
        {
            RptProductionMonthlyInspection oExecutionOrderUpdateStatus = new RptProductionMonthlyInspection();
            oExecutionOrderUpdateStatus.LockDate = oReader.GetDateTime("LockDate");
            oExecutionOrderUpdateStatus.AGrade = oReader.GetDouble("AGrade");
            oExecutionOrderUpdateStatus.BGrade = oReader.GetDouble("BGrade");
            oExecutionOrderUpdateStatus.RCShift = oReader.GetDouble("RCShift");
            oExecutionOrderUpdateStatus.TotalReject = oReader.GetDouble("TotalReject");
            oExecutionOrderUpdateStatus.RAShift = oReader.GetDouble("RAShift");
            oExecutionOrderUpdateStatus.RBShift = oReader.GetDouble("RBShift");
            oExecutionOrderUpdateStatus.YarnFault = oReader.GetDouble("YarnFault");
            oExecutionOrderUpdateStatus.YDFault = oReader.GetDouble("YDFault");
            oExecutionOrderUpdateStatus.Weaving = oReader.GetDouble("Weaving");
            oExecutionOrderUpdateStatus.Remarks = oReader.GetString("Remarks");
            return oExecutionOrderUpdateStatus;
        }
        private RptProductionMonthlyInspection CreateObject(NullHandler oReader)
        {
            RptProductionMonthlyInspection oExecutionOrderUpdateStatus = new RptProductionMonthlyInspection();
            oExecutionOrderUpdateStatus = MapObject(oReader);
            return oExecutionOrderUpdateStatus;
        }
        private List<RptProductionMonthlyInspection> CreateObjects(IDataReader oReader)
        {
            List<RptProductionMonthlyInspection> oExecutionOrderUpdateStatus = new List<RptProductionMonthlyInspection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptProductionMonthlyInspection oItem = CreateObject(oHandler);
                oExecutionOrderUpdateStatus.Add(oItem);
            }
            return oExecutionOrderUpdateStatus;
        }

        #endregion

        #region Interface implementation
        public List<RptProductionMonthlyInspection> Gets(DateTime dFromDate, DateTime dToDate, string sFEOIDs, string sBuyerIDs, string  sFMIDs, long nUserID)
        {
            List<RptProductionMonthlyInspection> oExecutionOrderUpdateStatuss = new List<RptProductionMonthlyInspection>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptProductionMonthlyInspectionDA.Gets(tc, dFromDate, dToDate, sFEOIDs, sBuyerIDs, sFMIDs);
                oExecutionOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExecutionOrderUpdateStatuss = new List<RptProductionMonthlyInspection>();
                RptProductionMonthlyInspection oExecutionOrderUpdateStatus = new RptProductionMonthlyInspection();
                oExecutionOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oExecutionOrderUpdateStatuss.Add(oExecutionOrderUpdateStatus);
                #endregion
            }
            return oExecutionOrderUpdateStatuss;
        }
        public List<RptProductionMonthlyInspection> Gets(string sSQL, Int64 nUserID)
        {
            List<RptProductionMonthlyInspection> oExecutionOrderUpdateStatuss = new List<RptProductionMonthlyInspection>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptProductionMonthlyInspectionDA.Gets(tc, sSQL);
                oExecutionOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExecutionOrderUpdateStatuss = new List<RptProductionMonthlyInspection>();
                RptProductionMonthlyInspection oExecutionOrderUpdateStatus = new RptProductionMonthlyInspection();
                oExecutionOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oExecutionOrderUpdateStatuss.Add(oExecutionOrderUpdateStatus);
                #endregion
            }
            return oExecutionOrderUpdateStatuss;
        }
        #endregion
    }
}
