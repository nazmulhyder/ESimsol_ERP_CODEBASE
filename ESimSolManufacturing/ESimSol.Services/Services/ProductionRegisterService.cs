using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class ProductionRegisterService : MarshalByRefObject, IProductionRegisterService
    {
        #region Private functions and declaration
        private ProductionRegister MapObject(NullHandler oReader)
        {
            ProductionRegister oProductionRegister = new ProductionRegister();
            oProductionRegister.ProductionSheetID = oReader.GetInt32("ProductionSheetID");

            oProductionRegister.PETransactionID = oReader.GetInt32("PETransactionID");
            oProductionRegister.ShiftID = oReader.GetInt32("ShiftID");
            oProductionRegister.MachineID = oReader.GetInt32("MachineID");
            oProductionRegister.ShiftName = oReader.GetString("ShiftName");
            oProductionRegister.MachineName = oReader.GetString("MachineName");
            oProductionRegister.TransactionDate = oReader.GetDateTime("TransactionDate");
            oProductionRegister.SheetNo = oReader.GetString("SheetNo");
            oProductionRegister.UnitSymbol = oReader.GetString("UnitSymbol");

            oProductionRegister.ProductionExecutionID = oReader.GetInt32("ProductionExecutionID");
            oProductionRegister.ProductID = oReader.GetInt32("ProductID");
            oProductionRegister.ProductCode = oReader.GetString("ProductCode");
            oProductionRegister.ProductName = oReader.GetString("ProductName");
            oProductionRegister.CustomerName = oReader.GetString("CustomerName");
            oProductionRegister.BuyerName = oReader.GetString("BuyerName");
            oProductionRegister.ExportPINo = oReader.GetString("ExportPINo");

            oProductionRegister.SheetQty = oReader.GetDouble("SheetQty");
            oProductionRegister.MoldingProduction = oReader.GetDouble("MoldingProduction");
            oProductionRegister.ActualMoldingProduction = oReader.GetDouble("ActualMoldingProduction");
            oProductionRegister.ActualRejectGoods = oReader.GetDouble("ActualRejectGoods");
            oProductionRegister.ActualFinishGoods = oReader.GetDouble("ActualFinishGoods");
            oProductionRegister.YetToProduction = oReader.GetDouble("YetToProduction");
            
            return oProductionRegister;
        }

        private ProductionRegister CreateObject(NullHandler oReader)
        {
            ProductionRegister oProductionRegister = new ProductionRegister();
            oProductionRegister = MapObject(oReader);
            return oProductionRegister;
        }

        private List<ProductionRegister> CreateObjects(IDataReader oReader)
        {
            List<ProductionRegister> oProductionRegister = new List<ProductionRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionRegister oItem = CreateObject(oHandler);
                oProductionRegister.Add(oItem);
            }
            return oProductionRegister;
        }

        #endregion

        #region Interface implementation
        public ProductionRegisterService() { }
        public List<ProductionRegister> Gets(string sSQL, EnumReportLayout eEnumReportLayout, int nUserID)
        {
            List<ProductionRegister> oProductionRegisters = new List<ProductionRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductionRegisterDA.Gets(tc, eEnumReportLayout, sSQL);
                oProductionRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionRegister", e);
                #endregion
            }

            return oProductionRegisters;
        }
        #endregion
    }   
    
   
}
