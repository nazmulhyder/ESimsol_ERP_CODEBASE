using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using ICS.Core.Utility;
using System.Drawing.Printing;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class FNRecipeController : Controller
    {
        #region Declaration
        FNRecipe _oFNRecipe = new FNRecipe();
        List<FNRecipe> _oFNRecipes = new List<FNRecipe>();
        string _sErrorMessage = "";

        #endregion

        #region FNRecipe Class
        public ActionResult ViewFNRecipes(int nId, int nFSCID, int buid)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            oFabricSalesContractDetail = oFabricSalesContractDetail.Get(nId, (int)Session[SessionInfo.currentUserID]);

            FNRecipe oFNRecipe = new FNRecipe();
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            ViewBag.FabricSalesContractDetail = oFabricSalesContractDetail;
            oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FSCDID =" + oFabricSalesContractDetail.FabricSalesContractDetailID , ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;

            ViewBag.FNTreatmentProcessList = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + nId + "' AND IsProcess = 'true'  AND ShadeID = '" + oFabricSalesContractDetail.ShadeID + "' ORDER BY FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);

            FnOrderExecute oFnOrderExecute = new FnOrderExecute();
            List<FnOrderExecute> oFnOrderExecutes = new List<FnOrderExecute>();
            
            try
            {
                oFnOrderExecutes = FnOrderExecute.Gets("SELECT * FROM View_FnOrderExecute  where FSCDID =" + oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
                if (oFnOrderExecutes.Count > 0)
                {
                    oFnOrderExecute = oFnOrderExecutes[0];
                }
            }
            catch
            {
                oFnOrderExecute = new FnOrderExecute();
            }
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            oFnOrderExecute.QtyOrder = oFabricSalesContractDetail.Qty;
            oFnOrderExecute.FSCDID = nId;
            //oFnOrderExecute.FNLabdipDetailID = oFabricSalesContractDetail.FNLabdipDetailID;
            ViewBag.FnOrderExecute = oFnOrderExecute;
            return View(oFNRecipes);
        }
        #endregion

         #region Functions

        [HttpPost]
        public JsonResult GetsFNRecipeByTreatmentProcessAndFSCDID(FNRecipe oFNRecipe)
        {
            _oFNRecipe = new FNRecipe();
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            try
            {
                oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FSCDID = '" + oFNRecipe.FSCDID + "' AND FNTPID = '" + oFNRecipe.FNTPID + "' AND ShadeID = " + (int)oFNRecipe.ShadeID + " AND IsProcess = 'false'", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Save(List<FNRecipe> oFNRecipes)
        {
            _oFNRecipe = new FNRecipe();
            _oFNRecipes = new List<FNRecipe>();
            //_oFNRecipes = oFNRecipes;
            try
            {
                foreach (FNRecipe oItem in oFNRecipes)
                {
                    _oFNRecipe = oItem;
                    _oFNRecipe = _oFNRecipe.Save((int)Session[SessionInfo.currentUserID]);
                }
                _oFNRecipes = FNRecipe.Gets("SELECT * FROM View_FNRecipe WHERE FSCDID = " + _oFNRecipe.FSCDID + " AND FNTPID = " + _oFNRecipe.FNTPID + " AND ShadeID = " + (int)_oFNRecipe.ShadeID + " AND IsProcess = '" + _oFNRecipe.IsProcess + "' ", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveProcess(FNRecipe oFNRecipe)
        {
            _oFNRecipe = oFNRecipe;
            try
            {
                _oFNRecipe = _oFNRecipe.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteDyes(FNRecipe oFNRecipe)
        {
            _oFNRecipe = oFNRecipe;
            try
            {
                _oFNRecipe.ErrorMessage = _oFNRecipe.Delete(oFNRecipe.FNRecipeID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteProcess(FNRecipe oFNRecipe)
        {
            _oFNRecipe = oFNRecipe;
            try
            {
                _oFNRecipe.ErrorMessage = _oFNRecipe.DeleteProcess(oFNRecipe.FNRecipeID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNRecipe);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsProcessByShade(FNRecipe oFNRecipe)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            try
            {
                oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + oFNRecipe.FSCDID + "'AND ShadeID = " + (int)oFNRecipe.ShadeID + " AND IsProcess = 'true' ORDER BY FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsProcessByFSCD(FNRecipe oFNRecipe)
        {
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            try
            {
                oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + oFNRecipe.FSCDID + "' AND IsProcess = 'true' ORDER BY FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDispo(FNRecipe oFNRecipe)
        {
            List<FabricSalesContractDetail> oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            try
            {
                oFabricSalesContractDetails = FabricSalesContractDetail.Gets("SELECT * FROM View_FabricSalesContractDetail WHERE FabricSalesContractDetailID != " + oFNRecipe.FSCDID + " AND FabricSalesContractDetailID IN (SELECT FSCDID FROM FNRecipe)", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFabricSalesContractDetails = new List<FabricSalesContractDetail>();
            }
            var jSonResult = Json(oFabricSalesContractDetails, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;

        }

        [HttpPost]
        public JsonResult CopyOrder(FNRecipe objFNRecipe)
        {
            FNRecipe oFNRecipe = objFNRecipe;
            List<FNRecipe> oFNRecipes = new List<FNRecipe>();

            int nCount = 0;
            int nFNLabDipDetailID = 0;
            int nFSCDID = 0, nFromFSCDID = 0;
            bool IsFromLabDip = true;
            int nShadeID = 0;

            try
            {
                if (!string.IsNullOrEmpty(oFNRecipe.ErrorMessage))
                {
                    nFNLabDipDetailID = Convert.ToInt32(oFNRecipe.ErrorMessage.Split('~')[nCount++]);
                    nFSCDID = Convert.ToInt32(oFNRecipe.ErrorMessage.Split('~')[nCount++]);
                    nFromFSCDID = Convert.ToInt32(oFNRecipe.ErrorMessage.Split('~')[nCount++]);
                    IsFromLabDip = Convert.ToBoolean(oFNRecipe.ErrorMessage.Split('~')[nCount++]);
                    nShadeID = (int)oFNRecipe.ShadeID;
                    oFNRecipes = oFNRecipe.CopyOrder(nFNLabDipDetailID, IsFromLabDip, nFSCDID, nFromFSCDID, nShadeID, (int)Session[SessionInfo.currentUserID]);
                    oFNRecipes = oFNRecipes.Where(x => x.IsProcess == true).ToList();
                }
            }
            catch (Exception ex)
            {
                _oFNRecipe = new FNRecipe();
                _oFNRecipe.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNRecipes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Print(int nFSCDID, int nBUID, int nShadeID)
        {
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();
            oFabricSalesContractDetail = oFabricSalesContractDetail.Get(nFSCDID, (int)Session[SessionInfo.currentUserID]);

            List<FNRecipe> oFNRecipes = new List<FNRecipe>();
            oFNRecipes = FNRecipe.Gets("select * from View_FNRecipe where FSCDID = '" + nFSCDID + "' AND ShadeID = '" + nShadeID + "' Order by FNTreatment, FNProcess", (int)Session[SessionInfo.currentUserID]);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);

            rptFNRecipe oReport = new rptFNRecipe();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            byte[] abytes = new byte[1];
            abytes = oReport.PrepareReport(oCompany, oFabricSalesContractDetail, oFNRecipes, oBusinessUnit);
            return File(abytes, "application/pdf");
        }
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
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

        #region FNOrderExecute
        [HttpPost]
        public JsonResult SaveFnOrderExecute(FnOrderExecute oFnOrderExecute)
        {
            FnOrderExecute _oFnOrderExecute = new FnOrderExecute();
            try
            {
                _oFnOrderExecute = oFnOrderExecute.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFnOrderExecute = new FnOrderExecute();
                _oFnOrderExecute.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFnOrderExecute);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetsProcessFromLabByShade(FNRecipeLab oFNRecipeLab)
        //{
        //    List<FNRecipeLab> oFNRecipeLabs = new List<FNRecipeLab>();
        //    try
        //    {
        //        if (oFNRecipeLab.ShadeID <= 0)
        //            oFNRecipeLab.ShadeID = EnumShade.A;
        //        oFNRecipeLabs = FNRecipeLab.Gets("SELECT * from View_FNRecipeLab where FNLDDID = " + oFNRecipeLab.FNLDDID + " AND ShadeID = " + (int)oFNRecipeLab.ShadeID + " AND IsProcess = 1 ORDER BY ShadeID, FNTreatment, Code", ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        oFNRecipeLabs = new List<FNRecipeLab>();
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFNRecipeLabs);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        #endregion

    }
}
