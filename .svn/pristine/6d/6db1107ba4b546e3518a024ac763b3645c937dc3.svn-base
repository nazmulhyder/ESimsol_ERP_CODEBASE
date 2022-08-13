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
    public class DUDeliveryChallanRegisterService : MarshalByRefObject, IDUDeliveryChallanRegisterService
    {
        #region Private functions and declaration
        private DUDeliveryChallanRegister MapObject(NullHandler oReader)
        {
            DUDeliveryChallanRegister oDUDeliveryChallanRegister = new DUDeliveryChallanRegister();
            oDUDeliveryChallanRegister.DUDeliveryChallanDetailID = oReader.GetInt32("DUDeliveryChallanDetailID");
            oDUDeliveryChallanRegister.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUDeliveryChallanRegister.ProductID = oReader.GetInt32("ProductID");
            //oDUDeliveryChallanRegister.MUnitID = oReader.GetInt32("MUnitID");
            oDUDeliveryChallanRegister.Qty = oReader.GetDouble("Qty");
            oDUDeliveryChallanRegister.ChallanNo = oReader.GetString("ChallanNo");
            //oDUDeliveryChallanRegister.BUID = oReader.GetInt32("BUID");
            oDUDeliveryChallanRegister.ChallanDate = oReader.GetDateTime("ChallanDate");
            //oDUDeliveryChallanRegister.ChallanStatus = (EnumChallanStatus)oReader.GetInt32("ChallanStatus");
            oDUDeliveryChallanRegister.ContractorID = oReader.GetInt32("ContractorID");
            oDUDeliveryChallanRegister.LotID = oReader.GetInt32("LotID");
            oDUDeliveryChallanRegister.ContactPersonnelID = oReader.GetInt32("ContactPersonnelID");
            oDUDeliveryChallanRegister.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDUDeliveryChallanRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            //oDUDeliveryChallanRegister.StoreInchargeID = oReader.GetInt32("StoreInchargeID");
            oDUDeliveryChallanRegister.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oDUDeliveryChallanRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oDUDeliveryChallanRegister.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDUDeliveryChallanRegister.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oDUDeliveryChallanRegister.ReceivedByName = oReader.GetString("ReceivedByName");
            oDUDeliveryChallanRegister.Remarks = oReader.GetString("Remarks");
            oDUDeliveryChallanRegister.ApproveByName = oReader.GetString("ApproveByName");
            oDUDeliveryChallanRegister.ProductCode = oReader.GetString("ProductCode");
            oDUDeliveryChallanRegister.ProductName = oReader.GetString("ProductName");
            //oDUDeliveryChallanRegister.MUName = oReader.GetString("MUName");
            oDUDeliveryChallanRegister.MUnit = oReader.GetString("MUSymbol");
            oDUDeliveryChallanRegister.ContractorName = oReader.GetString("ContractorName");
            oDUDeliveryChallanRegister.SCPName = oReader.GetString("SCPName");
            oDUDeliveryChallanRegister.CPName = oReader.GetString("CPName");
            oDUDeliveryChallanRegister.VehicleNo = oReader.GetString("VehicleNo");
            oDUDeliveryChallanRegister.VehicleName = oReader.GetString("VehicleName");
            oDUDeliveryChallanRegister.GatePassNo = oReader.GetString("GatePassNo");
            oDUDeliveryChallanRegister.DONo = oReader.GetString("DONo");
            oDUDeliveryChallanRegister.PINo = oReader.GetString("PI_SampleNo");
            oDUDeliveryChallanRegister.LotNo = oReader.GetString("LotNo");
            oDUDeliveryChallanRegister.LogNo = oReader.GetString("LogNo");
            oDUDeliveryChallanRegister.Shade = oReader.GetInt16("Shade");
            oDUDeliveryChallanRegister.ExportLCNo = oReader.GetString("ExportLCNo");
            oDUDeliveryChallanRegister.OrderNo = oReader.GetString("OrderNo");
            oDUDeliveryChallanRegister.NoCode = oReader.GetString("NoCode");
            oDUDeliveryChallanRegister.ColorName = oReader.GetString("ColorName");
            oDUDeliveryChallanRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oDUDeliveryChallanRegister.Qty_Order = oReader.GetDouble("Qty_Order");
            oDUDeliveryChallanRegister.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oDUDeliveryChallanRegister.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            
            
            return oDUDeliveryChallanRegister;
        }

        private DUDeliveryChallanRegister CreateObject(NullHandler oReader)
        {
            DUDeliveryChallanRegister oDUDeliveryChallanRegister = new DUDeliveryChallanRegister();
            oDUDeliveryChallanRegister = MapObject(oReader);
            return oDUDeliveryChallanRegister;
        }

        private List<DUDeliveryChallanRegister> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryChallanRegister> oDUDeliveryChallanRegister = new List<DUDeliveryChallanRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallanRegister oItem = CreateObject(oHandler);
                oDUDeliveryChallanRegister.Add(oItem);
            }
            return oDUDeliveryChallanRegister;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryChallanRegisterService() { }        
        public List<DUDeliveryChallanRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDeliveryChallanRegister> oDUDeliveryChallanRegister = new List<DUDeliveryChallanRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryChallanRegisterDA.Gets(tc, sSQL);
                oDUDeliveryChallanRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUDeliveryChallanRegister", e);
                #endregion
            }

            return oDUDeliveryChallanRegister;
        }
        #endregion
    }
}
