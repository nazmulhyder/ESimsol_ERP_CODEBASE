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
	public class FNProductionController : Controller
	{
		#region Declaration

		FNProduction _oFNProduction = new FNProduction();
		List<FNProduction> _oFNProductions = new  List<FNProduction>();
        List<FNProductionBatch> _oFNProductionBatchs = new List<FNProductionBatch>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewFNProductions(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oFNProductions = new List<FNProduction>();
            string sSQL = "SELECT * FROM View_FNProduction WHERE FNTreatment=1 AND ISNULL(EndDateTime,'') = '' ORder BY StartDateTime";
			_oFNProductions = FNProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.FNProcessList = EnumObject.jGets(typeof(EnumFNProcess));
			return View(_oFNProductions);
		}
        public ActionResult ViewFNBatchs_PS(int buid, int menuid, int treatment)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FNProduction).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));            

            List<FNProduction> oFNProductions = new List<FNProduction>(); ;
            oFNProductions = FNProduction.Gets("SELECT TOP 250 * FROM View_FNProduction ORDER BY IssueDate DESC", ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment)); ;
            ViewBag.Treatment = treatment;
            ViewBag.BUID = buid;
            return View(oFNProductions);
        }
      
        public ActionResult ViewFNBatch_PS_V2(int buid, int nId, int treatment)
        {
            FNProduction oFNProduction = new FNProduction();
            List<FNBatchCard> oFNBatchCards = new List<FNBatchCard>();
            List<FNProductionBatch> oFNProductionBatchs = new List<FNProductionBatch>();

            if (nId > 0)
            {
                string sSQL = "SELECT * FROM View_FNProductionBatch WHERE FNProductionID=" + nId;
                oFNProduction = oFNProduction.Get(nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNProduction.FNProductionBatchs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNProductionID=" + nId, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oFNProduction.FNBatchCards = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE FNBatchCardID IN (SELECT FNPB.FNBatchCardID FROM FNProductionBatch FNPB WHERE FNPB.FNProductionID =" + nId + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }

            ViewBag.FNMachineProcessList = FNMachine.Gets("SELECT * FROM View_FNMachine WHERE FNMachineType = 1", (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            //ViewBag.FNBatchs = _oFNBatchs;
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment)); ;
            ViewBag.Treatment = treatment;
            ViewBag.BUID = buid;
            return View(oFNProduction);
        }

		public ActionResult ViewFNProduction(int id)
		{
			_oFNProduction = new FNProduction();
			if (id > 0)
			{
				_oFNProduction = _oFNProduction.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionConsumptions= FNProductionConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.FNMachineProcessList = FNMachineProcess.Gets(_oFNProduction.FNMachineID, (int)Session[SessionInfo.currentUserID]);
			return View(_oFNProduction);
		}

		[HttpPost]
		public JsonResult Save(FNProduction oFNProduction)
		{
			_oFNProduction = new FNProduction();
			try
			{
				_oFNProduction = oFNProduction;
				_oFNProduction = _oFNProduction.Save((int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNProductionID=" + _oFNProduction.FNProductionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                //oFNProduction.FNBatchCards = FNBatchCard.Gets("SELECT * FROM View_FNBatchCard WHERE FNBatchCardID IN (SELECT FNPB.FNBatchCardID FROM FNProductionBatch FNPB WHERE FNPB.FNProductionID =" + _oFNProduction.FNProductionID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
			}
			catch (Exception ex)
			{
				_oFNProduction = new FNProduction();
				_oFNProduction.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNProduction);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult Run(FNProduction oFNProduction)
        {
            _oFNProduction = new FNProduction();
            try
            {
                _oFNProduction = oFNProduction;
                _oFNProduction = _oFNProduction.Run((int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNProductionID=" + _oFNProduction.FNProductionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNProduction = new FNProduction();
                _oFNProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult RunOut(FNProduction oFNProduction)
        {
            _oFNProduction = new FNProduction();
            try
            {
                _oFNProduction = oFNProduction;
                _oFNProduction = _oFNProduction.RunOut((int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets("SELECT * FROM View_FNProductionBatch WHERE FNProductionID=" + _oFNProduction.FNProductionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFNProduction = new FNProduction();
                _oFNProduction.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProduction);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult QCRequest(FNProductionBatch oFNProductionBatch)
        {
            //_oFNProductionBatch = new FNProductionBatch();
            EnumDBOperation eEnumDBOperation = EnumDBOperation.Request;
            try
            {
                if (oFNProductionBatch.QCStatus == EnumQCStatus.Yet_To_QC) { eEnumDBOperation = EnumDBOperation.Undo; }
                if (oFNProductionBatch.QCStatus == EnumQCStatus.In_QC) { eEnumDBOperation = EnumDBOperation.Request; }

                oFNProductionBatch = oFNProductionBatch.QCRequest(eEnumDBOperation, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(FNProduction oFNProduction)
		{
			string sFeedBackMessage = "";
			try
			{
                sFeedBackMessage = oFNProduction.Delete(oFNProduction.FNProductionID, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        //GetReProcessBatches
        [HttpPost]
        public JsonResult GetReProcessBatches(FNProductionBatch oFNProductionBatch)
        {
            _oFNProductionBatchs = new List<FNProductionBatch>();
            string sSQL = "SELECT * FROM View_FNProductionBatch WHERE FNPBatchID IN (SELECT FNProductionBatchQuality.FNPBatchID FROM FNProductionBatchQuality WHERE ISNULL(FNProductionBatchQuality.IsOk,0)=0) AND ISNULL(Ref_FNPBatchID,0) NOt IN (SELECT FNProductionBatchQuality.FNPBatchID FROM FNProductionBatchQuality  WHERE ISNULL(FNProductionBatchQuality.IsOk,0)=0)";
            try
            {
                _oFNProductionBatchs = FNProductionBatch.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNProductionBatchs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFNProductionBatch(FNProductionBatch oFNProductionBatch)
        {
            try
            {

                oFNProductionBatch = oFNProductionBatch.Get(oFNProductionBatch.FNPBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFNProductionBatch.FNProductionConsumptions = FNProductionConsumption.Gets("Select *  FROM View_FNProductionConsumption WHERE FNPBatchID=" + oFNProductionBatch.FNPBatchID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            catch (Exception ex)
            {
                oFNProductionBatch.ErrorMessage = ex.Message; 
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveFNPConsumption(FNProductionConsumption oFNProductionConsumption)
        {
            try
            {
                oFNProductionConsumption = oFNProductionConsumption.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNProductionConsumption = new FNProductionConsumption();
                oFNProductionConsumption.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionConsumption);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNPConsumption(FNProductionConsumption oFNPC)
        {
            string sMessage = "";
            try
            {
                if (oFNPC.FNPConsumptionID <= 0) { throw new Exception("Please select a valid item."); }
                 sMessage = oFNPC.Delete(oFNPC.FNPConsumptionID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNPC = new FNProductionConsumption();
                sMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFNPBatch(FNProductionBatch oFNProductionBatch)
        {
            try
            {
                oFNProductionBatch = oFNProductionBatch.Save(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFNPBatch_Process(FNProductionBatch oFNProductionBatch)
        {
            try
            {
                oFNProductionBatch = oFNProductionBatch.Save_Process(((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNProductionBatch);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNPBatch(FNProductionBatch oFNProductionBatch)
        {
            string sMessage = "";
            try
            {
                if (oFNProductionBatch.FNProductionID <= 0) { throw new Exception("Please select a valid item."); }
                sMessage = oFNProductionBatch.Delete(oFNProductionBatch.FNPBatchID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oFNProductionBatch = new FNProductionBatch();
                oFNProductionBatch.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
     


		#endregion

        #region HttpGet For Search
        [HttpPost]
        public JsonResult Search(FNProduction oFNProduction)
        {
            List<FNProduction> oFNProductions = new List<FNProduction>();
            try
            {
                string sSQL = GetSQL(oFNProduction);
                oFNProductions = FNProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNProductions = new List<FNProduction>();
                _oFNProduction = new FNProduction();
                _oFNProduction.ErrorMessage = ex.Message;
                oFNProductions.Add(_oFNProduction);
            }
            var jsonResult = Json(oFNProductions, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #region GetSQL
        private string GetSQL(FNProduction oFNProduction)
        {
            string sParams = oFNProduction.Params;
            int nIssueDateComboValue = 0;
            int  nFNTreatment = 0;
            int nBUID = 0, nReProcess = -1;
            string sFNMachineIDs = "";
            string sBatchNo = "", sDispoNo = "", sFNProcess = "";
            DateTime dStartIssueDate   = DateTime.Now;
            DateTime dEndIssueDate = DateTime.Now; ;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                nIssueDateComboValue = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStartIssueDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEndIssueDate = Convert.ToDateTime(sParams.Split('~')[nCount++]);

                nFNTreatment = Convert.ToInt32(sParams.Split('~')[nCount++]);
                sFNMachineIDs = sParams.Split('~')[nCount++];
               
                if (sParams.Split('~').Count() > 6)
                    sFNProcess = sParams.Split('~')[nCount++]; 
                if (sParams.Split('~').Count() > 7)
                    sBatchNo = sParams.Split('~')[nCount++];
                if (sParams.Split('~').Count() > 8)
                    nBUID = Convert.ToInt32(sParams.Split('~')[nCount++]);
                if (sParams.Split('~').Count() > 9)
                    sDispoNo = sParams.Split('~')[nCount++];
                if (sParams.Split('~').Count() > 10)
                    nReProcess = Convert.ToInt32(sParams.Split('~')[nCount++]);
            }
            string sReturn1 = "SELECT * FROM View_FNProduction";
            string sReturn = "";

       
            #region BatchNo
            if (!string.IsNullOrEmpty(sBatchNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNProductionID IN (SELECT FNProductionID FROM View_FNProductionBatch WHERE BatchNo LIKE '%" + sBatchNo + "%')";
            }
            #endregion

            #region Dispo No
            if (!string.IsNullOrEmpty(sDispoNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNProductionID IN (SELECT FNProductionID FROM View_FNProductionBatch WHERE FNExONo LIKE '%" + sDispoNo + "%')";
            }
            #endregion

            #region FNMachineID
            if (!string.IsNullOrEmpty(sFNMachineIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNMachineID IN (" + sFNMachineIDs + ")";
            }
            #endregion

            #region Technical Sheets
            if (nFNTreatment >0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNTreatment =" + nFNTreatment ;
            }

            #endregion

            #region FNProcess
            if (!string.IsNullOrEmpty(sFNProcess))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNProcess LIKE '%" + sFNProcess + "%'";
            }
            #endregion

            #region   Date Wise
            if (nIssueDateComboValue > 0)
            {
                if (nIssueDateComboValue == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate = '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate != '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate > '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate < '" + dStartIssueDate.ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate>= '" + dStartIssueDate.ToString("dd MMM yyyy") + "' AND IssueDate < '" + dEndIssueDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nIssueDateComboValue == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " IssueDate< '" + dStartIssueDate.ToString("dd MMM yyyy") + "' OR IssueDate > '" + dEndIssueDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }
            #endregion

            #region Dispo No
            if (nReProcess > -1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " FNProductionID IN (SELECT FNProductionID FROM FNProductionBatch WHERE ISNULL(IsProduction,0) = "+nReProcess+")";
            }
            #endregion

               sReturn = sReturn1 + sReturn + " ORDER BY FNProductionID";
            return sReturn;
        }
        #endregion


        #endregion
        
        #region Dyeing Production Tonmoy Bhowmik

        public ActionResult ViewFNDyeingProductions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductions = new List<FNProduction>();
            string sSQL = "SELECT * FROM View_FNProduction WHERE FNDyeingType<>0 AND FNDyeingType is not null AND ISNULL(EndDateTime,'') = '' ORder BY StartDateTime";
            _oFNProductions = FNProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
         
            return View(_oFNProductions);
        }
        public ActionResult ViewFNDyeingProduction(int id)
        {
            _oFNProduction = new FNProduction();
            if (id > 0)
            {
                _oFNProduction = _oFNProduction.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionConsumptions = FNProductionConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oFNProduction.FNProductionConsumptions = new List<FNProductionConsumption>();
            }
            ViewBag.FNMachineProcessList = FNMachineProcess.Gets(_oFNProduction.FNMachineID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FNDyeingProcesTypes = EnumObject.jGets(typeof(EnumFNDyeingType)).Where(x => x.Value != "None").ToList();
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFNProduction);
        }
        #endregion

        #region Finishing Production Tonmoy Bhowmik

        public ActionResult ViewFNFinishingProductions(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNProductions = new List<FNProduction>();
            string sSQL = "SELECT * FROM View_FNProduction WHERE FNTPID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment ="+(int)EnumFNTreatment.Finishing+") AND EndDateTime is null ORDER BY StartDateTime " ;//FNTreatment 3
            _oFNProductions = FNProduction.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
        
            return View(_oFNProductions);
        }
        public ActionResult ViewFNFinishingProduction(int id)
        {
            _oFNProduction = new FNProduction();
            if (id > 0)
            {
                _oFNProduction = _oFNProduction.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionBatchs = FNProductionBatch.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oFNProduction.FNProductionConsumptions = FNProductionConsumption.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oFNProduction.FNProductionConsumptions = new List<FNProductionConsumption>();
            }
            ViewBag.FNMachineProcessList = FNMachineProcess.Gets(_oFNProduction.FNMachineID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Shades = EnumObject.jGets(typeof(EnumShade));
            ViewBag.HRMShifts = HRMShift.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(_oFNProduction);
        }
        #endregion

        #region gets
        [HttpPost]
        public JsonResult GetsFNProductionByBatchNo(FNBatchCard oFNBatchCard)
        {
            List<FNReProRequestDetail> oFNReProRequestDetails = new List<FNReProRequestDetail>();
            try
            {
                string sSQL = "SELECT * FROM View_FNReProRequestDetail WHERE FNBatchID > 0 AND FNReProRequestID IN (SELECT FNReProRequestID FROM FNReProRequest WHERE Status = 1) AND ISNULL(Qty_YetTo,0) > 0 ";

                if (!string.IsNullOrEmpty(oFNBatchCard.FNBatchNo))
                    sSQL += " AND BatchNo LIKE '%" + oFNBatchCard.FNBatchNo + "%'";
                if (oFNBatchCard.FNTreatmentProcessID > 0)
                    sSQL += " AND FNTreatmentProcessID IN (" + oFNBatchCard.FNTreatmentProcessID + ")";
                if (oFNBatchCard.FNTreatment > 0)
                    sSQL += " AND FNTreatment =" + (int)oFNBatchCard.FNTreatment;

                oFNReProRequestDetails = FNReProRequestDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFNReProRequestDetails = new List<FNReProRequestDetail>();
                oFNReProRequestDetails.Add(new FNReProRequestDetail() { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNReProRequestDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }

}
