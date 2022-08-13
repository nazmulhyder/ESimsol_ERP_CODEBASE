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
    public class ShipmentRegisterService : MarshalByRefObject, IShipmentRegisterService
    {
        #region Private functions and declaration

        private ShipmentRegister MapObject(NullHandler oReader)
        {
            ShipmentRegister oShipmentRegister = new ShipmentRegister();
            oShipmentRegister.ShipmentDetailID = oReader.GetInt32("ShipmentDetailID");
            oShipmentRegister.ShipmentID = oReader.GetInt32("ShipmentID");
            oShipmentRegister.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oShipmentRegister.LotID = oReader.GetInt32("LotID");
            oShipmentRegister.CountryID = oReader.GetInt32("CountryID");
            oShipmentRegister.ShipmentQty = oReader.GetInt32("ShipmentQty");
            oShipmentRegister.CTNQty = oReader.GetInt32("CTNQty");
            oShipmentRegister.Remarks = oReader.GetString("Remarks");

            oShipmentRegister.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oShipmentRegister.CartonQty = oReader.GetInt32("CartonQty");
            oShipmentRegister.StyleNo = oReader.GetString("StyleNo");
            oShipmentRegister.TotalQuantity = oReader.GetInt32("TotalQuantity");
            oShipmentRegister.CountryName = oReader.GetString("CountryName");
            oShipmentRegister.CountryShortName = oReader.GetString("CountryShortName");
            //oShipmentRegister.AlreadyShipmentQty = oReader.GetInt32("AlreadyShipmentQty");
            //oShipmentRegister.YetToShipmentQty = oReader.GetInt32("YetToShipmentQty");
            oShipmentRegister.Balance = oReader.GetInt32("Balance");
            oShipmentRegister.LotNo = oReader.GetString("LotNo");

            oShipmentRegister.BUID = oReader.GetInt32("BUID");
            oShipmentRegister.BuyerID = oReader.GetInt32("BuyerID");
            oShipmentRegister.StoreID = oReader.GetInt32("StoreID");
            oShipmentRegister.ChallanNo = oReader.GetString("ChallanNo");
            oShipmentRegister.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oShipmentRegister.ShipmentMode = (EnumShipmentMode)oReader.GetInt32("ShipmentMode");
            oShipmentRegister.Remarks = oReader.GetString("Remarks");
            oShipmentRegister.TruckNo = oReader.GetString("TruckNo");
            oShipmentRegister.DriverName = oReader.GetString("DriverName");
            oShipmentRegister.DriverMobileNo = oReader.GetString("DriverMobileNo");
            oShipmentRegister.Depo = oReader.GetString("Depo");
            oShipmentRegister.Escord = oReader.GetString("Escord");
            oShipmentRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oShipmentRegister.FactoryName = oReader.GetString("FactoryName");
            oShipmentRegister.SecurityLock = oReader.GetString("SecurityLock");
            oShipmentRegister.EmptyCTNQty = oReader.GetInt32("EmptyCTNQty");
            oShipmentRegister.GumTapeQty = oReader.GetInt32("GumTapeQty");
            oShipmentRegister.BuyerName = oReader.GetString("BuyerName");
            oShipmentRegister.ApproveByName = oReader.GetString("ApproveByName");
            oShipmentRegister.StoreName = oReader.GetString("StoreName");

            oShipmentRegister.ReportLayout = (EnumReportLayout)oReader.GetInt32("ReportLayout");

            return oShipmentRegister;
        }

        private ShipmentRegister CreateObject(NullHandler oReader)
        {
            ShipmentRegister oShipmentRegister = new ShipmentRegister();
            oShipmentRegister = MapObject(oReader);
            return oShipmentRegister;
        }

        private List<ShipmentRegister> CreateObjects(IDataReader oReader)
        {
            List<ShipmentRegister> oShipmentRegister = new List<ShipmentRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShipmentRegister oItem = CreateObject(oHandler);
                oShipmentRegister.Add(oItem);
            }
            return oShipmentRegister;
        }

        #endregion

        #region Interface implementation


        public ShipmentRegister Get(int id, Int64 nUserId)
        {
            ShipmentRegister oShipmentRegister = new ShipmentRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ShipmentRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ShipmentRegister", e);
                #endregion
            }
            return oShipmentRegister;
        }

        public List<ShipmentRegister> Gets(int nShipmentID, Int64 nUserID)
        {
            List<ShipmentRegister> oShipmentRegisters = new List<ShipmentRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentRegisterDA.Gets(tc, nShipmentID);
                oShipmentRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ShipmentRegister oShipmentRegister = new ShipmentRegister();
                oShipmentRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oShipmentRegisters;
        }

        public List<ShipmentRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<ShipmentRegister> oShipmentRegisters = new List<ShipmentRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ShipmentRegisterDA.Gets(tc, sSQL);
                oShipmentRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentRegister", e);
                #endregion
            }
            return oShipmentRegisters;
        }

        #endregion
    }

}
