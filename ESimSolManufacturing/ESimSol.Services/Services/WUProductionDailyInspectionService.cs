using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
using ESimSol.BusinessObjects;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class WUProductionDailyInspectionService : MarshalByRefObject, IWUProductionDailyInspectionService
    {
        #region Private functions and declaration
        private WUProductionDailyInspection MapObject(NullHandler oReader)
        {
            WUProductionDailyInspection oWUProductionDailyInspection = new WUProductionDailyInspection();
            oWUProductionDailyInspection.FEOID = oReader.GetInt32("FEOID");
            oWUProductionDailyInspection.FEONo = oReader.GetString("FEONo");
            oWUProductionDailyInspection.BuyerID = oReader.GetInt32("BuyerID");
            oWUProductionDailyInspection.BuyerName = oReader.GetString("BuyerName");
            oWUProductionDailyInspection.Construction = oReader.GetString("Construction");
            oWUProductionDailyInspection.ProcessType = oReader.GetString("ProcessType");
            oWUProductionDailyInspection.GreyWidth = oReader.GetString("GreyWidth");
            oWUProductionDailyInspection.GradeA = Global.GetMeter(oReader.GetDouble("GradeA"),2);
            oWUProductionDailyInspection.GradeB = Global.GetMeter(oReader.GetDouble("GradeB"), 2);
            oWUProductionDailyInspection.Reject = Global.GetMeter(oReader.GetDouble("Reject"), 2);
            oWUProductionDailyInspection.Remarks = oReader.GetString("Remarks");
            oWUProductionDailyInspection.ReedCount = oReader.GetString("ReedCount");
            oWUProductionDailyInspection.TSUName = oReader.GetString("TSUName");
            oWUProductionDailyInspection.TSUID = oReader.GetInt32("TSUID");
            oWUProductionDailyInspection.ProductionDate = oReader.GetDateTime("ProductionDate");
            oWUProductionDailyInspection.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oWUProductionDailyInspection.IsInHouse = oReader.GetBoolean("IsInHouse");
           
            oWUProductionDailyInspection.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");

            return oWUProductionDailyInspection;
        }

        private WUProductionDailyInspection CreateObject(NullHandler oReader)
        {
            WUProductionDailyInspection oWUProductionDailyInspection = new WUProductionDailyInspection();
            oWUProductionDailyInspection = MapObject(oReader);
            return oWUProductionDailyInspection;
        }

        private List<WUProductionDailyInspection> CreateObjects(IDataReader oReader)
        {
            List<WUProductionDailyInspection> oWUProductionDailyInspections = new List<WUProductionDailyInspection>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WUProductionDailyInspection oItem = CreateObject(oHandler);
                oWUProductionDailyInspections.Add(oItem);
            }
            return oWUProductionDailyInspections;
        }
        #endregion

        #region Interface implementation
        public WUProductionDailyInspectionService() { }

        public List<WUProductionDailyInspection> Gets(DateTime dtFrom, string sFEOID, string sBuyerID, string sFMID, DateTime dtTO,int TSUID, Int64 nUserId)
        {
            List<WUProductionDailyInspection> oWUProductionDailyInspections = new List<WUProductionDailyInspection>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WUProductionDailyInspectionDA.Gets(tc, dtFrom, sFEOID, sBuyerID, sFMID, dtTO, TSUID);
                oWUProductionDailyInspections = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                WUProductionDailyInspection oWUProductionDailyInspection = new WUProductionDailyInspection();
                oWUProductionDailyInspection.ErrorMessage = e.Message;
                oWUProductionDailyInspections.Add(oWUProductionDailyInspection);
                #endregion
            }
            return oWUProductionDailyInspections;
        }

        #endregion
    }
}
