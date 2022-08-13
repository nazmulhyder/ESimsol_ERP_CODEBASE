using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using System.Web.Script.Serialization;
using ESimSol.Reports;
using System.IO;
using System.Xml.Serialization;
using ESimSol.BusinessObjects.ReportingObject;
using System.Drawing;
using System.Drawing.Imaging;
using ICS.Core.Utility;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class HomeController : Controller
    {
        #region Declaration
        List<MerchandiserDashboard> _oMerchandiserDashboards = new List<MerchandiserDashboard>();
        #endregion
        public ActionResult Index()
        {
            
            ViewBag.Message = "Welcome to ASP.NET MVC!";
            return View();
        }
        public ActionResult About()
        {
            return View();
        }
        public ActionResult MainAction(string action, string controller, double ts)
        {
            return RedirectToAction(action, controller);
        }

        #region Merchandiser Deshboard
        public ActionResult MerchandiserDashBoard(int menuid,  int EmployeeID, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            if (menuid == 0)
            {
                string sMainSQL = "SELECT TechnicalSheetID, BuyerID, StyleNo FROM View_TAP WHERE MerchandiserID = " + EmployeeID + "  AND ISNULL(ApprovedBy,0)!=0  AND TAPID In (SELECT DISTINCT TPD.TAPID FROM TAPDetail AS TPD WHERE TAPDetailID IN ((SELECT TAPDetailID  FROM TAPExecution WHERE ISNULL(IsDone,0)=0) UNION (SELECT TAPDetailID FROM TAPDetail WHERE TAPDetailID NOT IN (SELECT TAPDetailID FROM TAPExecution))) AND CONVERT(DATE,CONVERT(VARCHAR(12), TPD.ApprovalPlanDate))<=CONVERT(DATE, CONVERT(VARCHAR(12), '" + DateTime.Today.ToString("dd MMM yyyy") + "')))";
                string sORSQL = "(SELECT Count(*) aS NOOfOR FROM OrderRecap WHERE MerchandiserID = " + EmployeeID + " AND TechnicalSheetID =";
                string sCSSQL = "(SELECT Count(*) aS NOOfCS FROM CostSheet WHERE MerchandiserID = " + EmployeeID + " AND TechnicalSheetID =";
                string sPOSQL = "(SELECT Count(*) AS NOOfPO FROM View_GUProductionOrder WHERE   MerchandiserID =  " + EmployeeID + " AND   TechnicalSheetID =";
                string sPESQL = "(SELECT Count(*) AS NOOfPEP FROM View_ProductionExecutionPlan WHERE MerchandiserID =  " + EmployeeID + " AND TechnicalSheetID =";
                string sPendingSQL = "(SELECT Count(*) AS Pending FROM TAPDetail WHERE TAPDetailID IN ((SELECT TAPDetailID  FROM TAPExecution WHERE ISNULL(IsDone,0)=0) UNION (SELECT TAPDetailID FROM TAPDetail WHERE TAPDetailID NOT IN (SELECT TAPDetailID FROM TAPExecution))) AND CONVERT(DATE, CONVERT(VARCHAR(12), ApprovalPlanDate))< CONVERT(DATE, CONVERT(VARCHAR(12), GETDATE())) AND TAPID IN (SELECT MM.TAPID FROM View_TAP as MM  WHERE  MM.MerchandiserID =  " + EmployeeID + " AND  MM.TechnicalSheetID =";
                string sCompleteSQL = "(SELECT Count(*) AS CompleteTask FROM TAPDetail WHERE TAPDetailID IN (SELECT TAPDetailID  FROM TAPExecution WHERE ISNULL(IsDone,0) = 1) AND TAPID IN (SELECT MM.TAPID FROM View_TAP as MM  WHERE  MM.MerchandiserID =  " + EmployeeID + " AND  MM.TechnicalSheetID =";
                _oMerchandiserDashboards = new List<MerchandiserDashboard>();
                _oMerchandiserDashboards = MerchandiserDashboard.Gets(sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
            }
            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oMerchandiserDashboards);
        }


        [HttpPost]
        public JsonResult SearchMerchandiserDashboard(MerchandiserDashboard oMerchandiserDashboard)
        {
            
            _oMerchandiserDashboards = new List<MerchandiserDashboard>();
            try
            {
                string sCommonSQL = "AND TechnicalSheetID ="; string sTempSQL = ""; string sTempSQL2 = ""; string sExtraSQLForOR = "";
                string sMainSQL = "SELECT TechnicalSheetID, BuyerID, StyleNo FROM TechnicalSheet";
                string sORSQL = "(SELECT Count(*) aS NOOfOR FROM OrderRecap ";
                string sCSSQL = "(SELECT Count(*) aS NOOfCS FROM View_CostSheet ";
                string sPOSQL = "(SELECT Count(*) AS NOOfPO FROM View_GUProductionOrder ";
                string sPESQL = "(SELECT Count(*) AS NOOfPEP FROM View_ProductionExecutionPlan ";
                string sPendingSQL = "(SELECT Count(*) AS Pending FROM TAPDetail WHERE TAPDetailID IN ((SELECT TAPDetailID  FROM TAPExecution WHERE ISNULL(IsDone,0)=0) UNION (SELECT TAPDetailID FROM TAPDetail WHERE TAPDetailID NOT IN (SELECT TAPDetailID FROM TAPExecution))) AND CONVERT(DATE, CONVERT(VARCHAR(12), ApprovalPlanDate))< CONVERT(DATE, CONVERT(VARCHAR(12), GETDATE())) AND TAPID IN (SELECT MM.TAPID FROM View_TAP as MM  ";
                string sCompleteSQL = "(SELECT Count(*) AS CompleteTask FROM TAPDetail WHERE TAPDetailID IN (SELECT TAPDetailID  FROM TAPExecution WHERE ISNULL(IsDone,0) = 1) AND TAPID IN (SELECT MM.TAPID FROM View_TAP as MM  ";
                #region Data split
                string sBuyerIDs = oMerchandiserDashboard.sParam.Split('~')[0];
                string sDeptIDs = oMerchandiserDashboard.sParam.Split('~')[1];
                string sMerchandiserIDs = oMerchandiserDashboard.sParam.Split('~')[2];
                int nSesionID = Convert.ToInt32(oMerchandiserDashboard.sParam.Split('~')[3]);
                short nCompareStyleDate = Convert.ToInt16(oMerchandiserDashboard.sParam.Split('~')[4]);
                DateTime dStartStyleDate = Convert.ToDateTime(oMerchandiserDashboard.sParam.Split('~')[5]);
                DateTime dEndStyleDate = Convert.ToDateTime(oMerchandiserDashboard.sParam.Split('~')[6]);
                int BUID = Convert.ToInt32(oMerchandiserDashboard.sParam.Split('~')[7]);
                #endregion

                #region BU
                if (BUID>0)
                {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL +=" BUID ="+BUID;
                }
                #endregion
               
                #region BuyerID
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL +=" BuyerID IN ("+sBuyerIDs+")";
                }
                #endregion

              

                #region MerchandiserID
                if (!string.IsNullOrEmpty(sMerchandiserIDs))
                {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL += " MerchandiserID IN (" + sMerchandiserIDs + ")";
                }
                #endregion

                #region Sesion
                if (nSesionID>0)
                {
                    Global.TagSQL(ref sTempSQL);
                    sTempSQL +=" BusinessSessionID ="+nSesionID;

                }
                #endregion
                sExtraSQLForOR = sTempSQL;
                #region DeptID
                if (!string.IsNullOrEmpty(sDeptIDs))
                {
                    Global.TagSQL(ref sTempSQL);
                    Global.TagSQL(ref sExtraSQLForOR);

                    sTempSQL += " Dept IN (" + sDeptIDs + ")";
                    sExtraSQLForOR +=" TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE  Dept IN (" + sDeptIDs + "))";
                }
                #endregion

              
                sTempSQL2 = sTempSQL;
                #region DBServerDateTime
                if (nCompareStyleDate != 0)
                {
                    Global.TagSQL(ref sTempSQL);
                    Global.TagSQL(ref sExtraSQLForOR);
                    sTempSQL2 = sTempSQL;
                    sTempSQL += Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false);
                    sTempSQL2 += " TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE " + Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false)+")";
                    sExtraSQLForOR += " TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE " + Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false) + ")";
                    
                }
                #endregion

                sMainSQL += sTempSQL;
                sORSQL +=sExtraSQLForOR+sCommonSQL;


                sCSSQL += sTempSQL2 + sCommonSQL;
                sPOSQL += sTempSQL2 + sCommonSQL;
                sPESQL += sTempSQL2 + sCommonSQL;
                sPendingSQL += sTempSQL2 + sCommonSQL;
                sCompleteSQL += sTempSQL2 + sCommonSQL;


                this.Session.Remove(SessionInfo.ParamObj);
                this.Session.Add(SessionInfo.ParamObj, sTempSQL2);

                this.Session.Remove(SessionInfo.SearchData);
                this.Session.Add(SessionInfo.SearchData, sMainSQL+'~'+sPOSQL+'~'+sORSQL+'~'+sCSSQL+'~'+sPESQL+'~'+sPendingSQL+'~'+sCompleteSQL);
                _oMerchandiserDashboards = MerchandiserDashboard.Gets(sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
            }
            catch (Exception ex)
            {
                oMerchandiserDashboard = new MerchandiserDashboard();
                oMerchandiserDashboard.ErrorMessage = ex.Message;
                _oMerchandiserDashboards.Add(oMerchandiserDashboard);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMerchandiserDashboards);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #region GetsPickerValue
        [HttpPost]
        public JsonResult GetsPickerValue(MerchandiserDashboard oMerchandiserDashboard)
        {
            _oMerchandiserDashboards = new List<MerchandiserDashboard>();
            List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            List<CostSheet> oCostSheets = new List<CostSheet>();
            List<GUProductionOrder> oGUProductionOrders = new List<GUProductionOrder>();
            List<ProductionExecutionPlan> oProductionExecutionPlans = new List<ProductionExecutionPlan>();
            List<TAPDetail> oPendingList = new List<TAPDetail>();
            List<TAPDetail> oCompleteList = new List<TAPDetail>();
            string sSQL = "";
            #region Data split
            int nTSID = Convert.ToInt32(oMerchandiserDashboard.sParam.Split('~')[0]);
            int nOperationType = Convert.ToInt32(oMerchandiserDashboard.sParam.Split('~')[1]);
            string sPickerQuyeryCommon = (string)Session[SessionInfo.ParamObj];

            

            #endregion
            try
            {

                //nOperationType :1:Order Recap, 2:Cost Sheet, 3:PO, 4:PE, 5:Pending, 6:Complete
                switch (nOperationType)
                {
                    case 1:
                        sSQL = "SELECT * FROM View_OrderRecap " + sPickerQuyeryCommon + " AND TechnicalSheetID = " + nTSID;
                        oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;

                    case 2:
                        sSQL = "SELECT * FROM View_CostSheet  " + sPickerQuyeryCommon + " AND TechnicalSheetID = " + nTSID;
                        oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;
                    case 3:
                        sSQL = "SELECT * FROM View_GUProductionOrder " + sPickerQuyeryCommon + " AND TechnicalSheetID = " + nTSID;
                        oGUProductionOrders = GUProductionOrder.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;
                    case 4:
                        sSQL = "SELECT * FROM View_ProductionExecutionPlan " + sPickerQuyeryCommon + " AND TechnicalSheetID = " + nTSID;
                        oProductionExecutionPlans = ProductionExecutionPlan.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;

                    case 5:
                        sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID IN (SELECT TAPID FROM View_TAP " + sPickerQuyeryCommon + " AND TechnicalSheetID = " + nTSID+") AND ISNULL(ExecutionIsDone,0)=0  " ;
                        oPendingList = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;

                    case 6:
                        sSQL = "SELECT * FROM View_TAPDetail WHERE TAPID IN (SELECT TAPID FROM View_TAP " + sPickerQuyeryCommon + ") AND ISNULL(ExecutionIsDone,0)=1 AND TechnicalSheetID = " + nTSID;
                        oCompleteList = TAPDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;

                }
            }
            catch (Exception ex)
            {
                oMerchandiserDashboard = new MerchandiserDashboard();
                oMerchandiserDashboard.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if (nOperationType == 1)//OR
            {
                sjson = serializer.Serialize(oOrderRecaps);
            }
            else if (nOperationType == 2)//CS
            {
                sjson = serializer.Serialize(oCostSheets);
            }
            else if (nOperationType == 3)//PO
            {
                sjson = serializer.Serialize(oGUProductionOrders);
            }
            else if (nOperationType == 4)//PE
            {
                sjson = serializer.Serialize(oProductionExecutionPlans);
            }
            else if (nOperationType == 5)//Pending
            {
                sjson = serializer.Serialize(oPendingList);
            }
            else if (nOperationType == 6)//compete
            {
                sjson = serializer.Serialize(oCompleteList);
            }
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print


        public ActionResult PrintDeshboard(int nBUID)
        {
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string sValue = (string)Session[SessionInfo.SearchData];
            string sMainSQL = sValue.Split('~')[0];
            string sPOSQL = sValue.Split('~')[1];
            string sORSQL = sValue.Split('~')[2];
            string sCSSQL = sValue.Split('~')[3];
            string sPESQL = sValue.Split('~')[4];
            string sPendingSQL = sValue.Split('~')[5];
            string sCompleteSQL = sValue.Split('~')[6];

            _oMerchandiserDashboards = MerchandiserDashboard.Gets(sMainSQL, sPOSQL, sORSQL, sCSSQL, sPESQL, sPendingSQL, sCompleteSQL);
            if (nBUID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            byte[] abytes;
            rptMerchandiserDashboard oReport = new rptMerchandiserDashboard();
            abytes = oReport.PrepareReport(_oMerchandiserDashboards, oBusinessUnit, oCompany);
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
        #endregion

        #endregion





        #region Management Deshboard
        public ActionResult ViewManagementDashboard(int sessionid)
        {
            string sSQL = "";
            List<BusinessSession> oBusinessSessions = new List<BusinessSession>();
            oBusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
            if (sessionid <= 0)
            {
                if (oBusinessSessions.Count > 0)
                {
                    sessionid = oBusinessSessions[oBusinessSessions.Count - 1].BusinessSessionID;
                }
            }
            sSQL = "SELECT ORP.TechnicalSheetID, ORP.MerchandiserID, ORP.BuyerID FROM OrderRecap AS ORP WHERE ORP.BusinessSessionID=" + sessionid.ToString();
            List<ManagementDashboard> oManagementDashboards = new List<ManagementDashboard>();
            oManagementDashboards = ManagementDashboard.Gets(sSQL, sessionid, (int)Session[SessionInfo.currentUserID]);
            ViewBag.BusinessSessions = oBusinessSessions;
            ViewBag.BusinessSessionID = sessionid;
            return View(oManagementDashboards);
        }

        [HttpPost]
        public JsonResult GetsNumberOfStyles(ManagementDashboard oManagementDashboard)
        {
            List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
            string sSQL = "SELECT * FROM View_TechnicalSheet AS TS WHERE TS.TechnicalSheetID IN (SELECT TechnicalSheetID FROM OrderRecap WHERE MerchandiserID =" + oManagementDashboard.MerchandiserID.ToString() + " AND BusinessSessionID=" + oManagementDashboard.BusinessSessionID.ToString() + ") ORDER BY BuyerID, TechnicalSheetID";
            oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNumberOfDevelopments(ManagementDashboard oManagementDashboard)
        {
            List<DevelopmentRecap> oDevelopmentRecaps = new List<DevelopmentRecap>();
            string sSQL = "SELECT * FROM View_DevelopmentRecap AS DR WHERE DR.TechnicalSheetID IN(SELECT ORP.TechnicalSheetID FROM OrderRecap AS ORP WHERE ORP.MerchandiserID=" + oManagementDashboard.MerchandiserID.ToString() + " AND ORP.BusinessSessionID=" + oManagementDashboard.BusinessSessionID.ToString() + ") ORDER BY BuyerID, DevelopmentRecapID";
            oDevelopmentRecaps = DevelopmentRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDevelopmentRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNumberOfOrders(ManagementDashboard oManagementDashboard)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap AS SO WHERE  SO.MerchandiserID=" + oManagementDashboard.MerchandiserID.ToString() + " AND SO.BusinessSessionID=" + oManagementDashboard.BusinessSessionID.ToString() + " ORDER BY BuyerID, OrderRecapID";
            oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNumberOfTAPOrders(ManagementDashboard oManagementDashboard)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap AS SO WHERE SO.MerchandiserID=" + oManagementDashboard.MerchandiserID.ToString() + " AND SO.BusinessSessionID=" + oManagementDashboard.BusinessSessionID.ToString() + " AND SO.IsTAPExist = 1  ORDER BY BuyerID, OrderRecapID";
            oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNumberOfBuyers(ManagementDashboard oManagementDashboard)
        {
            List<Contractor> oContractors = new List<Contractor>();
            string sSQL = "SELECT * FROM Contractor AS B WHERE B.ContractorID IN(SELECT ORP.BuyerID FROM OrderRecap AS ORP WHERE ORP.MerchandiserID=" + oManagementDashboard.MerchandiserID.ToString() + " AND ORP.BusinessSessionID=" + oManagementDashboard.BusinessSessionID + ") ORDER BY ContractorID";
            oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsNumberOfFactorys(ManagementDashboard oManagementDashboard)
        {
            List<Contractor> oContractors = new List<Contractor>();
            string sSQL = "SELECT * FROM Contractor AS B WHERE B.ContractorID IN(SELECT SO.ProductionFactoryID FROM View_OrderRecap AS SO WHERE SO.MerchandiserID=" + oManagementDashboard.MerchandiserID.ToString() + " AND SO.BusinessSessionID=" + oManagementDashboard.BusinessSessionID.ToString() + ") ORDER BY ContractorID";
            oContractors = Contractor.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oContractors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
        public ActionResult Modal()
        {
            return PartialView();
        }
        public ActionResult AdvanceSearch()
        {
            return PartialView();
        }
        public ActionResult ModalMessage()
        {
            return PartialView();
        }


    }
}
