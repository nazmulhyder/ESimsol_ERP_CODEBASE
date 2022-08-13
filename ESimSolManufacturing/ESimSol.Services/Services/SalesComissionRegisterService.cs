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
    public class SalesComissionRegisterService : MarshalByRefObject, ISalesComissionRegisterService
    {
        #region Private functions and declaration
        private SalesComissionRegister MapObject(NullHandler oReader)
        {
            SalesComissionRegister oSalesComissionRegister = new SalesComissionRegister();
            oSalesComissionRegister.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oSalesComissionRegister.ExportPIID = oReader.GetInt32("ExportPIID");
            oSalesComissionRegister.ProductID = oReader.GetInt32("ProductID");
            oSalesComissionRegister.MUnitID = oReader.GetInt32("MUnitID");
            oSalesComissionRegister.Qty = oReader.GetDouble("Qty");
            oSalesComissionRegister.Amount = oReader.GetDouble("Amount");
            oSalesComissionRegister.StyleNo = oReader.GetString("StyleNo");
            oSalesComissionRegister.BUID = oReader.GetInt32("BUID");
            oSalesComissionRegister.IssueDate = oReader.GetDateTime("IssueDate");
            oSalesComissionRegister.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oSalesComissionRegister.MotherBuyerID = oReader.GetInt32("MotherBuyerID");
            oSalesComissionRegister.BuyerID = oReader.GetInt32("BuyerID");
            oSalesComissionRegister.OrderSheetDetailID = oReader.GetInt32("OrderSheetDetailID");
            oSalesComissionRegister.ModelReferenceID = oReader.GetInt32("ModelReferenceID");
            oSalesComissionRegister.ApproveBy = oReader.GetInt32("ApproveBy");
            oSalesComissionRegister.PIDateMonth = oReader.GetDateTime("PIDateMonth");
            oSalesComissionRegister.LCOpenDateMonth = oReader.GetDateTime("LCOpenDateMonth");
            oSalesComissionRegister.Description = oReader.GetString("Description");
            oSalesComissionRegister.BuyerReference = oReader.GetString("BuyerReference");
            oSalesComissionRegister.ProductDescription = oReader.GetString("ProductDescription");
            oSalesComissionRegister.ProductCode = oReader.GetString("ProductCode");
            oSalesComissionRegister.ProductName = oReader.GetString("ProductName");
            oSalesComissionRegister.MUName = oReader.GetString("MUName");
            oSalesComissionRegister.MUSymbol = oReader.GetString("MUSymbol");
            oSalesComissionRegister.BuyerName = oReader.GetString("BuyerName");
            oSalesComissionRegister.MotherBuyerName = oReader.GetString("MotherBuyerName");
            oSalesComissionRegister.Measurement = oReader.GetString("Measurement");
            oSalesComissionRegister.ColorInfo = oReader.GetString("ColorInfo");
            oSalesComissionRegister.CRate = oReader.GetDouble("CRate");
            oSalesComissionRegister.PINo = oReader.GetString("PINo");
            oSalesComissionRegister.ExportLCNo = oReader.GetString("ExportLCNo");
            oSalesComissionRegister.ChallanInfo = oReader.GetString("ChallanInfo");
            oSalesComissionRegister.Balance = oReader.GetDouble("Balance");
            oSalesComissionRegister.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            oSalesComissionRegister.LCValue = oReader.GetDouble("LCValue");
            oSalesComissionRegister.UnitPrice = oReader.GetDouble("UnitPrice");
            oSalesComissionRegister.ExportLCID = oReader.GetInt32("ExportLCID");
            oSalesComissionRegister.RateUnit = oReader.GetInt32("RateUnit");
            return oSalesComissionRegister;
        }

        private SalesComissionRegister CreateObject(NullHandler oReader)
        {
            SalesComissionRegister oSalesComissionRegister = new SalesComissionRegister();
            oSalesComissionRegister = MapObject(oReader);
            return oSalesComissionRegister;
        }

        private List<SalesComissionRegister> CreateObjects(IDataReader oReader)
        {
            List<SalesComissionRegister> oSalesComissionRegister = new List<SalesComissionRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalesComissionRegister oItem = CreateObject(oHandler);
                oSalesComissionRegister.Add(oItem);
            }
            return oSalesComissionRegister;
        }

        #endregion

        #region Interface implementation
        public SalesComissionRegisterService() { }        
        public List<SalesComissionRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<SalesComissionRegister> oSalesComissionRegister = new List<SalesComissionRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalesComissionRegisterDA.Gets(tc, sSQL);
                oSalesComissionRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalesComissionRegister", e);
                #endregion
            }

            return oSalesComissionRegister;
        }
        #endregion
    }
}
