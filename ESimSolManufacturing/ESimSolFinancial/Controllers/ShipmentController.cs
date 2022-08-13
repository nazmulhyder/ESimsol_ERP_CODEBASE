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
    public class ShipmentController : Controller
    {
        #region Declaration

        Shipment _oShipment = new Shipment();
        List<Shipment> _oShipments = new List<Shipment>();
        ShipmentDetail _oShipmentDetail = new ShipmentDetail();
        List<ShipmentDetail> _oShipmentDetails = new List<ShipmentDetail>();
        List<User> _oUsers = new List<User>();
        List<ShipmentRegister> _oShipmentRegisters = new List<ShipmentRegister>();
        string _sDateRange = "";
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewShipments(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Shipment).ToString() + "," + ((int)EnumModuleName.TAP).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oShipments = new List<Shipment>();
            _oShipments = Shipment.Gets("SELECT * FROM View_Shipment WHERE BUID = " + buid + " AND ISNULL(ApprovedBy,0)=0 ORDER BY ShipmentID ASC", (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Shipment, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;

            _oUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM View_User ORDER BY UserName", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Users = _oUsers;
            ViewBag.CompareOperator = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ShipmentMode = EnumObject.jGets(typeof(EnumShipmentMode));

            return View(_oShipments);
        }

        public ActionResult ViewShipment(int id, int buid)
        {
            _oShipment = new Shipment();
            if (id > 0)
            {
                _oShipment = _oShipment.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oShipment.ShipmentDetails = ShipmentDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oShipment.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Shipment, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            //oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.GUQC, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            ViewBag.ShipmentMode = EnumObject.jGets(typeof(EnumShipmentMode));
            return View(_oShipment);
        }

        public ActionResult ViewShipmentRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            _oShipment = new Shipment();
            _oShipment.BUID = buid;

            #region Gets Stores
            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(buid, EnumModuleName.Shipment, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            #endregion
            ViewBag.Stores = oWorkingUnits;
            
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.BUID = buid;
            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.ShipmentMode = EnumObject.jGets(typeof(EnumShipmentMode));
            _oUsers = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM View_User ORDER BY UserName", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Users = _oUsers;
            return View(_oShipment);
        }

        [HttpPost]
        public JsonResult Save(Shipment oShipment)
        {
            _oShipment = new Shipment();
            try
            {
                //_oShipment = oShipment;
                _oShipment = oShipment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShipment = new Shipment();
                _oShipment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShipment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Shipment oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                Shipment oShipment = new Shipment();
                sFeedBackMessage = oShipment.Delete(oJB.ShipmentID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult Approve(Shipment oShipment)
        {
            _oShipment = new Shipment();
            try
            {
                _oShipment = oShipment;
                _oShipment = oShipment.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShipment = new Shipment();
                _oShipment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShipment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsDetailsByID(ShipmentDetail oShipmentDetail)//Id=ContractorID
        {
            try
            {
                string Ssql = "SELECT*FROM View_ShipmentDetail WHERE ShipmentID=" + oShipmentDetail.ShipmentID + " ";
                _oShipmentDetails = new List<ShipmentDetail>();
                _oShipmentDetail.ShipmentDetails = ShipmentDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oShipmentDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShipmentDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetLot(Shipment oShipment)
        {
            List<Lot>_oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_LotGU AS HH WHERE HH.WorkingUnitID=" + oShipment.StoreID + " AND HH.OrderRecapID IN (SELECT MM.OrderRecapID FROM View_OrderRecap AS MM WHERE MM.BuyerID =" + oShipment.BuyerID + " AND MM.AlreadyShipmentQty<MM.TotalQuantity) AND BUID=" + oShipment.BUID + " AND ISNULL(ISNULL(OrderRecapNo,'')+ISNULL(StyleNo,''),'') LIKE '%" + oShipment.Remarks + "%' ORDER BY HH.OrderRecapID, HH.LotID ASC ";
                _oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLots = new List<Lot>();
            }
            var jsonResult = Json(_oLots, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult GetDriverMobileNo(Shipment oShipment)
        {
            _oShipments = new List<Shipment>();
            string sSQL = "";
            if (!string.IsNullOrEmpty(oShipment.DriverName))
            {
                sSQL += "SELECT TOP 1 * FROM Shipment WHERE DriverName LIKE '" + oShipment.DriverName + "%' ORDER BY ShipmentID DESC ";
            }
            _oShipments = Shipment.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            _oShipment = new Shipment();
            if (_oShipments.Count > 0)
            {
                _oShipment = _oShipments[0];
            }
            else
            {
                _oShipment.DriverMobileNo = "";
            }

            var jsonResult = Json(_oShipment, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region print
        [HttpPost]
        public ActionResult SetShipmentListData(Shipment oShipment)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oShipment);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintShipments()
        {
            _oShipment = new Shipment();
            try
            {
                _oShipment = (Shipment)Session[SessionInfo.ParamObj];
                string sSQL = "SELECT * FROM View_Shipment WHERE ShipmentID IN (" + _oShipment.ErrorMessage + ") Order By ShipmentID";
                _oShipments = Shipment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShipment = new Shipment();
                _oShipments = new List<Shipment>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oShipment.Company = oCompany;

            rptShipments oReport = new rptShipments();
            byte[] abytes = oReport.PrepareReport(_oShipments, oCompany);
            return File(abytes, "application/pdf");
            //return null;
        }

        public ActionResult ShipmentPrintPreview(int id)
        {
            _oShipment = new Shipment();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oShipment = _oShipment.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oShipment.ShipmentDetails = ShipmentDetail.Gets(_oShipment.ShipmentID, (int)Session[SessionInfo.currentUserID]);
                //_oShipment.BusinessUnit = oBusinessUnit.Get(_oShipment.BUID, (int)Session[SessionInfo.currentUserID]);
            }
            //else
            //{
            //    _oShipment.BusinessUnit = new BusinessUnit();
            //}
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            //_oShipment.Company = oCompany;
            byte[] abytes;
            rptShipment oReport = new rptShipment();
            abytes = oReport.PrepareReport(_oShipment, oCompany);
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
        #endregion

        #region AdvanceSearch
        [HttpPost]
        public JsonResult Search(Shipment oShipment)
        {
            _oShipments = new List<Shipment>();
            try
            {
                string sSQL = GetSQLAdvSrc(oShipment.ErrorMessage);
                _oShipments = Shipment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShipment = new Shipment();
                _oShipment.ErrorMessage = ex.Message;
                _oShipments.Add(_oShipment);
            }

            var jSonResult = Json(_oShipments, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }
        private string GetSQLAdvSrc(string sTemp)
        {
            string sChallanNo = sTemp.Split('~')[0];

            int nShipmentDateMenu = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dShipmentDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dShipmentDateTo = Convert.ToDateTime(sTemp.Split('~')[3]);

            string sBuyerIDs = sTemp.Split('~')[4];
            int nStoreID = Convert.ToInt32(sTemp.Split('~')[5]);
            int nShipmentMode = Convert.ToInt32(sTemp.Split('~')[6]);
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[7]);

            string sDrverName = sTemp.Split('~')[8];
            string sOrderRecapNo = sTemp.Split('~')[9];
            string sStyleNo = sTemp.Split('~')[10];

            string sReturn1 = "SELECT * FROM View_Shipment";
            string sReturn = "";

            if (sChallanNo != "undefined" && sChallanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%' ";
            }

            if (sDrverName != "undefined" && sDrverName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DriverName LIKE '%" + sDrverName + "%' ";
            }
            if (sOrderRecapNo != "undefined" && sOrderRecapNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ShipmentID IN(SELECT ShipmentID FROM View_ShipmentDetail WHERE OrderRecapNo LIKE '%" + sOrderRecapNo + "%') ";
            }
            if (sStyleNo != "undefined" && sStyleNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ShipmentID IN(SELECT ShipmentID FROM View_ShipmentDetail WHERE StyleNo LIKE '%" + sStyleNo + "%') ";
            }

            #region Shipment Date Wise

            if (nShipmentDateMenu > 0)
            {
                if (nShipmentDateMenu == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentDateTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ShipmentDate< '" + dShipmentDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentDateTo.AddDays(1).ToString("dd MMM yyyy") + "') ";
                }
            }
            #endregion

            if (sBuyerIDs != "undefined" && sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ") ";
            }

            if (nStoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreID = " + nStoreID + " ";
            }

            if (nShipmentMode > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ShipmentMode = " + nShipmentMode + " ";
            }

            if (nApproveByID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + nApproveByID + " ";
            }

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        #region Autocomplete Gets
        [HttpGet]
        public JsonResult GetsTruckForAutocomplete(string TruckNo)
        {
            List<Shipment> oShipments = new List<Shipment>();
            TruckNo = TruckNo == null ? "" : TruckNo;
            string sSQL = "SELECT * FROM View_Shipment AS HH WHERE HH.TruckNo LIKE '%" + TruckNo + "%' ORDER BY HH.TruckNo ASC";
            oShipments = Shipment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oShipments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetsDriverNameForAutocomplete(string DriverName)
        {
            List<Shipment> oShipments = new List<Shipment>();
            DriverName = DriverName == null ? "" : DriverName;
            string sSQL = "SELECT * FROM View_Shipment AS HH WHERE HH.DriverName LIKE '%" + DriverName + "%' ORDER BY HH.DriverName ASC";
            oShipments = Shipment.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oShipments, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

        #region Shipment Register
        public ActionResult SetSessionSearchCriteria(ShipmentRegister oShipmentRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oShipmentRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ShipmentRegister oShipmentRegister)
        {
            string sTemp = oShipmentRegister.ErrorMessage;
            string sChallanNo = sTemp.Split('~')[0];

            int nShipmentDateMenu = Convert.ToInt32(sTemp.Split('~')[1]);
            DateTime dShipmentDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            DateTime dShipmentDateTo = Convert.ToDateTime(sTemp.Split('~')[3]);
            #region make date range
            if (nShipmentDateMenu == (int)EnumCompareOperator.EqualTo)
            {
                _sDateRange = "Shipment Date: " + dShipmentDate.ToString("dd MMM yyyy");
            }
            else if (nShipmentDateMenu == (int)EnumCompareOperator.Between)
            {
                _sDateRange = "Shipment Date: " + dShipmentDate.ToString("dd MMM yyyy") + " - To - " + dShipmentDateTo.ToString("dd MMM yyyy");
            }
            else if (nShipmentDateMenu == (int)EnumCompareOperator.NotEqualTo)
            {
                _sDateRange = "Shipment Date: Not Equal to " + dShipmentDate.ToString("dd MMM yyyy");
            }
            else if (nShipmentDateMenu == (int)EnumCompareOperator.GreaterThan)
            {
                _sDateRange = "Shipment Date: Greater Than " + dShipmentDate.ToString("dd MMM yyyy");
            }
            else if (nShipmentDateMenu == (int)EnumCompareOperator.SmallerThan)
            {
                _sDateRange = "Shipment Date: Smaller Than " + dShipmentDate.ToString("dd MMM yyyy");
            }
            else if (nShipmentDateMenu == (int)EnumCompareOperator.NotBetween)
            {
                _sDateRange = "Shipment Date: Not Between " + dShipmentDate.ToString("dd MMM yyyy") + " - To - " + dShipmentDateTo.ToString("dd MMM yyyy");
            }
            #endregion

            int nBuyerID = Convert.ToInt32(sTemp.Split('~')[4]);
            int nStoreID = Convert.ToInt32(sTemp.Split('~')[5]);
            int nShipmentMode = Convert.ToInt32(sTemp.Split('~')[6]);
            int nApproveByID = Convert.ToInt32(sTemp.Split('~')[7]);

            string sDrverName = sTemp.Split('~')[8];
            string sOrderRecapNo = sTemp.Split('~')[9];
            string sStyleNo = sTemp.Split('~')[10];

            //int nLayout = Convert.ToInt32(sTemp.Split('~')[11]);

            string sReturn1 = "SELECT * FROM View_ShipmentRegister";
            string sReturn = "";

            #region query make
            if (sChallanNo != "undefined" && sChallanNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%' ";
            }

            if (sDrverName != "undefined" && sDrverName != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DriverName LIKE '%" + sDrverName + "%' ";
            }
            if (sOrderRecapNo != "undefined" && sOrderRecapNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapNo LIKE '%" + sOrderRecapNo + "%' ";
            }
            if (sStyleNo != "undefined" && sStyleNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo LIKE '%" + sStyleNo + "%' ";
            }

            #region Shipment Date Wise

            if (nShipmentDateMenu > 0)
            {
                if (nShipmentDateMenu == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentDateTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateMenu == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " (ShipmentDate< '" + dShipmentDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentDateTo.AddDays(1).ToString("dd MMM yyyy") + "') ";
                }
            }
            #endregion

            if (nBuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID =" + nBuyerID + " ";
            }

            if (nStoreID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreID = " + nStoreID + " ";
            }

            if (nShipmentMode > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ShipmentMode = " + nShipmentMode + " ";
            }

            if (nApproveByID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApprovedBy = " + nApproveByID + " ";
            }
            #endregion

            #region Report Layout
            //if (nLayout == (int)EnumReportLayout.DateWiseDetails)
            //{
            //    sReturn += " ORDER BY  BuyerID,ShipmentDate, StyleNo, OrderRecapID, ShipmentID, ShipmentDetailID ASC";
            //}
            //else if (nLayout == (int)EnumReportLayout.PartyWiseDetails)
            //{
            //    sReturn += " ORDER BY  BuyerName, ShipmentID, ShipmentDetailID ASC";
            //}
            //else
            //{
            //    sReturn += " ORDER BY ShipmentID, ShipmentDetailID ASC";
            //}
            #endregion

            sReturn += " ORDER BY  BuyerID,ShipmentDate, ChallanNo, StyleNo, OrderRecapID, ShipmentID, ShipmentDetailID ASC";
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }

        public ActionResult PrintShipmentRegister(double ts)
        {
            ShipmentRegister oShipmentRegister = new ShipmentRegister();
            string _sErrorMesage = "";
            try
            {
                _oShipmentRegisters = new List<ShipmentRegister>();
                oShipmentRegister = (ShipmentRegister)Session[SessionInfo.ParamObj];
                oShipmentRegister.BUID = Convert.ToInt32(oShipmentRegister.ErrorMessage.Split('~')[11]);
                string sSQL = this.GetSQL(oShipmentRegister);
                _oShipmentRegisters = ShipmentRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                //oShipmentRegister.ReportLayout = (EnumReportLayout)Convert.ToInt32(oShipmentRegister.ErrorMessage.Split('~')[11]);
                if (_oShipmentRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oShipmentRegisters = new List<ShipmentRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                if (oShipmentRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oShipmentRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
                }

                rptShipmentRegisters oReport = new rptShipmentRegisters();
                byte[] abytes = oReport.PrepareReport(_oShipmentRegisters, oCompany, _sDateRange);   //, oShipmentRegister.ReportLayout
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

        public void ExportToExcelShipmentRegister()
        {
            ShipmentRegister oShipmentRegister = new ShipmentRegister();
            string _sErrorMesage = "";
            try
            {
                _oShipmentRegisters = new List<ShipmentRegister>();
                oShipmentRegister = (ShipmentRegister)Session[SessionInfo.ParamObj];
                oShipmentRegister.BUID = Convert.ToInt32(oShipmentRegister.ErrorMessage.Split('~')[11]);
                string sSQL = this.GetSQL(oShipmentRegister);
                _oShipmentRegisters = ShipmentRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oShipmentRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oShipmentRegisters = new List<ShipmentRegister>();
                _sErrorMesage = ex.Message;
            }

            if (_sErrorMesage == "")
            {
                Company _oCompany = new Company();
                _oCompany = _oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oCompany.CompanyLogo = GetCompanyLogo(_oCompany);
                if (oShipmentRegister.BUID > 0)
                {
                    BusinessUnit oBU = new BusinessUnit();
                    oBU = oBU.Get(oShipmentRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                    _oCompany = GlobalObject.BUTOCompany(_oCompany, oBU);
                }

                //if (oShipmentRegister.ReportLayout == EnumReportLayout.DateWiseDetails)
                #region excel
                #region full excel
                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Party Wise Shipment Register(Details)");
                    sheet.Name = "Party Wise Shipment Register(Details)";
                    sheet.Column(2).Width = 15; //shipment date
                    sheet.Column(3).Width = 22; //style
                    sheet.Column(4).Width = 22; //order no
                    sheet.Column(5).Width = 8; //country
                    sheet.Column(6).Width = 15; //shipment qty
                    sheet.Column(7).Width = 10; //CTN qty

                    sheet.Column(8).Width = 10; //Total CTN
                    sheet.Column(9).Width = 10; //Challan no
                    
                    sheet.Column(10).Width = 15; //shipment mode
                    sheet.Column(11).Width = 20; //truck 
                    sheet.Column(12).Width = 20; //driver
                    sheet.Column(13).Width = 18; //mobile no
                    sheet.Column(14).Width = 20; //depo
                    sheet.Column(15).Width = 20; //escord
                    sheet.Column(16).Width = 20; //factory
                    sheet.Column(17).Width = 20; //security
                    sheet.Column(18).Width = 15; //empty CTN qty
                    sheet.Column(19).Width = 15; //GUM tape qty

                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 17].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = _oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 17].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Party Wise Shipment Register(Details)"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 17].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = _sDateRange; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 11; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Shipment Date"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Order NO"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Country"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Shipment Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "CTN Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Total CTN"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Challan No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Shipment Mode"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Truck"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Driver"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Mobile No"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Depo"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Escord"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "Factory"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Security Lock"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Empty CTN Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Gum Tape Qty"; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                    rowIndex = rowIndex + 1;
                    #endregion

                    #region group by
                    if (_oShipmentRegisters.Count > 0)
                    {
                        var data = _oShipmentRegisters.GroupBy(x => new { x.BuyerName }, (key, grp) => new  //, x.QCNo, x.QCByName, x.StoreName, x.BuyerName
                        {
                            BuyerName = key.BuyerName,
                            Results = grp.ToList() //All data
                        });
                    #endregion

                        #region Report Data
                        
                        foreach (var oData in data)
                        {
                            cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = "Party : @ " + oData.BuyerName; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            rowIndex = rowIndex + 1;

                            //count = 0; num = 0;
                            string SDate = "", styleNo = "", recapNo = "", challanNo = ""; int rowCount;
                            int TotalCTN = 0;
                            foreach (var oItem in oData.Results)
                            {
                                //count++;
                                if (SDate != oItem.ShipmentDateInString)
                                {
                                    rowCount = oData.Results.Count(x => x.ShipmentDateInString == oItem.ShipmentDateInString);
                                    cell = sheet.Cells[rowIndex, 2, rowIndex + (rowCount-1), 2]; cell.Merge = true; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }

                                if (styleNo != oItem.StyleNo)
                                {
                                    rowCount = oData.Results.Count(x => x.StyleNo == oItem.StyleNo);
                                    cell = sheet.Cells[rowIndex, 3, rowIndex + (rowCount - 1), 3]; cell.Merge = true; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }

                                if (recapNo != oItem.OrderRecapNo)
                                {
                                    rowCount = oData.Results.Count(x => x.OrderRecapNo == oItem.OrderRecapNo);
                                    cell = sheet.Cells[rowIndex, 4, rowIndex + (rowCount - 1), 4]; cell.Merge = true; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }

                                cell = sheet.Cells[rowIndex, 5]; cell.Merge = true; cell.Value = oItem.CountryShortName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 6]; cell.Merge = true; cell.Value = oItem.ShipmentQty.ToString("#,###.##;(#,###.##)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 7]; cell.Merge = true; cell.Value = oItem.CTNQty.ToString("#,###.##;(#,###.##)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                if (challanNo != oItem.ChallanNo)
                                {
                                    rowCount = oData.Results.Count(x => x.ChallanNo == oItem.ChallanNo);
                                    TotalCTN = oData.Results.Where(x => x.ChallanNo == oItem.ChallanNo).Sum(c => c.CTNQty);

                                    cell = sheet.Cells[rowIndex, 8, rowIndex + (rowCount - 1), 8]; cell.Merge = true; cell.Value = TotalCTN; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                    cell = sheet.Cells[rowIndex, 9, rowIndex + (rowCount - 1), 9]; cell.Merge = true; cell.Value = oItem.ChallanNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                }

                                cell = sheet.Cells[rowIndex, 10]; cell.Merge = true; cell.Value = oItem.ShipmentModeInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 11]; cell.Merge = true; cell.Value = oItem.TruckNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 12]; cell.Merge = true; cell.Value = oItem.DriverName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 13]; cell.Merge = true; cell.Value = oItem.DriverMobileNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 14]; cell.Merge = true; cell.Value = oItem.Depo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 15]; cell.Merge = true; cell.Value = oItem.Escord; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 16]; cell.Merge = true; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 17]; cell.Merge = true; cell.Value = oItem.SecurityLock; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 18]; cell.Merge = true; cell.Value = oItem.EmptyCTNQty.ToString("#,###.##;(#,###.##)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                                cell = sheet.Cells[rowIndex, 19]; cell.Merge = true; cell.Value = oItem.GumTapeQty.ToString("#,###.##;(#,###.##)"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                                border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                
                                rowIndex++;
                                SDate = oItem.ShipmentDateInString;
                                styleNo = oItem.StyleNo;
                                recapNo = oItem.OrderRecapNo;
                                challanNo = oItem.ChallanNo;
                            }
                            
                            //cell = sheet.Cells[rowIndex, 2, rowIndex, 11]; cell.Merge = true; cell.Value = ""; cell.Style.Font.Bold = true; cell.Style.WrapText = true;
                            //border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            //cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            //rowIndex = rowIndex + 1;
                        }

                        #endregion

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Party_Wise_Shipment_Register.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }

                }
                #endregion
                #endregion
                
            }
        }
        #endregion

    }

}
