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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Diagnostics;

namespace ESimSolFinancial.Controllers
{
	public class FDCRegisterController : Controller
	{
		#region Declaration
        string _sFormatter = "", _sErrorMesage = "", _sDateMsg = "";
        FDCRegister _oFDCRegister = new FDCRegister();
        List<FDCRegister> _oFDCRegisters = new List<FDCRegister>();
		#endregion

		#region Actions
		public ActionResult ViewFDCRegisters(int menuid, int buid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            ViewBag.DOTypes = EnumObject.jGets(typeof(EnumDOType));
            User oUser = new User();


            ViewBag.ApproveByList = ESimSol.BusinessObjects.User.GetsBySql("select * from View_User as VU WHere VU.UserID in (Select FDC.ApproveBy from FabricDeliveryChallan as FDC )", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DisburseByList = ESimSol.BusinessObjects.User.GetsBySql("select * from View_User as VU WHere VU.UserID in (Select FDC.DisburseBy from FabricDeliveryChallan as FDC )", ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            ViewBag.MktPersons = MarketingAccount.GetsByBU(buid, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
			return View(_oFDCRegister);
		}

        public ActionResult View_FDORegisters(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryOrder).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            
            ViewBag.BUID = buid;
            ViewBag.DOTypes = EnumObject.jGets(typeof(EnumDOType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oFDCRegister);
        }
        public ActionResult View_FDCRegisters(int menuid, int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.BUID = buid;
            ViewBag.DOTypes = EnumObject.jGets(typeof(EnumDOType));
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            return View(_oFDCRegister);
        }

        #region FDC Search
        [HttpPost]
        public JsonResult AdvSearch(FDCRegister oFDCRegister)
        {
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            FDCRegister _oFDCRegister = new FDCRegister();
            string sSQL = MakeSQL_FDO(oFDCRegister);
            if (sSQL == "Error")
            {
                _oFDCRegister = new FDCRegister();
                _oFDCRegister.ErrorMessage = "Please select a valid searching critaria.";
                oFDCRegisters = new List<FDCRegister>();
            }
            else
            {
                oFDCRegisters = new List<FDCRegister>();
                oFDCRegisters = FDCRegister.Gets_FDO(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDCRegisters.Count == 0)
                {
                    oFDCRegisters = new List<FDCRegister>();
                }
            }
            var jsonResult = Json(oFDCRegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL_FDO(FDCRegister oFDCRegister)
        {
            string sReturn1 = "";
            string sReturn = "";

            #region Challan No
            if (!string.IsNullOrEmpty(oFDCRegister.ChallanNo) && oFDCRegister.ChallanNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryChallan WHERE ChallanNo LIKE '%" + oFDCRegister.ChallanNo + "%')";
            }
            #endregion
            #region DO No
            if (!string.IsNullOrEmpty(oFDCRegister.DONo) && oFDCRegister.DONo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrder WHERE DONo LIKE '%" + oFDCRegister.DONo + "%')";
            }
            #endregion
            #region DoType
            if (oFDCRegister.DOType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrder WHERE FDOType =" + (int)oFDCRegister.DOType+")";
            }
            #endregion
            #region Construction
            if (!string.IsNullOrEmpty(oFDCRegister.Construction) && oFDCRegister.Construction != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID IN (SELECT FabricID FROM Fabric WHERE Construction LIKE '%" + oFDCRegister.Construction + "%')";
            }
            #endregion
            #region PINo
            if (!string.IsNullOrEmpty(oFDCRegister.PINo) && oFDCRegister.PINo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDCRegister.PINo + "%')";
            }
            #endregion
            #region LCNo
            if (!string.IsNullOrEmpty(oFDCRegister.LCNo) && oFDCRegister.LCNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExportPIID IN (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID in (Select ExportLCID from ExportLC where ExportLCNo like '%" + oFDCRegister.LCNo + "%'))";
            }
            #endregion
            #region Dispo No
            if (!string.IsNullOrEmpty(oFDCRegister.ExeNo) && oFDCRegister.ExeNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ExeNo LIKE '%" + oFDCRegister.ExeNo + "%')";
            }
            #endregion
            #region SC No
            if (!string.IsNullOrEmpty(oFDCRegister.SCNoFull) && oFDCRegister.SCNoFull != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FEOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE FabricSalesContractID IN (SELECT FabricSalesContractID FROM FabricSalesContract WHERE SCNo  LIKE '%" + oFDCRegister.SCNoFull + "%'))";
            }
            #endregion
            #region Fabric No
            if (!string.IsNullOrEmpty(oFDCRegister.FabricNo) && oFDCRegister.FabricNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID IN (SELECT FabricID FROM Fabric WHERE FabricNo LIKE '%" + oFDCRegister.SCNoFull + "%')";
            }
            #endregion

            #region MKTPerson
            if (!string.IsNullOrEmpty(oFDCRegister.MKTPerson) && oFDCRegister.MKTPerson != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "(ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in  (" + oFDCRegister.MKTPerson + "))   or ( isnull(ExportPIID,0)<=0  and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in  (" + oFDCRegister.MKTPerson + ")))))";
            }
            #endregion

            #region MKTGroup
            if (!string.IsNullOrEmpty(oFDCRegister.MKTGroup) && oFDCRegister.MKTGroup != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " (ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in (Select MarketingAccountID from MarketingAccount where GroupID in (" + oFDCRegister.MKTGroup + ")))  or ( isnull(ExportPIID,0)<=0  and FEOID in (Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in (Select MarketingAccountID from MarketingAccount where GroupID in (" + oFDCRegister.MKTGroup + "))))))";
            }
            #endregion

            #region Get Value From Param (Date Search) 

            int cboChallanDate = 0;
            DateTime ChallanStartDate = DateTime.Today;
            DateTime ChallanEndDate = DateTime.Today;
            int cboDODate = 0;
            DateTime DOStartDate = DateTime.Today;
            DateTime DOEndDate = DateTime.Today;

            if (oFDCRegister.DCDateSearchString.Split('~').Count() == 3) 
            {
                int nCount = 0;
                cboChallanDate = Convert.ToInt32(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
                ChallanStartDate = Convert.ToDateTime(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
                ChallanEndDate = Convert.ToDateTime(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
            }
            if (oFDCRegister.DCDateSearchString.Split('~').Count() == 3)
            {
                int nCount = 0;
                cboDODate = Convert.ToInt32(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
                DOStartDate = Convert.ToDateTime(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
                DOEndDate = Convert.ToDateTime(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
            }
            #endregion

            #region CHALLAN DATE SEARCH
            if (cboChallanDate > 0)
            {
                Global.TagSQL(ref sReturn);

                string sDateQuery = "";
                DateObject.CompareDateQuery(ref sDateQuery, "IssueDate", cboChallanDate, ChallanStartDate, ChallanEndDate);
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryChallan " + sDateQuery +")";
            }
            #endregion

            #region DO DATE SEARCH
            if (cboDODate > 0)
            {
                Global.TagSQL(ref sReturn);

                string sDateQuery = "";
                DateObject.CompareDateQuery(ref sDateQuery, "DODate", cboDODate, DOStartDate, DOEndDate);
                sReturn = sReturn + " FDOID IN (SELECT FDOID FROM FabricDeliveryOrder " + sDateQuery + ")";
            }
            #endregion
          
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        #endregion

        [HttpPost]
        public JsonResult AdvSearch_DC(FDCRegister oFDCRegister)
        {
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            FDCRegister _oFDCRegister = new FDCRegister();
            string sSQL = MakeSQL_FDC(oFDCRegister);
            if (sSQL == "Error")
            {
                _oFDCRegister = new FDCRegister();
                _oFDCRegister.ErrorMessage = "Please select a valid searching critaria.";
                oFDCRegisters = new List<FDCRegister>();
            }
            else
            {
                oFDCRegisters = new List<FDCRegister>();
                oFDCRegisters = FDCRegister.Gets_FDC(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDCRegisters.Count == 0)
                {
                    oFDCRegisters = new List<FDCRegister>();
                }
            }
            var jsonResult = Json(oFDCRegisters, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL_FDC(FDCRegister oFDCRegister)
        {

            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            string sReturn1 = "";
            string sReturn = " where FDCID IN (SELECT FDCID FROM FabricDeliveryChallan WHERE ISNULL(FabricDeliveryChallan.DisburseBy,0)<>0 )";

            #region Challan No
            if (!string.IsNullOrEmpty(oFDCRegister.ChallanNo) && oFDCRegister.ChallanNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM FabricDeliveryChallan WHERE ChallanNo LIKE '%" + oFDCRegister.ChallanNo + "%')";
            }
            #endregion
            #region DO No
            if (!string.IsNullOrEmpty(oFDCRegister.DONo) && oFDCRegister.DONo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FDOID IN (SELECT FDOID FROM FabricDeliveryOrder WHERE DONo LIKE '%" + oFDCRegister.DONo + "%'))";
            }
            #endregion
            #region DoType
            if (oFDCRegister.DOType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FDOID IN (SELECT FDOID FROM FabricDeliveryOrder WHERE FDOType =" + (int)oFDCRegister.DOType + "))";
            }
            #endregion
            #region Construction
            if (!string.IsNullOrEmpty(oFDCRegister.Construction) && oFDCRegister.Construction != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FabricID IN (SELECT FabricID FROM Fabric WHERE Construction LIKE '%" + oFDCRegister.Construction + "%'))";
            }
            #endregion
            #region PINo
            if (!string.IsNullOrEmpty(oFDCRegister.PINo) && oFDCRegister.PINo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE ExportPIID IN (SELECT ExportPIID FROM ExportPI WHERE PINo LIKE '%" + oFDCRegister.PINo + "%'))";
            }
            #region LCNo
            if (!string.IsNullOrEmpty(oFDCRegister.LCNo) && oFDCRegister.LCNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE ExportPIID IN (Select ExportPIID from ExportPILCMapping where Activity=1 and ExportLCID in (Select ExportLCID from ExportLC where ExportLCNo like '%" + oFDCRegister.LCNo + "%')))";
            }
            #endregion
            #endregion
            #region Dispo No
            if (!string.IsNullOrEmpty(oFDCRegister.ExeNo) && oFDCRegister.ExeNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FEOID IN (SELECT FabricSalesContractDetailID FROM FabricSalesContractDetail WHERE ExeNo LIKE '%" + oFDCRegister.ExeNo + "%'))";
            }
            #endregion
            #region SC No
            if (!string.IsNullOrEmpty(oFDCRegister.SCNoFull) && oFDCRegister.SCNoFull != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "  FDODID IN (SELECT FDC.FDODID FROM FabricDeliveryOrderDetail AS FDC WHERE FDC.FEOID IN (SELECT FSC.FabricSalesContractDetailID FROM FabricSalesContractDetail AS FSC WHERE FSC.FabricSalesContractID IN (SELECT DD.FabricSalesContractID FROM FabricSalesContract AS DD WHERE DD.SCNo  LIKE '%" + oFDCRegister.SCNoFull + "%')))";
            }
            #endregion
            #region Fabric No
            if (!string.IsNullOrEmpty(oFDCRegister.FabricNo) && oFDCRegister.FabricNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FabricID IN (SELECT FabricID FROM Fabric WHERE FabricNo '%" + oFDCRegister.SCNoFull + "%'))";
            }
            #endregion

            if (string.IsNullOrEmpty(oFDCRegister.MKTPerson))
            {
              oMarketingAccounts = MarketingAccount.GetsByBUAndGroup(oFDCRegister.BUID, (int)Session[SessionInfo.currentUserID]);
                if (oMarketingAccounts.Count > 0)
                {
                    oFDCRegister.MKTPerson = string.Join(",", oMarketingAccounts.Select(x => x.MarketingAccountID).ToList());
                }
            }


            #region MKTPerson
            if (!string.IsNullOrEmpty(oFDCRegister.MKTPerson) && oFDCRegister.MKTPerson != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE  (ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in (" + oFDCRegister.MKTPerson + ")))  or ( isnull(ExportPIID,0)<=0  and FabricDeliveryOrderDetail.FEOID in (   Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in (" + oFDCRegister.MKTPerson + ")))))";
            }
            #endregion

            #region MKTGroup
            if (!string.IsNullOrEmpty(oFDCRegister.MKTGroup) && oFDCRegister.MKTGroup != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE  (ExportPIID>0 and ExportPIID in (Select ExportPIID from  ExportPI where MKTEMPID in (Select MarketingAccountID from MarketingAccount where GroupID in (" + oFDCRegister.MKTGroup + "))))  or ( isnull(ExportPIID,0)<=0  and FabricDeliveryOrderDetail.FEOID in (   Select FabricSalesContractDetailID from  FabricSalesContractDetail where FabricSalesContractID in (Select FabricSalesContractID from FabricSalesContract where MktAccountID in (Select MarketingAccountID from MarketingAccount where GroupID in (" + oFDCRegister.MKTGroup + "))))))";
            }
            #endregion

            #region Get Value From Param (Date Search)

            int cboChallanDate = 0;
            DateTime ChallanStartDate = DateTime.Today;
            DateTime ChallanEndDate = DateTime.Today;
            int cboDODate = 0;
            DateTime DOStartDate = DateTime.Today;
            DateTime DOEndDate = DateTime.Today;

            if (oFDCRegister.DCDateSearchString.Split('~').Count() == 3)
            {
                int nCount = 0;
                cboChallanDate = Convert.ToInt32(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
                ChallanStartDate = Convert.ToDateTime(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
                ChallanEndDate = Convert.ToDateTime(oFDCRegister.DCDateSearchString.Split('~')[nCount++]);
            }
            if (oFDCRegister.DCDateSearchString.Split('~').Count() == 3)
            {
                int nCount = 0;
                cboDODate = Convert.ToInt32(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
                DOStartDate = Convert.ToDateTime(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
                DOEndDate = Convert.ToDateTime(oFDCRegister.DODateSearchString.Split('~')[nCount++]);
            }
            #endregion

            #region CHALLAN DATE SEARCH
            if (cboChallanDate > 0)
            {
                Global.TagSQL(ref sReturn);

                string sDateQuery = "";
                DateObject.CompareDateQuery(ref sDateQuery, "IssueDate", cboChallanDate, ChallanStartDate, ChallanEndDate);
                sReturn = sReturn + " FDCID IN (SELECT FDCID FROM FabricDeliveryChallan " + sDateQuery + ")";
                
                if (cboChallanDate == (int)EnumCompareOperator.EqualTo)
                    _sDateMsg = "Date: Equal to " + ChallanStartDate.ToString("dd MMM yyyy");
                else if (cboChallanDate == (int)EnumCompareOperator.NotEqualTo)
                    _sDateMsg = "Date: Not Equal to " + ChallanStartDate.ToString("dd MMM yyyy");
                else if (cboChallanDate == (int)EnumCompareOperator.GreaterThan)
                    _sDateMsg = "Date: Greater Than " + ChallanStartDate.ToString("dd MMM yyyy");
                else if (cboChallanDate == (int)EnumCompareOperator.SmallerThan)
                    _sDateMsg = "Date: Smaller Than " + ChallanStartDate.ToString("dd MMM yyyy");
                else if (cboChallanDate == (int)EnumCompareOperator.Between)
                    _sDateMsg = "Date: Between " + ChallanStartDate.ToString("dd MMM yyyy") + " To " + ChallanEndDate.ToString("dd MMM yyyy");
                else if (cboChallanDate == (int)EnumCompareOperator.NotBetween)
                    _sDateMsg = "Date: Not Between " + ChallanStartDate.ToString("dd MMM yyyy") + " To " + ChallanEndDate.ToString("dd MMM yyyy");
            }
            #endregion

            #region DO DATE SEARCH
            if (cboDODate > 0)
            {
                Global.TagSQL(ref sReturn);

                string sDateQuery = "";
                DateObject.CompareDateQuery(ref sDateQuery, "DODate", cboDODate, DOStartDate, DOEndDate);
                sReturn = sReturn + " FDODID IN (SELECT FDODID FROM FabricDeliveryOrderDetail WHERE FDOID IN (SELECT FDOID FROM FabricDeliveryOrder " + sDateQuery + "))";
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
       
        #endregion

        [HttpPost]
        public ActionResult SetSessionSearchCriteria(FDCRegister oFDCRegister)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFDCRegister);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        private FDCRegister MakeObject(string sString)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            if (!String.IsNullOrEmpty(sString))
            {
                int nCount = 12;
                oFDCRegister.Params = sString;
                oFDCRegister.ChallanNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.DONo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.DOType = (EnumDOType)Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.StoreName = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.StyleNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.DriverName = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.DeliveryPoient = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.ApproveBy = Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.DisburseBy = Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.ProductName = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.Construction = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.LCNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.PINo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.MKTPersonID = Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.ExeNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.SCNoFull = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.LotNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.ContractorName = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.BuyerName = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.FabricNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.Printlayout = Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.BUID = Convert.ToInt32(sString.Split('~')[nCount++]);
                oFDCRegister.VehicleNo = Convert.ToString(sString.Split('~')[nCount++]);
                oFDCRegister.ChallanRemarks = Convert.ToString(sString.Split('~')[nCount++]);
            }
            return oFDCRegister;
        }
        
        #region Print PDF
        public ActionResult PrintFDCRegister(string sString)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
            oFDCRegister = MakeObject(oFDCRegister.Params);
            string sSQL = MakeSQLForReport(oFDCRegister);
            
            Company oCompany = new Company();
            oCompany = oCompany.Get(oFDCRegister.BUID, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            rptFDCRegister oReport = new rptFDCRegister();
            rptErrorMessage oErrorReport = new rptErrorMessage();
            byte[] abytes = new byte[1];
            if (oFDCRegister.Printlayout == 1)
            {
                oFDCRegisters = FDCRegister.Gets(sSQL + " ORDER BY ChallanDate, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDCRegisters.Count>0)
                {
                    abytes = oReport.PrepareReportDateWise(oCompany, "Date Wise Fabric Delivery Challan", oFDCRegisters, oFDCRegister);
                }
                else
                {
                    abytes = oErrorReport.PrepareReport("No Data");
                }
            }
            else if (oFDCRegister.Printlayout == 2)
            {
                oFDCRegisters = FDCRegister.Gets(sSQL + " ORDER BY MKTPersonID, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDCRegisters.Count > 0)
                {
                    abytes = oReport.PrepareReportMKTPersonWise(oCompany, "Marketing Person Wise Fabric Delivery Challan", oFDCRegisters, oFDCRegister);
                }
                else
                {
                    abytes = oErrorReport.PrepareReport("No Data");
                }
            }
            else if (oFDCRegister.Printlayout == 3)
            {
                oFDCRegisters = FDCRegister.Gets(sSQL + " ORDER BY ContractorID, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFDCRegisters.Count > 0)
                {
                    abytes = oReport.PrepareReportPartyWise(oCompany, "Party Wise Fabric Delivery Challan", oFDCRegisters, oFDCRegister);
                }
                else
                {
                    abytes = oErrorReport.PrepareReport("No Data");
                }
            }
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

        #region Print Excel
        private string MakeSQLForReport(FDCRegister oFDCRegister)
        {
            string sReturn1 = "select * from View_FabricDeliveryChallanRegister";
            string sReturn = "";

            #region Challan No
            if (!string.IsNullOrEmpty(oFDCRegister.ChallanNo) && oFDCRegister.ChallanNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanNo LIKE '%" + oFDCRegister.ChallanNo + "%'";
            }
            #endregion
            #region DONo
            if (!string.IsNullOrEmpty(oFDCRegister.DONo) && oFDCRegister.DONo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDONo LIKE '%" + oFDCRegister.DONo + "%'";
            }
            #endregion
            #region DoType
            if (oFDCRegister.DOType > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FDOType =" + (int)oFDCRegister.DOType;
            }
            #endregion
            #region Store Name
            if (!string.IsNullOrEmpty(oFDCRegister.StoreName) && oFDCRegister.StoreName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StoreName LIKE '%" + oFDCRegister.StoreName + "%'";
            }
            #endregion
            #region Style No
            if (!string.IsNullOrEmpty(oFDCRegister.StyleNo) && oFDCRegister.StyleNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo LIKE '%" + oFDCRegister.StyleNo + "%'";
            }
            #endregion
            #region Driver Name
            if (!string.IsNullOrEmpty(oFDCRegister.DriverName) && oFDCRegister.DriverName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DriverName LIKE '%" + oFDCRegister.DriverName + "%'";
            }
            #endregion
            #region Delivery poient
            if (!string.IsNullOrEmpty(oFDCRegister.DeliveryPoient) && oFDCRegister.DeliveryPoient != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DeliveryPoint LIKE '%" + oFDCRegister.DeliveryPoient + "%'";
            }
            #endregion
            #region ApproveBy
            if (oFDCRegister.ApproveBy > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApproveBy =" + oFDCRegister.ApproveBy;
            }
            #endregion
            #region Disburse
            if (oFDCRegister.DisburseBy > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " DisburseBy =" + oFDCRegister.DisburseBy;
            }
            #endregion
            #region ProductName
            if (!string.IsNullOrEmpty(oFDCRegister.ProductName) && oFDCRegister.ProductName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductID IN (" + oFDCRegister.ProductName + ")";
            }
            #endregion
            #region Construction
            if (!string.IsNullOrEmpty(oFDCRegister.Construction) && oFDCRegister.Construction != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " Construction LIKE '%" + oFDCRegister.Construction + "%'";
            }
            #endregion
            #region LCNo
            if (!string.IsNullOrEmpty(oFDCRegister.LCNo) && oFDCRegister.LCNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LCNo LIKE '%" + oFDCRegister.LCNo + "%'";
            }
            #endregion
            #region PINo
            if (!string.IsNullOrEmpty(oFDCRegister.PINo) && oFDCRegister.PINo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " PINo LIKE '%" + oFDCRegister.PINo + "%'";
            }
            #endregion
            #region MKT Person
            if (!string.IsNullOrEmpty(oFDCRegister.MKTPerson) && oFDCRegister.MKTPerson != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MKTPerson =" + oFDCRegister.MKTPerson;
            }
            #endregion
            #region Vchicle
            if (!string.IsNullOrEmpty(oFDCRegister.VehicleNo) && oFDCRegister.VehicleNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " VehicleNo LIKE '%" + oFDCRegister.VehicleNo + "%'";
            }
            #endregion
            #region Challan Remarks
            if (!string.IsNullOrEmpty(oFDCRegister.ChallanRemarks))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ChallanRemarks LIKE '%" + oFDCRegister.ChallanRemarks + "%'";
            }
            #endregion
            #region Dispo No
            if (!string.IsNullOrEmpty(oFDCRegister.ExeNo) && oFDCRegister.ExeNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ExeNo LIKE '%" + oFDCRegister.ExeNo + "%'";
            }
            #endregion
            #region SC No
            if (!string.IsNullOrEmpty(oFDCRegister.SCNoFull) && oFDCRegister.SCNoFull != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " SCNoFull LIKE '%" + oFDCRegister.SCNoFull + "%'";
            }
            #endregion
            #region Lot No
            if (!string.IsNullOrEmpty(oFDCRegister.LotNo) && oFDCRegister.LotNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LotNo LIKE '%" + oFDCRegister.LotNo + "%'";
            }
            #endregion
            #region Party Name
            if (!string.IsNullOrEmpty(oFDCRegister.ContractorName) && oFDCRegister.ContractorName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN (" + oFDCRegister.ContractorName + ")";
            }
            #endregion
            #region Buyer Name
            if (!string.IsNullOrEmpty(oFDCRegister.BuyerName) && oFDCRegister.BuyerName != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + oFDCRegister.BuyerName + ")";
            }
            #endregion
            #region Fabric No
            if (!string.IsNullOrEmpty(oFDCRegister.FabricNo) && oFDCRegister.FabricNo != "undefined")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FabricID IN (" + oFDCRegister.FabricNo + ")";
            }
            #endregion
            #region Get Value From Param (Date Search)
            string sParams = oFDCRegister.Params;
            int cboChallanDate = 0;
            DateTime ChallanStartDate = DateTime.Today;
            DateTime ChallanEndDate = DateTime.Today;
            int cboDODate = 0;
            DateTime DOStartDate = DateTime.Today;
            DateTime DOEndDate = DateTime.Today;
            int cboApproveDate = 0;
            DateTime ApproveStartDate = DateTime.Today;
            DateTime ApproveEndDate = DateTime.Today;
            int cboDisburseDate = 0;
            DateTime DisburseStartDate = DateTime.Today;
            DateTime DisburseEndDate = DateTime.Today;

            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                cboChallanDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                ChallanStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                ChallanEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                cboDODate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                DOStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                DOEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                cboApproveDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                ApproveStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                ApproveEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                cboDisburseDate = Convert.ToInt32(sParams.Split('~')[nCount++]);
                DisburseStartDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                DisburseEndDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
            }
            #endregion

            #region CHALLAN DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " ChallanDate", cboChallanDate, ChallanStartDate, ChallanEndDate);
            #endregion
            #region DO DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " DODate", cboDODate, DOStartDate, DOEndDate);
            #endregion
            #region APPROVE DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " ApproveDate", cboApproveDate, ApproveStartDate, ApproveEndDate);
            #endregion
            #region DISVURSE DATE SEARCH
            DateObject.CompareDateQuery(ref sReturn, " DisburseDate", cboDisburseDate, DisburseStartDate, DisburseEndDate);
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        public void GetExcelDateWise(string sParam)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
            oFDCRegister = MakeObject(oFDCRegister.Params);
            string sSQL = MakeSQLForReport(oFDCRegister);
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            oFDCRegisters = FDCRegister.Gets(sSQL + " ORDER BY ChallanDate, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFDCRegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Party", Width = 35f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Buyer Name", Width = 35f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Store Name", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan By", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "MKT Person", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Code", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Construction", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Lot No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan Qty", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                sheet.Name = "FDC Register Report (Date Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Fabric Delivery Challan(Date Wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;

                string sPreviousChallanDate = "";
                int nChallanID = 0;
                double dSubTotal = 0.0;
                double dDateWiseTotal = 0.0;

                foreach (var obj in oFDCRegisters)
                {
                    nStartCol = 2;
                    
                    ExcelTool.Formatter = "";
                    if (sPreviousChallanDate != obj.ChallanDateSt)
                    {
                        ExcelTool.FillCellMerge(ref sheet, "Challan Date : " + obj.ChallanDateSt, nRowIndex, nRowIndex++, nStartCol, 20, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 19, true, ExcelHorizontalAlignment.Right,ExcelVerticalAlignment.Top, true);
                        ExcelTool.FillCellMerge(ref sheet, dSubTotal.ToString(), nRowIndex, nRowIndex++, 20,20, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (sPreviousChallanDate != obj.ChallanDateSt && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Date Wise Sub Total:", nRowIndex, nRowIndex, nStartCol, 19, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCellMerge(ref sheet, dDateWiseTotal.ToString(), nRowIndex, nRowIndex++, 20,20, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID)
                    {
                        int nRowSpan = oFDCRegisters.Where(x => x.ChallanDateSt.Equals(obj.ChallanDateSt) && x.FDCID == obj.FDCID).Count()-1;
                        dSubTotal = dSubTotal + obj.Qty;
                        ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top,false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ChallanNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DONo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.FDODateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ContractorName , nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.BuyerName, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.StoreName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DisburseByName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.MKTPerson.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    }
                    else
                    {
                        nStartCol = 11;
                    }
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo.ToString(), false);
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExeNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Construction.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SCNoFull.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo.ToString(), false);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, false);
                    //ExcelTool.FillCellMerge(ref sheet, obj.Qty.ToString(), nRowIndex, nRowIndex + 1, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    sPreviousChallanDate = obj.ChallanDateSt;
                    nChallanID = obj.FDCID;
                    dSubTotal = oFDCRegisters.Where(x => x.FDCID.Equals(obj.FDCID)).Sum(x => x.Qty);
                    dDateWiseTotal = oFDCRegisters.Where(x => x.ChallanDateSt.Equals(obj.ChallanDateSt)).Sum(x => x.Qty);

                }
                #region Grand Total
                double dGrandTotal = oFDCRegisters.Sum(x => x.Qty);
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 19, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 20, 20, true, ExcelHorizontalAlignment.Right, true);
                #endregion
                
                
                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FDCRegister-Report-DateWise.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void GetExcelPartyWise(string sParam)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
            oFDCRegister = MakeObject(oFDCRegister.Params);
            string sSQL = MakeSQLForReport(oFDCRegister);
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            oFDCRegisters = FDCRegister.Gets(sSQL + " ORDER BY ContractorID, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFDCRegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan Date", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Store Name", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan By", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "MKT Person", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Code", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 12f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Construction", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Lot No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan Qty", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                sheet.Name = "FDC Register Report";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Fabric Delivery Challan(Party Wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;

                int nPreviousContractorID = 0;
                int nChallanID = 0;
                double dSubTotal = 0.0;
                double dPartyWiseTotal = 0.0;

                foreach (var obj in oFDCRegisters)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (nPreviousContractorID != obj.ContractorID)
                    {
                        ExcelTool.FillCellMerge(ref sheet, "Party Name : " + obj.ContractorName, nRowIndex, nRowIndex++, nStartCol, 19, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCellMerge(ref sheet, dSubTotal.ToString(), nRowIndex, nRowIndex++, 19,19, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (nPreviousContractorID != obj.ContractorID && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Party Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCellMerge(ref sheet, dPartyWiseTotal.ToString(), nRowIndex, nRowIndex++, 19,19, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID)
                    {
                        int nRowSpan = oFDCRegisters.Where(x => x.ChallanDateSt.Equals(obj.ChallanDateSt) && x.FDCID == obj.FDCID).Count() - 1;
                        dSubTotal = dSubTotal + obj.Qty;
                        ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ChallanNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DONo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.FDODateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ChallanDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.StoreName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DisburseByName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.MKTPerson.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    }
                    else
                    {
                        nStartCol = 10;
                    }
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo.ToString(), false);
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExeNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Construction.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SCNoFull.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo.ToString(), false);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, false);
                    //ExcelTool.FillCellMerge(ref sheet, obj.Qty.ToString(), nRowIndex, nRowIndex + 1, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    nPreviousContractorID = obj.ContractorID;
                    nChallanID = obj.FDCID;
                    dSubTotal = oFDCRegisters.Where(x => x.FDCID.Equals(obj.FDCID)).Sum(x => x.Qty);
                    dPartyWiseTotal = oFDCRegisters.Where(x => x.ContractorID.Equals(obj.ContractorID)).Sum(x => x.Qty);
                }
                #region Grand Total
                double dGrandTotal = oFDCRegisters.Sum(x => x.Qty);
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 19, 19, true, ExcelHorizontalAlignment.Right, true);
                #endregion


                #endregion
                nRowIndex++;
                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FDCRegister-Report-PartyWise.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        public void GetExcelMKTPersonWise(string sParam)
        {
            FDCRegister oFDCRegister = new FDCRegister();
            oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
            oFDCRegister = MakeObject(oFDCRegister.Params);
            string sSQL = MakeSQLForReport(oFDCRegister);
            List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
            oFDCRegisters = FDCRegister.Gets(sSQL + "  ORDER BY MKTPersonID, ChallanNo", ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFDCRegister.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            #region Header
            List<TableHeader> table_header = new List<TableHeader>();
            table_header.Add(new TableHeader { Header = "#SL", Width = 7f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan Date", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Store Name", Width = 30f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan By", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Party Name", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Code", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Product Name", Width = 25f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Construction", Width = 15f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "LC No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "PI No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            //table_header.Add(new TableHeader { Header = "Lot No", Width = 18f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "M Unit", Width = 10f, IsRotate = false, Align = TextAlign.Center });
            table_header.Add(new TableHeader { Header = "Challan Qty", Width = 18f, IsRotate = false, Align = TextAlign.Right });
            #endregion

            #region Export Excel
            int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count() + nStartCol;
            ExcelRange cell; ExcelFill fill;
            OfficeOpenXml.Style.Border border;

            using (var excelPackage = new ExcelPackage())
            {
                excelPackage.Workbook.Properties.Author = "ESimSol";
                excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                sheet.Name = "FDC Register Report(Marketing Person Wise)";

                ExcelTool.SetColumnWidth(table_header, ref sheet, ref nStartCol, ref nEndCol);
                nEndCol = 25;
                #region Report Header
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Name; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = "Fabric Delivery Challan(Marketing Person Wise)"; cell.Style.Font.Bold = true;
                cell.Style.WrapText = true;
                cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex = nRowIndex + 1;
                #endregion

                #region Address & Date
                cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol]; cell.Merge = true;
                cell.Value = oBusinessUnit.Address; cell.Style.Font.Bold = true;
                cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                nRowIndex++;

                #endregion

                #region Data
                nRowIndex++;
                nStartCol = 2;
                ExcelTool.GenerateHeader(table_header, ref sheet, ref nRowIndex, 2, nEndCol, 10, true, true);
                int nCount = 0; nEndCol = table_header.Count() + nStartCol;

                int nPreviousMarketingPersonID = 0;
                int nChallanID = 0;
                double dSubTotal = 0.0;
                double dPartyWiseTotal = 0.0;

                foreach (var obj in oFDCRegisters)
                {
                    nStartCol = 2;

                    ExcelTool.Formatter = "";
                    if (nPreviousMarketingPersonID != obj.MKTPersonID)
                    {
                        ExcelTool.FillCellMerge(ref sheet, "Marketing Person Name : " + obj.MKTPerson, nRowIndex, nRowIndex++, nStartCol, 19, true, ExcelHorizontalAlignment.Left, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Sub Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCellMerge(ref sheet, dSubTotal.ToString(), nRowIndex, nRowIndex++, 19,19, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (nPreviousMarketingPersonID != obj.MKTPersonID && nCount > 0)
                    {
                        nStartCol = 2;
                        ExcelTool.FillCellMerge(ref sheet, "Marketing Person Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                        ExcelTool.FillCellMerge(ref sheet, dPartyWiseTotal.ToString(), nRowIndex, nRowIndex++, 19,19, true, ExcelHorizontalAlignment.Right, true);
                        nStartCol = 2;
                    }
                    if (nChallanID != obj.FDCID)
                    {
                        int nRowSpan = oFDCRegisters.Where(x => x.ChallanDateSt.Equals(obj.ChallanDateSt) && x.FDCID == obj.FDCID).Count() - 1;
                        dSubTotal = dSubTotal + obj.Qty;
                        ExcelTool.FillCellMerge(ref sheet, (++nCount).ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ChallanNo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DONo, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.FDODateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ChallanDateSt, nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.StoreName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.DisburseByName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                        ExcelTool.FillCellMerge(ref sheet, obj.ContractorName.ToString(), nRowIndex, nRowIndex + nRowSpan, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Top, false);
                    }
                    else
                    {
                        nStartCol = 10;
                    }
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.StyleNo.ToString(), false);
                    ExcelTool.Formatter = "";
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ProductName, false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.FabricNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.ExeNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Construction.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LCNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.SCNoFull.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.PINo.ToString(), false);
                    //ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.LotNo.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.MUName.ToString(), false);
                    ExcelTool.FillCell(sheet, nRowIndex, nStartCol++, obj.Qty.ToString(), true, false);
                    //ExcelTool.FillCellMerge(ref sheet, obj.Qty.ToString(), nRowIndex, nRowIndex + 1, nStartCol, nStartCol++, false, ExcelHorizontalAlignment.Right, false);
                    nRowIndex++;
                    nPreviousMarketingPersonID = obj.MKTPersonID;
                    nChallanID = obj.FDCID;
                    dSubTotal = oFDCRegisters.Where(x => x.FDCID.Equals(obj.FDCID)).Sum(x => x.Qty);
                    dPartyWiseTotal = oFDCRegisters.Where(x => x.ContractorID.Equals(obj.ContractorID)).Sum(x => x.Qty);

                }
                #region Grand Total
                double dGrandTotal = oFDCRegisters.Sum(x => x.Qty);
                nStartCol = 2;
                ExcelTool.FillCellMerge(ref sheet, "Grand Total:", nRowIndex, nRowIndex, nStartCol, 18, true, ExcelHorizontalAlignment.Right, true);
                ExcelTool.FillCellMerge(ref sheet, dGrandTotal.ToString(), nRowIndex, nRowIndex++, 19, 19, true, ExcelHorizontalAlignment.Right, true);
                #endregion


                #endregion
                nRowIndex++;

                cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 2];
                fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                fill.BackgroundColor.SetColor(Color.White);

                Response.ClearContent();
                Response.BinaryWrite(excelPackage.GetAsByteArray());
                Response.AddHeader("content-disposition", "attachment; filename=FDCRegister-Report-MktPersonWise.xlsx");
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Flush();
                Response.End();
            }
            #endregion
        }
        #endregion

        #region Print Excel (FD)
        public void ExportToExcelFDORegister(double ts)
        {
            Company oCompany = new Company();
            FDCRegister oFDCRegister = new FDCRegister();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDCRegisters = new List<FDCRegister>();
                oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL_FDO(oFDCRegister);
                _oFDCRegisters = FDCRegister.Gets_FDO(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFDCRegisters = new List<FDCRegister>();
                _sErrorMesage = ex.Message;
            }
            bool isAuthorized = false;
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.FabricDeliveryChallan && obj.OperationType == EnumRoleOperationType.RateView)
                {
                    isAuthorized = true;
                }
            }
            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });

                //table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "Challan Date", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Compsition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M Unit", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 15f, IsRotate = false });
                if (isAuthorized)
                {
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
                }
                table_header.Add(new TableHeader { Header = "DC Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To DC", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric Weave", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Width", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 20f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FDO Register");
                    sheet.Name = "FDO Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "FDO Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 2;
                    nRowIndex++;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    string sMUName = "";

                    #region Data
                    int nCount = 0, nRowSpan = 0;
                    foreach (var oItem in _oFDCRegisters)
                    {
                        nStartCol = 2;
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.DONo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FDODateSt, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.DOTypeStr, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Construction, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorInfo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.MUName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                        if (isAuthorized)
                        {
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.UnitPrice, 2).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.Amount, 2).ToString(), true);
                        }
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty_DC.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.YetToDC.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.SCNoFull, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.PINo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricWeave, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishTypeName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishWidth, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProcessTypeName, false);
                        nRowIndex++;

                        sMUName = oItem.MUName;
                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty).ToString(), true, true);
                    if (isAuthorized)
                    {
                        double avg = _oFDCRegisters.Sum(x => x.UnitPrice) / nCount;
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(avg, 2).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(_oFDCRegisters.Sum(x => x.Amount), 2).ToString(), true, true);
                    }
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty_DC).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.YetToDC).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = sMUName + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FDORegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        public void ExportToExcelFDCRegister(double ts)
        {
            Company oCompany = new Company();
            FDCRegister oFDCRegister = new FDCRegister();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDCRegisters = new List<FDCRegister>();
                oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL_FDC(oFDCRegister);
                _oFDCRegisters = FDCRegister.Gets_FDC(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oFDCRegisters = new List<FDCRegister>();
                _sErrorMesage = ex.Message;
            }
            bool isAuthorized = false;
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.FabricDeliveryChallan && obj.OperationType == EnumRoleOperationType.RateView)
                {
                    isAuthorized = true;
                }
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false });
                //table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Compsition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M Unit", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Qty", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Delivery", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To Delivery", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty Delivery", Width = 20f, IsRotate = false });
                if (isAuthorized)
                {
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
                }
                table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PONo", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric Weave", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Width", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 20f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                    sheet.Name = "FDC Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "FDC Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 2;
                    nRowIndex++;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    string sMUName = "";
                    int nFEOID = 0;
                    #region Data
                    int nCount = 0, nRowSpan = 0;
                    _oFDCRegisters = _oFDCRegisters.OrderBy(x => x.FEOID).ToList();
                    foreach (var oItem in _oFDCRegisters)
                    {
                        nStartCol = 1;
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ChallanNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ChallanDateSt, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.DOTypeStr, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.BuyerName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Construction, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ProductName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ColorInfo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.MUName, false);

                        if (oItem.FEOID != nFEOID)
                        {
                            int rowCount = (_oFDCRegisters.Count(x => x.FEOID == oItem.FEOID) - 1);
                            rowCount = (rowCount == -1) ? 0 : rowCount;
                            //FillCellMerge(sheet, nRowIndex, (nRowIndex + rowCount), ++nStartCol, nStartCol, Math.Round(oItem.QtyOrder, 2).ToString() );
                            cell = sheet.Cells[nRowIndex, ++nStartCol, (nRowIndex + rowCount), nStartCol]; cell.Merge = true; cell.Value = Math.Round(oItem.QtyOrder, 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //FillCell(sheet, nRowIndex + rowCount, ++nStartCol, Math.Round(oItem.QtyDelivery, 2).ToString(), true);
                            cell = sheet.Cells[nRowIndex, ++nStartCol, (nRowIndex + rowCount), nStartCol]; cell.Merge = true; cell.Value = Math.Round(oItem.QtyDelivery, 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            //FillCell(sheet, nRowIndex + rowCount, ++nStartCol, Math.Round(oItem.YetToDelivery, 2).ToString(), true);
                            cell = sheet.Cells[nRowIndex, ++nStartCol, (nRowIndex + rowCount), nStartCol]; cell.Merge = true; cell.Value = Math.Round((oItem.QtyOrder - oItem.QtyDelivery), 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;


                        }
                        else
                        {
                            nStartCol += 3;
                        }

                        //FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.QtyOrder, 2).ToString(), true);
                        //FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.QtyDelivery, 2).ToString(), true);
                        //FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.YetToDelivery, 2).ToString(), true);


                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.Qty, 2).ToString(), true);
                        if(isAuthorized)
                        {
                            FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.UnitPrice, 2).ToString(), true);
                            FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.Amount, 2).ToString(), true);
                        }
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.FabricNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.SCNoFull, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExeNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.PINo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.FabricWeave, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.FinishTypeName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.FinishWidth, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ProcessTypeName, false);
                        nRowIndex++;

                        sMUName = oItem.MUName;
                        nFEOID=oItem.FEOID;
                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);



                    var oresults = _oFDCRegisters.GroupBy(x => x.FEOID).Select(g => new
                    {
                        FEOID = g.FirstOrDefault().FEOID,
                        QtyOrder = g.FirstOrDefault().QtyOrder,
                        QtyDelivery = g.FirstOrDefault().QtyDelivery,
                        YetToDelivery = g.FirstOrDefault().QtyOrder - g.FirstOrDefault().QtyDelivery
                    });

                    FillCell(sheet, nRowIndex, ++nStartCol, oresults.Sum(x => x.QtyOrder).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oresults.Sum(x => x.QtyDelivery).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, oresults.Sum(x => x.YetToDelivery).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty).ToString(), true, true);

                    if(isAuthorized)
                    {
                        double avg = _oFDCRegisters.Sum(x => x.UnitPrice) / nCount;
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(avg, 2).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(_oFDCRegisters.Sum(x => x.Amount), 2).ToString(), true, true);
                    }
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = sMUName + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FDCRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        public void ExportToExcel_RPT_FDCTwo(double ts)
        {
            Company oCompany = new Company();
            FDCRegister oFDCRegister = new FDCRegister();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDCRegisters = new List<FDCRegister>();
                oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL_FDC(oFDCRegister);
                _oFDCRegisters = FDCRegister.Gets_For_FDC2(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oFDCRegisters = new List<FDCRegister>();
                _sErrorMesage = ex.Message;
            }
            bool isAuthorized = false;
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.FabricDeliveryChallan && obj.OperationType == EnumRoleOperationType.RateView)
                {
                    isAuthorized = true;
                }
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delivary Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Acc. Holder", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Garments Name", Width = 35f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "LC No/Bill", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 18f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Sample", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delivery Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Delivery", Width = 15f, IsRotate = false });
                if (isAuthorized)
                {
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 12f, IsRotate = false });
                }
                
                table_header.Add(new TableHeader { Header = "Remarks", Width = 20f, IsRotate = false });
                
                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                    sheet.Name = "FDC Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Fabric Delivery Challan Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 9, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Print Header
                    nStartCol = 2;
                    nRowIndex++;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    #endregion

                    #region Data
                    int nCount = 0;
                    //_oFDCRegisters = _oFDCRegisters.OrderBy(x => x.FEOID).ToList();
                    foreach (var oItem in _oFDCRegisters)
                    {
                        nStartCol = 1;
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, ++nStartCol, (++nCount).ToString("00"), true);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.DCDateSt, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.DCNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.BuyerName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.MKTPerson, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ContractorName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.Construction, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.LCNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.DONo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.DODateSt, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ProcessTypeName, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, oItem.ExeNo, false);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.SampleQty, 2).ToString(), true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.Qty, 2).ToString(), true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(oItem.QtyDelivery, 2).ToString(), true);
                        if (isAuthorized)
                        {
                            FillCell(sheet, nRowIndex, ++nStartCol, oItem.MUnit, false);
                        }
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                        nRowIndex++;
                    }
                    #endregion

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 11, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.SampleQty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Select(x=> (x.SampleQty+x.QtyDelivery)).Sum().ToString(), true, true);
                    
                    if (isAuthorized)
                    {
                        FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    }
                    
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = "#,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    nRowIndex += 2;

                    #region Second part
                    nStartCol = 6; _sFormatter = " #,##0;(#,##0)";

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 8]; cell.Merge = true;
                    cell.Value = _sDateMsg; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex++;

                    nStartCol = 6;
                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Type Of Category"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Qty"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = "Unit"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    nRowIndex++;

                    double nTotal = 0;
                    nStartCol = 6;
                    FillCell(sheet, nRowIndex, nStartCol++, "LC Yarn Dyeing", false);
                    double nTotalQty = _oFDCRegisters.Where(x => x.IsYD == true && x.IsPrint == false && (x.FDOType == (int)EnumDOType.Export || x.FDOType == (int)EnumDOType.Advance)).Sum(x => x.Qty);
                    FillCell(sheet, nRowIndex, nStartCol++, nTotalQty.ToString(), true);
                    FillCell(sheet, nRowIndex, nStartCol++, "YD", false);
                    nRowIndex++; nTotal += nTotalQty;

                    nStartCol = 6;
                    FillCell(sheet, nRowIndex, nStartCol++, "L/C Solid Dyeing", false);
                    nTotalQty = _oFDCRegisters.Where(x => x.IsYD == false && x.IsPrint == false && (x.FDOType == (int)EnumDOType.Export || x.FDOType == (int)EnumDOType.Advance)).Sum(x => x.Qty);
                    FillCell(sheet, nRowIndex, nStartCol++, nTotalQty.ToString(), true);
                    FillCell(sheet, nRowIndex, nStartCol++, "YD", false);
                    nRowIndex++; nTotal += nTotalQty;

                    nStartCol = 6;
                    FillCell(sheet, nRowIndex, nStartCol++, "L/C Y/D Print Fabric", false);
                    nTotalQty = _oFDCRegisters.Where(x => x.IsYD == true && x.IsPrint == true && (x.FDOType == (int)EnumDOType.Export || x.FDOType == (int)EnumDOType.Advance)).Sum(x => x.Qty);
                    FillCell(sheet, nRowIndex, nStartCol++, nTotalQty.ToString(), true);
                    FillCell(sheet, nRowIndex, nStartCol++, "YD", false);
                    nRowIndex++; nTotal += nTotalQty;

                    nStartCol = 6;
                    FillCell(sheet, nRowIndex, nStartCol++, "L/C S/D print fabric", false);
                    nTotalQty = _oFDCRegisters.Where(x => x.IsYD == false && x.IsPrint == true && (x.FDOType == (int)EnumDOType.Export || x.FDOType == (int)EnumDOType.Advance)).Sum(x => x.Qty);
                    FillCell(sheet, nRowIndex, nStartCol++, nTotalQty.ToString(), true);
                    FillCell(sheet, nRowIndex, nStartCol++, "YD", false);
                    nRowIndex++; nTotal += nTotalQty;

                    List<EnumObject> oEnumObjs = new List<EnumObject>();
                    oEnumObjs = EnumObject.jGets(typeof(EnumDOType)).Where(x => x.id != (int)EnumDOType.Export && x.id != (int)EnumDOType.Advance && x.id != (int)EnumDOType.None).ToList();

                    foreach (EnumObject oObj in oEnumObjs)
                    {
                        nStartCol = 6;
                        FillCell(sheet, nRowIndex, nStartCol++, oObj.Description, false);
                        nTotalQty = _oFDCRegisters.Where(x => x.FDOType == oObj.id).Sum(x => x.Qty);
                        FillCell(sheet, nRowIndex, nStartCol++, nTotalQty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, "YD", false);
                        nRowIndex++; nTotal += nTotalQty;
                    }

                    nStartCol = 6;
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, nTotal.ToString("#,##0.00;(#,##0.00)"), nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right);
                    FillCellMerge(ref sheet, " ", nRowIndex, nRowIndex, nStartCol, nStartCol++, true, ExcelHorizontalAlignment.Right);
                    nRowIndex++;

                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FDCRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }

        #region Excel Supprot
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber)
                cell.Value = Convert.ToDouble(sVal);
            else
                cell.Value = sVal;
            cell.Style.Font.Bold = IsBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;

            if (IsNumber)
            {
                cell.Style.Numberformat.Format = _sFormatter;
                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
            return cell;
        }

        private void FillCellMerge(ref ExcelWorksheet sheet, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, string sVal)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = false;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.None;
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex)
        {
            FillCellMerge(ref sheet, sVal, startRowIndex, endRowIndex, startColIndex, endColIndex, false, ExcelHorizontalAlignment.Left);
        }
        private void FillCellMerge(ref ExcelWorksheet sheet, string sVal, int startRowIndex, int endRowIndex, int startColIndex, int endColIndex, bool isBold, ExcelHorizontalAlignment HoriAlign)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[startRowIndex, startColIndex, endRowIndex, endColIndex];
            cell.Merge = true;
            cell.Value = sVal;
            cell.Style.Font.Bold = isBold;
            cell.Style.WrapText = true;
            cell.Style.HorizontalAlignment = HoriAlign;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
        }
        #endregion
        #endregion

        #region Print Excel Party Wise
        public void ExportToExcelFDCRegisterPartyWise(double ts)
        {
            Company oCompany = new Company();
            FDCRegister oFDCRegister = new FDCRegister();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDCRegisters = new List<FDCRegister>();
                oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL_FDC(oFDCRegister);
                _oFDCRegisters = FDCRegister.Gets_FDC(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oFDCRegisters = new List<FDCRegister>();
                _sErrorMesage = ex.Message;
            }
            bool isAuthorized = false;
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.FabricDeliveryChallan && obj.OperationType == EnumRoleOperationType.RateView)
                {
                    isAuthorized = true;
                }
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Compsition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M Unit", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO Qty", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Total Delivery", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Yet To Delivery", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty Delivery", Width = 20f, IsRotate = false });

                if (isAuthorized)
                {
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
                }
                table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PI No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric Weave", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Width", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 20f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                    sheet.Name = "FDC Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "FDC Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 2;
                    nRowIndex++;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    string sMUName = "";

                    #region Data
                    int nCount = 0, nRowSpan = 0;
                    int nPreviousBuyerID = -1;
                    bool bIsFirst = false;
                    double dSubTotal = 0.0;
                    int nFEOID = 0;
                    double nQty = 0.0;
                    //_oFDCRegisters = _oFDCRegisters.OrderBy(x => new { x.BuyerID, x.FEOID }).ToList();
                    _oFDCRegisters = _oFDCRegisters.OrderBy(x => x.BuyerName).ThenBy(y=>y.FEOID).ToList();
                    List<FDCRegister> oFDCRegisters = new List<FDCRegister>();
                    foreach (var oItem in _oFDCRegisters)
                    {
                        nStartCol = 2;
                        if (nPreviousBuyerID != oItem.BuyerID && bIsFirst==true)
                        {
                            oFDCRegisters = _oFDCRegisters.Where(x => x.BuyerID == nPreviousBuyerID).ToList();

                          
                            nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right);
                            nQty = oFDCRegisters.Sum(x => x.QtyOrder);
                            FillCell(sheet, nRowIndex, 11, Math.Round(nQty, 2).ToString(), true, true);
                            nQty = oFDCRegisters.Sum(x => x.QtyDelivery);
                            FillCell(sheet, nRowIndex, 12, Math.Round(nQty, 2).ToString(), true, true);
                            nQty = oFDCRegisters.Sum(x => x.YetToDelivery);
                            FillCell(sheet, nRowIndex, 13, Math.Round(nQty, 2).ToString(), true, true);
                            FillCell(sheet, nRowIndex, 14, Math.Round(dSubTotal, 2).ToString(), true, true);
                            FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, 15, 19, true, ExcelHorizontalAlignment.Right);
                            dSubTotal = 0;
                        }
                        nStartCol = 2;
                        if (nPreviousBuyerID != oItem.BuyerID)
                        {
                            FillCellMerge(ref sheet, "Party Name : " + oItem.BuyerName, nRowIndex, nRowIndex++, nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Left);
                        }
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ChallanNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ChallanDateSt, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.DOTypeStr, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Construction, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorInfo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.MUName, false);

                        //FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.QtyOrder, 2).ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.QtyDelivery, 2).ToString(), true);
                        //FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.YetToDelivery, 2).ToString(), true);
                        if (oItem.FEOID != nFEOID)
                        {
                            int rowCount = (_oFDCRegisters.Count(x => x.FEOID == oItem.FEOID && x.BuyerID == oItem.BuyerID) - 1);
                            rowCount = (rowCount == -1) ? 0 : rowCount;
                            cell = sheet.Cells[nRowIndex, nStartCol, (nRowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = Math.Round(oItem.QtyOrder, 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[nRowIndex, nStartCol, (nRowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = Math.Round(oItem.QtyDelivery, 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                            cell = sheet.Cells[nRowIndex, nStartCol, (nRowIndex + rowCount), nStartCol++]; cell.Merge = true; cell.Value = Math.Round(oItem.YetToDelivery, 2).ToString();
                            cell.Style.Font.Bold = false; border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;

                        }
                        else
                        {
                            nStartCol += 3;
                        }
                        
                        FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.Qty, 2).ToString(), true);
                        if(isAuthorized)
                        {
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.UnitPrice, 2).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.Amount, 2).ToString(), true);
                        }
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.SCNoFull, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.PINo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricWeave, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishTypeName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishWidth, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProcessTypeName, false);
                        nRowIndex++;

                        sMUName = oItem.MUName;
                        nPreviousBuyerID = oItem.BuyerID;
                        nFEOID = oItem.FEOID;
                        dSubTotal = dSubTotal + oItem.Qty;
                        bIsFirst = true;
                    }
                    #endregion

                    #region Last Sub Total

                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, 10, true, ExcelHorizontalAlignment.Right);
                   // oFDCRegisters = _oFDCRegisters.Where(x => x.FDCID.Equals(obj.FDCID)).Sum(x => x.Qty);
                    oFDCRegisters = _oFDCRegisters.Where(x => x.BuyerID == nPreviousBuyerID).ToList();

                    nQty = oFDCRegisters.Sum(x => x.QtyOrder);
                    FillCell(sheet, nRowIndex, 11, Math.Round(nQty, 2).ToString(), true, true);
                    nQty = oFDCRegisters.Sum(x => x.QtyDelivery);
                    FillCell(sheet, nRowIndex, 12, Math.Round(nQty, 2).ToString(), true, true);
                    nQty = oFDCRegisters.Sum(x => x.YetToDelivery);
                    FillCell(sheet, nRowIndex, 13, Math.Round(nQty, 2).ToString(), true, true);
                    FillCell(sheet, nRowIndex, 14, Math.Round(dSubTotal, 2).ToString(), true, true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, 15, 20, true, ExcelHorizontalAlignment.Right);
                    nStartCol = 2;
                    #endregion

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    nQty = _oFDCRegisters.Sum(x => x.QtyOrder);
                    FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(nQty, 2).ToString(), true, true);
                    nQty = _oFDCRegisters.Sum(x => x.QtyDelivery);
                    FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(nQty, 2).ToString(), true, true);
                    nQty = _oFDCRegisters.Sum(x => x.YetToDelivery);
                    FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(nQty, 2).ToString(), true, true);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty).ToString(), true, true);
                    if(isAuthorized)
                    {
                        //double avg = _oFDCRegisters.Sum(x => x.UnitPrice) / nCount;
                        FillCell(sheet, nRowIndex, ++nStartCol,"", true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(_oFDCRegisters.Sum(x => x.Amount), 2).ToString(), true, true);
                    }
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    FillCell(sheet, nRowIndex, ++nStartCol, "", false);
                    _sFormatter = sMUName + " #,##0.00;(#,##0.00)";
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol+1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FDCRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion

        #region Print Excel PI Wise
        public void ExportToExcelFDCRegisterPIWise(double ts)
        {
            Company oCompany = new Company();
            FDCRegister oFDCRegister = new FDCRegister();
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            try
            {
                _sErrorMesage = "";

                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                _oFDCRegisters = new List<FDCRegister>();
                oFDCRegister = (FDCRegister)Session[SessionInfo.ParamObj];
                string sSQL = this.MakeSQL_FDC(oFDCRegister);
                _oFDCRegisters = FDCRegister.Gets_FDC(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oFDCRegisters.Count <= 0)
                {
                    _sErrorMesage = "There is no data for print!";
                }
                oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricDeliveryChallan).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oFDCRegisters = new List<FDCRegister>();
                _sErrorMesage = ex.Message;
            }
            bool isAuthorized = false;
            foreach (AuthorizationRoleMapping obj in oAuthorizationRoleMappings)
            {
                if (obj.ModuleName == EnumModuleName.FabricDeliveryChallan && obj.OperationType == EnumRoleOperationType.RateView)
                {
                    isAuthorized = true;
                }
            }

            if (_sErrorMesage == "")
            {
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();
                table_header.Add(new TableHeader { Header = "#SL", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Challan Date", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "DO Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buyer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Construction", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Compsition", Width = 25f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Color", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "M Unit", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Qty", Width = 15f, IsRotate = false });
                if (isAuthorized)
                {
                    table_header.Add(new TableHeader { Header = "Unit Price", Width = 20f, IsRotate = false });
                    table_header.Add(new TableHeader { Header = "Amount", Width = 20f, IsRotate = false });
                }
                table_header.Add(new TableHeader { Header = "MKT Ref No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "PO No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Dispo No", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Fabric Weave", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Type", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Finish Width", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Process Type", Width = 20f, IsRotate = false });

                #endregion

                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("FDC Register");
                    sheet.Name = "FDC Register";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "FDC Register"; cell.Style.Font.Bold = true;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 18; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, 14]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    cell = sheet.Cells[nRowIndex, 15, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = ""; cell.Style.Font.Bold = false;
                    cell.Style.WrapText = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    nStartCol = 2;
                    nRowIndex++;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    nRowIndex++;
                    string sMUName = "";

                    #region Data
                    int nCount = 0;
                    string nPreviousPINo = "###";
                    double dSubTotal = 0.0;
                    bool bIsFirst = false;
                    _oFDCRegisters = _oFDCRegisters.OrderBy(x => x.PINo).ToList();
                    foreach (var oItem in _oFDCRegisters)
                    {
                        nStartCol = 2;
                        if (nPreviousPINo != oItem.PINo && bIsFirst==true)
                        {
                            nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                            FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right);
                            FillCell(sheet, nRowIndex, 12, Math.Round(dSubTotal, 2).ToString(), true, true);
                            FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, 13, nEndCol+1, true, ExcelHorizontalAlignment.Right);
                            dSubTotal = 0;
                        }
                        nStartCol = 2;
                        if (nPreviousPINo != oItem.PINo)
                        {
                            FillCellMerge(ref sheet, "PI No: " + oItem.PINo, nRowIndex, nRowIndex++, nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Left);
                        }
                        _sFormatter = " #,##0;(#,##0)";
                        FillCell(sheet, nRowIndex, nStartCol++, (++nCount).ToString(), false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ChallanNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ChallanDateSt, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.DOTypeStr, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.BuyerName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ContractorName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Construction, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProductName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ColorInfo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.MUName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.Qty, 2).ToString(), true);
                        if (isAuthorized)
                        {
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.UnitPrice, 2).ToString(), true);
                            FillCell(sheet, nRowIndex, nStartCol++, Math.Round(oItem.Amount, 2).ToString(), true);
                        }
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.SCNoFull, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ExeNo, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FabricWeave, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishTypeName, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.FinishWidth, false);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.ProcessTypeName, false);
                        nRowIndex++;

                        sMUName = oItem.MUName;
                        nPreviousPINo = oItem.PINo;
                        dSubTotal = dSubTotal + oItem.Qty;
                        bIsFirst = true;
                    }
                    #endregion

                    #region Last Sub Total

                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Sub Total: ", nRowIndex, nRowIndex, nStartCol, 11, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, 12, Math.Round(dSubTotal, 2).ToString(), true, true);
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, 12, nEndCol+1, true, ExcelHorizontalAlignment.Right);
                    nStartCol = 2;
                    #endregion

                    #region Grand Total
                    nStartCol = 2; _sFormatter = " #,##0;(#,##0)";
                    FillCellMerge(ref sheet, "Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 9, true, ExcelHorizontalAlignment.Right);
                    FillCell(sheet, nRowIndex, ++nStartCol, _oFDCRegisters.Sum(x => x.Qty).ToString(), true, true);
                    if (isAuthorized)
                    {
                        double avg = _oFDCRegisters.Sum(x => x.UnitPrice) / nCount;
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(avg, 2).ToString(), true, true);
                        FillCell(sheet, nRowIndex, ++nStartCol, Math.Round(_oFDCRegisters.Sum(x => x.Amount), 2).ToString(), true, true);
                    }
                    FillCellMerge(ref sheet, "", nRowIndex, nRowIndex++, ++nStartCol, nEndCol+1, true, ExcelHorizontalAlignment.Right);
                    _sFormatter = sMUName + " #,##0.00;(#,##0.00)";
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FDCRegister.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }
        #endregion
    }
}
