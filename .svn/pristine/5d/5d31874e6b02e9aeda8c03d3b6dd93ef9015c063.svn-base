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

    public class DevelopementRecapMgtReportService : MarshalByRefObject, IDevelopementRecapMgtReportService
    {
        #region Private functions and declaration
        private DevelopementRecapMgtReport MapObject(NullHandler oReader)
        {
            DevelopementRecapMgtReport oDevelopementRecapMgtReport = new DevelopementRecapMgtReport();

            oDevelopementRecapMgtReport.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopementRecapMgtReport.DevelopmentRecapDetailID = oReader.GetInt32("DevelopmentRecapDetailID");
            oDevelopementRecapMgtReport.DevelopmentRecapNo = oReader.GetString("DevelopmentRecapNo");
            oDevelopementRecapMgtReport.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oDevelopementRecapMgtReport.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oDevelopementRecapMgtReport.DevelopmentType = oReader.GetInt32("DevelopmentType");
            oDevelopementRecapMgtReport.DevelopmentTypeName = oReader.GetString("DevelopmentTypeName");
            oDevelopementRecapMgtReport.InquiryReceivedDate = oReader.GetDateTime("InquiryReceivedDate");
            oDevelopementRecapMgtReport.SendingDeadLine = oReader.GetDateTime("SendingDeadLine");
            oDevelopementRecapMgtReport.BuyerID = oReader.GetInt32("BuyerID");
            oDevelopementRecapMgtReport.FactoryID = oReader.GetInt32("FactoryID");
            oDevelopementRecapMgtReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oDevelopementRecapMgtReport.StyleNo = oReader.GetString("StyleNo");
            oDevelopementRecapMgtReport.BuyerName = oReader.GetString("BuyerName");
            oDevelopementRecapMgtReport.FactoryName = oReader.GetString("FactoryName");
            oDevelopementRecapMgtReport.MerchandiserName = oReader.GetString("MerchandiserName");
            oDevelopementRecapMgtReport.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oDevelopementRecapMgtReport.SampleQty = oReader.GetDouble("SampleQty");
            oDevelopementRecapMgtReport.OrderQty = oReader.GetDouble("OrderQty");

            return oDevelopementRecapMgtReport;
        }

        private DevelopementRecapMgtReport CreateObject(NullHandler oReader)
        {
            DevelopementRecapMgtReport oDevelopementRecapMgtReport = new DevelopementRecapMgtReport();
            oDevelopementRecapMgtReport = MapObject(oReader);
            return oDevelopementRecapMgtReport;
        }

        private List<DevelopementRecapMgtReport> CreateObjects(IDataReader oReader)
        {
            List<DevelopementRecapMgtReport> oDevelopementRecapMgtReport = new List<DevelopementRecapMgtReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopementRecapMgtReport oItem = CreateObject(oHandler);
                oDevelopementRecapMgtReport.Add(oItem);
            }
            return oDevelopementRecapMgtReport;
        }

        #endregion



        #region Interface implementation


        public List<DevelopementRecapMgtReport> Gets(string sSql, int ReportFormat, Int64 nUserID)
        {
            List<DevelopementRecapMgtReport> oDevelopementRecapMgtReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopementRecapMgtReportDA.Gets(sSql,ReportFormat , tc);
                oDevelopementRecapMgtReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopementRecapMgtReport", e);
                #endregion
            }

            return oDevelopementRecapMgtReport;
        }

  
        #endregion


    }
    

}
