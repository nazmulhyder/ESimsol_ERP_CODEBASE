using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class TransferRequisitionSlipRegisterService : MarshalByRefObject, ITransferRequisitionSlipRegisterService
    {
        #region Private functions and declaration
        private TransferRequisitionSlipRegister MapObject(NullHandler oReader)
        {
            TransferRequisitionSlipRegister oTransferRequisitionSlipRegister = new TransferRequisitionSlipRegister();
            oTransferRequisitionSlipRegister.TRSDetailID = oReader.GetInt32("TRSDetailID");
            oTransferRequisitionSlipRegister.TRSID = oReader.GetInt32("TRSID");
            oTransferRequisitionSlipRegister.ProductID = oReader.GetInt32("ProductID");
            oTransferRequisitionSlipRegister.MUnitID = oReader.GetInt32("MUnitID");
            oTransferRequisitionSlipRegister.Qty = oReader.GetDouble("Qty");
            oTransferRequisitionSlipRegister.TRSNO = oReader.GetString("TRSNO");
            oTransferRequisitionSlipRegister.IssueBUID = oReader.GetInt32("IssueBUID");
            oTransferRequisitionSlipRegister.IssueDateTime = oReader.GetDateTime("IssueDateTime");
            oTransferRequisitionSlipRegister.TransferStatus = (EnumTransferStatus)oReader.GetInt32("TransferStatus");
            oTransferRequisitionSlipRegister.ReceivedBUID = oReader.GetInt32("ReceivedBUID");
            oTransferRequisitionSlipRegister.IssueWorkingUnitID = oReader.GetInt32("IssueWorkingUnitID");
            oTransferRequisitionSlipRegister.ReceivedWorkingUnitID = oReader.GetInt32("ReceivedWorkingUnitID");
            oTransferRequisitionSlipRegister.DisburseByUserID = oReader.GetInt32("DisburseByUserID");
            oTransferRequisitionSlipRegister.RequisitionType = (EnumRequisitionType)oReader.GetInt32("RequisitionType");
            oTransferRequisitionSlipRegister.ReceivedByUserID = oReader.GetInt32("ReceivedByUserID");
            oTransferRequisitionSlipRegister.AuthorisedByID = oReader.GetInt32("AuthorisedByID");
            oTransferRequisitionSlipRegister.ReceivedBUName = oReader.GetString("ReceivedBUName");
            oTransferRequisitionSlipRegister.ReceivedByName = oReader.GetString("ReceivedByName");
            oTransferRequisitionSlipRegister.ReceivedStoreName = oReader.GetString("ReceivedStoreName");
            oTransferRequisitionSlipRegister.ApproveByName = oReader.GetString("ApproveByName");
            oTransferRequisitionSlipRegister.ColorName = oReader.GetString("ColorName");
            oTransferRequisitionSlipRegister.LotNo = oReader.GetString("LotNo");
            oTransferRequisitionSlipRegister.SupplierSName = oReader.GetString("SupplierSName");
            oTransferRequisitionSlipRegister.ProductCode = oReader.GetString("ProductCode");
            oTransferRequisitionSlipRegister.ProductName = oReader.GetString("ProductName");
            oTransferRequisitionSlipRegister.MUName = oReader.GetString("MUName");
            oTransferRequisitionSlipRegister.MUSymbol = oReader.GetString("MUSymbol");
            oTransferRequisitionSlipRegister.PreparedByName = oReader.GetString("PreparedByName");
            oTransferRequisitionSlipRegister.DriverName = oReader.GetString("DriverName");
            oTransferRequisitionSlipRegister.StyleNo = oReader.GetString("StyleNo");
            oTransferRequisitionSlipRegister.VehicleNo = oReader.GetString("VehicleNo");
            oTransferRequisitionSlipRegister.GatePassNo = oReader.GetString("GatePassNo");
            oTransferRequisitionSlipRegister.AythorisedByName = oReader.GetString("AythorisedByName");
            oTransferRequisitionSlipRegister.IssueStoreName = oReader.GetString("IssueStoreName");
            oTransferRequisitionSlipRegister.Remark = oReader.GetString("Remark");
            oTransferRequisitionSlipRegister.DisbursedByName = oReader.GetString("DisbursedByName");
            
            return oTransferRequisitionSlipRegister;
        }

        private TransferRequisitionSlipRegister CreateObject(NullHandler oReader)
        {
            TransferRequisitionSlipRegister oTransferRequisitionSlipRegister = new TransferRequisitionSlipRegister();
            oTransferRequisitionSlipRegister = MapObject(oReader);
            return oTransferRequisitionSlipRegister;
        }

        private List<TransferRequisitionSlipRegister> CreateObjects(IDataReader oReader)
        {
            List<TransferRequisitionSlipRegister> oTransferRequisitionSlipRegister = new List<TransferRequisitionSlipRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TransferRequisitionSlipRegister oItem = CreateObject(oHandler);
                oTransferRequisitionSlipRegister.Add(oItem);
            }
            return oTransferRequisitionSlipRegister;
        }

        #endregion

        #region Interface implementation
        public TransferRequisitionSlipRegisterService() { }        
        public List<TransferRequisitionSlipRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<TransferRequisitionSlipRegister> oTransferRequisitionSlipRegister = new List<TransferRequisitionSlipRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = TransferRequisitionSlipRegisterDA.Gets(tc, sSQL);
                oTransferRequisitionSlipRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferRequisitionSlipRegister", e);
                #endregion
            }

            return oTransferRequisitionSlipRegister;
        }
        #endregion
    }
}
