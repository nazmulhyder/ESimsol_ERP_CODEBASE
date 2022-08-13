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

namespace ESimSolFinancial.Controllers
{
    public class ExportLCUPUDController : Controller
    {
        #region Declaration

        ExportLCUPUD _oExportLCUPUD = new ExportLCUPUD();
        List<ExportLCUPUD> _oExportLCUPUDs = new List<ExportLCUPUD>();
        string _sFormatter = "";
        #endregion

        #region Functions
        #endregion

        #region Actions

        public ActionResult ViewExportLCUPUD(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oExportLCUPUDs = new List<ExportLCUPUD>();
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            if (buid <= 0)
                ViewBag.Units = BusinessUnit.Gets("SELECT * FROM BusinessUnit WHERE BusinessUnitType IN (5,6) ", ((User)(Session[SessionInfo.CurrentUser])).UserID);
            else
                ViewBag.Units = BusinessUnit.Gets("SELECT * FROM BusinessUnit WHERE BusinessUnitID = " + buid, ((User)(Session[SessionInfo.CurrentUser])).UserID);

            ViewBag.BUID = buid;
            return View(_oExportLCUPUDs);
        }

        #endregion

        #region Get

        [HttpPost]
        public JsonResult GetsData(ExportLCUPUD objExportLCUPUD)
        {
            List<ExportLCUPUD> oExportLCUPUDList = new List<ExportLCUPUD>();
            string sSQL = "";
            try
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
                sSQL = GetSQL(objExportLCUPUD);
                oExportLCUPUDList = ExportLCUPUD.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
            }

            var jSonResult = Json(oExportLCUPUDList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetsDataByLCNo(ExportLCUPUD objExportLCUPUD)
        {
            List<ExportLCUPUD> oExportLCUPUDList = new List<ExportLCUPUD>();
            string sSQL = "";
            try
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
                if (!string.IsNullOrEmpty(objExportLCUPUD.LCNo))
                {
                    sSQL = " AND EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  ExportLCNo LIKE '%" + objExportLCUPUD.LCNo + "%' )";
                }
                oExportLCUPUDList = ExportLCUPUD.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
            }

            var jSonResult = Json(oExportLCUPUDList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult GetsDataByPINo(ExportLCUPUD objExportLCUPUD)
        {
            List<ExportLCUPUD> oExportLCUPUDList = new List<ExportLCUPUD>();
            string sSQL = "";
            try
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
                if (!string.IsNullOrEmpty(objExportLCUPUD.PINo))
                {
                    sSQL = " AND EPILCM.PINo LIKE '%" + objExportLCUPUD.PINo + "%' ";
                }
                oExportLCUPUDList = ExportLCUPUD.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
            }

            var jSonResult = Json(oExportLCUPUDList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        public JsonResult GetUDReceive(int nExportLCID, int nVersionNo, int nUDID)
        {
            List<ExportUD> _oExportUDs = new List<ExportUD>();
            try
            {
                string Ssql = "SELECT * FROM View_ExportUD WHERE ExportLCID = " + nExportLCID + " AND ANo = " + nVersionNo + " AND ExportUDID = " + nUDID;
                _oExportUDs = new List<ExportUD>();
                _oExportUDs = ExportUD.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (_oExportUDs.Count > 0)
                {
                    if (nUDID > 0)
                    {
                        Ssql = "SELECT * FROM View_ExportUDDetail WHERE ExportUDID IN (SELECT ExportUDID FROM View_ExportUD WHERE ExportLCID = " + nExportLCID + " AND ANo = " + nVersionNo + ") AND ExportUDID = " + nUDID;
                        _oExportUDs[0].ExportUDDetails = ExportUDDetail.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                _oExportUDs = new List<ExportUD>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportUDs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetExportPILCMapping(int nExportLCID, int nVersionNo)
        {
            List<ExportPILCMapping> _oExportPILCMappings = new List<ExportPILCMapping>();
            try
            {
                string Ssql = "SELECT * FROM View_ExportPILCMapping WHERE ExportLCID = " + nExportLCID + " AND VersionNo = " + nVersionNo +" AND ExportPIID NOT IN (SELECT ExportPIID FROM ExportUDDetail) ";
                _oExportPILCMappings = new List<ExportPILCMapping>();
                _oExportPILCMappings = ExportPILCMapping.Gets(Ssql, ((User)Session[SessionInfo.CurrentUser]).UserID);
                
            }
            catch (Exception ex)
            {
                _oExportPILCMappings = new List<ExportPILCMapping>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPILCMappings);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetExportUDs(ExportUD objExportUD)
        {
            List<ExportUD> oExportUDList = new List<ExportUD>();
            string SQL = "SELECT * FROM View_ExportUD WHERE ISNULL(YetToUPAmount,0) > 0 ", sSQL = " ";//WHERE ExportUDID NOT IN (SELECT isnull(TT.ExportUDID,0) FROM ExportUPDetail AS TT)
            try
            {
                oExportUDList = new List<ExportUD>();
                if (!string.IsNullOrEmpty(objExportUD.UDNo))
                {
                    Global.TagSQL(ref sSQL);
                    sSQL += " ISNULL(UDNo,'') + ISNULL(ExportLCNo,'') LIKE '%" + objExportUD.UDNo + "%' ";
                }
                //if (!string.IsNullOrEmpty(objExportUD.ErrorMessage))
                //{
                //    Global.TagSQL(ref sSQL);
                //    sSQL += " ExportUDID NOT IN (" + objExportUD.ErrorMessage + ") ";
                //}
                if (objExportUD.BUID > 0)
                {
                    Global.TagSQL(ref sSQL);
                    sSQL += " ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE BUID = " + objExportUD.BUID + ")";
                }
                SQL += sSQL;
                oExportUDList = ExportUD.Gets(SQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                oExportUDList = new List<ExportUD>();
            }

            var jSonResult = Json(oExportUDList, JsonRequestBehavior.AllowGet);
            jSonResult.MaxJsonLength = int.MaxValue;
            return jSonResult;
        }

        [HttpPost]
        public JsonResult SaveUD(ExportUD oExportUD)
        {
            ExportUD _oExportUD = new ExportUD();
            try
            {
                _oExportUD = oExportUD.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oExportUD = new ExportUD();
                _oExportUD.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportUD);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        private string GetSQL(ExportLCUPUD oExportLCUPUD)
        {
            string sWhereCluse = " ", sSQL = " ";

            #region UD No
            if (!string.IsNullOrEmpty(oExportLCUPUD.UDNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + "  EUD.UDNo LIKE '%" + oExportLCUPUD.UDNo + "%' ";
            }
            #endregion

            #region UP No
            if (!string.IsNullOrEmpty(oExportLCUPUD.UPNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " EUD.ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE UPNo LIKE '%" + oExportLCUPUD.UPNo + "%')) ";
            }
            #endregion

            #region Bill No
            if (!string.IsNullOrEmpty(oExportLCUPUD.BillNo))
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportBill WHERE ExportBillNo LIKE '%" + oExportLCUPUD.BillNo + "%') ";
            }
            #endregion

            #region Status
            if (oExportLCUPUD.Status > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                if (oExportLCUPUD.Status == 1)//yet to UD
                    sWhereCluse = sWhereCluse + "  ISNULL(EUD.ExportUDID,0) <= 0 ";
                else if (oExportLCUPUD.Status == 2)//yet to UP
                    sWhereCluse = sWhereCluse + " EUD.ExportUDID NOT IN (SELECT isnull(ExportUDID,0) FROM ExportUPDetail where isnull(ExportUDID,0)>0)  ";
                else if (oExportLCUPUD.Status == 3)//UD Rcv but No UP Issue
                    sWhereCluse = sWhereCluse + " ISNULL(EUD.ExportUDID,0) > 0 AND EUD.ExportUDID NOT IN (SELECT isnull(ExportUDID,0) FROM ExportUPDetail where isnull(ExportUDID,0)>0) ";
            }
            #endregion

            #region BUID
            if (oExportLCUPUD.BUID > 0)
            {
                Global.TagSQL(ref sWhereCluse);
                sWhereCluse = sWhereCluse + " EPILCM.BUID = " + oExportLCUPUD.BUID;
            }
            #endregion

            if (!String.IsNullOrEmpty(oExportLCUPUD.ErrorMessage))
            {
                int nCount = 0;
                int nLCOpenDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCOpenDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCOpenDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                int nLCReceiveDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCReceiveDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCReceiveDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                int nUDReceiveDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dUDReceiveDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dUDReceiveDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                int nUPIssueDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dUPIssueDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dUPIssueDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                int nLCShipmentDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCShipmentDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dLCShipmentDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                int nExpiryDate = Convert.ToInt16(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dExpiryDateStart = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);
                DateTime dExpiryDateEnd = Convert.ToDateTime(oExportLCUPUD.ErrorMessage.Split('~')[nCount++]);

                //sSQL = "SELECT * FROM View_ExportLCUPUD ";

                #region LCOpen Date
                if (nLCOpenDate != (int)EnumCompareOperator.None)
                {
                    if (nLCOpenDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCOpenDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCOpenDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCOpenDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCOpenDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCOpenDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),OpeningDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCOpenDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                }
                #endregion

                #region LCReceive Date
                if (nLCReceiveDate != (int)EnumCompareOperator.None)
                {
                    if (nLCReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nLCReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nLCReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nLCReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nLCReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                    else if (nLCReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " CONVERT(DATE,CONVERT(VARCHAR(12),LCReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCReceiveDateEnd.ToString("dd MMM yyyy") + "', 106))";
                    }
                }
                #endregion

                #region UDReceive Date
                if (nUDReceiveDate != (int)EnumCompareOperator.None)
                {
                    if (nUDReceiveDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nUDReceiveDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nUDReceiveDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nUDReceiveDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nUDReceiveDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nUDReceiveDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),UDReceiveDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUDReceiveDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                }
                #endregion

                #region UPIssue Date
                if (nUPIssueDate != (int)EnumCompareOperator.None)
                {
                    if (nUPIssueDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                    else if (nUPIssueDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                    else if (nUPIssueDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                    else if (nUPIssueDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                    else if (nUPIssueDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateEnd.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                    else if (nUPIssueDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportUD WHERE ExportUDID IN (SELECT ExportUDID FROM ExportUPDetail WHERE ExportUPID IN (SELECT ExportUPID FROM ExportUP WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExportUPDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dUPIssueDateEnd.ToString("dd MMM yyyy") + "', 106)) )))";
                    }
                }
                #endregion

                #region LCShipment Date
                if (nLCShipmentDate != (int)EnumCompareOperator.None)
                {
                    if (nLCShipmentDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCShipmentDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCShipmentDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCShipmentDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCShipmentDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nLCShipmentDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ShipmentDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dLCShipmentDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                }
                #endregion

                #region Expiry Date
                if (nExpiryDate != (int)EnumCompareOperator.None)
                {
                    if (nExpiryDate == (int)EnumCompareOperator.EqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) = CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nExpiryDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE  CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) != CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nExpiryDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) > CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nExpiryDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) < CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nExpiryDate == (int)EnumCompareOperator.Between)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                    else if (nExpiryDate == (int)EnumCompareOperator.NotBetween)
                    {
                        Global.TagSQL(ref sWhereCluse);
                        sWhereCluse = sWhereCluse + " EPILCM.ExportLCID IN (SELECT ExportLCID FROM ExportLC WHERE CONVERT(DATE,CONVERT(VARCHAR(12),ExpiryDate,106)) NOT BETWEEN CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateStart.ToString("dd MMM yyyy") + "', 106)) AND CONVERT(DATE, CONVERT(VARCHAR(12), '" + dExpiryDateEnd.ToString("dd MMM yyyy") + "', 106)) )";
                    }
                }
                #endregion

            }
            return sWhereCluse;
        }


        #endregion

        #region print
        //[HttpPost]
        //public ActionResult SetExportLCUPUDListData(ExportLCUPUD oExportLCUPUD)
        //{
        //    this.Session.Remove(SessionInfo.ParamObj);
        //    this.Session.Add(SessionInfo.ParamObj, oExportLCUPUD);

        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(Global.SessionParamSetMessage);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult PrintExportLCUPUDs()
        //{
        //    _oExportLCUPUD = new ExportLCUPUD();
        //    try
        //    {
        //        _oExportLCUPUD = (ExportLCUPUD)Session[SessionInfo.ParamObj];
        //        string sSQL = "SELECT * FROM View_ExportLCUPUD WHERE ExportLCUPUDID IN (" + _oExportLCUPUD.ErrorMessage + ") Order By ExportLCUPUDID";
        //        _oExportLCUPUDs = ExportLCUPUD.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    catch (Exception ex)
        //    {
        //        _oExportLCUPUD = new ExportLCUPUD();
        //        _oExportLCUPUDs = new List<ExportLCUPUD>();
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oExportLCUPUD.Company = oCompany;

        //    rptExportLCUPUDs oReport = new rptExportLCUPUDs();
        //    byte[] abytes = oReport.PrepareReport(_oExportLCUPUDs, oCompany);
        //    return File(abytes, "application/pdf");
        //}

        //public ActionResult ExportLCUPUDPrintPreview(int id)
        //{
        //    _oExportLCUPUD = new ExportLCUPUD();
        //    Company oCompany = new Company();
        //    Contractor oContractor = new Contractor();
        //    BusinessUnit oBusinessUnit = new BusinessUnit();
        //    if (id > 0)
        //    {
        //        _oExportLCUPUD = _oExportLCUPUD.Get(id, (int)Session[SessionInfo.currentUserID]);
        //        _oExportLCUPUD.ExportLCUPUDDetails = ExportLCUPUDDetail.Gets(_oExportLCUPUD.ExportLCUPUDID, (int)Session[SessionInfo.currentUserID]);
        //        //_oExportLCUPUD.BusinessUnit = oBusinessUnit.Get(_oExportLCUPUD.BUID, (int)Session[SessionInfo.currentUserID]);
        //    }
        //    //else
        //    //{
        //    //    _oExportLCUPUD.BusinessUnit = new BusinessUnit();
        //    //}
        //    oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    //_oExportLCUPUD.Company = oCompany;
        //    byte[] abytes;
        //    rptExportLCUPUD oReport = new rptExportLCUPUD();
        //    abytes = oReport.PrepareReport(_oExportLCUPUD, oCompany);
        //    return File(abytes, "application/pdf");
        //}
        //public Image GetCompanyLogo(Company oCompany)
        //{
        //    if (oCompany.OrganizationLogo != null)
        //    {
        //        string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
        //        if (System.IO.File.Exists(fileDirectory))
        //        {
        //            System.IO.File.Delete(fileDirectory);
        //        }

        //        MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
        //        System.Drawing.Image img = System.Drawing.Image.FromStream(m);
        //        img.Save(fileDirectory, ImageFormat.Jpeg);
        //        return img;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}
        #endregion

        #region set
        [HttpPost]
        public ActionResult SetExportLCUPUDData(ExportLCUPUD oExportLCUPUD)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oExportLCUPUD);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region excel

        #region Excel Support
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber)
        {
            return FillCell(sheet, nRowIndex, nStartCol, sVal, IsNumber, false);
        }
        private ExcelRange FillCell(ExcelWorksheet sheet, int nRowIndex, int nStartCol, string sVal, bool IsNumber, bool IsBold)
        {
            ExcelRange cell;
            OfficeOpenXml.Style.Border border;

            cell = sheet.Cells[nRowIndex, nStartCol++];
            if (IsNumber && !string.IsNullOrEmpty(sVal))
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

        public void ExportLCUPUDsExcel()
        {
            List<ExportLCUPUD> oExportLCUPUDList = new List<ExportLCUPUD>();
            ExportLCUPUD objExportLCUPUD = new ExportLCUPUD();
            string sSQL = "";
            try
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
                objExportLCUPUD = (ExportLCUPUD)Session[SessionInfo.ParamObj];
                sSQL = GetSQL(objExportLCUPUD);
                oExportLCUPUDList = ExportLCUPUD.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                oExportLCUPUDList = new List<ExportLCUPUD>();
            }

            if (oExportLCUPUDList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
                #region Header
                List<TableHeader> table_header = new List<TableHeader>();

                table_header.Add(new TableHeader { Header = "#SL", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Customer Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Buying House", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "File No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "P/I No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C No", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C Open Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "A. No", Width = 8f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Amendment Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Shipment Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Expiry Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Issuing Bank Name", Width = 20f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Branch Name", Width = 20f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "L/C Qty", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "L/C Value", Width = 12f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "UD No", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "UD Rcv Date", Width = 13f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delay Days(From Shipment)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delay Days(From Expiry)", Width = 15f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "UP No", Width = 10f, IsRotate = false });

                table_header.Add(new TableHeader { Header = "UP Issue Date", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delay Days(From Shipment)", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Delay Days(From Expiry)", Width = 10f, IsRotate = false });
                table_header.Add(new TableHeader { Header = "Unit", Width = 10f, IsRotate = false });
                #endregion


                #region Export Excel
                int nRowIndex = 2, nStartCol = 2, nEndCol = table_header.Count;
                ExcelRange cell; ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Export LC UD & UP");
                    sheet.Name = "Export_LC_UD&UP";

                    foreach (TableHeader listItem in table_header)
                    {
                        sheet.Column(nStartCol++).Width = listItem.Width;
                    }

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    #endregion
                    nRowIndex++;
                    #region Address & Date
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    #endregion
                    nRowIndex++;
                    cell = sheet.Cells[nRowIndex, 2, nRowIndex, nEndCol + 1]; cell.Merge = true;
                    cell.Value = "Export LC UD & UP List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex += 2;
                    #region Column Header
                    nStartCol = 2;
                    foreach (TableHeader listItem in table_header)
                    {
                        cell = sheet.Cells[nRowIndex, nStartCol++]; cell.Value = listItem.Header; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    }
                    #endregion

                    #region Data

                    nRowIndex++;

                    _sFormatter = " #,##0.00;(#,##0.00)";
                    int nCount = 0;
                    foreach (var oItem in oExportLCUPUDList)
                    {
                        nStartCol = 2;

                        #region DATA                        
                        FillCellMerge(ref sheet, (++nCount).ToString("00"), nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ContractorName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.BuyerName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.FileNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.PINo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.LCNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.LCOpenDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.VersionNo.ToString(), true);

                        FillCellMerge(ref sheet, oItem.AmendmentDateSt, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ShipmentDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ExpiryDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.BankName_Issue, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.BBranchName_Issue, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Qty.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.Amount.ToString(), true);
                        FillCellMerge(ref sheet, oItem.UDNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.UDRecdDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.UDDelayDaysFromShipmjent.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.UDDelayDaysFromExpiry.ToString(), true);
                        FillCellMerge(ref sheet, oItem.UPNo, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCellMerge(ref sheet, oItem.ExportUPDateInString, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.UPDelayDaysFromShipmjent.ToString(), true);
                        FillCell(sheet, nRowIndex, nStartCol++, oItem.UPDelayDaysFromExpiry.ToString(), true);
                        FillCellMerge(ref sheet, oItem.BUName, nRowIndex, nRowIndex, nStartCol, nStartCol++);
                        
                        #endregion
                        nRowIndex++;

                    }

                    #region Total
                    //nStartCol = 2; _sFormatter = " #,##0.00;(#,##0.00)";

                    //FillCellMerge(ref sheet, " Total: ", nRowIndex, nRowIndex, nStartCol, nStartCol += 8, true, ExcelHorizontalAlignment.Right);
                    //FillCell(sheet, nRowIndex, ++nStartCol, nSubTotal_Qty_Requisition.ToString(), true, true);

                    //nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, table_header.Count + 2];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Export_LC_UD&UP.xlsx");
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
