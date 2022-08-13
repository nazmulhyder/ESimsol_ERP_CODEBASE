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
    public class ServiceInvoiceRegisterService : MarshalByRefObject, IServiceInvoiceRegisterService
    {
        #region Private functions and declaration

        private ServiceInvoiceRegister MapObject(NullHandler oReader)
        {
            ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
            oServiceInvoiceRegister.ServiceInvoiceDetailID = oReader.GetInt32("ServiceInvoiceDetailID");
            oServiceInvoiceRegister.ServiceInvoiceID = oReader.GetInt32("ServiceInvoiceID");
            oServiceInvoiceRegister.PartsNo = oReader.GetString("PartsNo");
            oServiceInvoiceRegister.PartsName = oReader.GetString("PartsName");
            oServiceInvoiceRegister.PartsCode = oReader.GetString("PartsCode");
            oServiceInvoiceRegister.CustomerID = oReader.GetInt32("CustomerID");
            oServiceInvoiceRegister.CustomerName = oReader.GetString("CustomerName");
            oServiceInvoiceRegister.ServiceInvoiceDate = oReader.GetDateTime("ServiceInvoiceDate");
            oServiceInvoiceRegister.VehicleRegNo = oReader.GetString("VehicleRegNo");
            oServiceInvoiceRegister.ChassisNo = oReader.GetString("ChassisNo");
            oServiceInvoiceRegister.EngineNo = oReader.GetString("EngineNo");
            oServiceInvoiceRegister.VehicleModelNo = oReader.GetString("VehicleModelNo");
            oServiceInvoiceRegister.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oServiceInvoiceRegister.MUnitID = oReader.GetInt32("MUnitID");
            oServiceInvoiceRegister.MUName = oReader.GetString("MUName");
            oServiceInvoiceRegister.Qty = oReader.GetDouble("Qty");
            oServiceInvoiceRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oServiceInvoiceRegister.TotalPrice = oReader.GetDouble("TotalPrice");
            oServiceInvoiceRegister.Remarks = oReader.GetString("Remarks");
            oServiceInvoiceRegister.VehiclePartsID = oReader.GetInt32("VehiclePartsID");
            oServiceInvoiceRegister.ChargeType = oReader.GetInt32("ChargeType");
            oServiceInvoiceRegister.ServiceOrderType = (EnumServiceOrderType)oReader.GetInt32("ServiceOrderType");

            return oServiceInvoiceRegister;
        }

        private ServiceInvoiceRegister CreateObject(NullHandler oReader)
        {
            ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
            oServiceInvoiceRegister = MapObject(oReader);
            return oServiceInvoiceRegister;
        }

        private List<ServiceInvoiceRegister> CreateObjects(IDataReader oReader)
        {
            List<ServiceInvoiceRegister> oServiceInvoiceRegister = new List<ServiceInvoiceRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ServiceInvoiceRegister oItem = CreateObject(oHandler);
                oServiceInvoiceRegister.Add(oItem);
            }
            return oServiceInvoiceRegister;
        }

        #endregion

        #region Interface implementation


        public ServiceInvoiceRegister Get(int id, Int64 nUserId)
        {
            ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ServiceInvoiceRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oServiceInvoiceRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ServiceInvoiceRegister", e);
                #endregion
            }
            return oServiceInvoiceRegister;
        }

        public List<ServiceInvoiceRegister> Gets(int nServiceInvoiceID, Int64 nUserID)
        {
            List<ServiceInvoiceRegister> oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceRegisterDA.Gets(tc, nServiceInvoiceID);
                oServiceInvoiceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
                oServiceInvoiceRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oServiceInvoiceRegisters;
        }

        public List<ServiceInvoiceRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ServiceInvoiceRegister> oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ServiceInvoiceRegisterDA.Gets(tc, sSQL);
                oServiceInvoiceRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ServiceInvoiceRegister", e);
                #endregion
            }
            return oServiceInvoiceRegisters;
        }

        #endregion
    }

}
