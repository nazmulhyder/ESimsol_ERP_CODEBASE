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
	public class FNMachineController : Controller
	{
		#region Declaration

		FNMachine _oFNMachine = new FNMachine();
		List<FNMachine> _oFNMachines = new  List<FNMachine>();
		#endregion

		#region Functions

		#endregion

		#region Actions

		public ActionResult ViewFNMachines(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);		
			_oFNMachines = new List<FNMachine>(); 
			_oFNMachines = FNMachine.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FNMachineTypes = EnumObject.jGets(typeof(EnumFNMachineType));
            //ViewBag.FNMachineTypes = EnumObject.jGets(typeof(EnumFNMachineType));
			return View(_oFNMachines);
		}

		public ActionResult ViewFNMachine(int id)
		{
			_oFNMachine = new FNMachine();
			if (id > 0)
			{
				_oFNMachine = _oFNMachine.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
            ViewBag.FNMachineTypes = EnumObject.jGets(typeof(EnumFNMachineType));
			return View(_oFNMachine);
		}

		[HttpPost]
		public JsonResult Save(FNMachine oFNMachine)
		{
			_oFNMachine = new FNMachine();
			try
			{
				_oFNMachine = oFNMachine;
				_oFNMachine = _oFNMachine.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNMachine = new FNMachine();
				_oFNMachine.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNMachine);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}

        [HttpPost]
        public JsonResult ChangeActivety(FNMachine oFNMachine)
        {
            _oFNMachine = new FNMachine();
            try
            {
                _oFNMachine = oFNMachine;
                _oFNMachine = _oFNMachine.ChangeActivety((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNMachine = new FNMachine();
                _oFNMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetFNMachines(FNMachineProcess oFNMachine)
        {
            _oFNMachines = new List<FNMachine>();
            try
            {
                string sSQL = "SELECT *  FROM View_FNMachine WHERE  FNMachineID >0 ";
                if (!string.IsNullOrEmpty(oFNMachine.Params))
                {
                    sSQL += " AND FNMachineType IN ( " + oFNMachine.Params + " )";
                }
                if (oFNMachine.FNTPID >0)
                {
                    sSQL += " AND FNMachineID IN (SELECT HH.FNMachineID FROM FNMachineProcess AS HH WHERE HH.FNTPID = " + oFNMachine.FNTPID + ")";
                }
                if (!string.IsNullOrEmpty(oFNMachine.Name))
                {
                    sSQL += "  AND Name+Code LIKE '%" + oFNMachine.Name + "%'";
                }
                
                _oFNMachines = FNMachine.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNMachine = new FNMachine();
                _oFNMachine.ErrorMessage = ex.Message;
                _oFNMachines.Add(_oFNMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFNBatcherTrolly(FNMachine oFNMachine)
        {
            _oFNMachines = new List<FNMachine>();
            try
            {
                string sSQL = "SELECT *  FROM View_FNMachine WHERE FNMachineType IN(2,3) ";//2--> Batcher ,3--> Trolly
                if (oFNMachine.CheckActiveProcess == true)
                {
                    sSQL += "  AND IsActiveProcessExist=1";
                }
                if (!string.IsNullOrEmpty(oFNMachine.Name))
                {
                    sSQL += "  AND Name+Code LIKE '%" + oFNMachine.Name + "%'";
                }

                _oFNMachines = FNMachine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNMachine = new FNMachine();
                _oFNMachine.ErrorMessage = ex.Message;
                _oFNMachines.Add(_oFNMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetFNMachinesForAllProcess(FNMachine oFNMachine)
        {
            _oFNMachines = new List<FNMachine>();
            try
            {
                string sSQL = "SELECT *  FROM View_FNMachine WHERE FNMachineType = " + oFNMachine.FNMachineTypeInt;
             
                if (!string.IsNullOrEmpty(oFNMachine.Name))
                {
                    sSQL += "  AND Name+Code LIKE '%" + oFNMachine.Name + "%'";
                }
                sSQL += "AND FNMachineID IN (SELECT FNMachineID FROM FNMachineProcess WHERE InActiveBy=0 AND FNTPID IN (SELECT FNTPID FROM FNTreatmentProcess WHERE FNTreatment=" + (int)oFNMachine.FNTreatment + "))";
                _oFNMachines = FNMachine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNMachine = new FNMachine();
                _oFNMachine.ErrorMessage = ex.Message;
                _oFNMachines.Add(_oFNMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


		[HttpGet]
		public JsonResult Delete(int id)
		{
			string sFeedBackMessage = "";
			try
			{
				FNMachine oFNMachine = new FNMachine();
				sFeedBackMessage = oFNMachine.Delete(id, (int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				sFeedBackMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(sFeedBackMessage);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 


		#endregion

        #region Action FNMachine wise Process
        public ActionResult ViewFNMachineProcess(int id)
        {
            _oFNMachine = new FNMachine();
            if (id > 0)
            {
                _oFNMachine = _oFNMachine.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oFNMachine.FNMachineProcessList = FNMachineProcess.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oFNMachine);
        }

        [HttpPost]
        public JsonResult GetsTreatmentProcess(FNMachine oFNMachine)
        {
            List<FNTreatmentProcess> oFNTreatmentProcessList = new List<FNTreatmentProcess>();
            try
            {
                string sSQL = "SELECT  * FROM FNTreatmentProcess WHERE FNTPID NOT IN (SELECT FNTPID FROM FNMachineProcess WHERE FNMachineID = " + oFNMachine.FNMachineID + ")";
                if(!string.IsNullOrEmpty(oFNMachine.Name))
                {
                    sSQL += " AND Description LIKE '%"+oFNMachine.Name+"%'";
                }
                sSQL += " ORDER BY FNTreatment";
                oFNTreatmentProcessList = FNTreatmentProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                FNTreatmentProcess oFNTreatmentProcess = new FNTreatmentProcess();
                oFNTreatmentProcess.ErrorMessage = ex.Message;
                oFNTreatmentProcessList.Add(oFNTreatmentProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNTreatmentProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFNMachinesForRequisition(FNMachine oFNMachine)
        {
            List<FNMachine> oFNMachines = new List<FNMachine>();
            try
            {
                string sSQL = "SELECT * FROM View_FNMachine WHERE FNMachineType = " + (int)EnumFNMachineType.Machine;
                if(!string.IsNullOrEmpty(oFNMachine.Name))
                {
                    sSQL += " AND Name LIKE '%"+oFNMachine.Name+"%'";
                }
                if ((int)oFNMachine.FNTreatment > 0)
                {
                    sSQL += " AND FNMachineID IN (SELECT FNMachineID FROM view_FNMachineProcess WHERE FNTreatment =" + (int)oFNMachine.FNTreatment + ")";
                }
                sSQL += " ORDER BY Name";
                oFNMachines = FNMachine.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                FNMachine objFNMachine = new FNMachine();
                objFNMachine.ErrorMessage = ex.Message;
                oFNMachines.Add(objFNMachine);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        

        [HttpPost]
        public JsonResult SaveFNMProcess(FNMachine oFNMachine)
        {
            FNMachineProcess oFNMachineProcess = new FNMachineProcess();
            try
            {
                oFNMachine = oFNMachineProcess.Save(oFNMachine, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNMachine = new FNMachine();
                oFNMachine.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNMachine);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFNMProcess(FNMachineProcess oFNMachineProcess)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFNMachineProcess.Delete(oFNMachineProcess.FNMProcessID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult ChangeActivetyProcess(FNMachineProcess oFNMachineProcess)
        {
            _oFNMachine = new FNMachine();
            try
            {
                oFNMachineProcess = oFNMachineProcess.ChangeActivety((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNMachineProcess = new FNMachineProcess();
                oFNMachineProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNMachineProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult GetFNMProcessList(FNMachineProcess oFNMachineProcess)
        {
            List<FNMachineProcess> oFNMachineProcessList = new List<FNMachineProcess>();
            try
            {
                oFNMachineProcessList = FNMachineProcess.Gets(oFNMachineProcess.FNMachineID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFNMachineProcess = new FNMachineProcess();
                oFNMachineProcess.ErrorMessage = ex.Message;
                oFNMachineProcessList.Add(oFNMachineProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNMachineProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFNTreatMentProcessByMachine(FNTreatmentProcess oFNTreatmentProcesss)
        {
            FNTreatmentProcess _oFNTreatmentProcess = new FNTreatmentProcess();
            List<FNTreatmentProcess> _oFNTreatmentProcesss = new List<FNTreatmentProcess>();
            try
            {
                string sSQL = "SELECT * from FNTreatmentProcess WHERE FNTPID IN ( SELECT FNTPID FROM view_FNMachineProcess WHERE FNMachineID =" + oFNTreatmentProcesss.FNMachineID + ")";
                _oFNTreatmentProcesss = FNTreatmentProcess.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNTreatmentProcess = new FNTreatmentProcess();
                _oFNTreatmentProcess.ErrorMessage = ex.Message;
                _oFNTreatmentProcesss.Add(_oFNTreatmentProcess);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNTreatmentProcesss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
