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
	public class FNTreatmentProcessController : Controller
	{
		#region Declaration
		FNTreatmentProcess _oFNTreatmentProcess = new FNTreatmentProcess();
		List<FNTreatmentProcess> _oFNTreatmentProcesss = new  List<FNTreatmentProcess>();
		#endregion
		#region Functions

		#endregion
		#region Actions
		public ActionResult ViewFNTreatmentProcessList(int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oFNTreatmentProcesss = new List<FNTreatmentProcess>(); 
			_oFNTreatmentProcesss = FNTreatmentProcess.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.FNTreatments = EnumObject.jGets(typeof (EnumFNTreatment));
            //ViewBag.FNProcessList = EnumObject.jGets(typeof(EnumFNProcess));
            ViewBag.FNTreatmentProcess = new FNTreatmentProcess();
			return View(_oFNTreatmentProcesss);
		}
        public ActionResult ViewFNTPListPractice(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oFNTreatmentProcesss = new List<FNTreatmentProcess>();
            _oFNTreatmentProcesss = FNTreatmentProcess.Gets((int)Session[SessionInfo.currentUserID]);

            ViewBag.FNTreatments = EnumObject.jGets(typeof(EnumFNTreatment));
            ViewBag.FNProcessList = EnumObject.jGets(typeof(EnumFNProcess));
            return View(_oFNTreatmentProcesss);
        }
		public ActionResult ViewFNTreatmentProcess(int id)
		{
			_oFNTreatmentProcess = new FNTreatmentProcess();
			if (id > 0)
			{
				_oFNTreatmentProcess = _oFNTreatmentProcess.Get(id, (int)Session[SessionInfo.currentUserID]);
			}
			return View(_oFNTreatmentProcess);
		}

        //[HttpPost]
        //public JsonResult GetFNProcessList(FNTreatmentProcess oFNTreatmentProcess)
        //{
        //    EnumObject oProcess = new EnumObject();
        //    List<EnumObject> oProcessList = new List<EnumObject>();      
        //    try
        //    {
        //        EnumFNTreatment eOperationType = (EnumFNTreatment)oFNTreatmentProcess.FNTreatmentInt;
        //        List<EnumObject> oEnumObjects = EnumObject.jGets(typeof(EnumFNProcess));
        //        foreach (EnumObject oItem in oEnumObjects)
        //        {
        //            switch (eOperationType)
        //            {
        //                case EnumFNTreatment.Pre_Treatment:
        //                    if ((EnumFNProcess)oItem.id == EnumFNProcess.Singeing || (EnumFNProcess)oItem.id == EnumFNProcess.Desizing || (EnumFNProcess)oItem.id == EnumFNProcess.Scouring || (EnumFNProcess)oItem.id == EnumFNProcess.Bleaching || (EnumFNProcess)oItem.id == EnumFNProcess.ReBleach_Mild_Bleach || (EnumFNProcess)oItem.id == EnumFNProcess.Washing || (EnumFNProcess)oItem.id == EnumFNProcess.Mercerizing)
        //                    {
        //                        oProcessList.Add(oItem);
        //                    }
        //                    break;

        //                case EnumFNTreatment.Dyeing:
        //                    if ((EnumFNProcess)oItem.id == EnumFNProcess.Dyeing || (EnumFNProcess)oItem.id == EnumFNProcess.Washing)
        //                    {
        //                        oProcessList.Add(oItem);
        //                    }
        //                    break;

        //                case EnumFNTreatment.Finishing:

        //                    if ((EnumFNProcess)oItem.id == EnumFNProcess.Stenter || (EnumFNProcess)oItem.id == EnumFNProcess.Sanforize || (EnumFNProcess)oItem.id == EnumFNProcess.Peaching)
        //                    {
        //                        oProcessList.Add(oItem);
        //                    }
        //                    break;
        //                case EnumFNTreatment.Inspection:
        //                    if ((EnumFNProcess)oItem.id == EnumFNProcess.Inspection)
        //                    {
        //                        oProcessList.Add(oItem);
        //                    }
        //                    break;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        oProcess = new EnumObject(); ;
        //        oProcess.Value = ex.Message;
        //        oProcessList.Add(oProcess);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oProcessList);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //} 

		[HttpPost]
		public JsonResult Save(FNTreatmentProcess oFNTreatmentProcess)
		{
			_oFNTreatmentProcess = new FNTreatmentProcess();
			try
			{
				_oFNTreatmentProcess = oFNTreatmentProcess;
				_oFNTreatmentProcess = _oFNTreatmentProcess.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oFNTreatmentProcess = new FNTreatmentProcess();
				_oFNTreatmentProcess.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oFNTreatmentProcess);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		} 

		[HttpPost]
        public JsonResult Delete(FNTreatmentProcess oFNTreatmentProcess)
		{
			string sFeedBackMessage = "";
			try
			{

                sFeedBackMessage = oFNTreatmentProcess.Delete(oFNTreatmentProcess.FNTPID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetsFNTreatmentProcessByName(FNTreatmentProcess oFNTreatmentProcess)
        {
            FNTreatmentProcess _oFNTreatmentProcess = oFNTreatmentProcess;
            List<FNTreatmentProcess> oFNTreatmentProcessList = new List<FNTreatmentProcess>();
            try
            {
                oFNTreatmentProcessList = FNTreatmentProcess.Gets("SELECT * FROM FNTreatmentProcess WHERE FNProcess Like '%" + _oFNTreatmentProcess.FNProcess + "%' order by FNTreatment", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNTreatmentProcess = new FNTreatmentProcess();
                _oFNTreatmentProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNTreatmentProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsFNTreatmentProcessForFNRecipeLab(FNTreatmentProcess oFNTreatmentProcess)
        {
            FNTreatmentProcess _oFNTreatmentProcess = oFNTreatmentProcess;
            //int nFNLDDID = Convert.ToInt32(oFNTreatmentProcess.ErrorMessage);
            List<FNTreatmentProcess> oFNTreatmentProcessList = new List<FNTreatmentProcess>();
            try
            {
                //oFNTreatmentProcessList = FNTreatmentProcess.Gets("SELECT * FROM FNTreatmentProcess WHERE FNProcess Like '%" + _oFNTreatmentProcess.FNProcess + "%' AND FNTPID NOT IN (SELECT FNTPID FROM FNRecipeLab WHERE FNLDDID = '"+nFNLDDID+"') order by FNTreatment", (int)Session[SessionInfo.currentUserID]);
                
                string sSQL = "SELECT * FROM FNTreatmentProcess WHERE FNProcess Like '%" + _oFNTreatmentProcess.FNProcess + "%'" ;

                if(oFNTreatmentProcess.FNTreatmentInt > 0) 
                {
                    sSQL = " AND FNTreatment = " + oFNTreatmentProcess.FNTreatmentInt;
                }
                
                oFNTreatmentProcessList = FNTreatmentProcess.Gets(sSQL + " ORDER BY FNTreatment", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNTreatmentProcess = new FNTreatmentProcess();
                _oFNTreatmentProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNTreatmentProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetsFNTProductionProcess(FNTreatmentProcess oFNTreatmentProcess)
        {
            FNTreatmentProcess _oFNTreatmentProcess = oFNTreatmentProcess;
            //int nFNLDDID = Convert.ToInt32(oFNTreatmentProcess.ErrorMessage);
            List<FNTreatmentProcess> oFNTreatmentProcessList = new List<FNTreatmentProcess>();
            try
            {
                //oFNTreatmentProcessList = FNTreatmentProcess.Gets("SELECT * FROM FNTreatmentProcess WHERE FNProcess Like '%" + _oFNTreatmentProcess.FNProcess + "%' AND FNTPID NOT IN (SELECT FNTPID FROM FNRecipeLab WHERE FNLDDID = '"+nFNLDDID+"') order by FNTreatment", (int)Session[SessionInfo.currentUserID]);

                string sSQL = "SELECT * FROM FNTreatmentProcess WHERE ISNULL(IsProduction,0)=1 AND FNProcess Like '%" + _oFNTreatmentProcess.FNProcess + "%'";

                if (oFNTreatmentProcess.FNTreatmentInt > 0)
                {
                    sSQL = " AND FNTreatment = " + oFNTreatmentProcess.FNTreatmentInt;
                }

                oFNTreatmentProcessList = FNTreatmentProcess.Gets(sSQL + " ORDER BY FNTreatment", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNTreatmentProcess = new FNTreatmentProcess();
                _oFNTreatmentProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFNTreatmentProcessList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region Sub Process
        public ActionResult ViewAddSubProcess(int nFNTPID)
        {
            _oFNTreatmentProcess = new FNTreatmentProcess();
            List<FNTreatmentSubProcess> oFNTreatmentSubProcesssList = new List<FNTreatmentSubProcess>();
            oFNTreatmentSubProcesssList = FNTreatmentSubProcess.Gets("SELECT * FROM [View_FNTreatmentSubProcess] WHERE FNTPID = " + nFNTPID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.FNTreatmentProcess = _oFNTreatmentProcess.Get(nFNTPID, (int)Session[SessionInfo.currentUserID]);
            return View(oFNTreatmentSubProcesssList);
        }

        [HttpPost]
        public JsonResult SaveSubProcess(FNTreatmentSubProcess oFNTreatmentSubProcess)
        {
            FNTreatmentSubProcess _oFNTreatmentSubProcess = new FNTreatmentSubProcess();
            try
            {
                _oFNTreatmentSubProcess = oFNTreatmentSubProcess.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                _oFNTreatmentSubProcess.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFNTreatmentSubProcess);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteSubProcess(FNTreatmentSubProcess oJB)
        {
            string sFeedBackMessage = "";
            try
            {
                FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                sFeedBackMessage = oFNTreatmentSubProcess.Delete(oJB.FNTreatmentSubProcessID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetSubProcessList(FNProductionBatch oFNProductionBatch)
        {
            List<FNTreatmentSubProcess> oFNTreatmentSubProcesss = new List<FNTreatmentSubProcess>();
            try
            {
                oFNTreatmentSubProcesss = FNTreatmentSubProcess.Gets("SELECT * FROM [View_FNTreatmentSubProcess] WHERE FNTPID = " + oFNProductionBatch.FNTPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                FNTreatmentSubProcess oFNTreatmentSubProcess = new FNTreatmentSubProcess();
                oFNTreatmentSubProcess.ErrorMessage = ex.Message;
                oFNTreatmentSubProcesss.Add(oFNTreatmentSubProcess);
            }

            var jsonResult = Json(oFNTreatmentSubProcesss, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion

    }

}
