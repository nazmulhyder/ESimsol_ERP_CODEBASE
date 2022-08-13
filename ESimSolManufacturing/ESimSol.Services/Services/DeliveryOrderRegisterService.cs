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
    public class DeliveryOrderRegisterService : MarshalByRefObject, IDeliveryOrderRegisterService
    {
        #region Private functions and declaration
        private DeliveryOrderRegister MapObject(NullHandler oReader)
        {
            DeliveryOrderRegister oDeliveryOrderRegister = new DeliveryOrderRegister();
            oDeliveryOrderRegister.DeliveryOrderDetailID = oReader.GetInt32("DeliveryOrderDetailID");
            oDeliveryOrderRegister.DeliveryOrderID = oReader.GetInt32("DeliveryOrderID");
            oDeliveryOrderRegister.ProductID = oReader.GetInt32("ProductID");
            oDeliveryOrderRegister.MUnitID = oReader.GetInt32("MUnitID");
            oDeliveryOrderRegister.Qty = oReader.GetDouble("Qty");
            oDeliveryOrderRegister.DONo = oReader.GetString("DONo");
            oDeliveryOrderRegister.BUID = oReader.GetInt32("BUID");
            oDeliveryOrderRegister.DODate = oReader.GetDateTime("DODate");
            oDeliveryOrderRegister.DOStatus = (EnumDOStatus)oReader.GetInt32("DOStatus");
            oDeliveryOrderRegister.ContractorID = oReader.GetInt32("ContractorID");

            oDeliveryOrderRegister.RefID = oReader.GetInt32("RefID");

            oDeliveryOrderRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oDeliveryOrderRegister.DeliveryDate = oReader.GetDateTime("ApproveDate");
            oDeliveryOrderRegister.Note = oReader.GetString("Note");
            oDeliveryOrderRegister.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oDeliveryOrderRegister.ApproveByName = oReader.GetString("ApproveByName");
            oDeliveryOrderRegister.ProductCode = oReader.GetString("ProductCode");
            oDeliveryOrderRegister.ProductName = oReader.GetString("ProductName");
            oDeliveryOrderRegister.MUName = oReader.GetString("MUName");
            oDeliveryOrderRegister.MUSymbol = oReader.GetString("MUSymbol");
            oDeliveryOrderRegister.ContractorName = oReader.GetString("ContractorName");
            oDeliveryOrderRegister.DONo = oReader.GetString("DONo");
            oDeliveryOrderRegister.PINo = oReader.GetString("PINo");
            oDeliveryOrderRegister.ExportLCNo = oReader.GetString("ExportLCNo");
            return oDeliveryOrderRegister;
        }

        private DeliveryOrderRegister CreateObject(NullHandler oReader)
        {
            DeliveryOrderRegister oDeliveryOrderRegister = new DeliveryOrderRegister();
            oDeliveryOrderRegister = MapObject(oReader);
            return oDeliveryOrderRegister;
        }

        private List<DeliveryOrderRegister> CreateObjects(IDataReader oReader)
        {
            List<DeliveryOrderRegister> oDeliveryOrderRegister = new List<DeliveryOrderRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DeliveryOrderRegister oItem = CreateObject(oHandler);
                oDeliveryOrderRegister.Add(oItem);
            }
            return oDeliveryOrderRegister;
        }

        #endregion

        #region Interface implementation
        public DeliveryOrderRegisterService() { }        
        public List<DeliveryOrderRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<DeliveryOrderRegister> oDeliveryOrderRegister = new List<DeliveryOrderRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DeliveryOrderRegisterDA.Gets(tc, sSQL);
                oDeliveryOrderRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DeliveryOrderRegister", e);
                #endregion
            }

            return oDeliveryOrderRegister;
        }
        #endregion
    }
}
