using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ESimSol.Reports;
using ESimSolFinancial.Models;
using ICS.Core.Utility;
using System.Web.Script.Serialization;
using OfficeOpenXml.Style;
using OfficeOpenXml;

namespace ESimSolFinancial.Controllers
{
    public class ExportPIShipmentController : Controller
    {
        #region Declaration
        ExportPIShipment _oExportPIShipment = new ExportPIShipment();
        List<ExportPIShipment> _oExportPIShipments = new List<ExportPIShipment>();
        #endregion
        [HttpGet]
        public ActionResult ViewExportPIShipments(int menuid,int buid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            string ssSQL = "SELECT * FROM View_ExportPIShipment  AS HH ORDER BY HH.ExportPIShipmentID ASC";
            _oExportPIShipments = ExportPIShipment.Gets(ssSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oExportPIShipments);
        }

        [HttpPost]
        public JsonResult Save(ExportPIShipment oExportPIShipment)
        {
            _oExportPIShipment = new ExportPIShipment();
            ExportPIShipment _temoExportPIShipment = new ExportPIShipment();
            if(oExportPIShipment.ExportPIID>0)
            _temoExportPIShipment = _temoExportPIShipment.GetByExportPIID(oExportPIShipment.ExportPIID, (int)Session[SessionInfo.currentUserID]);
            try
            {              
                if (_temoExportPIShipment.ExportPIShipmentID > 0)
                {
                    _oExportPIShipment = new ExportPIShipment();
                    _oExportPIShipment.ExportPIShipmentID = _temoExportPIShipment.ExportPIShipmentID;
                    _oExportPIShipment.ShipmentBy = oExportPIShipment.ShipmentBy;
                    _oExportPIShipment.ExportPIID = _temoExportPIShipment.ExportPIID;
                    _oExportPIShipment.ExportBillID = _temoExportPIShipment.ExportBillID;
                    _oExportPIShipment.DestinationPort = oExportPIShipment.DestinationPort;
                    _oExportPIShipment = _oExportPIShipment.Save((int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oExportPIShipment = oExportPIShipment;
                    _oExportPIShipment = _oExportPIShipment.Save((int)Session[SessionInfo.currentUserID]);
                }
                
            }
            catch (Exception ex)
            {
                _oExportPIShipment = new ExportPIShipment();
                _oExportPIShipment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oExportPIShipment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                ExportPIShipment oExportPIShipment = new ExportPIShipment();
                sFeedBackMessage = oExportPIShipment.Delete(id, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    }
}