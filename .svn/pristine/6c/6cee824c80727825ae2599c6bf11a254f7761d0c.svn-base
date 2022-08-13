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
    public class DeliveryChallanRegisterService : MarshalByRefObject, IDeliveryChallanRegisterService
    {
        #region Private functions and declaration
        private DeliveryChallanRegister MapObject(NullHandler oReader)
        {
            DeliveryChallanRegister oDeliveryChallanRegister = new DeliveryChallanRegister();
            oDeliveryChallanRegister.DeliveryChallanDetailID = oReader.GetInt32("DeliveryChallanDetailID");
            oDeliveryChallanRegister.DeliveryChallanID = oReader.GetInt32("DeliveryChallanID");
            oDeliveryChallanRegister.ProductID = oReader.GetInt32("ProductID");
            oDeliveryChallanRegister.MUnitID = oReader.GetInt32("MUnitID");
            oDeliveryChallanRegister.Qty = oReader.GetDouble("Qty");
            oDeliveryChallanRegister.ChallanNo = oReader.GetString("ChallanNo");
            oDeliveryChallanRegister.BUID = oReader.GetInt32("BUID");
            oDeliveryChallanRegister.ChallanDate = oReader.GetDateTime("ChallanDate");
            oDeliveryChallanRegister.ChallanStatus = (EnumChallanStatus)oReader.GetInt32("ChallanStatus");
            oDeliveryChallanRegister.ContractorID = oReader.GetInt32("ContractorID");
            oDeliveryChallanRegister.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDeliveryChallanRegister.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryChallanRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDeliveryChallanRegister.StoreInchargeID = oReader.GetInt32("StoreInchargeID");
            oDeliveryChallanRegister.ChallanType = (EnumChallanType)oReader.GetInt32("ChallanType");
            oDeliveryChallanRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oDeliveryChallanRegister.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oDeliveryChallanRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDeliveryChallanRegister.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oDeliveryChallanRegister.ReceivedByName = oReader.GetString("ReceivedByName");
            oDeliveryChallanRegister.Remarks = oReader.GetString("Remarks");
            oDeliveryChallanRegister.ApproveByName = oReader.GetString("ApproveByName");
            oDeliveryChallanRegister.ProductCode = oReader.GetString("ProductCode");
            oDeliveryChallanRegister.ProductName = oReader.GetString("ProductName");
            oDeliveryChallanRegister.MUName = oReader.GetString("MUName");
            oDeliveryChallanRegister.MUSymbol = oReader.GetString("MUSymbol");
            oDeliveryChallanRegister.ContractorName = oReader.GetString("ContractorName");
            oDeliveryChallanRegister.SCPName = oReader.GetString("SCPName");
            oDeliveryChallanRegister.CPName = oReader.GetString("CPName");
            oDeliveryChallanRegister.VehicleNo = oReader.GetString("VehicleNo");
            oDeliveryChallanRegister.VehicleName = oReader.GetString("VehicleName");
            oDeliveryChallanRegister.GatePassNo = oReader.GetString("GatePassNo");
            oDeliveryChallanRegister.DONo = oReader.GetString("DONo");
            oDeliveryChallanRegister.PINo = oReader.GetString("PINo");
            oDeliveryChallanRegister.ExportLCNo = oReader.GetString("ExportLCNo");
            oDeliveryChallanRegister.DeliveryToName = oReader.GetString("DeliveryToName");
            oDeliveryChallanRegister.VehicleDateTime = oReader.GetDateTime("VehicleDateTime");
            oDeliveryChallanRegister.SalePrice = oReader.GetDouble("SalePrice");
            oDeliveryChallanRegister.RateUnit = oReader.GetDouble("RateUnit");
            return oDeliveryChallanRegister;
        }

        private DeliveryChallanRegister CreateObject(NullHandler oReader)
        {
            DeliveryChallanRegister oDeliveryChallanRegister = new DeliveryChallanRegister();
            oDeliveryChallanRegister = MapObject(oReader);
            return oDeliveryChallanRegister;
        }

        private List<DeliveryChallanRegister> CreateObjects(IDataReader oReader)
        {
            List<DeliveryChallanRegister> oDeliveryChallanRegister = new List<DeliveryChallanRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryChallanRegister oItem = CreateObject(oHandler);
                oDeliveryChallanRegister.Add(oItem);
            }
            return oDeliveryChallanRegister;
        }

        #endregion

        #region Interface implementation
        public DeliveryChallanRegisterService() { }        
        public List<DeliveryChallanRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryChallanRegister> oDeliveryChallanRegister = new List<DeliveryChallanRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryChallanRegisterDA.Gets(tc, sSQL);
                oDeliveryChallanRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryChallanRegister", e);
                #endregion
            }

            return oDeliveryChallanRegister;
        }
        #endregion
    }
}
