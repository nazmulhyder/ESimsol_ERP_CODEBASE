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
using ReportManagement;
using System.Security;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class DUDyeingTypeController : PdfViewController
    {
        #region DUDyeingType
        public ActionResult View_DUDyeingType(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List< DUDyeingType>  oDUDyeingTypes = new List< DUDyeingType>();
            string sSQL = "Select * from  DUDyeingType";
            oDUDyeingTypes =  DUDyeingType.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.DyeingTypes = EnumObject.jGets(typeof(EumDyeingType));
            ViewBag.BUID = buid;
            return View( oDUDyeingTypes);

        }

        [HttpPost]
        public JsonResult Save( DUDyeingType  oDUDyeingType)
        {
            try
            {
                if ( oDUDyeingType.DUDyeingTypeID <= 0)
                {
                     oDUDyeingType =  oDUDyeingType.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                     oDUDyeingType =  oDUDyeingType.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                 oDUDyeingType = new  DUDyeingType();
                 oDUDyeingType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize( oDUDyeingType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete( DUDyeingType  oDUDyeingType)
        {
            try
            {
                if ( oDUDyeingType.DUDyeingTypeID <= 0) { throw new Exception("Please select an valid item."); }
                 oDUDyeingType =  oDUDyeingType.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                 oDUDyeingType = new  DUDyeingType();
                 oDUDyeingType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize( oDUDyeingType.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Get( DUDyeingType  oDUDyeingType)
        {
            try
            {
                if (oDUDyeingType.DUDyeingTypeID <= 0) throw new Exception("Select a valid item from list");

                else
                {
                    oDUDyeingType = DUDyeingType.Get(oDUDyeingType.DUDyeingTypeID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
               
            }
            catch (Exception ex)
            {
                 oDUDyeingType = new  DUDyeingType();
                 oDUDyeingType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize( oDUDyeingType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }



        #endregion
    }
}