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
    public class LeaveHeadController : Controller
    {
        #region Declaration

        private LeaveHead _oLeaveHead = new LeaveHead();
        private List<LeaveHead> _oLeaveHeads = new List<LeaveHead>();
        private string _sErrorMessage = "";

        #endregion

        public ActionResult ViewLeaveHeads(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oLeaveHeads = new List<LeaveHead>();
            _oLeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);


            ViewBag.LHRuleValueType = EnumObject.jGets(typeof(EnumLHRuleValueType));
            ViewBag.LHRuleType = EnumObject.jGets(typeof(EnumLHRuleType));
            ViewBag.LHRule = new LHRule();
            return View(_oLeaveHeads);
        }

        public ActionResult ViewLeaveHead(int id, double ts)
        {
            _oLeaveHead = new LeaveHead();
            if (id > 0)
            {
                _oLeaveHead = _oLeaveHead.Get(id, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            return PartialView(_oLeaveHead);
        }

        [HttpPost]
        public JsonResult Save(LeaveHead oLeaveHead)
        {
            _oLeaveHead = new LeaveHead();
            try
            {
                _oLeaveHead = oLeaveHead;
                _oLeaveHead = _oLeaveHead.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oLeaveHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(LeaveHead oLeaveHead)
        {
            string sFeedBackMessage = "";
            try
            {
                 _oLeaveHead = new LeaveHead();
                 sFeedBackMessage = oLeaveHead.Delete(oLeaveHead.LeaveHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
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
        public JsonResult ChangeActiveStatus(LeaveHead oLeaveHead)
        {
            _oLeaveHead = new LeaveHead();
            string sMsg;

            sMsg = _oLeaveHead.ChangeActiveStatus(oLeaveHead, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            _oLeaveHead = _oLeaveHead.Get(oLeaveHead.LeaveHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PickLeaveHead(double ts)
        {
            _oLeaveHead = new LeaveHead();
            _oLeaveHead.LeaveHeads = LeaveHead.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);

            return PartialView(_oLeaveHead);
        }

        [HttpPost]
        public JsonResult getLeaveHead(LeaveHead oLeaveHead)
        {
            try
            {
                _oLeaveHead = new LeaveHead();
                List<LHRuleDetail> oLHRuleDetails = new List<LHRuleDetail>();
                _oLeaveHead = _oLeaveHead.Get(oLeaveHead.LeaveHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                _oLeaveHead.LHRules = LHRule.Gets("SELECT * FROM View_LHRule WHERE LeaveHeadID = " + _oLeaveHead.LeaveHeadID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                oLHRuleDetails = LHRuleDetail.Gets("SELECT * FROM View_LHRuleDetail WHERE LHRuleID IN (SELECT LHRuleID From LHRule WHERE LeaveHeadID = " + _oLeaveHead.LeaveHeadID + ") ORDER BY Sequence", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                foreach(LHRuleDetail obj in oLHRuleDetails)
                {
                    foreach(LHRule temp in _oLeaveHead.LHRules)
                    {
                        if(obj.LHRuleID == temp.LHRuleID)
                        {
                            temp.LHRuleTypeDescription = temp.LHRuleTypeDescription + obj.LHRuleValue + " ";
                            temp.LHRuleDetails.Add(obj);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _oLeaveHeads = new List<LeaveHead>();
                _oLeaveHead.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLeaveHead);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}