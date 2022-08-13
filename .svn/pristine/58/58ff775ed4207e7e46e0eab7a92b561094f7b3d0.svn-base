using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;


namespace ESimSolFinancial.Controllers
{
    public class ExportSCDOController : Controller
    {
        #region Declaration
        ExportSCDO _oExportSCDO = new ExportSCDO();
        ExportSCDetailDO _oExportSCDetailDO = new ExportSCDetailDO();
        List<ExportSCDO> _oExportSCDOs = new List<ExportSCDO>();
        List<ExportSCDetailDO> _oExportSCDetailDOs = new List<ExportSCDetailDO>();
        List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
        #endregion

        #region ViewExportSCDOs
        public ActionResult ViewExportSCDOs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportSCDOs = new List<ExportSCDO>();
            _oExportSCDOs = ExportSCDO.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
        
            return View(_oExportSCDOs);
        }

        public ActionResult ViewExportSCDetailDO_Product(int id)
        {
            _oExportSCDO = new ExportSCDO();
            _oExportSCDO.ExportSCDetailDOs = new List<ExportSCDetailDO>();
            if (id > 0)
            {

                _oExportSCDO = _oExportSCDO.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportSCDO.ExportSCDetailDOs = ExportSCDetailDO.GetsByESCID(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            return View(_oExportSCDO);
        }


        [HttpPost]
        public JsonResult GetExportPIs(ExportSCDO oExportSCDO)
        {
            _oExportSCDOs = new List<ExportSCDO>();
            List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            try
            {
                string sSQL = "Select * from View_ExportSC_DO ";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oExportSCDO.PINo))
                {
                    oExportSCDO.PINo = oExportSCDO.PINo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo Like'%" + oExportSCDO.PINo + "%'";
                }
                if (!String.IsNullOrEmpty(oExportSCDO.ExportLCNo))
                {
                    oExportSCDO.PINo = oExportSCDO.ExportLCNo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ExportLCNo Like'%" + oExportSCDO.ExportLCNo + "%'";
                }
                if (oExportSCDO.LCID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LCID  in(" + oExportSCDO.LCID + ")";
                }
                if (oExportSCDO.ContractorID > 0 || oExportSCDO.BuyerID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in(" + oExportSCDO.ContractorID + "," + oExportSCDO.BuyerID + " )";
                }
                if (!oExportSCDO.IsRevisePI)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsRevisePI=0";
                }

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID=" + oExportSCDO.BUID;


                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TotalQty>DOQty and PIStatus in (2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved

                sSQL = sSQL + "" + sReturn;
                _oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportSCDO oItem in _oExportSCDOs)
                {
                    oDyeingOrder = new DyeingOrder();
                    oDyeingOrder.ExportPIID = oItem.ExportPIID;
                 
                    oDyeingOrder.DyeingOrderType = (int)EnumOrderType.BulkOrder;
                    oDyeingOrder.PaymentType = 0;
                    oDyeingOrder.ContractorID = oItem.ContractorID;
                    oDyeingOrder.ContractorName = oItem.ContractorName;
                    oDyeingOrder.DeliveryToID = oItem.BuyerID;
                    oDyeingOrder.DeliveryToName = oItem.BuyerName;
                    oDyeingOrder.ContactPersonnelID = 0;
                    oDyeingOrder.MKTEmpID = oItem.MKTEmpID;
                    oDyeingOrder.MKTPName = oItem.MKTPName;
                    oDyeingOrder.ContactPersonnelID_DelTo = 0;
                    oDyeingOrder.RefNo = "";
                    oDyeingOrder.StyleNo = "";
                    oDyeingOrder.ExportPINo = oItem.PINo;
                    oDyeingOrder.ExportLCNo = oItem.ExportLCNo + " " + oItem.ExportLCStatusSt;
                    oDyeingOrder.OrderValue = oItem.TotalAmount;
                    oDyeingOrder.OrderQty = oItem.TotalQty;
                    oDyeingOrder.TotalDOQty = oItem.POQty;
                    _oDyeingOrders.Add(oDyeingOrder);
                }
            }
            catch (Exception ex)
            {
                _oDyeingOrders = new List<DyeingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }// For Production  Order
        [HttpPost]
        public JsonResult GetExportPIs_DO(DUDeliveryOrder oDeliveryOrder)// For Delivery Order
        {
            string sSQL = "";
            _oExportSCDOs = new List<ExportSCDO>();
            List<DUDeliveryOrder> _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            DUDeliveryOrder oDUDeliveryOrder = new DUDeliveryOrder();
            try
            {
                if (oDeliveryOrder.OrderType == (int)EnumOrderType.BulkOrder || oDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly || oDeliveryOrder.OrderType == (int)EnumOrderType.ClaimOrder)
                {
                    sSQL = "Select * from View_ExportSC_DO ";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oDeliveryOrder.OrderNo))
                    {
                        oDeliveryOrder.OrderNo = oDeliveryOrder.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " PINo Like'%" + oDeliveryOrder.OrderNo + "%'";
                    }
                    if (!String.IsNullOrEmpty(oDeliveryOrder.ExportLCNo))
                    {
                        oDeliveryOrder.ExportLCNo = oDeliveryOrder.ExportLCNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ExportLCNo Like'%" + oDeliveryOrder.ExportLCNo + "%'";
                    }
                    //if (oDeliveryOrder.Expo > 0)
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + " LCID  in(" + oExportSCDO.LCID + ")";
                    //}
                    if (oDeliveryOrder.ContractorID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID  in(" + oDeliveryOrder.ContractorID + ")";
                    }
                    if (oDeliveryOrder.OrderType == (int)EnumOrderType.DyeingOnly)
                    {
                          Global.TagSQL(ref sReturn);
                          sReturn = sReturn + " ExportSCID in (Select ExportSCID from ExportSCDetail where ExportSCDetail.ProductionType in (" + (int)EnumProductionType.Commissioning + "," + (int)EnumProductionType.RawSale + ") )";
                         
                    }
                    else
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ExportSCID in (Select ExportSCID from ExportSCDetail where ExportSCDetail.ProductionType not in (" + (int)EnumProductionType.Commissioning + "," + (int)EnumProductionType.RawSale + "))";
                         
                    }
                    //if (oDeliveryOrder.OrderType == (int)EnumOrderType.ClaimOrder)
                    //{
                    //    Global.TagSQL(ref sReturn);
                    //    sReturn = sReturn + " ExportSCID in (Select OrderID from DUClaimOrder where OrderType=" + (int)EnumOrderType.BulkOrder + ")";

                    //}
                    //if (!oExportSCDO.IsRevisePI)
                    //{
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsRevisePI=0";
                    //}

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PIStatus in (2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                    //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved

                    sSQL = sSQL + "" + sReturn;
                    _oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (ExportSCDO oItem in _oExportSCDOs)
                    {
                        oDUDeliveryOrder = new DUDeliveryOrder();
                        oDUDeliveryOrder.ExportPIID = oItem.ExportPIID;

                        oDUDeliveryOrder.OrderType = oDeliveryOrder.OrderType;
                        oDUDeliveryOrder.ContractorID = oItem.ContractorID;
                        oDUDeliveryOrder.ContractorName = oItem.ContractorName;
                        //oDUDeliveryOrder.DeliveryToID = oItem.BuyerID;
                        oDUDeliveryOrder.DeliveryToName = oItem.ContractorName;
                        oDUDeliveryOrder.ContactPersonnelID = 0;
                        oDUDeliveryOrder.OrderID = oItem.ExportSCID;
                        oDUDeliveryOrder.MKTPName = oItem.MKTPName;
                        oDUDeliveryOrder.ExportPINo = oItem.PINo;
                        oDUDeliveryOrder.OrderNo = oItem.PINo;
                        oDUDeliveryOrder.ExportLCNo = oItem.ExportLCNo + " " + oItem.ExportLCStatusSt;
                        oDUDeliveryOrder.OrderValue = oItem.TotalAmount;
                        oDUDeliveryOrder.OrderQty = oItem.TotalQty;
                        //oDUDeliveryOrder.TotalDOQty = oItem.POQty;
                        _oDUDeliveryOrders.Add(oDUDeliveryOrder);
                    }
                }
                else
                {
                     sSQL = "Select * from View_DyeingOrder";
                    string sReturn = "";
                    if (!String.IsNullOrEmpty(oDeliveryOrder.OrderNo))
                    {
                        oDeliveryOrder.OrderNo = oDeliveryOrder.OrderNo.Trim();
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + "OrderNo Like'%" + oDeliveryOrder.OrderNo + "%'";
                    }
                   
                    if (oDeliveryOrder.ContractorID > 0)
                    {
                        Global.TagSQL(ref sReturn);
                        sReturn = sReturn + " ContractorID  in(" + oDeliveryOrder.ContractorID + ")";
                    }
                    //if (!oExportSCDO.IsRevisePI)
                    //{
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "DyeingOrderType=2";
                    //}

                 
                    sSQL = sSQL + "" + sReturn;
                    _oDyeingOrders = DyeingOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    foreach (DyeingOrder oItem in _oDyeingOrders)
                    {
                        oDUDeliveryOrder = new DUDeliveryOrder();
                        oDUDeliveryOrder.ExportPIID = oItem.ExportPIID;

                        oDUDeliveryOrder.OrderType = (int)EnumOrderType.SampleOrder;
                        oDUDeliveryOrder.ContractorID = oItem.ContractorID;
                        oDUDeliveryOrder.ContractorName = oItem.ContractorName;
                        //oDUDeliveryOrder.DeliveryToID = oItem.BuyerID;
                        oDUDeliveryOrder.DeliveryToName = oItem.ContractorName;
                        oDUDeliveryOrder.ContactPersonnelID = 0;
                        oDUDeliveryOrder.OrderID = oItem.DyeingOrderID;
                        oDUDeliveryOrder.MKTPName = oItem.MKTPName;
                        oDUDeliveryOrder.ExportPINo = oItem.ExportPINo;
                        oDUDeliveryOrder.OrderNo = oItem.OrderNoFull;
                        oDUDeliveryOrder.ExportLCNo = oItem.SampleInvocieNo;
                        //oDUDeliveryOrder.OrderValue = oItem.TotalAmount;
                        oDUDeliveryOrder.OrderQty = oItem.Qty;
                        //oDUDeliveryOrder.TotalDOQty = oItem.POQty;
                        _oDUDeliveryOrders.Add(oDUDeliveryOrder);
                    }
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryOrders = new List<DUDeliveryOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliveryOrders);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsExportSCDetailDO(ExportSCDO oExportSCDO)
        {
            _oExportSCDOs = new List<ExportSCDO>();
            List<ExportSCDetailDO> _oExportSCDetailDOs = new List<ExportSCDetailDO>();
            List<ExportSCDetailDO> oExportSCDetailDOs = new List<ExportSCDetailDO>();
            try
            {
                oExportSCDetailDOs = ExportSCDetailDO.GetsByPI(oExportSCDO.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (ExportSCDetailDO oItem in oExportSCDetailDOs)
                {
                    if (oItem.ProductionType <= 0 || oExportSCDO.ProductionType == (int)EnumProductionType.Full_Solution)
                    {
                        if (oItem.ProductionType <= 0 || oItem.ProductionType == (int)EnumProductionType.Full_Solution)
                        {
                            _oExportSCDetailDOs.Add(oItem);
                        }
                    }
                    else
                    {
                        if (oItem.ProductionType != (int)EnumProductionType.Full_Solution)
                        {
                            _oExportSCDetailDOs.Add(oItem);
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                _oExportSCDetailDOs = new List<ExportSCDetailDO>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCDetailDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDODetails(ExportSCDetailDO oExportSCDetailDO)
        {
            _oExportSCDOs = new List<ExportSCDO>();
            List<DyeingOrderDetail> _oDyeingOrderDetails = new List<DyeingOrderDetail>();
            try
            {
                _oDyeingOrderDetails = DyeingOrderDetail.Gets("SELECT * FROM View_DyeingOrderDetail where ExportSCDetailID =" + oExportSCDetailDO.ExportSCDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                _oDyeingOrderDetails = new List<DyeingOrderDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDyeingOrderDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportPI_ForServise(ExportSCDO oExportSCDO)
        {
            _oExportSCDOs = new List<ExportSCDO>();
            List<DyeingOrder> _oDyeingOrders = new List<DyeingOrder>();
            DyeingOrder oDyeingOrder = new DyeingOrder();
            try
            {
                string sSQL = "Select * from View_ExportSC_DO";
                string sReturn = "";
                if (!String.IsNullOrEmpty(oExportSCDO.PINo))
                {
                    oExportSCDO.PINo = oExportSCDO.PINo.Trim();
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PINo Like'%" + oExportSCDO.PINo + "%'";
                }
               
                if (oExportSCDO.ContractorID > 0 || oExportSCDO.BuyerID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ContractorID  in(" + oExportSCDO.ContractorID + "," + oExportSCDO.BuyerID + " )";
                }

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "BUID="+oExportSCDO.BUID;

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportSCID in ( Select ExportSCID from ExportSCDetail where ExportSCDetail.ProductionType in (2,3)) and IsRevisePI=0 and PIStatus in (2,3,4)"; //For Dyeing/Production Order issue->Must Checked valid PI(  Initialized = 0, RequestForApproved = 1,
                //Approved = 2,     PIIssue = 3,   BindWithLC = 4,   RequestForRevise = 5,  Cancel = 6) and Sales Contract is Approved
                sSQL = sSQL + "" + sReturn;
                _oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            catch (Exception ex)
            {
                _oDyeingOrders = new List<DyeingOrder>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }// For Production  Order
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
                MemoryStream m = new MemoryStream(oCompany.OrganizationTitle);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "CompanyImageTitle.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetSignature(UserImage oUserImage)
        {
            if (oUserImage.ImageFile != null)
            {
                MemoryStream m = new MemoryStream(oUserImage.ImageFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(this.ControllerContext.HttpContext.Server.MapPath("~/Content/") + "SignatureImage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #endregion

        #region Print Statement
        public ActionResult PrintStatement_DOBalance(int nExportSCID, int nDOID, int nProductID,double nts)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            #endregion
            this.PrintByExportSC(nExportSCID,0,nDOID, nProductID);

            _oExportSCDO.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
           
            string _sMessage = "";
            rptExportSC oReport = new rptExportSC();
            byte[] abytes = oReport.PrepareReport(_oExportSCDO, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");

        }
        public ActionResult PrintStatement_DOBalanceByPI(int nExportPIID, int nDOID, int nProductID, double nts)
        {
            #region Company Info
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #endregion

            this.PrintByExportSC(0, nExportPIID,nDOID, nProductID);

            _oExportSCDO.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            string _sMessage = "";
            rptExportSC oReport = new rptExportSC();
            byte[] abytes = oReport.PrepareReport(_oExportSCDO, oCompany, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        private void PrintByExportSC(int nExportSCID, int nExportPIID, int nDOID, int nProductID)
        {
            if (nExportSCID > 0)
            {
                _oExportSCDO = _oExportSCDO.Get(nExportSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (nExportPIID > 0)
            {
                _oExportSCDO = _oExportSCDO.GetByPI(nExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            _oExportSCDO.ExportSCDetailDOs = ExportSCDetailDO.GetsByESCID(_oExportSCDO.ExportSCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
            string sSQL = "";
            oExportPIDetails = ExportPIDetail.GetsByPI(_oExportSCDO.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oExportSCDO.ExportPIDetails = oExportPIDetails;
            _oExportSCDO.DyeingOrderDetails = new List<DyeingOrderDetail>();
            #region Production Order
            List<DyeingOrder> oDyeingOrders = new List<DyeingOrder>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            if (nProductID > 0)
            {
                oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where [Status]<9 and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and ExportPIID=" + _oExportSCDO.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderDetails = DyeingOrderDetail.Gets("Select * from View_DyeingOrderDetail where ProductID=" + nProductID + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrder.[Status]<9 and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and  ExportPIID=" + _oExportSCDO.ExportPIID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            else if (nDOID > 0)
            {
                oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where [Status]<9 and DyeingOrderID=" + nDOID + " and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and  ExportPIID=" + _oExportSCDO.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderDetails = DyeingOrderDetail.Gets("Select * from View_DyeingOrderDetail where  DyeingOrderID=" + nDOID + " and  DyeingOrderID in (Select DyeingOrderID from DyeingOrder where [Status]<9 and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and  ExportPIID=" + _oExportSCDO.ExportPIID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            else
            {
                oDyeingOrders = DyeingOrder.Gets("Select * from View_DyeingOrder where [Status]<9 and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and  ExportPIID=" + _oExportSCDO.ExportPIID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oDyeingOrderDetails = DyeingOrderDetail.Gets("Select * from View_DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where [Status]<9 and DyeingOrderType!=" + (int)EnumOrderType.ClaimOrder + " and ExportPIID=" + _oExportSCDO.ExportPIID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);

            }

            foreach (DyeingOrder oItem in oDyeingOrders)
            {
                List<DyeingOrderDetail> oDODetails = new List<DyeingOrderDetail>();
                oDODetails = oDyeingOrderDetails.Where(o => o.DyeingOrderID == oItem.DyeingOrderID).ToList();
                foreach (DyeingOrderDetail oItemDOD in oDODetails)
                {
                    oItemDOD.OrderNo = oItem.OrderNoFull;
                    //oItemDOD.OrderDate = oItem.OrderDateSt;
                    _oExportSCDO.DyeingOrderDetails.Add(oItemDOD);
                }
            }
            _oExportSCDO.AdjQty = oDyeingOrders.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder || o.DyeingOrderType == (int)EnumOrderType.SampleOrder_Two || o.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Qty);
            _oExportSCDO.AdjAmount = oDyeingOrders.Where(o => o.DyeingOrderType == (int)EnumOrderType.SampleOrder || o.DyeingOrderType == (int)EnumOrderType.SampleOrder_Two || o.DyeingOrderType == (int)EnumOrderType.SaleOrder).Sum(x => x.Amount);

            #endregion
            if (_oExportSCDO.ExportSCDetailDOs.Count > 0)
            {
                _oExportSCDO.MUName = _oExportSCDO.ExportSCDetailDOs[0].MUName;
            }
            if (_oExportSCDO.DyeingOrderDetails.Count > 0)
            {
                _oExportSCDO.MUName = _oExportSCDO.DyeingOrderDetails[0].MUnit;
            }

            #region Delivery Order
            ////
            List<DUDeliveryOrder> oDUDeliveryOrders = new List<DUDeliveryOrder>();
            //sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and DOStatus>0 and DO.OrderID=" + _oExportSCDO.ExportSCID;
            sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and DOStatus>0 and  DO.DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where ExportSCDetailID>0 and ExportSCDetailID in (Select ExportSCDetailID from ExportSCDetail where ExportSCID="+ _oExportSCDO.ExportSCID+"))";
            oDUDeliveryOrders = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUDeliveryOrders.Count > 0)
            {
                List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails = new List<DUDeliveryOrderDetail>();
                sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE ExportSCDetailID>0 and ExportSCDetailID in (Select ExportSCDetailID from ExportSCDetail where ExportSCID=" + _oExportSCDO.ExportSCID + ") and DOD.DUDeliveryOrderID in (" + string.Join(",", oDUDeliveryOrders.Select(x => x.DUDeliveryOrderID).ToList()) + ") and DOD.DUDeliveryOrderID in (SELECT DO.DUDeliveryOrderID FROM DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ") )";
                oDUDeliveryOrderDetails = DUDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUDeliveryOrder oItem in oDUDeliveryOrders)
                {
                    if (oItem.ApproveBy != 0)
                    {
                        List<DUDeliveryOrderDetail> oDU_DCDs = new List<DUDeliveryOrderDetail>();
                        oDU_DCDs = oDUDeliveryOrderDetails.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList();
                        foreach (DUDeliveryOrderDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.DeliveryDate = oItem.DODate;
                            oItemDCD.OrderNo = oItem.DONoFull;
                            _oExportSCDO.DUDeliveryOrderDetails.Add(oItemDCD);
                        }
                    }
                }
                _oExportSCDO.Qty_DO = _oExportSCDO.DUDeliveryOrderDetails.Select(c => c.Qty).Sum();
            }
            ///
            #endregion

            #region Challan
            ////
            if (_oExportSCDO.DUDeliveryOrderDetails.Count > 0)
            {
                sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE DC.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oExportSCDO.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + "))";
            }
            _oExportSCDO.DUDeliveryChallans = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (_oExportSCDO.DUDeliveryChallans.Count > 0)
            {
                List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails = new List<DUDeliveryChallanDetail>();
                sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DODetailID in (" + string.Join(",", _oExportSCDO.DUDeliveryOrderDetails.Select(x => x.DUDeliveryOrderDetailID).ToList()) + ")";
                oDUDeliveryChallanDetails = DUDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUDeliveryChallan oItem in _oExportSCDO.DUDeliveryChallans)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUDeliveryChallanDetail> oDU_DCDs = new List<DUDeliveryChallanDetail>();
                    oDU_DCDs = oDUDeliveryChallanDetails.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList();
                    foreach (DUDeliveryChallanDetail oItemDCD in oDU_DCDs)
                    {
                        oItemDCD.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy");
                        oItemDCD.ChallanNo = oItem.ChallanNo;
                        _oExportSCDO.DUDeliveryChallanDetails.Add(oItemDCD);
                    }
                    //}
                }
                _oExportSCDO.Qty_DC = _oExportSCDO.DUDeliveryChallanDetails.Select(c => c.Qty).Sum();

                #region Return Challan

                if (_oExportSCDO.DUDeliveryChallanDetails.Count > 0)
                {
                    List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                    sSQL = "Select * from View_DUReturnChallanDetail where DUDeliveryChallanDetailID in (" + string.Join(",", _oExportSCDO.DUDeliveryChallanDetails.Select(x => x.DUDeliveryChallanDetailID).ToList()) + ")";
                    oDUReturnChallanDetails = DUReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    //sSQL = "SELECT * FROM View_DUReturnChallan where DUDeliveryChallanID in (SELECT DC.DUDeliveryChallanID FROM VIEW_DUDeliveryChallan as DC WHERE DC.OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in (Select DUDeliveryOrderDetail.DUDeliveryOrderDetailID from  DUDeliveryOrderDetail where DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrder where DUDeliveryOrder.OrderID=" + _oExportSCDO.ExportSCID + " and OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")))))";
                    if (oDUReturnChallanDetails.Count > 0)
                    {
                        sSQL = "SELECT * FROM View_DUReturnChallan where DUReturnChallanID in (" + string.Join(",", oDUReturnChallanDetails.Select(x => x.DUReturnChallanID).ToList()) + ")";
                        _oExportSCDO.DUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }

                    foreach (DUReturnChallan oItem in _oExportSCDO.DUReturnChallans)
                    {
                        //if (oItem.ApprovedBy != 0)
                        //{
                        List<DUReturnChallanDetail> oDU_DCDs = new List<DUReturnChallanDetail>();
                        oDU_DCDs = oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList();
                        foreach (DUReturnChallanDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy");
                            oItemDCD.RCNo = oItem.DUReturnChallanNo;
                            _oExportSCDO.DUReturnChallanDetails.Add(oItemDCD);
                        }
                        //}
                    }
                    _oExportSCDO.Qty_RC = _oExportSCDO.DUReturnChallanDetails.Select(c => c.Qty).Sum();
                }
                #endregion End Return Challan
            }
            ///
            #endregion

            #region Export Bill
            ////
            if(_oExportSCDO.LCID>0)
            {
                ExportLC oExportLC = new ExportLC();
                oExportLC = oExportLC.Get(_oExportSCDO.LCID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oExportSCDO.Amount_LC = oExportLC.Amount;
                List<ExportBillDetail> oExportBillDetails = new List<ExportBillDetail>();
                List<ExportBill> oExportBills = new List<ExportBill>();
                sSQL = "SELECT * FROM View_ExportBill as EB WHERE [State]<>11 and EB.ExportBillID in (Select EBD.ExportBillID from ExportBillDetail as EBD where  EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID="+ _oExportSCDO.ExportPIID + "))";
                oExportBills = ExportBill.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oExportBills.Count > 0)
                {
                    sSQL = "SELECT * FROM View_ExportBillDetail as EBD WHERE EBD.ExportPIDetailID in (Select ExportPIDetail.ExportPIDetailID from ExportPIDetail where ExportPIID=" + _oExportSCDO.ExportPIID + " )";
                    oExportBillDetails = ExportBillDetail.GetsBySQL(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    //_oExportSCDO.Qty_DC = oExportBillDetails.Select(c => c.Qty).Sum();
                 
                }
                foreach (ExportBill oItem in oExportBills)
                {
                        List<ExportBillDetail> oEB_Details = new List<ExportBillDetail>();
                        oEB_Details = oExportBillDetails.Where(o => o.ExportBillID == oItem.ExportBillID).ToList();
                        foreach (ExportBillDetail oItemDCD in oEB_Details)
                        {
                            if (oItem.State >= EnumLCBillEvent.BOEInCustomerHand)
                            {
                                _oExportSCDO.AcceptanceIssue = _oExportSCDO.AcceptanceIssue + oItemDCD.Qty;
                            }
                            if (oItem.State >= EnumLCBillEvent.BuyerAcceptedBill)
                            {
                                _oExportSCDO.AcceptanceRcvd = _oExportSCDO.AcceptanceRcvd + oItemDCD.Qty;
                            }
                        
                        }
                        if (oItem.State >= EnumLCBillEvent.BankAcceptedBill)
                        {
                            _oExportSCDO.MaturityDate = oItem.MaturityDateSt;
                            _oExportSCDO.PaymentDate = oItem.RelizationDateSt;
                        }
                }
            }
            #endregion

            #region Claim Order
            ////
            List<DUClaimOrder> oDUClaimOrders = new List<DUClaimOrder>();

            //sSQL = "SELECT * FROM View_DUClaimorder WHERE OrderType in (" + (int)EnumOrderType.BulkOrder + "," + (int)EnumOrderType.DyeingOnly + ")  and OrderID=" + _oExportSCDO.ExportSCID;
            sSQL = "SELECT * FROM View_DUClaimorder WHERE  ParentDOID in (Select DyeingOrderID from DyeingOrder where  DyeingOrder.ExportPIID=" + _oExportSCDO.ExportPIID + ")";
            oDUClaimOrders = DUClaimOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUClaimOrders.Count > 0)
            {
                List<DUClaimOrderDetail> oDUClaimOrderDetails = new List<DUClaimOrderDetail>();
                sSQL = "Select * from View_DUClaimOrderDetail where DUClaimOrderID in (SELECT DUClaimOrderID FROM View_DUClaimorder WHERE  ParentDOID in (Select DyeingOrderID from DyeingOrder where  DyeingOrder.ExportPIID=" + _oExportSCDO.ExportPIID + "))";
                oDUClaimOrderDetails = DUClaimOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUClaimOrder oItem in oDUClaimOrders)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUClaimOrderDetail> oDU_DCDs = new List<DUClaimOrderDetail>();
                    oDU_DCDs = oDUClaimOrderDetails.Where(o => o.DUClaimOrderID == oItem.DUClaimOrderID).ToList();
                    foreach (DUClaimOrderDetail oItemDCD in oDU_DCDs)
                    {
                        oItemDCD.Date = oItem.OrderDate.ToString("dd MMM yyyy");
                        oItemDCD.ClaimOrderNo = oItem.ClaimOrderNo;
                        _oExportSCDO.DUClaimOrderDetails.Add(oItemDCD);
                    }
                    //}
                }
                _oExportSCDO.Qty_Claim = _oExportSCDO.DUClaimOrderDetails.Select(c => c.Qty).Sum();
            }
            ///
            #endregion

            #region Delivery Order Claim
            ////
            if (_oExportSCDO.DUClaimOrderDetails.Count > 0)
            {
                string sIDs = "";
                sIDs = string.Join(",", _oExportSCDO.DUClaimOrderDetails.Select(x => x.DyeingOrderDetailID).ToList());

                List<DUDeliveryOrder> oDUDeliveryOrders_Claim = new List<DUDeliveryOrder>();
                //sSQL = "SELECT * FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and DOStatus>0 and DO.OrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType in (" + (int)EnumOrderType.ClaimOrder + ") and DyeingOrder.ExportPIID=" + _oExportSCDO.ExportPIID + ")";
                sSQL = "SELECT * FROM View_DUDeliveryOrder as DO where DO.OrderType in (" + (int)EnumOrderType.ClaimOrder + ") and DO.DUDeliveryOrderID in (Select DUDeliveryOrderID from DUDeliveryOrderDetail where DyeingOrderDetailID in (" + sIDs + "))";
                oDUDeliveryOrders_Claim = DUDeliveryOrder.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDUDeliveryOrders_Claim.Count > 0)
                {
                    List<DUDeliveryOrderDetail> oDUDeliveryOrderDetails_Claim = new List<DUDeliveryOrderDetail>();
                    //sSQL = "SELECT * FROM View_DUDeliveryOrderDetail as DOD WHERE DOD.DUDeliveryOrderID in (SELECT DUDeliveryOrderID FROM View_DUDeliveryOrder as DO WHERE DO.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and DOStatus>0 and DO.OrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType  in (" + (int)EnumOrderType.ClaimOrder + ") and DyeingOrder.ExportPIID=" + _oExportSCDO.ExportPIID + "))";
                    sSQL = "Select * from View_DUDeliveryOrderDetail where DyeingOrderDetailID in (" + sIDs + ")";
                    oDUDeliveryOrderDetails_Claim = DUDeliveryOrderDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    foreach (DUDeliveryOrder oItem in oDUDeliveryOrders_Claim)
                    {
                        //if (oItem.ApproveBy != 0)
                        //{
                        List<DUDeliveryOrderDetail> oDU_DCDs = new List<DUDeliveryOrderDetail>();
                        oDU_DCDs = oDUDeliveryOrderDetails_Claim.Where(o => o.DUDeliveryOrderID == oItem.DUDeliveryOrderID).ToList();
                        foreach (DUDeliveryOrderDetail oItemDCD in oDU_DCDs)
                        {
                            oItemDCD.DeliveryDate = oItem.DODate;
                            oItemDCD.OrderNo = oItem.DONoFull;
                            _oExportSCDO.DUDeliveryOrderDetails_Claim.Add(oItemDCD);
                        }
                        //}
                    }
                    _oExportSCDO.Qty_DO = _oExportSCDO.Qty_DO + _oExportSCDO.DUDeliveryOrderDetails_Claim.Select(c => c.Qty).Sum();
                }
            }
            ///
            #endregion
            #region Challan Claim
            ////
            if(_oExportSCDO.DUDeliveryOrderDetails_Claim.Count>0)
            { 
            List<DUDeliveryChallan> oDUDeliveryChallans_Claim = new List<DUDeliveryChallan>();
            sSQL = "SELECT * FROM VIEW_DUDeliveryChallan as DC WHERE DC.OrderType in (" + (int)EnumOrderType.ClaimOrder + ")  and IsDelivered=1 and DC.DUDeliveryChallanID in (SELECT DCD.DUDeliveryChallanID FROM DUDeliveryChallanDetail as DCD where DCD.DODetailID in  (" + GetDODetailIDs(_oExportSCDO.DUDeliveryOrderDetails_Claim) + "))";
            oDUDeliveryChallans_Claim = DUDeliveryChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oDUDeliveryChallans_Claim.Count > 0)
            {
                List<DUDeliveryChallanDetail> oDUDeliveryChallanDetails_Claim = new List<DUDeliveryChallanDetail>();
                sSQL = "SELECT * FROM View_DUDeliveryChallanDetail as DCD where DCD.DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + ") and DCD.DODetailID in (" + GetDODetailIDs(_oExportSCDO.DUDeliveryOrderDetails_Claim) + ")";
                oDUDeliveryChallanDetails_Claim = DUDeliveryChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (DUDeliveryChallan oItem in oDUDeliveryChallans_Claim)
                {
                    //if (oItem.ApproveBy != 0)
                    //{
                    List<DUDeliveryChallanDetail> oDU_DCDs = new List<DUDeliveryChallanDetail>();
                    oDU_DCDs = oDUDeliveryChallanDetails_Claim.Where(o => o.DUDeliveryChallanID == oItem.DUDeliveryChallanID).ToList();
                    foreach (DUDeliveryChallanDetail oItemDCD in oDU_DCDs)
                    {
                        oItemDCD.ChallanDate = oItem.ChallanDate.ToString("dd MMM yyyy");
                        oItemDCD.ChallanNo = oItem.ChallanNo;
                        _oExportSCDO.DUDeliveryChallanDetails_Claim.Add(oItemDCD);
                    }
                    //}
                }
                //_oExportSCDO.Qty_DC = _oExportSCDO.DUDeliveryChallanDetails_Claim.Select(c => c.Qty).Sum();

                #region Return Challan
                if (oDUDeliveryChallans_Claim.Count > 0)
                {
                    sSQL = "SELECT * FROM View_DUReturnChallan where DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + ")";
                    _oExportSCDO.DUReturnChallans = DUReturnChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oExportSCDO.DUReturnChallans.Count > 0)
                    {
                        List<DUReturnChallanDetail> oDUReturnChallanDetails = new List<DUReturnChallanDetail>();
                        sSQL = "Select * from View_DUReturnChallanDetail where DUReturnChallanID in (SELECT RC.DUReturnChallanID FROM DUReturnChallan as RC where DUDeliveryChallanID in (" + GetDUDChallanIDs(oDUDeliveryChallans_Claim) + "))";
                        oDUReturnChallanDetails = DUReturnChallanDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                        foreach (DUReturnChallan oItem in _oExportSCDO.DUReturnChallans)
                        {
                            //if (oItem.ApprovedBy != 0)
                            //{
                            List<DUReturnChallanDetail> oDU_DCDs_Claim = new List<DUReturnChallanDetail>();
                            oDU_DCDs_Claim = oDUReturnChallanDetails.Where(o => o.DUReturnChallanID == oItem.DUReturnChallanID).ToList();
                            foreach (DUReturnChallanDetail oItemDCD in oDU_DCDs_Claim)
                            {
                                oItemDCD.RCDate = oItem.ReturnDate.ToString("dd MMM yyyy");
                                oItemDCD.RCNo = oItem.DUReturnChallanNo;
                                _oExportSCDO.DUReturnChallanDetails_Claim.Add(oItemDCD);
                            }
                            //}
                        }
                        _oExportSCDO.Qty_RC = _oExportSCDO.Qty_RC + _oExportSCDO.DUReturnChallanDetails.Select(c => c.Qty).Sum();
                    }
                }
                #endregion End Return Challan
            }
            }
            ///
            #endregion
        }

        private string GetDUDChallanIDs(List<DUDeliveryChallan> oDUDeliveryChallans)
        {
            string sResult = "";
            foreach (DUDeliveryChallan oItem in oDUDeliveryChallans)
            {
                sResult = oItem.DUDeliveryChallanID + "," + sResult;
            }
            if (!string.IsNullOrEmpty(sResult))
            {
                sResult = sResult.Remove((sResult.Length - 1), 1);
            }
            return sResult;
        }
        private string GetDODetailIDs(List<DUDeliveryOrderDetail> oDODetails)
        {
            string sResult = "";
            foreach (DUDeliveryOrderDetail oItem in oDODetails)
            {
                sResult = oItem.DUDeliveryOrderDetailID + "," + sResult;
            }
            if (!string.IsNullOrEmpty(sResult))
            {
                sResult = sResult.Remove((sResult.Length - 1), 1);
            }
            return sResult;
        }
        #endregion
        
        #region
        [HttpPost]
        public JsonResult GetbyNo(ExportSCDO oExportSCDO)
        {
            string sPINo = Convert.ToString(oExportSCDO.PINo.Split('~')[0]);
            string sSampleInvoiceNo = Convert.ToString(oExportSCDO.PINo.Split('~')[1]);
            string sOrderNo = Convert.ToString(oExportSCDO.PINo.Split('~')[2]);

            _oExportSCDOs = new List<ExportSCDO>();
            string sSQL = "SELECT * FROM View_ExportSC_DO";
            string sReturn = "";
            if (!String.IsNullOrEmpty(sPINo))
            {
               sPINo = sPINo.Trim();
               Global.TagSQL(ref sReturn);
               sReturn =sReturn+ "PINo like '%" + sPINo + "%'";
            }
            if (!String.IsNullOrEmpty(sSampleInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportPIID in ( Select  ExportPIID from  View_DyeingOrder where DyeingOrderType=2 and PaymentType in (2) and View_DyeingOrder.SampleInvocieNo='%" + sSampleInvoiceNo + "%')";
            }
            if (!String.IsNullOrEmpty(sOrderNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ExportPIID in ( Select  ExportPIID from  View_DyeingOrder where DyeingOrderType=3 and View_DyeingOrder.OrderNo='%" + sOrderNo + "%')";
            }

            Global.TagSQL(ref sReturn);
            sReturn = sReturn + "BUID="+ oExportSCDO.BUID;

             sSQL = sSQL + " " + sReturn;

            _oExportSCDOs = ExportSCDO.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportSCDOs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
