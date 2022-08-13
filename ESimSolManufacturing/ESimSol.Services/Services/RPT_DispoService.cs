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
    public class RPT_DispoService : MarshalByRefObject, IRPT_DispoService
    {
        #region Private functions and declaration
        private RPT_Dispo MapObject(NullHandler oReader)
        {
            RPT_Dispo oRPT_Dispo = new RPT_Dispo();
            oRPT_Dispo.ExeNo = oReader.GetString("ExeNo");
            oRPT_Dispo.ExeDate = oReader.GetDateTime("ExeDate");
            oRPT_Dispo.DODate = oReader.GetDateTime("DODate");
            oRPT_Dispo.ReviseDate = oReader.GetDateTime("ReviseDate");
            oRPT_Dispo.SCNoFull = oReader.GetString("SCNoFull");
            oRPT_Dispo.Qty_Dispo = oReader.GetDouble("Qty_Dispo");
            oRPT_Dispo.Qty_Order = oReader.GetDouble("Qty_Order");
            oRPT_Dispo.BuyerName = oReader.GetString("BuyerName");
            oRPT_Dispo.ContractorName = oReader.GetString("ContractorName");
            oRPT_Dispo.FinishTypeName = oReader.GetString("FinishTypeName");
            oRPT_Dispo.FinishDesign = oReader.GetString("FinishDesign");
            oRPT_Dispo.ProcessTypeName = oReader.GetString("ProcessTypeName");
            oRPT_Dispo.ProcessType = oReader.GetInt32("ProcessType");
            oRPT_Dispo.FabricWeave = oReader.GetInt32("FabricWeave");
            oRPT_Dispo.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oRPT_Dispo.Construction = oReader.GetString("Construction");
            oRPT_Dispo.YarnType = oReader.GetString("YarnType");
            oRPT_Dispo.Req_GreyYarn = oReader.GetDouble("Req_GreyYarn");
            oRPT_Dispo.GreyYarnReqWarp = oReader.GetDouble("GreyYarnReqWarp");
            oRPT_Dispo.DyedYarnReqWarp = oReader.GetDouble("DyedYarnReqWarp");
            oRPT_Dispo.YarnPriceWarp = oReader.GetDouble("YarnPriceWarp");
            oRPT_Dispo.GreyYarnReqWeft = oReader.GetDouble("GreyYarnReqWeft");
            oRPT_Dispo.DyedYarnReqWeft = oReader.GetDouble("DyedYarnReqWeft");
            oRPT_Dispo.ValueGreyYarn = oReader.GetDouble("ValueGreyYarn");
            oRPT_Dispo.YarnPriceWeft = oReader.GetDouble("YarnPriceWeft");
            oRPT_Dispo.ReqDyedYarn = oReader.GetDouble("ReqDyedYarn");
            oRPT_Dispo.ReqDyedYarnPro = oReader.GetDouble("ReqDyedYarnPro");
            oRPT_Dispo.ReqGreyFabrics = oReader.GetDouble("ReqGreyFabrics");
            oRPT_Dispo.GreyProductionActual = oReader.GetDouble("GreyProductionActual");
            oRPT_Dispo.ReqFinishedFabrics = oReader.GetDouble("ReqFinishedFabrics");
            oRPT_Dispo.ActualFinishfabrics = oReader.GetDouble("ActualFinishfabrics");
            oRPT_Dispo.PIRate = oReader.GetDouble("PIRate");
            oRPT_Dispo.YDChemicalValue = oReader.GetDouble("YDChemicalValue");
            oRPT_Dispo.YDDyesValue = oReader.GetDouble("YDDyesValue");
            oRPT_Dispo.YDYarnValue = oReader.GetDouble("YDYarnValue");
            oRPT_Dispo.PrintingDCValue = oReader.GetDouble("PrintingDCValue");
            oRPT_Dispo.SizingChemicalVal = oReader.GetDouble("SizingChemicalVal");
            oRPT_Dispo.FinishingChemicalVal = oReader.GetDouble("FinishingChemicalVal");
            oRPT_Dispo.ProdtionType = (EnumDispoProType)oReader.GetInt32("ProdtionType");
            oRPT_Dispo.DOQty = oReader.GetDouble("DOQty");
            oRPT_Dispo.DCQty = oReader.GetDouble("DCQty");
            oRPT_Dispo.ShortExcessPro = oReader.GetDouble("ShortExcessPro");
            oRPT_Dispo.IsPrint = oReader.GetBoolean("IsPrint");
            oRPT_Dispo.SampleQty = oReader.GetDouble("SampleQty");
            oRPT_Dispo.Qty = oReader.GetDouble("Qty");
            //oRPT_Dispo.FDOType = oReader.GetDouble("FDOType");
            oRPT_Dispo.DCNo = oReader.GetString("DCNo");
            oRPT_Dispo.DONo = oReader.GetString("DONo");
            oRPT_Dispo.DispoRef = oReader.GetString("DispoRef");
            oRPT_Dispo.ContractorID = oReader.GetInt32("ContractorID");
            oRPT_Dispo.BCPID = oReader.GetInt32("BCPID");
            oRPT_Dispo.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oRPT_Dispo.MKTPersonName = oReader.GetString("MKTPersonName");
            oRPT_Dispo.FabricID = oReader.GetInt32("FabricID");
            oRPT_Dispo.MUID = oReader.GetInt32("MUID");
            oRPT_Dispo.UnitPrice = oReader.GetDouble("UnitPrice");

            oRPT_Dispo.WarpCount = oReader.GetString("WarpCount");
            oRPT_Dispo.WeftCount = oReader.GetString("WeftCount");
            oRPT_Dispo.TotalEnds = oReader.GetDouble("TotalEnds");
            oRPT_Dispo.ProductName = oReader.GetString("ProductName");
            oRPT_Dispo.FabricNo = oReader.GetString("FabricNo");
            oRPT_Dispo.ColorInfo = oReader.GetString("ColorInfo");
            oRPT_Dispo.FabricWidth = oReader.GetString("FabricWidth");
            oRPT_Dispo.BuyerReference = oReader.GetString("BuyerReference");
            oRPT_Dispo.StyleNo = oReader.GetString("StyleNo");
            oRPT_Dispo.PINo = oReader.GetString("PINo");
            oRPT_Dispo.LCNo = oReader.GetString("LCNo");
            oRPT_Dispo.ProductID = oReader.GetInt32("ProductID");
            oRPT_Dispo.FinishType = oReader.GetInt32("FinishType");
            oRPT_Dispo.BuyerID = oReader.GetInt32("BuyerID");
            oRPT_Dispo.FSCID = oReader.GetInt32("FSCID");
            oRPT_Dispo.FSCDID = oReader.GetInt32("FSCDetailID");
            oRPT_Dispo.FEOSID = oReader.GetInt32("FEOSID");
            oRPT_Dispo.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oRPT_Dispo.MUnit = oReader.GetString("MUnit");
            oRPT_Dispo.Note = oReader.GetString("Note");
            oRPT_Dispo.FabricTypeName = oReader.GetString("FabrcTypeName");
            //// For Weaving
            oRPT_Dispo.ColorWarp = oReader.GetInt32("ColorWarp");
            oRPT_Dispo.ColorWeft = oReader.GetInt32("ColorWeft");
            oRPT_Dispo.WarpLength = oReader.GetDouble("RequiredWarpLength");
            oRPT_Dispo.WarpLengthRecd = oReader.GetDouble("WarpLengthRecd");
            
            oRPT_Dispo.ReedNo = oReader.GetDouble("ReedNo");
            oRPT_Dispo.Dent = oReader.GetDouble("Dent");
            oRPT_Dispo.REEDWidth = oReader.GetDouble("REEDWidth");

            /* FOR DISPO WISE REPORT*/
            oRPT_Dispo.Qty_Dye = oReader.GetDouble("Qty_Dye");
            oRPT_Dispo.Qty_Greige = oReader.GetDouble("Qty_Greige");
            oRPT_Dispo.Qty_SRS = oReader.GetDouble("Qty_SRS");
            oRPT_Dispo.Qty_SRM = oReader.GetDouble("Qty_SRM");
            oRPT_Dispo.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oRPT_Dispo.Qty_FWP = oReader.GetDouble("Qty_FWP");
            oRPT_Dispo.SWQty = oReader.GetDouble("SWQty");
            oRPT_Dispo.WYReqWarp = oReader.GetDouble("WYReqWarp");
            oRPT_Dispo.WYReqWeft = oReader.GetDouble("WYReqWeft");
            oRPT_Dispo.ExesCount = oReader.GetInt32("ExesCount");
            oRPT_Dispo.ExesQty = oReader.GetDouble("ExesQty");
            oRPT_Dispo.Qty_FWP_LB = oReader.GetDouble("Qty_FWP_LB");
            oRPT_Dispo.RequiredWarpLengthLB = oReader.GetDouble("RequiredWarpLengthLB");
          
            return oRPT_Dispo;
        }

        private RPT_Dispo CreateObject(NullHandler oReader)
        {
            RPT_Dispo oRPT_Dispo = new RPT_Dispo();
            oRPT_Dispo = MapObject(oReader);
            return oRPT_Dispo;
        }

        private List<RPT_Dispo> CreateObjects(IDataReader oReader)
        {
            List<RPT_Dispo> oRPT_Dispo = new List<RPT_Dispo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RPT_Dispo oItem = CreateObject(oHandler);
                oRPT_Dispo.Add(oItem);
            }
            return oRPT_Dispo;
        }

        #endregion

        #region Interface implementation
        public RPT_DispoService() { }
        public List<RPT_Dispo> Gets(string sSQL, int nReportType, Int64 nUserID)
        {
            List<RPT_Dispo> oRPT_Dispos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPT_DispoDA.Gets(tc, sSQL, nReportType, nUserID);
                oRPT_Dispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RPT_Dispo", e);
                #endregion
            }
            return oRPT_Dispos;
        }
        public List<RPT_Dispo> Gets_FYStockDispoWise(string sSQL, int nReportType, Int64 nUserID)
        {
            List<RPT_Dispo> oRPT_Dispos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPT_DispoDA.Gets_FYStockDispoWise(tc, sSQL, nReportType, nUserID);
                oRPT_Dispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RPT_Dispo", e);
                #endregion
            }
            return oRPT_Dispos;
        }
        public List<RPT_Dispo> Gets_Weaving(string sSQL, int nReportType, Int64 nUserID)
        {
            List<RPT_Dispo> oRPT_Dispos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPT_DispoDA.Gets_Weaving(tc, sSQL, nReportType, nUserID);
                oRPT_Dispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RPT_Dispo", e);
                #endregion
            }
            return oRPT_Dispos;
        }
        public List<RPT_Dispo> Gets(string sSQL, Int64 nUserID)
        {
            List<RPT_Dispo> oRPT_Dispos = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RPT_DispoDA.Gets(tc, sSQL);
                oRPT_Dispos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RPT_Dispo", e);
                #endregion
            }
            return oRPT_Dispos;
        }
        #endregion
    }   
}

