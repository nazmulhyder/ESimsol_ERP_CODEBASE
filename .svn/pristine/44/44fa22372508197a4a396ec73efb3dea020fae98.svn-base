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
    public class RPT_LotTrackingsService : MarshalByRefObject, IRPT_LotTrackingsService
    {
        #region Private functions and declaration
        private RPT_LotTrackings MapObject(NullHandler oReader)
        {
            RPT_LotTrackings oRPT_LotTrackings = new RPT_LotTrackings();
            oRPT_LotTrackings.LotID = oReader.GetInt32("LotID");
            oRPT_LotTrackings.LotNo = oReader.GetString("LotNo");
            oRPT_LotTrackings.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oRPT_LotTrackings.ProductCode = oReader.GetString("ProductCode");
            oRPT_LotTrackings.ProductName = oReader.GetString("ProductName");
            oRPT_LotTrackings.ContractorName = oReader.GetString("ContractorName");
            oRPT_LotTrackings.LCNo = oReader.GetString("LCNo");
            oRPT_LotTrackings.InvoiceNo = oReader.GetString("InvoiceNo");
            oRPT_LotTrackings.MUnit = oReader.GetString("MUnit");
            oRPT_LotTrackings.ProductID = oReader.GetInt32("ProductID");
            oRPT_LotTrackings.MUnitID = oReader.GetInt32("MUnitID");
            oRPT_LotTrackings.QtyGRN = oReader.GetDouble("QtyGRN");
            oRPT_LotTrackings.QtyAdjIn = oReader.GetDouble("QtyAdjIn");
            oRPT_LotTrackings.QtyAdjOut = oReader.GetDouble("QtyAdjOut");
            oRPT_LotTrackings.QtyProIn = oReader.GetDouble("QtyProIn");
            oRPT_LotTrackings.QtyProInDye = oReader.GetDouble("QtyProInDye");
            oRPT_LotTrackings.QtyProOut = oReader.GetDouble("QtyProOut");
            oRPT_LotTrackings.QtyRecycle = oReader.GetDouble("QtyRecycle");
            oRPT_LotTrackings.QtyWestage = oReader.GetDouble("QtyWestage");
            oRPT_LotTrackings.QtyDelivery = oReader.GetDouble("QtyDelivery");
            oRPT_LotTrackings.QtyReturn = oReader.GetDouble("QtyReturn");
            oRPT_LotTrackings.QtyRSCancel = oReader.GetDouble("QtyRSCancel");
            oRPT_LotTrackings.QtyShort = oReader.GetDouble("QtyShort");
            oRPT_LotTrackings.QtyProProcess = oReader.GetDouble("QtyProProcess");
            oRPT_LotTrackings.CurrentBalance = oReader.GetDouble("CurrentBalance");
            oRPT_LotTrackings.CurrentBalanceDye = oReader.GetDouble("CurrentBalanceDye");
            oRPT_LotTrackings.QtySoft = oReader.GetDouble("QtySoft");
            oRPT_LotTrackings.ParentType = oReader.GetInt32("ParentType");
            oRPT_LotTrackings.ParentLotID = oReader.GetInt32("ParentLotID");
            oRPT_LotTrackings.QtySoft_Dye = oReader.GetDouble("QtySoft_Dye");
            oRPT_LotTrackings.QtyProIn_Dye = oReader.GetDouble("QtyProIn_Dye");
            oRPT_LotTrackings.QtyRecycle_Dye = oReader.GetDouble("QtyRecycle_Dye");
            oRPT_LotTrackings.QtyWestage_Dye = oReader.GetDouble("QtyWestage_Dye");
            oRPT_LotTrackings.QtyRSCancel_Dye = oReader.GetDouble("QtyRSCancel_Dye");
            oRPT_LotTrackings.QtyDelivery_Dye = oReader.GetDouble("QtyDelivery_Dye");
            oRPT_LotTrackings.QtyReturn_Dye = oReader.GetDouble("QtyReturn_Dye");
            oRPT_LotTrackings.QtyProProcess_Dye = oReader.GetDouble("QtyProProcess_Dye");
            oRPT_LotTrackings.QtyShort_Dye = oReader.GetDouble("QtyShort_Dye");
            oRPT_LotTrackings.QtyFinish_Dye = oReader.GetDouble("QtyFinish_Dye");
            oRPT_LotTrackings.QtyTR = oReader.GetDouble("QtyTR");
            oRPT_LotTrackings.QtyFinish = oReader.GetDouble("QtyFinish");
            oRPT_LotTrackings.Recycle_Recd = oReader.GetDouble("Recycle_Recd");
            oRPT_LotTrackings.Recycle_Recd = oReader.GetDouble("Recycle_Recd");
            oRPT_LotTrackings.LotDate = oReader.GetDateTime("LotDate");
            
            return oRPT_LotTrackings;
        }

        private RPT_LotTrackings CreateObject(NullHandler oReader)
        {
            RPT_LotTrackings oRPT_LotTrackings = new RPT_LotTrackings();
            oRPT_LotTrackings = MapObject(oReader);
            return oRPT_LotTrackings;
        }

        private List<RPT_LotTrackings> CreateObjects(IDataReader oReader)
        {
            List<RPT_LotTrackings> oRPT_LotTrackings = new List<RPT_LotTrackings>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RPT_LotTrackings oItem = CreateObject(oHandler);
                oRPT_LotTrackings.Add(oItem);
            }
            return oRPT_LotTrackings;
        }

        #endregion

        #region Interface implementation
        public RPT_LotTrackingsService() { }
        public List<RPT_LotTrackings> Gets(string sSQL, int nReportType, int nBUID, Int64 nUserID)
        {
            List<RPT_LotTrackings> oRPT_LotTrackingss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPT_LotTrackingsDA.Gets(tc, sSQL, nReportType, nBUID, nUserID);
                oRPT_LotTrackingss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RPT_LotTrackings", e);
                #endregion
            }
            return oRPT_LotTrackingss;
        }
        #endregion
    }   
}

