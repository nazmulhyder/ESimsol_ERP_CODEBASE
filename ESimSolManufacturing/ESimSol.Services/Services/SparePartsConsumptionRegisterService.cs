using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class SparePartsConsumptionRegisterService : MarshalByRefObject, ISparePartsConsumptionRegisterService
    {
        #region Private functions and declaration
        private SparePartsConsumptionRegister MapObject(NullHandler oReader)
        {
            SparePartsConsumptionRegister oSPCRegister = new SparePartsConsumptionRegister();
            oSPCRegister.SparePartsChallanDetailID = oReader.GetInt32("SparePartsChallanDetailID");
            oSPCRegister.SparePartsChallanID = oReader.GetInt32("SparePartsChallanID");
            oSPCRegister.SparePartsRequisitionDetailID = oReader.GetInt32("SparePartsRequisitionDetailID");
            oSPCRegister.ProductID = oReader.GetInt32("ProductID");
            oSPCRegister.LotID = oReader.GetInt32("LotID");
            oSPCRegister.MUnitID = oReader.GetInt32("MUnitID");
            oSPCRegister.ConsumptionQty = oReader.GetInt32("ConsumptionQty");
            oSPCRegister.UnitPrice = oReader.GetInt32("UnitPrice");
            oSPCRegister.ChallanBy = oReader.GetInt32("ChallanBy");
            oSPCRegister.StoreID = oReader.GetInt32("StoreID");
            oSPCRegister.CRID = oReader.GetInt32("CRID");
            oSPCRegister.RequisitionBy = oReader.GetInt32("RequisitionBy");
            oSPCRegister.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oSPCRegister.ResourceType = oReader.GetInt32("ResourceType");
            oSPCRegister.SparePartsRequisitionID = oReader.GetInt32("SparePartsRequisitionID");
            oSPCRegister.BUID = oReader.GetInt32("BUID");
            oSPCRegister.ProductName = oReader.GetString("ProductName");
            oSPCRegister.ProductCode = oReader.GetString("ProductCode");
            oSPCRegister.LotNo = oReader.GetString("LotNo");
            oSPCRegister.LocationName = oReader.GetString("LocationName");
            oSPCRegister.Currency = oReader.GetString("Currency");
            oSPCRegister.MUnitName = oReader.GetString("MUnitName");
            oSPCRegister.Remarks = oReader.GetString("Remarks");
            oSPCRegister.ChallanByName = oReader.GetString("ChallanByName");
            oSPCRegister.ChallanNo = oReader.GetString("ChallanNo");
            oSPCRegister.RequisitionByName = oReader.GetString("RequisitionByName");
            oSPCRegister.RequisitionNo = oReader.GetString("RequisitionNo");
            oSPCRegister.ApprovedByName = oReader.GetString("ApprovedByName");
            oSPCRegister.StoreName = oReader.GetString("StoreName");
            oSPCRegister.CRCode = oReader.GetString("CRCode");
            oSPCRegister.CRName = oReader.GetString("CRName");
            oSPCRegister.CRModel = oReader.GetString("CRModel");
            oSPCRegister.CRBrand = oReader.GetString("CRBrand");
            oSPCRegister.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oSPCRegister.ChallanDate = oReader.GetDateTime("ChallanDate");
            oSPCRegister.IssueDate = oReader.GetDateTime("IssueDate");
            oSPCRegister.ResourceTypeName = oReader.GetString("ResourceTypeName");
            oSPCRegister.BUShortName = oReader.GetString("BUShortName");
            oSPCRegister.Amount = oReader.GetDouble("Amount");
            return oSPCRegister;
        }

        private SparePartsConsumptionRegister CreateObject(NullHandler oReader)
        {
            SparePartsConsumptionRegister oSparePartsConsumptionRegister = new SparePartsConsumptionRegister();
            oSparePartsConsumptionRegister = MapObject(oReader);
            return oSparePartsConsumptionRegister;
        }

        private List<SparePartsConsumptionRegister> CreateObjects(IDataReader oReader)
        {
            List<SparePartsConsumptionRegister> oSparePartsConsumptionRegister = new List<SparePartsConsumptionRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SparePartsConsumptionRegister oItem = CreateObject(oHandler);
                oSparePartsConsumptionRegister.Add(oItem);
            }
            return oSparePartsConsumptionRegister;
        }

        #endregion

        #region Interface implementation
        public SparePartsConsumptionRegisterService() { }
        public List<SparePartsConsumptionRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<SparePartsConsumptionRegister> oSparePartsConsumptionRegisters = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SparePartsConsumptionRegisterDA.Gets(tc, sSQL);
                oSparePartsConsumptionRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SparePartsConsumptionRegister", e);
                #endregion
            }
            return oSparePartsConsumptionRegisters;
        }
        #endregion
    }
}