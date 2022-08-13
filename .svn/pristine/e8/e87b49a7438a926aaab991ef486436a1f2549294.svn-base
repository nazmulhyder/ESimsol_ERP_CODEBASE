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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class ServiceInvoiceController : Controller
    {
        #region Declaration
        ServiceInvoice _oServiceInvoice = new ServiceInvoice();
        List<ServiceInvoice> _oServiceInvoices = new List<ServiceInvoice>();
        List<Product> _oProducts = new List<Product>();
       // ServiceInvoiceImage _oServiceInvoiceImage = new ServiceInvoiceImage();
        #endregion

        #region Actions
        public ActionResult ViewServiceInvoices(int InvoiceType, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oServiceInvoices = new List<ServiceInvoice>();
            string sSQL = "SELECT * FROM View_ServiceInvoice AS HH WHERE ISNULL(HH.ApproveByID,0) = 0 AND ISNULL(InvoiceType,0)=" + InvoiceType + "   ORDER BY HH.ServiceInvoiceID ASC";
            _oServiceInvoices = ServiceInvoice.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceInvoiceTypes = EnumObject.jGets(typeof(EnumServiceInvoiceType)); ;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            ViewBag.InvoiceType = InvoiceType;
            return View(_oServiceInvoices);
        }
        public ActionResult ViewServiceInvoice(int id, int InvoiceType)
        {
            _oServiceInvoice = new ServiceInvoice();

            if (id > 0)
            {
                _oServiceInvoice = _oServiceInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oServiceInvoice.ServiceInvoiceDetails = ServiceInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oServiceInvoice.ServiceILaborChargeDetails = ServiceILaborChargeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oServiceInvoice.ServiceInvoiceTermsList = ServiceInvoiceTerms.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else if (InvoiceType == (int)EnumInvoiceType.EstimatedInvoice)
            {
                List<ServiceInvoiceTerms> oServiceInvoiceTerms = new List<ServiceInvoiceTerms>();
                oServiceInvoiceTerms = ServiceInvoiceTerms.Gets("SELECT * FROM ServiceInvoiceTerms WHERE ServiceInvoiceID = (SELECT ServiceInvoiceID FROM ServiceInvoice WHERE ISNULL(InvoiceType,0) = 1 AND  ServiceInvoiceID IN (SELECT top 1 HH.ServiceInvoiceID FROM ServiceInvoiceTerms AS HH ORder By HH.ServiceInvoiceTermsID DESC))", (int)Session[SessionInfo.currentUserID]);
                foreach (ServiceInvoiceTerms oItem in oServiceInvoiceTerms) { oItem.ServiceInvoiceTermsID = 0; oItem.ServiceInvoiceID = 0; }//Reset
                _oServiceInvoice.ServiceInvoiceTermsList = oServiceInvoiceTerms;
            }
            string sSQL = "SELECT * FROM Employee WHERE ISActive =1 AND EmployeeDesignationType IN(" + (int)EnumEmployeeDesignationType.Service + ")";
            ViewBag.ServiceEmployees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.ServiceInvoiceTypes = EnumObject.jGets(typeof(EnumServiceInvoiceType));            
            ViewBag.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.MUnits = MeasurementUnit.Gets((int)Session[SessionInfo.currentUserID]);

            List<EnumObject> oEnumObjects = EnumObject.jGets(typeof(EnumServiceILaborChargeType));
            List<EnumObject> oLaborChargeTypes = new List<EnumObject>();
            List<EnumObject> oPartsChargeTypes = new List<EnumObject>();
            foreach (EnumObject oItem in oEnumObjects)
            {
                if ((EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.None || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.Paying || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.Complementary || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.UpdateingCost)
                {
                    oPartsChargeTypes.Add(oItem);
                    oLaborChargeTypes.Add(oItem);
                }
                
                if ((EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.GiftAndTips || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.RepairingAndMaintenance || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.Warranty)
                {
                    oPartsChargeTypes.Add(oItem);
                }
                
                if ((EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.Free || (EnumServiceILaborChargeType)oItem.id == EnumServiceILaborChargeType.RepairingAndMaintenance) 
                {
                    oLaborChargeTypes.Add(oItem);
                }
            }
            ViewBag.LaborChargeTypes = oLaborChargeTypes;
            ViewBag.PartsChargeTypes = oPartsChargeTypes;
            ViewBag.InvoiceType = InvoiceType;
            return View(_oServiceInvoice);
        }
        public ActionResult ViewServiceInvoice_Update(int id)
        {
            _oServiceInvoice = new ServiceInvoice();
            if (id > 0)
            {
                _oServiceInvoice = _oServiceInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oServiceInvoice);
        }
        public ActionResult ViewServiceSchedulings(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.ServiceInvoice).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oServiceInvoices = new List<ServiceInvoice>();
            string sSQL = "SELECT * FROM View_ServiceInvoice WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + DateTime.Now.ToString("dd MMM yyyy") + "',106))";
            _oServiceInvoices = ServiceInvoice.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.ServiceInvoiceTypes = EnumObject.jGets(typeof(EnumServiceInvoiceType)); ;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oServiceInvoices);
        }
        public ActionResult AdvSearchServiceInvoice()
        {
            return PartialView();
        }
       
        [HttpPost]
        public JsonResult Save(ServiceInvoice oServiceInvoice)
        {
            _oServiceInvoice = new ServiceInvoice();
            try
            {
                _oServiceInvoice = oServiceInvoice;
                _oServiceInvoice = _oServiceInvoice.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceInvoice = new ServiceInvoice();
                _oServiceInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Revise(ServiceInvoice oServiceInvoice)
        {
            _oServiceInvoice = new ServiceInvoice();
            try
            {
                _oServiceInvoice = oServiceInvoice;
                _oServiceInvoice = _oServiceInvoice.Revise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceInvoice = new ServiceInvoice();
                _oServiceInvoice.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceInvoice);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsServiceInvoiceLog(ServiceInvoice oServiceInvoice)
        {
            _oServiceInvoices = new List<ServiceInvoice>();
            try
            {
                _oServiceInvoices = ServiceInvoice.GetsServiceInvoiceLog(oServiceInvoice.ServiceInvoiceID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceInvoice = new ServiceInvoice();
                _oServiceInvoice.ErrorMessage = ex.Message;
                _oServiceInvoices.Add(_oServiceInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintSILogPreview(int id)
        {
            _oServiceInvoice = new ServiceInvoice();
            _oServiceInvoice = _oServiceInvoice.GetLog(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceInvoiceDetails = ServiceInvoiceDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceILaborChargeDetails = ServiceILaborChargeDetail.GetsLog(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.ServiceInvoicePreview, (int)Session[SessionInfo.currentUserID]);

            rptServiceInvoice oReport = new rptServiceInvoice();
            byte[] abytes = oReport.PrepareReport(_oServiceInvoice, oCompany, oSignatureSetups);
            return File(abytes, "application/pdf");
        }

        [HttpPost]
        public JsonResult Delete(ServiceInvoice oServiceInvoice)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oServiceInvoice.Delete(oServiceInvoice.ServiceInvoiceID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(ServiceInvoice oServiceInvoice)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oServiceInvoice.Approve(oServiceInvoice, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GET/GETS FUNCTION
        [HttpPost] 
        public JsonResult GetsByStatus(ServiceInvoice oServiceInvoice)
        {
            List<ServiceInvoice> oServiceInvoices = new List<ServiceInvoice>();
            try
            {
                oServiceInvoices = ServiceInvoice.Gets("SELECT * FROM View_ServiceInvoice Where InvoiceStatus =" + oServiceInvoice.InvoiceStatusInt, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oServiceInvoice = new ServiceInvoice();
                oServiceInvoice.ErrorMessage = ex.Message;
                oServiceInvoices.Add(oServiceInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ServiceInvoiceSearchByNo(ServiceInvoice oServiceInvoice)
        {
            _oServiceInvoices = new List<ServiceInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_ServiceInvoice WHERE ServiceInvoiceNo LIKE '%" + oServiceInvoice.ServiceInvoiceNo + "%'";
                _oServiceInvoices = ServiceInvoice.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oServiceInvoice = new ServiceInvoice();
                _oServiceInvoice.ErrorMessage = ex.Message;
                _oServiceInvoices.Add(_oServiceInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oServiceInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByRegOrInvoiceNo(ServiceInvoice oServiceInvoice)
        {
            List<ServiceInvoice> oServiceInvoices = new List<ServiceInvoice>();
            try
            {
                string sSQL = "SELECT * FROM View_ServiceInvoice WHERE (ServiceInvoiceNo+VehicleRegNo) LIKE ('%" + oServiceInvoice.ServiceInvoiceNo + "%')";
                oServiceInvoices = ServiceInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oServiceInvoice = new ServiceInvoice();
                oServiceInvoice.ErrorMessage = ex.Message;
                oServiceInvoices.Add(oServiceInvoice);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceInvoices);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (Convert.ToInt32(oProduct.Params) > 0)
                {
                    string sSQL = "SELECT * FROM View_Product WHERE ProductID IN (SELECT ProductID FROM PartsRequisitionDetail WHERE PartsRequisitionID In (SELECT HH.PartsRequisitionID FROM PartsRequisition AS HH WHERE HH.ServiceOrderID =" + Convert.ToInt32(oProduct.Params) + "))";
                    if (oProduct.ProductName != null && oProduct.ProductName != "")
                    {
                        sSQL += " AND ProductName+ProductCode LIKE '%" + oProduct.ProductName + "%'";
                    }
                    _oProducts = Product.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    if (oProduct.ProductName != null && oProduct.ProductName != "")
                    {
                        _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                    }
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetProductsByName(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.BUID > 0)
                {
                    if (oProduct.ProductName != null && oProduct.ProductName != "")
                    {
                        _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                    }
                }
                
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        
        [HttpPost]
        public JsonResult GetOpenProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, oProduct.ProductName, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.ServiceInvoice, EnumProductUsages.Regular, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult GetServiceInvoiceDetailsByInvoiceID(ServiceInvoiceDetail oServiceInvoiceDetail)
        {
            List<ServiceInvoiceDetail> oServiceInvoiceDetails = new List<ServiceInvoiceDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_VehicleInvoiceDetail WHERE VehicleInvoiceID=" + oServiceInvoiceDetail.ServiceInvoiceID;
                oServiceInvoiceDetails = ServiceInvoiceDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oServiceInvoiceDetail = new ServiceInvoiceDetail();
                oServiceInvoiceDetail.ErrorMessage = ex.Message;
                oServiceInvoiceDetails.Add(oServiceInvoiceDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oServiceInvoiceDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region ADV SEARCH

        [HttpPost]
        public JsonResult AdvSearch(ServiceInvoice oServiceInvoice)
        {
            List<ServiceInvoice> oServiceInvoices = new List<ServiceInvoice>();
            ServiceInvoice _oServiceInvoice = new ServiceInvoice();
            string sSQL = MakeSQL(oServiceInvoice);
            if (sSQL == "Error")
            {
                _oServiceInvoice = new ServiceInvoice();
                _oServiceInvoice.ErrorMessage = "Please select a searching critaria.";
                oServiceInvoices = new List<ServiceInvoice>();
            }
            else
            {
                oServiceInvoices = new List<ServiceInvoice>();
                oServiceInvoices = ServiceInvoice.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oServiceInvoices.Count == 0)
                {
                    oServiceInvoices = new List<ServiceInvoice>();
                }
            }
            var jsonResult = Json(oServiceInvoices, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(ServiceInvoice oServiceInvoice)
        {
            string sParams = oServiceInvoice.Params;
            int InvoiceTypeInt = 0;
            int nDateCriteria_InvDate = 0;

            string sInvoiceNo = "",
                   sRegIDs = "",
                   sBuyerIDs = "";

            DateTime dStart_InvDate = DateTime.Today,
                     dEnd_InvDate = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sInvoiceNo = sParams.Split('~')[nCount++];
                nDateCriteria_InvDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_InvDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_InvDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sRegIDs = sParams.Split('~')[nCount++];
                sBuyerIDs = sParams.Split('~')[nCount++];
                InvoiceTypeInt = Convert.ToInt32(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_ServiceInvoice AS EB";
            string sReturn = "";

            
            #region InvoiceType
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " EB.InvoiceType = " + InvoiceTypeInt;
            #endregion

            #region InvNo
            if (!string.IsNullOrEmpty(sInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ServiceInvoiceNo LIKE '%" + sInvoiceNo + "%'";
            }
            #endregion

            #region DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " EB.ServiceInvoiceDate", nDateCriteria_InvDate, dStart_InvDate, dEnd_InvDate);
            #endregion

            #region Model IDs
            if (!string.IsNullOrEmpty(sRegIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.VehicleRegistrationID IN (" + sRegIDs + ") ";
            }
            #endregion

            #region Buyer IDs
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.CustomerID IN (" + sBuyerIDs + ") ";
            }
            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region PrintList
        public ActionResult PrintList(int buid)
        {
            _oServiceInvoices = ServiceInvoice.Gets((int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            //rptServiceInvoiceList oReport = new rptServiceInvoiceList();
            //byte[] abytes = oReport.PrepareReport(_oServiceInvoices, oCompany, oBusinessUnit, "Service Invoice");
            //return File(abytes, "application/pdf");
            return null;
        }
        public ActionResult PrintSchedules(int buid)
        {
            _oServiceInvoices = ServiceInvoice.Gets((int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(buid, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            //rptServiceInvoiceList oReport = new rptServiceInvoiceList();
            //byte[] abytes = oReport.PrepareReport_Schedules(_oServiceInvoices, oCompany, oBusinessUnit, "Service Scheduling");
            //return File(abytes, "application/pdf");
            return null;
        }
        public ActionResult PrintServiceInvoicePreview(int id)
        {
            _oServiceInvoice = new ServiceInvoice();
            _oServiceInvoice = _oServiceInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceInvoiceDetails = ServiceInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceILaborChargeDetails = ServiceILaborChargeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceInvoiceTermsList = ServiceInvoiceTerms.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.ServiceInvoicePreview, (int)Session[SessionInfo.currentUserID]);

            if (_oServiceInvoice.InvoiceType== EnumInvoiceType.ServiceInvoice)
            {
                rptServiceInvoice oReport = new rptServiceInvoice();
                byte[] abytes = oReport.PrepareReport(_oServiceInvoice, oCompany, oSignatureSetups);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptServiceInvoice oReport = new rptServiceInvoice();
                byte[] abytes = oReport.PrepareReport(_oServiceInvoice, oCompany, oSignatureSetups);
                return File(abytes, "application/pdf");


                //rptEstimateInvoice oReport = new rptEstimateInvoice();
                //byte[] abytes = oReport.PrepareReport(_oServiceInvoice, oCompany, oSignatureSetups);
                //return File(abytes, "application/pdf");
            }
         
        }
        public ActionResult PrintServiceInvoiceChallan(int id)
        {
            _oServiceInvoice = new ServiceInvoice();
            _oServiceInvoice = _oServiceInvoice.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceInvoiceDetails = ServiceInvoiceDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oServiceInvoice.ServiceILaborChargeDetails = ServiceILaborChargeDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptServiceInvoice oReport = new rptServiceInvoice();
            byte[] abytes = oReport.PrepareReportChallan(_oServiceInvoice, oCompany);
            return File(abytes, "application/pdf");
        }

        #region Get Company Logo
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

        #endregion

        #region ServiceInvoice register

        List<ServiceInvoiceRegister> _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
        ServiceInvoiceRegister _oServiceInvoiceRegister = new ServiceInvoiceRegister();
        string _sDateRange = "";
        public ActionResult ViewServiceInvoiceRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oServiceInvoice = new ServiceInvoice();
            
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise)//(EnumReportLayout)oItem.id == EnumReportLayout.ProductWise ||
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.BUID = buid;
            return View(_oServiceInvoice);
        }
        public ActionResult SetSessionSearchCriteria(ServiceInvoiceRegister oServiceInvoiceRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oServiceInvoiceRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintServiceInvoiceRegister(double ts)
        {
            ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
            string _sErrorMesage = "";
            try
            {
                _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
                oServiceInvoiceRegister = (ServiceInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oServiceInvoiceRegister);
                _oServiceInvoiceRegisters = ServiceInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oServiceInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oServiceInvoiceRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oServiceInvoiceRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptServiceInvoiceRegisters oReport = new rptServiceInvoiceRegisters();
                byte[] abytes = oReport.PrepareReport(_oServiceInvoiceRegisters, oCompany, oServiceInvoiceRegister.ReportLayout, _sDateRange);
                return File(abytes, "application/pdf");
                //return null;
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQL(ServiceInvoiceRegister oServiceInvoiceRegister)
        {
            //string _sDateRange = "";
            string sSearchingData = oServiceInvoiceRegister.ErrorMessage;
            EnumCompareOperator eServiceInvoiceDate = (EnumCompareOperator)Convert.ToInt32(sSearchingData.Split('~')[0]);
            DateTime dServiceInvoiceStartDate = Convert.ToDateTime(sSearchingData.Split('~')[1]);
            DateTime dServiceInvoiceEndDate = Convert.ToDateTime(sSearchingData.Split('~')[2]);
            #region make date range
            if (eServiceInvoiceDate == EnumCompareOperator.EqualTo)
            {
                _sDateRange = "Invoice Date: " + dServiceInvoiceStartDate.ToString("dd MMM yyyy");
            }
            else if (eServiceInvoiceDate == EnumCompareOperator.Between)
            {
                _sDateRange = "Invoice Date: " + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + " - To - " + dServiceInvoiceEndDate.ToString("dd MMM yyyy");
            }
            else if (eServiceInvoiceDate == EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "Invoice Date: Not Equal to " + dServiceInvoiceStartDate.ToString("dd MMM yyyy");
            }
            else if (eServiceInvoiceDate == EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "Invoice Date: Greater Than " + dServiceInvoiceStartDate.ToString("dd MMM yyyy");
            }
            else if (eServiceInvoiceDate == EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "Invoice Date: Smaller Than " + dServiceInvoiceStartDate.ToString("dd MMM yyyy");
            }
            else if (eServiceInvoiceDate == EnumCompareOperator.NotBetween)
            {
                _sDateRange = "Invoice Date: Not Between " + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + " - To - " + dServiceInvoiceEndDate.ToString("dd MMM yyyy");
            }
            #endregion

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region BusinessUnit
            //if (oServiceInvoiceRegister.BUID > 0)
            //{
            //    Global.TagSQL(ref sWhereCluse);
            //    sWhereCluse = sWhereCluse + " BUID =" + oServiceInvoiceRegister.BUID.ToString();
            //}
            #endregion

            #region PartsNo
            if (oServiceInvoiceRegister.PartsNo != null && oServiceInvoiceRegister.PartsNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " PartsNo LIKE'%" + oServiceInvoiceRegister.PartsNo + "%'";
            }
            #endregion

            #region VehicleRegNo
            if (oServiceInvoiceRegister.VehicleRegNo != null && oServiceInvoiceRegister.VehicleRegNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " VehicleRegNo LIKE'%" + oServiceInvoiceRegister.VehicleRegNo + "%'";
            }
            #endregion

            #region VehicleModelNo
            if (oServiceInvoiceRegister.VehicleModelNo != null && oServiceInvoiceRegister.VehicleModelNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " VehicleModelNo LIKE'%" + oServiceInvoiceRegister.VehicleModelNo + "%'";
            }
            #endregion

            #region EngineNo
            if (oServiceInvoiceRegister.EngineNo != null && oServiceInvoiceRegister.EngineNo != "")
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " EngineNo LIKE'%" + oServiceInvoiceRegister.EngineNo + "%'";
            }
            #endregion

            #region VehiclePartsID
            if (oServiceInvoiceRegister.VehiclePartsID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " VehiclePartsID =" + oServiceInvoiceRegister.VehiclePartsID.ToString();
            }
            #endregion

            #region CustomerID
            if (oServiceInvoiceRegister.CustomerID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " CustomerID =" + oServiceInvoiceRegister.CustomerID.ToString();
            }
            #endregion

            #region ChargeType
            if (oServiceInvoiceRegister.ChargeType > 0)
            {
                if (oServiceInvoiceRegister.ChargeType == (int)EnumServiceILaborChargeType.Complementary)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " ChargeType = " + oServiceInvoiceRegister.ChargeType.ToString();
                }
                else
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " ChargeType != " + oServiceInvoiceRegister.ChargeType.ToString();
                }
                
            }
            #endregion

            #region Service Invoice Date
            if (eServiceInvoiceDate != EnumCompareOperator.None)
            {
                if (eServiceInvoiceDate == EnumCompareOperator.EqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eServiceInvoiceDate == EnumCompareOperator.NotEqualTo)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eServiceInvoiceDate == EnumCompareOperator.GreaterThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eServiceInvoiceDate == EnumCompareOperator.SmallerThan)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eServiceInvoiceDate == EnumCompareOperator.Between)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
                else if (eServiceInvoiceDate == EnumCompareOperator.NotBetween)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ServiceInvoiceDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceStartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dServiceInvoiceEndDate.ToString("dd MMM yyyy") + "', 106))";
                }
            }
            #endregion

            #region Report Layout
            if (oServiceInvoiceRegister.ReportLayout == EnumReportLayout.DateWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ServiceInvoiceRegister ";
                sOrderBy = " ORDER BY  ServiceInvoiceDate, ServiceInvoiceID, ServiceInvoiceDetailID ASC";
            }

            else if (oServiceInvoiceRegister.ReportLayout == EnumReportLayout.PartyWise)
            {
                sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
                sSQLQuery = "SELECT * FROM View_ServiceInvoiceRegister ";
                sOrderBy = " ORDER BY  CustomerName, ServiceInvoiceID, ServiceInvoiceDetailID ASC";
            }
            //else if (oServiceInvoiceRegister.ReportLayout == EnumReportLayout.ProductWise)
            //{
            //    sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            //    sSQLQuery = "SELECT * FROM View_ServiceInvoiceRegister ";
            //    sOrderBy = " ORDER BY  PartsName, ServiceInvoiceID, ServiceInvoiceDetailID ASC";
            //}
            
            #endregion

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        public void ExportToExcelServiceInvoice()
        {
            ServiceInvoiceRegister oServiceInvoiceRegister = new ServiceInvoiceRegister();
            string _sErrorMesage = "";
            try
            {
                _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
                oServiceInvoiceRegister = (ServiceInvoiceRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oServiceInvoiceRegister);
                _oServiceInvoiceRegisters = ServiceInvoiceRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oServiceInvoiceRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oServiceInvoiceRegisters = new List<ServiceInvoiceRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                if (oServiceInvoiceRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oServiceInvoiceRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                double GrandTotalQty = 0, GrandTotalUnitPrice = 0, GrandTotalPrice = 0;
                int count = 0, num = 0;
                double SubTotalQty = 0, SubTotalUnitPrice = 0, SubTotalTotalPrice = 0;
                double TotalQty = 0, TotalUnitPrice = 0, TotalTotalPrice = 0; ;
                string sQCNo = "";

                if (oServiceInvoiceRegister.ReportLayout == EnumReportLayout.DateWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Date Wise Service Invoice Register(Details)");
                        sheet.Name = "Date Wise Service Invoice Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 12; //part no
                        sheet.Column(4).Width = 20; //product name
                        sheet.Column(5).Width = 15; //party
                        sheet.Column(6).Width = 15; //reg no
                        sheet.Column(7).Width = 15; //model
                        sheet.Column(8).Width = 10; //engine
                        sheet.Column(9).Width = 10; //chassis
                        sheet.Column(10).Width = 12; //service type
                        sheet.Column(11).Width = 8; //M Unit
                        sheet.Column(12).Width = 10; //Qty
                        sheet.Column(13).Width = 10; //Unit Price
                        sheet.Column(14).Width = 10; //Total price
                        sheet.Column(15).Width = 15; //Remarks

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Date Wise Service Invoice Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Part No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Party"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Reg. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Model No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Engine No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Chassis No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Service Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Total Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oServiceInvoiceRegisters.Count > 0)
                        {
                            var data = _oServiceInvoiceRegisters.GroupBy(x => new { x.ServiceInvoiceDateInString }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                ServiceInvoiceDate = key.ServiceInvoiceDateInString,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalQty = 0; GrandTotalUnitPrice = 0; GrandTotalPrice = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = "Invoice Date : @ " + oData.ServiceInvoiceDate; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                                TotalQty = 0; TotalUnitPrice = 0; TotalTotalPrice = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    //if (sQCNo != "")
                                    //{
                                    //    if (sQCNo != oItem.QCNo && count > 1)
                                    //    {
                                    //        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        rowIndex = rowIndex + 1;
                                    //        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                    //    }
                                    //}
                                    #endregion

                                    num++;
                                    //if (sQCNo != oItem.QCNo)
                                    //{
                                    //    num++;
                                    //    int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                    //    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //}
                                    cell = sheet.Cells[rowIndex, 2];   cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 3];   cell.Value = oItem.PartsNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 4];   cell.Value = oItem.PartsName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5];   cell.Value = oItem.CustomerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 6];   cell.Value = oItem.VehicleRegNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 7];   cell.Value = oItem.VehicleModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8];   cell.Value = oItem.EngineNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9];   cell.Value = oItem.ChassisNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10];   cell.Value = oItem.ServiceOrderTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11];   cell.Value = oItem.MUName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12];   cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalQty += oItem.Qty;
                                    TotalQty += oItem.Qty;
                                    GrandTotalQty += oItem.Qty;

                                    cell = sheet.Cells[rowIndex, 13];   cell.Value = oItem.UnitPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalUnitPrice += oItem.UnitPrice;
                                    TotalUnitPrice += oItem.UnitPrice;
                                    GrandTotalUnitPrice += oItem.UnitPrice;

                                    cell = sheet.Cells[rowIndex, 14];   cell.Value = oItem.TotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalTotalPrice += oItem.TotalPrice;
                                    TotalTotalPrice += oItem.TotalPrice;
                                    GrandTotalPrice += oItem.TotalPrice;

                                    cell = sheet.Cells[rowIndex, 15];   cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex++;
                                    //sQCNo = oItem.QCNo;
                                }
                                #region subtotal
                                //if (sQCNo != "")
                                //{
                                //    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    rowIndex = rowIndex + 1;
                                //    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                //}
                                #endregion

                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Date Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = TotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;//TotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 14]; cell.Value = TotalTotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;//GrandTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Date_Wise_Service_Invoice_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                else if (oServiceInvoiceRegister.ReportLayout == EnumReportLayout.PartyWise)
                {
                    #region full excel
                    int rowIndex = 2;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;

                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Party Wise Service Invoice Register(Details)");
                        sheet.Name = "Party Wise Service Invoice Register(Details)";
                        sheet.Column(2).Width = 5; //SL
                        sheet.Column(3).Width = 12; //part no
                        sheet.Column(4).Width = 20; //product name
                        sheet.Column(5).Width = 12; //Invoice date
                        sheet.Column(6).Width = 15; //reg no
                        sheet.Column(7).Width = 15; //model
                        sheet.Column(8).Width = 10; //engine
                        sheet.Column(9).Width = 10; //chassis
                        sheet.Column(10).Width = 12; //service type
                        sheet.Column(11).Width = 8; //M Unit
                        sheet.Column(12).Width = 10; //Qty
                        sheet.Column(13).Width = 10; //Unit Price
                        sheet.Column(14).Width = 10; //Total price
                        sheet.Column(15).Width = 15; //Remarks

                        #region Report Header
                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "Party Wise Service Invoice Register(Details)"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 1;

                        sheet.Cells[rowIndex, 2, rowIndex, 15].Merge = true;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        rowIndex = rowIndex + 2;
                        #endregion

                        #region Column Header

                        cell = sheet.Cells[rowIndex, 2]; cell.Value = "#SL"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = "Part No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = "Product"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = "Invoice Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = "Reg. No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = "Model No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = "Engine No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = "Chassis No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = "Service Type"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 11]; cell.Value = "M Unit"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = "Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = "Unit Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = "Total Price"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = "Remarks"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        rowIndex = rowIndex + 1;
                        #endregion

                        #region group by
                        if (_oServiceInvoiceRegisters.Count > 0)
                        {
                            var data = _oServiceInvoiceRegisters.GroupBy(x => new { x.CustomerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                            {
                                CustomerName = key.CustomerName,
                                Results = grp.ToList() //All data
                            });
                        #endregion

                            #region Report Data
                            GrandTotalQty = 0; GrandTotalUnitPrice = 0; GrandTotalPrice = 0;

                            foreach (var oData in data)
                            {
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = "Party : @ " + oData.CustomerName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;

                                count = 0; num = 0;
                                SubTotalQty = 0; SubTotalUnitPrice = 0; SubTotalTotalPrice = 0;
                                TotalQty = 0; TotalUnitPrice = 0; TotalTotalPrice = 0;

                                foreach (var oItem in oData.Results)
                                {
                                    count++;
                                    #region subtotal
                                    //if (sQCNo != "")
                                    //{
                                    //    if (sQCNo != oItem.QCNo && count > 1)
                                    //    {
                                    //        cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                    //        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //        rowIndex = rowIndex + 1;
                                    //        SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                    //    }
                                    //}
                                    #endregion

                                    num++;
                                    //if (sQCNo != oItem.QCNo)
                                    //{
                                    //    num++;
                                    //    int rowCount = (oData.Results.Count(x => x.QCNo == oItem.QCNo) - 1);
                                    //    cell = sheet.Cells[rowIndex, 2, rowIndex + rowCount, 2]; cell.Merge = true; cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 3, rowIndex + rowCount, 3]; cell.Merge = true; cell.Value = oItem.QCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 4, rowIndex + rowCount, 4]; cell.Merge = true; cell.Value = oItem.QCByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 5, rowIndex + rowCount, 5]; cell.Merge = true; cell.Value = oItem.StoreName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //    cell = sheet.Cells[rowIndex, 6, rowIndex + rowCount, 6]; cell.Merge = true; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    //}
                                    cell = sheet.Cells[rowIndex, 2];   cell.Value = num; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 3];   cell.Value = oItem.PartsNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 4];   cell.Value = oItem.PartsName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 5];   cell.Value = oItem.ServiceInvoiceDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 6];   cell.Value = oItem.VehicleRegNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 7];   cell.Value = oItem.VehicleModelNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 8];   cell.Value = oItem.EngineNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9];   cell.Value = oItem.ChassisNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 10];   cell.Value = oItem.ServiceOrderTypeSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 11];   cell.Value = oItem.MUName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 12];   cell.Value = oItem.Qty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;                                    
                                    SubTotalQty += oItem.Qty;
                                    TotalQty += oItem.Qty;
                                    GrandTotalQty += oItem.Qty;

                                    cell = sheet.Cells[rowIndex, 13];   cell.Value = oItem.UnitPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalUnitPrice += oItem.UnitPrice;
                                    TotalUnitPrice += oItem.UnitPrice;
                                    GrandTotalUnitPrice += oItem.UnitPrice;

                                    cell = sheet.Cells[rowIndex, 14];   cell.Value = oItem.TotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                    SubTotalTotalPrice += oItem.TotalPrice;
                                    TotalTotalPrice += oItem.TotalPrice;
                                    GrandTotalPrice += oItem.TotalPrice;

                                    cell = sheet.Cells[rowIndex, 15];   cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    rowIndex++;
                                    //sQCNo = oItem.QCNo;
                                }
                                #region subtotal
                                //if (sQCNo != "")
                                //{
                                //    cell = sheet.Cells[rowIndex, 2, rowIndex, 8]; cell.Merge = true; cell.Value = "Sub Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 9]; cell.Value = SubTotalOrderQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 10]; cell.Value = SubTotalQCPassQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    cell = sheet.Cells[rowIndex, 11]; cell.Value = SubTotalRejectQty.ToString("#,##0.00;(#,##0.00)"); cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                //    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                //    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                //    rowIndex = rowIndex + 1;
                                //    SubTotalOrderQty = 0; SubTotalQCPassQty = 0; SubTotalRejectQty = 0;
                                //}
                                #endregion

                                #region total
                                cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Party Wise Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 12]; cell.Value = TotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;//TotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 14]; cell.Value = TotalTotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                rowIndex = rowIndex + 1;
                                #endregion

                                cell = sheet.Cells[rowIndex, 2, rowIndex, 15]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                rowIndex = rowIndex + 1;
                            }

                            #region grand total
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Grand Total : "; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 12]; cell.Value = GrandTotalQty; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;//GrandTotalUnitPrice.ToString("#,##0.00;(#,##0.00)")
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 14]; cell.Value = GrandTotalPrice; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            rowIndex = rowIndex + 1;
                            #endregion

                            #endregion

                            Response.ClearContent();
                            Response.BinaryWrite(excelPackage.GetAsByteArray());
                            Response.AddHeader("content-disposition", "attachment; filename=Party_Wise_Service_Invoice_Register.xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Flush();
                            Response.End();
                        }

                    }
                    #endregion
                }
                
            }
        }

        #endregion
    }

}
