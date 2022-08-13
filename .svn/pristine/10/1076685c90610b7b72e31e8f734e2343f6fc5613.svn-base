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
    public class FabricClaimController : Controller
    {
        #region Declaration

        FabricClaim _oFabricClaim = new FabricClaim();
        List<FabricClaim> _oFabricClaims = new List<FabricClaim>();
        FabricClaimDetail _oFabricClaimDetail = new FabricClaimDetail();
        List<FabricClaimDetail> _oFabricClaimDetails = new List<FabricClaimDetail>();
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewFabricClaims(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricClaim).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oFabricClaims = new List<FabricClaim>();
            _oFabricClaims = FabricClaim.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            return View(_oFabricClaims);
        }

        public ActionResult ViewFabricClaim(int id, int buid)
        {
            _oFabricClaim = new FabricClaim();
            if (id > 0)
            {
                _oFabricClaim = _oFabricClaim.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFabricClaim.FabricClaimDetails = FabricClaimDetail.Gets("SELECT * FROM View_FabricClaimDetail WHERE FabricClaimID="+id, (int)Session[SessionInfo.currentUserID]);
            }
            ViewBag.BUID = buid;

            ViewBag.ClaimSettleTypes = EnumObject.jGets(typeof(EnumImportClaimSettleType));
            return View(_oFabricClaim);
        }

        [HttpPost]
        public JsonResult Save(FabricClaim oFabricClaim)
        {
            _oFabricClaim = new FabricClaim();
            try
            {
                _oFabricClaim = oFabricClaim.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricClaim = new FabricClaim();
                _oFabricClaim.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricClaim);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(FabricClaim oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricClaim oFabricClaim = new FabricClaim();
                sFeedBackMessage = oFabricClaim.Delete(oJB, (int)Session[SessionInfo.currentUserID]);
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

        [HttpPost]
        public JsonResult GetFSC(FabricSalesContract oFabricSalesContract)
        {
            List<FabricSalesContract> oFabricSalesContracts = new List<FabricSalesContract>();
            try
            {
                string Ssql = "SELECT * FROM View_FabricSalesContract WHERE BUID=" + oFabricSalesContract.BUID;
                if (!string.IsNullOrEmpty(oFabricSalesContract.SCNo))
                    Ssql += " AND SCNo LIKE '%" + oFabricSalesContract.SCNo + "%'";
                oFabricSalesContracts = FabricSalesContract.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSalesContract = new FabricSalesContract();
                oFabricSalesContract.ErrorMessage = ex.Message;
                oFabricSalesContracts.Add(oFabricSalesContract);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContracts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetSCDetails(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            try
            {
                string Ssql = "SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractID=" + oFabricSalesContractDetail.FabricSalesContractID;
                oFabricSalesContractDetails = FabricSalesContractDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSalesContractDetail = new FabricSalesContractDetail();
                oFabricSalesContractDetail.ErrorMessage = ex.Message;
                oFabricSalesContractDetails.Add(oFabricSalesContractDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSalesContractDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region print
        [HttpPost]
        public ActionResult SetFabricClaimListData(FabricClaim oFabricClaim)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricClaim);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PrintFabricClaims()
        //{
        //    _oFabricClaim = new FabricClaim();
        //    try
        //    {
        //        _oFabricClaim = (FabricClaim)Session[SessionInfo.ParamObj];
        //        string sSQL = "SELECT * FROM View_FabricClaim WHERE FabricClaimID IN (" + _oFabricClaim.ErrorMessage + ") Order By FabricClaimID";
        //        _oFabricClaims = FabricClaim.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oFabricClaim = new FabricClaim();
        //        _oFabricClaims = new List<FabricClaim>();
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFabricClaim.Company = oCompany;

        //    rptFabricClaims oReport = new rptFabricClaims();
        //    byte[] abytes = oReport.PrepareReport(_oFabricClaims, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult FabricClaimPrintPreview(int id)
        //{
        //    _oFabricClaim = new FabricClaim();
        //    Company oCompany = new Company();
        //    Contractor oContractor = new Contractor();
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    if (id > 0)
        //    {
        //        _oFabricClaim = _oFabricClaim.Get(id, (int)Session[SessionInfo.currentUserID]);
        //        _oFabricClaim.FabricClaimDetails = FabricClaimDetail.Gets(_oFabricClaim.FabricClaimID, (int)Session[SessionInfo.currentUserID]);
        //        //_oFabricClaim.BusinessUnit = oBusinessUnit.Get(_oFabricClaim.BUID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    //else
        //    //{
        //    //    _oFabricClaim.BusinessUnit = new BusinessUnit();
        //    //}
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oFabricClaim.Company = oCompany;
        //    byte[] abytes;
        //    rptFabricClaim oReport = new rptFabricClaim();
        //    abytes = oReport.PrepareReport(_oFabricClaim, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #endregion

    }

}
