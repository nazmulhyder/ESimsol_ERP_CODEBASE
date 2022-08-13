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
    public class SalaryHeadController : Controller
    {
        #region Declaration

        private SalaryHead _oSalaryHead = new SalaryHead();
        private List<SalaryHead> _oSalaryHeads = new List<SalaryHead>();
        private string _sErrorMessage = "";

        #endregion

        public ActionResult ViewSalaryHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oSalaryHeads = new List<SalaryHead>();
            _oSalaryHeads = SalaryHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ClientOperationSetting oTempClientOperationSetting = new ClientOperationSetting();
            oTempClientOperationSetting = oTempClientOperationSetting.GetByOperationType((int)EnumOperationType.BanglaFont, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.COS = oTempClientOperationSetting;
            return View(_oSalaryHeads);
        }

        public ActionResult ViewSalaryHead(int id, double ts)
        {
            _oSalaryHead = new SalaryHead();
            if (id > 0)
            {
                _oSalaryHead = _oSalaryHead.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oSalaryHead);
        }

        [HttpPost]
        public JsonResult Save(SalaryHead oSalaryHead)
        {
            _oSalaryHead = new SalaryHead();
            try
            {
                _oSalaryHead = oSalaryHead;
                _oSalaryHead.SalaryHeadType = (EnumSalaryHeadType)oSalaryHead.SalaryHeadTypeInt;
                _oSalaryHead = _oSalaryHead.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oSalaryHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpDown(SalaryHead oSalaryHead)
        {
            SalaryHead oSH = new SalaryHead();
            List<SalaryHead> oSalaryHeads = new List<SalaryHead>();
            try
            {
                oSH = oSH.UpDown(oSalaryHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oSalaryHead = new SalaryHead();
                _oSalaryHead.ErrorMessage = ex.Message;
            }
            if (oSH.ErrorMessage == "")
            {
                try
                {
                    string sSql = "SELECT * from SalaryHead Order By Sequence";
                    oSalaryHeads = SalaryHead.Gets(sSql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                catch (Exception ex)
                {
                    _oSalaryHead = new SalaryHead();
                    _oSalaryHead.ErrorMessage = ex.Message;
                }
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oSalaryHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(SalaryHead oSalaryHead)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oSalaryHead.Delete(oSalaryHead.SalaryHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        /*
         SalaryHeadType = 1 : Get All SalaryHeads
         SalaryHeadType = 2 : Get All Basic SalaryHeads
         SalaryHeadType = 3 : Get All Addition SalaryHeads
         SalaryHeadType = 4 : Get All Deduction SalaryHeads
         SalaryHeadType = 5 : Get All Allowance
        */
        public ActionResult PickSalaryHead(int SalaryHeadType, double ts)
        {
            _oSalaryHead = new SalaryHead();
            //if (SalaryHeadType == 1)
            //{
            //    _oSalaryHead.oSalaryHeads = SalaryHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}
            if (SalaryHeadType == 1)
            {
                _oSalaryHead.oSalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=" + SalaryHeadType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            //if (SalaryHeadType == 3)
            //{
            //    _oSalaryHead.oSalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=" + SalaryHeadType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}
            //if (SalaryHeadType == 4)
            //{
            //    _oSalaryHead.oSalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=" + SalaryHeadType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}
            //if (SalaryHeadType == 5)
            //{
            //    _oSalaryHead.oSalaryHeads = SalaryHead.Gets("SELECT * FROM SalaryHead WHERE SalaryHeadType=" + SalaryHeadType, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            //}
            //RosterPlanDetail.Gets("SELECT * FROM View_RosterPlanDetail WHERE RosterPlanID=" + nid, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            return PartialView(_oSalaryHead);
        }
        [HttpPost]
        public JsonResult ChangeActiveStatus(SalaryHead oSalaryHead)
        {
            _oSalaryHead = new SalaryHead();
            string sMsg;

            sMsg = _oSalaryHead.ChangeActiveStatus(oSalaryHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oSalaryHead = _oSalaryHead.Get(oSalaryHead.SalaryHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GetAllowance

        [HttpGet]
        public JsonResult LoadAllowance(int Id)//Id=Index of salary Head Type
        {
            try
            {
                string Ssql = "SELECT * FROM SalaryHead WHERE SalaryHeadType=" + Id;
                _oSalaryHeads = new List<SalaryHead>();
                _oSalaryHeads = SalaryHead.Gets(Ssql, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oSalaryHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult getSalaryHead(SalaryHead oSalaryHead)
        {
            try
            {
                _oSalaryHead = new SalaryHead();
                _oSalaryHead = _oSalaryHead.Get(oSalaryHead.SalaryHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            }
            catch (Exception ex)
            {
                _oSalaryHeads = new List<SalaryHead>();
                _oSalaryHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSalaryHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}