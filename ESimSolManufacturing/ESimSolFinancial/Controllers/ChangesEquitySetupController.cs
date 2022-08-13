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


namespace ESimSolFinancial.Controllers
{
    public class ChangesEquitySetupController : Controller
    {
        #region Declaration
        ChangesEquitySetup _oChangesEquitySetup = new ChangesEquitySetup();
        List<ChangesEquitySetup> _oChangesEquitySetups = new List<ChangesEquitySetup>();
        List<ChangesEquitySetupDetail> _oChangesEquitySetupDetails = new List<ChangesEquitySetupDetail>();
        #endregion

        #region Functions
        private void MakeSQL(string Arguments)
        {
            //_sSQL = "";
            //_sSQL = "SELECT * FROM View_ChangesEquitySetup";
            //string sCode = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            //string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            ////string sChangesEquitySetupIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            //string sSQL = "";


            //#region Code
            //if (sCode != null)
            //{
            //    if (sCode != "")
            //    {
            //        Global.TagSQL(ref sSQL);
            //        sSQL = sSQL + " Code LIKE '%" + sCode + "%' ";
            //    }
            //}
            //#endregion
            //#region Name
            //if (sName != null)
            //{
            //    if (sName != "")
            //    {
            //        Global.TagSQL(ref sSQL);
            //        sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
            //    }
            //}
            //#endregion

            //if (sSQL != "")
            //{ _sSQL = _sSQL + sSQL; }

        }
        #endregion

        #region Used Code
        public ActionResult ViewChangesEquitySetups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            
            _oChangesEquitySetups = new List<ChangesEquitySetup>();
            _oChangesEquitySetups = ChangesEquitySetup.Gets((int)Session[SessionInfo.currentUserID]);

            return View(_oChangesEquitySetups);
        }

        public ActionResult ViewChangesEquitySetup(int id)
        {
            _oChangesEquitySetup = new ChangesEquitySetup();
            if (id > 0)
            {
                _oChangesEquitySetup = _oChangesEquitySetup.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oChangesEquitySetup.ChangesEquitySetupDetails = ChangesEquitySetupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oChangesEquitySetup.EquityCategorys = EnumObject.jGets(typeof(EnumEquityCategory));
            return View(_oChangesEquitySetup);
        }

        [HttpPost]
        public JsonResult PrepareCEDs(ChangesEquitySetupDetail oChangesEquitySetupDetail)
        {
            List<ChangesEquitySetupDetail> oChangesEquitySetupDetails = new List<ChangesEquitySetupDetail>();
            oChangesEquitySetupDetail.AccountHeadName = oChangesEquitySetupDetail.AccountHeadName == null ? "" : oChangesEquitySetupDetail.AccountHeadName;

            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.ComponentID = 4;
            oChartsOfAccount.AccountType = EnumAccountType.Ledger;
            oChartsOfAccount.AccountHeadCodeName = oChangesEquitySetupDetail.AccountHeadName;
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            oChartsOfAccounts = ChartsOfAccount.GetsByComponentAndCodeName(oChartsOfAccount, ((User)Session[SessionInfo.CurrentUser]).UserID);

            foreach (ChartsOfAccount oItem in oChartsOfAccounts)
            {
                ChangesEquitySetupDetail oCED = new ChangesEquitySetupDetail();
                oCED.EffectedAccountID = oItem.AccountHeadID;
                oCED.AccountHeadName = oItem.AccountHeadName;
                oCED.AccountCode = oItem.AccountCode;
                oChangesEquitySetupDetails.Add(oCED);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oChangesEquitySetupDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
               
        [HttpPost]
        public JsonResult Get(ChangesEquitySetup oChangesEquitySetup)
        {
            _oChangesEquitySetup = new ChangesEquitySetup();
            try
            {
                if (oChangesEquitySetup.ChangesEquitySetupID > 0)
                {
                    _oChangesEquitySetup = _oChangesEquitySetup.Get(oChangesEquitySetup.ChangesEquitySetupID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oChangesEquitySetup = new ChangesEquitySetup();
                _oChangesEquitySetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChangesEquitySetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
            #endregion
        }
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        
        [HttpPost]
        public JsonResult Save(ChangesEquitySetup oChangesEquitySetup)
        {
            _oChangesEquitySetup = new ChangesEquitySetup();                        
            try
            {
                _oChangesEquitySetup = oChangesEquitySetup;
                _oChangesEquitySetup = _oChangesEquitySetup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oChangesEquitySetup = new ChangesEquitySetup();
                _oChangesEquitySetup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oChangesEquitySetup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ChangesEquitySetup oChangesEquitySetup)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oChangesEquitySetup.Delete(oChangesEquitySetup.ChangesEquitySetupID, (int)Session[SessionInfo.currentUserID]);
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



    }
}
