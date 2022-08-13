using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using System.Web;

namespace ESimSolFinancial.Controllers
{
    public class BusinessUnitController : Controller
    {
        #region Declaration
        BusinessUnit _oBusinessUnit = new BusinessUnit();
        List<BusinessUnit> _oBusinessUnits = new List<BusinessUnit>();

        TLocation _oTLocation = new TLocation();
        List<TLocation> _oTLocations = new List<TLocation>();
        List<Location> _oLocations = new List<Location>();
        string _sErrorMessage = "";
        string _sSQL = "";
        #endregion
        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM View_BusinessUnit";
            string sCode = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            //string sBusinessUnitIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sSQL = "";

            #region Code
            if (sCode != null)
            {
                if (sCode != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Code LIKE '%" + sCode + "%' ";
                }
            }
            #endregion
            #region Name
            if (sName != null)
            {
                if (sName != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
                }
            }
            #endregion
           
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
           
        }
        #endregion

        #region Used Code
        public ActionResult ViewBusinessUnits(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.BusinessUnit).ToString(), (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));
            _oBusinessUnits = new List<BusinessUnit>();
            _oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(_oBusinessUnits);
        }
        
        public ActionResult ViewBusinessUnit(int id)
        {
            _oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oBusinessUnit = _oBusinessUnit.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oBusinessUnit.BusinessNatures = EnumObject.jGets(typeof(EnumBusinessNature));
            _oBusinessUnit.LegalFormats = EnumObject.jGets(typeof(EnumLegalFormat));
            _oBusinessUnit.BusinessUnitTypeObjs = EnumObject.jGets(typeof(EnumBusinessUnitType));

            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COS = oTempClientOperationSetting;

            return View(_oBusinessUnit);
        }
        [HttpPost]
        public JsonResult Gets(BusinessUnit oBusinessUnit)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnit.ErrorMessage = oBusinessUnit.ErrorMessage == null ? "" : oBusinessUnit.ErrorMessage;
            this.MakeSQL(oBusinessUnit.ErrorMessage);

            oBusinessUnits = BusinessUnit.Gets(_sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = new BusinessUnit();
            try
            {
                if (oBusinessUnit.BusinessUnitID > 0)
                {
                    _oBusinessUnit = _oBusinessUnit.Get(oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oBusinessUnit = new BusinessUnit();
                _oBusinessUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBusinessUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewUploadLogo(int id)
        {
            _oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oBusinessUnit = _oBusinessUnit.GetWithImage(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            _oBusinessUnit.BusinessNatures = EnumObject.jGets(typeof(EnumBusinessNature));
            _oBusinessUnit.LegalFormats = EnumObject.jGets(typeof(EnumLegalFormat));
            _oBusinessUnit.BusinessUnitTypeObjs = EnumObject.jGets(typeof(EnumBusinessUnitType));

            if (_oBusinessUnit.BUImage != null)
            {
                _oBusinessUnit.BUImageSt = "data:image/Jpeg;base64," + Convert.ToBase64String(_oBusinessUnit.BUImage);
            }
            return View(_oBusinessUnit);
        }

        [HttpPost]
        public JsonResult UploadLogo(double ts)
        {
            byte[] aImageInByteArray;
            _oBusinessUnit = new BusinessUnit();
            string sResult = "";
            try
            {
                _oBusinessUnit.BusinessUnitID = Convert.ToInt32(Request.Headers["BusinessUnitID"]);
                #region File
                if (Request.Files.Count > 0)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    int fileSize = file.ContentLength;
                    //fileName = file.FileName;
                    string mimeType = file.ContentType;
                    if (file != null && file.ContentLength > 0)
                    {
                        Image oImage = Image.FromStream(file.InputStream, true, true);
                        aImageInByteArray = null;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            oImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            aImageInByteArray = ms.ToArray();
                        }
                        _oBusinessUnit.BUImage = aImageInByteArray;
                    }
                    else
                    {
                        _oBusinessUnit.BUImage = null;
                    }
                }
                else
                {
                    _oBusinessUnit.BUImage = null;
                }

                #endregion

                _oBusinessUnit = _oBusinessUnit.UpdateImage((int)Session[SessionInfo.currentUserID]);
                sResult = "Data Save Successfully";
            }
            catch (Exception ex)
            {
                _oBusinessUnit = new BusinessUnit();
                _oBusinessUnit.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sResult);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public Image GetBULogo(BusinessUnit oBusinessUnit)
        {
            if (oBusinessUnit.BUImage != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oBusinessUnit.BUImage);
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

        public ActionResult ViewBusinessUnitPicker(string sName, double nts)
        {
            _oBusinessUnits = new List<BusinessUnit>();
            _oBusinessUnit = new BusinessUnit();
            string sSql = "";
            try
            {
                if (string.IsNullOrEmpty(sName))
                {
                    sSql = "SELECT * FROM View_BusinessUnit order by Name ";
                }
                else
                {
                    sSql = "SELECT * FROM View_BusinessUnit WHERE Name LIKE '%" + sName + "%'";
                }
                _oBusinessUnits = BusinessUnit.Gets(sSql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oBusinessUnits.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oBusinessUnits = new List<BusinessUnit>();
                _oBusinessUnit.ErrorMessage = ex.Message;
                _oBusinessUnits.Add(_oBusinessUnit);
            }
            return PartialView(_oBusinessUnits);
        }

        [HttpPost]
        public JsonResult Save(BusinessUnit oBusinessUnit)
        {
            _oBusinessUnit = new BusinessUnit();
            oBusinessUnit.Name = oBusinessUnit.Name == null ? "" : oBusinessUnit.Name;
            oBusinessUnit.ShortName = oBusinessUnit.ShortName == null ? "" : oBusinessUnit.ShortName;
            oBusinessUnit.RegistrationNo = oBusinessUnit.RegistrationNo == null ? "" : oBusinessUnit.RegistrationNo;
            oBusinessUnit.TINNo = oBusinessUnit.TINNo == null ? "" : oBusinessUnit.TINNo;
            oBusinessUnit.VatNo = oBusinessUnit.VatNo == null ? "" : oBusinessUnit.VatNo;
            oBusinessUnit.Address = oBusinessUnit.Address == null ? "" : oBusinessUnit.Address;
            oBusinessUnit.Phone = oBusinessUnit.Phone == null ? "" : oBusinessUnit.Phone;
            oBusinessUnit.Email = oBusinessUnit.Email == null ? "" : oBusinessUnit.Email;
            oBusinessUnit.WebAddress = oBusinessUnit.WebAddress == null ? "" : oBusinessUnit.WebAddress;
            oBusinessUnit.Note = oBusinessUnit.Note == null ? "" : oBusinessUnit.Note;
            oBusinessUnit.NameInBangla = oBusinessUnit.NameInBangla == null ? "" : oBusinessUnit.NameInBangla;
            oBusinessUnit.AddressInBangla = oBusinessUnit.AddressInBangla == null ? "" : oBusinessUnit.AddressInBangla;
            oBusinessUnit.FaxNo = oBusinessUnit.FaxNo == null ? "" : oBusinessUnit.FaxNo;
            try
            {
                _oBusinessUnit = oBusinessUnit;
                _oBusinessUnit = _oBusinessUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oBusinessUnit = new BusinessUnit();
                _oBusinessUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBusinessUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(BusinessUnit oBusinessUnit)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oBusinessUnit.Delete(oBusinessUnit.BusinessUnitID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetBusinessUnitsByCode(BusinessUnit oBusinessUnit)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
          
            oBusinessUnits=BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetBusinessUnits()
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit.Name = "-- Select BusinessUnit --";
            oBusinessUnits.Add(oBusinessUnit);
            oBusinessUnits.AddRange(BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID));
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsBUByCodeOrName(BusinessUnit oBusinessUnit)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = BusinessUnit.GetsBUByCodeOrName(oBusinessUnit.NameCode, (int)Session[SessionInfo.currentUserID]);
            if(oBusinessUnit.BusinessUnitID>0)
            {
                oBusinessUnits.RemoveAll(x => x.BusinessUnitID != oBusinessUnit.BusinessUnitID);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


       

        [HttpPost]
        public JsonResult GetsBUByCodeOrNameAndAccountHead(BusinessUnit oBusinessUnit)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = BusinessUnit.GetsBUByCodeOrNameAndAccountHead(oBusinessUnit.NameCode, oBusinessUnit.BusinessUnitID, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult PrintBusinessUnits(FormCollection DataCollection)
        {
            _oBusinessUnits = new List<BusinessUnit>();
            _oBusinessUnits = new JavaScriptSerializer().Deserialize<List<BusinessUnit>>(DataCollection["txtBusinessUnitCollectionList"]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            

            string Messge = "BusinessUnit List";
            rptBusinessUnits oReport = new rptBusinessUnits();
            byte[] abytes = oReport.PrepareReport(_oBusinessUnits,oCompany, Messge);
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

        public ActionResult PrintBusinessUnitsInXL()
        {
            //_productsServices = new ProductsServices();
            var stream = new MemoryStream();
            var serializer = new XmlSerializer(typeof(List<BusinessUnitXL>));

            //We load the data
            List<BusinessUnit> oBusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            int nCount = 0; double nTotal = 0;
            BusinessUnitXL oBusinessUnitXL = new BusinessUnitXL();
            List<BusinessUnitXL> oBusinessUnitXLs = new List<BusinessUnitXL>();
            foreach (BusinessUnit oItem in oBusinessUnits)
            {
                nCount++;
                oBusinessUnitXL = new BusinessUnitXL();
                oBusinessUnitXL.SLNo = nCount.ToString();
                oBusinessUnitXL.Code = oItem.Code;
                oBusinessUnitXL.Name = oItem.Name;
                oBusinessUnitXL.ShortName = oItem.ShortName;
                oBusinessUnitXL.RegistrationNo = oItem.RegistrationNo;
                oBusinessUnitXL.Address = oItem.Address;
                oBusinessUnitXL.Phone = oItem.Phone;
                oBusinessUnitXL.Email = oItem.Email;
                oBusinessUnitXL.WebAddress = oItem.WebAddress;
                oBusinessUnitXL.Note = oItem.Note;
                oBusinessUnitXLs.Add(oBusinessUnitXL);
                nTotal = nTotal + nCount;
            }           

            //We turn it into an XML and save it in the memory
            serializer.Serialize(stream, oBusinessUnitXLs);
            stream.Position = 0;

            //We return the XML from the memory as a .xls file
            return File(stream, "application/vnd.ms-excel", "Orders.xls");
        }

        #region BusinessLocations
        public ActionResult ViewBusinessLocationAssign(int id)
        {
            _oBusinessUnit = new BusinessUnit();
            if (id > 0)
            {
                _oBusinessUnit = _oBusinessUnit.Get(id, ((User)Session[SessionInfo.CurrentUser]).UserID);
               
            }
            return View(_oBusinessUnit);
        }
  
        private List<TLocation> GetChild(int nLocationID)
        {
            List<TLocation> oTLocations = new List<TLocation>();
            foreach (TLocation oItem in _oTLocations)
            {
                if (oItem.parentid == nLocationID)
                {
                    oTLocations.Add(oItem);
                }
            }
            return oTLocations;
        }

        private void AddTreeNodes(ref TLocation oTLocation)
        {
            List<TLocation> oChildNodes;
            oChildNodes = GetChild(oTLocation.id);
            oTLocation.children = oChildNodes;

            foreach (TLocation oItem in oChildNodes)
            {
                TLocation oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }
        #endregion

        [HttpPost]
        public JsonResult GetsBusinessUnitWithPermission(BusinessUnit oBusinessUnit)
        {
            _oBusinessUnits = new List<BusinessUnit>();
            try
            {
                string sSQL = "";
                sSQL = "SELECT * FROM BusinessUnit WHERE BusinessUnitID IN(SELECT BusinessUnitID FROM DepartmentRequirementPolicy WHERE DepartmentRequirementPolicyID<>0";
                if (((User)(Session[SessionInfo.CurrentUser])).FinancialUserType != EnumFinancialUserType.GroupAccounts)
                {
                    sSQL = sSQL + " AND DepartmentRequirementPolicyID IN(SELECT DRPID FROM DepartmentRequirementPolicyPermission WHERE UserID =" + ((User)(Session[SessionInfo.CurrentUser])).UserID + " )";
                }
                sSQL = sSQL + ")";
                _oBusinessUnits = BusinessUnit.Gets(sSQL, (int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                _oBusinessUnit = new BusinessUnit();
                _oBusinessUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oBusinessUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}
