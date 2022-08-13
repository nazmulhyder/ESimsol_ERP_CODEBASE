using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;
namespace ESimSolFinancial.Controllers
{
    public class LoanProductRateController : Controller
    {
        public ActionResult ViewLoanProductRates(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<LoanProductRate> _oLoanProductRate = new List<LoanProductRate>();
            string sSQL = "SELECT * FROM View_LoanProductRate  ORDER BY LoanProductRateID ASC";
            _oLoanProductRate = LoanProductRate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oLoanProductRate);
        }

        public ActionResult ViewLoanProductRate(int id, int buid)
        {

            LoanProductRate _oLoanProductRate = new LoanProductRate();
            if (id > 0)
            {
                _oLoanProductRate = _oLoanProductRate.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            List<Currency> _oCurrencys = new List<Currency>();
            _oCurrencys = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CurrencyTypes = _oCurrencys;
            return View(_oLoanProductRate);

        }

        [HttpPost]
        public JsonResult SearchDataByDate(LoanProductRate oLoanProductRate)
        {
            List<LoanProductRate> _oLoanProductRates = new List<LoanProductRate>();
            try
            {
                string sSearchingData = oLoanProductRate.Note;
                DateTime dStartDate = Convert.ToDateTime(sSearchingData.Split('~')[0]);
                DateTime dEndDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
                string sReturn1 = "SELECT * FROM View_LoanProductRate";
                string sReturn = "";
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dStartDate.ToString("dd MMM yyyy HH:mm:ss") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dEndDate.ToString("dd MMM yyyy HH:mm:ss") + "',106))";
                string sSQL = sReturn1 + sReturn;
                _oLoanProductRates = LoanProductRate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                LoanProductRate _oLoanProductRate = new LoanProductRate();
                oLoanProductRate.ErrorMessage = ex.Message;
                _oLoanProductRates.Add(oLoanProductRate);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoanProductRates);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SearchDataByProduct(LoanProductRate oLoanProductRate)
        {
            List<LoanProductRate> _oLoanProductRates = new List<LoanProductRate>();
            try
            {
                string sSearchingData = oLoanProductRate.Note;
                string sSQL="";
                string ProductName = Convert.ToString(sSearchingData.Split('~')[0]);
                string sReturn1 = "SELECT * FROM View_LoanProductRate";
                string sReturn = "";
                if (ProductName != "" && ProductName != null)
                {
                 
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductName LIKE '%" + ProductName + "%'";

                }
                else
                {
                    string ProductCode = Convert.ToString(sSearchingData.Split('~')[1]);
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ProductCode LIKE '%" + ProductCode + "%'";
                }
                sSQL = sReturn1 + sReturn;
                _oLoanProductRates = LoanProductRate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                LoanProductRate _oLoanProductRate = new LoanProductRate();
                oLoanProductRate.ErrorMessage = ex.Message;
                _oLoanProductRates.Add(oLoanProductRate);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoanProductRates);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(LoanProductRate oLoanProductRate)
        {
            LoanProductRate _oLoanProductRate = new LoanProductRate();
            try
            {

                _oLoanProductRate = oLoanProductRate.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLoanProductRate = new LoanProductRate();
                _oLoanProductRate.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLoanProductRate);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(LoanProductRate oLoanProductRate)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oLoanProductRate.Delete(oLoanProductRate.LoanProductRateID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SetLoanProductRateListData(LoanProductRate oLoanProductRate)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oLoanProductRate);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintLoanProductRates()
        {
            LoanProductRate _oLoanProductRate = new LoanProductRate();
            List<LoanProductRate> _oLoanProductRates = new List<LoanProductRate>();
            try
            {
                _oLoanProductRate = (LoanProductRate)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_LoanProductRate WHERE LoanProductRateID IN (" + _oLoanProductRate.ErrorMessage + ") Order By LoanProductRateID";
                _oLoanProductRates = LoanProductRate.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLoanProductRate = new LoanProductRate();
                _oLoanProductRates = new List<LoanProductRate>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            rptLoanProductRates oReport = new rptLoanProductRates();
            byte[] abytes = oReport.PrepareReport(_oLoanProductRates, oCompany);
            return File(abytes, "application/pdf");
        }


        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
         List <Product> _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.LoanProductRate, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.LoanProductRate, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                Product _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oProducts);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        public Image GetCompanyLogo(Company oCompany)
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

    }
}