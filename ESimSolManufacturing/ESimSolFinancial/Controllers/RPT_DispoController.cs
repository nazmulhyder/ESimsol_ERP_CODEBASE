using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class RPT_DispoController : Controller
    {
        #region Declaration
        RPT_Dispo _oRPT_Dispo = new RPT_Dispo();
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<RPT_Dispo> _oRPT_Dispos = new List<RPT_Dispo>();
        string sFeedBackMessage = ""; int _nStatus = 0;

        #endregion

        #region ACTIONS
        public ActionResult ViewRPT_Dispos(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList().Distinct();
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.MktPersons = null;
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            return View(oRPT_Dispos);
        }
        public ActionResult ViewRPT_Dispos_Stock(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList().Distinct();
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.BUID = buid;
            ViewBag.MktPersons = null;
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            return View(oRPT_Dispos);
        }
        public ActionResult ViewRPT_Dispos_Weaving(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList().Distinct();
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.MktPersons = null;
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            return View(oRPT_Dispos);
        }
        public ActionResult ViewDispos_WeavingPlan(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.OrderTypes = Enum.GetValues(typeof(EnumOrderType)).Cast<EnumOrderType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList().Distinct();
            List<FabricProcess> oFabricProcesss = new List<FabricProcess>();
            oFabricProcesss = FabricProcess.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.ProcessTypes = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Process).ToList();
            ViewBag.MktPersons = null;
            ViewBag.FabricWeaves = oFabricProcesss.Where(o => o.ProcessType == EnumFabricProcess.Weave).ToList();
            ViewBag.BU = _oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oRPT_Dispos);
        }
        public ActionResult ViewDispoPlane(int nId)
        {
            string sSQL = "";
            _oRPT_Dispo = new RPT_Dispo();
            List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            if (nId > 0)
            {
                _oRPT_Dispo = new RPT_Dispo();// DyeingOrder.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //_oRPT_Dispo = RPT_Dispo.Gets("", (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_FabricExecutionOrderYarnReceive as FF where FF.FEOSID=" + nId + "  and WYRequisitionID in (Select WYRequisitionID  from WYRequisition where RequisitionType=102   AND WYarnType !=" + (int)EnumWYarnType.LeftOver + ") order by FF.FEOYID";
                oFabricEOYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.FabricExecutionOrderSpecification = FabricExecutionOrderSpecification.Get(nId, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricWarpPlans = FabricWarpPlan.Gets("SELECT * FROM View_FabricWarpPlan WHERE FEOSID = " + nId, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FabricSizingPlans = FabricSizingPlan.Gets("SELECT * FROM View_FabricSizingPlan WHERE FEOSID = " + nId, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BU = oBusinessUnit;
            ViewBag.FabricEOYarnReceives = oFabricEOYarnReceives;
            return View(_oRPT_Dispo);
        }
        #endregion

        #region ADV SEARCH
        [HttpPost]
        public JsonResult AdvSearch(RPT_Dispo oRPT_Dispo)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            try
            {
                string sSQL = MakeSQL(oRPT_Dispo.Params);
                oRPT_Dispos = RPT_Dispo.Gets(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRPT_Dispo.ErrorMessage = ex.Message;
                oRPT_Dispos.Add(oRPT_Dispo);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(oRPT_Dispos);
            //return Json(sjson, JsonRequestBehavior.AllowGet);

            var jsonResult = Json(oRPT_Dispos, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult AdvSearch_Weaving(RPT_Dispo oRPT_Dispo)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            try
            {
                _nStatus = oRPT_Dispo.Status;
                string sSQL = MakeSQL(oRPT_Dispo.Params);
                oRPT_Dispos = RPT_Dispo.Gets_Weaving(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRPT_Dispo.ErrorMessage = ex.Message;
                oRPT_Dispos.Add(oRPT_Dispo);
            }
            var jsonResult = Json(oRPT_Dispos, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(string sString)
        {
            int nCount = 0;
            string ExeNo = "";
            int DateRange = 0;
            DateTime StartDate = DateTime.Today;
            DateTime EndDate = DateTime.Today;

            int ncboTransferDate = 0;
            DateTime dFromTransferAdv = DateTime.Today;
            DateTime dToTransferAdv = DateTime.Today;

            string BuyerName = "";
            string GarmentsName = "";

            int nSearchType = 0;

            if (!String.IsNullOrEmpty(sString))
            {
                ExeNo = Convert.ToString(sString.Split('~')[nCount++]);
                DateRange = Convert.ToInt32(sString.Split('~')[nCount++]);
                StartDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
                EndDate = Convert.ToDateTime(sString.Split('~')[nCount++]);
                BuyerName = Convert.ToString(sString.Split('~')[nCount++]);
                GarmentsName = Convert.ToString(sString.Split('~')[nCount++]);
                nSearchType = 0;
                if (sString.Split('~').Length > nCount)
                    Int32.TryParse(sString.Split('~')[nCount++], out nSearchType);
                //nCount++;
                ncboTransferDate = 0;
                if (sString.Split('~').Length > nCount)
                    Int32.TryParse(sString.Split('~')[nCount++], out ncboTransferDate);
                //ncboTransferDate = Convert.ToInt32(sString.Split('~')[nCount++]);
                if (ncboTransferDate > 0)
                {
                    dFromTransferAdv = Convert.ToDateTime(sString.Split('~')[nCount++]);
                    dToTransferAdv = Convert.ToDateTime(sString.Split('~')[nCount++]);
                }
              
            }
            //string sReturn1 = "select FSCDetailID,FEOSID,sum(Qty) from View_DyeingOrderFabricDetail";
            string sReturn = "";

            #region DispoNo
            if (!string.IsNullOrEmpty(ExeNo))
            {
                Global.TagSQL(ref sReturn);
              //  sReturn = sReturn + "ExeNo like '%" + ExeNo + "%'";
                sReturn = sReturn + "FSCDID in (Select dd.FabricsalesContractDetailID from FabricsalesContractDetail as dd where ExeNo like '%" + ExeNo + "%')";
            }
            #endregion

            #region Exe Date Search
            if (DateRange > 0)
            {
                if (DateRange == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + StartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + StartDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (DateRange == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + StartDate.ToString("dd MMM yyyy") + "'";
                }
                if (DateRange == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + StartDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + EndDate.AddDays(1).ToString("dd MMM yyyy") + "')";
                }
                if (DateRange == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate< '" + StartDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + EndDate.ToString("dd MMM yyyy") + "')";
                }
            }
            #endregion
            #region ncboTransferDate Search
            if (ncboTransferDate > 0)
            {
                if (ncboTransferDate == (int)EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate>= '" + dFromTransferAdv.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dFromTransferAdv.AddDays(1).ToString("dd MMM yyyy") + "')))";
                }
                if (ncboTransferDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate< '" + dFromTransferAdv.ToString("dd MMM yyyy") + "' AND IssueDate>'" + dFromTransferAdv.AddDays(1).ToString("dd MMM yyyy") + "')))";
                }
                if (ncboTransferDate == (int)EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate>'" + dFromTransferAdv.ToString("dd MMM yyyy") + ")))";
                }
                if (ncboTransferDate == (int)EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate<'" + dFromTransferAdv.ToString("dd MMM yyyy") + ")))";
                }
                if (ncboTransferDate == (int)EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate>= '" + dFromTransferAdv.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dToTransferAdv.AddDays(1).ToString("dd MMM yyyy") + "' ) ))";
                }
                if (ncboTransferDate == (int)EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail as GG where GG.FEOSDID in (  Select FEOSDID from FabricExecutionOrderYarnReceive where  WYRequisitionID in  (Select WYRequisitionID from WYRequisition where IssueDate<'" + dFromTransferAdv.ToString("dd MMM yyyy") + "' AND IssueDate >= '" + dToTransferAdv.AddDays(1).ToString("dd MMM yyyy") + "' ) ))";
                    
                }
            }
            #endregion

            #region Buyer Name
            if (!string.IsNullOrEmpty(BuyerName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FSCDID in (Select FabricsalesContractDetailID from FabricsalesContractDetail as dd where dd.FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where BuyerID IN (" + BuyerName + ")))";
            }
            #endregion
            #region Garments Name
            if (!string.IsNullOrEmpty(GarmentsName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FSCDID in ( Select FabricsalesContractDetailID from FabricsalesContractDetail as dd where dd.FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where ContractorID IN (" + GarmentsName + ")))";
            }
            #endregion
            #region Status
            if (_nStatus > 0)
            {
                if (_nStatus == 1)//Waiting For Beam Receive
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecificationDetail WHERE FEOSDID IN (SELECT FEOSDID FROM FabricExecutionOrderYarnReceive WHERE ISNULL(ReceiveBy,0) <= 0 )) ";
                }
                else if (_nStatus == 2)//Warping Beam Receive But Plan Pending
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID in (select FEOSID from (Select FEOS.FEOSID,FEOS.RequiredWarpLength,isnull((Select sum( FP.Qty) from FabricWarpPlan as FP where FEOSID=FEOS.FEOSID ),0) as QtyPlane from FabricExecutionOrderSpecification as FEOS  where  ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessFabric + ") and FEOS.FEOSID in (Select FEOSID from FabricExecutionOrderSpecificationDetail where FEOSDID in ( Select FEOSDID from FabricExecutionOrderYarnReceive where FEOSDID>0)) ) as HH where HH.RequiredWarpLength>HH.QtyPlane) ";
                }
                else if (_nStatus == 3)//Beam Complete But Transfer Pending
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID IN (SELECT FEOSID FROM FabricBeamFinish WHERE ISNULL(IsFinish,0) = 0) ";
                }
                else if (_nStatus == 4)//Wait For Sizing Plan
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " FEOSID in (SELECT WW.FEOSID FROM FabricSizingPlan AS WW WHERE ((SELECT QQ.RequiredWarpLength FROM FabricExecutionOrderSpecification AS QQ WHERE QQ.FEOSID = WW.FEOSID) - WW.Qty) > 1) ";
                }
            }
            #endregion
            #region nSearchType
            if (nSearchType > 0)
            {
                if (nSearchType == 1)//Excess dispo
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  ProdtionType in (" + (int)EnumDispoProType.General + ") and FSCDID in (select Distinct(TT.FSCDID) from FabricExecutionOrderSpecification as TT where ProdtionType in (2) group by FSCDID)or FSCDID in (select FabricSalescontractDetailID from FabricSalescontractDetail where SCDetailType in (" + (int)EnumSCDetailType.ExtraOrder + ") )";
                }
               
            }
            #endregion

            #region ForwardToDOby
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "isnull(ForwardToDOby,0)<>0 and ProdtionType in (" + (int)EnumDispoProType.General + "," + (int)EnumDispoProType.ExcessDyeingQty + "," + (int)EnumDispoProType.ExcessFabric + ")";
            #endregion

            return sReturn;
        }
        #endregion

        #region Print PDF
        public ActionResult PrintDispoStatement(int nID, int nPID, int nDOID, int nBUID) //nID :: FEOSID
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            List<RPT_Dispo> oRPT_DisposReport = new List<RPT_Dispo>();
            List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
            List<RSRawLot> oRSRawLots = new List<RSRawLot>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);


            string sSQL = "SELECT * FROM View_DURequisitionDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrderFabricDetail WHERE FEOSID =" + nID +")";
                                                               // + " AND ProductID = "+ nPID ;
            oDURequisitionDetails = DURequisitionDetail.Gets(sSQL + " ORDER BY ReqDate, RequisitionNo", ((User)Session[SessionInfo.CurrentUser]).UserID);

            string sTemo = string.Join(",", oDURequisitionDetails.Select(x => x.DestinationLotID).ToList());

            if (string.IsNullOrEmpty(sTemo))
                sTemo = "0";
            sSQL = "Select * from View_RSRawLot where LotID in (" + sTemo + ")  and DyeingOrderID  in (SELECT DyeingOrderID FROM DyeingOrderFabricDetail WHERE FEOSID =" + nID + ")";
            oRSRawLots = RSRawLot.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            sSQL = "Select * from View_FabricExecutionOrderYarnReceive where WYarnType in (" + (int)EnumWYarnType .Gray+ ") and FSCDID>0 and FEOSID IN (" + nID + ")";
            oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);



            sSQL = " WHERE DODF.FEOSID = " + nID;
            oRPT_Dispos = RPT_Dispo.Gets_FYStockDispoWise(sSQL, 1, (int)Session[SessionInfo.currentUserID]);
            sSQL = "select * ,(Select SUM(Qty) from DURequisitionDetail as DUR where  ProductID=HH.ProductID and DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=101 AND ISNULL(ReceiveByID,0)<>0) AND  LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID )as Qty_SRS"
+ " ,(Select SUM(Qty) from DURequisitionDetail as DUR where ProductID=HH.ProductID and   DURequisitionID in (SELECT DURequisitionID FROM DURequisition WHERE RequisitionType=102 AND ISNULL(ReceiveByID,0)<>0) AND  DestinationLotID in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID) and   DyeingOrderID =HH.DyeingOrderID) as Qty_SRM"
+",(Select SUM(Qty) from DyeingOrderFabricDetail as DODF where DODF.FEOSID=HH.FEOSID and DODF.ProductID=HH.ProductID ) as Qty_Dispo"
+ ",isnull((Select SUM(Qty) from View_RSRawLot where LotID  in (Select LotID from Lot where LotID=HH.LotID or ParentLotID=HH.LotID)  and DyeingOrderID=HH.DyeingOrderID and RouteSheetID in (Select RouteSheetID from View_RouteSheetDO where ProductID=HH.ProductID and  DyeingOrderID=HH.DyeingOrderID )),0) as Qty_Dye"
+ " from (Select FLA.LotID, DOF.DyeingOrderID,DOF.ProductID, DOF.FEOSID, Lot.LotNo as StyleNo, Product.ProductName,SUM(FLA.Qty)as Req_GreyYarn from DyeingOrderFabricDetail as DOF left join FabricLotAssign as FLA ON FLA.FEOSDID =DOF.FEOSDID left join Lot as Lot ON Lot.LotID =FLA.LotID"
+ " left join Product as Product ON Product.ProductID =FLA.ProductID where DOF.FEOSID=" + nID + "  group by FEOSID, FLA.LotID, DOF.DyeingOrderID,DOF.ProductID,Lot.LotNo,Product.ProductName) as HH order by HH.DyeingOrderID";
            oRPT_DisposReport = RPT_Dispo.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            rpt_RPT_Dispo_Stock oReport = new rpt_RPT_Dispo_Stock();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            byte[] abytes = new byte[1];

            if (oRPT_DisposReport.Count > 0)
            {
                var sDispo = "(" + oRPT_Dispos.Select(x => x.ExeNo).FirstOrDefault() + ")";
                abytes = oReport.PrepareReport(" Dispo Wise Store Report " + sDispo, oRPT_Dispos, oDURequisitionDetails, oRSRawLots, oBusinessUnit, oCompany, oRPT_DisposReport, oFabricExecutionOrderYarnReceives);
            }
            else
            {
                abytes = oErrorReport.PrepareReport("No Data");
            }
           
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Export Exel Dispo
        public void ExportToExcel(string sParams, int nReportType)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            string sString = MakeSQL(sParams);
            oRPT_Dispos = RPT_Dispo.Gets(sString, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL No", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
           
            table_header.Add(new TableHeader { Header = "Dispo Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Qty(Yds)", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "PO Qty", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "End Buyer", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Garments name", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Process Type", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Fabric Type", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Design", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Finish Type", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Construction", Width =25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Yarn Category", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            //table_header.Add(new TableHeader { Header = "Req.Grey Yarn", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo Grey Yarn Req (Warp)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Actual Grey Yarn Issue (Warp)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Yarn Price", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo Grey Yarn Req(Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Actual Grey Yarn Issue (Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Yarn Price", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Total Dispo Grey Yarn Req.", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Total Grey Yarn Issue", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Actual Grey Yarn Value", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Req.Dyed Yarn", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dyed Yarn Production", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Req Grey Fabrics(MTR)", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Req Grey Fabrics(YDS)", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Grey Production Actual(YDS)", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Req Finished Fabrics(MTR)", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Req Finished Fabrics (YDS)", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Actual Finish fabrics (YDS)", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "PI Rate", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "YD Yarn Value", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "YD Chemical Value", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "YD Dyes Value", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Printing D/C Value", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Sizing & Finishing Chemical", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Acc.Holder", Width = 28f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "DO No", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "DO Qty", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Remarks", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Short Excess Production", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Print", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Sample Qty.", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo Ref.", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Excess Dispo", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Excess Qty", Width = 15f, IsRotate = false, Align = TextAlign.Left });


            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Dispo Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Dispo Report");
                sheet.Name = "Dispo Report (Machine Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Dispo Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                RPT_Dispo oRPT_Dispo = new RPT_Dispo();
                foreach (var obj in oRPT_Dispos)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Dispo, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.Qty_Dispo = obj.Qty_Dispo + oRPT_Dispo.Qty_Dispo;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Order, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.Qty_Order = obj.Qty_Order + oRPT_Dispo.Qty_Order;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProcessTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishDesign, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FabricWeaveName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.YarnType, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWarp = obj.GreyYarnReqWarp + oRPT_Dispo.GreyYarnReqWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DyedYarnReqWarp = obj.DyedYarnReqWarp + oRPT_Dispo.DyedYarnReqWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YarnPriceWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YarnPriceWarp = obj.YarnPriceWarp + oRPT_Dispo.YarnPriceWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWeft = obj.GreyYarnReqWeft;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DyedYarnReqWeft = obj.DyedYarnReqWeft + oRPT_Dispo.DyedYarnReqWeft;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YarnPriceWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YarnPriceWeft = obj.YarnPriceWeft + oRPT_Dispo.YarnPriceWeft;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.TotalDispoGreyYarnReq = obj.TotalDispoGreyYarnReq + oRPT_Dispo.TotalDispoGreyYarnReq;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalGreyYarnIssue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.TotalGreyYarnIssue = obj.TotalGreyYarnIssue + oRPT_Dispo.TotalGreyYarnIssue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ActualGreyYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ActualGreyYarnValue = obj.ActualGreyYarnValue + oRPT_Dispo.ActualGreyYarnValue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqDyedYarn, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqDyedYarn = oRPT_Dispo.ReqDyedYarn + obj.ReqDyedYarn;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqDyedYarnPro, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqDyedYarnPro = obj.ReqDyedYarnPro + oRPT_Dispo.ReqDyedYarnPro;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqGreyFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqGreyFabrics = obj.ReqGreyFabrics + oRPT_Dispo.ReqGreyFabrics;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqGreyFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqGreyFabricsY = obj.ReqGreyFabricsY + oRPT_Dispo.ReqGreyFabricsY;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyProductionActual, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyProductionActual = obj.GreyProductionActual + oRPT_Dispo.GreyProductionActual;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqFinishedFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqFinishedFabrics = obj.ReqFinishedFabrics + oRPT_Dispo.ReqFinishedFabrics;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqFinishedFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqFinishedFabricsY = obj.ReqFinishedFabricsY + oRPT_Dispo.ReqFinishedFabricsY;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ActualFinishfabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ActualFinishfabrics = obj.ActualFinishfabrics + oRPT_Dispo.ActualFinishfabrics;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.PINo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.PIRate, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.PIRate = obj.PIRate + oRPT_Dispo.PIRate;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YDYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YDYarnValue = obj.YDYarnValue + oRPT_Dispo.YDYarnValue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YDChemicalValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YDChemicalValue = obj.YDChemicalValue + oRPT_Dispo.YDChemicalValue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YDDyesValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YDDyesValue = obj.YDDyesValue + oRPT_Dispo.YDDyesValue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.PrintingDCValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.PrintingDCValue = obj.PrintingDCValue + oRPT_Dispo.PrintingDCValue;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.SizingChemicalVal, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.SizingChemicalVal = obj.SizingChemicalVal + oRPT_Dispo.SizingChemicalVal;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.MKTPersonName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DONo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DOQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DOQty = obj.DOQty + oRPT_Dispo.DOQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DCQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DCQty = obj.DCQty + oRPT_Dispo.DCQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Note, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ShortExcessPro, 2).ToString(), false, ExcelHorizontalAlignment.Left, false, false); oRPT_Dispo.ShortExcessPro = obj.ShortExcessPro + oRPT_Dispo.ShortExcessPro;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.IsPrintSt, false, ExcelHorizontalAlignment.Center, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.SampleQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.SampleQty = obj.SampleQty + oRPT_Dispo.SampleQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DispoRef, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProdtionTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExesCount.ToString(), true, ExcelHorizontalAlignment.Center, false, false); oRPT_Dispo.ExesCount = obj.ExesCount + oRPT_Dispo.ExesCount;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ExesQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ExesQty = obj.ExesQty + oRPT_Dispo.ExesQty;

                    nSL++;
                    nRowIndex++;
                }
                #region total
                nStartCol = 2;
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "Total :", false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Qty_Dispo, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Qty_Order, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.BuyerName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ContractorName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ProcessTypeName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FinishDesign, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FabricWeaveName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FinishTypeName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.Construction, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.YarnType, false, ExcelHorizontalAlignment.Left, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Req_GreyYarn, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DyedYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YarnPriceWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DyedYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YarnPriceWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.TotalGreyYarnIssue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ActualGreyYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqDyedYarn, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqDyedYarnPro, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqGreyFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqGreyFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyProductionActual, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqFinishedFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqFinishedFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ActualFinishfabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.PINo, false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.PIRate, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YDYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YDChemicalValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YDDyesValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.PrintingDCValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.SizingChemicalVal, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.MKTPersonName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.DONo, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DOQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DCQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.Note, false, ExcelHorizontalAlignment.Left, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ShortExcessPro, 2).ToString(), false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, false, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.SampleQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ExesCount.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.SampleQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                #endregion
                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Dispo Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void ExportToExcelWeving(string sParams, int nReportType)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            string sString = MakeSQL(sParams);
            oRPT_Dispos = RPT_Dispo.Gets_Weaving(sString, nReportType, ((User)Session[SessionInfo.CurrentUser]).UserID);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "SL No", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Dispo Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Qty(Yds)", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "PO Qty", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "End Buyer", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Garments name", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Process Type", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Fabric Type", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Design", Width = 12f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Composition", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Total Ends", Width = 10f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Warp Count", Width = 12f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Weft Count", Width = 12f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Finish Type", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            ////table_header.Add(new TableHeader { Header = "Yarn Category", Width = 35f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Dispo Grey Yarn Req (Warp)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Grey Yarn Req (Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Dyed Yarn Req (Warp)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Dyed Yarn Req (Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Actual Grey Yarn Issue (Warp)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            ////table_header.Add(new TableHeader { Header = "Yarn Price", Width = 12f, IsRotate = false, Align = TextAlign.Right });
            ////table_header.Add(new TableHeader { Header = "Dispo Grey Yarn Req(Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            ////table_header.Add(new TableHeader { Header = "Actual Grey Yarn Issue (Weft)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            ////table_header.Add(new TableHeader { Header = "Yarn Price", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Total Dispo Grey Yarn Req.", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Total Grey Yarn Issue", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            ////table_header.Add(new TableHeader { Header = "Actual Grey Yarn Value", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Req.Dyed Yarn", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            //table_header.Add(new TableHeader { Header = "Dyed Yarn Production", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Req Grey Fabrics(MTR)", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Req Grey Fabrics(YDS)", Width = 18f, IsRotate = false, Align = TextAlign.Right });

            table_header.Add(new TableHeader { Header = "Color Warp", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Color Weft", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Reed No", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "R.Width", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Receive Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Remining Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Warp Plane", Width = 25f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Acc.Holder", Width = 28f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Total Grey Yarn Issued", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "S/W Total Yarn Issued Qty", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Warp Yarn Consumption", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Weft Yarn Consumption", Width = 12f, IsRotate = false, Align = TextAlign.Center });

            table_header.Add(new TableHeader { Header = "DO No", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "DO Qty", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Remarks", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Short Excess Production", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Print", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Sample Qty.", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Dispo Ref.", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false, Align = TextAlign.Left });

            table_header.Add(new TableHeader { Header = "Excess Dispo", Width = 18f, IsRotate = false, Align = TextAlign.Left });
            table_header.Add(new TableHeader { Header = "Excess Qty", Width = 15f, IsRotate = false, Align = TextAlign.Left });


            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Dispo Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Dispo Report");
                sheet.Name = "Dispo Report (Machine Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Dispo Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;
                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                int nSL = 1;
                nEndCol = table_header.Count() + nStartCol;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                RPT_Dispo oRPT_Dispo = new RPT_Dispo();
                foreach (var obj in oRPT_Dispos)
                {
                    nStartCol = 2;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Dispo, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.Qty_Dispo = obj.Qty_Dispo + oRPT_Dispo.Qty_Dispo;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Order, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.Qty_Order = obj.Qty_Order + oRPT_Dispo.Qty_Order;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProcessTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishDesign, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FabricWeaveName, false, ExcelHorizontalAlignment.Left, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalEnds, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.WarpCount, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.WeftCount, false, ExcelHorizontalAlignment.Left, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.YarnType, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWarp = obj.GreyYarnReqWarp + oRPT_Dispo.GreyYarnReqWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWeft = obj.GreyYarnReqWeft + oRPT_Dispo.GreyYarnReqWeft;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DyedYarnReqWarp = obj.DyedYarnReqWarp + oRPT_Dispo.DyedYarnReqWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DyedYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DyedYarnReqWeft = obj.DyedYarnReqWeft + oRPT_Dispo.DyedYarnReqWeft;
                    ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWeft = obj.GreyYarnReqWeft;
                    ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.GreyYarnReqWeftActual, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.GreyYarnReqWeftActual = obj.GreyYarnReqWeftActual + oRPT_Dispo.GreyYarnReqWeftActual;
                    ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.YarnPriceWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.YarnPriceWeft = obj.YarnPriceWeft + oRPT_Dispo.YarnPriceWeft;
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.TotalDispoGreyYarnReq = obj.TotalDispoGreyYarnReq + oRPT_Dispo.TotalDispoGreyYarnReq;
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalGreyYarnIssue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.TotalGreyYarnIssue = obj.TotalGreyYarnIssue + oRPT_Dispo.TotalGreyYarnIssue;
                    ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ActualGreyYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ActualGreyYarnValue = obj.ActualGreyYarnValue + oRPT_Dispo.ActualGreyYarnValue;
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqDyedYarn, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqDyedYarn = oRPT_Dispo.ReqDyedYarn + obj.ReqDyedYarn;
                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqDyedYarnPro, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqDyedYarnPro = obj.ReqDyedYarnPro + oRPT_Dispo.ReqDyedYarnPro;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqGreyFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqGreyFabrics = obj.ReqGreyFabrics + oRPT_Dispo.ReqGreyFabrics;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ReqGreyFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ReqGreyFabricsY = obj.ReqGreyFabricsY + oRPT_Dispo.ReqGreyFabricsY;

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ColorWarp.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ColorWeft.ToString(), false, ExcelHorizontalAlignment.Left, false, false);

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ReedNoSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.REEDWidth.ToString(), false, ExcelHorizontalAlignment.Left, false, false);

                    //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.REEDWidth, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.REEDWidth = obj.REEDWidth + oRPT_Dispo.REEDWidth;


                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WarpLength, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.WarpLength = obj.WarpLength + oRPT_Dispo.WarpLength;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WarpLengthRecd, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.WarpLengthRecd = obj.WarpLengthRecd + oRPT_Dispo.WarpLengthRecd;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.LengthReaming, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);// oRPT_Dispo.LengthReaming = obj.LengthReaming + oRPT_Dispo.ReqGreyFabricsY;

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_FWP, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.Qty_FWP = obj.Qty_FWP + oRPT_Dispo.Qty_FWP;
                    //table_header.Add(new TableHeader { Header = "Color Warp", Width = 25f, IsRotate = false, Align = TextAlign.Left });
                    //table_header.Add(new TableHeader { Header = "Color Weft", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    //table_header.Add(new TableHeader { Header = "Reed No", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    //table_header.Add(new TableHeader { Header = "R.Width", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    //table_header.Add(new TableHeader { Header = "Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    //table_header.Add(new TableHeader { Header = "Receive Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    //table_header.Add(new TableHeader { Header = "Remining Warp Length", Width = 25f, IsRotate = false, Align = TextAlign.Left });

                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.MKTPersonName, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.SWQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.SWQty = obj.SWQty + oRPT_Dispo.SWQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WYReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.WYReqWarp = obj.WYReqWarp + oRPT_Dispo.WYReqWarp;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WYReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.WYReqWeft = obj.WYReqWeft + oRPT_Dispo.WYReqWeft;


                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DONo, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DOQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DOQty = obj.DOQty + oRPT_Dispo.DOQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.DCQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.DCQty = obj.DCQty + oRPT_Dispo.DCQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Note, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ShortExcessPro, 2).ToString(), false, ExcelHorizontalAlignment.Left, false, false); oRPT_Dispo.ShortExcessPro = obj.ShortExcessPro + oRPT_Dispo.ShortExcessPro;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.IsPrintSt, false, ExcelHorizontalAlignment.Center, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.SampleQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.SampleQty = obj.SampleQty + oRPT_Dispo.SampleQty;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DispoRef, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProdtionTypeSt, false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExesCount.ToString(), true, ExcelHorizontalAlignment.Center, false, false); oRPT_Dispo.ExesCount = obj.ExesCount + oRPT_Dispo.ExesCount;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.ExesQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false); oRPT_Dispo.ExesQty = obj.ExesQty + oRPT_Dispo.ExesQty;

                    nSL++;
                    nRowIndex++;
                }
                #region total
                nStartCol = 2;
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "Total :", false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Qty_Dispo, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Qty_Order, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.BuyerName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ContractorName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ProcessTypeName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FinishDesign, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FabricWeaveName, false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.FinishTypeName, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.Construction, false, ExcelHorizontalAlignment.Left, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.YarnType, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DyedYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DyedYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWarpActual, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YarnPriceWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.GreyYarnReqWeftActual, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ////ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.YarnPriceWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.TotalGreyYarnIssue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ActualGreyYarnValue, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqDyedYarn, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqDyedYarnPro, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqGreyFabrics, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ReqGreyFabricsY, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);


                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ColorWarp.ToString(), false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ColorWeft.ToString(), false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ReedNoSt, false, ExcelHorizontalAlignment.Left, true, false);
                //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ColorWeft.ToString(), false, ExcelHorizontalAlignment.Left, false, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.WarpLength, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.WarpLengthRecd, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.LengthReaming, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);// oRPT_Dispo.LengthReaming = obj.LengthReaming + oRPT_Dispo.ReqGreyFabricsY;

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.Qty_FWP, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false); 

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.MKTPersonName, false, ExcelHorizontalAlignment.Left, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.TotalDispoGreyYarnReq, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.SWQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.WYReqWarp, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.WYReqWeft, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.DONo, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DOQty, 2).ToString(), false, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.DCQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.Note, false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ShortExcessPro, 2).ToString(), false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Center, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.SampleQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispo.ExesCount.ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispo.ExesQty, 2).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                #endregion
                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=Dispo Report.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public ActionResult Print_HWStatement(int nID, int nBUID)
        {
            List<DUHardWinding> _oDUHardWindings = new List<DUHardWinding>();
            List<DUHardWinding> oDUHardWindingList = new List<DUHardWinding>();

            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();

            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<DyeingOrderFabricDetail> oDyeingOrderFabricDetails = new List<DyeingOrderFabricDetail>();
            List<FabricExecutionOrderYarnReceive> oFabricExecutionOrderYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<RouteSheetPacking> oRouteSheetPackings = new List<RouteSheetPacking>();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            string sErrorMessage = "";
            try
            {
                if (nID <= 0)
                {
                    throw new Exception("Nothing To Print.");
                }
                else
                {
                    string sSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DD.DyeingOrderID FROM DyeingOrderFabricDetail as DD where DD.FEOSID= " + nID + ")";
                    oDyeingOrderDetails = DyeingOrderDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                    oFabricExecutionOrderSpecification = FabricExecutionOrderSpecification.Get(nID, (int)Session[SessionInfo.currentUserID]);


                    if (oDyeingOrderDetails.Count <= 0)
                    {
                        throw new Exception("No Data Found.");
                    }
                    _oDUHardWindings = DUHardWinding.Gets("SELECT * FROM View_DUHardWinding WHERE DyeingOrderDetailID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    oRouteSheetPackings = RouteSheetPacking.Gets("Select * from view_RouteSheetPacking where PackedByEmpID<>0 and DyeingOrderDetailID in (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderDetailID)) + ")", (int)Session[SessionInfo.currentUserID]);
                  
                    oDyeingOrders = DyeingOrder.Gets("SELECT * FROM View_DyeingOrder WHERE DyeingOrderID IN (" + string.Join(",", oDyeingOrderDetails.Select(x => x.DyeingOrderID)) + ")", (int)Session[SessionInfo.currentUserID]);
                    oDyeingOrderFabricDetails = DyeingOrderFabricDetail.Gets("SELECT * FROM View_DyeingOrderFabricDetail  as DD   where DD.FEOSID= " + nID + "", (int)Session[SessionInfo.currentUserID]);
                    if (oDyeingOrderFabricDetails.Count > 0)
                    {
                        sSQL = "Select * from View_FabricExecutionOrderYarnReceive where FSCDID>0 and FEOSID IN (" + string.Join(",", oDyeingOrderFabricDetails.Select(x => x.FEOSID)) + ")";
                        oFabricExecutionOrderYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    }
                    if (oFabricExecutionOrderYarnReceives.Count > 0)
                    {
                        oDyeingOrderFabricDetails.ForEach(x =>
                        {
                            x.Qty_Assign = _oDUHardWindings.Where(p => p.DyeingOrderDetailID == x.DyeingOrderDetailID).Select(o => o.Qty).Sum();
                            x.Qty_RS = oFabricExecutionOrderYarnReceives.Where(p => p.FEOSDID == x.FEOSDID).Select(o => o.ReceiveQty).Sum();
                            x.Length = oFabricExecutionOrderYarnReceives.Where(p => p.FEOSDID == x.FEOSDID).Select(o => o.TFLength).Sum();

                        });
                        _oDUHardWindings.ForEach(x =>
                        {
                            x.Qty_RSOut = oFabricExecutionOrderYarnReceives.Where(p => p.IssueLotID == x.LotID).Select(o => o.ReceiveQty).Sum();
                        });
                    }
                }

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);

                oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception e)
            {
                sErrorMessage = e.Message;
            }
            if (!string.IsNullOrEmpty(sErrorMessage))
            {
                rptErrorMessage orptErrorMessage = new rptErrorMessage();
                byte[] abytes_error = orptErrorMessage.PrepareReport(sErrorMessage);
                return File(abytes_error, "application/pdf");
            }

            rptHWStatement oReport = new rptHWStatement();
            byte[] abytes = oReport.PrepareReport(_oDUHardWindings, oDyeingOrders, oDyeingOrderDetails, oDyeingOrderFabricDetails, oBusinessUnit, oCompany, oFabricExecutionOrderYarnReceives, oRouteSheetPackings, oFabricExecutionOrderSpecification);
            return File(abytes, "application/pdf");
        }
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult SetDispoData(RPT_Dispo oRPT_Dispo)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oRPT_Dispo);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExcelDispos()
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            RPT_Dispo oRPT_Dispo = new RPT_Dispo();
            try
            {
                oRPT_Dispo = (RPT_Dispo)Session[SessionInfo.ParamObj];
                _nStatus = oRPT_Dispo.Status;
                string sSQL = MakeSQL(oRPT_Dispo.Params);
                oRPT_Dispos = RPT_Dispo.Gets_Weaving(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRPT_Dispos = new List<RPT_Dispo>();
                oRPT_Dispo.ErrorMessage = ex.Message;
                oRPT_Dispos.Add(oRPT_Dispo);
            }

            if (oRPT_Dispos.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);

                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "SL No", Width = 7f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Dispo Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Mkt No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "End Buyer", Width = 30f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Garments Name", Width = 35f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 15f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Dispo Qty(Yds)", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "PO Qty", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Fabric Type", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Fabric Weave", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Finish", Width = 18f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Color Warp", Width = 12f, IsRotate = false, Align = TextAlign.Left });
                table_header.Add(new TableHeader { Header = "Color Weft", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Reed No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "R. Width", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Warp Length", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Warp Length (Recd)", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Remaing Length", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Warping Plan", Width = 12f, IsRotate = false, Align = TextAlign.Center });
                table_header.Add(new TableHeader { Header = "Type", Width = 15f, IsRotate = false, Align = TextAlign.Center });
                
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Dispo Report";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Dispo Report");
                    sheet.Name = "Dispo Report";

                    ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Dispo Report"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;
                    #endregion

                    #region Data
                    nRowIndex++;
                    nStartCol = 2;
                    int nSL = 1;
                    nEndCol = table_header.Count() + nStartCol;
                    ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                    foreach (var obj in oRPT_Dispos)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nSL.ToString(), false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FabricNo, false, ExcelHorizontalAlignment.Left, false, false);                        
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProcessTypeName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.Formatter = "###0.00;(###0.00)";
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Dispo, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_Order, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishDesign, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FabricWeaveName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.FinishTypeName, false, ExcelHorizontalAlignment.Left, false, false);

                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Construction, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ColorWarp.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ColorWeft.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ReedNoSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.REEDWidth, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WarpLength, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.WarpLengthRecd, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.LengthReaming, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(obj.Qty_FWP, 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProdtionTypeSt, false, ExcelHorizontalAlignment.Left, false, false);

                        nSL++;
                        nRowIndex++;
                    }
                    #region total
                    nStartCol = 2;
                    ExcelTool.FillCellMerge(ref sheet, "Total:", nRowIndex, nRowIndex, nStartCol, nStartCol+=7, true, ExcelHorizontalAlignment.Right, false);
                    nStartCol++;
                    ExcelTool.Formatter = "###0.00;(###0.00)";
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x=>x.Qty_Dispo), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.Qty_Order), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol += 3, true, ExcelHorizontalAlignment.Right, false);
                    nStartCol++;
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispos.Sum(x => x.ColorWarp).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispos.Sum(x => x.ColorWeft).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.REEDWidth), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.WarpLength), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.WarpLengthRecd), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.LengthReaming), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, Math.Round(oRPT_Dispos.Sum(x => x.Qty_FWP), 2).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Left, false, false);

                    nStartCol++;

                    #endregion
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Dispo_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            
        }
        #endregion

        #region Export Excel Dispo Report Stock Wise
        [HttpPost]
        public JsonResult AdvSearch_Stock(RPT_Dispo oRPT_Dispo)
        {
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            try
            {
                string sSQL = MakeSQL_DispoStock(oRPT_Dispo);
                oRPT_Dispos = RPT_Dispo.Gets_FYStockDispoWise(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oRPT_Dispo.ErrorMessage = ex.Message;
                oRPT_Dispos.Add(oRPT_Dispo);
            }

            var jsonResult = Json(oRPT_Dispos, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL_DispoStock(RPT_Dispo oRPT_Dispo)
        {
            /*
             SELECT SCNo FROM FabricSalesContract
             SELECT * FROM DyeingOrderFabricDetail
             SELECT IssueDate FROM FabricExecutionOrderSpecification
             SELECT * FROM FabricExecutionOrderSpecificationDetail

             --DODF.FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecification) --IssueDate
             EXEC SP_RPT_FabricYarnStockDispoWise ' WHERE DODF.FEOSID > 0  AND DODF.FSCDetailID > 1 ',1
            */

            int nCount = 2;
            int nCompnareDate_Exe = 0;
            DateTime StartDateExe = DateTime.Today;
            DateTime EndDateExe = DateTime.Today;

            if (!String.IsNullOrEmpty(oRPT_Dispo.Params))
            {
                nCompnareDate_Exe = Convert.ToInt32(oRPT_Dispo.Params.Split('~')[nCount++]);
                StartDateExe = Convert.ToDateTime(oRPT_Dispo.Params.Split('~')[nCount++]);
                EndDateExe = Convert.ToDateTime(oRPT_Dispo.Params.Split('~')[nCount++]);
            }
            string sReturn = " WHERE DODF.FEOSID > 0 ";

            #region DispoNo
            if (!string.IsNullOrEmpty(oRPT_Dispo.ExeNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODF.FSCDetailID  IN (SELECT DD.FabricsalesContractDetailID FROM FabricsalesContractDetail AS DD WHERE ExeNo LIKE '%" + oRPT_Dispo.ExeNo + "%')";
            }
            #endregion

            #region PO No
            if (!string.IsNullOrEmpty(oRPT_Dispo.SCNoFull))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DODF.FSCDetailID IN (SELECT FF.FabricsalesContractDetailID FROM FabricsalesContractDetail AS FF WHERE FF.FabricSalesContractID IN (SELECT DD.FabricSalesContractID FROM FabricSalesContract AS DD WHERE DD.SCNo LIKE '%" + oRPT_Dispo.SCNoFull + "%'))";
            }
            #endregion

            #region Exe Date Search

            if (nCompnareDate_Exe > 0) 
            {
                string sDateSearch = "";
                DateObject.CompareDateQuery(ref sDateSearch, "IssueDate", nCompnareDate_Exe, StartDateExe, EndDateExe);
                sReturn += " AND DODF.FEOSID IN (SELECT FEOSID FROM FabricExecutionOrderSpecification "+sDateSearch+")";
            }

            #endregion

            return sReturn;
        }
        
        public void PrintExcelDispoWiseStock(string sParam, int nBUID)
        {
            RPT_Dispo oRPT_Dispo = new RPT_Dispo();
            oRPT_Dispo.Params = sParam;
            List<RPT_Dispo> oRPT_Dispos = new List<RPT_Dispo>();
            DateTime dtIssueFrom = DateTime.Now;
            DateTime dtIssueTo = DateTime.Now;
            try
            {
                oRPT_Dispo.SCNoFull = oRPT_Dispo.Params.Split('~')[0];
                oRPT_Dispo.ExeNo = oRPT_Dispo.Params.Split('~')[1];

                int nDateSearch = Convert.ToInt16(oRPT_Dispo.Params.Split('~')[2]);
                if (nDateSearch > 0)
                {
                    dtIssueFrom = Convert.ToDateTime(oRPT_Dispo.Params.Split('~')[3]);
                    dtIssueTo = Convert.ToDateTime(oRPT_Dispo.Params.Split('~')[4]);
                }

                string sSQL = MakeSQL_DispoStock(oRPT_Dispo);
                oRPT_Dispos = RPT_Dispo.Gets_FYStockDispoWise(sSQL, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {

            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();

            //Count	Dispo	Rqr. Dt.(PO Full Qty)	Current Date	Dateline	 Require (Req Qty) 	 Issued (Soft Issue) 	 Balance 	Customer	
            //Yarn Type	Section	Ref.Dispo/PO	Issued Date	 Revise Dt./Sc.rtn

            table_header.Add(new TableHeader { Header = "SL#", Width = 7f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Count", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo Date", Width = 14f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Current Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dateline", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Require Qty", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Issued (Soft Issue)", Width = 15f, IsRotate = false, Align = TextAlign.Right });
            table_header.Add(new TableHeader { Header = "Balance", Width = 15f, IsRotate = false, Align = TextAlign.Right });

            table_header.Add(new TableHeader { Header = "Customer", Width = 28f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Yarn Type", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Section", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Ref.Dispo/PO", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Issued Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Revise Dt./Sc.rtn", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol - 2;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Stock Wise Dispo Report";
                var sheet = excelPackage.Workbook.Worksheets.Add("Stock Wise Dispo Report");
                sheet.Name = "Stock Wise Dispo Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = table_header.Count() + 1;

                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Stock Wise Dispo Report"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Date: (" + dtIssueFrom.ToString("dd MMM yyyy") + " to " + dtIssueTo.ToString("dd MMM yyyy") + ")"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                
                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                nEndCol = table_header.Count() + 1;

                ExcelTool.Formatter = "#,##,##0.00;(#,##,##0.00)";

                var oRPT_Dispos_Grp = oRPT_Dispos.GroupBy(x=> x.ProductID, (key, grp) => new 
                {
                    ProductID = key,
                    Results = grp,
                    Qty_Greige = grp.Sum(x => x.Qty_Greige),
                    Qty_SW = grp.Sum(x => x.Qty_SRS - x.Qty_SRM),
                    Qty_Balance = grp.Sum(x => x.Qty_Greige + x.Qty_SRS - x.Qty_SRM),
                });

                //Count	    Dispo	Rqr. Dt.(PO Full Qty)	Current Date	Dateline	 Require (Req Qty) 	 Issued (Soft Issue) 	 Balance 	Customer	
                //Yarn Type	Section	Ref.Dispo/PO	Issued Date	 Revise Dt./Sc.rtn

                foreach (var oGrp in oRPT_Dispos_Grp)
                {
                    int nCount = 1;

                    foreach (var obj in oGrp.Results)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, nCount++.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.YarnType, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ExeDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.CurrentDateSt, false, ExcelHorizontalAlignment.Left, false, false);
                        
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Dateline.ToString(), false, ExcelHorizontalAlignment.Center, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.Qty_Greige.ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (obj.Qty_SRS - obj.Qty_SRM).ToString(), true, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, (obj.Qty_Greige + obj.Qty_SRS - obj.Qty_SRM).ToString(), true, ExcelHorizontalAlignment.Right, false, false);

                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.BuyerName, false, ExcelHorizontalAlignment.Left, false, false);
                        //ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ContractorName, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ProductName.ToString(), false, ExcelHorizontalAlignment.Right, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DCNo, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.SCNoFull, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.DODateSt, false, ExcelHorizontalAlignment.Left, false, false);
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, obj.ReviseDateSt, false, ExcelHorizontalAlignment.Left, false, false);

                        nRowIndex++;
                    }

                    nStartCol = 2;

                    ExcelTool.FillCellMerge(ref sheet, "Sub Total", nRowIndex,nRowIndex,  nStartCol, nStartCol += 5, false, ExcelHorizontalAlignment.Right, true);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oGrp.Qty_Greige.ToString(), true, ExcelHorizontalAlignment.Right, true, true);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oGrp.Qty_SW.ToString(), true, ExcelHorizontalAlignment.Right, true, true);
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oGrp.Qty_Balance.ToString(), true, ExcelHorizontalAlignment.Right, true, true);

                    while(nStartCol <= nEndCol)
                        ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, true, true);

                    nRowIndex++;
                }
                #endregion

                #region Total
                nStartCol = 2;

                ExcelTool.FillCellMerge(ref sheet, "Grand Total", nRowIndex, nRowIndex, nStartCol,  nStartCol += 5, false, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispos.Sum(x => x.Qty_Greige).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispos.Sum(x => (x.Qty_SRS - x.Qty_SRM)).ToString(), true, ExcelHorizontalAlignment.Right, true, false);
                ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, oRPT_Dispos.Sum(x => (x.Qty_Greige + x.Qty_SRS - x.Qty_SRM)).ToString(), true, ExcelHorizontalAlignment.Right, true, false);

                while (nStartCol <= nEndCol)
                    ExcelTool.FillCellBasic(sheet, nRowIndex, nStartCol++, "", false, ExcelHorizontalAlignment.Right, true, true);

                nRowIndex++;
                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=StockWiseDispoReport.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }

        public ActionResult DispoWiseStatementPrint(int nId, int nBUID)
        {
            string sSQL = "";
            _oRPT_Dispos = new List<RPT_Dispo>();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            List<FabricExecutionOrderYarnReceive> oFabricEOYarnReceives = new List<FabricExecutionOrderYarnReceive>();
            List<FabricWarpPlan> oFabricWarpPlans = new List<FabricWarpPlan>();
            List<FabricSizingPlan> oFabricSizingPlans = new List<FabricSizingPlan>();
            List<FabricBatch> oFabricBatchs = new List<FabricBatch>();
            List<FabricBatchRawMaterial> oFabricBatchRawMaterials = new List<FabricBatchRawMaterial>();
            List<FabricTransferPackingList> oFabricTransferPackingLists = new List<FabricTransferPackingList>();
            if (nId > 0)
            {
                _oRPT_Dispos = RPT_Dispo.Gets_Weaving(" WHERE FEOSID = " + nId, 1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricExecutionOrderSpecification =  FabricExecutionOrderSpecification.Get(nId, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_FabricExecutionOrderYarnReceive as FF where FF.FEOSID=" + nId + " order by FF.FEOYID";
                oFabricEOYarnReceives = FabricExecutionOrderYarnReceive.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                oFabricWarpPlans = FabricWarpPlan.Gets("SELECT * FROM View_FabricWarpPlan WHERE FEOSID = " + nId, (int)Session[SessionInfo.currentUserID]);
                oFabricSizingPlans = FabricSizingPlan.Gets("SELECT * FROM View_FabricSizingPlan WHERE FEOSID = " + nId, (int)Session[SessionInfo.currentUserID]);

                oFabricBatchs = FabricBatch.Gets("Select * from FabricBatch WHERE FEOSID = " + nId, (int)Session[SessionInfo.currentUserID]);
                sSQL = string.Join(",", oFabricBatchs.Select(x => x.FBID));
                if (!string.IsNullOrEmpty(sSQL))
                oFabricBatchRawMaterials = FabricBatchRawMaterial.Gets("Select * from View_FabricBatchRawMaterial where FBID IN (" + sSQL + ") ORDER BY FBID", (int)Session[SessionInfo.currentUserID]);
                oFabricTransferPackingLists = FabricTransferPackingList.Gets("SELECT * FROM [View_FabricTransferPackingList] WHERE FEOID = " + oFabricExecutionOrderSpecification.FSCDID, (int)Session[SessionInfo.currentUserID]);
            }
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBusinessUnit);
            }
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes;
            rptDispoWiseStatementPrint oReport = new rptDispoWiseStatementPrint();
            abytes = oReport.PrepareReport(_oRPT_Dispos[0], oFabricEOYarnReceives, oFabricWarpPlans, oFabricSizingPlans, oFabricBatchRawMaterials, oFabricBatchs, oFabricTransferPackingLists, oCompany);
            return File(abytes, "application/pdf");
        }

        #endregion
    }
}