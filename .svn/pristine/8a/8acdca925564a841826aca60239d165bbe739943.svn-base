using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{

    public class FNExecutionOrderStatusService : MarshalByRefObject, IFNExecutionOrderStatusService
    {
        #region Private functions and declaration
        private static FNExecutionOrderStatus MapObject(NullHandler oReader)
        {
            FNExecutionOrderStatus oFNExecutionOrderStatus = new FNExecutionOrderStatus();
            oFNExecutionOrderStatus.FNExOID = oReader.GetInt32("FNExOID");
            oFNExecutionOrderStatus.FabricNo = oReader.GetString("FabricNo");
            oFNExecutionOrderStatus.FNExONo = oReader.GetString("FNExONo");
            oFNExecutionOrderStatus.IssueDate = oReader.GetDateTime("IssueDate");
            oFNExecutionOrderStatus.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFNExecutionOrderStatus.Construction = oReader.GetString("Construction");
            oFNExecutionOrderStatus.SCNo = oReader.GetString("SCNo");
            oFNExecutionOrderStatus.DispoNo = oReader.GetString("DispoNo");
            oFNExecutionOrderStatus.ReviseCount = oReader.GetInt32("ReviseCount");
            oFNExecutionOrderStatus.FabricID = oReader.GetInt32("FabricID");
            oFNExecutionOrderStatus.RawFabricRcvQty = oReader.GetDouble("RawFabricRcvQty");
            oFNExecutionOrderStatus.OrderQty = oReader.GetDouble("OrderQty");
            oFNExecutionOrderStatus.PlannedQty = oReader.GetDouble("PlannedQty");
            oFNExecutionOrderStatus.BatchQty = oReader.GetDouble("BatchQty");
            oFNExecutionOrderStatus.InspectionQty = oReader.GetDouble("InspectionQty");
            oFNExecutionOrderStatus.ReadyStock = oReader.GetDouble("ReadyStock");
            oFNExecutionOrderStatus.DeliveredQty = oReader.GetDouble("DeliveredQty");
            oFNExecutionOrderStatus.Balance = oReader.GetDouble("Balance");
            //oFNExecutionOrderStatus.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oFNExecutionOrderStatus.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFNExecutionOrderStatus.FabricSalesContractDetailID = oReader.GetInt32("FabricSalesContractDetailID");
            oFNExecutionOrderStatus.OrderType = (EnumOrderType)oReader.GetInt16("OrderType");
            oFNExecutionOrderStatus.OrderTypeInt = oReader.GetInt16("OrderType");
            oFNExecutionOrderStatus.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFNExecutionOrderStatus.ProcessType = oReader.GetInt32("ProcessType");
            oFNExecutionOrderStatus.ReviseNo = oReader.GetString("ReviseNo");

            oFNExecutionOrderStatus.DispoQty = oReader.GetDouble("DispoQty");
            oFNExecutionOrderStatus.InceptionQty = oReader.GetDouble("InceptionQty");

            return oFNExecutionOrderStatus;
        }

        public static FNExecutionOrderStatus CreateObject(NullHandler oReader)
        {
            FNExecutionOrderStatus oFNExecutionOrderStatus = new FNExecutionOrderStatus();
            oFNExecutionOrderStatus = MapObject(oReader);
            return oFNExecutionOrderStatus;
        }

        public static List<FNExecutionOrderStatus> CreateObjects(IDataReader oReader)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatus = new List<FNExecutionOrderStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNExecutionOrderStatus oItem = CreateObject(oHandler);
                oFNExecutionOrderStatus.Add(oItem);
            }
            return oFNExecutionOrderStatus;
        }

        #endregion

        #region Interface implementation
        public FNExecutionOrderStatusService() { }

        public List<FNExecutionOrderStatus> Gets(string sFNExONo, string sFNExOIDs, Int64 nUserId)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNExecutionOrderStatusDA.Gets(tc, sFNExONo, sFNExOIDs);
                oFNExecutionOrderStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                FNExecutionOrderStatus oFNExecutionOrderStatus = new FNExecutionOrderStatus();
                oFNExecutionOrderStatus.ErrorMessage = e.Message;
                oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
                oFNExecutionOrderStatuss.Add(oFNExecutionOrderStatus);
                #endregion
            }

            return oFNExecutionOrderStatuss;
        }

        public List<FNExecutionOrderStatus> GetsReport(string sSQL, Int64 nUserId)
        {
            List<FNExecutionOrderStatus> oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FNExecutionOrderStatusDA.Gets(tc, sSQL);
                oFNExecutionOrderStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                FNExecutionOrderStatus oFNExecutionOrderStatus = new FNExecutionOrderStatus();
                oFNExecutionOrderStatus.ErrorMessage = e.Message;
                oFNExecutionOrderStatuss = new List<FNExecutionOrderStatus>();
                oFNExecutionOrderStatuss.Add(oFNExecutionOrderStatus);
                #endregion
            }

            return oFNExecutionOrderStatuss;
        }

        #endregion
    }

}
