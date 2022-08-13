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
    public class DUProductionStatusService : MarshalByRefObject, IDUProductionStatusService
    {
        #region Private functions and declaration
        private static DUProductionStatus MapObject(NullHandler oReader)
        {
            DUProductionStatus oDUProductionStatus = new DUProductionStatus();
            oDUProductionStatus.BUID = oReader.GetInt32("BUID");
            oDUProductionStatus.StartDate = oReader.GetDateTime("StartDate");
            oDUProductionStatus.EndDate = oReader.GetDateTime("EndDate");
            oDUProductionStatus.QtyDyeing = oReader.GetDouble("QtyDyeing");
            oDUProductionStatus.QtyRecycle = oReader.GetDouble("QtyRecycle");
            oDUProductionStatus.QtyWestage = oReader.GetDouble("QtyWestage");
            oDUProductionStatus.QtyPacking = oReader.GetDouble("QtyPacking");

            oDUProductionStatus.QtyDyeing_ReP = oReader.GetDouble("QtyDyeing_ReP");
            oDUProductionStatus.QtyRecycle_ReP = oReader.GetDouble("QtyRecycle_ReP");
            oDUProductionStatus.QtyWestage_ReP = oReader.GetDouble("QtyWestage_ReP");
            oDUProductionStatus.QtyPacking_ReP = oReader.GetDouble("QtyPacking_ReP");

            //oDUProductionStatus.QtyDyeing_In = oReader.GetDouble("Production_InHouse");
            //oDUProductionStatus.QtyDyeing_Knit = oReader.GetDouble("Production_Knit");
            //oDUProductionStatus.QtyDyeing_Out = oReader.GetDouble("Production_OutSide");
            oDUProductionStatus.QtyDyeing_Sweater = oReader.GetDouble("Production_Sweater");
            oDUProductionStatus.QtyDyeing_Total = oReader.GetDouble("Production_Total");
            oDUProductionStatus.ReProcess_InHouse = oReader.GetDouble("ReProcess_InHouse");
            oDUProductionStatus.ReProcess_OutSide = oReader.GetDouble("ReProcess_OutSide");
            oDUProductionStatus.ReProcess_Percentage = oReader.GetDouble("ReProcess_Percentage");
            oDUProductionStatus.ReProcess_Total = oReader.GetDouble("ReProcess_Total");
            oDUProductionStatus.Remarks = oReader.GetString("Remarks");

            oDUProductionStatus.RefID = oReader.GetInt32("RefID");
            oDUProductionStatus.RefName = oReader.GetString("RefName");
            oDUProductionStatus.OrderTypeSt = oReader.GetString("OrderTypeSt");

            return oDUProductionStatus;
        }

        public static DUProductionStatus CreateObject(NullHandler oReader)
        {
            DUProductionStatus oDUProductionStatus = MapObject(oReader);
            return oDUProductionStatus;
        }

        private List<DUProductionStatus> CreateObjects(IDataReader oReader)
        {
            List<DUProductionStatus> oDUProductionStatuss = new List<DUProductionStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUProductionStatus oItem = CreateObject(oHandler);
                oDUProductionStatuss.Add(oItem);
            }
            return oDUProductionStatuss;
        }

        #endregion

        #region Function
        public List<DUProductionStatus> GetsDUProductionStatus(int nBUID, int nLayout, DateTime StartDate, DateTime Enddate, string sSQL, EnumRSState nRSState, Int64 nUserID)
        {
            List<DUProductionStatus> oDUProductionStatuss = new List<DUProductionStatus>();
            DUProductionStatus oDUProductionStatus = new DUProductionStatus();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProductionStatusDA.GetsDUProductionStatus(tc, nBUID, nLayout, StartDate, Enddate, sSQL, nRSState);
                oDUProductionStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUProductionStatus.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUProductionStatuss.Add(oDUProductionStatus);
                #endregion
            }

            return oDUProductionStatuss;
        }
        #endregion
    }
}
