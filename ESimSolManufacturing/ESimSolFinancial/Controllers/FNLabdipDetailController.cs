using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSol.Controllers
{
    public class FNLabDipDetailController : Controller
    {
        #region Declaration
        FNLabDipDetail _oFNLabDipDetail = new FNLabDipDetail();
        List<FNLabDipDetail> _oFNLabDipDetails = new List<FNLabDipDetail>();
        string _sDateRange = "";
        #endregion

        #region FNLabDipDetail
        public ActionResult ViewFNLabDipDetails(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            FabricPOSetup oFabricPOSetup = new FabricPOSetup();
            oFabricPOSetup = oFabricPOSetup.GetsActive(((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            _oFNLabDipDetails = FNLabDipDetail.Gets("SELECT top(200)* FROM  View_FNLabDipDetail order by IssueDate DESC,FSCID asc,SLNo asc", ((User)Session[SessionInfo.CurrentUser]).UserID);//where Isnull(ReceiveBY,0)=0
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.EnumShades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.FabricPOSetup = oFabricPOSetup;

            ViewBag.LDStatuss= EnumObject.jGets(typeof(EnumLDStatus)); 

            ViewBag.FabricOrderSetups = FabricOrderSetup.Gets("SELECT * FROM FabricOrderSetup WHERE FabricOrderType IN (" 
                                        + (int)EnumFabricRequestType.Labdip
                                        + "," + (int)EnumFabricRequestType.YarnSkein + ") AND Activity=1", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
           
            return View(_oFNLabDipDetails);
        }
        public ActionResult ViewFNLabDipDetail(int nid, int buid)
        {
            Fabric oFabric = new Fabric();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            _oFNLabDipDetails = FNLabDipDetail.Gets("SELECT * FROM  View_FNLabDipDetail where Isnull(FabricID,0)=" + nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabric = oFabric.Get(nid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BU = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Fabric = oFabric;
            ViewBag.EnumShades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.BUID = buid;

            return View(_oFNLabDipDetails);
        }
    
        [HttpPost]
        public JsonResult SaveDetail(FNLabDipDetail oFNLabDipDetail)
        {
            try
            {
                if (oFNLabDipDetail.FNLabDipDetailID <= 0)
                {
                    oFNLabDipDetail = oFNLabDipDetail.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNLabDipDetail = oFNLabDipDetail.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateLot(FNLabDipDetail oFNLabDipDetail)
        {
            try
            {
               if (oFNLabDipDetail.FNLabDipDetailID <= 0) { throw new Exception("Please select an valid item."); }
                else
                {
                    oFNLabDipDetail = oFNLabDipDetail.UpdateLot(((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteDetail(FNLabDipDetail oFNLabDipDetail)
        {
            try
            {
                if (oFNLabDipDetail.FNLabDipDetailID <= 0) { throw new Exception("Please select an valid item."); }
                oFNLabDipDetail = oFNLabDipDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail=new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Search
        [HttpPost]
        public JsonResult GetbyLDNo(FNLabDipDetail oFNLabDipDetail)    
        {
            string sContractorIDs = "";
            if (!string.IsNullOrEmpty(oFNLabDipDetail.Params))
            {
                sContractorIDs = Convert.ToString(oFNLabDipDetail.Params.Split('~')[0]);
            }

            string sSQL = "";
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            string sReturn1 = "SELECT top(100)* FROM View_FNLabdipDetail ";
            string sReturn = "";

            #region LD No
            if (!string.IsNullOrEmpty(oFNLabDipDetail.LabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + oFNLabDipDetail.LabdipNo + "%'";
            }
            #endregion

            //#region Dispo No
            //if (!string.IsNullOrEmpty(oFNLabDipDetail.fn))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " LabdipNo LIKE '%" + oFNLabDipDetail.LabdipNo + "%'";
            //}
            //#endregion

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "isnull(LabdipNo,'')<>''";

            #region Contractor
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID in (" + sContractorIDs + ")";
            }
            #endregion
            #region Fabric
            if (oFNLabDipDetail.FabricID>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID =" + oFNLabDipDetail.FabricID + "";
            }
            #endregion

            //#region BUID
            //if (oExportLC.BUID > 0)
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " BUID=" + oExportLC.BUID;
            //}
            //#endregion
            sSQL = sReturn1 + sReturn + " order by  IssueDate DESC,FSCID asc,SLNo asc";
            _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetByLDNoOrDispoNo(FNLabDipDetail oFNLabDipDetail)
        {
            string sSQL = "";
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            string sReturn1 = "SELECT * FROM View_FNLabdipDetail ";
            string sReturn = "";

            #region LD No
            if (!string.IsNullOrEmpty(oFNLabDipDetail.LabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + oFNLabDipDetail.LabdipNo + "%'";
            }
            #endregion

            //#region Dispo No
            //if (!string.IsNullOrEmpty(oFNLabDipDetail.fn))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " LabdipNo LIKE '%" + oFNLabDipDetail.LabdipNo + "%'";
            //}
            //#endregion

            sSQL = sReturn1 + sReturn;
            _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLDNo(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            FabricSalesContractDetail _oFabricSalesContractDetail = oFabricSalesContractDetail;
            try
            {
                //FNBatch oFNBatch = new FNBatch();
                //List<FNBatch> oFNBatchs = new List<FNBatch>();
                //oFNBatchs = FNBatch.Gets("SELECT * From View_FNBatch WHERE FNExOID = " + _oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //if(oFNBatchs.Count==0)
                //{
                    if ( _oFabricSalesContractDetail.FabricSalesContractDetailID > 0)
                    {
                        _oFabricSalesContractDetail = _oFabricSalesContractDetail.SaveLDNo(_oFabricSalesContractDetail, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                    else
                    {
                        _oFabricSalesContractDetail.ErrorMessage = "Can Not Update. Because Batch Already Created";
                    }
                //}
            }
            catch (Exception ex)
            {
                _oFabricSalesContractDetail = new FabricSalesContractDetail();
                _oFabricSalesContractDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricSalesContractDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AdvSearch(FNLabDipDetail oFNLabDipDetail)
        {
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            try
            {
                string sSQL = MakeSQL(oFNLabDipDetail);
                _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNLabDipDetails = new List<FNLabDipDetail>();
                _oFNLabDipDetail.ErrorMessage = ex.Message;
                _oFNLabDipDetails.Add(_oFNLabDipDetail);
            }
            var jsonResult = Json(_oFNLabDipDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(FNLabDipDetail oFNLabDipDetail)
        {
            string sParams = oFNLabDipDetail.ErrorMessage;

            string sContractorIDs = "";
            string sProductIDs = "";

            int ncboIssueDate = 0;
            DateTime dFromIssueDate = DateTime.Today;
            DateTime dToIssueDate = DateTime.Today;

            int ncboReceiveDate = 0;
            DateTime dFromReceiveDate = DateTime.Today;
            DateTime dToReceiveDate = DateTime.Today;
            int ncboSubmitDate = 0;
            DateTime dFromSubmitDate = DateTime.Today;
            DateTime dToSubmitDate = DateTime.Today;
            int nCboMkPerson = 0;

            string sLabdipNo = "";
            string sFabricNo = "";
            string sColorName = "";
            string sConstruction = "";
            int nBuid=0;
            int nOrderType = 0;
            int nLDStatus = 0;

            if (!string.IsNullOrEmpty(sParams))
            {
             
                sContractorIDs = Convert.ToString(sParams.Split('~')[0]);
                sProductIDs = Convert.ToString(sParams.Split('~')[1]);

                ncboIssueDate = Convert.ToInt32(sParams.Split('~')[2]);
                if (ncboIssueDate > 0)
                {
                    dFromIssueDate = Convert.ToDateTime(sParams.Split('~')[3]);
                    dToIssueDate = Convert.ToDateTime(sParams.Split('~')[4]);
                }

                ncboReceiveDate = Convert.ToInt32(sParams.Split('~')[5]);
                if (ncboReceiveDate>0)
                {
                    dFromReceiveDate = Convert.ToDateTime(sParams.Split('~')[6]);
                    dToReceiveDate = Convert.ToDateTime(sParams.Split('~')[7]);
                }

                ncboSubmitDate = Convert.ToInt32(sParams.Split('~')[8]);
                if (ncboSubmitDate > 0)
                {
                    dFromSubmitDate = Convert.ToDateTime(sParams.Split('~')[9]);
                    dToSubmitDate = Convert.ToDateTime(sParams.Split('~')[10]);
                }
                nCboMkPerson =0;

                sLabdipNo = Convert.ToString(sParams.Split('~')[12]);
                sFabricNo = Convert.ToString(sParams.Split('~')[13]);
                sColorName = Convert.ToString(sParams.Split('~')[14]);
                sConstruction = Convert.ToString(sParams.Split('~')[15]);

                if (sParams.Split('~').Count() > 16)
                {
                    nBuid = Convert.ToInt32(sParams.Split('~')[16]);
                    nOrderType = Convert.ToInt32(sParams.Split('~')[17]);
                    nLDStatus = Convert.ToInt32(sParams.Split('~')[18]);
                }
            }

            string sReturn1 = "";
            string sReturn = "";
            sReturn1 = "SELECT * FROM View_FNLabDipDetail";


            #region Contractor
            if (!String.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BuyerID in(" + sContractorIDs + ")";
            }
            #endregion

            #region ProductName
            if (!String.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID in(" + sProductIDs + ")";
            }
            #endregion

            #region Issue Date
            if (ncboIssueDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboIssueDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (ncboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (ncboIssueDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (ncboIssueDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromIssueDate.ToString("dd MMM yyyy");
                }
                else if (ncboIssueDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
                else if (ncboIssueDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromIssueDate.ToString("dd MMM yyyy") + " To " + dToIssueDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region ncboReceiveDate
            if (ncboReceiveDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);

                Global.TagSQL(ref sReturn);
                if (ncboReceiveDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ReceiveByDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: " + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotEqualTo->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: GreaterThen->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: SmallerThen->" + dFromReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: From " + dFromReceiveDate.ToString("dd MMM yyyy") + " To " + dToReceiveDate.ToString("dd MMM yyyy");
                }
                else if (ncboReceiveDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromReceiveDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToReceiveDate.ToString("dd MMM yyyy") + "',106)) ";
                    _sDateRange = "Date: NotBetween " + dFromReceiveDate.ToString("dd MMM yyyy") + " To " + dToReceiveDate.ToString("dd MMM yyyy");
                }
            }
            #endregion

            #region Submit Date
            if (ncboSubmitDate != (int)EnumCompareOperator.None)
            {
                Global.TagSQL(ref sReturn);
                if (ncboSubmitDate == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
                else if (ncboSubmitDate == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),SubmitByDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromSubmitDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToSubmitDate.ToString("dd MMM yyyy") + "',106)) ";
                }
            }
            #endregion


         

            #region sConstruction
            if (!string.IsNullOrEmpty(sConstruction))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "Construction LIKE '%" + sConstruction + "%'";
            }
            #endregion

            #region sLabdipNo
            if (!string.IsNullOrEmpty(sLabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipNo LIKE '%" + sLabdipNo + "%'";
            }
            #endregion

            #region nOrder Type
            if (nOrderType>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID IN (SELECT FabricID FROM Fabric WHERE FabricOrderType="+nOrderType + ")";
            }
            #endregion

            #region sFabricNo
            if (!string.IsNullOrEmpty(sFabricNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricNo LIKE '%" + sFabricNo + "%' ";
            }
            #endregion

            #region nLDStatus
            if (nLDStatus>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LDStatus =" + nLDStatus;
            }
            #endregion

            #region sColorName
            if (!string.IsNullOrEmpty(sColorName))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ColorName LIKE '%" + sColorName + "%' ";
            }
            #endregion

            #region PO No
            if (!string.IsNullOrEmpty(oFNLabDipDetail.SCNoFull))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SCNoFull LIKE '%" + oFNLabDipDetail.SCNoFull + "%' ";
            }
            #endregion


            string sSQL = sReturn1 + " " + sReturn + " Order BY SCNoFull";
            return sSQL;
        }
        #endregion

        [HttpPost]
        public JsonResult GetFNLabDipDetail(FNLabDipDetail oFNLabDipDetail)
        {
            try
            {
                if (oFNLabDipDetail.FNLabDipDetailID <= 0)
                    throw new Exception("Please select a valid item.");
                oFNLabDipDetail = FNLabDipDetail.Get(oFNLabDipDetail.FNLabDipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetByFabric(FNLabDipDetail oFNLabDipDetail)
        {
            try
            {
                if (oFNLabDipDetail.FabricID <= 0)
                    throw new Exception("Please select a valid item.");
                _oFNLabDipDetails = FNLabDipDetail.GetsBy(oFNLabDipDetail.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                _oFNLabDipDetails = new List<FNLabDipDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
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
        #endregion

        #region Laboratory

        #region Color Issue
        [HttpPost]
        public JsonResult IssueLDNoMultiple(List<FNLabDipDetail> oFNLabDipDetails)
        {
            _oFNLabDipDetails = new List<FNLabDipDetail>();
            try
            {
                _oFNLabDipDetails = FNLabDipDetail.IssueLDNoMultiple(oFNLabDipDetails, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oFNLabDipDetail = new FNLabDipDetail();
                _oFNLabDipDetail.ErrorMessage = ex.Message;
                _oFNLabDipDetails = new List<FNLabDipDetail>();
                _oFNLabDipDetails.Add(_oFNLabDipDetail);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save_LDNo(FNLabDipDetail oFNLabDipDetail)
        {
            oFNLabDipDetail.RemoveNulls();
            try
            {
                oFNLabDipDetail = oFNLabDipDetail.Save_LDNo(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Submited(FNLabDipDetail oFNLabDipDetail)
        {
            oFNLabDipDetail.RemoveNulls();
            try
            {
                oFNLabDipDetail = oFNLabDipDetail.Submited(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Issued(FNLabDipDetail oFNLabDipDetail)
        {
            oFNLabDipDetail.RemoveNulls();
            try
            {
                oFNLabDipDetail.LDStatus = EnumLDStatus.Issued;
                oFNLabDipDetail = oFNLabDipDetail.Issued(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateShade(FNLabdipShade oFNLabdipShade)
        {
            oFNLabdipShade.RemoveNulls();
            try
            {
                if (oFNLabdipShade.FNLabdipShadeID<=0)
                {
                    oFNLabdipShade = oFNLabdipShade.IUD( (int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNLabdipShade = oFNLabdipShade.IUD((int)EnumDBOperation.Active, ((User)Session[SessionInfo.CurrentUser]).UserID);  //active for update only shade id
                }
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Labdip Shade & Recipe


        [HttpPost]
        public JsonResult SaveFNLabdipShade(FNLabdipShade oFNLabdipShade)
        {
            try
            {
                if (oFNLabdipShade.FNLabdipShadeID <= 0)
                {
                    oFNLabdipShade = oFNLabdipShade.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNLabdipShade = oFNLabdipShade.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNLabdipShade(FNLabdipShade oFNLabdipShade)
        {
            try
            {
                if (oFNLabdipShade.FNLabdipShadeID <= 0) { throw new Exception("Please select an valid item."); }
                oFNLabdipShade = oFNLabdipShade.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApproveFNLabdipShade(FNLabdipShade oFNLabdipShade)
        {
            try
            {
                if (oFNLabdipShade.FNLabdipShadeID <= 0) { throw new Exception("Please select an valid item."); }
                oFNLabdipShade = oFNLabdipShade.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CopyFNLabdipShade(FNLabdipShade oFNLabdipShade)
        {
            try
            {

                if (oFNLabdipShade.FNLabdipShadeID <= 0 || oFNLabdipShade.FNLabDipDetailID <= 0) { throw new Exception("Please select an valid item."); }

                FNLabDipDetail oFNLabDipDetail = new FNLabDipDetail();
                oFNLabDipDetail = FNLabDipDetail.Get(oFNLabdipShade.FNLabDipDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
                string sSQL = "Select * from View_FNLabdipShade Where FNLabDipDetailID=" + oFNLabDipDetail.FNLabDipDetailID;
                oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                oFNLabdipShade = oFNLabdipShades.FirstOrDefault(x => x.FNLabdipShadeID == oFNLabdipShade.FNLabdipShadeID);

                if (oFNLabdipShade == null || oFNLabdipShade.FNLabdipShadeID <= 0) throw new Exception("Invalid labdip shade. Please refresh & try again.");

                if (oFNLabdipShade.FNLabdipShadeID > 0)
                {
                    List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();
                    sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID=" + oFNLabdipShade.FNLabdipShadeID;
                    oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oFNLabdipRecipes.Count() > 0)
                    {
                        oFNLabdipRecipes.ForEach(x => { x.FNLabdipRecipeID = 0; x.FNLabdipShadeID = 0; });
                        oFNLabdipShade.RecipeDyes = oFNLabdipRecipes.Where(x => x.IsDyes == true).ToList();
                        oFNLabdipShade.RecipeChemicals = oFNLabdipRecipes.Where(x => x.IsDyes == false).ToList();
                    }

                    int nShadeCount = oFNLabDipDetail.ShadeCount;
                    EnumShade previousShade = oFNLabdipShades.Max(x => x.ShadeID);
                    EnumShade maxShade = Enum.GetValues(typeof(EnumShade)).Cast<EnumShade>().Max();
                    nShadeCount = ((int)maxShade < nShadeCount) ? (int)maxShade : nShadeCount;
                    if (previousShade == maxShade || nShadeCount == (int)previousShade) throw new Exception("No remaining shade available to create.");

                    oFNLabdipShade.FNLabdipShadeID = 0;
                    oFNLabdipShade.ShadeID = Enum.GetValues(typeof(EnumShade)).Cast<EnumShade>().Where(x => (int)x > (int)previousShade).First();

                    oFNLabdipShade = oFNLabdipShade.CopyFNLabdipShade(((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFNLabdipRecipe(FNLabdipRecipe oFNLabdipRecipe)
        {
            try
            {
                FNLabdipShade oFNLabdipShade = FNLabdipShade.Get(oFNLabdipRecipe.FNLabdipShadeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNLabdipRecipe.FNLabdipRecipeID <= 0)
                {
                    oFNLabdipRecipe = oFNLabdipRecipe.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFNLabdipRecipe = oFNLabdipRecipe.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oFNLabdipShade.Qty > 0)
                {
                    oFNLabdipRecipe.PerTage = (oFNLabdipRecipe.Qty * 100) / oFNLabdipShade.Qty;
                }
            }
            catch (Exception ex)
            {
                oFNLabdipRecipe = new FNLabdipRecipe();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNLabdipRecipe(FNLabdipRecipe oFNLabdipRecipe)
        {
            try
            {
                if (oFNLabdipRecipe.FNLabdipRecipeID <= 0) { throw new Exception("Please select an valid item."); }
                oFNLabdipRecipe = oFNLabdipRecipe.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNLabdipRecipe = new FNLabdipRecipe();
                oFNLabdipRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipRecipe.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsFNLabdipShade(FNLabDipDetail oFNLabDipDetail)
        {
            FNLabdipShade oFNLabdipShade = new FNLabdipShade();
            try
            {
                string sSQL = "Select * from View_FNLabdipShade Where FNLabDipDetailID = " + oFNLabDipDetail.FNLabDipDetailID;
                oFNLabdipShade.FNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFNLabdipShade.FNLabdipShades.Count() > 0 && oFNLabdipShade.FNLabdipShades[0].FNLabdipShadeID > 0)
                {
                    List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();

                    sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID In (" + string.Join(",", oFNLabdipShade.FNLabdipShades.Select(x => x.FNLabdipShadeID)) + ")";
                    oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFNLabdipShade.RecipeDyes = oFNLabdipRecipes.Where(x => x.IsDyes == true).ToList();
                    oFNLabdipShade.RecipeChemicals = oFNLabdipRecipes.Where(x => x.IsDyes == false).ToList();
                }
            }
            catch (Exception ex)
            {
                oFNLabdipShade = new FNLabdipShade();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShade);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get Product BU, User and Name wise ( write by Mamun)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            EnumProductUsages eEnumProductUsages = new EnumProductUsages();
            try
            {
                eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.FNLabdip, eEnumProductUsages, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.FNLabdip, eEnumProductUsages, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult GetProductsYarn(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                oProducts = new List<Product>();
                string sSQL = "Select top(250)* from VIEW_Product as P Where P.ProductID in (Select Distinct(ProductID) from View_FNLabdipDetail) ";
                if (!String.IsNullOrEmpty(oProduct.ProductName))
                {
                    sSQL = sSQL + " and P.ProductName Like '%" + oProduct.ProductName + "%'";
                }
                sSQL = sSQL + " Order By ProductName";
                oProducts = Product.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            //var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetMKTPersons(MarketingAccount oMarketingAccount)
        {
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();

            try
            {
                string sSQL = "SELECT top(100)* FROM View_MarketingAccount where MarketingAccountID in (Select Distinct(MKTPersonID) from View_FNLabdipDetail) ";
                if (!String.IsNullOrEmpty(oMarketingAccount.Name))
                {

                    sSQL = sSQL + " Name Like '%" + oMarketingAccount.Name + "%'";
                }
                sSQL = sSQL + "Order By NAME";

                oMarketingAccounts = MarketingAccount.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oMarketingAccount = new MarketingAccount();
                oMarketingAccount.ErrorMessage = ex.Message;
                oMarketingAccounts.Add(oMarketingAccount);
            }
          
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMarketingAccounts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetContractor(Contractor oContractor)
        {
            List<Contractor> oContractors = new List<Contractor>();
            try
            {
                oContractors = new List<Contractor>();
                string sSQL = "Select top(250)* from Contractor  where ContractorID in (Select Distinct(View_FNLabdipDetail.BuyerID) from View_FNLabdipDetail)";
                if (!String.IsNullOrEmpty(oContractor.Name))
                {
                    sSQL = sSQL + " and Name Like '%" + oContractor.Name + "%'";
                }
                sSQL = sSQL + " Order By Name ";
                oContractors = Contractor.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oContractor = new Contractor();
                oContractor.ErrorMessage = ex.Message;
                oContractors.Add(oContractor);
            }
            //var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            //jsonResult.MaxJsonLength = int.MaxValue;
            //return jsonResult;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsShade(FNLabDipDetail oFNLabDipDetail)
        {
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            try
            {
                string sSQL = "";
                sSQL = "Select * from View_FNLabdipShade Where FNLabDipDetailID=" + oFNLabDipDetail.FNLabDipDetailID;
                oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FNLabdipShade oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
                oFNLabdipShades.Add(oFNLabdipShade);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShades);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsApprovedShade(FNLabDipDetail oFNLabDipDetail)
        {
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            try
            {
                string sSQL = "";
                sSQL = "SELECT * FROM View_FNLabdipShade WHERE ISNULL(ApproveBy,0)!=0 AND FNLabDipDetailID=" + oFNLabDipDetail.FNLabDipDetailID;
                oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FNLabdipShade oFNLabdipShade = new FNLabdipShade();
                oFNLabdipShade.ErrorMessage = ex.Message;
                oFNLabdipShades.Add(oFNLabdipShade);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNLabdipShades);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult UpdateColorSet(FNLabDipDetail oFNLabDipDetail)
        {
            _oFNLabDipDetail = new FNLabDipDetail();
            try
            {
                _oFNLabDipDetail = oFNLabDipDetail.UpdateColorSet((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNLabDipDetail = new FNLabDipDetail();
                _oFNLabDipDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNLabDipDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLabDipRecipe(int nId, int nDetailID, double nts)
        {
            string sSQL = "";
            Fabric _oFabric = new Fabric();
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();
            try
            {
                if (nId > 0)
                {
                    _oFabric = _oFabric.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oFabric.FabricID > 0)
                    {
                       // string sSQL = "Select * from View_LabdipDetail Where LabDipID=" + _oLabDip.LabDipID;

                        if (nDetailID > 0)
                        {
                            sSQL = "Select * from View_FNLabdipDetail Where FabricID=" + _oFabric.FabricID + " And FNLabDipDetailID=" + nDetailID;
                            _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipShade Where FNLabDipDetailID =" + nDetailID;
                            oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID in (Select FNLabdipShadeID from FNLabdipShade Where FNLabdipDetailID =" + nDetailID + ")";
                            oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        else
                        {
                            sSQL = "Select * from View_FNLabdipDetail Where FabricID=" + _oFabric.FabricID;
                            _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipShade Where FNLabdipDetailID in (Select FNLabdipDetailID from FNLabdipDetail where FabricID=" + _oFabric.FabricID + ")";
                            oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID in (Select FNLabdipShadeID from FNLabdipShade Where FNLabdipDetailID in (Select FNLabdipDetailID from FNLabdipDetail where FabricID=" + _oFabric.FabricID + "))";
                            oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);



            rptFNLabDipRecipe oReport = new rptFNLabDipRecipe();
            byte[] abytes = oReport.PrepareReport(_oFNLabDipDetails, oCompany, oBusinessUnit, oFNLabdipShades, oFNLabdipRecipes, _oFabric);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintLabDipSubCard(int nId, int nDetailID, int buid, double nts)
        {
            string sSQL = "";
            Fabric _oFabric = new Fabric();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<FNLabdipShade> oFNLabdipShades = new List<FNLabdipShade>();
            FabricSCReport oFabricSCReport = new FabricSCReport();
            _oFNLabDipDetail = new FNLabDipDetail();
            //List<FNLabdipRecipe> oFNLabdipRecipes = new List<FNLabdipRecipe>();
            try
            {
                if (nId > 0)
                {
                    _oFabric = _oFabric.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                   
                        if (nDetailID > 0)
                        {
                            //sSQL = "Select * from View_FNLabdipDetail Where FabricID=" + _oFabric.FabricID + " And FNLabDipDetailID=" + nDetailID;
                            //_oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipShade Where FNLabDipDetailID =" + nDetailID;
                            oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            //sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID in (Select FNLabdipShadeID from FNLabdipShade Where FNLabdipDetailID =" + nDetailID + ")";
                            //oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oFNLabDipDetail = FNLabDipDetail.Get(nDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            _oFNLabDipDetails.Add(_oFNLabDipDetail);
                        }
                        else
                        {
                            sSQL = "Select * from View_FNLabdipDetail Where FabricID=" + _oFabric.FabricID;
                            _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            sSQL = "Select * from View_FNLabdipShade Where FNLabdipDetailID in (Select FNLabdipDetailID from FNLabdipDetail where FabricID=" + _oFabric.FabricID + ")";
                            oFNLabdipShades = FNLabdipShade.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                            //sSQL = "Select * from View_FNLabdipRecipe Where FNLabdipShadeID in (Select FNLabdipShadeID from FNLabdipShade Where FNLabdipDetailID in (Select FNLabdipDetailID from FNLabdipDetail where FabricID=" + _oFabric.FabricID + "))";
                            //oFNLabdipRecipes = FNLabdipRecipe.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        }
                        oFabricSCReport = oFabricSCReport.Get(_oFNLabDipDetail.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            Fabric oFabric = new Fabric();
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFNLabDipSubCard oReport = new rptFNLabDipSubCard();
            byte[] abytes = oReport.PrepareReport(_oFNLabDipDetails, oCompany, oBusinessUnit, oFabricSCReport, oFNLabdipShades, _oFNLabDipDetail);
            return File(abytes, "application/pdf");
        }

        public void PrintLabDipsXL(string Params, double nts)
        {
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Finishing, ((User)Session[SessionInfo.CurrentUser]).UserID);

            _oFNLabDipDetails = new List<FNLabDipDetail>();
            try
            {
                string sSQL = "";
                if (string.IsNullOrEmpty(Params))
                    sSQL = "SELECT * FROM  View_FNLabDipDetail where Isnull(ReceiveBY,0)=0";
                else
                    sSQL = MakeSQL(new FNLabDipDetail { ErrorMessage = Params });

                _oFNLabDipDetails = FNLabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNLabDipDetails = new List<FNLabDipDetail>();
                _oFNLabDipDetail.ErrorMessage = ex.Message;
                _oFNLabDipDetails.Add(_oFNLabDipDetail);
            }

            #region Print XL

            int nRowIndex = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0, nCount = 0;
            ExcelRange cell;
            ExcelRange HeaderCell;
            ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FNLabDip");
                sheet.Name = "Lab Dip";

                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Issue Date", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric No", Width = 11f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Composition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LD Order No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color Name", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Panton No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Status", Width = 20f, IsRotate = false });

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);

                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                cell.Value = "Lab Dip"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, 7]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                cell = sheet.Cells[nRowIndex, 8, nRowIndex, 10]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = false;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                #region Blank
                nStartCol = 2;
                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 10]; cell.Merge = true;
                cell.Value = ""; cell.Style.Font.Bold = true;
                border = cell.Style.Border; border.Bottom.Style = ExcelBorderStyle.Thin; border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
                cell.Style.Font.Size = 13; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion
                
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10);

                #region Report Data

                #region Body
                foreach (var oItem in _oFNLabDipDetails)
                {
                    nStartCol = 2;
                    nCount++;
                    FillCell(sheet, nRowIndex, nStartCol++, nCount.ToString(), false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.IssueDateSt, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.Construction, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.LabdipNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorName, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.PantonNo, false);
                    FillCell(sheet, nRowIndex, nStartCol++, oItem.Status, false);

                    sheet.Row(nRowIndex).Style.Font.Size = 10;
                    nRowIndex++;
                }
                #endregion

                #endregion

                cell = sheet.Cells[1, 1, nRowIndex, 13];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FNLabDipList.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;
            cell = sheet.Cells[nRowIndex, nStartCol++];
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            if (IsNumber)
                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }
        
     
    }
}