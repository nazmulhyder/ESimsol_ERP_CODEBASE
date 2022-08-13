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
    public class FnQCTestGroupController : Controller
    {
        #region Declaration

        FnQCTestGroup _oFnQCTestGroup = new FnQCTestGroup();
        List<FnQCTestGroup> _oFnQCTestGroups = new List<FnQCTestGroup>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewFnQCTestGroups(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FnQCTestGroup).ToString() + "," + ((int)EnumModuleName.FnQCTestGroup).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFnQCTestGroups = new List<FnQCTestGroup>();
            _oFnQCTestGroups = FnQCTestGroup.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oFnQCTestGroups);
        }

        public ActionResult ViewFnQCTestGroup(int id, int buid)
        {
            _oFnQCTestGroup = new FnQCTestGroup();
            if (id > 0)
            {
                _oFnQCTestGroup = _oFnQCTestGroup.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oFnQCTestGroup);
        }

        [HttpPost]
        public JsonResult Save(FnQCTestGroup oFnQCTestGroup)
        {
            _oFnQCTestGroup = new FnQCTestGroup();
            try
            {
                _oFnQCTestGroup = oFnQCTestGroup;
                _oFnQCTestGroup = oFnQCTestGroup.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFnQCTestGroup = new FnQCTestGroup();
                _oFnQCTestGroup.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFnQCTestGroup);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult Delete(FnQCTestGroup oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FnQCTestGroup oFnQCTestGroup = new FnQCTestGroup();
                sFeedBackMessage = oFnQCTestGroup.Delete(oJB.FnQCTestGroupID, (int)Session[SessionInfo.currentUserID]);
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


        #region Get

        //[HttpPost]
        //public JsonResult GetsDetailsByID(FnQCTestGroupDetail oFnQCTestGroupDetail)//Id=ContractorID
        //{
        //    try
        //    {
        //        string Ssql = "SELECT*FROM View_FnQCTestGroupDetail WHERE FnQCTestGroupID=" + oFnQCTestGroupDetail.FnQCTestGroupID + " ";
        //        _oFnQCTestGroupDetails = new List<FnQCTestGroupDetail>();
        //        _oFnQCTestGroupDetail.FnQCTestGroupDetails = FnQCTestGroupDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFnQCTestGroupDetail.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oFnQCTestGroupDetail);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        #endregion


        #region print
        //[HttpPost]
        //public ActionResult SetFnQCTestGroupListData(FnQCTestGroup oFnQCTestGroup)
        //{
        //    this.Session.Remove(SessionInfo.ParamObj);
        //    this.Session.Add(SessionInfo.ParamObj, oFnQCTestGroup);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(Global.SessionParamSetMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PrintFnQCTestGroups()
        //{
        //    _oFnQCTestGroup = new FnQCTestGroup();
        //    try
        //    {
        //        _oFnQCTestGroup = (FnQCTestGroup)Session[SessionInfo.ParamObj];
        //        string sSQL = "SELECT * FROM View_FnQCTestGroup WHERE FnQCTestGroupID IN (" + _oFnQCTestGroup.ErrorMessage + ") Order By FnQCTestGroupID";
        //        _oFnQCTestGroups = FnQCTestGroup.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFnQCTestGroup = new FnQCTestGroup();
        //        _oFnQCTestGroups = new List<FnQCTestGroup>();
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFnQCTestGroup.Company = oCompany;

        //    rptFnQCTestGroups oReport = new rptFnQCTestGroups();
        //    byte[] abytes = oReport.PrepareReport(_oFnQCTestGroups, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult FnQCTestGroupPrintPreview(int id)
        //{
        //    _oFnQCTestGroup = new FnQCTestGroup();
        //    Company oCompany = new Company();
        //    Contractor oContractor = new Contractor();
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    if (id > 0)
        //    {
        //        _oFnQCTestGroup = _oFnQCTestGroup.Get(id, (int)Session[SessionInfo.currentUserID]);
        //        _oFnQCTestGroup.FnQCTestGroupDetails = FnQCTestGroupDetail.Gets(_oFnQCTestGroup.FnQCTestGroupID, (int)Session[SessionInfo.currentUserID]);
        //        //_oFnQCTestGroup.BusinessUnit = oBusinessUnit.Get(_oFnQCTestGroup.BUID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    //else
        //    //{
        //    //    _oFnQCTestGroup.BusinessUnit = new BusinessUnit();
        //    //}
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFnQCTestGroup.Company = oCompany;
        //    byte[] abytes;
        //    rptFnQCTestGroup oReport = new rptFnQCTestGroup();
        //    abytes = oReport.PrepareReport(_oFnQCTestGroup, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
        //        if (System.IO.File.Exists(fileDirectory))
        //        {
        //            System.IO.File.Delete(fileDirectory);
        //        }

        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(fileDirectory, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

    }

}
