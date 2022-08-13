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
    public class DUDeliverySummaryRPTController : Controller
    {
        #region Declaration

        DUDeliverySummaryRPT _oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
        List<DUDeliverySummaryRPT> _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
        string _sDateRange;
        #endregion

        #region Functions
        #endregion

        #region Actions
        public ActionResult ViewDUDeliverySummaryRPT(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator)).Where(x => x.id == (int)EnumCompareOperator.EqualTo || x.id == (int)EnumCompareOperator.Between);
            ViewBag.OrderTypes = DUOrderSetup.Gets("SELECT * FROM DUOrderSetup", (int)Session[SessionInfo.currentUserID]);
            return View(_oDUDeliverySummaryRPTs);
        }
        #endregion
        #region Get
        [HttpPost]
        public JsonResult GetsData(DUDeliverySummaryRPT oDUDeliverySummaryRPT)
        {
            try
            {
                _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
                _oDUDeliverySummaryRPTs = DUDeliverySummaryRPT.GetsData(oDUDeliverySummaryRPT, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oDUDeliverySummaryRPTs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region print
        [HttpPost]
        public ActionResult SetDUDeliverySummaryRPT(DUDeliverySummaryRPT oDUDeliverySummaryRPT)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oDUDeliverySummaryRPT);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult PrintDUDeliverySummaryRPTs()
        {
            DUDeliverySummaryRPT oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
            //int nBUID = 0;
            try
            {
                oDUDeliverySummaryRPT = (DUDeliverySummaryRPT)Session[SessionInfo.ParamObj];
                _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
                _oDUDeliverySummaryRPTs = DUDeliverySummaryRPT.GetsData(oDUDeliverySummaryRPT, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
                _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
            }
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oDUDeliverySummaryRPTs.Count > 0)
            {
                rptDUDeliverySummaryRPT oReport = new rptDUDeliverySummaryRPT();
                byte[] abytes = oReport.PrepareReport(_oDUDeliverySummaryRPTs, oCompany, oDUDeliverySummaryRPT);
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }
        }

        public ActionResult PrintDeliveryChallanRegister()
        {
            DUDeliverySummaryRPT oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
            List<DUDeliveryChallanRegister> _oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
            string _sErrorMesage = "";
            try
            {
                oDUDeliverySummaryRPT = (DUDeliverySummaryRPT)Session[SessionInfo.ParamObj];
                string sSQL = this.GetSQL(oDUDeliverySummaryRPT);
                _oDUDeliveryChallanRegisters = DUDeliveryChallanRegister.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oDUDeliveryChallanRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
            }
            catch (Exception ex)
            {
                _oDUDeliveryChallanRegisters = new List<DUDeliveryChallanRegister>();
                _sErrorMesage = ex.Message;
            }
            if (_sErrorMesage == "")
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                BusinessUnit oBU = new BusinessUnit();
                oBU = oBU.Get(oDUDeliverySummaryRPT.BUID, (int)Session[SessionInfo.currentUserID]);
                oCompany = GlobalObject.BUTOCompany(oCompany, oBU);

                rptDUDeliveryChallanRegisters oReport = new rptDUDeliveryChallanRegisters();
                byte[] abytes = oReport.PrepareReport(_oDUDeliveryChallanRegisters, oCompany, EnumReportLayout.ChallanWise, "");
                return File(abytes, "application/pdf");
            }
            else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport(_sErrorMesage);
                return File(abytes, "application/pdf");
            }
        }

        private string GetSQL(DUDeliverySummaryRPT oDUDeliverySummaryRPT)
        {
            _sDateRange = "";

            string sSQLQuery = "", sWhereCluse = "", sGroupBy = "", sOrderBy = "";

            #region month
            if (oDUDeliverySummaryRPT.ReportLayout == 1)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + oDUDeliverySummaryRPT.StartDate.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + oDUDeliverySummaryRPT.EndDate.ToString("dd MMM yyyy") + "', 106))";
                //_sDateRange = "Challan Date Between " + dPIIssueStartDate.ToString("dd MMM yyyy 08:00:00") + " To " + dPIIssueEndDate.ToString("dd MMM yyyy 08:00:00");
            }
            #endregion

            #region day
            if (oDUDeliverySummaryRPT.ReportLayout == 2)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),ChallanDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + oDUDeliverySummaryRPT.StartDate.ToString("dd MMM yyyy") + "', 106))";
                //_sDateRange = "Challan Date @ " + dPIIssueStartDate.ToString("dd MMM yyyy 08:00:00");
            }
            #endregion

            #region Product
            if (oDUDeliverySummaryRPT.ReportLayout == 3)
            {
                if (oDUDeliverySummaryRPT.RefID > 0)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " ProductID IN(" + oDUDeliverySummaryRPT.RefID + ")";
                }
            }
            #endregion

            #region Contractor
            if (oDUDeliverySummaryRPT.ReportLayout == 4)
            {
                if (oDUDeliverySummaryRPT.RefID > 0)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " ContractorID IN(" + oDUDeliverySummaryRPT.RefID + ")";
                }
            }

            #endregion

            #region MKT person
            if (oDUDeliverySummaryRPT.ReportLayout == 5)
            {
                if (oDUDeliverySummaryRPT.RefID > 0)
                {
                    Global.TagSQL(ref sWhereCluse);
                    sWhereCluse = sWhereCluse + " MKTEmpID IN(" + oDUDeliverySummaryRPT.RefID + ")";
                }
            }

            #endregion

            sSQLQuery = ""; sGroupBy = ""; sOrderBy = "";
            sSQLQuery = "SELECT * FROM View_DUDeliveryChallanRegister ";
            sOrderBy = " ORDER BY  ChallanDate, DUDeliveryChallanID, DUDeliveryChallanDetailID ASC";

            sSQLQuery = sSQLQuery + sWhereCluse + sGroupBy + sOrderBy;
            return sSQLQuery;
        }

        [HttpGet]
        public ActionResult PrintOrderTypeWisePreview(string sTempString)
        {
            string _sTempString = sTempString;
            _oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
            _oDUDeliverySummaryRPT.BUID = Convert.ToInt32(_sTempString.Split('~')[0]);
            _oDUDeliverySummaryRPT.ReportLayout = Convert.ToInt32(_sTempString.Split('~')[1]);
            _oDUDeliverySummaryRPT.StartDate = Convert.ToDateTime(_sTempString.Split('~')[2]);
            _oDUDeliverySummaryRPT.EndDate = Convert.ToDateTime(_sTempString.Split('~')[3]);
            try
            {
                _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
                _oDUDeliverySummaryRPTs = DUDeliverySummaryRPT.GetsData(_oDUDeliverySummaryRPT, (int)Session[SessionInfo.currentUserID]);
            }

            catch (Exception ex)
            {
                 _oDUDeliverySummaryRPT = new DUDeliverySummaryRPT();
                 _oDUDeliverySummaryRPTs = new List<DUDeliverySummaryRPT>();
            }

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            if (_oDUDeliverySummaryRPTs.Count>0)
            {
                rptDUDeliveryOrderWiseSummary oReport = new rptDUDeliveryOrderWiseSummary();
                string sDateRange = "Date Between "+_oDUDeliverySummaryRPT.StartDate.ToString("dd MMM yyyy") + " To " + _oDUDeliverySummaryRPT.EndDate.ToString("dd MMM yyyy");
                byte[] abytes = oReport.PrepareReport(_oDUDeliverySummaryRPTs, oCompany, _oDUDeliverySummaryRPT.ReportLayout, sDateRange);
                return File(abytes, "application/pdf");

            }
             else
            {
                rptErrorMessage oReport = new rptErrorMessage();
                byte[] abytes = oReport.PrepareReport("No Data Found!!");
                return File(abytes, "application/pdf");
            }

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
        #endregion

    }

}
