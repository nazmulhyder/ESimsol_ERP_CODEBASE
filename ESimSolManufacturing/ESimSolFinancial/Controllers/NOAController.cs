using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using System.Linq;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using ICS.Core.Utility;
using ReportManagement;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class NOAController : Controller
    {
        #region Declaration
        NOA _oNOA = new NOA();
        NOAQuotation _oNOAQuotation = new NOAQuotation();
        List<NOA> _oNOAs = new List<NOA>();
        NOADetail _oNOADetail = new NOADetail();
        List<NOADetail> _oNOADetails = new List<NOADetail>();
        List<NOAQuotation> _oNOAQuotations = new List<NOAQuotation>();
        List<Product> _oProducts = new List<Product>();
        Product _oProduct = new Product();
        List<NOARequisition> _oNOARequisitions = new List<NOARequisition>();
        NOARequisition _oNOARequisition = new NOARequisition();
        List<SupplierRateProcess> _oSupplierRateProcesss = new List<SupplierRateProcess>();
        List<NOASpec> _oNOASpecs = new List<NOASpec>();
        #endregion

        #region Function
      
        #endregion

        #region New Task
        #region View Actions
        public ActionResult ViewNOAs(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.NOA).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);

            EnumRoleOperationType eRole_Signatory_Approve = EnumRoleOperationType.None;
            EnumRoleOperationType eRole_Add = EnumRoleOperationType.None;
            if (oARMs.Count > 0)
            {
                foreach (AuthorizationRoleMapping oARM in oARMs)
                {
                    if (oARM.OperationType == EnumRoleOperationType.Signatory_Approve) { eRole_Signatory_Approve = oARM.OperationType; }
                    if (oARM.OperationType == EnumRoleOperationType.Add) { eRole_Add = oARM.OperationType; }
                }
            }
          
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();
            //oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oNOAs = new List<NOA>();
          
            if (eRole_Signatory_Approve == EnumRoleOperationType.Signatory_Approve)
            {
                _oNOAs = NOA.Gets("SELECT * FROM View_NOA WHERE isnull(ApproveBy,0) = 0 and NOAID in (Select NOAID from NOASignatory where  isnull(ApproveBy,0)=0 and RequestTo=" + ((User)Session[SessionInfo.CurrentUser]).UserID + " and NOASignatoryID in (Select NOASignatoryID from ( Select *,ROW_NUMBER() OVER (Partition by NOAID Order by NOASignatory.SLNO ASC) AS RowNumber from NOASignatory where isnull(ApproveBy,0)=0 ) as dd where dd.RowNumber in (1,2) and dd.ApprovalHeadID in ( Select ApprovalHeadID from ( Select *,ROW_NUMBER() OVER (Partition by NOAID Order by NOASignatory.SLNO ASC) AS RowNumber from NOASignatory where isnull(IsApprove,0)=0  ) as ff where ff.RowNumber=1)))", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            else if (eRole_Add == EnumRoleOperationType.Add)
            {
                if (buid > 0)//if apply style configuration business unit
                {
                    _oNOAs = NOA.Gets("SELECT * FROM View_NOA WHERE ApproveBy = 0 AND BUID =" + buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oNOAs = NOA.Gets("SELECT * FROM View_NOA WHERE isnull(ApproveBy,0) = 0", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            else
            {
                _oNOAs = NOA.Gets("SELECT * FROM View_NOA WHERE ApproveBy = 0", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            ViewBag.NOAOperationType = oClientOperationSettingNOA;
            ViewBag.Employees = Employee.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(_oNOAs);
        }
        public ActionResult ViewNOAReviseHistory(int id)
        {
            _oNOAs = NOA.Gets("SELECT * FROM View_NOALog WHERE NOAID=" + id, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();
            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            ViewBag.NOAOperationType = oClientOperationSettingNOA;

            ViewBag.Employees = Employee.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oNOAs);
        }
        public ActionResult ViewNOAEntry(int id, double ts)
        {
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();
            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            _oNOASpecs = new List<NOASpec>();
            string sNOADetailIDs = "";
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            ViewBag.NOAOperationType = oClientOperationSettingNOA;
            _oNOA = new NOA();
            if (id > 0)
            {
                _oNOA = _oNOA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOA.NOADetailLst = NOADetail.Gets( _oNOA.NOAID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOA.NOARequisitionList = NOARequisition.Gets(_oNOA.NOAID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oNOA.NOADetailLst.Count > 0)
                {
                    sNOADetailIDs = string.Join(",", _oNOA.NOADetailLst.Select(x => x.NOADetailID.ToString()));
                }
                if (!string.IsNullOrEmpty(sNOADetailIDs))
                {
                    string sSQL = "SELECT * FROM View_NOASpec WHERE NOADetailID IN (" + sNOADetailIDs + ")";
                    _oNOASpecs = NOASpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (NOADetail oItem in _oNOA.NOADetailLst)
                {
                    oItem.Specifications = string.Join(",", _oNOASpecs.Where(x => x.NOADetailID == oItem.NOADetailID).ToList().Select(x => x.SpecName + " : " + x.NOADescription));
                }
            }
            else
            {
                _oNOA.PrepareByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            return View(_oNOA);
        }
        public ActionResult ViewNOARevise(int id)
        {
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();
            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            _oNOASpecs = new List<NOASpec>();
            string sNOADetailIDs = "";
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            ViewBag.NOAOperationType = oClientOperationSettingNOA;
            _oNOA = new NOA();
            if (id > 0)
            {
                _oNOA = _oNOA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOA.NOADetailLst = NOADetail.Gets(_oNOA.NOAID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOA.NOARequisitionList = NOARequisition.Gets(_oNOA.NOAID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oNOA.NOADetailLst.Count > 0)
                {
                    sNOADetailIDs = string.Join(",", _oNOA.NOADetailLst.Select(x => x.NOADetailID.ToString()));
                }
                if (!string.IsNullOrEmpty(sNOADetailIDs))
                {
                    string sSQL = "SELECT * FROM View_NOASpec WHERE NOADetailID IN (" + sNOADetailIDs + ")";
                    _oNOASpecs = NOASpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (NOADetail oItem in _oNOA.NOADetailLst)
                {
                    oItem.Specifications = string.Join(",", _oNOASpecs.Where(x => x.NOADetailID == oItem.NOADetailID).ToList().Select(x => x.SpecName + " : " + x.NOADescription));
                }
            }
            else
            {
                _oNOA.PrepareByName = ((User)Session[SessionInfo.CurrentUser]).UserName;
            }
            return View(_oNOA);
        }

        [HttpPost]
        public JsonResult SaveRevise(NOA oNOA)
        {
            _oNOA = new NOA();
            oNOA.Note = oNOA.Note == null ? "" : oNOA.Note;
            try
            {
                _oNOA = oNOA;
                _oNOA = _oNOA.AcceptRevise(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewNOA_Approve(int id, double ts)// Approved
        {
           // this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.NOA).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]););
            List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
            NOASignatory oNOASignatory = new NOASignatory();
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();
            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            oNOASignatorys = NOASignatory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            ViewBag.NOAOperationType = oClientOperationSettingNOA;
            ViewBag.NOASignatorys = oNOASignatorys;
            ViewBag.NOASignatory = oNOASignatorys.Where(x => x.RequestTo == (int)Session[SessionInfo.currentUserID]).FirstOrDefault();
            _oNOA = new NOA();

            //int maxValue = oNOASignatorys.Max();
            oNOASignatorys = oNOASignatorys.Where(p => p.ApproveBy != 0).ToList();
            int nNOASignatoryID = 0;
            if (oNOASignatorys.Count > 0)
            {
                nNOASignatoryID = oNOASignatorys.Where(p => p.ApproveBy != 0).Last().NOASignatoryID;
            }
            if (id > 0)
            {
                _oNOA = _oNOA.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oNOA.ApproveBy != 0)
                {
                    _oNOA.SupplierRateProcess = SupplierRateProcess.Gets(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oNOA.SupplierRateProcess = SupplierRateProcess.GetsBy(id, nNOASignatoryID,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }

            return View(_oNOA);
        }
        #endregion

        #region HTTP Posts
        [HttpPost]
        public JsonResult GetNOAQuotations(NOA oNOA)
        {
            _oNOAQuotations = new List<NOAQuotation>();
            try
            {
                string sSQL = "Select * from View_NOAQuotation WHERE NOAID = "+oNOA.NOAID+" Order By NOAID";
                _oNOAQuotations = NOAQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOAQuotation = new NOAQuotation();
                _oNOAQuotation.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(_oNOAQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsRateBySignatory(NOASignatory oNOASignatory)
        {
            List<SupplierRateProcess> oSupplierRateProcess = new List<SupplierRateProcess>();
            try
            {
                oSupplierRateProcess = SupplierRateProcess.GetsBy(oNOASignatory.NOAID, oNOASignatory.NOASignatoryID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                //SupplierRateProcess oSupplierRateP = new SupplierRateProcess();
                //oSupplierRateProcess.ErrorMessage = ex.Message;
                oSupplierRateProcess= new  List<SupplierRateProcess>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSupplierRateProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNOAQuotationsMapping(NOADetail oNOADetail)
        {
            _oNOAQuotations = new List<NOAQuotation>();
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            List<PQSpec> oPQSpecs = new List<PQSpec>();
            string sPQDetailIDs = "";
            try
            {
                string sSQL = "Select * from View_NOAQuotation WHERE NOADetailID = " + oNOADetail.NOADetailID + " Order By NOADetailID";
                _oNOAQuotations = NOAQuotation.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oPurchaseQuotationDetails = PurchaseQuotationDetail.Gets(" SELECT * FROM View_PurchaseQuotationDetail WHERE ProductID = " + oNOADetail.ProductID + " AND ExpiredDate >= '" + DateTime.Today.ToString("dd MMM yyyy") + "' AND PurchaseQuotationID IN(SELECT PurchaseQuotationID FROM PurchaseQuotation WHERE ISNULL(ApprovedBy, 0)<>0)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oNOAQuotations.Count > 0)
                {
                    sPQDetailIDs = string.Join(",", _oNOAQuotations.Select(x => x.PQDetailID.ToString()));
                }
                if(oPurchaseQuotationDetails.Count>0)
                {
                    if (sPQDetailIDs != "") { sPQDetailIDs += ","; }
                    sPQDetailIDs += string.Join(",", oPurchaseQuotationDetails.Select(x => x.PurchaseQuotationDetailID.ToString()));
                }
                if (!string.IsNullOrEmpty(sPQDetailIDs))
                {
                    sSQL = "SELECT * FROM View_PQSpec WHERE PQDetailID IN (" + sPQDetailIDs + ")";
                    oPQSpecs = PQSpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

                foreach(NOAQuotation oItem in _oNOAQuotations)
                {
                    oItem.Specifications = string.Join(",", oPQSpecs.Where(x => x.PQDetailID == oItem.PQDetailID).ToList().Select(x => x.SpecName + " : " + x.PQDescription));
                }

                foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                {
                    if (_oNOAQuotations.Where(x => x.PQDetailID == oItem.PurchaseQuotationDetailID).ToList().Count <= 0)
                    {
                        _oNOAQuotation = new NOAQuotation();
                        _oNOAQuotation.NOADetailID = oNOADetail.NOADetailID;
                        _oNOAQuotation.PQDetailID = oItem.PurchaseQuotationDetailID;
                        _oNOAQuotation.SupplierName = oItem.SupplierName;
                        _oNOAQuotation.PQNo = oItem.PurchaseQuotationNo;
                        _oNOAQuotation.UnitPrice = oItem.UnitPrice;
                        _oNOAQuotation.Specifications =  string.Join(",",oPQSpecs.Where(x=>x.PQDetailID==oItem.PurchaseQuotationDetailID).ToList().Select(x=>x.SpecName +" : " +x.PQDescription));
                        _oNOAQuotation.bIsExist = false;
                        _oNOAQuotations.Add(_oNOAQuotation);
                    }
                }


            }
            catch (Exception ex)
            {
                _oNOAQuotation = new NOAQuotation();
                _oNOAQuotation.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(_oNOAQuotation);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetNOARequisitions(NOA oNOA)
        {
            _oNOARequisitions = new List<NOARequisition>();
            try
            {
                string sSQL = "Select * from View_NOARequisition WHERE NOAID = " + oNOA.NOAID + " Order By NOAID";
                _oNOARequisitions = NOARequisition.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOARequisition = new NOARequisition();
                _oNOARequisition.ErrorMessage = ex.Message;
                _oNOARequisitions.Add(_oNOARequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOARequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //ApproveNOA

        [HttpPost]
        public JsonResult Approve(NOA oNOA)
        {
            _oNOA = new NOA();
            try
            {
                _oNOA = oNOA.Approve(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        [HttpPost]
        public JsonResult RequestRevise(NOA oNOA)
        {
            _oNOA = new NOA();
            try
            {
                _oNOA = oNOA;

                _oNOA = _oNOA.RequestNOARevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchByNOANo(NOA oNOA)
        {
            _oNOAs = new List<NOA>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            try
            {
                string sSQL = "SELECT * FROM View_NOA";
                string sTemp = "";
                if (oNOA.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " BUID =" + oNOA.BUID;
                }

                if (!String.IsNullOrEmpty(oNOA.NOANo))
                {
                    Global.TagSQL(ref sTemp);
                    sTemp += " NOANo Like '%" + oNOA.NOANo + "%'";
                }
                sSQL = sSQL + sTemp;
                _oNOAs = NOA.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
                _oNOAs.Add(_oNOA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNOAFORPO(NOA oNOA)
        {
            _oNOAs = new List<NOA>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_NOA WHERE  ISNULL(ApproveBy,0)!=0";
                if (oNOA.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oNOA.BUID;
                }
                if (oNOA.NOANo !=null && oNOA.NOANo != "")
                {
                    sSQL += " AND NOANo Like '%" + oNOA.NOANo + "%'";
                }
                sSQL += " Order by NOANo DESC";
                _oNOAs = NOA.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
                _oNOAs.Add(_oNOA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult SearchByNOANo(string sTempData, double ts)
        {
            _oNOAs = new List<NOA>();

            try
            {
                _oNOAs = NOA.Gets("SELECT * FROM View_NOA WHERE NOANo Like '%" + sTempData + "%'", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
                _oNOAs.Add(_oNOA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Save(NOA oNOA)
        {
            _oNOA = new NOA();
            _oNOASpecs = new List<NOASpec>();
            string sNOADetailIDs = "";
            try
            {
                _oNOA = oNOA;
                _oNOA.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                _oNOA = _oNOA.Save(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oNOA.NOADetailLst.Count > 0)
                {
                    sNOADetailIDs = string.Join(",", _oNOA.NOADetailLst.Select(x => x.NOADetailID.ToString()));
                }
                if (!string.IsNullOrEmpty(sNOADetailIDs))
                {
                    string sSQL = "SELECT * FROM View_NOASpec WHERE NOADetailID IN (" + sNOADetailIDs + ")";
                    _oNOASpecs = NOASpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                foreach (NOADetail oItem in _oNOA.NOADetailLst)
                {
                    oItem.Specifications = string.Join(",", _oNOASpecs.Where(x => x.NOADetailID == oItem.NOADetailID).ToList().Select(x => x.SpecName + " : " + x.NOADescription));
                }
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Copy(NOA oNOA)
        {
            try
            {
                _oNOA = oNOA;
                _oNOA.ApproveBy = 0;
                _oNOA.NOADetailLst = NOADetail.Gets(oNOA.NOAID, (int)Session[SessionInfo.currentUserID]);
                foreach (NOADetail oitem in _oNOA.NOADetailLst)
                {
                    oitem.NOADetailID = 0;
                    oitem.NOAID = 0;
                }

                //Another List Need to confirm
                _oNOA.NOARequisitionList = NOARequisition.Gets(oNOA.NOAID, (int)Session[SessionInfo.currentUserID]);
                foreach (NOARequisition oitem in _oNOA.NOARequisitionList)
                {
                    oitem.NOARequisitionID = 0;
                    oitem.NOAID = 0;
                }

                _oNOA.NOAID = 0;
                _oNOA = _oNOA.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            NOA oNOA = new NOA();
            try
            {
                sFeedBackMessage = oNOA.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult UndoApprove(NOA oNOA)
        {
            _oNOA = new NOA();
            try
            {
                _oNOA = oNOA.UndoApprove(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOA);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult DeleteDetail(int nNOAID, int nNOADetailID)
        {
            _oSupplierRateProcesss = new List<SupplierRateProcess>();
            try
            {
                _oSupplierRateProcesss = NOADetail.Delete(nNOAID, nNOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                SupplierRateProcess oSupplierRateProcess = new SupplierRateProcess();
                oSupplierRateProcess.ErrorMessage = ex.Message;
                _oSupplierRateProcesss.Add(oSupplierRateProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSupplierRateProcesss);
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
                    _oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.NOA, EnumProductUsages.Regular, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    _oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.NOA, EnumProductUsages.Regular, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetProductsAdv(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();

            try
            {
                string sSql = string.Empty;

                sSql = "SELECT * FROM View_Product AS HH WHERE HH.Activity = 1 AND  ProductID IN (Select Distinct(ProductID) from NOADetail)";

                if (!string.IsNullOrEmpty(oProduct.ProductName))
                {
                    sSql = sSql + "and HH.ProductName LIKE '%" + oProduct.ProductName + "%'";
                }

                sSql = sSql + " ORDER BY HH.ProductName";
                oProducts = Product.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetRequisitions(PurchaseRequisition oPurchaseRequisition)
        {
            
            List<PurchaseRequisition> _oPurchaseRequisitions = new List<PurchaseRequisition>();
            try
            {
                string sql = "SELECT top(100)* FROM View_PurchaseRequisition where PRID>0";
                if (!string.IsNullOrEmpty(oPurchaseRequisition.PRNo))
                {
                    sql += " and PRNO LIKE'%" + oPurchaseRequisition.PRNo + "%'";
                }
                sql += "  order by PRDate DESC";
              
                _oPurchaseRequisitions = PurchaseRequisition.Gets(sql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oPurchaseRequisitions = _oPurchaseRequisitions.OrderByDescending(X => X.PRDate).ToList();
            }
            catch (Exception ex)
            {
                PurchaseRequisition _oPurchaseRequisition = new PurchaseRequisition();
                _oPurchaseRequisition.ErrorMessage = ex.Message;
                _oPurchaseRequisitions.Add(_oPurchaseRequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetPRDetails(PurchaseRequisitionDetail oPurchaseRequisitionDetail)
        {

            List<PurchaseRequisitionDetail> oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();          
            List<PurchaseRequisitionDetail> _oPurchaseRequisitionDetails = new List<PurchaseRequisitionDetail>();
            List<PRSpec> oPRSpecs = new List<PRSpec>();
            string sPRDetailIDs = "";
            try
            {
                string sSQL = "SELECT * FROM View_PurchaseRequisitionDetail WHERE PRID IN (" + oPurchaseRequisitionDetail.Remarks + ") Order by PRID";
                _oPurchaseRequisitionDetails = PurchaseRequisitionDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                sPRDetailIDs = string.Join(",", _oPurchaseRequisitionDetails.Select(x => x.PRDetailID.ToString()));
                if (!string.IsNullOrEmpty(sPRDetailIDs))
                {
                    oPRSpecs = PRSpec.Gets("SELECT * FROM View_PRSpec WHERE PRDetailID In (" + sPRDetailIDs + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (_oPurchaseRequisitionDetails.Any())
                {
                    List<PurchaseRequisitionDetail> PurchaseRequisitionDetailsWithSpec = _oPurchaseRequisitionDetails.Where(x => x.IsSpecExist == true).ToList();
                    List<PurchaseRequisitionDetail> PurchaseRequisitionDetailsWithOuthSpec = _oPurchaseRequisitionDetails.Where(x => x.IsSpecExist == false).ToList();
                    if (PurchaseRequisitionDetailsWithOuthSpec.Any())
                    {
                        _oPurchaseRequisitionDetails = PurchaseRequisitionDetailsWithOuthSpec.GroupBy(x => new { x.ProductID }, (key, grp) =>
                        new PurchaseRequisitionDetail
                        {
                            MUnitID = grp.First().MUnitID,
                            ProductID = key.ProductID,
                            Qty = grp.Sum(p => p.Qty),
                            ProductCode = grp.First().ProductCode,
                            ProductName = grp.First().ProductName,
                            UnitName = grp.First().UnitName,
                            PRDetailID = grp.First().PRDetailID
                        }).ToList();
                        foreach (PurchaseRequisitionDetail oItem in PurchaseRequisitionDetailsWithSpec)
                        {
                            oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                            oPurchaseRequisitionDetail.PRDetailID = oItem.PRDetailID;
                            oPurchaseRequisitionDetail.PRID = oItem.PRID;
                            oPurchaseRequisitionDetail.ProductCode = oItem.ProductCode;
                            oPurchaseRequisitionDetail.ProductID = oItem.ProductID;
                            oPurchaseRequisitionDetail.ProductName = oItem.ProductName;
                            oPurchaseRequisitionDetail.Qty = oItem.Qty;
                            oPurchaseRequisitionDetail.MUnitID = oItem.MUnitID;
                            oPurchaseRequisitionDetail.UnitName = oItem.UnitName;
                            oPurchaseRequisitionDetail.Specifications = string.Join(",", oPRSpecs.Where(x => x.PRDetailID == oItem.PRDetailID).ToList().Select(x => x.SpecName + " : " + x.PRDescription));
                            _oPurchaseRequisitionDetails.Add(oPurchaseRequisitionDetail);
                        }
                    }
                    
                   
                    _oPurchaseRequisitionDetails.OrderBy(x => x.ProductID);
                }
            }
            catch (Exception ex)
            {
                oPurchaseRequisitionDetail = new PurchaseRequisitionDetail();
                oPurchaseRequisitionDetail.ErrorMessage = ex.Message;
                _oPurchaseRequisitionDetails.Add(oPurchaseRequisitionDetail);
            }



            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPurchaseRequisitionDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region HttpGet For Search
        [HttpPost]
        public JsonResult NOASearch(NOA oNOA)
        {
           _oNOAs = new List<NOA>();
            try
            {
                string sSQL = GetSQL(oNOA.Note);
                _oNOAs = NOA.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oNOA = new NOA();
                _oNOA.ErrorMessage = ex.Message;
                _oNOAs.Add(_oNOA);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            /*NOA Date Set*/
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            int nNOARcvDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dNOARcvStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dInquerRcvEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            int nAroveDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dApproveStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dApproveEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);
            string sNOANo = sTemp.Split('~')[6];
            int nBUID = Convert.ToInt32(sTemp.Split('~')[7]);
            string sDeptIDs = sTemp.Split('~')[8];
            string sProductIDs = sTemp.Split('~')[9];
            string sMPRNo = sTemp.Split('~')[10];
            string sQuotationNo = sTemp.Split('~')[11];
            string sSupplierIDs = sTemp.Split('~')[12];

            string sReturn1 = "SELECT * FROM View_NOA";
            string sReturn = "";

            #region NOA No
            if (sNOANo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOANo ='" + sNOANo + "'";
            }
            #endregion

            #region Issue Date Wise
          
            if (nNOARcvDate > 0)
            {
                if (nNOARcvDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate = '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nNOARcvDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate != '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nNOARcvDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate > '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nNOARcvDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate < '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nNOARcvDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate>= '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "' AND NOADate < '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nNOARcvDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NOADate< '" + dNOARcvStartDate.ToString("dd MMM yyyy") + "' OR NOADate > '" + dInquerRcvEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region Approve Date Wise
            if (nAroveDate > 0)
            {
                if (nAroveDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate = '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nAroveDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate != '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nAroveDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate > '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nAroveDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate < '" + dApproveStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nAroveDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate>= '" + dApproveStartDate.ToString("dd MMM yyyy") + "' AND ApproveDate < '" + dApproveEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nAroveDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ApproveDate< '" + dApproveStartDate.ToString("dd MMM yyyy") + "' OR ApproveDate > '" + dApproveEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region BUID
            if (nBUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region DeptID
            if (!string.IsNullOrEmpty(sDeptIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOAID IN (SELECT NOAID FROM NOARequisition  WHERE PRID IN (SELECt PRID FROM PurchaseRequisition WHERE DepartmentID In ("+sDeptIDs+") ))";
            }
            #endregion
            #region Product ID
            if (!string.IsNullOrEmpty(sProductIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOAID IN (Select NOAID from NOADetail where ProductID in ("+ sProductIDs + "))";
            }
            #endregion

            #region MPR NO
            if (!string.IsNullOrEmpty(sMPRNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOAID IN (SELECT NOAID FROM View_NOARequisition WHERE PRNo LIKE '%" + sMPRNo + "%')";
            }
            #endregion

            #region Quotation NO
            if (!string.IsNullOrEmpty(sQuotationNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOAID IN (SELECT NOAID FROM NOADetail WHERE PQDetailID IN (SELECT PurchaseQuotationDetailID FROm PurchaseQuotationDetail WHERE PurchaseQuotationID IN (SELECT PurchaseQuotationID FROM PurchaseQuotation WHERE PurchaseQuotationNo LIKE '%" + sQuotationNo + "%')))";
            }
            #endregion

            #region Supplier
            if (!string.IsNullOrEmpty(sSupplierIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " NOAID IN (SELECT NOAID FROM View_NOADetail  WHERE SupplierID IN (" + sSupplierIDs + ") )";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Printing

        public void PrintListInExcel(string searchStr)
        {
            #region Get Data
            _oNOAs = new List<NOA>();
            string sSQL = GetSQL(searchStr);
            _oNOAs = NOA.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
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
                var sheet = excelPackage.Workbook.Worksheets.Add("CS Collection");
                sheet.Name = "Material Purchase Requisition";
                sheet.Column(2).Width = 6;   // SL NO                        
                sheet.Column(3).Width = 25;  //CS No                       
                sheet.Column(4).Width = 25;  // Date                 
                sheet.Column(5).Width = 20;  // Prepared By  
                sheet.Column(6).Width = 20;  //Approved By
                sheet.Column(7).Width = 20;  // Remarks
            
                nEndCol = 7;

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
                cell.Value = "CS Collection"; cell.Style.Font.Bold = true;
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

                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "CS NO"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Date"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Prepared By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Approved By"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

        
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Data
                foreach (NOA oItem in _oNOAs)
                {
                    nCount++;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.Numberformat.Format = "###0;(###0)";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.NOANo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.NOADateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.PrepareByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.ApprovedByName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.Note; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
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
                Response.AddHeader("content-disposition", "attachment; filename=CS_Collection.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
            #endregion
        }
        public ActionResult PrintNOAs(string sParam)
        {
            _oNOA = new NOA();
            Company oCompany = new Company();
            string sSql = "SELECT * FROM View_NOA WHERE NOAID IN (" + sParam + ")";
            _oNOAs = NOA.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oNOAs.Count > 0)
            {
                rptNOAs oReport = new rptNOAs();
                byte[] abytes = oReport.PrepareReport(_oNOAs, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }
        public ActionResult PrintNOA(int id)
        {
            _oNOA = new NOA();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oNOA = _oNOA.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.SupplierRateProcess = SupplierRateProcess.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOADetailLst = NOADetail.Gets(_oNOA.NOAID, (int)Session[SessionInfo.currentUserID]);
            oBusinessUnit = oBusinessUnit.Get(_oNOA.BUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptNOA oReport = new rptNOA();
            byte[] abytes = oReport.PrepareReport(_oNOA, oBusinessUnit, oCompany);
            return File(abytes, "application/pdf");
        }
        public ActionResult PrintNOAWithFormat(int id, bool bWithTechHead, bool bIsNormalFormat)
        {
            List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
            _oNOA = new NOA();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oNOA = _oNOA.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOADetailLst = NOADetail.Gets(_oNOA.NOAID, (int)Session[SessionInfo.currentUserID]);
            oNOASignatorys = NOASignatory.Gets(_oNOA.NOAID, (int)Session[SessionInfo.currentUserID]);
            if (_oNOA.ApproveBy==0)//Last change by : Md. Mahabub 13 @DEC 18 for PTL
            {
                oNOASignatorys = oNOASignatorys.Where(p => p.ApproveBy != 0).ToList();
                int nNOASignatoryID = 0;
                if (oNOASignatorys.Count > 0)
                {
                    nNOASignatoryID = oNOASignatorys.Where(p => p.ApproveBy != 0).Last().NOASignatoryID;
                }
                if (nNOASignatoryID > 0)
                {
                    _oNOA.NOASignatoryComments = NOASignatoryComment.Gets("SELECT * FROM View_NOASignatoryComment WHERE NOASignatoryID = "+nNOASignatoryID, (int)Session[SessionInfo.currentUserID]);
                }
                _oNOA.SupplierRateProcess = SupplierRateProcess.GetsBy(id, nNOASignatoryID, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oNOA.SupplierRateProcess = SupplierRateProcess.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            
            string sSql = "select * From View_PurchaseQuotationDetail where PurchaseQuotationID IN(select PQID from View_NOAQuotation where NOAID=" + id + ")";
            _oNOA.PurchaseQuotationDetailList = PurchaseQuotationDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOAQuotationList = NOAQuotation.Gets("SELECT * FROM View_NOAQuotation WHERE NOAID=" + id, (int)Session[SessionInfo.currentUserID]);
            if (_oNOA.PurchaseQuotationDetailList.Any())
            {
                _oNOA.PQSpecs = PQSpec.Gets("SELECT * FROM View_PQSpec Where PQDetailID IN ( " + (string.Join(",", _oNOA.PurchaseQuotationDetailList.Select(x => x.PurchaseQuotationDetailID))) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            if (_oNOA.NOADetailLst.Any())
            {
                _oNOA.NOASpecs = NOASpec.Gets("SELECT * FROM View_NOASpec Where NOADetailID IN ( " + (string.Join(",", _oNOA.NOADetailLst.Select(x => x.NOADetailID))) + ")", (int)Session[SessionInfo.currentUserID]);
            }

            oBusinessUnit = oBusinessUnit.Get(_oNOA.BUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();

            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseRequisitionPreview, (int)Session[SessionInfo.currentUserID]);

            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.NOAReportFormat, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (Convert.ToInt32(oTempClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default)
            {
                rptNOA oReport = new rptNOA();
                byte[] abytes = oReport.PrepareReport(_oNOA, oBusinessUnit, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptNOAFormat1 oReport = new rptNOAFormat1();
                byte[] abytes = oReport.PrepareReport(_oNOA, oBusinessUnit, oCompany, oTempClientOperationSetting, oSignatureSetups, oClientOperationSettingNOA, bWithTechHead, oNOASignatorys, bIsNormalFormat);
                return File(abytes, "application/pdf");
            }

        }
        public ActionResult PrintNOAWithFormatLog(int id, bool bWithTechHead, bool bIsNormalFormat)
        {
            List<NOASignatory> oNOASignatorys = new List<NOASignatory>();
            _oNOA = new NOA();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oNOA = _oNOA.GetByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.SupplierRateProcess = SupplierRateProcess.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOADetailLst = NOADetail.GetsByLog(_oNOA.NOALogID, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOADetailLst.ForEach(x =>
            {
                x.NOADetailID = x.NOADetailLogID;
            });
            oNOASignatorys = NOASignatory.GetsByLog(_oNOA.NOALogID, (int)Session[SessionInfo.currentUserID]);
            string sSql = "select * From View_PurchaseQuotationDetail where PurchaseQuotationID IN(select PQID from View_NOAQuotationLog where NOALogID=" + id + ")";
            _oNOA.PurchaseQuotationDetailList = PurchaseQuotationDetail.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOAQuotationList = NOAQuotation.GetsByLog("SELECT * FROM View_NOAQuotationLog WHERE NOALogID=" + id, (int)Session[SessionInfo.currentUserID]);
            _oNOA.NOAQuotationList.ForEach(x =>
            {
                x.NOADetailID = x.NOADetailLogID;
            });
            if (_oNOA.PurchaseQuotationDetailList.Any())
            {
                _oNOA.PQSpecs = PQSpec.Gets("SELECT * FROM View_PQSpec Where PQDetailID IN ( " + (string.Join(",", _oNOA.PurchaseQuotationDetailList.Select(x => x.PurchaseQuotationDetailID))) + ")", (int)Session[SessionInfo.currentUserID]);
            }
            if (_oNOA.NOADetailLst.Any())
            {
                _oNOA.NOASpecs = NOASpec.GetsByLog("SELECT * FROM View_NOASpecLog Where NOADetailLogID IN ( " + (string.Join(",", _oNOA.NOADetailLst.Select(x => x.NOADetailLogID))) + ")", (int)Session[SessionInfo.currentUserID]);
                _oNOA.NOASpecs.ForEach(x =>
                {
                    x.NOADetailID = x.NOADetailLogID;
                });
            }

            oBusinessUnit = oBusinessUnit.Get(_oNOA.BUID, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            ClientOperationSetting oClientOperationSettingNOA = new ClientOperationSetting();

            oClientOperationSettingNOA = oClientOperationSettingNOA.GetByOperationType((int)EnumOperationType.NOAOperationType, (int)Session[SessionInfo.currentUserID]);
            if (!string.IsNullOrEmpty(oClientOperationSettingNOA.Value))
            {
                oClientOperationSettingNOA.Value = oClientOperationSettingNOA.Value;
            }
            else
            {
                oClientOperationSettingNOA.Value = "NOA";
            }
            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.PurchaseRequisitionPreview, (int)Session[SessionInfo.currentUserID]);

            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.NOAReportFormat, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (Convert.ToInt32(oTempClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default)
            {
                rptNOA oReport = new rptNOA();
                byte[] abytes = oReport.PrepareReport(_oNOA, oBusinessUnit, oCompany);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptNOAFormat1 oReport = new rptNOAFormat1();
                byte[] abytes = oReport.PrepareReport(_oNOA, oBusinessUnit, oCompany, oTempClientOperationSetting, oSignatureSetups, oClientOperationSettingNOA, bWithTechHead, oNOASignatorys,bIsNormalFormat);
                return File(abytes, "application/pdf");
            }

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
        [HttpPost]
        public JsonResult GetPRDetail(PurchaseRequisition  oPR)
        {
            _oNOADetails = new List<NOADetail>();

            List<PurchaseRequisitionDetail> oPRDetails = new List<PurchaseRequisitionDetail>();
            try
            {
                oPRDetails = PurchaseRequisitionDetail.Gets(oPR.PRID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (PurchaseRequisitionDetail oItem in oPRDetails)
                {
                    _oNOADetail = new NOADetail();
                    _oNOADetail.ProductID = oItem.ProductID;
                   
                    _oNOADetail.ProductName = oItem.ProductName;
                    _oNOADetail.ProductCode = oItem.ProductCode;
                    _oNOADetail.ProductSpec = oItem.ProductSpec;
                  
                    _oNOADetail.MUnitID = oItem.MUnitID;
                    _oNOADetail.MUnitName = oItem.UnitName;

                    _oNOADetail.PurchaseQty = oItem.Qty;
                    _oNOADetail.Note = "lowest price";
                 
                  
                    _oNOADetails.Add(_oNOADetail);
                }
            }
            catch (Exception ex)
            {
                NOADetail oNOADetail = new NOADetail();
                oNOADetail.ErrorMessage = ex.Message;
                _oNOADetails.Add(oNOADetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOADetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetQuotation(NOADetail oNOADetail)
        {
            List<NOAQuotation> _oNOAQuotations = new List<NOAQuotation>();
            NOAQuotation oNOAQuotation = new NOAQuotation();
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            try
            {
                oNOADetail = oNOADetail.Get(oNOADetail.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOAQuotations = NOAQuotation.Gets(oNOADetail.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                 
                if (_oNOAQuotations.Count <= 0)
                {

                    oPurchaseQuotationDetails = PurchaseQuotationDetail.GetsForNOA(oNOADetail.ProductID, oNOADetail.MUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                    {
                        oNOAQuotation = new NOAQuotation();
                        oNOAQuotation.SupplierID = oItem.SupplierID;
                        oNOAQuotation.SupplierName = oItem.SupplierName;
                        oNOAQuotation.UnitPrice = oItem.UnitPrice;
                        oNOAQuotation.PQID = oItem.PurchaseQuotationDetailID;
                        _oNOAQuotations.Add(oNOAQuotation);
                    }
                }
              
            }
            catch (Exception ex)
            {
                NOAQuotation oTempNOADetail = new NOAQuotation();
                oTempNOADetail.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(oTempNOADetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetQuotationAll(NOADetail oNOADetail)
        {
            List<NOAQuotation> _oNOAQuotations = new List<NOAQuotation>();
            NOAQuotation oNOAQuotation = new NOAQuotation();
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            try
            {
                oNOADetail = oNOADetail.Get(oNOADetail.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                 oPurchaseQuotationDetails = PurchaseQuotationDetail.GetsForNOA(oNOADetail.ProductID, oNOADetail.MUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                    {
                        oNOAQuotation = new NOAQuotation();
                        oNOAQuotation.SupplierID = oItem.SupplierID;
                        oNOAQuotation.SupplierName = oItem.SupplierName;
                        oNOAQuotation.UnitPrice = oItem.UnitPrice;
                        oNOAQuotation.PQID = oItem.PurchaseQuotationDetailID;
                        _oNOAQuotations.Add(oNOAQuotation);
                    }

            }
            catch (Exception ex)
            {
                NOAQuotation oTempNOADetail = new NOAQuotation();
                oTempNOADetail.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(oTempNOADetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
          [HttpPost]
        public JsonResult GetQuotationByProduct(NOADetail oNOADetail)
        {
            List<NOAQuotation> _oNOAQuotations = new List<NOAQuotation>();
            NOAQuotation oNOAQuotation = new NOAQuotation();
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            try
            {
                oNOADetail = oNOADetail.Get(oNOADetail.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oPurchaseQuotationDetails = PurchaseQuotationDetail.GetsBy(oNOADetail.ProductID, oNOADetail.MUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                {
                    oNOAQuotation = new NOAQuotation();
                    oNOAQuotation.SupplierID = oItem.SupplierID;
                    oNOAQuotation.SupplierName = oItem.SupplierName;
                    oNOAQuotation.UnitPrice = oItem.UnitPrice;
                    oNOAQuotation.PQID = oItem.PurchaseQuotationDetailID;
                    oNOAQuotation.PQNo = oItem.PurchaseQuotationNo;
                    _oNOAQuotations.Add(oNOAQuotation);
                }

            }
            catch (Exception ex)
            {
                NOAQuotation oTempNOADetail = new NOAQuotation();
                oTempNOADetail.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(oTempNOADetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region SAve NOA Quotation

        [HttpPost]
        public JsonResult SaveNOAQuotation(NOA oNOA)
        {
            try
            {
                _oNOAQuotations = new List<NOAQuotation>();
                NOAQuotation oNOAQuotation = new NOAQuotation();
                oNOAQuotation.NOAQuotations = oNOA.NOAQuotationList;
                _oNOAQuotations = NOAQuotation.Save(oNOAQuotation, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                NOAQuotation oNOAQ = new NOAQuotation();
                oNOAQ.ErrorMessage = ex.Message;
                _oNOAQuotations.Add(oNOAQ);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOAQuotations);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //RemoveNOAQuotation
        [HttpGet]
        public JsonResult RemoveNOAQuotation(int id)
        {
            string sFeedBackMessage = "";
            NOAQuotation oNOAQuotation = new NOAQuotation();
            try
            {
                sFeedBackMessage = oNOAQuotation.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region SAve NOA Requisition

        [HttpPost]
        public JsonResult SaveNOARequisition(NOARequisition oNOARequisition)
        {
            try
            {
                _oNOARequisitions = new List<NOARequisition>();
                _oNOARequisitions = NOARequisition.Save(oNOARequisition, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oNOARequisition = new NOARequisition();
                oNOARequisition.ErrorMessage = ex.Message;
                _oNOARequisitions.Add(oNOARequisition);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOARequisitions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveNOARequisition(int id)
        {
            string sFeedBackMessage = "";
            NOARequisition oNOARequisition = new NOARequisition();
            try
            {
                sFeedBackMessage = oNOARequisition.Delete(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
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

        #region SpecificationHead
        [HttpPost]
        public JsonResult ProductSpecHeadFORPOByProduct(NOASpec oNOASpec)
        {
            ProductSpecHead _oProductSpecHead = new ProductSpecHead();
            List<ProductSpecHead> _oSProductSpecHeads = new List<ProductSpecHead>();
            NOASpec _oNOASpec = new NOASpec();
            List<NOASpec> oNOASpecs = new List<NOASpec>();
            string sSQL = string.Empty;
            try
            {
                sSQL = "Select * from View_NOASpec Where NOADetailID =" + oNOASpec.NOADetailID;
                oNOASpecs = NOASpec.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                sSQL = "Select * from View_ProductSpecHead Where ProductID =" + oNOASpec.ProductID + "Order By Sequence";
                _oSProductSpecHeads = ProductSpecHead.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (_oSProductSpecHeads.Any())
                {
                    if (oNOASpecs.Any())
                    {
                        _oSProductSpecHeads.RemoveAll(x => oNOASpecs.Select(p => p.SpecHeadID).Contains(x.SpecHeadID));

                    }
                    if (_oSProductSpecHeads.Any())
                    {
                        foreach (var oitem in _oSProductSpecHeads)
                        {
                            _oNOASpec = new NOASpec();
                            _oNOASpec.SpecName = oitem.SpecName;
                            _oNOASpec.NOASpecID = 0;
                            _oNOASpec.SpecHeadID = oitem.SpecHeadID;
                            _oNOASpec.NOADescription = string.Empty;
                            _oNOASpec.NOADetailID = oNOASpec.NOADetailID;
                            oNOASpecs.Add(_oNOASpec);

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

            var jsonResult = Json(oNOASpecs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult IUDNOASpec(NOASpec oNOASpec)
        
        {
            try
            {
                oNOASpec = oNOASpec.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oNOASpec = new NOASpec();
                oNOASpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNOASpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

    }
}
