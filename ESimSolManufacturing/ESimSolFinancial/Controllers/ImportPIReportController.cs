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

namespace ESimSolFinancial.Controllers
{
    public class ImportPIReportController : Controller
    {
        ImportPI _oImportPI = new ImportPI();
        List<ImportPI> _oImportPIs = new List<ImportPI>();
        List<ImportPIDetail> _oImportPIDetails = new List<ImportPIDetail>();
        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();

        #region Actions
        public ActionResult ViewImportPIReports(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ImportPI).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oImportPIs = new List<ImportPI>();
            ViewBag.BUID = buid;
            return View(_oImportPIs);
        }
        #endregion

        #region View Advance Search
        [HttpPost]
        public JsonResult GetsSearchedData(ImportPI oImportPI)
        {
            _oImportPI.ImportPIDetails = new List<ImportPIDetail>();
            List<ImportPI> oImportPIs = new List<ImportPI>();
            try
            {
                string sSQL = GetSQL(oImportPI);
                oImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImportPIs = new List<ImportPI>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(ImportPI oImportPI)
        {
            int nIssueDate = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[0]);
            DateTime dStartDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[1]);
            DateTime dEndDate = Convert.ToDateTime(oImportPI.ErrorMessage.Split('~')[2]);
            string sImportPINo = oImportPI.ErrorMessage.Split('~')[3];
            string sSupplierIDs = oImportPI.ErrorMessage.Split('~')[4];
            int nBUID = Convert.ToInt32(oImportPI.ErrorMessage.Split('~')[5]);
            string sReturn1 = "SELECT * FROM View_ImportPI ";
            string sReturn = "";

            #region Supplier id
            if (sSupplierIDs != null && sSupplierIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SupplierID IN(" + sSupplierIDs + ")";
            }
            #endregion
            #region ImportPINo
            if (sImportPINo != null && sImportPINo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ImportPINo LIKE " + "'" + sImportPINo + "%'";
            }
            #endregion

            #region Date Wise
            if (nIssueDate > 0)
            {
                if (nIssueDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dStartDate + "'";
                }
                if (nIssueDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dStartDate + "'";
                }
                if (nIssueDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dStartDate + "'";
                }
                if (nIssueDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dStartDate + "'";
                }
                if (nIssueDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate>= '" + dStartDate + "' AND IssueDate < '" + dEndDate + "')";
                }
                if (nIssueDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (IssueDate< '" + dStartDate + "' OR IssueDate > '" + dEndDate + "')";
                }
            }
            #endregion

            #region Bu
            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            sReturn = sReturn1 + sReturn;

            return sReturn;
        }
        #endregion
        
        #region Printing
        [HttpPost]
        public ActionResult SetImportPIListData(ImportPI oImportPI)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oImportPI);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintImportPIs()
        {
            _oImportPI = new ImportPI();
            try
            {
                _oImportPI = (ImportPI)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_ImportPI WHERE ImportPIID IN (" + _oImportPI.Note + ") Order By ImportPIID";
                _oImportPI.ImportPIs = ImportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oImportPI = new ImportPI();
                _oImportPIs = new List<ImportPI>();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oImportPI.Company = oCompany;

            string Messge = "Import PI List";
            rptImportPIs oReport = new rptImportPIs();
            byte[] abytes = oReport.PrepareReport(_oImportPI, Messge);
            return File(abytes, "application/pdf");
        }
        //public ActionResult PrintoImportPIPreview(int id)
        //{
        //    _oImportPI = new ImportPI();
        //    _oImportPI = _oImportPI.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    _oImportPI.ImportPIDetails = ImportPIDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    oCompany.CompanyTitle = GetCompanyTitle(oCompany);
        //    Contractor oSupplier = new Contractor();
        //    oSupplier = oSupplier.Get(_oImportPI.SupplierID, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.Get(_oImportPI.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    rptImportPI oReport = new rptImportPI();//for plastic and integrated
        //    byte[] abytes = oReport.PrepareReport(_oImportPI, oSupplier,oCompany, oBusinessUnit);
        //    return File(abytes, "application/pdf");
        //}
        #endregion

        #region Get Product BU, User and Name wise ( write by Mahabub)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ImportPI,EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ImportPI, EnumProductUsages.Regular,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

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

        public Image GetCompanyTitle(Company oCompany)
        {
            if (oCompany.OrganizationTitle != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyImageTitle.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
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

