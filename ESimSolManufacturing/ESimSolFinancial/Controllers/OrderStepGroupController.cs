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
	public class OrderStepGroupController : Controller
	{
		#region Declaration

		OrderStepGroup _oOrderStepGroup = new OrderStepGroup();
		List<OrderStepGroup> _oOrderStepGroups = new  List<OrderStepGroup>();
        List<OrderStepGroupDetail>  _oOrderStepGroupDetails = new List<OrderStepGroupDetail>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewOrderStepGroups(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderStepGroup).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oOrderStepGroups = new List<OrderStepGroup>(); 

			_oOrderStepGroups = OrderStepGroup.Gets(buid,(int)Session[SessionInfo.currentUserID]);
			return View(_oOrderStepGroups);
		}

		public ActionResult ViewOrderStepGroup(int id)
		{
			_oOrderStepGroup = new OrderStepGroup();
			if (id > 0)
			{
				_oOrderStepGroup = _oOrderStepGroup.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderStepGroup.OrderStepGroupDetails = OrderStepGroupDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oOrderStepGroup);
		}

		[HttpPost]
		public JsonResult Save(OrderStepGroup oOrderStepGroup)
		{
			_oOrderStepGroup = new OrderStepGroup();
			try
			{
				_oOrderStepGroup = oOrderStepGroup;
				_oOrderStepGroup = _oOrderStepGroup.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oOrderStepGroup = new OrderStepGroup();
				_oOrderStepGroup.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oOrderStepGroup);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				OrderStepGroup oOrderStepGroup = new OrderStepGroup();
				sFeedBackMessage = oOrderStepGroup.Delete(id, (int)Session[SessionInfo.currentUserID]);
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

        #region HTTP functions
        [HttpPost]
        public JsonResult UpDown(OrderStepGroupDetail oOrderStepGroupDetail)
        {
            _oOrderStepGroup = new OrderStepGroup();
            _oOrderStepGroupDetails = new List<OrderStepGroupDetail>();
            try
            {
                _oOrderStepGroup = _oOrderStepGroup.UpDown(oOrderStepGroupDetail, (int)Session[SessionInfo.currentUserID]);//Save
                _oOrderStepGroupDetails = OrderStepGroupDetail.Gets(oOrderStepGroupDetail.OrderStepGroupID, (int)Session[SessionInfo.currentUserID]);//Save

            }
            catch (Exception ex)
            {
                _oOrderStepGroup = new OrderStepGroup();
                _oOrderStepGroup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderStepGroupDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

    }

}
