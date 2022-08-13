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
    public class PartsRequisitionRegisterService : MarshalByRefObject, IPartsRequisitionRegisterService
    {
        #region Private functions and declaration

        private PartsRequisitionRegister MapObject(NullHandler oReader)
        {
            PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
            oPartsRequisitionRegister.PartsRequisitionDetailID = oReader.GetInt32("PartsRequisitionDetailID");
            oPartsRequisitionRegister.PartsRequisitionID = oReader.GetInt32("PartsRequisitionID");
            oPartsRequisitionRegister.ProductID = oReader.GetInt32("ProductID");
            oPartsRequisitionRegister.LotID = oReader.GetInt32("LotID");
            oPartsRequisitionRegister.UnitID = oReader.GetInt32("UnitID");
            oPartsRequisitionRegister.Quantity = oReader.GetDouble("Quantity");
            oPartsRequisitionRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oPartsRequisitionRegister.Amount = oReader.GetDouble("Amount");
            oPartsRequisitionRegister.ProductCode = oReader.GetString("ProductCode");
            oPartsRequisitionRegister.ProductName = oReader.GetString("ProductName");
            oPartsRequisitionRegister.LotNo = oReader.GetString("LotNo");
            oPartsRequisitionRegister.Balance = oReader.GetDouble("Balance");
            oPartsRequisitionRegister.LotUnitPrice = oReader.GetDouble("LotUnitPrice");
            oPartsRequisitionRegister.LotUnitID = oReader.GetInt32("LotUnitID");
            oPartsRequisitionRegister.StyleID = oReader.GetInt32("StyleID");
            oPartsRequisitionRegister.ColorID = oReader.GetInt32("ColorID");
            oPartsRequisitionRegister.SizeID = oReader.GetInt32("SizeID");
            oPartsRequisitionRegister.StyleNo = oReader.GetString("StyleNo");
            oPartsRequisitionRegister.BuyerName = oReader.GetString("BuyerName");
            oPartsRequisitionRegister.ColorName = oReader.GetString("ColorName");
            oPartsRequisitionRegister.SizeName = oReader.GetString("SizeName");
            oPartsRequisitionRegister.UnitName = oReader.GetString("UnitName");
            oPartsRequisitionRegister.Symbol = oReader.GetString("Symbol");
            oPartsRequisitionRegister.ProductGroupName = oReader.GetString("ProductGroupName");
            oPartsRequisitionRegister.YetToReturnQty = oReader.GetDouble("YetToReturnQty");
            oPartsRequisitionRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oPartsRequisitionRegister.LocationName = oReader.GetString("LocationName");
            oPartsRequisitionRegister.OperationUnitName = oReader.GetString("OperationUnitName");
            oPartsRequisitionRegister.OUShortName = oReader.GetString("OUShortName");
            oPartsRequisitionRegister.LocationShortName = oReader.GetString("LocationShortName");

            oPartsRequisitionRegister.RequisitionNo = oReader.GetString("RequisitionNo");
            oPartsRequisitionRegister.BUID = oReader.GetInt32("BUID");
            oPartsRequisitionRegister.ServiceOrderID = oReader.GetInt32("ServiceOrderID");
            oPartsRequisitionRegister.VehicleRegID = oReader.GetInt32("VehicleRegID");
            oPartsRequisitionRegister.PRType = (EnumPRequisutionType)oReader.GetInt32("PRType");
            oPartsRequisitionRegister.PRTypeInt = oReader.GetInt32("PRType");
            oPartsRequisitionRegister.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oPartsRequisitionRegister.PRStatus = (EnumCRStatus)oReader.GetInt32("PRStatus");
            oPartsRequisitionRegister.PRStatusInt = oReader.GetInt32("PRStatus");
            oPartsRequisitionRegister.IssueDate = oReader.GetDateTime("IssueDate");
            oPartsRequisitionRegister.StoreID = oReader.GetInt32("StoreID");
            oPartsRequisitionRegister.Remarks = oReader.GetString("Remarks");
            oPartsRequisitionRegister.DeliveryBy = oReader.GetInt32("DeliveryBy");
            oPartsRequisitionRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPartsRequisitionRegister.StoreCode = oReader.GetString("StoreCode");
            oPartsRequisitionRegister.StoreName = oReader.GetString("StoreName");
            oPartsRequisitionRegister.ServiceOrderNo = oReader.GetString("ServiceOrderNo");
            oPartsRequisitionRegister.ChassisNo = oReader.GetString("ChassisNo");
            oPartsRequisitionRegister.EngineNo = oReader.GetString("EngineNo");
            oPartsRequisitionRegister.VehicleRegNo = oReader.GetString("VehicleRegNo");
            oPartsRequisitionRegister.ModelNo = oReader.GetString("ModelNo");
            oPartsRequisitionRegister.RequisitionByName = oReader.GetString("RequisitionByName");
            oPartsRequisitionRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oPartsRequisitionRegister.DeliveryByName = oReader.GetString("DeliveryByName");
            oPartsRequisitionRegister.CustomerID = oReader.GetInt32("CustomerID");
            oPartsRequisitionRegister.CustomerName = oReader.GetString("CustomerName");
            oPartsRequisitionRegister.RackID = oReader.GetInt32("RackID");
            oPartsRequisitionRegister.RackNo = oReader.GetString("RackNo");
            oPartsRequisitionRegister.ShelfName = oReader.GetString("ShelfName");
            oPartsRequisitionRegister.ShelfNo = oReader.GetString("ShelfNo");

            return oPartsRequisitionRegister;
        }

        private PartsRequisitionRegister CreateObject(NullHandler oReader)
        {
            PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
            oPartsRequisitionRegister = MapObject(oReader);
            return oPartsRequisitionRegister;
        }

        private List<PartsRequisitionRegister> CreateObjects(IDataReader oReader)
        {
            List<PartsRequisitionRegister> oPartsRequisitionRegister = new List<PartsRequisitionRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PartsRequisitionRegister oItem = CreateObject(oHandler);
                oPartsRequisitionRegister.Add(oItem);
            }
            return oPartsRequisitionRegister;
        }

        #endregion

        #region Interface implementation


        public PartsRequisitionRegister Get(int id, Int64 nUserId)
        {
            PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = PartsRequisitionRegisterDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPartsRequisitionRegister = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PartsRequisitionRegister", e);
                #endregion
            }
            return oPartsRequisitionRegister;
        }

        public List<PartsRequisitionRegister> Gets(int nPartsRequisitionID, Int64 nUserID)
        {
            List<PartsRequisitionRegister> oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PartsRequisitionRegisterDA.Gets(tc, nPartsRequisitionID);
                oPartsRequisitionRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                PartsRequisitionRegister oPartsRequisitionRegister = new PartsRequisitionRegister();
                oPartsRequisitionRegister.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPartsRequisitionRegisters;
        }

        public List<PartsRequisitionRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<PartsRequisitionRegister> oPartsRequisitionRegisters = new List<PartsRequisitionRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = PartsRequisitionRegisterDA.Gets(tc, sSQL);
                oPartsRequisitionRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PartsRequisitionRegister", e);
                #endregion
            }
            return oPartsRequisitionRegisters;
        }

        #endregion
    }

}
