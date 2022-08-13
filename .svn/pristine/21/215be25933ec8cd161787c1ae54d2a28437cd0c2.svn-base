using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{

    public class DUDeliverySummaryService : MarshalByRefObject, IDUDeliverySummaryService
    {
        #region Private functions and declaration
        DateTime dDate = new DateTime();
        double nUnManageQty = 0;
     
   
        private DUDeliverySummary MapObject(NullHandler oReader)
        {
            DUDeliverySummary oDUDeliverySummary = new DUDeliverySummary();
            oDUDeliverySummary.RSNo = oReader.GetString("RouteSheetNo");
            oDUDeliverySummary.RSID = oReader.GetInt32("RouteSheetID");
            oDUDeliverySummary.ChallanID = oReader.GetInt32("ChallanID");
            oDUDeliverySummary.ChallanNo = oReader.GetString("ChallanNo");
            dDate = oReader.GetDateTime("RSDate");
            oDUDeliverySummary.RSDate = dDate.ToString("dd MMM yyyy");

            dDate = oReader.GetDateTime("ChallanDate");
            oDUDeliverySummary.ChallanDate = dDate.ToString("dd MMM yyyy");
            oDUDeliverySummary.RSQty = oReader.GetDouble("RSQty");
            oDUDeliverySummary.Product = oReader.GetString("ProductName");
            oDUDeliverySummary.OrderNo = oReader.GetString("OrderNo");
            oDUDeliverySummary.DeliverTo = oReader.GetString("DeliverdTo");
            oDUDeliverySummary.Buyer = oReader.GetString("Buyer");
            oDUDeliverySummary.FactN = oReader.GetString("Factory");
            oDUDeliverySummary.Location = oReader.GetString("LocationName");
            oDUDeliverySummary.RawYarnIssue = oReader.GetDouble("RawYarnIssue");
            oDUDeliverySummary.InSubFinish = oReader.GetDouble("InSubFinish");
            oDUDeliverySummary.InFinishing = oReader.GetDouble("InFinishing");
            oDUDeliverySummary.FreshDyedYarn = oReader.GetDouble("FreshDyedYarn");
            oDUDeliverySummary.ManagedQty = oReader.GetDouble("ManagedQty");
            oDUDeliverySummary.Wastage = oReader.GetDouble("Wastage");
            oDUDeliverySummary.Recycle = oReader.GetDouble("Recycle");
            oDUDeliverySummary.Gain = oReader.GetDouble("Gain");
            oDUDeliverySummary.Loss = oReader.GetDouble("Loss");
            oDUDeliverySummary.DeliveredQty = oReader.GetDouble("DeliveredQty");
            oDUDeliverySummary.InRecycleRec = oReader.GetDouble("InRecycleRec");
            oDUDeliverySummary.InWastageRec = oReader.GetDouble("InWastageRec");
            oDUDeliverySummary.TotalDel = oReader.GetDouble("TotalDel");

            return oDUDeliverySummary;
        }

        private DUDeliverySummary CreateObject(NullHandler oReader)
        {
            DUDeliverySummary oDUDeliverySummary = new DUDeliverySummary();
            oDUDeliverySummary = MapObject(oReader);
            return oDUDeliverySummary;
        }

        private List<DUDeliverySummary> CreateObjects(IDataReader oReader)
        {
            List<DUDeliverySummary> oDUDeliverySummary = new List<DUDeliverySummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliverySummary oItem = CreateObject(oHandler);
                oDUDeliverySummary.Add(oItem);
            }
            return oDUDeliverySummary;
        }

        private DUDeliverySummary MapObject_Order(NullHandler oReader)
        {
            DUDeliverySummary oDUDeliverySummary = new DUDeliverySummary();
            oDUDeliverySummary.OrderID = oReader.GetInt32("OrderID");
            oDUDeliverySummary.OrderType = oReader.GetInt32("OrderType");
            oDUDeliverySummary.OrderNo = oReader.GetString("OrderNo");
           // oDUDeliverySummary.DeliverTo = oReader.GetString("DeliverdTo");
            oDUDeliverySummary.FactN = oReader.GetString("Factory");
            //oDUDeliverySummary.Buyer = oReader.GetString("Buyer");
            oDUDeliverySummary.RawYarnIssue = oReader.GetDouble("RawYarnIssue");
            oDUDeliverySummary.InSubFinish = oReader.GetDouble("InSubFinish");
            oDUDeliverySummary.InFinishing = oReader.GetDouble("InFinishing");
            oDUDeliverySummary.FreshDyedYarn = oReader.GetDouble("FreshDyedYarn");
            oDUDeliverySummary.ManagedQty = oReader.GetDouble("ManagedQty");
            oDUDeliverySummary.Wastage = oReader.GetDouble("Wastage");
            oDUDeliverySummary.Recycle = oReader.GetDouble("Recycle");
            oDUDeliverySummary.Gain = oReader.GetDouble("Gain");
            oDUDeliverySummary.Loss = oReader.GetDouble("Loss");
            oDUDeliverySummary.DeliveredQty = oReader.GetDouble("DeliveredQty");
            oDUDeliverySummary.InRecycleRec = oReader.GetDouble("InRecycleRec");
            oDUDeliverySummary.InWastageRec = oReader.GetDouble("InWastageRec");
            oDUDeliverySummary.TotalDel = oReader.GetDouble("TotalDel");

            return oDUDeliverySummary;
        }

        private DUDeliverySummary CreateObject_Order(NullHandler oReader)
        {
            DUDeliverySummary oDUDeliverySummary = new DUDeliverySummary();
            oDUDeliverySummary = MapObject_Order(oReader);
            return oDUDeliverySummary;
        }

        private List<DUDeliverySummary> CreateObjects_Order(IDataReader oReader)
        {
            List<DUDeliverySummary> oDUDeliverySummary = new List<DUDeliverySummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliverySummary oItem = CreateObject_Order(oHandler);
                oDUDeliverySummary.Add(oItem);
            }
            return oDUDeliverySummary;
        }
        #endregion

        #region Interface implementation
        public DUDeliverySummaryService() { }

        public List<DUDeliverySummary> Gets(DateTime dStartDate, DateTime dEndDate, int nOrderType, int nReportType, Int64 nUserId)
        {
            List<DUDeliverySummary> oDUDeliverySummarys = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDeliverySummaryDA.Gets(tc, dStartDate, dEndDate, nOrderType, nReportType);
                if (nReportType == 1)
                {
                    oDUDeliverySummarys = CreateObjects(reader);
                }
                else
                {
                    oDUDeliverySummarys = CreateObjects_Order(reader);
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
                throw new ServiceException("Failed to Get DUDeliverySummary", e);
                #endregion
            }

            return oDUDeliverySummarys;
        }

        #endregion
    }    
    
  
}
