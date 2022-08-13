using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using iTextSharp.text;
using ESimSol.Reports;
using System.Drawing.Imaging;
using System.IO;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ReportManagement;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using OfficeOpenXml;
using OfficeOpenXml.Style;


namespace ESimSolFinancial.Controllers
{
    public class AddressConfigController : PdfViewController
    {
        #region Declaration
        AddressConfig _oAddressConfig;
        List<AddressConfig> _oAddressConfigs;
        
        #endregion

        #region Views
        public ActionResult View_AddressConfigs(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oAddressConfigs = new List<AddressConfig>();


            //ViewBag.Districts = AddressConfig.Gets("SELECT * FROM AddressConfig WHERE AddressType=" + (int)EnumConfig_AddressType.District, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //ViewBag.Thanas = AddressConfig.Gets("SELECT * FROM AddressConfig WHERE AddressType=" + (int)EnumConfig_AddressType.District, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //ViewBag.PostOffices = AddressConfig.Gets("SELECT * FROM AddressConfig WHERE AddressType=" + (int)EnumConfig_AddressType.District, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.AddressType = EnumObject.jGets(typeof(EnumConfig_AddressType));

            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COS = oTempClientOperationSetting;

            return View(_oAddressConfigs);
        }

        #endregion

        #region IUD AddressConfig
        [HttpPost]
        public JsonResult AddressConfig_IU(AddressConfig oAddressConfig)
        {
            _oAddressConfig = new AddressConfig();
            try
            {
                _oAddressConfig = oAddressConfig;
                if (_oAddressConfig.AddressConfigID > 0)
                {
                    _oAddressConfig = _oAddressConfig.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oAddressConfig = _oAddressConfig.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oAddressConfig = new AddressConfig();
                _oAddressConfig.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAddressConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddressConfig_Delete(AddressConfig oAddressConfig)
        {
            _oAddressConfig = new AddressConfig();
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            try
            {
                _oAddressConfig = oAddressConfig.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                
            }
            catch (Exception ex)
            {
                _oAddressConfig = new AddressConfig();
                _oAddressConfig.ErrorMessage = ex.Message;
                oAddressConfigs.Add(_oAddressConfig);
            }

            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAddressConfig);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        
        #region Search

        [HttpPost]
        public JsonResult GetsAddressByType(AddressConfig oAddressConfig)
        {
            _oAddressConfigs = new List<AddressConfig>();
            _oAddressConfig = new AddressConfig();
            try
            {
                string sSql = "SELECT * FROM AddressConfig WHERE AddressType="+(int)EnumConfig_AddressType.District;
                _oAddressConfigs = AddressConfig.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAddressConfigs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAddressConfigs = new List<AddressConfig>();
                _oAddressConfig.ErrorMessage = ex.Message;
                _oAddressConfigs.Add(_oAddressConfig);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAddressConfigs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetsThanaPOVill(AddressConfig oAddressConfig)
        {
            _oAddressConfigs = new List<AddressConfig>();
            _oAddressConfig = new AddressConfig();
            try
            {
                string sSql = "SELECT * FROM AddressConfig WHERE ParentAddressID="+oAddressConfig.ParentAddressID;
                _oAddressConfigs = AddressConfig.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oAddressConfigs.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oAddressConfigs = new List<AddressConfig>();
                _oAddressConfig.ErrorMessage = ex.Message;
                _oAddressConfigs.Add(_oAddressConfig);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oAddressConfigs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetsAddressAutoSearchDist(int Value, string SearchText)
        {
            
            string sSql = "";
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            SearchText = SearchText == null ? "" : SearchText;
            sSql = "SELECT * FROM AddressConfig WHERE AddressType=" + Value + " AND (NameInEnglish LIKE'%" + SearchText + "%' OR NameInBangla LIKE'%" + SearchText + "%')";


            oAddressConfigs = AddressConfig.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpGet]
        public JsonResult GetsAddressAutoSearchPostOffice(int Value, string SearchText)
        {
            int nPostOfficeID = 0;
            try
            {
                nPostOfficeID = (int)Session["PostOfficeID"];
            }
            catch (Exception ex)
            {
                nPostOfficeID = 0;
            }
            string sSql = "";
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            SearchText = SearchText == null ? "" : SearchText;
            sSql = "SELECT * FROM AddressConfig WHERE AddressType=" + Value + " AND (NameInEnglish LIKE'%" + SearchText + "%' OR NameInBangla LIKE'%" + SearchText + "%')";

            if (nPostOfficeID > 0)
            {
                sSql += " AND ParentAddressID=" + nPostOfficeID;
            }

            oAddressConfigs = AddressConfig.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult GetsAddressAutoSearchThana(int Value, string SearchText)
        {
            int nThanaID = 0;
            try
            {
                nThanaID = (int)Session["DistrictID"];
            }
            catch (Exception ex)
            {
                nThanaID = 0;
            }
            string sSql = "";
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            SearchText = SearchText == null ? "" : SearchText;
            sSql = "SELECT * FROM AddressConfig WHERE AddressType=" + Value + " AND (NameInEnglish LIKE'%" + SearchText + "%' OR NameInBangla LIKE'%" + SearchText + "%')";

            if (nThanaID > 0)
            {
                sSql += " AND ParentAddressID=" + nThanaID;
            }

            oAddressConfigs = AddressConfig.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpGet]
        public JsonResult GetsAddressAutoSearchVill(int Value, string SearchText)
       {
            int nVillageID = 0;
            try
            {
                nVillageID = (int)Session["VillageID"];
            }
            catch (Exception ex)
            {
                nVillageID = 0;
            }
            string sSql = "";
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            SearchText = SearchText == null ? "" : SearchText;
            sSql = "SELECT * FROM AddressConfig WHERE AddressType=" + Value + " AND (NameInEnglish LIKE'%" + SearchText + "%' OR NameInBangla LIKE'%" + SearchText + "%')";

            if (nVillageID > 0)
            {
                sSql += " AND ParentAddressID=" + nVillageID;
            }

            oAddressConfigs = AddressConfig.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        [HttpPost]
        public JsonResult SetAutocompleteSessionDataDist(AddressConfig oAddressConfig)
        {
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            this.Session.Remove("DistrictID");
            this.Session.Add("DistrictID", oAddressConfig.ParentAddressID);
            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SetAutocompleteSessionDataPO(AddressConfig oAddressConfig)
        {
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            this.Session.Remove("PostOfficeID");
            this.Session.Add("PostOfficeID", oAddressConfig.ParentAddressID);
            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SetAutocompleteSessionDataThana(AddressConfig oAddressConfig)
        {
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            this.Session.Remove("ThanaID");
            this.Session.Add("ThanaID", oAddressConfig.ParentAddressID);
            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        [HttpPost]
        public JsonResult SetAutocompleteSessionDataVill(AddressConfig oAddressConfig)
        {
            List<AddressConfig> oAddressConfigs = new List<AddressConfig>();
            this.Session.Remove("VillageID");
            this.Session.Add("VillageID", oAddressConfig.ParentAddressID);
            var jsonResult = Json(oAddressConfigs, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        #endregion
    }
}
