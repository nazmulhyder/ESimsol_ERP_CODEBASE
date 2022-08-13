using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ESimSol.Reports;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class IncomeTaxBasicSetupController : Controller
    {
        #region Declaration
        ITaxAssessmentYear _oITaxAssessmentYear;
        List<ITaxAssessmentYear> _oITaxAssessmentYears;
        ITaxRateScheme _oITaxRateScheme;
        List<ITaxRateScheme> _oITaxRateSchemes;
        ITaxRebateItem _oITaxRebateItem;
        List<ITaxRebateItem> _oITaxRebateItems;
        #endregion

        #region Views
        public ActionResult View_AssessmentYears(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxAssessmentYears = new List<ITaxAssessmentYear>();
            string sSql = "SELECT * FROM ITaxAssessmentYear WHERE IsActive=1";
            _oITaxAssessmentYears = ITaxAssessmentYear.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            
            return View(_oITaxAssessmentYears);
        }

        public ActionResult View_TaxRates(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxRateSchemes = new List<ITaxRateScheme>();
            string sSql = "SELECT *  FROM ITaxRateScheme";
            _oITaxRateSchemes = ITaxRateScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oITaxRateSchemes);
        }

        public ActionResult View_TaxRate(int nId, double ts)
        {
            _oITaxRateScheme = new ITaxRateScheme();

            if (nId > 0)
            {
                _oITaxRateScheme = ITaxRateScheme.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string sSql = "SELECT *  FROM ITaxRateSlab WHERE ITaxRateSchemeID=" + nId;
                _oITaxRateScheme.ITaxRateSlabs = ITaxRateSlab.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oITaxRateScheme);
        }

        public ActionResult View_RateAndRebates(int ITaxAssessmentYearID)
        {
            _oITaxRateSchemes = new List<ITaxRateScheme>();
            string RateSlabSql = "SELECT * FROM ITaxRateSlab WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + ITaxAssessmentYearID + ")";
            string RebateSchemeSql = "SELECT * FROM ITaxRebateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + ITaxAssessmentYearID + ")";

            ViewBag.ITaxRateSchemes = ITaxRateScheme.Gets("SELECT * FROM ITaxRateScheme WHERE ITaxAssessmentYearID =" + ITaxAssessmentYearID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ITaxRateSchemeSlabs = ITaxRateSlab.Gets(RateSlabSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ITaxRebateSchemes = ITaxRebateScheme.Gets(RebateSchemeSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ITaxRebateSchemeSlabs = ITaxRebateSchemeSlab.Gets("SELECT * FROM ITaxRebateSchemeSlab WHERE ITaxRebateSchemeID IN( SELECT ITaxRebateSchemeID FROM ITaxRebateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + ITaxAssessmentYearID + "))", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //ViewBag.ITaxRebateSchemeSlabDetails = ITaxRebateSchemeSlabDetail.Gets("SELECT * FROM ITaxRebateSchemeSlabDetail WHERE ITaxRSSID IN( SELECT ITaxRSSID FROM ITaxRebateSchemeSlab WHERE ITaxRebateSchemeID IN(SELECT ITaxRebateSchemeID FROM ITaxRebateScheme WHERE ITaxRateSchemeID IN(SELECT ITaxRateSchemeID FROM ITaxRateScheme WHERE ITaxAssessmentYearID=" + ITaxAssessmentYearID + ")))", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.Company = Company.Gets("SELECT TOP(1)* FROM View_Company", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.SalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=3", ((User)(Session[SessionInfo.CurrentUser])).UserID);
           
            return View(_oITaxRateSchemes);
        }

        #endregion

        #region ITaxRebateItems
        public ActionResult View_TaxRebateItems(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oITaxRebateItems = new List<ITaxRebateItem>();
            string sSql = "SELECT *  FROM ITaxRebateItem";
            _oITaxRebateItems = ITaxRebateItem.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.EnumITaxRebateTypes = Enum.GetValues(typeof(EnumITaxRebateType)).Cast<EnumITaxRebateType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oITaxRebateItems);
        }

        [HttpPost]
        public JsonResult ITaxRebateItem_IU(ITaxRebateItem oITaxRebateItem)
        {
            try
            {
                if (oITaxRebateItem.ITaxRebateItemID > 0)
                {
                    oITaxRebateItem = oITaxRebateItem.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRebateItem = oITaxRebateItem.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateItem);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRebateItem_Delete(ITaxRebateItem oITaxRebateItem)
        {
            try
            {
                if (oITaxRebateItem.ITaxRebateItemID <= 0)
                    throw new Exception("Please select a valid item from list.");
                oITaxRebateItem = oITaxRebateItem.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateItem.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRebateItem_Activity(ITaxRebateItem oITaxRebateItem)
        {
            try
            {
                if (oITaxRebateItem.ITaxRebateItemID<=0)
                    throw new Exception("Please select a valid item from list.");
                oITaxRebateItem = ITaxRebateItem.Activite(oITaxRebateItem.ITaxRebateItemID, !oITaxRebateItem.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateItem);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetITaxRebateItem(ITaxRebateItem oITaxRebateItem)
        {
            try
            {
                oITaxRebateItem = ITaxRebateItem.Get(oITaxRebateItem.ITaxRebateItemID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oITaxRebateItem = new ITaxRebateItem();
                oITaxRebateItem.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateItem);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region ITaxAssessmentYear

        [HttpPost]
        public JsonResult ITaxAssessmentYear_IU(ITaxAssessmentYear oITaxAssessmentYear)
        {

            _oITaxAssessmentYear = new ITaxAssessmentYear();
            try
            {
                oITaxAssessmentYear.Description = oITaxAssessmentYear.StartDate.ToString("yyyy") + "-" + oITaxAssessmentYear.EndDate.ToString("yyyy");
                _oITaxAssessmentYear = oITaxAssessmentYear;

                if (_oITaxAssessmentYear.ITaxAssessmentYearID > 0)
                {
                    _oITaxAssessmentYear = _oITaxAssessmentYear.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oITaxAssessmentYear = _oITaxAssessmentYear.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oITaxAssessmentYear = new ITaxAssessmentYear();
                _oITaxAssessmentYear.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxAssessmentYear);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxAssessmentYear_GetForEdit(int nITaxAssessmentYearID)
        {

            _oITaxAssessmentYear = new ITaxAssessmentYear();
            try
            {
                string sSql = "SELECT * FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID =" + nITaxAssessmentYearID;
                _oITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oITaxAssessmentYear = new ITaxAssessmentYear();
                _oITaxAssessmentYear.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxAssessmentYear);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ITaxAssessmentYear_Delete(int nITaxAssessmentYearID, double ts)
        {
            _oITaxAssessmentYear = new ITaxAssessmentYear();
            try
            {
                _oITaxAssessmentYear.ITaxAssessmentYearID = nITaxAssessmentYearID;
                _oITaxAssessmentYear = _oITaxAssessmentYear.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxAssessmentYear = new ITaxAssessmentYear();
                _oITaxAssessmentYear.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxAssessmentYear.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxAssessmentYear_Inactive(ITaxAssessmentYear oITaxAssessmentYear)
        {
            _oITaxAssessmentYear = new ITaxAssessmentYear();
            try
            {
                string sSql = "UPDATE ITaxAssessmentYear SET IsActive = 0 WHERE ITaxAssessmentYearID =" + oITaxAssessmentYear.ITaxAssessmentYearID +
                              "SELECT * FROM ITaxAssessmentYear WHERE ITaxAssessmentYearID =" + oITaxAssessmentYear.ITaxAssessmentYearID;
                _oITaxAssessmentYear = ITaxAssessmentYear.Get(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxAssessmentYear = new ITaxAssessmentYear();
                _oITaxAssessmentYear.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxAssessmentYear);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region ITaxRateScheme
       
        [HttpPost]
        public JsonResult ITaxRateScheme_IU(ITaxRateScheme oITaxRateScheme)
        {

            _oITaxRateScheme = new ITaxRateScheme();
            try
            {
                _oITaxRateScheme = oITaxRateScheme;
                _oITaxRateScheme.TaxPayerType = (EnumTaxPayerType)oITaxRateScheme.TaxPayerTypeInint;
                _oITaxRateScheme.TaxArea = (EnumTaxArea)oITaxRateScheme.TaxAreaInint;
                //for (int i = 0; i < _oITaxRateScheme.ITaxRateSlabs.Count; i++)
                //{
                //    _oITaxRateScheme.ITaxRateSlabs[i].SequenceType = (EnumSequenceType)oITaxRateScheme.ITaxRateSlabs[i].SequenceTypeInint;
                //}
                if (_oITaxRateScheme.ITaxRateSchemeID > 0)
                {
                    _oITaxRateScheme = _oITaxRateScheme.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oITaxRateScheme = _oITaxRateScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oITaxRateScheme = new ITaxRateScheme();
                _oITaxRateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRateScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ITaxRateScheme_Delete(int nITaxRateSchemeID, double ts)
        {
            _oITaxRateScheme = new ITaxRateScheme();
            try
            {
                _oITaxRateScheme.ITaxRateSchemeID = nITaxRateSchemeID;
                _oITaxRateScheme = _oITaxRateScheme.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxRateScheme = new ITaxRateScheme();
                _oITaxRateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRateScheme.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRateSchemeSlab_IU(ITaxRateSlab oITaxRateSchemeSlab)
        {
            try
            {
                oITaxRateSchemeSlab.SequenceType = (EnumSequenceType)oITaxRateSchemeSlab.SequenceTypeInint;

                if (oITaxRateSchemeSlab.ITaxRateSlabID > 0)
                {
                    oITaxRateSchemeSlab = oITaxRateSchemeSlab.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRateSchemeSlab = oITaxRateSchemeSlab.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRateSchemeSlab = new ITaxRateSlab();
                oITaxRateSchemeSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRateSchemeSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ITaxRateSlab_Delete(int nITaxRateSlabID, double ts)
        {
            ITaxRateSlab _oITaxRateSlab = new ITaxRateSlab();
            try
            {
                _oITaxRateSlab.ITaxRateSlabID = nITaxRateSlabID;
                _oITaxRateSlab = _oITaxRateSlab.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxRateSlab = new ITaxRateSlab();
                _oITaxRateSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRateSlab.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ITaxRateScheme_Activity(ITaxRateScheme oITaxRateScheme)
        {
            _oITaxRateScheme = new ITaxRateScheme();
            try
            {

                _oITaxRateScheme = oITaxRateScheme;
                _oITaxRateScheme = ITaxRateScheme.Activite(_oITaxRateScheme.ITaxRateSchemeID, _oITaxRateScheme.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oITaxRateScheme = new ITaxRateScheme();
                _oITaxRateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRateScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        #endregion

        #region Rebate
        #region ITaxRebateItem_IUD
        [HttpPost]
        public JsonResult ITaxRebateScheme_IU(ITaxRebateScheme oITaxRebateScheme)
        {

            try
            {
                oITaxRebateScheme.ITaxRebateType = (EnumITaxRebateType)oITaxRebateScheme.ITaxRebateTypeInint;
                if (oITaxRebateScheme.ITaxRebateSchemeID > 0)
                {
                    oITaxRebateScheme = oITaxRebateScheme.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRebateScheme = oITaxRebateScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRebateScheme = new ITaxRebateScheme();
                oITaxRebateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult ITaxRebateScheme_Delete(int nITaxRebateSchemeID, double ts)
        {
            ITaxRebateScheme oITaxRebateScheme = new ITaxRebateScheme();
            try
            {
                oITaxRebateScheme.ITaxRebateSchemeID = nITaxRebateSchemeID;
                oITaxRebateScheme = oITaxRebateScheme.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oITaxRebateScheme = new ITaxRebateScheme();
                oITaxRebateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateScheme.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion ITaxRebateItem_IUD

        #region ITaxRebateSchemeSlab_IUD
        [HttpPost]
        public JsonResult ITaxRebateSchemeSlab_IU(ITaxRebateSchemeSlab oITaxRebateSchemeSlab)
        {
            try
            {
                if (oITaxRebateSchemeSlab.ITaxRSSID > 0)
                {
                    oITaxRebateSchemeSlab = oITaxRebateSchemeSlab.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRebateSchemeSlab = oITaxRebateSchemeSlab.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
                oITaxRebateSchemeSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateSchemeSlab);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ITaxRebateSchemeSlab_Delete(int nITaxRSSID, double ts)
        {
            ITaxRebateSchemeSlab oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
            try
            {
                oITaxRebateSchemeSlab.ITaxRSSID = nITaxRSSID;
                oITaxRebateSchemeSlab = oITaxRebateSchemeSlab.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oITaxRebateSchemeSlab = new ITaxRebateSchemeSlab();
                oITaxRebateSchemeSlab.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateSchemeSlab.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion ITaxRebateSchemeSlab_IUD

        #region ITaxRebateSchemeSlabDetail_IUD
        [HttpPost]
        public JsonResult ITaxRebateSchemeSlabDetail_IU(ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail)
        {
            try
            {
                if (oITaxRebateSchemeSlabDetail.ITaxRSSDID > 0)
                {
                    oITaxRebateSchemeSlabDetail = oITaxRebateSchemeSlabDetail.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oITaxRebateSchemeSlabDetail = oITaxRebateSchemeSlabDetail.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                oITaxRebateSchemeSlabDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateSchemeSlabDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult ITaxRebateSchemeSlabDetail_Delete(int nITaxRSSDID, double ts)
        {
            ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
            try
            {
                oITaxRebateSchemeSlabDetail.ITaxRSSDID = nITaxRSSDID;
                oITaxRebateSchemeSlabDetail = oITaxRebateSchemeSlabDetail.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                oITaxRebateSchemeSlabDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateSchemeSlabDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsITaxRebateSchemeSlabDetail(int nITaxRSSID)
        {
            List<ITaxRebateSchemeSlabDetail> oITaxRebateSchemeSlabDetails = new List<ITaxRebateSchemeSlabDetail>();
            try
            {
                if (nITaxRSSID > 0)
                {
                    string sSql = "SELECT * FROM ITaxRebateSchemeSlabDetail WHERE ITaxRSSID=" + nITaxRSSID;
                    oITaxRebateSchemeSlabDetails = ITaxRebateSchemeSlabDetail.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                ITaxRebateSchemeSlabDetail oITaxRebateSchemeSlabDetail = new ITaxRebateSchemeSlabDetail();
                oITaxRebateSchemeSlabDetails = new List<ITaxRebateSchemeSlabDetail>();
                oITaxRebateSchemeSlabDetail.ErrorMessage = ex.Message;
                oITaxRebateSchemeSlabDetails.Add(oITaxRebateSchemeSlabDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oITaxRebateSchemeSlabDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion ITaxRebateSchemeSlab_IUD

        #endregion Rebate

        #region Gets_RateSchem_With_Salb_And_Rebate

        [HttpPost]
        public JsonResult Gets_RateSchem_With_Salb_And_Rebate(int nITaxRateSchemeID)
        {
            _oITaxRateScheme = new ITaxRateScheme();
            try
            {
                _oITaxRateScheme = ITaxRateScheme.Get(nITaxRateSchemeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                string sSql_Slab = "SELECT * FROM ITaxRateSlab WHERE ITaxRateSchemeID=" + nITaxRateSchemeID + " ORDER BY Percents";
                string sSql_Rebate = "SELECT * FROM ITaxRebateScheme WHERE ITaxRateSchemeID=" + nITaxRateSchemeID;
                _oITaxRateScheme.ITaxRateSlabs = ITaxRateSlab.Gets(sSql_Slab, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oITaxRateScheme.ITaxRebateSchemes = ITaxRebateScheme.Gets(sSql_Rebate, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oITaxRateScheme = new ITaxRateScheme();
                _oITaxRateScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oITaxRateScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Gets_RateSchem_With_Salb_And_Rebate

    }
}
