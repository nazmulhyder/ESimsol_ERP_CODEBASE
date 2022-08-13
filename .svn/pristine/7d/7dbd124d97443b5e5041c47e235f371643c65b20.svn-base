using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
using iTextSharp.text.pdf;
using System.Web.Services;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class LotMixingController : Controller
    {
        #region Declaration
        LotMixing _oLotMixing = new LotMixing();
        List<LotMixing> _oLotMixings = new List<LotMixing>();
        string _sErrorMessage = "";
        #endregion
        #region ActionResult
        public ActionResult ViewLotMixings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.LotMixing).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region Issue Stores
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            //oIssueStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.LotMixing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            _oLotMixings = new List<LotMixing>();
            _oLotMixings = LotMixing.Gets("SELECT * FROM View_LotMixing WHERE ISNULL(CompleteByID,0)=0", (int)Session[SessionInfo.currentUserID]);
            return View(_oLotMixings);
        }
        public ActionResult ViewLotMixing(int id, int buid)
        {
            _oLotMixing = new LotMixing();
            if (id > 0)
            {
                _oLotMixing = _oLotMixing.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oLotMixing.LotMixingDetails = LotMixingDetail.Gets("SELECT * FROM View_LotMixingDetail WHERE LotMixingID=" + id, (int)Session[SessionInfo.currentUserID]);
                _oLotMixing.LotMixingDetails_In = _oLotMixing.LotMixingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Receive).ToList();
                _oLotMixing.LotMixingDetails_Out = _oLotMixing.LotMixingDetails.Where(x => x.InOutTypeInt == (int)EnumInOutType.Disburse).ToList();
            }
            else
            {
                _oLotMixing.BUID = buid;
                _oLotMixing.PrepareByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            #region Issue Stores
            List<WorkingUnit> oIssueStores = new List<WorkingUnit>();
            //oIssueStores = WorkingUnit.Gets((int)Session[SessionInfo.currentUserID]);
            oIssueStores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.LotMixing, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]);
            #endregion

            ViewBag.IssueStores = oIssueStores;
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.LotMixingTypes = EnumObject.jGets(typeof(EnumColorType));
            return View(_oLotMixing);
        }

        [HttpPost]
        public JsonResult Save(LotMixing oLotMixing)
        {
            _oLotMixing = new LotMixing();
            try
            {
                _oLotMixing = oLotMixing;
                _oLotMixing = _oLotMixing.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLotMixing = new LotMixing();
                _oLotMixing.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotMixing);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(LotMixing oLotMixing)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLotMixing.Delete(oLotMixing.LotMixingID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Approve, UnApprove, Complete
        [HttpPost]
        public JsonResult Approve(LotMixing oLotMixing)
        {
            _oLotMixing = new LotMixing();
            _oLotMixing = oLotMixing.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotMixing);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UndoApprove(LotMixing oLotMixing)
        {
            _oLotMixing = new LotMixing();
            //oLotMixing.Status = EnumLotMixingStatus.Initialize;
            _oLotMixing = oLotMixing.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotMixing);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Complete(LotMixing oLotMixing)
        {
            _oLotMixing = new LotMixing();
            _oLotMixing = oLotMixing.Complete(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLotMixing);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Adv Search
        [HttpPost]
        public JsonResult AdvSearch(LotMixing oLotMixing)
        {
            List<LotMixing> oLotMixings = new List<LotMixing>();
            LotMixing _oLotMixing = new LotMixing();
            string sSQL = MakeSQL(oLotMixing);
            if (sSQL == "Error")
            {
                _oLotMixing = new LotMixing();
                _oLotMixing.ErrorMessage = "Please select a searching critaria.";
                oLotMixings = new List<LotMixing>();
            }
            else
            {
                oLotMixings = new List<LotMixing>();
                oLotMixings = LotMixing.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLotMixings.Count == 0)
                {
                    oLotMixings = new List<LotMixing>();
                }
            }
            var jsonResult = Json(oLotMixings, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(LotMixing oLotMixing)
        {
            string sParams = oLotMixing.Params;

            int nDateCriteria_Issue = 0,
                nStore=0;

            bool nYTComplete = false,
                 nYTApprove = false;

            string sSLNo = "",  
                   sLotNo = "", 
                   sProductIDs = "";
            
            DateTime dStart_Issue = DateTime.Today,
                     dEnd_Issue = DateTime.Today;
        
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sSLNo = sParams.Split('~')[nCount++];
                sLotNo = sParams.Split('~')[nCount++];
                sProductIDs = sParams.Split('~')[nCount++];
                nStore=Convert.ToInt32(sParams.Split('~')[nCount++]);
                nDateCriteria_Issue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Issue = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                nYTApprove=Convert.ToBoolean(sParams.Split('~')[nCount++]);
                nYTComplete = Convert.ToBoolean(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_LotMixing AS EB";
            string sReturn = "";

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.CreateDate", nDateCriteria_Issue, dStart_Issue, dEnd_Issue);
            #endregion

            #region sSLNo
            if (!string.IsNullOrEmpty(sSLNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.SLNoFULL LIKE '%" + sSLNo + "%'";
            }
            #endregion

            #region LotNo
            if (!string.IsNullOrEmpty(sLotNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn +" EB.LotMixingID IN ( SELECT LotMixingID FROM LotMixingDetail WHERE LotNo LIKE '%" + sLotNo + "%' )";
            }
            #endregion

            #region WorkingUnitID
            if (nStore>0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.WorkingUnitID =" + nStore;
            }
            #endregion

            #region Product IDs
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.LotMixingID IN ( SELECT LotMixingID FROM LotMixingDetail WHERE ProductID IN (" + sProductIDs + ")) ";
            }
            #endregion

            #region nYTApprove
            if (nYTApprove)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(ApproveByID,0)= 0";
            }
            #endregion

            #region nYTComplete
            if (nYTComplete)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ISNULL(CompleteByID,0)= 0";
            }
            #endregion
         
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Product
        [HttpPost]
        public JsonResult GetProducts(Lot oLot)
        {
            List<Product> oProducts = new List<Product>();
            try
            {

                if (oLot.ProductName != null && oLot.ProductName != "")
                {
                    oProducts = Product.Gets("SELECT * FROM View_Product WHERE ProductName LIKE '%" + oLot.ProductName + "%' AND ProductID IN (SELECT ProductID FROM Lot  WHERE Balance>0.1 and WorkingUnitID=" + oLot.WorkingUnitID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.LotMixing, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.Gets("SELECT * FROM View_Product WHERE ProductID IN (SELECT ProductID FROM Lot WHERE  Balance>0.1 and WorkingUnitID=" + oLot.WorkingUnitID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

                   // oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.LotMixing, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                Product oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets Lot
        [HttpPost]
        public JsonResult GetsLot(Lot oLot)
        {
            List<Lot> _oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_Lot WHERE LotID<>0 ";

                if (oLot.ProductID > 0)
                    sSQL = sSQL + " And ProductID = " + oLot.ProductID;
                if (oLot.BUID > 0)
                    sSQL = sSQL + " And BUID = " + oLot.BUID;
                if (oLot.WorkingUnitID > 0)
                    sSQL = sSQL + " And WorkingUnitID=" + oLot.WorkingUnitID;
                if (!string.IsNullOrEmpty(oLot.LotNo))
                    sSQL = sSQL + " And LotNo LIKE '%" + oLot.LotNo + "%'";

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
        #endregion

        #region Report
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
        public ActionResult PrintLotMixing(string sTemp)
        {
            List<LotMixing> oLotMixingList = new List<LotMixing>();
            LotMixing oLotMixing = new LotMixing();

            int nBUID;
            if (string.IsNullOrEmpty(sTemp))
            {
                throw new Exception("Nothing To Print.");
            }
            else
            {
                oLotMixing.Params = sTemp;
                string sSQL = MakeSQL(oLotMixing);
                oLotMixingList = LotMixing.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                nBUID = Convert.ToInt32(sTemp.Split('~')[11]);
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            //rptLotMixing oReport = new rptLotMixing();
            //byte[] abytes = oReport.PrepareReport(oLotMixingList, oCompany, oBusinessUnit, "Soft Winding Report", "");
            //return File(abytes, "application/pdf");
            return null;
        }
     #endregion
    }

}

