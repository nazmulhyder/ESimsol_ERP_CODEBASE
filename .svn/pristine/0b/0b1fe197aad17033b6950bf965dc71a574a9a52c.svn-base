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
    public class MachineController : PdfViewController
    {
        #region Declaration
        Machine _oMachine = new Machine();
        List<Machine> _oMachines = new List<Machine>();
        DUStepWiseSetup _oDUStepWiseSetup = new DUStepWiseSetup();
        #endregion

        public ActionResult ViewMachines(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<Machine> oMachines = new List<Machine>();

            string sSQL = "SELECT * FROM View_Machine";
            if (buid > 0)
                sSQL += " WHERE BUID=" + buid;

            oMachines = Machine.Gets(sSQL + " ORDER BY SequenceNo", ((User)Session[SessionInfo.CurrentUser]).UserID);

            BusinessUnit oBusinessUnit = new BusinessUnit();
            if (buid > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
                ViewBag.BusinessUnits = oBusinessUnit;
            }
            else
                ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.BU = oBusinessUnit;

            sSQL = "SELECT * FROM MachineType";
            if (buid > 0)
                sSQL += " WHERE BUID=" + buid;

            ViewBag.MahcineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Locations = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
           
            return View(oMachines);
        }
        public ActionResult ViewMachine(int nId, int buid, double ts)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            Machine oMachine = new Machine();
            if (nId > 0)
            {
                oMachine = oMachine.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.BUID = buid;

            ViewBag.BusinessUnits = BusinessUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.BUID = buid;
            string sSQL = "SELECT * FROM MachineType";
            if (buid > 0)
                sSQL += " WHERE BUID=" + buid;

            ViewBag.MachineTypes = MachineType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.Locations = Location.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oMachine);
        }
        public ActionResult ViewMachineSequences(int buid, double ts)
        {
            List<Machine> oMachines = new List<Machine>();

            string sSQL = "SELECT * FROM View_Machine";
            if (buid > 0)
                sSQL += " WHERE BUID=" + buid;

            oMachines = Machine.Gets(sSQL+" ORDER BY SequenceNo", ((User)Session[SessionInfo.CurrentUser]).UserID);

            return View(oMachines);
        }
        [HttpPost]
        public JsonResult MachineLiquorSave(MachineLiquor oMachineLiquor)
        {
            MachineLiquor _oMachineLiquor = new MachineLiquor();
            try
            {
                _oMachineLiquor = oMachineLiquor;
                _oMachineLiquor = _oMachineLiquor.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMachineLiquor = new MachineLiquor();
                _oMachineLiquor.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachineLiquor);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult MachineLiquorDelete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                MachineLiquor _MachineLiquor = new MachineLiquor();
                sFeedBackMessage = _MachineLiquor.Delete(id, (int)Session[SessionInfo.currentUserID]);

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
        public JsonResult GetAllMachineLiquor(MachineLiquor oMachineLiquor)
        {
            MachineLiquor _oMachineLiquor = new MachineLiquor();
            List<MachineLiquor> _oMachineLiquors = new List<MachineLiquor>();
            String SQL = "";
            try
            {
                if (oMachineLiquor.MachineID > 0)
                {
                    SQL = "SELECT * FROM View_MachineLiquor AS HH WHERE HH.MachineID = " + oMachineLiquor.MachineID+" Order By MachineLiquorID";
                }

                _oMachineLiquors = MachineLiquor.Gets(SQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMachineLiquor = new MachineLiquor();
                _oMachineLiquor.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachineLiquors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Activity(Machine oMachine)
        {
            oMachine.RemoveNulls();
            _oMachine = new Machine();
            try
            {
                _oMachine = oMachine.Activate(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMachine = new Machine();
                _oMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Save(Machine oMachine)
        {
            oMachine.RemoveNulls();
            _oMachine = new Machine();
            try
            {
                _oMachine = oMachine.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oMachine = new Machine();
                _oMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        [HttpPost]
        public JsonResult Delete(Machine oMachine)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oMachine.Delete(((User)Session[SessionInfo.CurrentUser]).UserID);
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
            List<Machine> oMachines = new List<Machine>();
            oMachines = Machine.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Update(List<Machine> oMachines)
        {
            _oMachines = new List<Machine>();
            try
            {
                _oMachines = oMachines;
                _oMachines = _oMachine.Update(oMachines,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMachines = new List<Machine>();
                _oMachine = new Machine();
                _oMachine.ErrorMessage = ex.Message; _oMachines.Add(_oMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
        
        #region Search  by Press Enter
        [HttpPost]
        public JsonResult GetsByName(Machine oMachine)
        {
            List<Machine> oMachines = new List<Machine>();            
            string sSQL = "SELECT * FROM View_Machine WHERE Name LIKE '%" + oMachine.Name + "%' AND BUID = " + oMachine.BUID + " AND Activity = '" + oMachine.Activity + "'";
            oMachines = Machine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult SearchByType(Machine oMachine, double ts)
        {
            _oMachines = new List<Machine>();
            string sSQL = "";

            sSQL = "SELECT * FROM View_Machine WHERE MachineTypeID =" + oMachine.MachineTypeID;

            try
            {
                _oMachines = Machine.Gets(sSQL + " ORDER BY SequenceNo", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oMachine = new Machine();
                _oMachine.ErrorMessage = ex.Message;
                _oMachines.Add(_oMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public JsonResult SearchByInvoiceNo(string sTempData, double ts)
        //{
        //    _oMachines = new List<Machine>();
        //    string sSQL = "";

        //    sSQL = "SELECT * FROM View_Machine where MachineID in (Select MachineID from View_MachineDetail where View_MachineDetail.ImportInvoiceNo  LIKE'%" + sTempData + "%')";

        //    try
        //    {
        //        _oMachines = Machine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oMachine = new Machine();
        //        _oMachine.ErrorMessage = ex.Message;
        //        _oMachines.Add(_oMachine);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oMachines);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //public ActionResult PrintMachinePreview(int id)
        //{
        //    _oMachine = new Machine();
        //    _oMachine = _oMachine.Get(id, (int)Session[SessionInfo.currentUserID]);
        //    _oMachine.MachineDetails = MachineDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);

        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);

        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    oBusinessUnit = oBusinessUnit.GetByType((int)EnumBusinessUnitType.Dyeing, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //    rptMachine oReport = new rptMachine();
        //    byte[] abytes = oReport.PrepareReport(_oMachine, oCompany, oBusinessUnit);
        //    return File(abytes, "application/pdf");
        //}
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

        #region Adv Search
        public JsonResult AdvSearch(string sTemp)
        {
            List<Machine> oMachines = new List<Machine>();
            try
            {
                string sSQL = MakeSQL(sTemp);
                oMachines = Machine.Gets( (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oMachines = new List<Machine>();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private string MakeSQL(string sTemp)
        {

            int ncboChallanDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime txtChallanDateFrom = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime txtChallanDateTo = Convert.ToDateTime(sTemp.Split('~')[2]);
            //
            string sChallanNo = Convert.ToString(sTemp.Split('~')[3]);
            string sImportInvoiceNo = Convert.ToString(sTemp.Split('~')[4]);

            string sReturn1 = "SELECT * FROM View_Machine ";
            string sReturn = "";



            #region Challan No
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + sChallanNo + "%' ";
            }
            #endregion

            #region SC No
            if (!string.IsNullOrEmpty(sImportInvoiceNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MachineID in (Select MachineID from View_MachineDetail where ImportInvoiceNo  LIKE '%" + sImportInvoiceNo + "%')";
            }
            #endregion

            //#region PI No
            //if (!string.IsNullOrEmpty(oFDOrder.PINo))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " FDOID IN (SELECT FDOID FROM MachineDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDOrder.PINo + "%')) ";
            //}
            //#endregion

            //#region Delivery To
            //if (!string.IsNullOrEmpty(sApplicantIDs))
            //{
            //    Global.TagSQL(ref sReturn);
            //    sReturn = sReturn + " ContractorID in( " + sApplicantIDs + ")";
            //}
            //#endregion

            #region Issue Date Wise
            if (ncboChallanDate > 0)
            {
                if (ncboChallanDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateTo.ToString("dd MMM yyyy") + "',106))";
                }
                if (ncboChallanDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateFrom.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + txtChallanDateTo.ToString("dd MMM yyyy") + "',106))";
                }
            }
            #endregion

            string sSQL = sReturn1 + " " + sReturn + " ORDER BY SequenceNo";
            return sSQL;
        }
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        #endregion

    }
}
