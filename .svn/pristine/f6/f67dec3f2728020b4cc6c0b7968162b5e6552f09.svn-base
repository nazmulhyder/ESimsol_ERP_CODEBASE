using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using System.Web.Script.Serialization;

namespace ESimSolFinancial.Controllers
{
    public class TextileSubUnitController : Controller
    {
        #region Declaration
        TextileSubUnit _oTextileSubUnit = new TextileSubUnit();
        List<TextileSubUnit> _oTextileSubUnits = new List<TextileSubUnit>();
        TextileSubUnitMachine _oTextileSubUnitMachine = new TextileSubUnitMachine();
        List<TextileSubUnitMachine> _oTextileSubUnitMachines = new List<TextileSubUnitMachine>();
        string _sErrorMessage = "";
        
        #endregion

        public ActionResult View_TextileSubUnit(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'Bank', 'BankBranch'", (int)Session[SessionInfo.currentUserID], ((User)Session[SessionInfo.CurrentUser]).UserID));

            _oTextileSubUnits = new List<TextileSubUnit>();
            _oTextileSubUnits = TextileSubUnit.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

            

            ViewBag.TextileUnits = EnumObject.jGets(typeof(EnumTextileUnit));

            return View(_oTextileSubUnits);
        }

        #region New Code
        [HttpPost]
        public JsonResult Save(TextileSubUnit oTextileSubUnit)
        {
            _oTextileSubUnit = new TextileSubUnit();
            try
            {
                _oTextileSubUnit = oTextileSubUnit;
                _oTextileSubUnit = _oTextileSubUnit.Save((int)EnumDBOperation.Approval,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oTextileSubUnit = new TextileSubUnit();
                _oTextileSubUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTextileSubUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult Delete(TextileSubUnit oTextileSubUnit)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oTextileSubUnit.Delete(oTextileSubUnit.TSUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult Approve(TextileSubUnit oTextileSubUnit)
        {
            try
            {
                if (oTextileSubUnit.TSUID <= 0) { throw new Exception("Please select an valid item."); }


                oTextileSubUnit = oTextileSubUnit.Save((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oTextileSubUnit = new TextileSubUnit();
                oTextileSubUnit.ErrorMessage = ex.Message;
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTextileSubUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Get(TextileSubUnit oTextileSubUnit)
        {
            _oTextileSubUnit = new TextileSubUnit();
            try
            {
                if (oTextileSubUnit.TSUID > 0)
                {
                    _oTextileSubUnit = _oTextileSubUnit.Get(oTextileSubUnit.TSUID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    string sSql = "select * from View_TextileSubUnitMachine where TSUID=" + oTextileSubUnit.TSUID;
                    _oTextileSubUnit.TextileSubUnitMachines = TextileSubUnitMachine.Gets(sSql,((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                _oTextileSubUnit = new TextileSubUnit();
                _oTextileSubUnit.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTextileSubUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Gets(TextileSubUnit oTextileSubUnit)
        {
            try
            {
                string sSQL = "SELECT * FROM View_TextileSubUnit WHERE Name LIKE '%" + oTextileSubUnit.Name + "%' ORDER BY Name";
                _oTextileSubUnits = TextileSubUnit.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oTextileSubUnit = new TextileSubUnit();
                _oTextileSubUnit.ErrorMessage = ex.Message;
                _oTextileSubUnits.Add(_oTextileSubUnit);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTextileSubUnit);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AssignMachineToTextileUnit(TextileSubUnit oTextileSubUnit)
        {
            List<FabricMachine> oFabricMachines = new List<FabricMachine>();
            try
            {
                if (!string.IsNullOrEmpty(oTextileSubUnit.Params))
                {
                    string sSQL = "";
                    var machines = oTextileSubUnit.Params.Split(',').Select(x => Convert.ToInt32(x)).ToList();
                    List<TextileSubUnitMachine> oTSUMs = new List<TextileSubUnitMachine>();
                    machines.ForEach(x=> oTSUMs.Add(new TextileSubUnitMachine{
                        TSUMachineID = 0,
                        TSUID = oTextileSubUnit.TSUID,
                        FMID = x,
                        ActiveDate = DateTime.Now,
                        Note = "",
                    }));

                    oTSUMs = TextileSubUnitMachine.AssignMachineToTextileUnit(oTSUMs, ((User)Session[SessionInfo.CurrentUser]).UserID);

                    if (oTSUMs.Any() && oTSUMs.First().TSUMachineID > 0)
                    {
                        sSQL = "Select * from View_FabricMachine Where FMID In (" + string.Join(",", oTSUMs.Select(x => x.FMID).ToList()) + ")";
                        oFabricMachines = FabricMachine.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    }
                }
            }
            catch (Exception ex)
            {
                oFabricMachines = new List<FabricMachine>();
                oFabricMachines.Add(new FabricMachine { ErrorMessage = ex.Message });
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricMachines);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}

