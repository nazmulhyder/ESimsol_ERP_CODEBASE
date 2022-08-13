using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;


namespace ESimSolFinancial.Controllers
{

    public class SparePartsChallanController : Controller
    {

        #region Declartion
        SparePartsRequisition _oSparePartsRequisition = new SparePartsRequisition();
        SparePartsChallan _oSparePartsChallan = new SparePartsChallan();
        List<SparePartsChallan> _oSparePartsChallans = new List<SparePartsChallan>();
        SparePartsChallanDetail _oSparePartsChallanDetail = new SparePartsChallanDetail();
        List<SparePartsChallanDetail> _oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
        #endregion

        #region Collection Page
        public ActionResult ViewSparePartsChallans(int nSPRID, int buid)
        {
            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            oSparePartsRequisition = oSparePartsRequisition.Get(nSPRID, (int)Session[SessionInfo.currentUserID]);
            _oSparePartsChallans = SparePartsChallan.Gets("SELECT * FROM View_SparePartsChallan WHERE SparePartsRequisitionID = " + nSPRID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SparePartsRequisition = oSparePartsRequisition;
            return View(_oSparePartsChallans);
        }
        #endregion

        #region Add, Edit, Delete
        public ActionResult ViewSparePartsChallan(int nSPRID, int nChallanID, int buid)
        {
            if (nSPRID > 0)
            {
                _oSparePartsRequisition = _oSparePartsRequisition.Get(nSPRID, (int)Session[SessionInfo.currentUserID]);
                if (nChallanID > 0)
                {
                    _oSparePartsChallan = _oSparePartsChallan.Get(nChallanID, (int)Session[SessionInfo.currentUserID]);
                    _oSparePartsChallan.SparePartsChallanDetails = SparePartsChallanDetail.GetsBySparePartsChallanID(_oSparePartsChallan.SparePartsChallanID, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oSparePartsChallan = new SparePartsChallan();
                    _oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets("SELECT * FROM View_SparePartsRequisitionDetail WHERE SparePartsRequisitionID = " + _oSparePartsRequisition.SparePartsRequisitionID + " AND Quantity>0", (int)Session[SessionInfo.currentUserID]);
                    foreach (SparePartsRequisitionDetail oItem in _oSparePartsRequisition.SparePartsRequisitionDetails)
                    {
                        _oSparePartsChallanDetail = new SparePartsChallanDetail();
                        _oSparePartsChallanDetail.SparePartsChallanDetailID = 0;
                        _oSparePartsChallanDetail.SparePartsChallanID = 0;
                        _oSparePartsChallanDetail.SparePartsRequisitionDetailID = oItem.SparePartsRequisitionDetailID;
                        _oSparePartsChallanDetail.ProductID = oItem.ProductID;
                        _oSparePartsChallanDetail.ProductName = oItem.ProductName;
                        _oSparePartsChallanDetail.ProductCode = oItem.ProductCode;
                        _oSparePartsChallanDetail.RequisitionQty = oItem.Quantity;
                        _oSparePartsChallanDetail.ChallanQty = 0;
                        _oSparePartsChallanDetail.Remarks = oItem.Remarks;
                        _oSparePartsChallan.SparePartsChallanDetails.Add(_oSparePartsChallanDetail);
                    }
                }
            }
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.SparePartsChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SparePartsRequisition = _oSparePartsRequisition;
            return View(_oSparePartsChallan);
        }

        [HttpPost]
        public JsonResult GetsAllRequisitionproducts(SparePartsRequisition oSparePartsRequisition)
        {
            if (oSparePartsRequisition.SparePartsRequisitionID > 0)
            {
                _oSparePartsRequisition = _oSparePartsRequisition.Get(oSparePartsRequisition.SparePartsRequisitionID, (int)Session[SessionInfo.currentUserID]);

                _oSparePartsChallan = new SparePartsChallan();
                _oSparePartsRequisition.SparePartsRequisitionDetails = SparePartsRequisitionDetail.Gets("SELECT * FROM View_SparePartsRequisitionDetail WHERE SparePartsRequisitionID = " + _oSparePartsRequisition.SparePartsRequisitionID, (int)Session[SessionInfo.currentUserID]);
                foreach (SparePartsRequisitionDetail oItem in _oSparePartsRequisition.SparePartsRequisitionDetails)
                {
                    _oSparePartsChallanDetail = new SparePartsChallanDetail();
                    _oSparePartsChallanDetail.SparePartsChallanDetailID = 0;
                    _oSparePartsChallanDetail.SparePartsChallanID = 0;
                    _oSparePartsChallanDetail.SparePartsRequisitionDetailID = oItem.SparePartsRequisitionDetailID;
                    _oSparePartsChallanDetail.ProductID = oItem.ProductID;
                    _oSparePartsChallanDetail.ProductName = oItem.ProductName;
                    _oSparePartsChallanDetail.ProductCode = oItem.ProductCode;
                    _oSparePartsChallanDetail.RequisitionQty = oItem.Quantity;
                    _oSparePartsChallanDetail.ChallanQty = 0;
                    _oSparePartsChallanDetail.Remarks = oItem.Remarks;
                    _oSparePartsChallan.SparePartsChallanDetails.Add(_oSparePartsChallanDetail);
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsChallan.SparePartsChallanDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HTTP Save
        [HttpPost]
        public JsonResult Save(SparePartsChallan oSparePartsChallan)
        {

            _oSparePartsChallan = new SparePartsChallan();
            _oSparePartsChallanDetails = new List<SparePartsChallanDetail>();
            try
            {
                _oSparePartsChallan = oSparePartsChallan;
                _oSparePartsChallan = _oSparePartsChallan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSparePartsChallan = new SparePartsChallan();
                _oSparePartsChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Disburse(SparePartsChallan oSparePartsChallan)
        {

            _oSparePartsChallan = new SparePartsChallan();
            try
            {
                if (oSparePartsChallan.SparePartsChallanID > 0)
                {
                    _oSparePartsChallan = oSparePartsChallan;
                    _oSparePartsChallan = _oSparePartsChallan.Disburse((int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oSparePartsChallan = new SparePartsChallan();
                _oSparePartsChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSparePartsChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        //[HttpPost]
        //public JsonResult GetsByNameCRAndBUID(CRWiseSpareParts oCRWiseSpareParts)
        //{
        //    List<CRWiseSpareParts> _oCRWiseSparePartss = new List<CRWiseSpareParts>();
        //    try
        //    {
        //        _oCRWiseSparePartss = CRWiseSpareParts.GetsByNameCRAndBUIDWithLot(oCRWiseSpareParts.ProductName, oCRWiseSpareParts.CRID, oCRWiseSpareParts.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oCRWiseSparePartss = new List<CRWiseSpareParts>();
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oCRWiseSparePartss);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #region HTTP GET Delete
        [HttpGet]
        public JsonResult Delete(int nSparePartsChallanID)
        {
            string smessage = "";
            try
            {
                SparePartsChallan oSparePartsChallan = new SparePartsChallan();
                smessage = oSparePartsChallan.Delete(nSparePartsChallanID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                smessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(smessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        [HttpPost]
        public JsonResult GetsAllLotBalanceByProduct(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT SUM(Balance) AS Balance, ProductID, MUnitID, MUname from View_Lot Where ProductID IN(" + oLot.Params + ") AND WorkingUnitID = " + oLot.WorkingUnitID + " AND BUID = " + oLot.BUID + " Group By ProductID, MUnitID, MUname";
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsLotForAvilableStock(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sLotNo = (!string.IsNullOrEmpty(oLot.LotNo)) ? oLot.LotNo.Trim() : "";
                int nProductID = oLot.ProductID;

                string sSQL = "Select * from View_Lot Where LotID<>0 ";

                if (!string.IsNullOrEmpty(sLotNo))
                    sSQL = sSQL + " And LotNo Like '%" + sLotNo + "%'";
                if (nProductID > 0)
                    sSQL = sSQL + " And ProductID=" + nProductID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID=" + oLot.BUID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;

                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                _oLots = new List<Lot>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //#region Get Lots
        //[HttpPost]
        //public JsonResult GetLots(Lot oInvLot)
        //{
        //    List<Lot> oInvLots = new List<Lot>();
        //    if (oInvLot.ProductID > 0 && oInvLot.WorkingUnitID > 0 && (int)oInvLot.ParentType > 0)
        //    {
        //        //oInvLots = InvLot.Gets((int)oInvLot.ParentType, oInvLot.WorkingUnitID,oInvLot.ProductID, (int)Session[SessionInfo.currentUserID]);
        //    }

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oInvLots);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //#endregion


        //#region Gets Product & Lots
        //[HttpPost]
        //public JsonResult SearchProducts(Lot oLot)
        //{
        //    List<Product> oProducts = new List<Product>();
        //    Product oProduct = new Product();
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_Product";
        //        string sSQL1 = "";

        //        #region BUID
        //        if (oLot.BUID > 0)//if apply style configuration business unit
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductCategoryID IN (SELECT ProductCategoryID FROM  BUWiseProductCategory WHERE BUID =" + oLot.BUID.ToString() + ")";
        //        }
        //        #endregion

        //        #region ProductName

        //        if (!string.IsNullOrEmpty(oLot.ProductName))
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductName LIKE '%" + oLot.ProductName + "%'";
        //        }
        //        #endregion

        //        #region Deafult
        //        Global.TagSQL(ref sSQL1);
        //        sSQL1 = sSQL1 + " Activity=1";
        //        #endregion
        //        #region WorkingUnitID
        //        if (oLot.WorkingUnitID > 0) //Hare ProductID  Use as a StyleID
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductID in (Select ProductID from Lot where Balance>0 and WorkingUnitID=" + oLot.WorkingUnitID + ")";
        //        }
        //        #endregion

        //        #region Style Wise Suggested Product
        //        if (oLot.ProductID > 0) //Hare ProductID  Use as a StyleID
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductID IN (SELECT HH.ProductID FROM BillOfMaterial AS HH WHERE HH.TechnicalSheetID=" + oLot.ProductID.ToString() + ")";
        //        }
        //        #endregion

        //        sSQL = sSQL + sSQL1 + " Order By ProductName ASC";

        //        oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (oProducts.Count() <= 0) throw new Exception("No product found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        oProducts = new List<Product>();
        //        oProduct = new Product();
        //        oProduct.ErrorMessage = ex.Message;
        //        oProducts.Add(oProduct);
        //    }
        //    var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
        //    jsonResult.MaxJsonLength = int.MaxValue;
        //    return jsonResult;
        //}
        //[HttpPost]
        //public JsonResult SearchLots(Lot oLot)
        //{
        //    List<Lot> oLots = new List<Lot>();
        //    ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
        //    oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
        //    try
        //    {
        //        string sSQL = "SELECT * FROM View_Lot";
        //        string sSQL1 = "";

        //        #region BUID
        //        if (oLot.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " BUID =" + oLot.BUID.ToString();
        //        }
        //        #endregion

        //        #region ProductID
        //        if (oLot.ProductID > 0)
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " ProductID =" + oLot.ProductID.ToString();
        //        }
        //        #endregion

        //        #region WorkingUnitID
        //        if (oLot.WorkingUnitID > 0)
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " WorkingUnitID =" + oLot.WorkingUnitID.ToString();
        //        }
        //        #endregion

        //        #region LotNo
        //        if (oLot.LotNo == null) { oLot.LotNo = ""; }
        //        if (oLot.LotNo != "")
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " LotNo LIKE '%" + oLot.LotNo + "%'";
        //        }
        //        #endregion

        //        #region Deafult
        //        Global.TagSQL(ref sSQL1);
        //        sSQL1 = sSQL1 + " Balance>0";
        //        #endregion

        //        #region Style Wise Suggested Lot
        //        if (oLot.StyleID > 0)
        //        {
        //            Global.TagSQL(ref sSQL1);
        //            sSQL1 = sSQL1 + " LotID IN (SELECT HH.LotID FROM Lot AS HH WHERE HH.StyleID=" + oLot.StyleID.ToString() + ")";
        //        }
        //        #endregion

        //        sSQL = sSQL + sSQL1 + " ORDER BY LotID ASC";

        //        oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //        if (oLots.Count() <= 0) throw new Exception("No Lot found.");
        //    }
        //    catch (Exception ex)
        //    {
        //        oLots = new List<Lot>();
        //        oLot = new Lot();
        //        oLot.ErrorMessage = ex.Message;
        //        oLots.Add(oLot);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oLots);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Reports
        public ActionResult PrintSparePartsChallan(int id, int buid)
        {
            int nCompanyID = ((User)Session[SessionInfo.CurrentUser]).CompanyID;
            Company oCompany = new Company();
            oCompany = oCompany.Get(nCompanyID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);

            SparePartsChallan oSparePartsChallan = new SparePartsChallan();
            oSparePartsChallan = oSparePartsChallan.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            SparePartsRequisition oSparePartsRequisition = new SparePartsRequisition();
            oSparePartsRequisition = oSparePartsRequisition.Get(oSparePartsChallan.SparePartsRequisitionID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oSparePartsChallan.SparePartsChallanDetails = SparePartsChallanDetail.Gets("SELECT * FROM View_SparePartsChallanDetail Where SparePartsChallanID =" + id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            rptSparePartsChallan oReport = new rptSparePartsChallan();
            byte[] abytes = oReport.PrepareReport(oSparePartsChallan, oSparePartsRequisition, oCompany, oBusinessUnit);
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
        #endregion
    }
}