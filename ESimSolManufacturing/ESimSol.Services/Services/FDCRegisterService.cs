using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class FDCRegisterService : MarshalByRefObject, IFDCRegisterService
    {
        #region Private functions and declaration
        private FDCRegister MapObject(NullHandler oReader)
        {
            FDCRegister oFDCDetail = new FDCRegister();
            oFDCDetail.FDCDID = oReader.GetInt32("FDCDID");
            oFDCDetail.FDCID = oReader.GetInt32("FDCID");
            oFDCDetail.FDODID = oReader.GetInt32("FDODID");
            oFDCDetail.LotID = oReader.GetInt32("LotID");
            oFDCDetail.MUID = oReader.GetInt32("MUID");
            oFDCDetail.Qty = oReader.GetDouble("Qty");
            oFDCDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFDCDetail.FabricID = oReader.GetInt32("FabricID");
            oFDCDetail.FabricNo = oReader.GetString("FabricNo");
            oFDCDetail.Construction = oReader.GetString("Construction");
            oFDCDetail.ProductID = oReader.GetInt32("ProductID");
            oFDCDetail.ProductCode = oReader.GetString("ProductCode");
            oFDCDetail.ProductName = oReader.GetString("ProductName");
            oFDCDetail.ColorInfo = oReader.GetString("ColorInfo");
            oFDCDetail.FabricDesignName = oReader.GetString("FabricDesignName");
            oFDCDetail.FinishTypeName = oReader.GetString("FinishTypeName");
            oFDCDetail.FinishWidth = oReader.GetString("FabricWidth");
            oFDCDetail.VehicleNo = oReader.GetString("VehicleNo");
            oFDCDetail.Shrinkage = oReader.GetString("Shrinkage");
            oFDCDetail.Weight = oReader.GetString("Weight");
            oFDCDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFDCDetail.FabricWeave = oReader.GetString("FabricWeaveName");
            oFDCDetail.StyleNo = oReader.GetString("StyleNo");
            oFDCDetail.BuyerRef = oReader.GetString("BuyerRef");
            oFDCDetail.LotNo = oReader.GetString("LotNo");
            oFDCDetail.MUName = oReader.GetString("MUnit");
            oFDCDetail.LotQty = oReader.GetDouble("LotQty");
            oFDCDetail.StockInHand = oReader.GetDouble("StockInHand");
            oFDCDetail.Qty_DO = oReader.GetDouble("Qty_DO");
            oFDCDetail.Qty_DC = oReader.GetDouble("Qty_DC");
            oFDCDetail.QtyOrder = oReader.GetDouble("QtyOrder");
            oFDCDetail.QtyDelivery = oReader.GetDouble("QtyDelivery");
            oFDCDetail.FEONo = oReader.GetString("FEONo");
            oFDCDetail.ExeNo = oReader.GetString("ExeNo");
            oFDCDetail.BuyerName = oReader.GetString("BuyerName");
            oFDCDetail.MKTPerson = oReader.GetString("MKTPerson");
            oFDCDetail.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFDCDetail.BuyerCPName = oReader.GetString("BuyerCPName");
            oFDCDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFDCDetail.LCNo = oReader.GetString("LCNo");
            oFDCDetail.PINo = oReader.GetString("PINo");
            oFDCDetail.PIDate = oReader.GetDateTime("PIDate");
            oFDCDetail.LCNo = oReader.GetString("LCNo");
            oFDCDetail.LCDate = oReader.GetDateTime("LCDate");
            oFDCDetail.ShadeID = (EnumFNShade)oReader.GetInt16("ShadeID");
            oFDCDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFDCDetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oFDCDetail.ParentFDCID = oReader.GetInt32("ParentFDCID");
            oFDCDetail.DisburseByName = oReader.GetString("DisburseByName");
            oFDCDetail.ChallanRemarks = oReader.GetString("ChallanRemarks");
            oFDCDetail.ContractorID = oReader.GetInt32("ContractorID");
            oFDCDetail.BuyerID = oReader.GetInt32("BuyerID");
            oFDCDetail.FEOID = oReader.GetInt32("FEOID");
            oFDCDetail.IsDelivered = oReader.GetBoolean("IsDelivered");
            oFDCDetail.IsDelivered = oReader.GetBoolean("IsDelivered");
            oFDCDetail.FDODate = oReader.GetDateTime("DODate");
            oFDCDetail.ContractorName = oReader.GetString("ContractorName");
            oFDCDetail.SCNoFull = oReader.GetString("SCNoFull");
            oFDCDetail.DOType = (EnumDOType)oReader.GetInt16("FDOType");
            oFDCDetail.ApproveBy = oReader.GetInt32("ApproveBy");
            oFDCDetail.DisburseBy = oReader.GetInt32("DisburseBy");
            oFDCDetail.DONo = oReader.GetString("DONo");
            oFDCDetail.StoreName = oReader.GetString("StoreName");
            oFDCDetail.DriverName = oReader.GetString("DriverName");
            oFDCDetail.ChallanNo = oReader.GetString("DCNo");
            oFDCDetail.ChallanDate = oReader.GetDateTime("DCDate");
            oFDCDetail.DeliveryPoient = oReader.GetString("DeliveryPoint");

            oFDCDetail.FDOID = oReader.GetInt32("FDOID");
            oFDCDetail.FDOType = oReader.GetInt32("FDOType");
            oFDCDetail.DCNo = oReader.GetString("DCNo");
            oFDCDetail.DCDate = oReader.GetDateTime("DCDate");
            oFDCDetail.DODate = oReader.GetDateTime("DODate");
            oFDCDetail.SampleQty = oReader.GetDouble("SampleQty");
            oFDCDetail.BCPID = oReader.GetInt32("BCPID");
            oFDCDetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oFDCDetail.BuyerReference = oReader.GetString("BuyerReference");
            oFDCDetail.FinishType = oReader.GetInt32("FinishType");
            oFDCDetail.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFDCDetail.ProcessType = oReader.GetInt32("ProcessType");
            oFDCDetail.FSCID = oReader.GetInt32("FSCID");
            oFDCDetail.MUnit = oReader.GetString("MUnit");
            oFDCDetail.IsPrint = oReader.GetBoolean("IsPrint");
            oFDCDetail.IsYD = oReader.GetBoolean("IsYD");
            
            return oFDCDetail;
        }

        public static FDCRegister CreateObject(NullHandler oReader)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            FDCRegisterService oFDCDService = new FDCRegisterService();
            oFDCRegister = oFDCDService.MapObject(oReader);
            return oFDCRegister;
        }

        private List<FDCRegister> CreateObjects(IDataReader oReader)
        {
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FDCRegister oItem = CreateObject(oHandler);
                oFDCRegisters.Add(oItem);
            }
            return oFDCRegisters;
        }
        #endregion

        #region Interface implementatio
        public List<FDCRegister> Gets(string sSQL, Int64 nUserId)
        {
            List<FDCRegister> oFDCDetails = new List<FDCRegister>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDCRegisterDA.Gets(tc, sSQL, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FDCRegister>();
                #endregion
            }

            return oFDCDetails;
        }
        public List<FDCRegister> Gets_FDO(string sSQL, Int64 nUserId)
        {
            List<FDCRegister> oFDCDetails = new List<FDCRegister>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDCRegisterDA.Gets_FDO(tc, sSQL, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FDCRegister>();
                #endregion
            }

            return oFDCDetails;
        }
        public List<FDCRegister> Gets_FDC(string sSQL, Int64 nUserId)
        {
            List<FDCRegister> oFDCDetails = new List<FDCRegister>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDCRegisterDA.Gets_FDC(tc, sSQL, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FDCRegister>();
                #endregion
            }

            return oFDCDetails;
        }

        public List<FDCRegister> Gets_For_FDC2(string sSQL, Int64 nUserId)
        {
            List<FDCRegister> oFDCDetails = new List<FDCRegister>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FDCRegisterDA.Gets_For_FDC2(tc, sSQL, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FDCRegister>();
                #endregion
            }

            return oFDCDetails;
        }

        #endregion 
    }
}