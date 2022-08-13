using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

namespace ESimSolFinancial.Controllers
{
    public class DeliveryOrderController : Controller
    {
        #region Declaration
        DeliveryOrder _oDeliveryOrder = new DeliveryOrder();
        List<DeliveryOrder> _oDeliveryOrders = new List<DeliveryOrder>();
        DeliveryOrderDetail _oDeliveryOrderDetail = new DeliveryOrderDetail();
        List<DeliveryOrderDetail> _oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
        #endregion

        #region Actions
        public ActionResult ViewDeliveryOrders(int ProductNature, int buid, int menuid)
        {
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.DeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oDeliveryOrders = new List<DeliveryOrder>();
            string sSQL = "SELECT * FROM View_DeliveryOrder AS HH WHERE HH.BUID=" + buid.ToString() + " AND HH.ProductNature=" + ProductNature.ToString() + " AND HH.DOStatus<=4 ORDER BY DeliveryOrderID ASC";
            _oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oDeliveryOrders);
        }
        public ActionResult ViewDeliveryOrder(int id)
        {
            _oDeliveryOrder = new DeliveryOrder();
            if (id > 0)
            {
                _oDeliveryOrder = _oDeliveryOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryOrder.DeliveryOrderDetails = DeliveryOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }

            ViewBag.EnumRefTypes = Enum.GetValues(typeof(EnumRefType)).Cast<EnumRefType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oDeliveryOrder);
        }
        public ActionResult ViewDeliveryOrderMDApprove(int id)
        {
            _oDeliveryOrder = new DeliveryOrder();
            ExportPIRegister oExportPIRegister = new ExportPIRegister();
            ExportPIRegisterController oExportPIRegisterController = new ExportPIRegisterController();
            if (id > 0)
            {
                _oDeliveryOrder = _oDeliveryOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryOrder.DeliveryOrderDetails = DeliveryOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

                oExportPIRegister.SearchingData = "0~" + DateTime.Today.ToString("dd MMM yyyy") + "~" + DateTime.Today.ToString("dd MMM yyyy") + "~0";
                oExportPIRegister.IsWithDue = true;
                oExportPIRegister.BuyerName= _oDeliveryOrder.ContractorID.ToString();
                string sSQL = oExportPIRegisterController.GetSQL(oExportPIRegister);
                ViewBag.ExportPIRegisters = ExportPIRegister.Gets(sSQL, (int)EnumReportLayout.PartyWise,1, (int)Session[SessionInfo.currentUserID]);
            }
         
            ViewBag.EnumRefTypes = Enum.GetValues(typeof(EnumRefType)).Cast<EnumRefType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oDeliveryOrder);
        }
        public ActionResult ViewDeliveryOrderRevise(int id)
        {
            _oDeliveryOrder = new DeliveryOrder();
            _oDeliveryOrder = _oDeliveryOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oDeliveryOrder.DeliveryOrderDetails = DeliveryOrderDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            ViewBag.EnumRefTypes = Enum.GetValues(typeof(EnumRefType)).Cast<EnumRefType>().Select(x => new SelectListItem { Text = Global.EnumerationFormatter(x.ToString()), Value = ((int)x).ToString() }).ToList();
            return View(_oDeliveryOrder);
        }
        [HttpPost]
        public JsonResult Save(DeliveryOrder oDeliveryOrder)
        {
            _oDeliveryOrder = new DeliveryOrder();
            try
            {
                short nDBOperation = (short)((oDeliveryOrder.DeliveryOrderID<=0)? EnumDBOperation.Insert:EnumDBOperation.Update);
                _oDeliveryOrder = oDeliveryOrder.IUD(nDBOperation, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryOrder = new DeliveryOrder();
                _oDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult AcceptRevise(DeliveryOrder oDeliveryOrder)
        {
            _oDeliveryOrder = new DeliveryOrder();
            try
            {
                short nDBOperation = (short)((oDeliveryOrder.DeliveryOrderID <= 0) ? EnumDBOperation.Insert : EnumDBOperation.Update);
                _oDeliveryOrder = oDeliveryOrder.AcceptRevise(nDBOperation, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryOrder = new DeliveryOrder();
                _oDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(DeliveryOrder oDeliveryOrder)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oDeliveryOrder.DeliveryOrderID <= 0)
                    throw new Exception("Invalid delivery order.");
                oDeliveryOrder = oDeliveryOrder.IUD((short)EnumDBOperation.Delete, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrder.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       [HttpPost]
        public JsonResult Approve(DeliveryOrder oDeliveryOrder)
        {
            try
            {
                oDeliveryOrder = oDeliveryOrder.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryOrder = new DeliveryOrder();
                oDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
        [HttpPost]
        public JsonResult GetExportPIDetails(ExportPI oExportPI)
        {
            List<DeliveryOrderDetail> oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPIDetail WHERE ExportPIID = " + oExportPI.ExportPIID + " AND ISNULL(YetToDeliveryOrderQty,0)>0";
                oExportPIDetails = ExportPIDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                ExportPIDetail oExportPIDetail = new ExportPIDetail();
                oExportPIDetail.ErrorMessage = ex.Message;
                oExportPIDetails.Add(oExportPIDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Searching
        private string GetSQL(string sTemp)
        {

            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtDOFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtDOTo = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sDONo = sTemp.Split('~')[3];
            string sPINo = sTemp.Split('~')[4];
            string sBuyerIDs = sTemp.Split('~')[5];
            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[6]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[7]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[8]);
            int nProductNature = Convert.ToInt32(sTemp.Split('~')[9]);
            
            string sReturn1 = "SELECT * FROM View_DeliveryOrder";
            string sReturn = "";

            #region DO
            if (!string.IsNullOrEmpty(sDONo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DONo LIKE '%" + sDONo+"%'";
            }
            #endregion

            #region PI
            if (!string.IsNullOrEmpty(sPINo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " RefNo LIKE '%" + sPINo + "%' AND RefType = "+(int)EnumRefType.ExportPI;
            }
            #endregion
            #region BU
            if (nBUID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion
            #region ProductNature
            if (nProductNature != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductNature = " + nProductNature;
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + sBuyerIDs + ")";
            }
            #endregion


            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate = '" + dtDOFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate != '" + dtDOFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate > '" + dtDOFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate < '" + dtDOFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate>= '" + dtDOFrom.ToString("dd MMM yyyy") + "' AND DODate < '" + dtDOTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DODate< '" + dtDOFrom.ToString("dd MMM yyyy") + "' OR DODate > '" + dtDOTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ISNULL(ApproveBy,0) != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  ISNULL(ApproveBy,0) = 0";
            }
            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY ContractorID, DeliveryOrderID";
            return sReturn;
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<DeliveryOrder> oDeliveryOrders = new List<DeliveryOrder>();
            try
            {
                string sSQL = GetSQL(Temp);
                oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDeliveryOrders = new List<DeliveryOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Gets from different Module

        [HttpPost]
        public JsonResult SearchByExportPIBUWise(ExportPI oExportPI)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            try
            {
                string sSQL = "SELECT * FROM View_ExportPI WHERE BUID = " + oExportPI.BUID + "  AND ISNULL(ApprovedBy,0)!=0 AND ISNULL(YetToDeliveryOrderQty,0)>0";
                oExportPIs = ExportPI.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oExportPI = new ExportPI();
                oExportPI.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oExportPIs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
     
        #region GetDeliveryOrders
      
        [HttpPost]
        public JsonResult GetDODetails(DeliveryOrder oDeliveryOrder)
        {
            _oDeliveryOrderDetails = new List<DeliveryOrderDetail>();
            try
            {
                _oDeliveryOrderDetails = DeliveryOrderDetail.Gets(oDeliveryOrder.DeliveryOrderID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryOrderDetail = new DeliveryOrderDetail();
                _oDeliveryOrderDetail.ErrorMessage = ex.Message;
                _oDeliveryOrderDetails.Add(_oDeliveryOrderDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetReviseHistory(DeliveryOrder oDeliveryOrder)
        {
            _oDeliveryOrders = new List<DeliveryOrder>();
            try
            {
                string sSQL = "SELECT * FROM View_DeliveryOrderLog WHERE DeliveryOrderID = " + oDeliveryOrder.DeliveryOrderID;
                _oDeliveryOrders = DeliveryOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryOrder = new DeliveryOrder();
                _oDeliveryOrder.ErrorMessage = ex.Message;
                _oDeliveryOrders.Add(_oDeliveryOrder);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(DeliveryOrder oDeliveryOrder)
        {
            _oDeliveryOrder = new DeliveryOrder();
            _oDeliveryOrder = oDeliveryOrder;
            try
            {
                if (oDeliveryOrder.ActionTypeExtra == "RequestForApproval")
                {
                    _oDeliveryOrder.DOActionType = EnumDOActionType.RequestForApproved;
                    _oDeliveryOrder.ApprovalRequest.RequestBy = ((int)Session[SessionInfo.currentUserID]);
                    _oDeliveryOrder.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.DeliveryOrder;

                }
                else if (oDeliveryOrder.ActionTypeExtra == "UndoRequest")
                {
                    _oDeliveryOrder.DOActionType = EnumDOActionType.Undo_Request;
                }
                else if (oDeliveryOrder.ActionTypeExtra == "Approve")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.Approved;

                }
                else if (oDeliveryOrder.ActionTypeExtra == "UndoApprove")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.Undo_Approved;
                }
                else if (oDeliveryOrder.ActionTypeExtra == "RequestForMDApprove")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.Request_For_MD_Approve;

                }
                else if (oDeliveryOrder.ActionTypeExtra == "MDApprove")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.MD_Approve;

                }
                else if (oDeliveryOrder.ActionTypeExtra == "SendToFactory")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.Send_To_Factory;
                }
                else if (oDeliveryOrder.ActionTypeExtra == "Received")
                {

                    _oDeliveryOrder.DOActionType = EnumDOActionType.Received;
                }
                else if (oDeliveryOrder.ActionTypeExtra == "Cancel")
                {
                    _oDeliveryOrder.DOActionType = EnumDOActionType.Cancel;
                }
                else if (oDeliveryOrder.ActionTypeExtra == "Request_For_Revise")
                {
                    _oDeliveryOrder.DOActionType = EnumDOActionType.Request_For_Revise;
                }
                _oDeliveryOrder = SetORSStatus(_oDeliveryOrder);
                _oDeliveryOrder = _oDeliveryOrder.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDeliveryOrder = new DeliveryOrder();
                _oDeliveryOrder.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDeliveryOrder);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Set Status
        private DeliveryOrder SetORSStatus(DeliveryOrder oDeliveryOrder)//Set EnumOrderStatus Value
        {
            switch (oDeliveryOrder.DOStatusInInt)
            {
                case 0:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Initialized;
                        break;
                    }
                case 1:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.RequestForApproved;
                        break;
                    }
                case 2:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Approved;
                        break;
                    }

                case 3:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Request_For_MD_Approve;
                        break;
                    }
                case 4:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.MD_Approve;
                        break;
                    }
                case 5:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Send_To_Factory;
                        break;
                    }
                case 6:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Received;
                        break;
                    }
                case 7:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Challan_Issue;
                        break;
                    }
                case 8:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Challan_Deliverd;
                        break;
                    }
                case 9:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Cancel;
                        break;
                    }
                case 10:
                    {
                        oDeliveryOrder.DOStatus = EnumDOStatus.Request_For_Revise;
                        break;
                    }
            }

            return oDeliveryOrder;
        }
        #endregion
        #endregion


        #region Print
        public ActionResult DeliveryOrderPrintPreview(int id)
        {
            _oDeliveryOrder = new DeliveryOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oDeliveryOrder = _oDeliveryOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryOrder.DeliveryOrderDetails = DeliveryOrderDetail.Gets(_oDeliveryOrder.DeliveryOrderID, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryOrder.BusinessUnit = oBusinessUnit.Get(_oDeliveryOrder.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oDeliveryOrder.Company = oCompany;

            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.PrepareBy, (int)Session[SessionInfo.currentUserID]);
            Image oPrepareBySignature = this.GetSignature(oAttachDocument);

            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.ApproveBy, (int)Session[SessionInfo.currentUserID]);
            Image oSESignature = this.GetSignature(oAttachDocument);

            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.ReceiveBy, (int)Session[SessionInfo.currentUserID]);
            Image oReceivedSignature = this.GetSignature(oAttachDocument);


            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.MDApproveBy, (int)Session[SessionInfo.currentUserID]);
            Image oMDApprovedSignature = this.GetSignature(oAttachDocument);

            byte[] abytes;
            rptDeliveryOrderPreview oReport = new rptDeliveryOrderPreview();
            abytes = oReport.PrepareReport(_oDeliveryOrder, oPrepareBySignature, oSESignature, oReceivedSignature, oMDApprovedSignature);
            return File(abytes, "application/pdf");
        }
        public ActionResult DOPreviewWithStock(int id)
        {
            _oDeliveryOrder = new DeliveryOrder();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oDeliveryOrder = _oDeliveryOrder.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oDeliveryOrder.DeliveryOrderDetails = DeliveryOrderDetail.Gets(_oDeliveryOrder.DeliveryOrderID, (int)Session[SessionInfo.currentUserID]);
                if (_oDeliveryOrder.RefType == EnumRefType.ExportPI)
                {
                    List<PTUUnit2> oPTUUnit2s = new List<PTUUnit2>();
                    string sSQL = "SELECT * FROM View_PTUUnit2 WHERE ExportSCDetailID IN (" + string.Join(",", _oDeliveryOrder.DeliveryOrderDetails.Select(x => x.RefDetailID)).ToString() + ")";
                    oPTUUnit2s = PTUUnit2.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                    foreach (DeliveryOrderDetail oItem in _oDeliveryOrder.DeliveryOrderDetails)
                    {
                        PTUUnit2 oPTUUnit2 = new PTUUnit2();
                        oPTUUnit2 = oPTUUnit2s.Where(x => x.ExportSCDetailID == oItem.RefDetailID).FirstOrDefault();
                        if (oPTUUnit2 != null)
                        {
                            oItem.Note = "@Stock Qty: " + oPTUUnit2.ReadyStockQty.ToString() + " " + oPTUUnit2.UnitSymbol;
                        }
                    }
                }
                _oDeliveryOrder.BusinessUnit = oBusinessUnit.Get(_oDeliveryOrder.BUID, (int)Session[SessionInfo.currentUserID]);

            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oDeliveryOrder.Company = oCompany;

            AttachDocument oAttachDocument = new AttachDocument();
            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.PrepareBy, (int)Session[SessionInfo.currentUserID]);
            Image oPrepareBySignature = this.GetSignature(oAttachDocument);

            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.ApproveBy, (int)Session[SessionInfo.currentUserID]);
            Image oSESignature = this.GetSignature(oAttachDocument);

            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.ReceiveBy, (int)Session[SessionInfo.currentUserID]);
            Image oReceivedSignature = this.GetSignature(oAttachDocument);


            oAttachDocument = new AttachDocument();
            oAttachDocument = oAttachDocument.GetUserSignature(_oDeliveryOrder.MDApproveBy, (int)Session[SessionInfo.currentUserID]);
            Image oMDApprovedSignature = this.GetSignature(oAttachDocument);

            byte[] abytes;
            rptDeliveryOrderPreview oReport = new rptDeliveryOrderPreview();
            abytes = oReport.PrepareReport(_oDeliveryOrder, oPrepareBySignature, oSESignature, oReceivedSignature, oMDApprovedSignature);
            return File(abytes, "application/pdf");
        }
        #endregion Print

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
        
        public Image GetSignature(AttachDocument oAttachDocument)
        {
            if (oAttachDocument.AttachFile != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oAttachDocument.AttachFile);
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
