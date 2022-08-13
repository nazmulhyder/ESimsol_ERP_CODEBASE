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
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;

namespace ESimSolFinancial.Controllers
{
    public class SparePartsRequestController : Controller
    {

        #region Declaration
        PurchaseRequisition _oPurchaseRequisition = new PurchaseRequisition();
        List<PurchaseRequisition> _oPurchaseRequisitions = new List<PurchaseRequisition>();
        PurchaseRequisitionDetail _oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
        List<PurchaseRequisitionDetail> _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();
        ClientOperationSetting _oClientOperationSetting = new ClientOperationSetting();
        string _sSQL = "";
        #endregion
       
        #region Actions
        public ActionResult ViewSparePartsRequests(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseRequisition).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oPurchaseRequisitions = new List<PurchaseRequisition>();
            string sSQL = "SELECT * FROM View_PurchaseRequisition WHERE  PRID IN(SELECT HH.ObjectRefID FROM View_ApprovalHistory AS HH WHERE HH.ModuleID = " + (int)EnumModuleName.PurchaseRequisition + " AND HH.SendToPersonID = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ") AND  ApprovalSequence  = (SELECT top 1 AP.ApprovalSequence FROM View_ApprovalHeadPerson AS AP WHERE AP.ApprovalHeadID IN (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.PurchaseRequisition + " ) AND AP.UserID = " + ((User)Session[SessionInfo.CurrentUser]).UserID + " )";
            _oPurchaseRequisitions = PurchaseRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPurchaseRequisitions = _oPurchaseRequisitions.OrderByDescending(X => X.PRDate).ToList();
            ViewBag.COs = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.buid = buid;
            return View(_oPurchaseRequisitions);
        }

        public ActionResult ViewSparePartsRequest(int id)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            _oClientOperationSetting = new ClientOperationSetting();
            if (id > 0)
            {
                _oPurchaseRequisition = _oPurchaseRequisition.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseRequisition.PurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(_oPurchaseRequisition.PRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else
            {
                _oPurchaseRequisition.PrepareByName=((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            ViewBag.RequisitionTypes = EnumObject.jGets(typeof(EnumPurchaseRequisitionType));
            ViewBag.ClientOperationSetting = _oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsProcurementwithStyleNo, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.PriortyLevels = EnumObject.jGets(typeof(EnumPriortyLevel));
            return View(_oPurchaseRequisition);
        }
        [HttpPost]
        public JsonResult GetsData(PurchaseRequisition oPurchaseRequisition)
        {
            //_oPurchaseRequisition.ErrorMessage = sTemp;
            List<PurchaseRequisition> oPurchaseRequisitions = new List<PurchaseRequisition>();
            _oPurchaseRequisition.ErrorMessage = _oPurchaseRequisition.ErrorMessage == null ? "" : _oPurchaseRequisition.ErrorMessage;
           _sSQL= this.GetSQL(oPurchaseRequisition.ErrorMessage);
            oPurchaseRequisitions = PurchaseRequisition.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string GetSQL(string sTemp)
        {
            /*BOQ Date Set*/
            int nBOQRcvDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dBOQRcvStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dInquerRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            string sBOQNo = sTemp.Split('~')[3];
            string sBuyerIDs = sTemp.Split('~')[4];
            string sDeptIDs = sTemp.Split('~')[7];
            string sProductIDs = "";// sTemp.Split('~')[5];

            
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            List<AuthorizationRoleMapping> oTempAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PurchaseRequisition).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID);
            oTempAuthorizationRoleMappings=oAuthorizationRoleMappings.Where(x=>(x.OperationType==EnumRoleOperationType.All_Search)).ToList();

            string sReturn1 = "SELECT * FROM View_PurchaseRequisition";
            string sReturn = "";


            #region BOQ No

            if (sBOQNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "PRNo ='" + sBOQNo + "'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }

            #endregion

            #region Prouduct

            if (sProductIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID_Req IN (" + sProductIDs + ")";
            }

            #endregion


            #region PR Date
            if (nBOQRcvDate > 0)
            {
                if (nBOQRcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate = '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nBOQRcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate != '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nBOQRcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate > '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nBOQRcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate < '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nBOQRcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate>= '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "' AND PRDate < '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nBOQRcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PRDate< '" + dBOQRcvStartDate.ToString("dd MMM yyyy") + "' OR PRDate > '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

        

            #endregion

            #region Authorization
            if (oTempAuthorizationRoleMappings.Count <= 0 && Convert.ToInt32(((User)Session[SessionInfo.CurrentUser]).UserID)!=-9)//Not superuser and permisssion for All search
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DBUserID = " + ((User)Session[SessionInfo.CurrentUser]).UserID + " OR (PRID IN(SELECT HH.ObjectRefID FROM View_ApprovalHistory AS HH WHERE HH.ModuleID = " + (int)EnumModuleName.PurchaseRequisition + " AND HH.SendToPersonID = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ") AND  ApprovalSequence  = (SELECT top 1 AP.ApprovalSequence FROM View_ApprovalHeadPerson AS AP WHERE AP.ApprovalHeadID IN (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.PurchaseRequisition + " ) AND AP.UserID = " + ((User)Session[SessionInfo.CurrentUser]).UserID + " ) )";
            }
            #endregion


            #region Dept

            if (sDeptIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "DepartmentID IN (" + sDeptIDs + ")";
            }

            #endregion
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        [HttpPost]
        public JsonResult SetPrintingData(PurchaseRequisition oPurchaseRequisition)
        {
            List<PurchaseRequisition> oPurchaseRequisitions = new List<PurchaseRequisition>();
            _oPurchaseRequisition.ErrorMessage = _oPurchaseRequisition.ErrorMessage == null ? "" : _oPurchaseRequisition.ErrorMessage;
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oPurchaseRequisition);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Get(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            try
            {
                if (oPurchaseRequisition.PRID > 0)
                {
                    _oPurchaseRequisition = _oPurchaseRequisition.Get(oPurchaseRequisition.PRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
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

        #region Get Requisition
        [HttpPost]
        public JsonResult GetYetToAttachPRs(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisitions = new List<PurchaseRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseRequisition WHERE  ISNULL(ApproveBy,0)!=0 AND PRID NOt IN (SELECT PRID FROM NOARequisition)";
                if (oPurchaseRequisition.PRNo != null && oPurchaseRequisition.PRNo != "")
                {
                    sSQL += " AND PRNo LIKE '%" + oPurchaseRequisition.PRNo + "%'";
                }
                if (oPurchaseRequisition.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oPurchaseRequisition.BUID ;
                }
                _oPurchaseRequisitions = PurchaseRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
                _oPurchaseRequisitions.Add(_oPurchaseRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Approved(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            //oPurchaseRequisition.Name = oPurchaseRequisition.Name == null ? "" : oPurchaseRequisition.Name;
            //oPurchaseRequisition.JobType = oPurchaseRequisition.JobType == null ? 0 : oPurchaseRequisition.JobType;
            oPurchaseRequisition.Note = oPurchaseRequisition.Note == null ? "" : oPurchaseRequisition.Note;
            try
            {
                _oPurchaseRequisition = oPurchaseRequisition;
                _oPurchaseRequisition = _oPurchaseRequisition.Approved(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Finish(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            int id = 0;
            //oPurchaseRequisition.Name = oPurchaseRequisition.Name == null ? "" : oPurchaseRequisition.Name;
            //oPurchaseRequisition.JobType = oPurchaseRequisition.JobType == null ? 0 : oPurchaseRequisition.JobType;
            oPurchaseRequisition.Note = oPurchaseRequisition.Note == null ? "" : oPurchaseRequisition.Note;
            try
            {
                _oPurchaseRequisition = oPurchaseRequisition;
                id = _oPurchaseRequisition.PRID;
                _oPurchaseRequisition = _oPurchaseRequisition.Finish(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }

            if (string.IsNullOrEmpty(_oPurchaseRequisition.ErrorMessage))
            {
                _oPurchaseRequisition = _oPurchaseRequisition.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Cancel(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            int id = 0;
            //oPurchaseRequisition.Name = oPurchaseRequisition.Name == null ? "" : oPurchaseRequisition.Name;
            //oPurchaseRequisition.JobType = oPurchaseRequisition.JobType == null ? 0 : oPurchaseRequisition.JobType;
            oPurchaseRequisition.Note = oPurchaseRequisition.Note == null ? "" : oPurchaseRequisition.Note;
            try
            {
                _oPurchaseRequisition = oPurchaseRequisition;
                id = _oPurchaseRequisition.PRID;
                _oPurchaseRequisition = _oPurchaseRequisition.Cancel(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            if (string.IsNullOrEmpty(_oPurchaseRequisition.ErrorMessage))
            {
                _oPurchaseRequisition = _oPurchaseRequisition.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            //oPurchaseRequisition.Name = oPurchaseRequisition.Name == null ? "" : oPurchaseRequisition.Name;
            //oPurchaseRequisition.JobType = oPurchaseRequisition.JobType == null ? 0 : oPurchaseRequisition.JobType;
            oPurchaseRequisition.Note = oPurchaseRequisition.Note == null ? "" : oPurchaseRequisition.Note;
            try
            {
                _oPurchaseRequisition = oPurchaseRequisition;
                _oPurchaseRequisition = _oPurchaseRequisition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveWithPRDetail(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            oPurchaseRequisition.Note = oPurchaseRequisition.Note == null ? "" : oPurchaseRequisition.Note;
            try
            {
                _oPurchaseRequisition = oPurchaseRequisition;
                _oPurchaseRequisition = _oPurchaseRequisition.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseRequisition.PurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(_oPurchaseRequisition.PRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SavePRDetail(PurchaseRequisitionDetail oPRDetail)
        {
            _oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
            _oPurchaseRequisitionDetail.Note = _oPurchaseRequisitionDetail.Note == null ? "" : _oPurchaseRequisitionDetail.Note;
            try
            {
                _oPurchaseRequisitionDetail = oPRDetail;
                _oPurchaseRequisitionDetail = _oPurchaseRequisitionDetail.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                _oPurchaseRequisitionDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitionDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            _oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.PurchaseRequisition, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.PurchaseRequisition, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }           
            }
            catch (Exception ex)
            {
                _oProduct = new Product();
                _oProduct.ErrorMessage = ex.Message;
                _oProducts.Add(_oProduct);
            }
            var jSonResult = Json(_oProducts, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        [HttpPost]
        public JsonResult Delete(PurchaseRequisition oPurchaseRequisition)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPurchaseRequisition.Delete(oPurchaseRequisition.PRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult DeleteDetail(PurchaseRequisitionDetail oPurchaseRequisitionDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPurchaseRequisitionDetail.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SearchByPRNo(string sTempData, double ts)
        {
            _oPurchaseRequisitions = new List<PurchaseRequisition>();
            try
            {
                _oPurchaseRequisitions = PurchaseRequisition.Gets("SELECT * FROM View_PurchaseRequisition WHERE PRNo Like '%" + sTempData + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
                _oPurchaseRequisitions.Add(_oPurchaseRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Print
        public ActionResult PrintoSparePartsRequest(int id)
        {
            _oPurchaseRequisition = new PurchaseRequisition();
            _oClientOperationSetting = new ClientOperationSetting();
            _oPurchaseRequisition = _oPurchaseRequisition.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oPurchaseRequisition.PurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<ApprovalHead> oApprovalHead = new List<ApprovalHead>();
            string ssql = "select * from ApprovalHead where ModuleID = 29";
            oApprovalHead = ApprovalHead.Gets(ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseRequisition.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseRequisitionPreview, (int)Session[SessionInfo.currentUserID]);
            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.PurchaseRequisitionReportFormat, ((User)Session[SessionInfo.CurrentUser]).UserID);
            rptSparePartsRequest oReport = new rptSparePartsRequest();
            byte[] abytes = oReport.PrepareReport(_oPurchaseRequisition, oBusinessUnit, oCompany, oTempClientOperationSetting, oSignatureSetups, oApprovalHead);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintoSparePartsRequestList(string sID)
        {
            string Message="";
            try
            {
                string sSQL = "SELECT * FROM  View_PurchaseRequisition WHERE PRID IN ( " + sID + " )";
                _oPurchaseRequisitions = PurchaseRequisition.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception e)
            {
                Message = e.Message;
                throw;
            }
           
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oPurchaseRequisitions[0].BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompanys.First();
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oPurchaseRequisitions.Count == 0 || !string.IsNullOrEmpty(Message))
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("Sorry, No Data Found!!");
                return File(abytes, "application/pdf");
            }
            else
            {
                List<SelectedField> oSelectedFields = new List<SelectedField>();//PRNo,PRDateST,RequirementDateSt,RequisitionByName,ApproveByName,StatusSt,DeptName
                SelectedField oSelectedField = new SelectedField("PRNo", "SPR No", 30, SelectedField.FieldType.Data, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("PRDateST", "SPR Date", 45, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("RequirementDateSt", "Req. Date", 47, SelectedField.FieldType.Data, SelectedField.Alginment.CENTER); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("RequisitionByName", "Requisition By", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("ApprovedByName", "Approve By", 60, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("StatusSt", "Status", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                oSelectedField = new SelectedField("Note", "Remarks", 47, SelectedField.FieldType.Total, SelectedField.Alginment.LEFT); oSelectedFields.Add(oSelectedField);
                
                rptDynamicReport oReport = new rptDynamicReport(595, 842);
                oReport.SpanTotal = 0;//ColSpanForTotal
                byte[] abytes = oReport.PrepareReport(_oPurchaseRequisitions.Cast<object>().ToList(), oBusinessUnit, oCompany, "Spare Parts Request", oSelectedFields);
                return File(abytes, "application/pdf");
            }
        }
        public void PrintListInExcel()
        {
            #region Get Data
            List<PurchaseRequisition> oPurchaseRequisitions = new List<PurchaseRequisition>();
            Company oCompany = new Company();
            PurchaseRequisition oPurchaseRequisition = new PurchaseRequisition();
            oPurchaseRequisition = (PurchaseRequisition)Session[SessionInfo.ParamObj]; 
            string sSQL = GetSQL(oPurchaseRequisition.ErrorMessage);
            oPurchaseRequisitions = PurchaseRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #endregion

            #region Export To Excel
            #region Buying Commission Statement
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Material Purchase Requisition Report");
                    sheet.Name = "Material Purchase Requisition";
                    sheet.Column(2).Width = 6;   // SL NO                        
                    sheet.Column(3).Width = 25;  //Req. DAte                       
                    sheet.Column(4).Width = 25;  // Priority                       
                    sheet.Column(5).Width = 20;  // Requirement DAte                        
                    sheet.Column(6).Width = 20;  // Requisition By  
                    sheet.Column(7).Width = 20;  // Department
                    sheet.Column(8).Width = 15;  // Status
                    sheet.Column(9).Width = 15;  // Finish By
                    sheet.Column(10).Width = 15; // NOte
                    nEndCol = 10;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Material Purchase Requisition"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    int nCount = 0;

                    #region Column Header
                    nRowIndex = nRowIndex + 1;
                    nStartRow = nRowIndex;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Requisition Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Priority"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Requirement Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Requisition By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Department"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Status"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Finish By"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Note"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Data
                    foreach (PurchaseRequisition oItem in oPurchaseRequisitions)
                    {
                        nCount++;
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.PRDateST; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.PriortyLevelInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.RequirementDateSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.RequisitionByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.DepartmentName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.StatusSt; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FinishByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.Note; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Material_Purchase_Requisition.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            #endregion
        }

        #endregion

        [HttpPost]
        public JsonResult GetPurchaseRequsition(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                //string sSQL = "SELECT * FROM View_PurchaseRequisitionDetail WHERE PRID IN ( SELECT PRID FROM PurchaseRequisition  WHERE ISNULL(ApproveBy,0)!=0  AND DATEADD(DAY,30,RequirementDate) > '" + DateTime.Today.ToString("dd MMM yyyy") + "' )";
                string sSQL = "SELECT * FROM View_PurchaseRequisitionDetail WHERE PRID IN ( SELECT PRID FROM PurchaseRequisition  WHERE ISNULL(ApproveBy,0)!=0)";//remove date for requiremtn :PTL By salaman
                if (oPurchaseRequisition.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oPurchaseRequisition.BUID;
                }
                if (oPurchaseRequisition.PRNo != "" && oPurchaseRequisition.PRNo != null)
                {
                     sSQL += " AND ( PRNo Like '%" + oPurchaseRequisition.PRNo + "%' OR ProductName LIKE '%"+oPurchaseRequisition.PRNo+"%' )";
                }
                sSQL += "ORDER BY PRDate DESC";
              
                _oPurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                _oPurchaseRequisitionDetail.ErrorMessage = ex.Message;
                _oPurchaseRequisitionDetails.Add(_oPurchaseRequisitionDetail);
            }

            var jsonResult = Json(_oPurchaseRequisitionDetails, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetRequistionDetails(NOA oNOA)
        {
            _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            int nNOAID = Convert.ToInt32(oNOA.Note.Split('~')[0]);
            int nProductID = Convert.ToInt32(oNOA.Note.Split('~')[1]);
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseRequisitionDetail AS VPD WHERE VPD.ProductID = "+nProductID+"  Order BY PRID , ProductID";
                _oPurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                _oPurchaseRequisitionDetail.ErrorMessage = ex.Message;
                _oPurchaseRequisitionDetails.Add(_oPurchaseRequisitionDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitionDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPurchaseRequsitionFORPO(PurchaseRequisition oPurchaseRequisition)
        {
            _oPurchaseRequisitions = new List<PurchaseRequisition>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "Select * from View_PurchaseRequisition WHERE ISNULL(ApproveBy,0)!=0 AND PRID NOT IN (SELECT RefID FROM PurchaseOrder  WHERE RefType = "+(int)EnumPOReferenceType.Requistion+")";
                if (oPurchaseRequisition.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oPurchaseRequisition.BUID ;
                }
                if (oPurchaseRequisition.PRNo != "" && oPurchaseRequisition.PRNo != null)
                {
                    sSQL += "AND PRNo Like '%" + oPurchaseRequisition.PRNo + "%'";
                }
                _oPurchaseRequisitions = PurchaseRequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);


            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
                _oPurchaseRequisitions.Add(_oPurchaseRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveApprovalHistory(ApprovalHistory oApprovalHistory)
        {
            ApprovalHistory _oApprovalHistory = new ApprovalHistory();
            PurchaseRequisition _oPurchaseRequisition = new PurchaseRequisition();
            
            try
            {
                _oApprovalHistory = oApprovalHistory;
                _oApprovalHistory = _oApprovalHistory.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);

                _oPurchaseRequisition = _oPurchaseRequisition.Get(oApprovalHistory.ObjectRefID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisition);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region SpecificationHead
        [HttpPost]
        public JsonResult ProductSpecHeadFORPOByProduct(PRSpec oPRSpec)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            PRSpec _oPQSpec = new PRSpec();
            List<PRSpec> oPQSpecs = new List<PRSpec>();
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select * from View_PRSpec Where PRDetailID =" + oPRSpec.PRDetailID;
                oPQSpecs = PRSpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oPRSpec.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oSProductSpecHeads.Any())
                {
                    if (oPQSpecs.Any())
                    {
                        _oSProductSpecHeads.RemoveAll(x => oPQSpecs.Select(p => p.SpecHeadID).Contains(x.SpecHeadID));

                    }
                    if (_oSProductSpecHeads.Any())
                    {
                        foreach (var oitem in _oSProductSpecHeads)
                        {
                            _oPQSpec = new PRSpec();
                            _oPQSpec.SpecName = oitem.SpecName;
                            _oPQSpec.PRSpecID = 0;
                            _oPQSpec.SpecHeadID = oitem.SpecHeadID;
                            _oPQSpec.PRDescription = string.Empty;
                            _oPQSpec.PRDetailID = oPRSpec.PRDetailID;
                            oPQSpecs.Add(_oPQSpec);

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                _oProductSpecHead = new ProductSpecHead();
                _oProductSpecHead.ErrorMessage = ex.Message;
                _oSProductSpecHeads.Add(_oProductSpecHead);
            }

            var jsonResult = Json(oPQSpecs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult IUDPRSpec(PRSpec oPRSpec)
        {
            try
            {
                oPRSpec = oPRSpec.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oPRSpec = new PRSpec();
                oPRSpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPRSpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }
}
