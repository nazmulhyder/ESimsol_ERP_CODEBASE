using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ExportPILCMappingDetailService : MarshalByRefObject, IExportPILCMappingDetailService
    {
        #region Private functions and declaration
        private ExportPILCMappingDetail MapObject(NullHandler oReader)
        {
            ExportPILCMappingDetail oExportPILCMappingDetail = new ExportPILCMappingDetail();
            oExportPILCMappingDetail.ExportPILCMappingID = oReader.GetInt32("ExportPILCMappingID");
            oExportPILCMappingDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPILCMappingDetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oExportPILCMappingDetail.ProductID = oReader.GetInt32("ProductID");
            oExportPILCMappingDetail.Qty = oReader.GetDouble("Qty");
            oExportPILCMappingDetail.Qty_Delivery = oReader.GetDouble("Qty_Delivery");
            oExportPILCMappingDetail.Qty_Invoice = oReader.GetDouble("Qty_Invoice");
            oExportPILCMappingDetail.RateUnit = oReader.GetInt32("RateUnit");
            oExportPILCMappingDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oExportPILCMappingDetail.MUnitID = oReader.GetInt32("MUnitID");
            //Derived 
            oExportPILCMappingDetail.EBillQty = oReader.GetDouble("EBillQty");
            oExportPILCMappingDetail.MUName = oReader.GetString("MUName");
            oExportPILCMappingDetail.ProductCode = oReader.GetString("ProductCode");
            oExportPILCMappingDetail.ProductName = oReader.GetString("ProductName");
            //oExportPILCMappingDetail.PICode = oReader.GetString("PICode");
            oExportPILCMappingDetail.PINo = oReader.GetString("PINo");
            oExportPILCMappingDetail.StyleNo = oReader.GetString("StyleNo");
            /// Fabric
            oExportPILCMappingDetail.IsDeduct = oReader.GetBoolean("IsDeduct");
            oExportPILCMappingDetail.Construction = oReader.GetString("Construction");
            oExportPILCMappingDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oExportPILCMappingDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oExportPILCMappingDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oExportPILCMappingDetail.FabricWidth = oReader.GetString("FabricWidth");
            oExportPILCMappingDetail.ColorInfo = oReader.GetString("ColorInfo");
            oExportPILCMappingDetail.StyleRef = oReader.GetString("StyleRef");
            oExportPILCMappingDetail.FabricNo = oReader.GetString("FabricNo");
           
            return oExportPILCMappingDetail;
        }

    

        private ExportPILCMappingDetail CreateObject(NullHandler oReader)
        {
            ExportPILCMappingDetail oExportPILCMappingDetail = new ExportPILCMappingDetail();
            oExportPILCMappingDetail = MapObject(oReader);
            return oExportPILCMappingDetail;
        }
      
        private List<ExportPILCMappingDetail> CreateObjects(IDataReader oReader)
        {
            List<ExportPILCMappingDetail> oExportPILCMappingDetails = new List<ExportPILCMappingDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPILCMappingDetail oItem = CreateObject(oHandler);
                oExportPILCMappingDetails.Add(oItem);
            }
            return oExportPILCMappingDetails;
        }
       

        #endregion

        #region Interface implementation
        public ExportPILCMappingDetailService() { }



        public List<ExportPILCMappingDetail> GetsBy(int nExportLCID, Int64 nUserId)
        {
            List<ExportPILCMappingDetail> oExportPILCMappingDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDetailDA.GetsBy(tc, nExportLCID);
                oExportPILCMappingDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PILCMappingDetail", e);
                #endregion
            }

            return oExportPILCMappingDetail;
        }

        public List<ExportPILCMappingDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPILCMappingDetail> oExportPILCMappingDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPILCMappingDetailDA.Gets(tc, sSQL);
                oExportPILCMappingDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPILCMappingDetails", e);
                #endregion
            }

            return oExportPILCMappingDetails;
        }
        #endregion
    }
}
