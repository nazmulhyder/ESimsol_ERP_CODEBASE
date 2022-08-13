using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;
using ICS.Core.DataAccess;

using ICS.Core.Utility;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricFSStatusService : MarshalByRefObject, IFabricFSStatusService
    {
        #region Private functions and declaration
        private FabricFSStatus MapObject(NullHandler oReader)
        {
            FabricFSStatus oFabricFSStatus = new FabricFSStatus();
            oFabricFSStatus.FabricSalesContractID = oReader.GetInt32("FabricSalesContractID");
            oFabricFSStatus.SCNo = oReader.GetString("SCNo");
            oFabricFSStatus.BuyerID = oReader.GetInt32("BuyerID");
            oFabricFSStatus.SCDate = oReader.GetDateTime("SCDate");
            oFabricFSStatus.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricFSStatus.MktAccountID = oReader.GetInt32("MktAccountID");
            oFabricFSStatus.OrderType = oReader.GetInt32("OrderType");
            oFabricFSStatus.OrderName = oReader.GetString("OrderName");
            oFabricFSStatus.BuyerName = oReader.GetString("BuyerName");
            oFabricFSStatus.MktPersonName = oReader.GetString("MktPersonName");
            oFabricFSStatus.ReviseNo = oReader.GetInt32("ReviseNo");
            oFabricFSStatus.NoofDispo = oReader.GetInt32("NoofDispo");
            oFabricFSStatus.Qty = oReader.GetDouble("Qty");
            oFabricFSStatus.QtyDC = oReader.GetDouble("QtyDC");
            oFabricFSStatus.AppDCDate = oReader.GetDateTime("AppDCDate");
            oFabricFSStatus.LastDCDate = oReader.GetDateTime("LastDCDate");
            oFabricFSStatus.CurrentStatus = oReader.GetInt32("CurrentStatus");
            oFabricFSStatus.isPrint = oReader.GetBoolean("isPrint");
            return oFabricFSStatus;
        }

        private FabricFSStatus CreateObject(NullHandler oReader)
        {
            FabricFSStatus oFabricFSStatus = new FabricFSStatus();
            oFabricFSStatus = MapObject(oReader);
            return oFabricFSStatus;
        }

        private List<FabricFSStatus> CreateObjects(IDataReader oReader)
        {
            List<FabricFSStatus> oFabricFSStatuss = new List<FabricFSStatus>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricFSStatus oItem = CreateObject(oHandler);
                oFabricFSStatuss.Add(oItem);
            }
            return oFabricFSStatuss;
        }
        #endregion

        #region Interface implementation
        public FabricFSStatusService() { }
        public List<FabricFSStatus> GetsFabricFSStatus(string SQL, int nRpeortType, Int64 nUserId)
        {
            List<FabricFSStatus> oFabricFSStatuss = new List<FabricFSStatus>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                 reader = FabricFSStatusDA.GetsFabricFSStatus(tc, nRpeortType, SQL);
                oFabricFSStatuss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Status", e);

                #endregion
            }
            return oFabricFSStatuss;
        }

        #endregion
    }
}
