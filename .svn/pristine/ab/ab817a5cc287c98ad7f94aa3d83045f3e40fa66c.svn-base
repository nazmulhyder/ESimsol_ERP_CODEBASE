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
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DesignationResponsibilityController : Controller
    {
        

        #region DesignationResponsibility

        [HttpPost]
        public JsonResult Save(DesignationResponsibility oDesignationResponsibility)
        {
            try
            {
                if(oDesignationResponsibility.DesignationResponsibilitys.Count()<=0)
                    throw new Exception("No items found to save.");

                oDesignationResponsibility = oDesignationResponsibility.Save((int)(Session[SessionInfo.currentUserID]));
            }
            catch (Exception ex)
            {
                oDesignationResponsibility = new DesignationResponsibility();
                oDesignationResponsibility.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDesignationResponsibility);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpGet]
        public JsonResult GetsDesRes(DesignationResponsibility oDesignationResponsibility)
        {
            List<DesignationResponsibility> oDesignationResponsibilitys = new List<DesignationResponsibility>();
            string sSQL = "select * from View_DesignationResponsibility where DRPID = "+oDesignationResponsibility.DRPID +" and DesignationID="+oDesignationResponsibility.DesignationID;
            
            try
            {
                oDesignationResponsibilitys = DesignationResponsibility.Gets(sSQL, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            catch (Exception ex)
            {
                oDesignationResponsibilitys[0] = new DesignationResponsibility();
                oDesignationResponsibilitys[0].ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDesignationResponsibilitys);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


    }
}
