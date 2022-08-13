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
    public class QCController : Controller
    {

        #region Declaration

        QC _oQC = new QC();
        List<QC> _oQCs = new List<QC>();
       
        List<ProductionProcedure> _oProductionProcedures = new List<ProductionProcedure>();
        ProductionProcedure _oProductionProcedure = new ProductionProcedure();
        #endregion

        #region Functions

        #endregion

        #region Actions

        public ActionResult ViewQC(int ProductNature, int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.QC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ViewBag.BUID = buid;
            ViewBag.ProductNature = ProductNature;
            ViewBag.Stores = WorkingUnit.GetsPermittedStore(buid, EnumModuleName.QC, EnumStoreType.ReceiveStore, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oQC);
        }

        public ActionResult ViewQCList( int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.QC).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oQC = new QC();
            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oQC);
        }



        [HttpPost]
        public JsonResult SaveQC(QC oQC)
        {
            _oQC = new QC();
            try
            {
                _oQC = oQC.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oQC = new QC();
                _oQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetQCs(QC oQC)
        {
            _oQCs = new List<QC>();
            try
            {
                string sSQL = "SELECT * FROM View_QC WHERE ProductionSheetID = " + oQC.ProductionSheetID + " ORDER BY QCID";
                _oQCs = QC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oQC = new QC();
                _oQC.ErrorMessage = ex.Message;
                _oQCs.Add(_oQC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsQCList(QC oQC)
        {
            _oQCs = new List<QC>();
            try
            {
                string sSQL = "SELECT * FROM View_QC WHERE BUID = " + oQC.BUID + " AND CONVERT(Date,OperationTime) = CONVERT(Date, '" + oQC.OperationTime + "') AND IsExist=1  ORDER BY OperationTime DESC";
                _oQCs = QC.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oQC = new QC();
                _oQC.ErrorMessage = ex.Message;
                _oQCs.Add(_oQC);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQCs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public JsonResult CommitProcess(QC oQC)
        {
            _oQC = new QC();
            _oQCs = new List<QC>();
            try
            {
                _oQC = oQC.Save((int)Session[SessionInfo.currentUserID]);
                if(_oQC.ErrorMessage=="" || _oQC.ErrorMessage==null)
                {
                    _oQCs = QC.Gets("SELECT * FROM View_QC WHERE QCID IN ("+oQC.sParam+")", (int)Session[SessionInfo.currentUserID]);
                }

            }
            catch (Exception ex)
            {
                _oQC = new QC();
                _oQC.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oQC);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        public ActionResult PrintFGCost(int QCID)
        {
            List<FGCost> oFGCosts = new List<FGCost>();
            Company oCompany = new Company();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            _oQC = new QC();
            _oQC = _oQC.Get(QCID, (int)Session[SessionInfo.currentUserID]);
            _oQC.FGCosts = FGCost.Gets(QCID, (int)Session[SessionInfo.currentUserID]);
            _oQC.BusinessUnit = oBusinessUnit.Get(_oQC.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oQC.Company = oCompany;


            rptFGCost oReport = new rptFGCost();
            byte[] abytes = oReport.PrepareReport(_oQC);
            return File(abytes, "application/pdf");
          
        }


        #region GEt Lot
        [HttpPost]
        public JsonResult GetLots(QC oQC)
        {
            List<Lot> oLots = new List<Lot>();
            try
            {
                string sSQL = "SELECT * FROM View_Lot WHERE ParentType = "+(int)EnumTriggerParentsType.FinishGoodsQC+" AND WorkingUnitID = "+oQC.WorkingUnitID+" AND ParentID IN (SELECT QCID FROM View_QC WHERE ProductionSheetID = "+oQC.ProductionSheetID+")";
                oLots = Lot.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                Lot oLot = new Lot();
                oLot.ErrorMessage = ex.Message;
                oLots.Add(oLot);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLots);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
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

    }
    
    
  
}