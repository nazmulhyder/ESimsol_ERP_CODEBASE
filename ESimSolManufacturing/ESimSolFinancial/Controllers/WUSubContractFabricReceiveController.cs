using ESimSol.BusinessObjects;
using ESimSol.Reports;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class WUSubContractFabricReceiveController : Controller
    {
        #region Declaration
        WUSubContractFabricReceive _oWUSubContractFabricReceive = new WUSubContractFabricReceive();
        List<WUSubContractFabricReceive> _oWUSubContractFabricReceives = new List<WUSubContractFabricReceive>();
        #endregion

        public ActionResult ViewWUSubContractFabricReceives(int WUSubContractID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WUSubContractFabricReceive).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract = oWUSubContract.Get(WUSubContractID, (int)Session[SessionInfo.currentUserID]);
            _oWUSubContractFabricReceives = WUSubContractFabricReceive.Gets("SELECT * FROM View_WUSubContractFabricReceive WHERE WUSubContractID=" + WUSubContractID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WUSubContract = oWUSubContract;
            return View(_oWUSubContractFabricReceives);
        }

        public ActionResult ViewWUSubContractFabricReceive(int id, int WUSubContractID)
        {
            WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();

            if (id > 0)
            {
                oWUSubContractFabricReceive = oWUSubContractFabricReceive.Get(id, (int)Session[SessionInfo.currentUserID]);
            }

            #region Gets Stores

            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract = oWUSubContract.Get(WUSubContractID, (int)Session[SessionInfo.currentUserID]);

            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(oWUSubContract.BUID, EnumModuleName.WUSubContractFabricReceive, EnumStoreType.ReceiveStore, (int)Session[SessionInfo.currentUserID]));

            #endregion

            ViewBag.Stores = oWorkingUnits;
            ViewBag.WUSubContract = oWUSubContract;
            return View(oWUSubContractFabricReceive);
        }

        [HttpPost]
        public JsonResult Save(WUSubContractFabricReceive oWUSubContractFabricReceive)
        {
            _oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            try
            {
                _oWUSubContractFabricReceive = oWUSubContractFabricReceive;
                _oWUSubContractFabricReceive = _oWUSubContractFabricReceive.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                _oWUSubContractFabricReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContractFabricReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(WUSubContractFabricReceive oWUSubContractFabricReceive)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oWUSubContractFabricReceive.Delete(oWUSubContractFabricReceive.WUSubContractFabricReceiveID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ApproveFabricReceive(WUSubContractFabricReceive oWUSubContractFabricReceive)
        {
            _oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            try
            {
                oWUSubContractFabricReceive = oWUSubContractFabricReceive.Approve((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                _oWUSubContractFabricReceive.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oWUSubContractFabricReceive);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPreview(int id, int buid, double ts)
        {
            WUSubContractFabricReceive oWUSubContractFabricReceive = new WUSubContractFabricReceive();
            try
            {
                oWUSubContractFabricReceive = oWUSubContractFabricReceive.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWUSubContractFabricReceive = new WUSubContractFabricReceive();
                oWUSubContractFabricReceive.ErrorMessage = ex.Message;
            }

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.WUSubContractFabricReceivePreview, (int)Session[SessionInfo.currentUserID]);

            this.Session.Remove(SessionInfo.ParamObj);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            BusinessUnit oBU = new BusinessUnit();
            if (buid > 0)
            {
                oBU = oBU.Get(buid, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);
            }
            oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

            rptWUSubContractFabricReceive rptWUSubContractFabricReceive = new rptWUSubContractFabricReceive();
            byte[] abytes = rptWUSubContractFabricReceive.PrepareReport(oWUSubContractFabricReceive, oCompany, oBU, oSignatureSetups);
            return File(abytes, "application/pdf");
        }

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