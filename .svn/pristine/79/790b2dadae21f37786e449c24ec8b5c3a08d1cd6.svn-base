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
//using Microsoft.Office.Interop.Word;
using System.Reflection;


namespace ESimSolFinancial.Controllers
{
    public class TimeCardController : Controller
    {
        #region Dynamic Time Card
        public ActionResult PrintTimeCardConf(string sTemp)
        {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBlockIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];
            int nMOCID = Convert.ToInt32(sTemp.Split('~')[10]);

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCardMaxOTConf(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBlockIDs, sGroupIDs, nMOCID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

            if (sBUnitIDs != "")
            {
                oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }

            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            rptTimeCard_F6_1 oReport = new rptTimeCard_F6_1();
            byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
            return File(abytes, "application/pdf");
        }

        #endregion
        #region Static(From Enum) Time Card
        public ActionResult PrintTimeCard(string sTemp, int nType, string sVersion)
         {
            AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

            #region Searching And Get Data
            string sEmployeeIDs = sTemp.Split('~')[0];
            DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
            string sLocationID = sTemp.Split('~')[3];
            string sDepartmentIds = sTemp.Split('~')[4];
            string sBUnitIDs = sTemp.Split('~')[5];
            double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
            double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
            string sBMMIDs = sTemp.Split('~')[8];
            string sGroupIDs = sTemp.Split('~')[9];

            List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
            
            #region set Version Parameter
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F2) sVersion = "F2";
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F2_1) sVersion = "F2.1";
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F3) sVersion = "F3";
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F4) sVersion = "F4";
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F6) sVersion = "F6";
            #endregion

            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F6 || nType == (int)EnumEmployeeTimeCard.Time_Card_F3 || nType == (int)EnumEmployeeTimeCard.Time_Card_F2_1 || nType == (int)EnumEmployeeTimeCard.Time_Card_FC7)
            {
                AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sVersion, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            else
            {
                AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCard(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, sVersion, sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
            #endregion

            #region Basic Information
            List<Company> oCompanys = new List<Company>();
            oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
            oAttendanceDaily_ZN.Company = oCompanys.First();
            oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
            oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
            #endregion

            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F1)
            {
                rptMamiyaTimeCard oReport = new rptMamiyaTimeCard();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F2 || nType == (int)EnumEmployeeTimeCard.Time_Card_F3)
            {
                oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
                if (sBUnitIDs != "")
                {
                    oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                rptTimeCard_F2 oReport = new rptTimeCard_F2();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F4)
            {
                rptTimeCard_F4 oReport = new rptTimeCard_F4();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F5)
            {
                rptTimeCard_F6 oReport = new rptTimeCard_F6();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_F6)
            {
                rptTimeCard_F7 oReport = new rptTimeCard_F7();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_FC7)
            {
                rptTimeCard_FC7 oReport = new rptTimeCard_FC7();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_AMG_Worker)
            {
                oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_F6_1 oReport = new rptTimeCard_F6_1();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Time_Card_Worker)
            {
                oAttendanceDaily_ZN.LeaveHeads = LeaveHead.Gets("SELECT * FROM LeaveHead", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                rptTimeCard_Worker oReport = new rptTimeCard_Worker();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            if (nType == (int)EnumEmployeeTimeCard.Job_Card)
            {
                oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;
                if (sBUnitIDs != "")
                {
                    oAttendanceDaily_ZN.BusinessUnits = BusinessUnit.Gets("SELECT * FROM View_BusinessUnit WHERE BusinessUnitID IN(" + sBUnitIDs + ")", ((User)(Session[SessionInfo.CurrentUser])).UserID);
                }
                rptJobCard oReport = new rptJobCard();
                byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
                return File(abytes, "application/pdf");
            }
            return RedirectToAction("~/blank");
        }
        //public ActionResult PrintTimeCard_F7(string sTemp)
        //{
        //    AttendanceDaily_ZN oAttendanceDaily_ZN = new AttendanceDaily_ZN();

        //    string sEmployeeIDs = sTemp.Split('~')[0];
        //    DateTime Startdate = Convert.ToDateTime(sTemp.Split('~')[1]);
        //    DateTime EndDate = Convert.ToDateTime(sTemp.Split('~')[2]);
        //    string sLocationID = sTemp.Split('~')[3];
        //    string sDepartmentIds = sTemp.Split('~')[4];
        //    string sBUnitIDs = sTemp.Split('~')[5];
        //    double nStartSalaryRange = Convert.ToDouble(sTemp.Split('~')[6]);
        //    double nEndSalaryRange = Convert.ToDouble(sTemp.Split('~')[7]);
        //    string sBMMIDs = sTemp.Split('~')[8];
        //    string sGroupIDs = sTemp.Split('~')[9];

        //    List<AttendanceDaily_ZN> AttendanceDaily_ZNs = new List<AttendanceDaily_ZN>();
        //    AttendanceDaily_ZNs = AttendanceDaily_ZN.GetsTimeCardComp(sEmployeeIDs, Startdate, EndDate, sLocationID, sDepartmentIds, "", sBUnitIDs, nStartSalaryRange, nEndSalaryRange, sBMMIDs, sGroupIDs, ((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.AttendanceDaily_ZNs = AttendanceDaily_ZNs;

        //    List<Company> oCompanys = new List<Company>();
        //    oCompanys = Company.Gets(((User)(Session[SessionInfo.CurrentUser])).UserID);
        //    oAttendanceDaily_ZN.Company = oCompanys.First();
        //    oAttendanceDaily_ZN.Company.CompanyLogo = GetImage(oAttendanceDaily_ZN.Company.OrganizationLogo);
        //    oAttendanceDaily_ZN.ErrorMessage = Startdate.ToString("dd MMM yyyy") + "~" + EndDate.ToString("dd MMM yyyy");
        //    rptTimeCard_F7 oReport = new rptTimeCard_F7();
        //    byte[] abytes = oReport.PrepareReport(oAttendanceDaily_ZN);
        //    return File(abytes, "application/pdf");
        //}
        #endregion
        public Image GetImage(byte[] Image)
        {
            if (Image != null)
            {
                string fileDirectory = Server.MapPath("~/Content/Image.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }

                MemoryStream m = new MemoryStream(Image);
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
