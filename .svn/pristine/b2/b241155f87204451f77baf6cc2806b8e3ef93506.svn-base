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
using ReportManagement;



namespace ESimSolFinancial.Controllers
{
    public class ProductionUnitController : PdfViewController
    {
        #region Declaration
        PLineConfigure _oPLineConfigure = new PLineConfigure();
        ProductionUnit _oProductionUnit = new ProductionUnit();
        List<ProductionUnit> _oProductionUnits = new List<ProductionUnit>();
        GUPReportSetUp _oGUPReportSetUp;
        List<GUPReportSetUp> _oGUPReportSetUps;
        #endregion

        #region Production Unit
        public ActionResult ViewProductionUnits(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<ProductionUnit> oProductionUnits = new List<ProductionUnit>();
            oProductionUnits = ProductionUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            return View(oProductionUnits);
        }
        public ActionResult ViewProductionUnit(int nId, int buid, double ts)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            if (nId > 0)
            {
                oProductionUnit = oProductionUnit.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.ProductionUnitTypes = EnumObject.jGets(typeof(EnumProductionUnitType));
            return View(oProductionUnit);
        }

        [HttpPost]
        public JsonResult Save(ProductionUnit oProductionUnit)
        {
            oProductionUnit.RemoveNulls();
            _oProductionUnit = new ProductionUnit();
            try
            {
                _oProductionUnit = oProductionUnit.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oProductionUnit = new ProductionUnit();
                _oProductionUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oProductionUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(ProductionUnit oProductionUnit)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oProductionUnit.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult Gets()
        {
            List<ProductionUnit> oProductionUnits = new List<ProductionUnit>();
            oProductionUnits = ProductionUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductionUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsByName(ProductionUnit oProductionUnit)
        {
            List<ProductionUnit> oProductionUnits = new List<ProductionUnit>();
            string sSQL="SELECT * FROM View_ProductionUnit WHERE Name LIKE '%"+oProductionUnit.Name+"%' OR ShortName LIKE '%"+oProductionUnit.Name+"%'";
            oProductionUnits = ProductionUnit.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oProductionUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Production Configure
        public ActionResult ViewPLineConfigure(int nId, int buid, double ts)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            if (nId > 0)
            {
                oProductionUnit = oProductionUnit.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            string sSQL = "SELECT * FROM View_PLineConfigure WHERE ProductionUnitID=" + nId;
            ViewBag.PLineConfigures = PLineConfigure.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oProductionUnit);
        }
        [HttpPost]
        public JsonResult SavePLineConfigure(PLineConfigure oPLineConfigure)
        {
            oPLineConfigure.RemoveNulls();
            _oPLineConfigure = new PLineConfigure();
            try
            {
                _oPLineConfigure = oPLineConfigure.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPLineConfigure = new PLineConfigure();
                _oPLineConfigure.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPLineConfigure);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeletePLineConfigure(PLineConfigure oPLineConfigure)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oPLineConfigure.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult GetPLineConfigures(PLineConfigure oPLineConfigure)
        {
            List<PLineConfigure> oPLineConfigures = new List<PLineConfigure>();
            try
            {
                oPLineConfigures = PLineConfigure.Gets(oPLineConfigure.ProductionUnitID,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oPLineConfigure = new PLineConfigure();
                _oPLineConfigure.ErrorMessage = ex.Message;
                oPLineConfigures.Add(_oPLineConfigure);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPLineConfigures);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        #endregion

        #region GUReport Header
        #region Views
        public ActionResult ViewGUPReportSetUps(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oGUPReportSetUps = new List<GUPReportSetUp>();
            _oGUPReportSetUps = GUPReportSetUp.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.ProductionSteps = ProductionStep.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            return View(_oGUPReportSetUps);
        }
       
        #endregion

        #region IUD GUPReportSetUp
        [HttpPost]
        public JsonResult SaveGUPReportSetUp(GUPReportSetUp oGUPReportSetUp)
        {
            _oGUPReportSetUp = new GUPReportSetUp();
            try
            {
                _oGUPReportSetUp = oGUPReportSetUp;
                _oGUPReportSetUp = _oGUPReportSetUp.Save( ((User)(Session[SessionInfo.CurrentUser])).UserID);
             
            }
            catch (Exception ex)
            {
                _oGUPReportSetUp = new GUPReportSetUp();
                _oGUPReportSetUp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGUPReportSetUp);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteGUPReportSetUp(GUPReportSetUp oGUPReportSetUp)
        {
            string sMessage = "";
            _oGUPReportSetUp = new GUPReportSetUp();
            try
            {
                sMessage = _oGUPReportSetUp.Delete(oGUPReportSetUp.GUPReportSetUpID,((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oGUPReportSetUp = new GUPReportSetUp();
                sMessage  = ex.Message;
                
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpDown(GUPReportSetUp oGUPReportSetUp)
        {
            GUPReportSetUp oAH = new GUPReportSetUp();
            List<GUPReportSetUp> oGUPReportSetUps = new List<GUPReportSetUp>();
            try
            {
                oGUPReportSetUps = GUPReportSetUp.UpDown(oGUPReportSetUp, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                _oGUPReportSetUp = new GUPReportSetUp();
                _oGUPReportSetUp.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oGUPReportSetUps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion
    }
}
