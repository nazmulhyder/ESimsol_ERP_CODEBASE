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
    public class DevelopmentRecapSummaryService : MarshalByRefObject, IDevelopmentRecapSummaryService
    {
        #region Private functions and declaration
        private DevelopmentRecapSummary MapObject(NullHandler oReader)
        {
            DevelopmentRecapSummary oDevelopmentRecapSummary = new DevelopmentRecapSummary();
            oDevelopmentRecapSummary.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopmentRecapSummary.DevelopmentRecapNo = oReader.GetString("DevelopmentRecapNo");
            oDevelopmentRecapSummary.SessionName = oReader.GetString("SessionName");
            oDevelopmentRecapSummary.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oDevelopmentRecapSummary.DevelopmentStatus = (EnumDevelopmentStatus)oReader.GetInt32("DevelopmentStatus");
            oDevelopmentRecapSummary.InquiryReceivedDate = oReader.GetDateTime("InquiryReceivedDate");
            oDevelopmentRecapSummary.SampleQty = oReader.GetInt32("SampleQty");
            oDevelopmentRecapSummary.UnitPrice = oReader.GetDouble("UnitPrice");
            oDevelopmentRecapSummary.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oDevelopmentRecapSummary.SampleSizeID = oReader.GetInt32("SampleSizeID");
            oDevelopmentRecapSummary.SampleReceivedDate = oReader.GetDateTime("SampleReceivedDate");
            oDevelopmentRecapSummary.SampleSendingDate = oReader.GetDateTime("SampleSendingDate");
            oDevelopmentRecapSummary.SendingDeadLine = oReader.GetDateTime("SendingDeadLine");
            oDevelopmentRecapSummary.AwbNo = oReader.GetString("AwbNo");
            oDevelopmentRecapSummary.GG = oReader.GetString("GG");
            oDevelopmentRecapSummary.SpecialFinish = oReader.GetString("SpecialFinish");
            oDevelopmentRecapSummary.Count = oReader.GetString("Count");
            oDevelopmentRecapSummary.Weight = oReader.GetString("Weight");
            oDevelopmentRecapSummary.Remarks = oReader.GetString("Remarks");
            oDevelopmentRecapSummary.StyleNo = oReader.GetString("StyleNo");
            oDevelopmentRecapSummary.BuyerID = oReader.GetInt32("BuyerID");
            oDevelopmentRecapSummary.SampleSize = oReader.GetString("SampleSize");
            oDevelopmentRecapSummary.Fabrication = oReader.GetString("Fabrication");
            oDevelopmentRecapSummary.BuyerName = oReader.GetString("BuyerName");
            oDevelopmentRecapSummary.BrandName = oReader.GetString("BrandName");
            oDevelopmentRecapSummary.PrepareBy = oReader.GetString("PrepareBy");
            oDevelopmentRecapSummary.ApprovedByName = oReader.GetString("ApprovedByName");
            oDevelopmentRecapSummary.CollectionName = oReader.GetString("CollectionName");
            oDevelopmentRecapSummary.StyleDescription = oReader.GetString("StyleDescription");
            oDevelopmentRecapSummary.KnittingPattern = oReader.GetInt32("KnittingPattern");
            oDevelopmentRecapSummary.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oDevelopmentRecapSummary.ColorRange = oReader.GetString("ColorRange");
            oDevelopmentRecapSummary.MerchandiserName = oReader.GetString("MerchandiserName");
            oDevelopmentRecapSummary.FactoryName = oReader.GetString("FactoryName");
            oDevelopmentRecapSummary.RowNumber = oReader.GetInt32("RowNumber");
            oDevelopmentRecapSummary.MaxRowNumber = oReader.GetInt32("MaxRowNumber");
            oDevelopmentRecapSummary.DevelopmentType =oReader.GetInt32("DevelopmentType");
            oDevelopmentRecapSummary.DevelopmentTypeName = oReader.GetString("DevelopmentTypeName");
            return oDevelopmentRecapSummary;
        }

        private DevelopmentRecapSummary CreateObject(NullHandler oReader)
        {
            DevelopmentRecapSummary oDevelopmentRecapSummary = new DevelopmentRecapSummary();
            oDevelopmentRecapSummary = MapObject(oReader);
            return oDevelopmentRecapSummary;
        }

        private List<DevelopmentRecapSummary> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentRecapSummary> oDevelopmentRecapSummary = new List<DevelopmentRecapSummary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentRecapSummary oItem = CreateObject(oHandler);
                oDevelopmentRecapSummary.Add(oItem);
            }
            return oDevelopmentRecapSummary;
        }

        #endregion

        #region Interface implementation
        public DevelopmentRecapSummaryService() { }        
        public List<DevelopmentRecapSummary> GetsRecapWithDevelopmentRecapSummarys(int nStartRow, int nEndRow, string SQL, string sDevelopmentRecapSummaryIDs, bool bIsPrint, Int64 nUserID)
        {
            List<DevelopmentRecapSummary> oDevelopmentRecapSummary = new List<DevelopmentRecapSummary>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapSummaryDA.GetsRecapWithOrderRecapSummerys(tc, nStartRow, nEndRow, SQL, sDevelopmentRecapSummaryIDs, bIsPrint, nUserID);
                oDevelopmentRecapSummary = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapSummary", e);
                #endregion
            }

            return oDevelopmentRecapSummary;
        }
        #endregion
    }
}
