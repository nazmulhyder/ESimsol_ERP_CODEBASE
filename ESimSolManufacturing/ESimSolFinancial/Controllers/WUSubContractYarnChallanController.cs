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
    public class WUSubContractYarnChallanController : Controller
    {
        #region Declaration
        WUSubContractYarnChallan _oWUSubContractYarnChallan = new WUSubContractYarnChallan();
        List<WUSubContractYarnChallan> _oWUSubContractYarnChallans = new List<WUSubContractYarnChallan>();
        WUSubContractYarnChallanDetail _oWUSubContractYarnChallanDetail = new WUSubContractYarnChallanDetail();
        List<WUSubContractYarnChallanDetail> _oWUSubContractYarnChallanDetails = new List<WUSubContractYarnChallanDetail>();
        #endregion

        public ActionResult ViewWUSubContractYarnChallans(int WUSubContractID)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.WUSubContractYarnChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract = oWUSubContract.Get(WUSubContractID, (int)Session[SessionInfo.currentUserID]);
            _oWUSubContractYarnChallans = WUSubContractYarnChallan.Gets("SELECT * FROM View_WUSubContractYarnChallan WHERE WUSubContractID=" + WUSubContractID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.WUSubContract = oWUSubContract;
            return View(_oWUSubContractYarnChallans);
        }

        public ActionResult ViewWUSubContractYarnChallan(int id, int WUSubContractID)
        {            
            WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            List<WUSubContractYarnConsumption> oWUSubContractYarnConsumptions = new List<WUSubContractYarnConsumption>();
            oWUSubContractYarnConsumptions = WUSubContractYarnConsumption.Gets(WUSubContractID, (int)Session[SessionInfo.currentUserID]);

            if (id > 0)
            {
                oWUSubContractYarnChallan = oWUSubContractYarnChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                List<WUSubContractYarnChallanDetail> oWUSubContractYarnChallanDetails = new List<WUSubContractYarnChallanDetail>();
                oWUSubContractYarnChallanDetails = WUSubContractYarnChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContractYarnChallan.WUSubContractYarnChallanDetails = oWUSubContractYarnChallanDetails;

                double nYetToChallanQty = 0;
                foreach (WUSubContractYarnConsumption oItem in oWUSubContractYarnConsumptions)
                {
                    nYetToChallanQty = 0;
                    if(oWUSubContractYarnChallanDetails.Where(x => x.WUSubContractYarnConsumptionID == oItem.WUSubContractYarnConsumptionID).ToList().Count >0)
                    {
                        nYetToChallanQty = oWUSubContractYarnChallanDetails.Where(x => x.WUSubContractYarnConsumptionID == oItem.WUSubContractYarnConsumptionID).FirstOrDefault().YetToChallanQty;
                        oItem.YetToChallanQty = nYetToChallanQty;
                    }                    
                }
            }

            #region Gets Stores

            WUSubContract oWUSubContract = new WUSubContract();
            oWUSubContract = oWUSubContract.Get(WUSubContractID, (int)Session[SessionInfo.currentUserID]);

            WorkingUnit oWorkingUnit = new WorkingUnit();
            List<WorkingUnit> oWorkingUnits = new List<WorkingUnit>();
            oWorkingUnit.LocationName = "--Select Store--";
            oWorkingUnit.OperationUnitName = "";
            oWorkingUnits.Add(oWorkingUnit);
            oWorkingUnits.AddRange(WorkingUnit.GetsPermittedStore(oWUSubContract.BUID, EnumModuleName.WUSubContractYarnChallan, EnumStoreType.IssueStore, (int)Session[SessionInfo.currentUserID]));
            
            #endregion
            
            ViewBag.Stores = oWorkingUnits;
            ViewBag.YarnConsumptions = oWUSubContractYarnConsumptions;
            ViewBag.MeasurementUnits = MeasurementUnit.Gets("SELECT * FROM MeasurementUnit", (int)Session[SessionInfo.currentUserID]);            
            return View(oWUSubContractYarnChallan);
        }

        [HttpPost]
        public JsonResult Save(WUSubContractYarnChallan oWUSubContractYarnChallan)
        {
            _oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            try
            {
                _oWUSubContractYarnChallan = oWUSubContractYarnChallan;
                _oWUSubContractYarnChallan = _oWUSubContractYarnChallan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                _oWUSubContractYarnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContractYarnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(WUSubContractYarnChallan oWUSubContractYarnChallan)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oWUSubContractYarnChallan.Delete(oWUSubContractYarnChallan.WUSubContractYarnChallanID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ApproveChallan(WUSubContractYarnChallan oWUSubContractYarnChallan)
        {
            _oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            try
            {
                _oWUSubContractYarnChallan = oWUSubContractYarnChallan.Approve((int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                _oWUSubContractYarnChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oWUSubContractYarnChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintPreview(int id, int buid, double ts)
        {
            WUSubContractYarnChallan oWUSubContractYarnChallan = new WUSubContractYarnChallan();
            try
            {
                oWUSubContractYarnChallan = oWUSubContractYarnChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                oWUSubContractYarnChallan.WUSubContractYarnChallanDetails = WUSubContractYarnChallanDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oWUSubContractYarnChallan = new WUSubContractYarnChallan();
                oWUSubContractYarnChallan.ErrorMessage = ex.Message;
            }

            List<SignatureSetup> oSignatureSetups = new List<SignatureSetup>();
            oSignatureSetups = SignatureSetup.GetsByReportModule(EnumReportModule.WUSubContractYarnChallanPreview, (int)Session[SessionInfo.currentUserID]);

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

            rptWUSubContractYarnChallan rptWUSubContractYarnChallan = new rptWUSubContractYarnChallan();
            byte[] abytes = rptWUSubContractYarnChallan.PrepareReport(oWUSubContractYarnChallan, oCompany, oBU, oSignatureSetups);
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