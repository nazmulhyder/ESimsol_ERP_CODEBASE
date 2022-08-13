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
using System.Drawing.Printing;
using System.Reflection;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class YarnRequisitionController : Controller
    {
        #region Declaration

        YarnRequisition _oYarnRequisition = new YarnRequisition();
        List<YarnRequisition> _oYarnRequisitions = new List<YarnRequisition>();
        YarnRequisitionDetail _oYarnRequisitionDetail = new YarnRequisitionDetail();
        List<YarnRequisitionDetail> _oYarnRequisitionDetails = new List<YarnRequisitionDetail>();        
        #endregion

        #region Actions

        public ActionResult ViewYarnRequisitions(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.YarnRequisition).ToString() + "," + ((int)EnumModuleName.KnittingYarnChallan).ToString() + "," + ((int)EnumModuleName.KnittingFabricReceive).ToString() + "," + ((int)EnumModuleName.KnittingYarnReturn).ToString() + "," + ((int)EnumModuleName.KnittingComposition).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_YarnRequisition AS HH WHERE  ISNULL(HH.BUID,0) = " + buid.ToString() + " ORDER BY HH.YarnRequisitionID ASC";
            _oYarnRequisitions = new List<YarnRequisition>();
            _oYarnRequisitions = YarnRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oYarnRequisitions);
        }

        public ActionResult ViewYarnRequisition(int id, int buid)
        {
            _oYarnRequisition = new YarnRequisition();
            if (id > 0)
            {
                _oYarnRequisition = _oYarnRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oYarnRequisition.YarnRequisitionDetails = YarnRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oYarnRequisition.YarnRequisitionProducts = YarnRequisitionProduct.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }            
            List<BusinessSession> oBusinessSessions = new List<BusinessSession>();
            oBusinessSessions.Add(new BusinessSession { BusinessSessionID=0, SessionName="Session" });
            oBusinessSessions.AddRange( BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]));
            ViewBag.BusinessSessions =oBusinessSessions;
            ViewBag.Merchandisers = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oYarnRequisition);
        }

        [HttpPost]
        public JsonResult Save(YarnRequisition oYarnRequisition)
        {
            _oYarnRequisition = new YarnRequisition();
            try
            {
                _oYarnRequisition = oYarnRequisition.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oYarnRequisition = new YarnRequisition();
                _oYarnRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oYarnRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(YarnRequisition oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                YarnRequisition oYarnRequisition = new YarnRequisition();
                sFeedBackMessage = oYarnRequisition.Delete(oJB.YarnRequisitionID, (int)Session[SessionInfo.currentUserID]);
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

        
        #region Advance search 
        [HttpPost]
        public JsonResult GetsByRequisitionNo(YarnRequisition oYarnRequisition)
        {
            List<YarnRequisition> oYarnRequisitions = new List<YarnRequisition>();
            try
            {
                string sSQL = "SELECT * FROM view_YarnRequisition AS HH WHERE HH.RequisitionNo LIKE '%" + oYarnRequisition.RequisitionNo + "%' AND HH.BUID = " + oYarnRequisition.BUID.ToString() + " ORDER BY HH.RequisitionNo ASC";
                oYarnRequisitions = YarnRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oYarnRequisitions = new List<YarnRequisition>();
                oYarnRequisition = new YarnRequisition();
                oYarnRequisition.ErrorMessage = ex.Message;
                oYarnRequisitions.Add(oYarnRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oYarnRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsByStyleOROrderRecapNo(YarnRequisition oYarnRequisition)
        {
            List<YarnRequisition> oYarnRequisitions = new List<YarnRequisition>();
            try
            {
                string sSQL = "SELECT * FROM View_YarnRequisition AS HH WHERE HH.YarnRequisitionID IN (SELECT YRD.YarnRequisitionID FROM View_YarnRequisitionDetail AS YRD WHERE YRD.StyleNo LIKE '%" + oYarnRequisition.Params + "%' OR YRD.OrderRecapNo LIKE '%" + oYarnRequisition.Params + "%') AND BUID = 0 ORDER BY HH.YarnRequisitionID ASC";
                oYarnRequisitions = YarnRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oYarnRequisitions = new List<YarnRequisition>();
                oYarnRequisition = new YarnRequisition();
                oYarnRequisition.ErrorMessage = ex.Message;
                oYarnRequisitions.Add(oYarnRequisition);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oYarnRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<YarnRequisition> oYarnRequisitions = new List<YarnRequisition>();
            try
            {
                string sSQL = GetSQL(Temp);
                oYarnRequisitions = YarnRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oYarnRequisitions = new List<YarnRequisition>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oYarnRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         private string GetSQL(string sTemp)
        {
            string sYarnRequisitionNo = sTemp.Split('~')[0];
            int nOrderDate = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dOrderDateStart = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dOrderDateEnd = Convert.ToDateTime(sTemp.Split('~')[3]);
            int nStartDate = Convert.ToInt32(sTemp.Split('~')[4]);
            DateTime dStartDateStart = Convert.ToDateTime(sTemp.Split('~')[5]);
            DateTime dStartDateEnd = Convert.ToDateTime(sTemp.Split('~')[6]);
            int nCompleteDate = Convert.ToInt32(sTemp.Split('~')[7]);
            DateTime dCompleteDateStart = Convert.ToDateTime(sTemp.Split('~')[8]);
            DateTime dCompleteDateEnd = Convert.ToDateTime(sTemp.Split('~')[9]);
            int nFactoryID = Convert.ToInt32(sTemp.Split('~')[10]);
            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[11]);
            int nBusinessSessionID = Convert.ToInt32(sTemp.Split('~')[12]);

            int nStyleID = Convert.ToInt32(sTemp.Split('~')[13]);
            int nFabricID = Convert.ToInt32(sTemp.Split('~')[14]);
            int nYarnID = Convert.ToInt32(sTemp.Split('~')[15]);

            string sMICDia = sTemp.Split('~')[16];
            string sFinishDia = sTemp.Split('~')[17];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[18]);

            string sReturn1 = "SELECT * FROM view_YarnRequisition";
            string sReturn = "";

            if (!string.IsNullOrEmpty(sYarnRequisitionNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderNo LIKE '%" + sYarnRequisitionNo + "%' ";
            }

            #region order date
            if (nOrderDate > 0)
            {
                if (nOrderDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dOrderDateStart.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))>= CONVERT(Date,Convert(Varchar(12),'" + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),OrderDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nOrderDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), OrderDate,106))< 'CONVERT(Date,Convert(Varchar(12)," + dOrderDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),OrderDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dOrderDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Start Date

            if (nStartDate > 0)
            {
                if (nStartDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nStartDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), StartDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dStartDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),StartDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dStartDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            #region  Complete Date

            if (nCompleteDate > 0)
            {
                if (nCompleteDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) = CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) != CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) >= CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) AND CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
                if (nCompleteDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "CONVERT(Date,Convert(Varchar(12), ApproxCompleteDate,106)) < CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateStart.ToString("dd MMM yyyy") + "',106)) OR CONVERT(Date,Convert(Varchar(12),ApproxCompleteDate,106)) > CONVERT(Date,Convert(Varchar(12),'" + dCompleteDateEnd.AddDays(1).ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            if (nFactoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FactoryID = " + nFactoryID;
            }

            if (nBUID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }

            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM YarnRequisitionDetail WHERE StyleID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE BuyerID = " + nBuyerID + "))";
            }

            if (nBusinessSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nBusinessSessionID;
            }
            if (nStyleID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM YarnRequisitionDetail WHERE StyleID = " + nStyleID + " )";
            }
            if (nFabricID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM YarnRequisitionDetail WHERE FabricID = " + nFabricID + " )";
            }
            if (nYarnID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM KnittingYarn WHERE YarnID = " + nYarnID + " )";
            }
            if (!string.IsNullOrEmpty(sMICDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM YarnRequisitionDetail WHERE MICDia LIKE '%" + sMICDia + "%' )";
            }
            if (!string.IsNullOrEmpty(sFinishDia))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " YarnRequisitionID IN (SELECT YarnRequisitionID FROM YarnRequisitionDetail WHERE FinishDia LIKE '%" + sFinishDia + "%' )";
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Get

        public JsonResult GetYarnCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    //oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.YarnRequisition, EnumProductUsages.Yarn, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oProducts = Product.Gets("SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND HH.ProductName LIKE '%" + oProduct.ProductName + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }   
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.YarnRequisition, EnumProductUsages.Yarn, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFabricCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.YarnRequisition, EnumProductUsages.Fabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.YarnRequisition, EnumProductUsages.Fabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetsDetailsByID(YarnRequisitionDetail oYarnRequisitionDetail)//Id=ContractorID
        //{
        //    try
        //    {
        //        string Ssql = "SELECT*FROM View_YarnRequisitionDetail WHERE YarnRequisitionID=" + oYarnRequisitionDetail.YarnRequisitionID + " ";
        //        _oYarnRequisitionDetails = new List<YarnRequisitionDetail>();
        //        _oYarnRequisitionDetail.YarnRequisitionDetails = YarnRequisitionDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oYarnRequisitionDetail.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oYarnRequisitionDetail);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #region Approve & Finish
        [HttpPost]
        public JsonResult Approve(YarnRequisition oYarnRequisition)
        {
            _oYarnRequisition = new YarnRequisition();
            try
            {
                _oYarnRequisition = oYarnRequisition.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oYarnRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oYarnRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UnApprove(YarnRequisition oYarnRequisition)
        {
            _oYarnRequisition = new YarnRequisition();
            try
            {
                _oYarnRequisition = oYarnRequisition.UnApprove((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oYarnRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oYarnRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region print

        public ActionResult YarnRequisitionPrintPreview(int id)
        {
            BusinessUnit oBU = new BusinessUnit();
            _oYarnRequisition = new YarnRequisition();
            if (id > 0)
            {
                _oYarnRequisition = _oYarnRequisition.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oYarnRequisition.YarnRequisitionDetails = YarnRequisitionDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oYarnRequisition.YarnRequisitionProducts = YarnRequisitionProduct.Gets(id, (int)Session[SessionInfo.currentUserID]);                
            }

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.YarnRequisitionPreview, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oYarnRequisition.BUID > 0)
            {
                oBU = oBU.Get(_oYarnRequisition.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }            
            byte[] abytes;
            rptYarnRequisitionPreview oReport = new rptYarnRequisitionPreview();
            abytes = oReport.PrepareReport(_oYarnRequisition, oSignatureSetups, oCompany);
            return File(abytes, "application/pdf");
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

        [HttpPost]
        public ActionResult SetYarnRequisitionListData(YarnRequisition oYarnRequisition)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oYarnRequisition);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintYarnRequisitions()
        {
            //_oYarnRequisition = new YarnRequisition();
            //try
            //{
            //    _oYarnRequisition = (YarnRequisition)Session[SessionInfo.ParamObj];
            //    string sSQL = "SELECT * FROM View_YarnRequisition WHERE YarnRequisitionID IN (" + _oYarnRequisition.ErrorMessage + ") Order By YarnRequisitionID";
            //    _oYarnRequisitions = YarnRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            //}
            //catch (Exception ex)
            //{
            //    _oYarnRequisition = new YarnRequisition();
            //    _oYarnRequisitions = new List<YarnRequisition>();
            //}
            //Company oCompany = new Company();
            //oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            //oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            ////_oYarnRequisition.Company = oCompany;

            //rptYarnRequisitions oReport = new rptYarnRequisitions();
            //byte[] abytes = oReport.PrepareReport(_oYarnRequisitions, oCompany);
            //return File(abytes, "application/pdf");

            return null;
        }

        
        #endregion
    }

}
