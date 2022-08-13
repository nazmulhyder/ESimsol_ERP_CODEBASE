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
	public class MerchanidisingReportController : Controller
	{
		#region Declaration

		MerchanidisingReport _oMerchanidisingReport = new MerchanidisingReport();
		List<MerchanidisingReport> _oMerchanidisingReports = new  List<MerchanidisingReport>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewMerchanidisingReport(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			_oMerchanidisingReports = new List<MerchanidisingReport>();
            ViewBag.BusinessSessions = BusinessSession.Gets((int)Session[SessionInfo.currentUserID]);
			return View(_oMerchanidisingReports);
		}


		[HttpPost]
		public JsonResult Gets(MerchanidisingReport oMerchanidisingReport)
		{
            _oMerchanidisingReports = new List<MerchanidisingReport>(); 
			try
			{
                string sCommonSQL = ""; string sTempSQL = ""; string sTSIDsSQL = ""; string sPickerQuyeryTS = ""; string sPickerQueryOR = ""; string sPickerQueryCS = ""; string sPickerQueryTAP = ""; string sPickerQueryCommon = "";
                string sMainSQL = "";  string sTSSQL = ""; string sORSQL = ""; string sCSSQL = ""; string sTAPSQL = "";
                #region Data split
                string sBuyerIDs = oMerchanidisingReport.sParam.Split('~')[0];
                string sDeptIDs = oMerchanidisingReport.sParam.Split('~')[1];
                int nSessionID = Convert.ToInt32(oMerchanidisingReport.sParam.Split('~')[2]);
                short nCompareStyleDate = Convert.ToInt16( oMerchanidisingReport.sParam.Split('~')[3]);
                DateTime dStartStyleDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[4]);
                DateTime dEndStyleDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[5]);
                short nCompareORDate = Convert.ToInt16(oMerchanidisingReport.sParam.Split('~')[6]);
                DateTime dStartORDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[7]);
                DateTime dEndORDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[8]);
                short nCompareCSDate = Convert.ToInt16(oMerchanidisingReport.sParam.Split('~')[9]);
                DateTime dStartCSDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[10]);
                DateTime dEndCSDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[11]);
                short nCompareTAPDate = Convert.ToInt16(oMerchanidisingReport.sParam.Split('~')[12]);
                DateTime dStartTAPDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[13]);
                DateTime dEndTAPDate = Convert.ToDateTime(oMerchanidisingReport.sParam.Split('~')[14]);
                int BUID = Convert.ToInt32(oMerchanidisingReport.sParam.Split('~')[15]);
                #endregion

                #region Make Get TSIDs GET
                
                if(nCompareStyleDate!=0)
                {
                    sTSIDsSQL += " TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE " + Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false) + ")";
                }
                #endregion

                #region Make common sql
                if (!string.IsNullOrEmpty(sDeptIDs))
                {
                    Global.TagSQL(ref sCommonSQL);
                    sCommonSQL += " Dept IN ("+sDeptIDs+")";
                    sPickerQueryCommon += " AND Dept IN (" + sDeptIDs + ")";
                    if (nCompareStyleDate != 0) { sTSIDsSQL += " AND Dept IN (" + sDeptIDs + ")"; } else { sTSIDsSQL = ""; } //for TS IDS
                }
                if (nSessionID>0)
                {
                    Global.TagSQL(ref sCommonSQL);
                    sCommonSQL += " BusinessSessionID =" + nSessionID;
                    sPickerQueryCommon += " AND  BusinessSessionID =" + nSessionID;
                    //for TS IDS
                    if (nCompareStyleDate != 0) { sTSIDsSQL += " AND BusinessSessionID =" + nSessionID; } else { sTSIDsSQL = ""; } //for TS IDS
                }
                if (BUID > 0)
                {
                    Global.TagSQL(ref sCommonSQL);
                    sCommonSQL += " BUID =" + BUID;
                    sPickerQueryCommon += " AND BUID =" + BUID;
                    //for TS IDS
                    if (nCompareStyleDate != 0) { sTSIDsSQL += " AND BUID =" + BUID; } else { sTSIDsSQL = ""; } //for TS IDS
                }
                sPickerQuyeryTS = sTSIDsSQL;//SET TS Path
                #endregion

                #region MainSQL Make
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    sMainSQL = "SELECT * FROM dbo.SplitInToDataSet('" + sBuyerIDs + "',',')";
                }else
                {
                    sMainSQL = "SELECT DISTINCT(BuyerID) FROM TechnicalSheet ";
                    if(nCompareStyleDate!=0)
                    {
                        if (string.IsNullOrEmpty(sCommonSQL))
                        {
                            sTempSQL = " WHERE ";
                        }
                        else
                        {
                            sTempSQL = " AND ";
                        }
                        sTempSQL += Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false);
                    }

                    sMainSQL += sCommonSQL+sTempSQL;
                    sTempSQL = "";//reset
                }
                #endregion

                #region TS SQL
                sTSSQL = "(SELECT COUNT(*) AS TSCount FROM TechnicalSheet";
                if (string.IsNullOrEmpty(sCommonSQL))
                {
                    sTempSQL = " WHERE ";
                }
                else
                {
                    sTempSQL = " AND ";
                }
                if (nCompareStyleDate != 0)
                {  
                    sTempSQL += Global.DateSQLGenerator("DBServerDateTime", nCompareStyleDate, dStartStyleDate, dEndStyleDate, false);
                    Global.TagSQL(ref sTempSQL);    
                }
                sTSSQL += sCommonSQL + sTempSQL + " BuyerID =";
                sTempSQL = "";//reset
                #endregion

                #region OR SQL
                sORSQL = "(SELECT COUNT(*) AS ORCount FROM View_OrderRecap";
                if (string.IsNullOrEmpty(sCommonSQL))
                {
                    sTempSQL = " WHERE ";
                }
                else
                {
                    sTempSQL = " AND ";
                }
                if (nCompareORDate != 0)
                {  
                    //sTempSQL += Global.DateSQLGenerator("OrderDate", nCompareORDate, dStartORDate, dEndORDate, false);
                    //sPickerQueryOR = " AND " + Global.DateSQLGenerator("OrderDate", nCompareORDate, dStartORDate, dEndORDate, false);
                    //requirement by Jahid 
                    sTempSQL += Global.DateSQLGenerator("DBServerDateTime", nCompareORDate, dStartORDate, dEndORDate, false);
                    sPickerQueryOR = " AND " + Global.DateSQLGenerator("DBServerDateTime", nCompareORDate, dStartORDate, dEndORDate, false);

                    Global.TagSQL(ref sTempSQL);
                }
                else if(nCompareStyleDate!=0)
                {
                   sTempSQL += sTSIDsSQL;
                   Global.TagSQL(ref sTempSQL);
                }
                sORSQL += sCommonSQL + sTempSQL + " BuyerID =";
                sTempSQL = "";//reset
                #endregion

                #region CS SQL
                sCSSQL = "(SELECT COUNT(*) AS CSCount FROM View_CostSheet";
                if (string.IsNullOrEmpty(sCommonSQL))
                {
                    sTempSQL = " WHERE ";
                }
                else
                {
                    sTempSQL = " AND ";
                }
                if (nCompareCSDate != 0)
                {
                    sTempSQL += Global.DateSQLGenerator("CostingDate", nCompareCSDate, dStartCSDate, dEndCSDate, false);
                    sPickerQueryCS = " AND "+Global.DateSQLGenerator("CostingDate", nCompareCSDate, dStartCSDate, dEndCSDate, false);
                    Global.TagSQL(ref sTempSQL);
                }
                else if (nCompareStyleDate != 0)
                {
                    sTempSQL += sTSIDsSQL;
                    Global.TagSQL(ref sTempSQL);
                }
                sCSSQL += sCommonSQL + sTempSQL + " BuyerID =";
                sTempSQL = "";//reset
                #endregion

                #region TAP SQL
                sTAPSQL = "(SELECT COUNT(*) AS CSCount FROM View_TAP";
                if (string.IsNullOrEmpty(sCommonSQL))
                {
                    sTempSQL = " WHERE ";
                }
                else
                {
                    sTempSQL = " AND ";
                }
                if (nCompareTAPDate != 0)
                {
                    sTempSQL += Global.DateSQLGenerator("PlanDate", nCompareTAPDate, dStartTAPDate, dEndTAPDate, false);
                    sPickerQueryTAP = "AND " + Global.DateSQLGenerator("PlanDate", nCompareTAPDate, dStartTAPDate, dEndTAPDate, false);
                    Global.TagSQL(ref sTempSQL);
                }
                else if (nCompareStyleDate != 0)
                {
                    sTempSQL += sTSIDsSQL;
                    Global.TagSQL(ref sTempSQL);
                }
                
                sTAPSQL += sCommonSQL + sTempSQL + " BuyerID =";
                sTempSQL = "";//reset
                #endregion

                this.Session.Remove(SessionInfo.ParamObj);
                this.Session.Add(SessionInfo.ParamObj, sPickerQuyeryTS+'~'+sPickerQueryOR+'~'+sPickerQueryCS+'~'+sPickerQueryTAP+'~'+sPickerQueryCommon);

                this.Session.Remove(SessionInfo.SearchData);
                this.Session.Add(SessionInfo.SearchData, sMainSQL+'~'+sTSSQL+'~'+sORSQL+'~'+sCSSQL+'~'+sTAPSQL);
                _oMerchanidisingReports = MerchanidisingReport.Gets(sMainSQL,sTSSQL, sORSQL, sCSSQL, sTAPSQL,(int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oMerchanidisingReport = new MerchanidisingReport();
				_oMerchanidisingReport.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oMerchanidisingReports);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

	
		#endregion
        #region GetsPickerValue
        [HttpPost]
        public JsonResult GetsPickerValue(MerchanidisingReport oMerchanidisingReport)
        {
            _oMerchanidisingReports = new List<MerchanidisingReport>();
            List<TechnicalSheet> oTechnicalSheets = new List<TechnicalSheet>();
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            List<CostSheet> oCostSheets = new List<CostSheet>();
            List<TAP> oTAPs = new List<TAP>();
            string sSQL = "";
            #region Data split
            int nBuyerID = Convert.ToInt32(oMerchanidisingReport.sParam.Split('~')[0]);
            int nOperationType = Convert. ToInt32(oMerchanidisingReport.sParam.Split('~')[1]);
            string sValue = (string)Session[SessionInfo.ParamObj];
            string sPickerQuyeryTS = sValue.Split('~')[0];
            string sPickerQueryOR = sValue.Split('~')[1];
            string sPickerQueryCS = sValue.Split('~')[2];
            string sPickerQueryTAP = sValue.Split('~')[3];
            string sPickerQueryCommon = sValue.Split('~')[4];

            #endregion
            try
            {
             
                //nOperationType :1:Style,2:Order Recap, 3:Cost Sheet, 4:TAP
                switch(nOperationType)
                {
                    case 1:
                        sSQL = "SELECT * FROM View_TechnicalSheet WHERE BuyerID ="+nBuyerID;
                        if (!string.IsNullOrEmpty(sPickerQuyeryTS)) { sSQL += " AND " + sPickerQuyeryTS; }
                        oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;
                    case 2:
                        sSQL = "SELECT * FROM View_OrderRecap WHERE BuyerID =" + nBuyerID;
                        if (!string.IsNullOrEmpty(sPickerQueryOR)) { sSQL += sPickerQueryOR + sPickerQueryCommon; } else if (!string.IsNullOrEmpty(sPickerQuyeryTS)) { sSQL += " AND " + sPickerQuyeryTS; }
                        oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;

                    case 3:
                        sSQL = "SELECT * FROM View_CostSheet WHERE BuyerID =" + nBuyerID;
                        if (!string.IsNullOrEmpty(sPickerQueryCS)) { sSQL += sPickerQueryCS + sPickerQueryCommon; } else if (!string.IsNullOrEmpty(sPickerQuyeryTS)) { sSQL += " AND " + sPickerQuyeryTS; }
                        oCostSheets = CostSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;
                    case 4:
                        sSQL = "SELECT * FROM View_TAP WHERE BuyerID =" + nBuyerID;
                        if (!string.IsNullOrEmpty(sPickerQueryTAP)) { sSQL += sPickerQueryTAP + sPickerQueryCommon; } else if (!string.IsNullOrEmpty(sPickerQuyeryTS)) { sSQL += " AND " + sPickerQuyeryTS; }
                        oTAPs = TAP.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                        break;
                }
            }
            catch (Exception ex)
            {
                _oMerchanidisingReport = new MerchanidisingReport();
                _oMerchanidisingReport.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = "";
            if(nOperationType==1)//Style
            {
                sjson = serializer.Serialize(oTechnicalSheets);
            }
            else if (nOperationType == 2)//OR
            {
                sjson = serializer.Serialize(oOrderRecaps);
            }
            else if (nOperationType == 3)//CS
            {
                sjson = serializer.Serialize(oCostSheets);
            }
            else //TAP
            {
                sjson = serializer.Serialize(oTAPs);
            }            
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 
        #endregion

        #region Print


        public ActionResult PrintMerchandisingReport(int  nBUID)
        {
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            BusinessUnit oBusinessUnit = new BusinessUnit();
            string sValue = (string)Session[SessionInfo.SearchData];
            string sMainSQL = sValue.Split('~')[0];
            string sTSSQL = sValue.Split('~')[1];
            string sORSQL = sValue.Split('~')[2];
            string sCSSQL = sValue.Split('~')[3];
            string sTAPSQL = sValue.Split('~')[4];

            _oMerchanidisingReports = MerchanidisingReport.Gets(sMainSQL, sTSSQL, sORSQL, sCSSQL, sTAPSQL, (int)Session[SessionInfo.currentUserID]);
            if (nBUID > 0)
            {
                oBusinessUnit = oBusinessUnit.Get(nBUID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            
            byte[] abytes;
            rptMerchanidisingReport oReport = new rptMerchanidisingReport();
            abytes = oReport.PrepareReport(_oMerchanidisingReports, oBusinessUnit, oCompany);
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
    }

}
