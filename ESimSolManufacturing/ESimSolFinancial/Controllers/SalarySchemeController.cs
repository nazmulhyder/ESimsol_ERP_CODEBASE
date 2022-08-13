using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;

using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSolFinancial.Controllers
{
    public class SalarySchemeController : Controller
    {
        #region Declaration
        SalaryScheme _oSalaryScheme;
        private List<SalaryScheme> _oSalarySchemes;
        #endregion

        #region Views
        public ActionResult View_SalarySchemes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            _oSalarySchemes = new List<SalaryScheme>();
            sSQL = "select * from SalaryScheme";
            _oSalarySchemes = SalaryScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oSalarySchemes);
        }

        public ActionResult View_SalarySchemes_V1(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string sSQL = "";
            _oSalarySchemes = new List<SalaryScheme>();
            sSQL = "select * from SalaryScheme";
            _oSalarySchemes = SalaryScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oSalarySchemes);
        }

        public ActionResult View_SalaryScheme(int nId, double ts)//nId=SalarySchemeID
        {

            _oSalaryScheme = GetForPartialView(nId);
            return PartialView(_oSalaryScheme);

        }

        public ActionResult View_SalaryScheme_V1(string sid, string sMsg)//nId=SalarySchemeID
        {
            string sEquation = "";
            int nSSID = Convert.ToInt32(sid != "0" ? Global.Decrypt(sid) : "0");
            _oSalaryScheme = GetForPartialView(nSSID);

            if (nSSID > 0)
            {
                List<SalarySchemeDetailCalculation> oSalarySchemeDetailCalculations = new List<SalarySchemeDetailCalculation>();
                string sSql = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + nSSID + ") ORDER BY SalarySchemeDetailID ";
                oSalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(_oSalaryScheme.SalarySchemeDetails, oSalarySchemeDetailCalculations);
                string sSql_PB = "SELECT * FROM ProductionBonus WHERE SalarySchemeID=" + nSSID ;
                _oSalaryScheme.ProductionBonuss = ProductionBonus.Gets(sSql_PB, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                

                List<SalarySchemeDetailCalculation> oCalculations = new List<SalarySchemeDetailCalculation>();
                oCalculations = SalarySchemeDetailCalculation.GetsBySalarySchemeGross(nSSID, (int)Session[SessionInfo.currentUserID]);
                sEquation = SalarySchemeDetail.GetEquation(oCalculations);
            }
            if (sMsg != "N/A")
            {
                _oSalaryScheme.ErrorMessage = sMsg;
            }
            ViewBag.GrossEquation = sEquation;
            ViewBag.PayRollApplyOns = Enum.GetValues(typeof(EnumPayrollApplyOn)).Cast<EnumPayrollApplyOn>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            ViewBag.SalaryTypes = Enum.GetValues(typeof(EnumSalaryType)).Cast<EnumSalaryType>().Select(x => new SelectListItem { Text = x.ToString(), Value = ((int)x).ToString() }).ToList();
            return View(_oSalaryScheme);

        }

        public SalaryScheme GetForPartialView(int nId)
        {
            _oSalaryScheme = new SalaryScheme();
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();

            List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();
            if (nId > 0)
            {

                _oSalaryScheme = SalaryScheme.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.Gets(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID).OrderBy(x=>x.SalaryHeadID).ToList();

                string sSql = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1 AND SalaryHeadID NOT IN (SELECT SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=" + nId + ")  ORDER BY SalaryHeadID ";
                oSalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (SalaryHead oitem in oSalaryHeads)
                {
                    SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
                    oSalarySchemeDetail.SalaryHeadID = oitem.SalaryHeadID;
                    oSalarySchemeDetail.SalaryHeadName = oitem.Name;
                    oSalarySchemeDetail.SalaryHeadType = oitem.SalaryHeadType;
                    _oSalaryScheme.SalarySchemeDetails.Add(oSalarySchemeDetail);
                }
            }
            else
            {
                string sSql = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1 ORDER BY SalaryHeadID";// Basic Type
                oSalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (SalaryHead oitem in oSalaryHeads)
                {
                    SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
                    oSalarySchemeDetail.SalarySchemeID = nId;
                    oSalarySchemeDetail.SalaryHeadID = oitem.SalaryHeadID;
                    oSalarySchemeDetail.SalaryHeadName = oitem.Name;
                    oSalarySchemeDetails.Add(oSalarySchemeDetail);
                }
                _oSalaryScheme.SalarySchemeDetails = oSalarySchemeDetails;
            }
            

            return _oSalaryScheme;
        }

        public ActionResult View_AddAllowance(int nId, double ts)//nId=SalarySchemeDetailID
        {
            SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
            oSalarySchemeDetail.SalarySchemeID = nId;
            return PartialView(oSalarySchemeDetail);
        }

        public ActionResult View_AddSalaryHeadCalculation(double ts)
        {
            SalarySchemeDetailCalculation oSalarySchemeDetailCalculation = new SalarySchemeDetailCalculation();
            return PartialView(oSalarySchemeDetailCalculation);
        }

        #endregion

        #region IUD
        [HttpPost]
        public JsonResult SalaryScheme_IU(SalaryScheme oSalaryScheme)
        {

            _oSalaryScheme = new SalaryScheme();
            try
            {
                _oSalaryScheme = oSalaryScheme;
                _oSalaryScheme.NatureOfEmployee = (EnumEmployeeNature)oSalaryScheme.NatureOfEmployeeInt;
                _oSalaryScheme.PaymentCycle = (EnumPaymentCycle)oSalaryScheme.PaymentCycleInt;
                _oSalaryScheme.OverTimeON = (EnumOverTimeON)oSalaryScheme.OverTimeONInt;
                _oSalaryScheme.CompOverTimeON = (EnumOverTimeON)oSalaryScheme.CompOverTimeONInt;
                if (_oSalaryScheme.SalarySchemeID > 0)
                {
                    _oSalaryScheme = _oSalaryScheme.IUD((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oSalaryScheme = _oSalaryScheme.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult SalaryScheme_Delete(int nId, double ts)//nId=SalarySchemeID
        {
            _oSalaryScheme = new SalaryScheme();
            try
            {

                _oSalaryScheme.SalarySchemeID = nId;
                _oSalaryScheme = _oSalaryScheme.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult SalarySchemeDetail_Delete(int nId, double ts)//nId=SalarySchemeDetailID
        {
            SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
            string sFeedBackMessage = "";
            try
            {

                oSalarySchemeDetail.SalarySchemeDetailID = nId;
                sFeedBackMessage = oSalarySchemeDetail.Delete(((User)(Session[SessionInfo.CurrentUser])).UserID);

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
        public JsonResult SalarySchemeSave_V1(SalaryScheme oSalaryScheme)
        {

            _oSalaryScheme = new SalaryScheme();
            try
            {
                _oSalaryScheme = oSalaryScheme;
                _oSalaryScheme.NatureOfEmployee = (EnumEmployeeNature)oSalaryScheme.NatureOfEmployeeInt;
                _oSalaryScheme.PaymentCycle = (EnumPaymentCycle)oSalaryScheme.PaymentCycleInt;
                _oSalaryScheme.OverTimeON = (EnumOverTimeON)oSalaryScheme.OverTimeONInt;

                if (_oSalaryScheme.SalarySchemeID > 0)
                {
                    _oSalaryScheme = _oSalaryScheme.SalarySchemeSave((int)EnumDBOperation.Update, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    _oSalaryScheme = _oSalaryScheme.SalarySchemeSave((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveDetailAndCalculation_V1(SalarySchemeDetail oSalarySchemeDetail)
        {
            try
            {
                oSalarySchemeDetail = oSalarySchemeDetail.IUD((int)Session[SessionInfo.currentUserID]);
                List<SalarySchemeDetailCalculation> oCalculations = new List<SalarySchemeDetailCalculation>();
                oCalculations = SalarySchemeDetailCalculation.GetsBySalarySchemeDetail(oSalarySchemeDetail.SalarySchemeDetailID, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetail.Calculation = SalarySchemeDetail.GetEquation(oCalculations);
            }
            catch (Exception ex)
            {
                oSalarySchemeDetail = new SalarySchemeDetail();
                oSalarySchemeDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveDetailAndCalculation_V1ForGross(SalarySchemeDetail oSalarySchemeDetail)
        {
            try
            {
                oSalarySchemeDetail = oSalarySchemeDetail.IUDGross((int)Session[SessionInfo.currentUserID]);
                List<SalarySchemeDetailCalculation> oCalculations = new List<SalarySchemeDetailCalculation>();
                oCalculations = SalarySchemeDetailCalculation.GetsBySalarySchemeGross(oSalarySchemeDetail.SalarySchemeID, (int)Session[SessionInfo.currentUserID]);
                oSalarySchemeDetail.Calculation = SalarySchemeDetail.GetEquation(oCalculations);
            }
            catch (Exception ex)
            {
                oSalarySchemeDetail = new SalarySchemeDetail();
                oSalarySchemeDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemeDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region AdvanceSearch
        public ActionResult AdvanceSearch(double ts)//nId=SalarySchemeID
        {
            _oSalaryScheme = new SalaryScheme();
            ViewBag.EmployeeTypes = EmployeeType.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oSalaryScheme);
        }
        [HttpGet]
        public JsonResult GetsForAdvanceSearch(string sTemp, double ts)
        {

            List<SalaryScheme> oSalarySchemes = new List<SalaryScheme>();

            try
            {
                string Name = sTemp.Split('~')[0];
                int EmployeeTypeID = Convert.ToInt16(sTemp.Split('~')[1]);
                int NatureOfEmployeeInt = Convert.ToInt16(sTemp.Split('~')[2]);
                int PaymentCycleInt = Convert.ToInt16(sTemp.Split('~')[3]);
                int Activity = Convert.ToInt16(sTemp.Split('~')[4]);
                int PBase = Convert.ToInt16(sTemp.Split('~')[5]);

                string sReturn1 = "SELECT * FROM SalaryScheme";
                string sReturn = "";
                if (Name != "")
                {

                    Global.TagSQL(ref sReturn);
                    //sReturn = sReturn + " Name = '" + Name.ToString()+"'";
                    sReturn = sReturn + " Name = " + "'" + Name + "'";

                }
                if (EmployeeTypeID > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " EmployeeTypeID = " + EmployeeTypeID.ToString();

                }
                if (NatureOfEmployeeInt > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " NatureOfEmployee = " + NatureOfEmployeeInt.ToString();

                }
                if (PaymentCycleInt > 0)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " PaymentCycle = " + PaymentCycleInt.ToString();

                }

                if (Activity >= 0 && Activity < 2)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IsActive = " + Activity;

                }

                if (PBase == 1)
                {

                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IsProductionBase = " + PBase;

                }

                string sSQL = sReturn1 + sReturn;
                oSalarySchemes = SalaryScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalarySchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SalarySchemeSearchByName(string sSSName, double nts)
        {
            _oSalarySchemes = new List<SalaryScheme>();
            _oSalaryScheme = new SalaryScheme();
            try
            {
                string sSql = "";
                if (sSSName == "")
                {
                    sSql = "SELECT * FROM SalaryScheme";
                }
                else
                {
                    sSql = "SELECT * FROM SalaryScheme WHERE Name LIKE '%" + sSSName + "%' ";
                }
                _oSalarySchemes = SalaryScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oSalarySchemes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oSalarySchemes = new List<SalaryScheme>();
                _oSalaryScheme.ErrorMessage = ex.Message;
                _oSalarySchemes.Add(_oSalaryScheme);
            }
            return PartialView(_oSalarySchemes);
        }

        #endregion

        #region LoadEnumSalaryHeadType

        public class EnumLoad
        {
            public EnumLoad()
            {
                int Id = 0;
                string Value = "";

            }


            public int Id { get; set; }
            public string Value { get; set; }
            public List<EnumLoad> EnumLoads { get; set; }
        }

        [HttpGet]
        public JsonResult LoadAllowanceType()
        {
            List<EnumLoad> oEnumLoads = new List<EnumLoad>();
            EnumLoad oEnumLoad = new EnumLoad();
            try
            {
                foreach (int oItem in Enum.GetValues(typeof(EnumSalaryHeadType)))
                {

                    if (oItem != 1)
                    {
                        oEnumLoad = new EnumLoad();
                        oEnumLoad.Id = oItem;
                        oEnumLoad.Value = ((EnumSalaryHeadType)oItem).ToString();
                        oEnumLoads.Add(oEnumLoad);
                    }

                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oEnumLoads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Activity
        [HttpPost]
        public JsonResult SalaryScheme_Activity(SalaryScheme oSalaryScheme)
        {
            _oSalaryScheme = new SalaryScheme();
            try
            {

                _oSalaryScheme = oSalaryScheme;
                _oSalaryScheme.IsActive = !_oSalaryScheme.IsActive;
                _oSalaryScheme = SalaryScheme.Activite(_oSalaryScheme.SalarySchemeID, _oSalaryScheme.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region CopyFromOtherGrade
        [HttpGet]
        public JsonResult CopyFromOtherGrade(int nId)//nId=SalarySchemeID
        {

            SalaryScheme oSalarySchemes = new SalaryScheme();
            try
            {

                _oSalaryScheme = new SalaryScheme();
                List<SalaryHead> oSalaryHeads = new List<SalaryHead>();

                List<SalarySchemeDetail> oSalarySchemeDetails = new List<SalarySchemeDetail>();


                _oSalaryScheme = SalaryScheme.Get(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.Gets(nId, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                string sSql1 = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + nId + ") ORDER BY SalarySchemeDetailID ";

                _oSalaryScheme.SalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSql1, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(_oSalaryScheme.SalarySchemeDetails, _oSalaryScheme.SalarySchemeDetailCalculations);

                string sSql = "SELECT * FROM SalaryHead WHERE SalaryHeadType=1 AND SalaryHeadID NOT IN (SELECT SalaryHeadID FROM SalarySchemeDetail WHERE SalarySchemeID=" + nId + ")";
                oSalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach (SalaryHead oitem in oSalaryHeads)
                {
                    SalarySchemeDetail oSalarySchemeDetail = new SalarySchemeDetail();
                    oSalarySchemeDetail.SalaryHeadID = oitem.SalaryHeadID;
                    oSalarySchemeDetail.SalaryHeadName = oitem.Name;
                    oSalarySchemeDetail.SalaryHeadType = oitem.SalaryHeadType;
                    _oSalaryScheme.SalarySchemeDetails.Add(oSalarySchemeDetail);
                }


                
            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Employee Information
        public JsonResult GetsBySalarySchemeName(SalaryScheme oSalaryScheme)
        {

            _oSalarySchemes = new List<SalaryScheme>();
            _oSalaryScheme = new SalaryScheme();
            try
            {
                string sSql = "";
                string sSSName = oSalaryScheme.Name;
                if (sSSName == "")
                {
                    sSql = "SELECT * FROM SalaryScheme Where SalarySchemeID<>0 And IsActive=1";
                }
                else
                {
                    sSql = "SELECT * FROM SalaryScheme WHERE SalarySchemeID<>0 And IsActive=1 And Name LIKE '%" + sSSName + "%' ";
                }

                _oSalarySchemes = SalaryScheme.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (_oSalarySchemes.Count <= 0)
                {
                    throw new Exception("Data Not Found !");
                }
            }
            catch (Exception ex)
            {
                _oSalarySchemes = new List<SalaryScheme>();
                _oSalaryScheme.ErrorMessage = ex.Message;
                _oSalarySchemes.Add(_oSalaryScheme);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalarySchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion Employee Information

        #region Production Bonus
        [HttpPost]
        public JsonResult ProductionBonus_IUD(ProductionBonus oProductionBonus)
        {
            ProductionBonus oPBs = new ProductionBonus();
            try
            {
                oPBs = oProductionBonus;
                if (oPBs.ProductionBonusID > 0)
                {
                    oPBs = oPBs.IUD((int)EnumDBOperation.Delete, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                else
                {
                    oPBs = oPBs.IUD((int)EnumDBOperation.Insert, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

            }
            catch (Exception ex)
            {
                oPBs = new ProductionBonus();
                oPBs.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPBs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Activity_ProductionBonus
        [HttpPost]
        public JsonResult ProductionBonus_Activity(ProductionBonus oProductionBonus)
        {
            
            try
            {
                oProductionBonus = ProductionBonus.ActivityStatus(oProductionBonus.ProductionBonusID, oProductionBonus.IsActive, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                oProductionBonus = new ProductionBonus();
                oProductionBonus.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProductionBonus);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Get Salary Scheme Info
        [HttpPost]
        public JsonResult GetSalarySchemeInfo(SalaryScheme oSalaryScheme)
        {

            try
            {
                if (oSalaryScheme.SalarySchemeID > 0)
                {
                    oSalaryScheme = GetForPartialView(oSalaryScheme.SalarySchemeID);
                    string sSQL = "";
                    sSQL = "SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID IN (SELECT SalarySchemeDetailID FROM  SalarySchemeDetail WHERE SalarySchemeID=" + oSalaryScheme.SalarySchemeID + ") ORDER BY SalarySchemeDetailID ";
                    oSalaryScheme.SalarySchemeDetailCalculations = SalarySchemeDetailCalculation.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                    oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.GetNewSalarySchemeDetail(oSalaryScheme.SalarySchemeDetails, oSalaryScheme.SalarySchemeDetailCalculations);
                    sSQL = "SELECT * FROM ProductionBonus WHERE SalarySchemeID=" + oSalaryScheme.SalarySchemeID;
                    oSalaryScheme.SalarySchemeDetails = oSalaryScheme.SalarySchemeDetails.OrderBy(x => x.SalaryHeadTypeInt).ToList();
                    oSalaryScheme.ProductionBonuss = ProductionBonus.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }

            }
            catch (Exception ex)
            {
                oSalaryScheme = new SalaryScheme();
                oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(SalaryScheme oSalaryScheme)
        {

            try
            {
                if (oSalaryScheme.SalarySchemeID > 0)
                {
                    oSalaryScheme = SalaryScheme.Get(oSalaryScheme.SalarySchemeID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
            }
            catch (Exception ex)
            {
                oSalaryScheme = new SalaryScheme();
                oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        
        [HttpGet]
        public JsonResult GetsSchemeByName(string sName, double nts)
        {
            SalaryScheme oSalaryScheme = new SalaryScheme();
            _oSalarySchemes = new List<SalaryScheme>();

            try
            {
                string sSQL = "";
                sSQL = "SELECT * FROM SalaryScheme WHERE Name LIKE '%" + sName + "%'";

                _oSalarySchemes = SalaryScheme.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oSalaryScheme = new SalaryScheme();
                oSalaryScheme.ErrorMessage = ex.Message;
                _oSalarySchemes.Add(oSalaryScheme);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalarySchemes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Copy(SalaryScheme oSalaryScheme)
        {
            SalarySchemeDetail oSSD = new SalarySchemeDetail();
            List<SalarySchemeDetail> oSSDs = new List<SalarySchemeDetail>();
            SalarySchemeDetailCalculation oSSDC = new SalarySchemeDetailCalculation();
            List<SalarySchemeDetailCalculation> oSSDCs = new List<SalarySchemeDetailCalculation>();
            try
            {
                int nBaseID = oSalaryScheme.SalarySchemeID;
                //Salary Scheme
                _oSalaryScheme = oSalaryScheme;
                _oSalaryScheme = SalaryScheme.Get(oSalaryScheme.SalarySchemeID, (int)Session[SessionInfo.currentUserID]);
                _oSalaryScheme.SalarySchemeID = 0;
                _oSalaryScheme.Name += "-1";

                _oSalaryScheme = _oSalaryScheme.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                
                //Salary Scheme Detail
                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.Gets(nBaseID, (int)Session[SessionInfo.currentUserID]);

                _oSalaryScheme.SalarySchemeDetails = _oSalaryScheme.SalarySchemeDetails.OrderBy(x => x.SalarySchemeDetailID).ToList();

                foreach (SalarySchemeDetail oitem in _oSalaryScheme.SalarySchemeDetails)
                {
                    oitem.SalarySchemeDetailID = 0;
                    oitem.SalarySchemeID = _oSalaryScheme.SalarySchemeID;
                }
                string sDetailIDs = "";
                foreach (SalarySchemeDetail oitem in _oSalaryScheme.SalarySchemeDetails)
                {
                    oSSD = oitem.IUD((int)Session[SessionInfo.currentUserID]);
                    sDetailIDs += oSSD.SalarySchemeDetailID + ","; 
                }

                _oSalaryScheme.SalarySchemeDetails = SalarySchemeDetail.Gets(nBaseID, (int)Session[SessionInfo.currentUserID]);

                string[] sIDs = new string[_oSalaryScheme.SalarySchemeDetails.Count];
                int[] nIDs = new int[_oSalaryScheme.SalarySchemeDetails.Count];

                if(sDetailIDs.Length > 0) {
                    sDetailIDs = sDetailIDs.Substring(0, sDetailIDs.Length - 1);
                    sIDs = sDetailIDs.Split(',');
                    nIDs = Array.ConvertAll(sIDs, int.Parse);
                }


                //Salary Scheme Detail Calculation
                int nCounter = 0;
                _oSalaryScheme.SalarySchemeDetails = _oSalaryScheme.SalarySchemeDetails.OrderBy(x=>x.SalarySchemeDetailID).ToList();
                foreach (SalarySchemeDetail oitem in _oSalaryScheme.SalarySchemeDetails)
                {
                    oSSDCs = SalarySchemeDetailCalculation.Gets("SELECT * FROM View_SalarySchemeDetailCalculation WHERE SalarySchemeDetailID=" + oitem.SalarySchemeDetailID, (int)Session[SessionInfo.currentUserID]);
                    foreach (SalarySchemeDetailCalculation obj in oSSDCs)
                    {
                        obj.SSDCID = 0;
                        obj.SalarySchemeDetailID = nIDs[nCounter];
                        oSSDC = obj.IUD((int)EnumDBOperation.Insert, (int)Session[SessionInfo.currentUserID]);
                    }
                    ++nCounter;
                }

            }
            catch (Exception ex)
            {
                _oSalaryScheme = new SalaryScheme();
                _oSalaryScheme.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryScheme);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


    }
}
