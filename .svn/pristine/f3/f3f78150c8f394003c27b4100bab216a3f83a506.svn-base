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
using ESimSol.Reports;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace ESimSolFinancial.Controllers
{
    public class ShipmentScheduleController : Controller
    {

        #region Declaration
        ShipmentSchedule _oShipmentSchedule = new ShipmentSchedule();
        List<ShipmentSchedule> _oShipmentSchedules = new List<ShipmentSchedule>();
        ShipmentScheduleDetail _oShipmentScheduleDetail = new ShipmentScheduleDetail();
        List<ShipmentScheduleDetail> _oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
        OrderRecap _oOrderRecap = new OrderRecap();
        #endregion

        #region Function
        #region Make Color Size ration from |Detial
        private List<ColorSizeRatio> MapColorSizeRationFromShipmentScheduleDetail(List<ShipmentScheduleDetail> oShipmentScheduleDetails, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
            {
                if (oItem.ColorID != nColorID)
                {
                    oColorSizeRatio = new ColorSizeRatio();
                    oColorSizeRatio.ColorID = oItem.ColorID;
                    oColorSizeRatio.ColorName = oItem.ColorName;
                    nCount = 0;
                    foreach (TechnicalSheetSize oSize in oSizes)
                    {
                        nCount++;
                        #region Set OrderQty
                        sPropertyName = "OrderQty" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oShipmentScheduleDetails), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oShipmentScheduleDetails), null);
                        }
                        #endregion
                    }

                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oShipmentScheduleDetails), null);
                    }
                    #endregion

                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }
        private double GetColorWiseTotalQty(int nColorID, List<ShipmentScheduleDetail> oShipmentScheduleDetails)
        {
            double nTotalQty = 0;
            foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Qty;
                }
            }
            return nTotalQty;
        }
        private double GetQty(int nSizeID, int nColorID, List<ShipmentScheduleDetail> oShipmentScheduleDetails)
        {
            foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Qty;
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<ShipmentScheduleDetail> oShipmentScheduleDetails)
        {
            foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.ShipmentScheduleDetailID;
                }
            }
            return 0;
        }
        #endregion
        #region Make POD from Color size Ratio
        private List<ShipmentScheduleDetail> MapShipmentScheduleDetailFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, ShipmentSchedule oShipmentSchedule)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
            ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
            ShipmentScheduleDetail oTempShipmentScheduleDetail = new ShipmentScheduleDetail();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempShipmentScheduleDetail = new ShipmentScheduleDetail();
                    oTempShipmentScheduleDetail = GetObjIDAndQty(nCount, oItem);
                    if (oTempShipmentScheduleDetail.Qty > 0)
                    {
                        oShipmentScheduleDetail = new ShipmentScheduleDetail();
                        oShipmentScheduleDetail.ShipmentScheduleDetailID = oTempShipmentScheduleDetail.ShipmentScheduleDetailID;
                        oShipmentScheduleDetail.ShipmentScheduleID = oShipmentSchedule.ShipmentScheduleID;
                        oShipmentScheduleDetail.ColorID = oItem.ColorID;
                        oShipmentScheduleDetail.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oShipmentScheduleDetail.UnitID = oShipmentSchedule.MeasurementUnitID;
                        oShipmentScheduleDetail.Qty = oTempShipmentScheduleDetail.Qty;
                        oShipmentScheduleDetails.Add(oShipmentScheduleDetail);
                    }
                }
            }
            return oShipmentScheduleDetails;
        }
        private ShipmentScheduleDetail GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
            switch (nCount)
            {
                case 1:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID1;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID2;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID3;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID4;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID5;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID6;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID7;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID8;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID9;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID10;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID11;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID12;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID13;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID14;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID15;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID16;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID17;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID18;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID19;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = oColorSizeRatio.OrderObjectID20;
                    oShipmentScheduleDetail.Qty = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oShipmentScheduleDetail;
        }

        #endregion

        private List<ShipmentScheduleDetail> GetShipmentScheduleDetails(List<OrderRecapDetail> oOrderRecapDetails)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
            ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();

            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.YetToPoductionQty > 0)
                {
                    oShipmentScheduleDetail = new ShipmentScheduleDetail();
                    oShipmentScheduleDetail.ShipmentScheduleDetailID = 0;
                    oShipmentScheduleDetail.ShipmentScheduleID = 0;
                    oShipmentScheduleDetail.ColorID = oItem.ColorID;
                    oShipmentScheduleDetail.SizeID = oItem.SizeID;
                    oShipmentScheduleDetail.UnitID = oItem.MeasurementUnitID;
                    oShipmentScheduleDetail.Qty = oItem.YetToScheduleQty;
                    oShipmentScheduleDetail.ColorName = oItem.ColorName;
                    oShipmentScheduleDetail.UnitName = oItem.UnitName;
                    oShipmentScheduleDetail.SizeName = oItem.SizeName;
                    oShipmentScheduleDetail.Symbol = "";
                    oShipmentScheduleDetail.OrderQty = oItem.Quantity;
                    oShipmentScheduleDetails.Add(oShipmentScheduleDetail);
                }
            }


            return oShipmentScheduleDetails;
        }
        #endregion

        #region Views
        public ActionResult ViewShipmentSchedules(int id)//order Recap Id
        {
            _oShipmentSchedule = new ShipmentSchedule();
            _oShipmentSchedules = ShipmentSchedule.Gets(id, (int)Session[SessionInfo.currentUserID]);
            return View(_oShipmentSchedules);
        }

        public ActionResult ViewShipmentSchedule(int id, int ORID)//Shipment Schedule Id
        {
            _oShipmentSchedule = new ShipmentSchedule();
            _oShipmentScheduleDetail = new ShipmentScheduleDetail();
            _oOrderRecap = new OrderRecap();
            if (id > 0)
            {
                _oShipmentSchedule = _oShipmentSchedule.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oShipmentScheduleDetails = ShipmentScheduleDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oShipmentSchedule.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oShipmentSchedule.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            }
            else
            {
                _oOrderRecap = _oOrderRecap.Get(ORID, (int)Session[SessionInfo.currentUserID]);
                _oShipmentSchedule.OrderRecapNo = _oOrderRecap.OrderRecapNo;
                _oShipmentSchedule.OrderRecapID = _oOrderRecap.OrderRecapID;
                _oShipmentSchedule.TechnicalSheetID = _oOrderRecap.TechnicalSheetID;
                _oShipmentSchedule.YetToScheduleQty = _oOrderRecap.YetToScheduleQty;
                _oShipmentSchedule.OrderQty = _oOrderRecap.TotalQuantity;
                _oShipmentSchedule.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
                oOrderRecapDetails = OrderRecapDetail.Gets(_oOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                _oShipmentScheduleDetails = GetShipmentScheduleDetails(oOrderRecapDetails);
            }
            _oShipmentSchedule.ColorSizeRatios = MapColorSizeRationFromShipmentScheduleDetail(_oShipmentScheduleDetails, _oShipmentSchedule.TechnicalSheetSizes);
            ViewBag.CountryList = Country.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.CutOffTypes = EnumObject.jGets(typeof(EnumCutOffType));
            ViewBag.ShipmentModes = EnumObject.jGets(typeof(EnumShipmentBy));
            return View(_oShipmentSchedule);
        }
        #endregion

        #region HTTP
        [HttpPost]
        public JsonResult Save(ShipmentSchedule oShipmentSchedule)
        {
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oShipmentSchedule.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oShipmentSchedule.ShipmentScheduleDetails = MapShipmentScheduleDetailFromColorSizeRation(oShipmentSchedule.ColorSizeRatios, oTechnicalSheetSizes, oShipmentSchedule);
                oShipmentSchedule = oShipmentSchedule.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShipmentSchedule = new ShipmentSchedule();
                _oShipmentSchedule.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oShipmentSchedule);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                _oShipmentSchedule = new ShipmentSchedule();
                sErrorMease = _oShipmentSchedule.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region OrderRecap Register
        public ActionResult ViewOrderRecapRegisters(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            OrderRecap oOrderRecap = new OrderRecap();

            #region Approval User
            string sSQL = "SELECT * FROM View_User AS HH WHERE HH.UserID IN (SELECT DISTINCT MM.AcceptedBy FROM ImportInvoice AS MM WHERE ISNULL(MM.AcceptedBy,0)!=0) ORDER BY HH.UserName";
            List<User> oApprovalUsers = new List<ESimSol.BusinessObjects.User>();
            ESimSol.BusinessObjects.User oApprovalUser = new ESimSol.BusinessObjects.User();
            oApprovalUser.UserID = 0; oApprovalUser.UserName = "--Select Approval User--";
            oApprovalUsers.Add(oApprovalUser);
            oApprovalUsers.AddRange(ESimSol.BusinessObjects.User.GetsBySql(sSQL, (int)Session[SessionInfo.currentUserID]));
            #endregion

            #region Report Layout
            List<EnumObject> oReportLayouts = new List<EnumObject>();
            List<EnumObject> oTempReportLayouts = new List<EnumObject>();
            oTempReportLayouts = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportLayouts)
            {
                if ((EnumReportLayout)oItem.id == EnumReportLayout.InvoiceWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWise || (EnumReportLayout)oItem.id == EnumReportLayout.DateWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWise || (EnumReportLayout)oItem.id == EnumReportLayout.PartyWiseDetails || (EnumReportLayout)oItem.id == EnumReportLayout.ProductWise)
                {
                    oReportLayouts.Add(oItem);
                }
            }
            #endregion

            ViewBag.BUID = buid;
            ViewBag.ApprovalUsers = oApprovalUsers;
            ViewBag.ReportLayouts = oReportLayouts;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.ImportInvoiceStates = EnumObject.jGets(typeof(EnumInvoiceEvent));
            ViewBag.ImportInvoiceTypes = EnumObject.jGets(typeof(EnumImportPIType));
            return View(oOrderRecap);
        }
        public ActionResult PintOrderRecapRegisters(int nORID, double tsv) //OrderRecapID: nORID
        {
            _oOrderRecap = new OrderRecap();
            Company _oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            List<ShipmentScheduleDetail> oShipmentScheduleDetailDetailList = new List<ShipmentScheduleDetail>();
            List<ShipmentSchedule> oShipmentSchedules = new List<ShipmentSchedule>();

            try
            {
                _oOrderRecap = _oOrderRecap.Get(nORID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.Company = _oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.Company.CompanyLogo = GetCompanyLogo(_oOrderRecap.Company);
                _oOrderRecap.OrderRecapDetails = OrderRecapDetail.Gets(nORID, (int)Session[SessionInfo.currentUserID]);
                
                string sSQL = "SELECT * FROM View_ShipmentScheduleDetail WHERE ShipmentScheduleID IN (SELECT ShipmentScheduleID FROM ShipmentSchedule WHERE OrderRecapID =" + nORID + ")";
                oShipmentScheduleDetailDetailList = ShipmentScheduleDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

                oBusinessUnit = oBusinessUnit.Get(_oOrderRecap.BUID, (int)Session[SessionInfo.currentUserID]);
                sSQL = "SELECT * FROM View_ShipmentSchedule WHERE OrderRecapID=" + nORID
                    //"SELECT   ShipmentDate,CountryID,CutOffType,CutOffDate,CutOffWeek,ShipmentMode,CountryName,CountryShortName FROM View_ShipmentSchedule WHERE OrderRecapID="+nORID
                    //+" GROUP BY ShipmentDate,CountryID,CutOffType,CutOffDate,CutOffWeek,ShipmentMode,CountryName,CountryShortName"
                    + " ORDER BY ShipmentDate,CutOffType,CutOffDate";
                oShipmentSchedules = ShipmentSchedule.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception e) 
            {
                _oOrderRecap=new OrderRecap();
                _oOrderRecap.ErrorMessage = e.Message;
            }

            if (_oOrderRecap.ErrorMessage == "")
            {
                rptOrderRecapRegister oReport = new rptOrderRecapRegister();
                string sHeader = "Buyer: " + _oOrderRecap.BuyerName + "      Style: " + _oOrderRecap.StyleNo + "     Order No: " + _oOrderRecap.OrderRecapNo;
                byte[] abytes = oReport.PrepareReport(_oOrderRecap, oShipmentScheduleDetailDetailList, oShipmentSchedules, oBusinessUnit, sHeader);
                return File(abytes, "application/pdf");
            }
            else
            {
                string sMessage = "There is no Data for Selected Date ";
                return RedirectToAction("MessageHelper", "User", new { message = _oOrderRecap.ErrorMessage });//sMessage
            }

        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region Http functions

        [HttpGet]
        public JsonResult GetShipmentSchedules(int id, double ts)
        {
            _oShipmentSchedules = new List<ShipmentSchedule>();
            try
            {
                if (id > 0)
                {

                    _oShipmentSchedules = ShipmentSchedule.Gets(id, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oShipmentSchedules = new List<ShipmentSchedule>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShipmentSchedules);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}