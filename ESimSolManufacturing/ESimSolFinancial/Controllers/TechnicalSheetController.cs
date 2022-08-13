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

namespace ESimSolFinancial.Controllers
{
    public class TechnicalSheetController : Controller
    {
        #region Declaration
        TechnicalSheet _oTechnicalSheet = new TechnicalSheet();       
        List<TechnicalSheet> _oTechnicalSheets = new List<TechnicalSheet>();
        MeasurementSpec _oMeasurementSpec = new MeasurementSpec();
        MeasurementSpecDetail _oMeasurementSpecDetail = new MeasurementSpecDetail();
        List<MeasurementSpecDetail> _oMeasurementSpecDetails = new List<MeasurementSpecDetail>();
        List<TempMeasurementSpecDetail> _oTempMeasurementSpecDetails = new List<TempMeasurementSpecDetail>();
        TechnicalSheetImage _oTechnicalSheetImage = new TechnicalSheetImage();
        List<TechnicalSheetImage> _oTechnicalSheetImages = new List<TechnicalSheetImage>();
        TechnicalSheetThumbnail _oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
        List<TechnicalSheetThumbnail> _oTechnicalSheetThumbnails = new List<TechnicalSheetThumbnail>();        
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();       
        List<TechnicalSheetSize> _oTechnicalSheetSizes = new List<TechnicalSheetSize>();
        private byte[] aImageInByteArray;
        #endregion


        public ActionResult ViewTechnicalSheets(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.TechnicalSheet).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheets = new List<TechnicalSheet>();
            if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                _oTechnicalSheets = TechnicalSheet.BUWiseGets(buid, ((int)EnumDevelopmentStatus.RequestForApproved).ToString(), (int)Session[SessionInfo.currentUserID]);
                ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM Users WHERE UserID IN(SELECT DBUserID FROM TechnicalSheet WHERE BUID = "+buid+")", (int)Session[SessionInfo.currentUserID]);
            }
            else
            {

                _oTechnicalSheets = TechnicalSheet.Gets("SELECT * FROM View_TechnicalSheet WHERE  DevelopmentStatus IN ( "+((int)EnumDevelopmentStatus.RequestForApproved).ToString()+ ")", (int)Session[SessionInfo.currentUserID]);
                ViewBag.Users = ESimSol.BusinessObjects.User.GetsBySql("SELECT * FROM Users WHERE UserID IN(SELECT DBUserID FROM TechnicalSheet)", (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            //ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            ViewBag.Buyers = Contractor.GetsByNamenType("",((int)EnumContractorType.Buyer).ToString(),buid,(int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            
            return View(_oTechnicalSheets);            
        }       

        #region REPORTS
        public ActionResult PrintTechnicalSheets(string sParam)
        {
             _oTechnicalSheet = new TechnicalSheet();

            Company oCompany = new Company();
            string sSql = "SELECT * FROM View_TechnicalSheet WHERE TechnicalSheetID IN (" + sParam + ")";

            _oTechnicalSheet.TechnicalSheetList = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTechnicalSheet.Company = oCompany;  

            if (_oTechnicalSheet.TechnicalSheetList.Count > 0)
            {

                rptTechnicalSheets oReport = new rptTechnicalSheets();
                byte[] abytes = oReport.PrepareReport(_oTechnicalSheet);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }
        public ActionResult PrintTechnicalSheet(int id)
        {
            Company oCompany = new Company();
            _oTechnicalSheet = new TechnicalSheet();
            _oMeasurementSpec = new MeasurementSpec();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oTechnicalSheetImage = new TechnicalSheetImage();
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = new List<MeasurementSpecAttachment>();

            _oTechnicalSheet = _oTechnicalSheet.Get(id,(int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.TechnicalSheetImage = _oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.TechnicalSheetThumbnailForMeasurmentSpec = _oTechnicalSheetThumbnail.GetMeasurementSpecImage(id, (int)Session[SessionInfo.currentUserID]);            
            _oTechnicalSheet.TechnicalSheetSizes = TechnicalSheetSize.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.TechnicalSheetColors= TechnicalSheetColor.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.MeasurementSpecDetails = MeasurementSpecDetail.GetsByTechnicalSheet(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.TempMeasurementSpecDetails = MapTempMeasurementSpecDetail(_oTechnicalSheet.TechnicalSheetSizes, _oTechnicalSheet.MeasurementSpecDetails);
            _oTechnicalSheet.MeasurementSpec = _oMeasurementSpec.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.MeasurementSpecAttachments = MeasurementSpecAttachment.Gets(id, true, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.TechnicalSheetShipments = TechnicalSheetShipment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            oBillOfMaterials = BillOfMaterial.GetsWithImage(id, (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.BusinessUnit = oBusinessUnit.Get(_oTechnicalSheet.BUID, (int)Session[SessionInfo.currentUserID]);
            foreach (BillOfMaterial oItem in oBillOfMaterials)
            {
                oItem.AttachImage = GetImage(oItem);
            }
            _oTechnicalSheet.BillOfMaterials = oBillOfMaterials;
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oTechnicalSheet.Company = oCompany;

            rptTechnicalSheet oReport = new rptTechnicalSheet();
            byte[] abytes = oReport.PrepareReport(_oTechnicalSheet);
            return File(abytes, "application/pdf");

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
        public Image GetImage(BillOfMaterial oBillOfMaterial)
        {
            if (oBillOfMaterial.AttachFile!= null)
            {
                MemoryStream m = new MemoryStream(oBillOfMaterial.AttachFile);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }  
        #endregion

        #region  All Search
        [HttpPost]
        public JsonResult Gets(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
           
                string sSQL = "SELECT * FROM View_TechnicalSheet";
                string stempsql = "";
                int nbuyerid = Convert.ToInt32(oTechnicalSheet.Note.Split('~')[0]);
                string sStyleNo = oTechnicalSheet.Note.Split('~')[1];
                int nBusinessSessionID = Convert.ToInt32(oTechnicalSheet.Note.Split('~')[2]);
                int nMerchendiserID = Convert.ToInt32(oTechnicalSheet.Note.Split('~')[3]);
                if (nbuyerid > 0)
                {
                    Global.TagSQL(ref stempsql);
                    stempsql = stempsql + " BuyerID =" + nbuyerid.ToString();
                }
                if (sStyleNo != "")
                {
                    if (sStyleNo != "Style No")
                    {
                        Global.TagSQL(ref stempsql);
                        stempsql = stempsql + " StyleNo ='" + sStyleNo + "'";
                    }
                }
                if (nBusinessSessionID >0)
                {
                    Global.TagSQL(ref stempsql);
                    stempsql = stempsql + " BusinessSessionID =" + nBusinessSessionID.ToString();
                }

                if (nMerchendiserID > 0)
                {
                    Global.TagSQL(ref stempsql);
                    stempsql = stempsql + " MerchandiserID =" + nMerchendiserID;
                }
                if (oTechnicalSheet.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    Global.TagSQL(ref stempsql);
                    stempsql += " BUID = " + oTechnicalSheet.BUID;
                }
                sSQL = sSQL + stempsql;
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsForHidden(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            string sSQL = "SELECT * FROM View_TechnicalSheet";
            string stempsql = "";
            string sbuyerids = oTechnicalSheet.Note.Split('~')[0];            
            int nBusinessSessionID = Convert.ToInt32(oTechnicalSheet.Note.Split('~')[1]);
            string sBusinessUnitIDs = oTechnicalSheet.Note.Split('~')[2];            
            if (sbuyerids !="0")
            {
                Global.TagSQL(ref stempsql);
                stempsql = stempsql + " BuyerID IN(" + sbuyerids+")";
            }
            if (nBusinessSessionID > 0)
            {
                Global.TagSQL(ref stempsql);
                stempsql = stempsql + " BusinessSessionID =" + nBusinessSessionID.ToString();
            }
            if (!string.IsNullOrEmpty(sBusinessUnitIDs))
            {
                Global.TagSQL(ref stempsql);
                stempsql = stempsql + " BUID IN(" + sBusinessUnitIDs + ")";
            }
            sSQL = sSQL + stempsql;
            _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult AdvanceSearch(string Temp)
        {
            List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
            try
            {
                string sSQL = GetSQL(Temp);
                oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oTechnicalSheets = new List<TechnicalSheet>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dtTSFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dtTSTo = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sStyleDescription = sTemp.Split('~')[3];
            int nPreparedBy = Convert.ToInt32(sTemp.Split('~')[4]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[5]);

            int nSessionID = Convert.ToInt32(sTemp.Split('~')[6]);
            string sDepartmentIDs = sTemp.Split('~')[7];
            string sMerchandiserIDs = sTemp.Split('~')[8];
            string sBuyerIDs = sTemp.Split('~')[9];

            string sReturn1 = "SELECT * FROM View_TechnicalSheet";
            string sReturn = "";

            #region StyleDescription
            if (!string.IsNullOrEmpty(sStyleDescription))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleDescription LIKE '%" + sStyleDescription + "%'";
            }
            #endregion
       
            #region BU
            if (nBUID != 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }
            #endregion

            #region SEssion
            if (nSessionID != 0 )//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID;
            }
            #endregion

            #region MerchandiserID
            if (!string.IsNullOrEmpty(sMerchandiserIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN (" + sMerchandiserIDs + ")";
            }
            #endregion

            #region Buyer
            if (!string.IsNullOrEmpty(sBuyerIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion
            #region DepartmentID
            if (!string.IsNullOrEmpty(sDepartmentIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Dept IN (" + sDepartmentIDs + ")";
            }
            #endregion

            #region nPreparedBy
            if (nPreparedBy != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DBUserID = " + nPreparedBy;
            }
            #endregion

            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime = '" + dtTSFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime != '" + dtTSFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime > '" + dtTSFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime < '" + dtTSFrom.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime>= '" + dtTSFrom.ToString("dd MMM yyyy") + "' AND DBServerDateTime < '" + dtTSTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " DBServerDateTime< '" + dtTSFrom.ToString("dd MMM yyyy") + "' OR DBServerDateTime > '" + dtTSTo.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            sReturn = sReturn1 + sReturn + " ORDER BY  TechnicalSheetID";
            return sReturn;
        }

        #endregion

        #region Wait for Approval
        [HttpPost]
        public JsonResult WaitForApproval(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sSQL = "SELECT * FROM View_TechnicalSheet";
            string sTempSql = ""; ;
            Global.TagSQL(ref sTempSql);
            sTempSql +=  " DevelopmentStatus = " + (int)EnumDevelopmentStatus.RequestForApproved + " AND TechnicalSheetID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE OperationType = " + (int)EnumApprovalRequestOperationType.TechnicalSheet + " AND RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            if (oTechnicalSheet.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sTempSql);
                sTempSql += " BUID = " + oTechnicalSheet.BUID;
            }
            sSQL = sSQL + sTempSql;
            _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion 

        #region ViewTechnicalSheet
        public ActionResult ViewTechnicalSheet(int id, double ts)
        {
            _oTechnicalSheet = new TechnicalSheet();
            _oMeasurementSpec = new MeasurementSpec();            
            Contractor oContractor = new Contractor();
            oContractor.Name = "-- Select a Buyer --";
            oContractor.ContractorType = EnumContractorType.Buyer;
            if (id > 0)
            {
                _oTechnicalSheet = _oTechnicalSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (_oTechnicalSheet.MeasurementSpecID > 0)
                {
                    _oTechnicalSheet.MeasurementSpec = _oMeasurementSpec.Get(_oTechnicalSheet.MeasurementSpecID, (int)Session[SessionInfo.currentUserID]);
                }
                _oTechnicalSheet.MeasurementSpecDetails = MeasurementSpecDetail.GetsByTechnicalSheet(id, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.BillOfMaterials = BillOfMaterial.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.OrderRecapYarns = OrderRecapYarn.Gets(id, (int)EnumRecapRefType.TechnicalSheet, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.TechnicalSheetSizes = TechnicalSheetSize.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.TechnicalSheetColors = TechnicalSheetColor.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.TempMeasurementSpecDetails = MapTempMeasurementSpecDetail(_oTechnicalSheet.TechnicalSheetSizes, _oTechnicalSheet.MeasurementSpecDetails);
                _oTechnicalSheet.TechnicalSheetImages = TechnicalSheetImage.Gets(_oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.ImageComments = ImageComment.Gets(_oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.BuyerWiseBrands = BuyerWiseBrand.GetsByBuyer(_oTechnicalSheet.BuyerID, (int)Session[SessionInfo.currentUserID]);
                _oTechnicalSheet.TechnicalSheetShipments = TechnicalSheetShipment.Gets(_oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]); 
            }

            _oTechnicalSheet.Buyers = new List<Contractor>();
            _oTechnicalSheet.Buyers.Add(oContractor);
            //_oTechnicalSheet.Buyers.AddRange(Contractor.GetsByNamenType("", ((int)EnumContractorType.Buyer).ToString(), 0, (int)Session[SessionInfo.currentUserID]));
            //_oTechnicalSheet.BuyerConcerns = BuyerConcern.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.GarmentsClasss = GarmentsClass.GetsGarmentsClass((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.GarmentsSubClasss = GarmentsClass.GetsGarmentsSubClass((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.SampleSizes = SizeCategory.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.GarmentsTypes = GarmentsType.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.MeasurementUnits = MeasurementUnit.Gets(EnumUniteType.Length,  (int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.SizeClasss = GarmentsClass.GetsGarmentsClass((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.BusinessSessions = BusinessSession.Gets(true,(int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.MaterialTypes = MaterialType.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.DevelopmentStatusInInt = (int)_oTechnicalSheet.DevelopmentStatus;
            _oTechnicalSheet.StyleDepartments = StyleDepartment.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.KnittingPatternList = Knitting.Gets((int)Session[SessionInfo.currentUserID]);
            _oTechnicalSheet.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SubGenders = EnumObject.jGets(typeof(EnumSubGender));
            ViewBag.FabricTypes = EnumObject.jGets(typeof(EnumRecapYarnType));
            return View(_oTechnicalSheet);
        }
        #endregion
        
        #region View Technical Sheet Approval Request
        public ActionResult ViewTechnicalSheetApprovalRequest()
        {
            _oApprovalRequest = new ApprovalRequest();
            string sSql = "SELECT * FROM View_User WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE EmployeeDesignationType = " +(int)EnumEmployeeDesignationType.Management + ")";
            _oApprovalRequest.UserList = ESimSol.BusinessObjects.User.GetsBySql(sSql, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oApprovalRequest);
        }

        #endregion
        
        #region Get TechnicalSheet Sizes

        [HttpGet]
        public JsonResult GetTechnicalSheetSizes(int id)
        {
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            string sSql = "SELECT * FROM MeasurementUnit ORDER BY UnitType ASC";// +(int)EnumUniteType.Count;

            try
            {

                oTechnicalSheet.GarmentsClasss = GarmentsClass.GetsGarmentsClass((int)Session[SessionInfo.currentUserID]);
                oTechnicalSheet.GarmentsTypes = GarmentsType.Gets((int)Session[SessionInfo.currentUserID]);
                oTechnicalSheet.MeasurementUnits = MeasurementUnit.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                oTechnicalSheet.TechnicalSheetSizes = TechnicalSheetSize.Gets(id, (int)Session[SessionInfo.currentUserID]); 
            }
            catch (Exception ex)
            {
                TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();
                oTechnicalSheetSize.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get TechnicalSheet Sizes

        [HttpPost]
        public JsonResult GetTechnicalSheetColors(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            try
            {
                _oTechnicalSheet.TechnicalSheetColors = TechnicalSheetColor.Gets(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion
        public Image GetImg(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            //TechnicalSheetThumbnail oTechnicalSheetImage = new TechnicalSheetThumbnail();
            oTechnicalSheetImage = oTechnicalSheetImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;

                //return File(oTechnicalSheetImage.LargeImage, "image/jpg");
            }
            else
            {
                return null;
            }

        }

        public ActionResult GetViewImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            //TechnicalSheetThumbnail oTechnicalSheetImage = new TechnicalSheetThumbnail();
            oTechnicalSheetImage = oTechnicalSheetImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            return PartialView(oTechnicalSheetImage);
        }
        [HttpGet]
        public JsonResult GetMeasermentSpecDetails(int id, double ts)//id is TechnicalSheet ID
        {
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            if (id > 0)
            {
                _oMeasurementSpecDetails = MeasurementSpecDetail.GetsByTechnicalSheet(id, (int)Session[SessionInfo.currentUserID]);
                oTechnicalSheet.TechnicalSheetSizes = TechnicalSheetSize.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oTechnicalSheet.TempMeasurementSpecDetails = MapTempMeasurementSpecDetail(oTechnicalSheet.TechnicalSheetSizes, _oMeasurementSpecDetails);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBuyerConcerns(int nBuyerID, double ts)
        {
            List<BuyerConcern> oBuyerConcerns = new List<BuyerConcern>();
            oBuyerConcerns = BuyerConcern.GetsByContractor(nBuyerID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBuyerConcerns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetContactPersonnel(int nBuyerID)
        {
            List<ContactPersonnel> oContactPersonnels = new List<ContactPersonnel>();
            oContactPersonnels = ContactPersonnel.GetsByContractor(nBuyerID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContactPersonnels);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public Image GetThumImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage!= null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        [HttpGet]
        public JsonResult DeleteTechnicalSheetImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetImage = oTechnicalSheetImage.Get(id, (int)Session[SessionInfo.currentUserID]);
            string sErrorMease = "";
            int nId = 0;
            nId = oTechnicalSheetImage.TechnicalSheetID;
            try
            {
               oTechnicalSheetImage.Delete(id, (int)Session[SessionInfo.currentUserID]);
               oTechnicalSheetThumbnail.Delete(id, (int)Session[SessionInfo.currentUserID]);
               sErrorMease = "Delete Successfully";
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            try
            {
                _oTechnicalSheet = oTechnicalSheet;
                _oTechnicalSheet.DevelopmentStatus = (EnumDevelopmentStatus)oTechnicalSheet.DevelopmentStatusInInt;
                if(_oTechnicalSheet.TSTypeInInt == (int)EnumTSType.Woven)//for Woven
                {
                    _oTechnicalSheet.GarmentsClassID = 0;
                    _oTechnicalSheet.GarmentsSubClassID = 0;
                }
                _oTechnicalSheet = _oTechnicalSheet.Save((int)Session[SessionInfo.currentUserID]);
                if(oTechnicalSheet.OldTSID>0)//for copy paste
                {
                    _oTechnicalSheet.BillOfMaterials = BillOfMaterial.PasteBillOfMaterials(oTechnicalSheet.OldTSID, _oTechnicalSheet.TechnicalSheetID,  (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveImageCommets(TechnicalSheet oTechnicalSheet)
        {
            ImageComment oImageComment = new ImageComment();
            List<ImageComment> oImageComments = new List<ImageComment>();
            try
            {
                oImageComments = ImageComment.Save(oTechnicalSheet, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oImageComment = new ImageComment();
                oImageComments = new List<ImageComment>();
                oImageComment.ErrorMessage = ex.Message;
                oImageComments.Add(oImageComment);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oImageComments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Style Picker
        public ActionResult ViewStyleSearch(string sTempStyle, double ts)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            string sSQL = "SELECT * FROM View_TechnicalSheet WHERE StyleNo LIKE '%"+sTempStyle+"%'";            
            _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oTechnicalSheets);
        }
        [HttpPost]
        public JsonResult StyleSearch(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                _oTechnicalSheets = new List<TechnicalSheet>();
                string sSQL = "SELECT * FROM View_TechnicalSheet WHERE StyleNo LIKE '%" + oTechnicalSheet.StyleNo + "%'";
                if (oTechnicalSheet.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = "+oTechnicalSheet.BUID;
                }
                if (oTechnicalSheet.BuyerID > 0)
                {
                    sSQL += " AND BuyerID = " + oTechnicalSheet.BuyerID;
                }
                if (oTechnicalSheet.BusinessSessionID > 0)
                {
                    sSQL += " AND BusinessSessionID = " + oTechnicalSheet.BusinessSessionID;
                }
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(_oTechnicalSheets);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            var jSonResult = Json(_oTechnicalSheets, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        #endregion

        [HttpPost]
        public JsonResult RequestForApproval(ApprovalRequest oApprovalRequest)
        {
            _oTechnicalSheet = new TechnicalSheet();
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            try
            {
                _oApprovalRequest = oApprovalRequest;
                _oApprovalRequest.RequestBy = ((User)Session[SessionInfo.CurrentUser]).UserID;
                _oApprovalRequest.OperationType = EnumApprovalRequestOperationType.TechnicalSheet;

                oTechnicalSheet.TechnicalSheetID = oApprovalRequest.OperationObjectID;
                oTechnicalSheet.DevelopmentStatusInInt =(int) EnumDevelopmentStatus.RequestForApproved;

                _oTechnicalSheet = _oTechnicalSheet.UpdateStatus(oTechnicalSheet.TechnicalSheetID, oTechnicalSheet.DevelopmentStatusInInt,_oApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Approved(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheet = new TechnicalSheet();
            try
            {
                _oTechnicalSheet = _oTechnicalSheet.UpdateStatus(oTechnicalSheet.TechnicalSheetID, oTechnicalSheet.DevelopmentStatusInInt,_oApprovalRequest, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private List<TempMeasurementSpecDetail> MapTempMeasurementSpecDetail(List<TechnicalSheetSize> oTechnicalSheetSizes, List<MeasurementSpecDetail> oMeasurementSpecDetails)
        {
            _oTempMeasurementSpecDetails = new List<TempMeasurementSpecDetail>();
            string sDescriptionNote = "";
            string sPropertyName = "";
            int i = 0;
            TempMeasurementSpecDetail oTempMeasurementSpecDetail = new TempMeasurementSpecDetail();
            foreach (MeasurementSpecDetail oItem in oMeasurementSpecDetails)
            {
                if (oItem.DescriptionNote != sDescriptionNote)
                {
                    oTempMeasurementSpecDetail = new TempMeasurementSpecDetail();
                    oTempMeasurementSpecDetail.MeasurementSpecDetailID = oItem.MeasurementSpecDetailID;
                    oTempMeasurementSpecDetail.POM = oItem.POM;
                    oTempMeasurementSpecDetail.DescriptionNote = oItem.DescriptionNote;
                    oTempMeasurementSpecDetail.Addition = oItem.Addition;
                    oTempMeasurementSpecDetail.Deduction = oItem.Deduction;
                    oTempMeasurementSpecDetail.Sequence = oItem.Sequence;
                    i = 0;
                    foreach (TechnicalSheetSize oTechnicalSheetSize in oTechnicalSheetSizes)
                    {
                        i++;
                        sPropertyName = "SizeValue" + i.ToString();
                        PropertyInfo prop = oTempMeasurementSpecDetail.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oTempMeasurementSpecDetail, GetSizeValue(oTechnicalSheetSize.SizeCategoryID, oItem.POM, oMeasurementSpecDetails), null);
                        }
                    }
                    _oTempMeasurementSpecDetails.Add(oTempMeasurementSpecDetail);
                }
                sDescriptionNote = oItem.DescriptionNote;
            }
            return _oTempMeasurementSpecDetails;
        }

        private double GetSizeValue(int nSizeID, string sPOM, List<MeasurementSpecDetail> oMeasurementSpecDetails)
        {
            foreach (MeasurementSpecDetail oItem in oMeasurementSpecDetails)
            {
                if (oItem.POM == sPOM && oItem.SizeID == nSizeID)
                {
                    return oItem.SizeValue;
                }

            }
            return 0.00;
        }

        private List<MeasurementSpecDetail> MapMeasurementSpecDetail(List<TechnicalSheetSize> oTechnicalSheetSizes, List<TempMeasurementSpecDetail> oTempMeasurementSpecDetails)
        {
            _oMeasurementSpecDetails = new List<MeasurementSpecDetail>();
            int i = 0;
            string sPropertyName = "";
            foreach (TempMeasurementSpecDetail oTempMeasurementSpecDetail in oTempMeasurementSpecDetails)
            {
                i = 0;
                foreach (TechnicalSheetSize oItem in oTechnicalSheetSizes)
                {

                    _oMeasurementSpecDetail = new MeasurementSpecDetail();
                    _oMeasurementSpecDetail.MeasurementSpecDetailID = oTempMeasurementSpecDetail.MeasurementSpecDetailID;
                    _oMeasurementSpecDetail.POM = oTempMeasurementSpecDetail.POM;
                    _oMeasurementSpecDetail.DescriptionNote = oTempMeasurementSpecDetail.DescriptionNote;
                    _oMeasurementSpecDetail.Deduction = oTempMeasurementSpecDetail.Deduction;
                    _oMeasurementSpecDetail.Addition = oTempMeasurementSpecDetail.Addition;
                    _oMeasurementSpecDetail.SizeID = oItem.SizeCategoryID;
                    _oMeasurementSpecDetail.Sequence = oTempMeasurementSpecDetail.Sequence;
                    i++;
                    sPropertyName = "SizeValue" + i.ToString();
                    Type obJectType = oTempMeasurementSpecDetail.GetType();
                    PropertyInfo[] aPropertys = obJectType.GetProperties();
                    foreach (PropertyInfo oProperty in aPropertys)
                    {
                        if (oProperty.Name == sPropertyName)
                        {
                            _oMeasurementSpecDetail.SizeValue = Convert.ToDouble(oTempMeasurementSpecDetail.GetType().GetProperty(oProperty.Name).GetValue(oTempMeasurementSpecDetail, null));
                            break;
                        }
                    }
                    _oMeasurementSpecDetails.Add(_oMeasurementSpecDetail);
                }
            }
            return _oMeasurementSpecDetails;
        }

    
        [HttpPost]
        public JsonResult CommitMeasurementSpece(MeasurementSpec oMeasurementSpec)
        {
            _oMeasurementSpec = new MeasurementSpec();
            try
            {
                _oMeasurementSpec = oMeasurementSpec;
                if (oMeasurementSpec.TempMeasurementSpecDetails!=null)
                {
                    _oMeasurementSpec.MeasurementSpecDetails = MapMeasurementSpecDetail(oMeasurementSpec.TechnicalSheetSizes, oMeasurementSpec.TempMeasurementSpecDetails);
                }
                _oMeasurementSpec = _oMeasurementSpec.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMeasurementSpec = new MeasurementSpec();
                _oMeasurementSpec.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMeasurementSpec);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                TechnicalSheet oTechnicalSheet = new TechnicalSheet();
                sFeedBackMessage = oTechnicalSheet.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        public ViewResult ImageHelper(int id, string ms)
        {
            _oTechnicalSheetImage = new TechnicalSheetImage();
            _oTechnicalSheetImage.TechnicalSheetID = id;
            List<TechnicalSheetThumbnail> oTechnicalSheetThumbnails = new List<TechnicalSheetThumbnail>();
            oTechnicalSheetThumbnails = TechnicalSheetThumbnail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            //foreach (TechnicalSheetThumbnail oItem in oTechnicalSheetThumbnails)
            //{
            //    oItem.ThumImage = GetImage(oItem.ThumbnailImage);
            //}
            _oTechnicalSheetImage.TechnicalSheetThumbnails = oTechnicalSheetThumbnails;
            _oTechnicalSheetImage.ImageTypeObjs = EnumObject.jGets(typeof(EnumImageType));
            TempData["message"] = ms;
            return View(_oTechnicalSheetImage);
        }

        [HttpPost]
        public ActionResult ImageHelper(HttpPostedFileBase file, TechnicalSheetImage oTechnicalSheetImage)
        {
           
            // Verify that the user selected a file
            string sErrorMessage = "";
            if (file != null && file.ContentLength > 0)
            {
                Image oImage = Image.FromStream(file.InputStream, true, true);
                //oImage.Save(@"F:\images\" + file.FileName + ".jpg");

                #region Get Thumbnail image
                Image.GetThumbnailImageAbort myCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
                Image myThumbnail = oImage.GetThumbnailImage(100, 100, myCallback, IntPtr.Zero);
                //myThumbnail.Save(@"F:\images\thum" + file.FileName + ".jpg");
                #endregion

                //Orginal Image to byte array
                byte[] aImageInByteArray = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aImageInByteArray = ms.ToArray();
                }

                //Thumbnail Image to byte array
                byte[] aThumbnailImageInByteArray = null;
                using (MemoryStream Thumbnailms = new MemoryStream())
                {
                    myThumbnail.Save(Thumbnailms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    aThumbnailImageInByteArray = Thumbnailms.ToArray();
                }
                double nMaxLength = 1000 * 1024;// File Size upto 1 MB
                if (aImageInByteArray.Length > nMaxLength)
                {
                    sErrorMessage = "You can selecte maximum 1MB image";
                }
                else if (oTechnicalSheetImage.ImageTitle == null || oTechnicalSheetImage.ImageTitle == "")
                {
                    sErrorMessage = "Please enter image title!";
                }
                else
                {
                    oTechnicalSheetImage.LargeImage = aImageInByteArray;
                    oTechnicalSheetImage.TechnicalSheetThumbnailID = 0;
                    oTechnicalSheetImage.ThumbnailImage = aThumbnailImageInByteArray;
                    oTechnicalSheetImage = oTechnicalSheetImage.Save((int)Session[SessionInfo.currentUserID]);
                }
            }
            else
            {
                sErrorMessage = "Please select an image!";
            }
            return RedirectToAction("ImageHelper", new { id = oTechnicalSheetImage.TechnicalSheetID, ms = sErrorMessage });
        }

        [HttpPost]
        public ActionResult ReloadImage(TechnicalSheet oTechnicalSheet)
        {
            double tsv = DateTime.Now.Ticks;
            return RedirectToAction("ViewTechnicalSheet", new { id = oTechnicalSheet.TechnicalSheetID, ts = tsv });
        }

        public bool ThumbnailCallback()
        {
            return false;
        }

        private Image GetImage(byte[] aImageinarray)
        {
            if (aImageinarray != null)
            {
                MemoryStream m = new MemoryStream(aImageinarray);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            return null;
        }

        public ActionResult DownloadTSImage(int id, double ts)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            try
            {
                oTechnicalSheetImage.TechnicalSheetImageID = id;
                oTechnicalSheetImage = oTechnicalSheetImage.Get(id, (int)Session[SessionInfo.currentUserID]);
                if (oTechnicalSheetImage.LargeImage != null)
                {
                    var file = File(oTechnicalSheetImage.LargeImage, "image/jpeg");
                    file.FileDownloadName = "Image #" + oTechnicalSheetImage.TechnicalSheetImageID;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + "Your Image");
            }
        }

        #region Style Search Picker

        public ActionResult StyleSearchPicker()
        {

            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            return PartialView(oTechnicalSheet);
        }

        //GetClass

        [HttpPost]
        public JsonResult GetClass()
        {
            List<GarmentsClass> oGarmentsClasses = new List<GarmentsClass>();
            GarmentsClass oGarmentsClass = new GarmentsClass();
            try
            {
                string sSql = "";
                sSql = "select * from GarmentsClass where ParentClassID =1";
                oGarmentsClasses = GarmentsClass.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oGarmentsClass = new GarmentsClass();
                oGarmentsClass.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGarmentsClasses);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult Refresh(string Search_creiteria)
        {
            List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
            try
            {
                string sSql = "";
                string sfirst_crieateria = Search_creiteria.Split('~')[0];

                _oTechnicalSheets = new List<TechnicalSheet>();                               
                if (sfirst_crieateria == "StyleNo")
                {
                    string StyleNo = Search_creiteria.Split('~')[1];
                    sSql = "select * from View_TechnicalSheet where StyleNo ='"+StyleNo+"'";
                    oTechnicalSheets = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }                
                else if (sfirst_crieateria == "BuyerID")
                {
                    string sBuyerID = Search_creiteria.Split('~')[1];
                    int BuyerID = Convert.ToInt32(sBuyerID);
                    sSql = "Select * from View_TechnicalSheet where BuyerID =" + BuyerID;
                    oTechnicalSheets = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {

                    string sGarmentsClassID = Search_creiteria.Split('~')[1];
                    string sDept = Search_creiteria.Split('~')[3];
                    int GarmentsClassID = Convert.ToInt32(sGarmentsClassID);
                    int DeptID = Convert.ToInt32(sDept);

                    sSql = "select * from View_TechnicalSheet where  Dept = "+DeptID+" or GarmentsClassID = "+GarmentsClassID;
                    oTechnicalSheets = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Sequence Pickers 
        public ActionResult ColorSequence(double ts)
        {
            TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();

            return PartialView(oTechnicalSheetColor);
        }

        //SizeSequence
        public ActionResult SizeSequence(double ts)
        {
            TechnicalSheetSize oTechnicalSheetSize = new TechnicalSheetSize();

            return PartialView(oTechnicalSheetSize);
        }
        #endregion

        #region MeasurementSpec Attachment
        public ViewResult Attachment(int id,bool bIsMeasurmentSpecAttachment, string ms, double ts)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = new MeasurementSpecAttachment();
            List<MeasurementSpecAttachment> oMeasurementSpecAttachments = new List<MeasurementSpecAttachment>();
            oMeasurementSpecAttachments = MeasurementSpecAttachment.Gets(id,bIsMeasurmentSpecAttachment, (int)Session[SessionInfo.currentUserID]);
            oMeasurementSpecAttachment.TechnicalSheetID = id;
            oMeasurementSpecAttachment.IsMeasurmentSpecAttachment = bIsMeasurmentSpecAttachment;
            oMeasurementSpecAttachment.MeasurementSpecAttachments = oMeasurementSpecAttachments;
            TempData["message"] = ms;
            return View(oMeasurementSpecAttachment);
        }

        [HttpPost]
        public ActionResult UploadAttchment(HttpPostedFileBase file, MeasurementSpecAttachment oMeasurementSpecAttachment)
        {
            string sErrorMessage = "";
            byte[] data;
            try
            {

                if (file != null && file.ContentLength > 0)
                {
                    using (Stream inputStream = file.InputStream)
                    {
                        MemoryStream memoryStream = inputStream as MemoryStream;
                        if (memoryStream == null)
                        {
                            memoryStream = new MemoryStream();
                            inputStream.CopyTo(memoryStream);
                        }
                        data = memoryStream.ToArray();
                    }

                    double nMaxLength = 1000 * 1024; // File size upto 1 MB
                    if (data == null || data.Length <= 0)
                    {
                        sErrorMessage = "Please select an file!";
                    }
                    else if (data.Length > nMaxLength)
                    {
                        sErrorMessage = "You can select maximum 1MB file size!";
                    }
                    else if (oMeasurementSpecAttachment.TechnicalSheetID <= 0)
                    {
                        sErrorMessage = "Your Selected MeasurementSpec Is Invalid!";
                    }
                    else
                    {
                        oMeasurementSpecAttachment.AttatchFile = data;
                        oMeasurementSpecAttachment.AttatchmentName = file.FileName;
                        oMeasurementSpecAttachment.FileType = file.ContentType;
                        oMeasurementSpecAttachment = oMeasurementSpecAttachment.Save((int)Session[SessionInfo.currentUserID]);
                    }
                }
                else
                {
                    sErrorMessage = "Please select an file!";
                }


            }
            catch (Exception ex)
            {
                sErrorMessage = "";
                sErrorMessage = ex.Message;
            }
            return RedirectToAction("Attachment", new { id = oMeasurementSpecAttachment.TechnicalSheetID, bIsMeasurmentSpecAttachment = oMeasurementSpecAttachment.IsMeasurmentSpecAttachment,  ms = sErrorMessage, ts = Convert.ToDouble(DateTime.Now.Millisecond) });
        }


        public ActionResult DownloadAttachment(int id, double ts)
        {
            MeasurementSpecAttachment oMeasurementSpecAttachment = new MeasurementSpecAttachment();
            try
            {
                oMeasurementSpecAttachment.MeasurementSpecAttachmentID = id;
                oMeasurementSpecAttachment = MeasurementSpecAttachment.GetWithAttachFile(id, (int)Session[SessionInfo.currentUserID]);
                if (oMeasurementSpecAttachment.AttatchFile != null)
                {
                    var file = File(oMeasurementSpecAttachment.AttatchFile, oMeasurementSpecAttachment.FileType);
                    file.FileDownloadName = oMeasurementSpecAttachment.AttatchmentName;
                    return file;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw new HttpException(404, "Couldn't find " + oMeasurementSpecAttachment.AttatchmentName);
            }
        }

        [HttpPost]
        public JsonResult DeleteAttachment(MeasurementSpecAttachment oMeasurementSpecAttachment)
        {
            string sErrorMease = "";
            try
            {
                sErrorMease = oMeasurementSpecAttachment.Delete(oMeasurementSpecAttachment.MeasurementSpecAttachmentID, (int)Session[SessionInfo.currentUserID]);
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

        #region Search Style by Press Enter
        [HttpGet]
        public JsonResult SearchStyle(string sStyleNo, string sBUIDs, double ts)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            _oTechnicalSheets = new List<TechnicalSheet>();
            string sSQL = "SELECT * FROM View_TechnicalSheet WHERE StyleNo LIKE ('%" + sStyleNo + "%')";
            if (!string.IsNullOrEmpty(sBUIDs) && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value))==true)//if apply style configuration business unit
            {
                sSQL += " AND BUID IN ( "+sBUIDs+" )";
            }
            try
            {                
                TechnicalSheet oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTechnicalSheetsByStyleNo(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            string sSQL = "SELECT * FROM View_TechnicalSheet WHERE StyleNo LIKE ISNULL(('%" + oTechnicalSheet.StyleNo + "%'),'') AND ISNULL(TechnicalSheetID,0) NOT IN (SELECT TechnicalSheetID FROM View_Job) AND BUID = " + oTechnicalSheet.BUID + " ";
            
            try
            {
                //TechnicalSheet oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        public Image GetLargeImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        [HttpGet]
        public ActionResult GetStyleImageInBase64(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage == null)
            {
                oTechnicalSheetImage.LargeImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetImage.LargeImage) }, JsonRequestBehavior.AllowGet);
        }


        #region Get Product BU, User and Name wise
        [HttpPost]
        public JsonResult GetFinishGoods(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.FinishGoods, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.FinishGoods, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetYarnCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.RawMaterial, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.RawMaterial, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetPocketLinkFabric(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.PocketLinkFabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.TechnicalSheet, EnumProductUsages.PocketLinkFabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        #endregion

    }
}
