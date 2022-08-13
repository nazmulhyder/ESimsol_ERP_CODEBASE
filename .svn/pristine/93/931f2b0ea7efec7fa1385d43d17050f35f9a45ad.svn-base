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
    public class ExportPIRegisterService : MarshalByRefObject, IExportPIRegisterService
    {
        #region Private functions and declaration
        private ExportPIRegister MapObject(NullHandler oReader)
        {
            ExportPIRegister oExportPIRegister = new ExportPIRegister();
            
            oExportPIRegister.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPIRegister.MotherBuyerID = oReader.GetInt32("MotherBuyerID");
            oExportPIRegister.ProductNature = (EnumProductNature) oReader.GetInt32("ProductNature");
            oExportPIRegister.Amount = oReader.GetDouble("Amount");
            oExportPIRegister.ExportPINo = oReader.GetString("ExportPINo");
            oExportPIRegister.BUID = oReader.GetInt32("BUID");
            oExportPIRegister.ExportPIDate = oReader.GetDateTime("ExportPIDate");
            oExportPIRegister.BuyerID = oReader.GetInt32("BuyerID");
            oExportPIRegister.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportPIRegister.PIValue = oReader.GetDouble("PIValue");
            oExportPIRegister.DeliveryValue = oReader.GetDouble("DeliveryValue");
            oExportPIRegister.PaidByLC = oReader.GetDouble("PaidByLC");
            oExportPIRegister.DueAmount = oReader.GetDouble("DueAmount");
            oExportPIRegister.MotherBuyerName = oReader.GetString("MotherBuyerName");
            oExportPIRegister.BuyerName = oReader.GetString("BuyerName");
            oExportPIRegister.PaidByCash = oReader.GetDouble("PaidByCash");
            oExportPIRegister.CurrencyName = oReader.GetString("CurrencyName");
            oExportPIRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
                      
            return oExportPIRegister;
        }

        private ExportPIRegister CreateObject(NullHandler oReader)
        {
            ExportPIRegister oExportPIRegister = new ExportPIRegister();
            oExportPIRegister = MapObject(oReader);
            return oExportPIRegister;
        }

        private List<ExportPIRegister> CreateObjects(IDataReader oReader)
        {
            List<ExportPIRegister> oExportPIRegister = new List<ExportPIRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIRegister oItem = CreateObject(oHandler);
                oExportPIRegister.Add(oItem);
            }
            return oExportPIRegister;
        }

        #endregion

        #region Interface implementation
        public ExportPIRegisterService() { }        
        public List<ExportPIRegister> Gets(string sSQL, int nReportLayout, int nDueOptions, Int64 nUserID)
        {
            List<ExportPIRegister> oExportPIRegister = new List<ExportPIRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIRegisterDA.Gets(tc, sSQL, nReportLayout, nDueOptions);
                oExportPIRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIRegister", e);
                #endregion
            }

            return oExportPIRegister;
        }
        #endregion
    }
}
