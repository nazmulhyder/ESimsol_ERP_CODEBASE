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
    public class FabricDeliveryChallanDetailService : MarshalByRefObject, IFabricDeliveryChallanDetailService
    {
        #region Private functions and declaration
        private FabricDeliveryChallanDetail MapObject(NullHandler oReader)
        {
            FabricDeliveryChallanDetail oFDCDetail = new FabricDeliveryChallanDetail();
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
            //oFDCDetail.FabrichWidth = oReader.GetString("FinishWidth");
            oFDCDetail.Shrinkage = oReader.GetString("Shrinkage");
            oFDCDetail.Weight = oReader.GetString("Weight");
            oFDCDetail.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oFDCDetail.FabricWeave = oReader.GetString("FabricWeaveName");
            oFDCDetail.StyleNo = oReader.GetString("StyleNo");
            oFDCDetail.BuyerRef = oReader.GetString("BuyerReference");
            oFDCDetail.LotNo = oReader.GetString("LotNo");
            oFDCDetail.MUName = oReader.GetString("MUName");
            oFDCDetail.LotQty = oReader.GetDouble("LotQty");
            oFDCDetail.StockInHand = oReader.GetDouble("StockInHand");
            oFDCDetail.Qty_DO = oReader.GetDouble("Qty_DO");
            oFDCDetail.FEONo = oReader.GetString("FEONo");
            oFDCDetail.ExeNo = oReader.GetString("ExeNo");
            oFDCDetail.BuyerName = oReader.GetString("BuyerName");
            oFDCDetail.MKTPerson = oReader.GetString("MKTPerson");
            oFDCDetail.BuyerCPName = oReader.GetString("BuyerCPName");
            oFDCDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFDCDetail.LCNo = oReader.GetString("LCNo");
            oFDCDetail.PINo = oReader.GetString("PINo");
            oFDCDetail.PIDate = oReader.GetDateTime("PIDate");
            oFDCDetail.RollNo = oReader.GetString("RollNo");

            oFDCDetail.ChallanNo = oReader.GetString("ChallanNo");
            oFDCDetail.LCDate = oReader.GetDateTime("LCDate");
            oFDCDetail.ShadeID = (EnumFNShade)oReader.GetInt16("ShadeID");
            oFDCDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFDCDetail.ParentFDCID = oReader.GetInt32("ParentFDCID");
            oFDCDetail.FNBatchQCDetailID = oReader.GetInt32("FNBatchQCDetailID");
            oFDCDetail.DOPriceType = (EnumDOPriceType)oReader.GetInt32("DOPriceType");
            return oFDCDetail;
        }

        public static FabricDeliveryChallanDetail CreateObject(NullHandler oReader)
        {
            FabricDeliveryChallanDetail oFabricDeliveryChallanDetail = new FabricDeliveryChallanDetail();
            FabricDeliveryChallanDetailService oFDCDService = new FabricDeliveryChallanDetailService();
            oFabricDeliveryChallanDetail = oFDCDService.MapObject(oReader);
            return oFabricDeliveryChallanDetail;
        }

        private List<FabricDeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliveryChallanDetail> oFabricDeliveryChallanDetails = new List<FabricDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliveryChallanDetail oItem = CreateObject(oHandler);
                oFabricDeliveryChallanDetails.Add(oItem);
            }
            return oFabricDeliveryChallanDetails;
        }
        #endregion

        #region Interface implementatio
        public FabricDeliveryChallanDetailService() { }

        public FabricDeliveryChallanDetail IUD(FabricDeliveryChallanDetail oFDCDetail, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oFDCDetail.FDCID == 0)
                {

                    if (oFDCDetail.FDC != null)
                    {
                        oFDCDetail.FDC = this.SetNullable(oFDCDetail.FDC);
                        reader = FabricDeliveryChallanDA.IUD(tc, oFDCDetail.FDC, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFDC = FabricDeliveryChallanService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No fabric delivery challan found to save."); }
                    oFDCDetail.FDCID = oFDC.FDCID;
                }
                reader = FabricDeliveryChallanDetailDA.IUD(tc, oFDCDetail, nDBOperation, nUserId,"");
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDCDetail = new FabricDeliveryChallanDetail();
                    oFDCDetail = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFDCDetail = new FabricDeliveryChallanDetail();
                    oFDCDetail.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
           

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetail = new FabricDeliveryChallanDetail();
                oFDCDetail.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            oFDCDetail.FDC = oFDC;
            return oFDCDetail;
        }

        public FabricDeliveryChallanDetail Get(int nFDCDID, Int64 nUserId)
        {
            FabricDeliveryChallanDetail oFDCDetail = new FabricDeliveryChallanDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryChallanDetailDA.Get(tc, nFDCDID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDCDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetail = new FabricDeliveryChallanDetail();
                oFDCDetail.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFDCDetail;
        }
        public List<FabricDeliveryChallanDetail> Gets(int nFDCID, bool bIsSample,Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryChallanDetailDA.Gets(tc, nFDCID,bIsSample, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FabricDeliveryChallanDetail>();
                #endregion
            }

            return oFDCDetails;
        }
        public List<FabricDeliveryChallanDetail> GetsForAdj(int nContractorID, int nParentFDCID, Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryChallanDetailDA.GetsForAdj(tc,  nContractorID,  nParentFDCID, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FabricDeliveryChallanDetail>();
                #endregion
            }

            return oFDCDetails;
        }
        public FabricDeliveryChallanDetail Update_Adj(FabricDeliveryChallanDetail oFabricDeliveryChallanDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryChallanDetailDA.Update_Adj(tc, oFabricDeliveryChallanDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricDeliveryChallanDetail = new FabricDeliveryChallanDetail();
                oFabricDeliveryChallanDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryChallanDetail;
        }
        public List<FabricDeliveryChallanDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryChallanDetailDA.Gets(tc, sSQL, nUserId);
                oFDCDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCDetails = new List<FabricDeliveryChallanDetail>();
                #endregion
            }

            return oFDCDetails;
        }


        public FabricDeliveryChallanDetail SaveMultipleFDCD(FabricDeliveryChallanDetail oFDCD,  Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricDeliveryChallan oFDC = new FabricDeliveryChallan();
            List<FabricDeliveryChallanDetail> oFDCDetails = new List<FabricDeliveryChallanDetail>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                
                if (oFDCD.FabricDeliveryChallanDetails.FirstOrDefault().FDCID<=0)
                {
                    oFDC = oFDCD.FabricDeliveryChallanDetails.FirstOrDefault().FDC;

                    if (oFDC != null)
                    {

                        oFDC = this.SetNullable(oFDC);
                        reader = FabricDeliveryChallanDA.IUD(tc, oFDC, (int)EnumDBOperation.Insert, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFDC = FabricDeliveryChallanService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No fabric delivery challan found to save."); }
                }


                foreach (FabricDeliveryChallanDetail oItem in oFDCD.FabricDeliveryChallanDetails)
                {
                    FabricDeliveryChallanDetail oFDCDetail = new FabricDeliveryChallanDetail();

                    oItem.FDCID = (oItem.FDCID <= 0) ? oFDC.FDCID : oItem.FDCID;
                    reader = FabricDeliveryChallanDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId,"");
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFDCDetail = new FabricDeliveryChallanDetail();
                        oFDCDetail = CreateObject(oReader);
                    }
                    reader.Close();
                    oFDCDetails.Add(oFDCDetail);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDCD = new FabricDeliveryChallanDetail();
                oFDCD.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            oFDCD.FDC = oFDC;
            oFDCD.FabricDeliveryChallanDetails = oFDCDetails;
            return oFDCD;
        }


        private FabricDeliveryChallan SetNullable(FabricDeliveryChallan oFDC)
        {
            
            oFDC.DeliveryPoint = oFDC.DeliveryPoint ?? "";
            oFDC.VehicleNo = oFDC.VehicleNo ?? "";
            oFDC.DriverName = oFDC.DriverName ?? "";
            oFDC.Note = oFDC.Note ?? "";
            return oFDC;
        }
        #endregion
    }
}