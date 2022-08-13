using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class RptExecutionOrderUpdateStatusService : MarshalByRefObject, IRptExecutionOrderUpdateStatusService
    {
        #region Private functions and declaration
        private RptExecutionOrderUpdateStatus MapObject(NullHandler oReader)
        {
            RptExecutionOrderUpdateStatus oExecutionOrderUpdateStatus = new RptExecutionOrderUpdateStatus();
            oExecutionOrderUpdateStatus.FEOID = oReader.GetInt32("FEOID");
            oExecutionOrderUpdateStatus.FEONo = oReader.GetString("FEONo");
            oExecutionOrderUpdateStatus.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oExecutionOrderUpdateStatus.IsInHouse = oReader.GetBoolean("IsInHouse");
            oExecutionOrderUpdateStatus.OrderDate = oReader.GetDateTime("OrderDate");
            oExecutionOrderUpdateStatus.StartDate = oReader.GetDateTime("StartDate");
            oExecutionOrderUpdateStatus.OrderQty = oReader.GetDouble("OrderQty");
            oExecutionOrderUpdateStatus.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oExecutionOrderUpdateStatus.ExpDelEndDate = oReader.GetDateTime("ExpDelEndDate");
            oExecutionOrderUpdateStatus.PPSampleDate = oReader.GetDateTime("PPSampleDate");
            oExecutionOrderUpdateStatus.BuyerID = oReader.GetInt32("BuyerID");
            oExecutionOrderUpdateStatus.BuyerName = oReader.GetString("BuyerName");
            oExecutionOrderUpdateStatus.FabricID = oReader.GetInt32("FabricID");
            oExecutionOrderUpdateStatus.Construction = oReader.GetString("Construction");
            oExecutionOrderUpdateStatus.YTODate = oReader.GetString("YTODate");
            oExecutionOrderUpdateStatus.YTOCount = oReader.GetString("YTOCount");
            oExecutionOrderUpdateStatus.YTOQty = oReader.GetString("YTOQty");
            oExecutionOrderUpdateStatus.DUYarnReceiveQty = oReader.GetDouble("DUYarnReceiveQty");
            oExecutionOrderUpdateStatus.DUIssueDate = oReader.GetString("DUIssueDate");
            oExecutionOrderUpdateStatus.DEOCount = oReader.GetString("DEOCount");
            oExecutionOrderUpdateStatus.Color = oReader.GetString("Color");
            oExecutionOrderUpdateStatus.DEOQty = oReader.GetString("DEOQty");
            oExecutionOrderUpdateStatus.WURecvQty = oReader.GetString("WURecvQty");
            oExecutionOrderUpdateStatus.WUReceiveDate = oReader.GetString("WUReceiveDate");
            oExecutionOrderUpdateStatus.WUColor = oReader.GetString("WUColor");
            oExecutionOrderUpdateStatus.WUYarnReceiveQty = oReader.GetString("WUYarnReceiveQty");
            oExecutionOrderUpdateStatus.WarpingDate = oReader.GetString("WarpingDate");
            oExecutionOrderUpdateStatus.WarpingPlanQty = Global.GetMeter(oReader.GetDouble("WarpingPlanQty"),2);
            oExecutionOrderUpdateStatus.WarpingConsumeQty = Global.GetMeter(oReader.GetDouble("WarpingConsumeQty"), 2);
            oExecutionOrderUpdateStatus.WarpingDoneQty = Global.GetMeter(oReader.GetDouble("WarpingDoneQty"),2);
            oExecutionOrderUpdateStatus.SizingDate = oReader.GetString("SizingDate");
            oExecutionOrderUpdateStatus.SizingDoneQty = Global.GetMeter(oReader.GetDouble("SizingDoneQty"),2);
            oExecutionOrderUpdateStatus.LoomDate = oReader.GetString("LoomDate");
            oExecutionOrderUpdateStatus.LoomDoneQty = Global.GetMeter(oReader.GetDouble("LoomDoneQty"),2);
            oExecutionOrderUpdateStatus.InspectionDate = oReader.GetString("InspectionDate");
            oExecutionOrderUpdateStatus.QcAndInspectionQty = Global.GetMeter(oReader.GetDouble("QcAndInspectionQty"),2);
            oExecutionOrderUpdateStatus.ReadyToDelivery = Global.GetMeter(oReader.GetDouble("ReadyToDelivery"),2);
            oExecutionOrderUpdateStatus.DeliverdToFU = Global.GetMeter(oReader.GetDouble("DeliverdToFU"),2);
            oExecutionOrderUpdateStatus.TransferDate = oReader.GetString("TransferDate");
            oExecutionOrderUpdateStatus.FNReceiveDate = oReader.GetString("FNReceiveDate");
            oExecutionOrderUpdateStatus.FNReceiveQty = Global.GetMeter(oReader.GetDouble("FNReceiveQty"),2);
            oExecutionOrderUpdateStatus.DeliveryDate = oReader.GetString("DeliveryDate");
            oExecutionOrderUpdateStatus.DeliveryQty = Global.GetMeter(oReader.GetDouble("DeliveryQty"), 2);
            oExecutionOrderUpdateStatus.StyleRef = oReader.GetString("StyleRef");
            oExecutionOrderUpdateStatus.FactoryName = oReader.GetString("FactoryName");
            oExecutionOrderUpdateStatus.FinishType = oReader.GetString("FinishType");
            oExecutionOrderUpdateStatus.FinishWidth = oReader.GetString("FinishWidth");
            oExecutionOrderUpdateStatus.DEODate = oReader.GetDateTime("DEODate");
            return oExecutionOrderUpdateStatus;
        }
        private RptExecutionOrderUpdateStatus CreateObject(NullHandler oReader)
        {
            RptExecutionOrderUpdateStatus oExecutionOrderUpdateStatus = new RptExecutionOrderUpdateStatus();
            oExecutionOrderUpdateStatus = MapObject(oReader);
            return oExecutionOrderUpdateStatus;
        }
        private List<RptExecutionOrderUpdateStatus> CreateObjects(IDataReader oReader)
        {
            List<RptExecutionOrderUpdateStatus> oExecutionOrderUpdateStatus = new List<RptExecutionOrderUpdateStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RptExecutionOrderUpdateStatus oItem = CreateObject(oHandler);
                oExecutionOrderUpdateStatus.Add(oItem);
            }
            return oExecutionOrderUpdateStatus;
        }

        #endregion

        #region Interface implementation
        public List<RptExecutionOrderUpdateStatus> Gets(string sFEOIDs, Int64 nUserID)
        {
            List<RptExecutionOrderUpdateStatus> oExecutionOrderUpdateStatuss = new List<RptExecutionOrderUpdateStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RptExecutionOrderUpdateStatusDA.Gets(tc, sFEOIDs);
                oExecutionOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExecutionOrderUpdateStatuss = new List<RptExecutionOrderUpdateStatus>();
                RptExecutionOrderUpdateStatus oExecutionOrderUpdateStatus = new RptExecutionOrderUpdateStatus();
                oExecutionOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oExecutionOrderUpdateStatuss.Add(oExecutionOrderUpdateStatus);
                #endregion
            }
            return oExecutionOrderUpdateStatuss;
        }
        #endregion
    }
}
