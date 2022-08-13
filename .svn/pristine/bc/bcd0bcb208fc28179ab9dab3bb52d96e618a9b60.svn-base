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

namespace ESimSolFinancial.Controllers
{
	public class NOASignatoryController : Controller
	{
		#region Declaration

		NOASignatory _oNOASignatory = new NOASignatory();
		List<NOASignatory> _oNOASignatorys = new  List<NOASignatory>();
        List<NOASignatoryComment> _oNOASignatoryComments = new List<NOASignatoryComment>();
		#endregion
		#region Functions
		#endregion
		#region Actions
        public ActionResult ViewNOASignatorys(int id, double ts) 
		{
            NOA oNOA = new NOA(); 
            _oNOASignatorys = new List<NOASignatory>();
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            List<ApprovalHeadPerson> oApprovalHeadPersons = new List<ApprovalHeadPerson>();
            oNOA = oNOA.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oNOASignatorys = NOASignatory.Gets(id, (int)Session[SessionInfo.currentUserID]);
            if (_oNOASignatorys.Count == 0)
            {
                _oNOASignatorys = NOASignatory.Gets("SELECT * FROM view_NOASignatory WHERE NOAID in (Select max(NOAID) from NOARequisition where  NOAID in (SELECT NOAID FROM NOASignatory) and NOAID!=" + oNOA.NOAID + " and PRID in (Select PurchaseRequisition.PRID from PurchaseRequisition where DepartmentID in (Select PurchaseRequisition.DepartmentID from PurchaseRequisition where PRID in (Select PRID from NOARequisition where NOAID=" + oNOA.NOAID + "))))", (int)Session[SessionInfo.currentUserID]);

                if (_oNOASignatorys.Count == 0)
                {
                    oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.NOA + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
                    //oApprovalHeadPersons = ApprovalHeadPerson.Gets("SELECT * FROM View_ApprovalHeadPerson WHERE ApprovalHeadID in (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.NOA + " ) and UserID not in (SELECT RequestTo FROM NOASignatory WHERE NOAID=" + id + ")", (int)Session[SessionInfo.currentUserID]);
                    foreach (ApprovalHead oitem in oApprovalHeads)
                    {
                        _oNOASignatory = new NOASignatory();
                        _oNOASignatory.NOAID = id;
                        _oNOASignatory.ApprovalHeadID = oitem.ApprovalHeadID;
                        if (oitem.Sequence == 1)
                        {
                            _oNOASignatory.RequestTo = (int)Session[SessionInfo.currentUserID];
                            _oNOASignatory.Name_Request = (string)Session[SessionInfo.currentUserName];
                        }
                        _oNOASignatory.ApproveDate = DateTime.MinValue;
                        //_oNOASignatory.SLNo = oitem.ApprovalHeadID;
                        _oNOASignatory.SLNo = oitem.Sequence;
                        _oNOASignatory.HeadName = oitem.Name;
                        _oNOASignatorys.Add(_oNOASignatory);
                    }
                }
                else
                {
                    foreach (NOASignatory oitem in _oNOASignatorys)
                    {
                        oitem.NOASignatoryID = 0;
                        oitem.NOAID = oNOA.NOAID;
                        oitem.ApproveDate = DateTime.MinValue;
                        oitem.ApproveBy = 0;
                        oitem.IsApprove = false;

                    }

                }
            }
            _oNOASignatorys = _oNOASignatorys.OrderBy(x => x.SLNo).ToList();
            ViewBag.NOA = oNOA;
			return View(_oNOASignatorys);
		}
		public ActionResult ViewNOASignatory(int id)
		{
			_oNOASignatory = new NOASignatory();
			if (id > 0)
			{
				_oNOASignatory = _oNOASignatory.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oNOASignatory);
		}
		[HttpPost]
        public JsonResult SaveAll(List<NOASignatory> oNOASignatorys)
		{
            _oNOASignatorys = new List<NOASignatory>();
			try
			{
                string sNOASignatoryIDs = string.Join(",", oNOASignatorys.Where(p=>p.NOASignatoryID>0).ToList().Select(x => x.NOASignatoryID).ToList());
                if (string.IsNullOrEmpty(sNOASignatoryIDs)) { sNOASignatoryIDs = "0"; }

                _oNOASignatorys = NOASignatory.SaveAll(oNOASignatorys, sNOASignatoryIDs, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oNOASignatory = new NOASignatory();
				_oNOASignatory.ErrorMessage = ex.Message;
                _oNOASignatorys = new List<NOASignatory>();
                _oNOASignatorys.Add(_oNOASignatory);
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOASignatorys);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult Save(NOASignatory oNOASignatory)
        {
            try
            {
                _oNOASignatory = oNOASignatory.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oNOASignatory = new NOASignatory();
                _oNOASignatory.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOASignatory);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveNOASIGComment(NOASignatoryComment oNOASignatoryComment)
        {
            //_oNOASignatory = new NOASignatory();
            try
            {
                //_oNOASignatory = oNOASignatory;
                oNOASignatoryComment = oNOASignatoryComment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oNOASignatoryComment = new NOASignatoryComment();
                oNOASignatoryComment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNOASignatoryComment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveNOASIGCommentAll(List<NOASignatoryComment> oSCs)
        {
            List<NOASignatoryComment> oNOASignatoryComments = new List<NOASignatoryComment>();
            try
            {
                oNOASignatoryComments = NOASignatoryComment.SaveAll(oSCs, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                NOASignatoryComment oNOASignatoryComment = new NOASignatoryComment();
                oNOASignatoryComment.ErrorMessage = ex.Message;
                oNOASignatoryComments = new List<NOASignatoryComment>();
                oNOASignatoryComments.Add(oNOASignatoryComment);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNOASignatoryComments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(NOASignatory oNOASignatory)
		{
			string sFeedBackMessage = "";
			try
			{
				sFeedBackMessage = oNOASignatory.Delete( (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetByNOADetail(NOASignatoryComment oNOASignatoryComment)
        {
            List<NOAQuotation> oNOAQuotations = new List<NOAQuotation>();
            NOADetail oNOADetail = new NOADetail();
            NOASignatoryComment oNOASigComment = new NOASignatoryComment();
            try
            {
                if (oNOASignatoryComment.NOADetailID <= 0)
                    throw new Exception("Please select a valid item.");
                oNOADetail = oNOADetail.Get(oNOASignatoryComment.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oNOASignatoryComments = NOASignatoryComment.Gets(oNOASignatoryComment.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oNOAQuotations = NOAQuotation.Gets(oNOASignatoryComment.NOADetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                foreach (NOASignatoryComment oItem in _oNOASignatoryComments)
                {
                    if (oItem.NOASignatoryID >0 && oItem.NOASignatoryID == oNOASignatoryComment.NOASignatoryID)
                    { 
                        oNOASigComment.NOASignatoryID = oItem.NOASignatoryID;
                        oNOASigComment.Comment = oItem.Comment;
                        oNOASigComment.NOASignatoryCommentID = oItem.NOASignatoryCommentID;
                        oNOASigComment.NOADetailID = oItem.NOADetailID;
                        oNOASigComment.PQDetailID = oItem.PQDetailID;
                        oNOASigComment.SupplierName = oItem.SupplierName;
                        oNOASigComment.Name = oItem.Name;
                        oNOASigComment.UnitPrice = oItem.UnitPrice;
                        oNOASigComment.DBServerDateTime = oItem.DBServerDateTime;
                        oNOASigComment.NOASignatoryID = oItem.NOASignatoryID;
                    }
                }
                //if (_oNOASignatoryComments.Count > 0)
                //{
                //    oNOASigComment = _oNOASignatoryComments.Where(x => x.NOASignatoryID == oNOASignatoryComment.NOASignatoryID).FirstOrDefault();
                //}
                oNOASigComment.NOAQuotations = new List<NOAQuotation>();
                oNOASigComment.NOASignatoryComments = new List<NOASignatoryComment>();
                oNOASigComment.NOASignatoryComments = _oNOASignatoryComments;
                oNOASigComment.NOAQuotations = oNOAQuotations;
                oNOASigComment.ProductName = oNOADetail.ProductName;
            }
            catch (Exception ex)
            {
                oNOASigComment = new NOASignatoryComment();
                _oNOASignatoryComments = new List<NOASignatoryComment>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oNOASigComment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetByNOASignatory(NOASignatory oNOASignatory)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            _oNOASignatorys = new List<NOASignatory>();
            try
            {
                _oNOASignatorys = NOASignatory.Gets(oNOASignatory.NOAID, (int)Session[SessionInfo.currentUserID]);
                string sApprovalHeadIDs = string.Join(",", _oNOASignatorys.Where(p => p.ApprovalHeadID > 0).ToList().Select(x => x.ApprovalHeadID).ToList());
                if (string.IsNullOrEmpty(sApprovalHeadIDs)) { sApprovalHeadIDs = "0"; }
                oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ApprovalHeadID not in (" + sApprovalHeadIDs + ") and ModuleID = " + (int)EnumModuleName.NOA + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
                foreach (ApprovalHead oitem in oApprovalHeads)
                {
                    _oNOASignatory = new NOASignatory();
                    _oNOASignatory.NOAID = oNOASignatory.NOAID;
                    _oNOASignatory.RequestTo =0;
                    _oNOASignatory.Name_Request ="";
                    _oNOASignatory.ApproveDate = DateTime.MinValue;
                    _oNOASignatory.ApprovalHeadID = oitem.ApprovalHeadID;
                    _oNOASignatory.HeadName = oitem.Name;
                    _oNOASignatory.SLNo = oitem.Sequence;  // oApprovalHeads.Where(p => p.ApprovalHeadID == oitem.ApprovalHeadID).FirstOrDefault().Sequence;
                    _oNOASignatorys.Add(_oNOASignatory);
                }
                _oNOASignatorys = _oNOASignatorys.OrderBy(x => x.SLNo).ToList();
            }
            catch (Exception ex)
            {
                //oNOASignatoryComment = new NOASignatoryComment();
                _oNOASignatorys = new List<NOASignatory>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oNOASignatorys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsApprovalHead(ApprovalHead oApprovalHead)
        {
            List<ApprovalHead> oApprovalHeads = new List<ApprovalHead>();
            try
            {
                 oApprovalHeads = ApprovalHead.Gets("SELECT * FROM ApprovalHead WHERE ModuleID = " + (int)EnumModuleName.NOA + " ORDER BY Sequence", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oApprovalHeads = new List<ApprovalHead>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oApprovalHeads);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetUesrsByName(User oUser)
        {
            List<User> oUsers = new List<User>();
            try
            {
                string sSQL = "Select * from View_User Where UserID<>0 ";
                if (!String.IsNullOrEmpty(oUser.UserName))
                {
                    oUser.UserName = oUser.UserName.Trim();
                    sSQL = sSQL+"And UserName+LogInID like '%" + oUser.UserName + "%'";
                }
                else
                {
                    sSQL =sSQL+ "and UserID in (SELECT UserID FROM ApprovalHeadPerson WHERE ApprovalHeadID in (SELECT ApprovalHeadID FROM ApprovalHead WHERE ModuleID =" + (int)EnumModuleName.NOA + "))";
                }
                oUsers = ESimSol.BusinessObjects.User.GetsBySql(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oUsers.Count <= 0)
                {
                    throw new Exception("No information found");
                }
            }
            catch (Exception ex)
            {
                oUsers = new List<User>();
                oUser.ErrorMessage = ex.Message;
                oUsers.Add(oUser);

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oUsers);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

		#endregion

	}

}
