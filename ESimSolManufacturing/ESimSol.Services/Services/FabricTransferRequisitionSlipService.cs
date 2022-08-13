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
    public class FabricTransferRequisitionSlipService : MarshalByRefObject, IFabricTransferRequisitionSlipService
    {
        #region Private functions and declaration

        private FabricTransferRequisitionSlip MapObject(NullHandler oReader)
        {
            FabricTransferRequisitionSlip oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
            oFabricTransferRequisitionSlip.FabricTRSID = oReader.GetInt32("FabricTRSID");
            oFabricTransferRequisitionSlip.TRSNO = oReader.GetString("TRSNO");
            oFabricTransferRequisitionSlip.RequisitionType = oReader.GetInt32("RequisitionType");
            oFabricTransferRequisitionSlip.BUIDIssue = oReader.GetInt32("BUIDIssue");
            oFabricTransferRequisitionSlip.WorkingUnitIDIssue = oReader.GetInt32("WorkingUnitIDIssue");
            oFabricTransferRequisitionSlip.BUIDReceive = oReader.GetInt32("BUIDReceive");
            oFabricTransferRequisitionSlip.WorkingUnitIDReceive = oReader.GetInt32("WorkingUnitIDReceive");
            oFabricTransferRequisitionSlip.PrepareBy = oReader.GetInt32("PrepareBy");
            oFabricTransferRequisitionSlip.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricTransferRequisitionSlip.Remarks = oReader.GetString("Remarks");
            oFabricTransferRequisitionSlip.IssueDateTime = oReader.GetDateTime("IssueDateTime");
            oFabricTransferRequisitionSlip.VehicleNo = oReader.GetString("VehicleNo");
            oFabricTransferRequisitionSlip.DriverName = oReader.GetString("DriverName");
            oFabricTransferRequisitionSlip.GatePassNo = oReader.GetString("GatePassNo");
            oFabricTransferRequisitionSlip.DisburseBy = oReader.GetInt32("DisburseBy");
            oFabricTransferRequisitionSlip.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oFabricTransferRequisitionSlip.LocationNameIssue = oReader.GetString("LocationNameIssue");
            oFabricTransferRequisitionSlip.LocationShortNameIssue = oReader.GetString("LocationShortNameIssue");
            oFabricTransferRequisitionSlip.OperationUnitNameIssue = oReader.GetString("OperationUnitNameIssue");
            oFabricTransferRequisitionSlip.LocationNameReceive = oReader.GetString("LocationNameReceive");
            oFabricTransferRequisitionSlip.LocationShortNameReceive = oReader.GetString("LocationShortNameReceive");
            oFabricTransferRequisitionSlip.OperationUnitNameReceive = oReader.GetString("OperationUnitNameReceive");
            return oFabricTransferRequisitionSlip;
        }

        private FabricTransferRequisitionSlip CreateObject(NullHandler oReader)
        {
            FabricTransferRequisitionSlip oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
            oFabricTransferRequisitionSlip = MapObject(oReader);
            return oFabricTransferRequisitionSlip;
        }

        private List<FabricTransferRequisitionSlip> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferRequisitionSlip> oFabricTransferRequisitionSlip = new List<FabricTransferRequisitionSlip>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferRequisitionSlip oItem = CreateObject(oHandler);
                oFabricTransferRequisitionSlip.Add(oItem);
            }
            return oFabricTransferRequisitionSlip;
        }

        #endregion

        #region Interface implementation
        public FabricTransferRequisitionSlip Get(int id, Int64 nUserId)
        {
            FabricTransferRequisitionSlip oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricTransferRequisitionSlipDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricTransferRequisitionSlip = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricTransferRequisitionSlip", e);
                #endregion
            }
            return oFabricTransferRequisitionSlip;
        }

        public List<FabricTransferRequisitionSlip> Gets(Int64 nUserID)
        {
            List<FabricTransferRequisitionSlip> oFabricTransferRequisitionSlips = new List<FabricTransferRequisitionSlip>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricTransferRequisitionSlipDA.Gets(tc);
                oFabricTransferRequisitionSlips = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricTransferRequisitionSlip oFabricTransferRequisitionSlip = new FabricTransferRequisitionSlip();
                oFabricTransferRequisitionSlip.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricTransferRequisitionSlips;
        }

        public List<FabricTransferRequisitionSlip> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricTransferRequisitionSlip> oFabricTransferRequisitionSlips = new List<FabricTransferRequisitionSlip>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricTransferRequisitionSlipDA.Gets(tc, sSQL);
                oFabricTransferRequisitionSlips = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricTransferRequisitionSlip", e);
                #endregion
            }
            return oFabricTransferRequisitionSlips;
        }

        #endregion
    }

}
