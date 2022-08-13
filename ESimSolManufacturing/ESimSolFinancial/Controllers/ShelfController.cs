using System;
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


namespace ESimSolFinancial.Controllers
{
    public class ShelfController : Controller
    {
        #region Declaration
        Shelf _oShelf = new Shelf();
        List<Shelf> _oShelfs = new List<Shelf>();
        Rack _oRack = new Rack();
        List<Rack> _oRacks = new List<Rack>();
        string _sSQL = "";
        #endregion
        #region Functions
        private void MakeSQL(string Arguments)
        {
            _sSQL = "";
            _sSQL = "SELECT * FROM Shelf";
            string sCode = (Arguments.Split(';')[1].Split('~')[0] == null) ? "" : Arguments.Split(';')[1].Split('~')[0];
            string sName = (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1];
            //string sShelfIDs = _bForPrint ? (Arguments.Split(';')[1].Split('~')[1] == null) ? "" : Arguments.Split(';')[1].Split('~')[1] : "";
            string sSQL = "";


            #region Code
            if (sCode != null)
            {
                if (sCode != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Code LIKE '%" + sCode + "%' ";
                }
            }
            #endregion
            #region Name
            if (sName != null)
            {
                if (sName != "")
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " Name LIKE '%" + sName + "%' ";
                }
            }
            #endregion
            //#region ShelfIDs
            //if (sShelfIDs != null)
            //{
            //    if (sShelfIDs != "")
            //    {
            //        Global.TagSQL(ref sSQL);
            //        sSQL = sSQL + " ShelfID IN (" + sShelfIDs + ") ";
            //    }
            //}
            //#endregion
            if (sSQL != "")
            { _sSQL = _sSQL + sSQL; }
            _sSQL = _sSQL;
        }
        #endregion

        #region Actions
        public ActionResult ViewShelfs(int buid, int menuid)
        {
       
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.Shelf).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            ViewBag.BUID = buid;//SEt Business UNIt
            _oShelfs = new List<Shelf>();       
            _oShelfs = Shelf.Gets((int)Session[SessionInfo.currentUserID]);

            int nCount = 5, n = 0, id=0;
            OrderInfo oOrderInfo = new OrderInfo();
            List<OrderInfo> oOrderInfos = new List<OrderInfo>();
            foreach (Shelf oItem in _oShelfs)
            {
                n++;
                oOrderInfos = new List<OrderInfo>();
                for (int i = 1; i <= (nCount - n); i++)
                {
                    id++;
                    oOrderInfo = new OrderInfo();
                    oOrderInfo.OrderID = id;
                    oOrderInfo.OrderNo = id.ToString("00000");
                    oOrderInfo.OrderDate = DateTime.Today.AddDays(n);
                    oOrderInfo.OrderQty = (100*id);
                    oOrderInfo.Symbol = "LBS";
                    oOrderInfos.Add(oOrderInfo);
                }
                oItem.OrderInfos = oOrderInfos;
            }
            return View(_oShelfs);
        }

        public ActionResult ViewShelf(int id)
        {
            _oShelf = new Shelf();
            if (id > 0)
            {
                _oShelf = _oShelf.Get(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oShelf);
        }
        [HttpPost]
        public JsonResult Gets(Shelf oShelf)
        {
            List<Shelf> oShelfs = new List<Shelf>();
            oShelf.ErrorMessage = oShelf.ErrorMessage == null ? "" : oShelf.ErrorMessage;
            this.MakeSQL(oShelf.ErrorMessage);

            oShelfs = Shelf.Gets(_sSQL, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oShelfs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(Shelf oShelf)
        {
            _oShelf = new Shelf();
            try
            {
                if (oShelf.ShelfID > 0)
                {
                    _oShelf = _oShelf.Get(oShelf.ShelfID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oShelf = new Shelf();
                _oShelf.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShelf);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        public Image GetDBCompanyLogo(int id)
        {
            #region From DB
            Company oCompany = new Company();
            oCompany = oCompany.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
            #endregion
        }

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Contract Personnel
        public ActionResult ViewRack(int id)
        {
            _oShelf = new Shelf();
            if (id > 0)
            {
                _oShelf = _oShelf.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oShelf.Racks = Rack.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            return View(_oShelf);
        }

        [HttpPost]
        public JsonResult SaveRack(Rack oRack)
        {
            _oRack = new Rack();
            try
            {
                _oRack = oRack;
                _oRack = _oRack.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oRack.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditRack(Rack oRack)
        {
            _oRack = new Rack();
            _oRack = oRack;
            _oRack = _oRack.Save((int)Session[SessionInfo.currentUserID]);


            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteRack(Rack oRack)
        {
            string sFeedBackMessage = "";
            try
            {
                if (oRack.RackID <= 0) { throw new Exception("Please select a valid Rack."); }
                sFeedBackMessage = oRack.Delete(oRack.RackID, (int)Session[SessionInfo.currentUserID]);

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
        public JsonResult GetRacks(Shelf oShelf)
        {
            _oRacks = new List<Rack>();
            _oRack = new Rack();
            try
            {
                if (oShelf.ShelfID <= 0) { throw new Exception("Please select a valid Shelf."); }
                _oRacks = Rack.Gets(oShelf.ShelfID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oRack.ErrorMessage = ex.Message;
                _oRacks.Add(_oRack);
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)_oRacks);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetRack(Rack oRack)
        {
            _oRack = new Rack();
            try
            {
                if (oRack.RackID <= 0) { throw new Exception("Please select a valid Rack."); }
                _oRack = _oRack.Get(oRack.RackID, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oRack.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oRack);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion
     
        [HttpPost]
        public JsonResult Save(Shelf oShelf)
        {
            _oShelf = new Shelf();
            oShelf.ShelfName = oShelf.ShelfName == null ? "" : oShelf.ShelfName;
            oShelf.Remarks = oShelf.Remarks == null ? "" : oShelf.Remarks;
            try
            {
                _oShelf = oShelf;
                _oShelf = _oShelf.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShelf = new Shelf();
                _oShelf.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShelf);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Delete(Shelf oShelf)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oShelf.Delete(oShelf.ShelfID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetShelfs()
        {
            List<Shelf> oShelfs = new List<Shelf>();
            Shelf oShelf = new Shelf();
            oShelf.ShelfName = "-- Select Shelf --";
            oShelfs.Add(oShelf);
            oShelfs.AddRange(Shelf.Gets((int)Session[SessionInfo.currentUserID]));
            
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize((object)oShelfs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PrintShelfs(FormCollection DataCollection)
        {
            _oShelfs = new List<Shelf>();
            _oShelfs = new JavaScriptSerializer().Deserialize<List<Shelf>>(DataCollection["txtShelfCollectionList"]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            

            string Messge = "Shelf List";
            rptShelfs oReport = new rptShelfs();
            byte[] abytes = oReport.PrepareReport(_oShelfs,oCompany, Messge);
            return File(abytes, "application/pdf");

        }


        [HttpPost]
        public JsonResult SearchByShelfBUWise(Shelf oShelf)
        {
            _oShelfs = new List<Shelf>();
            string sSQL = "SELECT * FROM Shelf WHERE ShelfName LIKE '%"+oShelf.ShelfName+"%' AND BUID = "+oShelf.BUID;
            try
            {
                _oShelfs = Shelf.Gets(sSQL,(int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oShelf = new Shelf();
                _oShelf.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oShelfs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Autocomplete Gets
        [HttpGet]
        public JsonResult GetsRackByAutocomplete(string ShelfWithRackNo)
        {
            List<Rack> oRacks = new List<Rack>();
            ShelfWithRackNo = ShelfWithRackNo == null ? "" : ShelfWithRackNo;
            string sSQL = "SELECT * FROM View_Rack AS HH WHERE ISNULL(HH.ShelfName,'') + ISNULL(HH.RackNo,'')  LIKE '%" + ShelfWithRackNo + "%' ORDER BY HH.ShelfName ASC";
            oRacks = Rack.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            var jsonResult = Json(oRacks, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;

            return jsonResult;
        }

        #endregion
       
    }
}
