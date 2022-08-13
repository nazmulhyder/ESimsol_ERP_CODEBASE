using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class FabricMachineGroupController : Controller
    {
        #region Declaration
        FabricMachineGroup _oFabricMachineGroup = new FabricMachineGroup();
        List<FabricMachineGroup> _oFabricMachineGroups = new List<FabricMachineGroup>();
        #endregion
        [HttpGet]
        public ActionResult ViewFabricMachineGroups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_FabricMachineGroup  AS HH ORDER BY HH.FabricMachineGroupID ASC";
            _oFabricMachineGroups = FabricMachineGroup.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oFabricMachineGroups);
        }

        [HttpPost]
        public JsonResult Save(FabricMachineGroup oFabricMachineGroup)
        {
            _oFabricMachineGroup = new FabricMachineGroup();
            try
            {
                _oFabricMachineGroup = oFabricMachineGroup;
                _oFabricMachineGroup = _oFabricMachineGroup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricMachineGroup = new FabricMachineGroup();
                _oFabricMachineGroup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachineGroup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricMachineGroup oFabricMachineGroup = new FabricMachineGroup();
                sFeedBackMessage = oFabricMachineGroup.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Print List

        public ActionResult SetSessionSearchCriterias(FabricMachineGroup oFabricMachineGroup)
        {
            //this.Session.Remove(SessionInfo.ParamObj);
            //this.Session.Add(SessionInfo.ParamObj, oFabricMachineGroup);

            //JavaScriptSerializer serializer = new JavaScriptSerializer();
            //string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            //return Json(sjson, JsonRequestBehavior.AllowGet);
            throw new NotImplementedException();
        }

        public ActionResult PrintLists()
        {
            //_oFabricMachineGroup = new FabricMachineGroup();
            //_oFabricMachineGroups = new List<FabricMachineGroup>();
            //_oFabricMachineGroup = (FabricMachineGroup)Session[SessionInfo.ParamObj];

            //string sSQL = "SELECT * FROM View_FabricMachineGroup AS HH WHERE HH.FabricMachineGroupID IN (" + _oFabricMachineGroup.ErrorMessage + ") ORDER BY HH.FabricMachineGroupID ASC";
            //_oFabricMachineGroups = FabricMachineGroup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            //Company oCompany = new Company();
            //oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            //rptFabricMachineGroups oReport = new rptFabricMachineGroups();
            //byte[] abytes = oReport.PrepareReport(_oFabricMachineGroups, oCompany, "");
            //return File(abytes, "application/pdf");
            throw new NotImplementedException();
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

    }
        #endregion
}