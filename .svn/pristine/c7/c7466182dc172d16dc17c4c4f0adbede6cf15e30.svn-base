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
    public class FNOrderUpdateStatusService : MarshalByRefObject, IFNOrderUpdateStatusService
    {
        #region Private functions and declaration
        private FNOrderUpdateStatus MapObject(NullHandler oReader)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            oFNOrderUpdateStatus.FSCDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            oFNOrderUpdateStatus.ColorInfo = oReader.GetString("ColorInfo");
            oFNOrderUpdateStatus.Construction = oReader.GetString("Construction");
            oFNOrderUpdateStatus.FabricID = oReader.GetInt32("FabricID");
            oFNOrderUpdateStatus.OrderQty = oReader.GetDouble("OrderQty");
            oFNOrderUpdateStatus.ExeNo = oReader.GetString("ExeNo");
            oFNOrderUpdateStatus.SCNo = oReader.GetString("SCNo");
            oFNOrderUpdateStatus.OrderType = oReader.GetInt32("OrderType");
            oFNOrderUpdateStatus.BuyerID = oReader.GetInt32("BuyerID");
            oFNOrderUpdateStatus.FabricNo = oReader.GetString("FabricNo");
            oFNOrderUpdateStatus.BuyerName = oReader.GetString("BuyerName");
            oFNOrderUpdateStatus.ContractorID = oReader.GetInt32("ContractorID");
            oFNOrderUpdateStatus.ContractorName = oReader.GetString("ContractorName");
            oFNOrderUpdateStatus.SCNoFull = oReader.GetString("SCNoFull");
            oFNOrderUpdateStatus.OrderQty = oReader.GetDouble("OrderQty");
            oFNOrderUpdateStatus.GradeAQty = oReader.GetDouble("GAQty");
            oFNOrderUpdateStatus.GradeBQty = oReader.GetDouble("GBQty");
            oFNOrderUpdateStatus.GradeCQty = oReader.GetDouble("GCQty");
            oFNOrderUpdateStatus.GradeDQty = oReader.GetDouble("GDQty");
            oFNOrderUpdateStatus.RejQty = oReader.GetDouble("RejQty");
            oFNOrderUpdateStatus.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oFNOrderUpdateStatus.DONo = oReader.GetString("DoNo");
            oFNOrderUpdateStatus.StoreRcvQty = oReader.GetDouble("StoreRcvQty");
            oFNOrderUpdateStatus.StoreRcvQtyDay = oReader.GetDouble("StoreRcvQtyDay");
            oFNOrderUpdateStatus.DCQtyDay = oReader.GetDouble("DCQtyDay");


            #region Stock Report
            oFNOrderUpdateStatus.PINo = oReader.GetString("PINo");
            oFNOrderUpdateStatus.MKTPerson = oReader.GetString("MKTPerson");
            oFNOrderUpdateStatus.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFNOrderUpdateStatus.WeaveName = oReader.GetString("WeaveName");
            oFNOrderUpdateStatus.Color = oReader.GetString("Color");
            oFNOrderUpdateStatus.FabricWidth = oReader.GetString("FabricWidth");
            oFNOrderUpdateStatus.Location = oReader.GetString("Location");
            oFNOrderUpdateStatus.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFNOrderUpdateStatus.ReviseNo = oReader.GetInt32("ReviseNo");
            oFNOrderUpdateStatus.ProcessType = oReader.GetInt32("ProcessType");
            oFNOrderUpdateStatus.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFNOrderUpdateStatus.FabricWeave = oReader.GetInt32("FabricWeave");
            oFNOrderUpdateStatus.DaysStay = oReader.GetInt32("DaysStay");
            //oFNOrderUpdateStatus.ReadyStock = oReader.GetDouble("ReadyStock");
            oFNOrderUpdateStatus.Balance = oReader.GetDouble("Balance");
            oFNOrderUpdateStatus.DOQty = oReader.GetDouble("DOQty");
            oFNOrderUpdateStatus.QtyOpen = oReader.GetDouble("QtyOpen");
            oFNOrderUpdateStatus.QtyIn = oReader.GetDouble("QtyIn");
            oFNOrderUpdateStatus.QtyOut = oReader.GetDouble("QtyOut");
            oFNOrderUpdateStatus.RollNo = oReader.GetDouble("RollNo");
            oFNOrderUpdateStatus.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFNOrderUpdateStatus.OrderName = oReader.GetString("OrderName");
            #endregion
            oFNOrderUpdateStatus.GreyRecd = oReader.GetDouble("GreyRecd");
            oFNOrderUpdateStatus.BatchQty = oReader.GetDouble("BatchQty");
            //oFNOrderUpdateStatus.InsQty = oReader.GetDouble("InsQty");
            oFNOrderUpdateStatus.DCQty = oReader.GetDouble("DCQty");
            oFNOrderUpdateStatus.RCQty = oReader.GetDouble("RCQty");
            oFNOrderUpdateStatus.StockInHand = oReader.GetDouble("StockInHand");
            oFNOrderUpdateStatus.ExcessQty = oReader.GetDouble("ExcessQty");
            oFNOrderUpdateStatus.ExcessDCQty = oReader.GetDouble("ExcessDCQty");
            oFNOrderUpdateStatus.ReqGreyRcv = oReader.GetDouble("ReqGreyRcv");
            if (oFNOrderUpdateStatus.StockInHand < 0) { oFNOrderUpdateStatus.StockInHand = 0; }

            return oFNOrderUpdateStatus;
        }
        private FNOrderUpdateStatus CreateObject(NullHandler oReader)
        {
            FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
            oFNOrderUpdateStatus = MapObject(oReader);
            return oFNOrderUpdateStatus;
        }
        private List<FNOrderUpdateStatus> CreateObjects(IDataReader oReader)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatus = new List<FNOrderUpdateStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNOrderUpdateStatus oItem = CreateObject(oHandler);
                oFNOrderUpdateStatus.Add(oItem);
            }
            return oFNOrderUpdateStatus;
        }

        #endregion
        #region Interface implementation
        public List<FNOrderUpdateStatus> Gets(string sSQL, int nType, DateTime dtStart, DateTime dtEnd, Int64 nUserID)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNOrderUpdateStatusDA.Gets(tc,  sSQL,  nType,  dtStart,  dtEnd);
                oFNOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
                FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
                oFNOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
                #endregion
            }
            return oFNOrderUpdateStatuss;
        }
        public List<FNOrderUpdateStatus> GetStockReport(DateTime dtStart, DateTime dtEnd, int nReportType, int nWorkingUnitID, long nUserID)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNOrderUpdateStatusDA.GetStockReport(tc, dtStart, dtEnd, nReportType, nWorkingUnitID, nUserID);
                oFNOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
                FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
                oFNOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
                #endregion
            }
            return oFNOrderUpdateStatuss;
        }




        

        public List<FNOrderUpdateStatus> Gets(string sSQL, Int64 nUserID)
        {
            List<FNOrderUpdateStatus> oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNOrderUpdateStatusDA.Gets(tc, sSQL);
                oFNOrderUpdateStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNOrderUpdateStatuss = new List<FNOrderUpdateStatus>();
                FNOrderUpdateStatus oFNOrderUpdateStatus = new FNOrderUpdateStatus();
                oFNOrderUpdateStatus.ErrorMessage = e.Message.Split('~')[0];
                oFNOrderUpdateStatuss.Add(oFNOrderUpdateStatus);
                #endregion
            }
            return oFNOrderUpdateStatuss;
        }

        #endregion
    }
}
