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
    public class WorkOrderRegisterService : MarshalByRefObject, IWorkOrderRegisterService
    {
        #region Private functions and declaration
        private WorkOrderRegister MapObject(NullHandler oReader)
        {
            WorkOrderRegister oWorkOrderRegister = new WorkOrderRegister();
            oWorkOrderRegister.WorkOrderDetailID = oReader.GetInt32("WorkOrderDetailID");
            oWorkOrderRegister.WorkOrderID = oReader.GetInt32("WorkOrderID");
            oWorkOrderRegister.ProductID = oReader.GetInt32("ProductID");
            oWorkOrderRegister.MUnitID = oReader.GetInt32("MUnitID");
            oWorkOrderRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oWorkOrderRegister.Qty = oReader.GetDouble("Qty");
            oWorkOrderRegister.Amount = oReader.GetDouble("Amount");
            oWorkOrderRegister.WorkOrderNo = oReader.GetString("WorkOrderNo");
            oWorkOrderRegister.BUID = oReader.GetInt32("BUID");
            oWorkOrderRegister.FiLeNo = oReader.GetString("FiLeNo");
            oWorkOrderRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oWorkOrderRegister.WorkOrderStatus = (EnumWorkOrderStatus)oReader.GetInt32("WorkOrderStatus");
            oWorkOrderRegister.SupplierID = oReader.GetInt32("SupplierID");
            oWorkOrderRegister.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oWorkOrderRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oWorkOrderRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oWorkOrderRegister.WorkOrderDate = oReader.GetDateTime("WorkOrderDate");
            oWorkOrderRegister.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");

            oWorkOrderRegister.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oWorkOrderRegister.Remarks = oReader.GetString("Remarks");
            oWorkOrderRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oWorkOrderRegister.ProductCode = oReader.GetString("ProductCode");
            oWorkOrderRegister.ProductName = oReader.GetString("ProductName");
            oWorkOrderRegister.MUName = oReader.GetString("MUName");
            oWorkOrderRegister.MUSymbol = oReader.GetString("MUSymbol");
            oWorkOrderRegister.SupplierName = oReader.GetString("SupplierName");
            oWorkOrderRegister.SCPName = oReader.GetString("SCPName");
            oWorkOrderRegister.CurrencyName = oReader.GetString("CurrencyName");
            oWorkOrderRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oWorkOrderRegister.RateUnit = oReader.GetInt32("RateUnit");
            return oWorkOrderRegister;
        }

        private WorkOrderRegister CreateObject(NullHandler oReader)
        {
            WorkOrderRegister oWorkOrderRegister = new WorkOrderRegister();
            oWorkOrderRegister = MapObject(oReader);
            return oWorkOrderRegister;
        }

        private List<WorkOrderRegister> CreateObjects(IDataReader oReader)
        {
            List<WorkOrderRegister> oWorkOrderRegister = new List<WorkOrderRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WorkOrderRegister oItem = CreateObject(oHandler);
                oWorkOrderRegister.Add(oItem);
            }
            return oWorkOrderRegister;
        }

        #endregion

        #region Interface implementation
        public WorkOrderRegisterService() { }        
        public List<WorkOrderRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<WorkOrderRegister> oWorkOrderRegister = new List<WorkOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WorkOrderRegisterDA.Gets(tc, sSQL);
                oWorkOrderRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get WorkOrderRegister", e);
                #endregion
            }

            return oWorkOrderRegister;
        }
        #endregion
    }
}
