using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;

using ICS.Core.Utility;
using System.IO;
using System.Drawing.Imaging;
using ESimSol.Reports;
using System.Drawing;


namespace ESimSolFinancial.Controllers
{
    public class FabricMachineTypeController : Controller
    {
        #region Declaration
        FabricMachineType _oFabricMachineType = new FabricMachineType();
        List<FabricMachineType> _oFabricMachineTypes = new List<FabricMachineType>();
        List<FMTTree> _oFMTs = new List<FMTTree>();
        FMTTree _oFMT = new FMTTree();
        string _sErrorMessage = "";
        #endregion

        #region Tree
        private FMTTree GetRoot()
        {
            FMTTree oFMT = new FMTTree();
            foreach (FMTTree oItem in _oFMTs)
            {
                if (oItem.parentid == 0)
                {
                    return oItem;
                }
            }
            return oFMT;
        }

        private List<FMTTree> GetChild(int nRSDID)
        {
            List<FMTTree> oFMTs = new List<FMTTree>();
            foreach (FMTTree oItem in _oFMTs)
            {
                if (oItem.parentid == nRSDID)
                {
                    oFMTs.Add(oItem);
                }
            }
            return oFMTs;
        }

        private void AddTreeNodes(ref FMTTree oFMT)
        {
            List<FMTTree> oChildNodes;
            oChildNodes = GetChild(oFMT.id);
            oFMT.children = oChildNodes;

            foreach (FMTTree oItem in oChildNodes)
            {
                FMTTree oTemp = oItem;
                AddTreeNodes(ref oTemp);
            }
        }

        private FMTTree MakeFMTTree(List<FabricMachineType> oFabricMachineTypes)
        {
            foreach (FabricMachineType oItem in oFabricMachineTypes)
            {
                _oFMT = new FMTTree();
                _oFMT.id = oItem.FabricMachineTypeID;
                _oFMT.parentid = oItem.ParentID;
                _oFMT.text = oItem.Name;
                _oFMT.attributes = "";
                _oFMT.code = "";
                _oFMT.FabricMachineTypeID = oItem.FabricMachineTypeID;
                _oFMT.Name = oItem.Name;
                _oFMT.Brand = oItem.Brand;
                _oFMT.Note = oItem.Note;
                _oFMTs.Add(_oFMT);
            }
            _oFMT = new FMTTree();
            _oFMT = GetRoot();
            this.AddTreeNodes(ref _oFMT);
            return _oFMT;
        }
        #endregion


        public ActionResult ViewFabricMachineTypes(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricMachineType).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oFabricMachineTypes = new List<FabricMachineType>();
            _oFabricMachineTypes = FabricMachineType.Gets((int)Session[SessionInfo.currentUserID]);
            _oFabricMachineType = new FabricMachineType();
            _oFabricMachineType.FMTTree = this.MakeFMTTree(_oFabricMachineTypes);

            return View(_oFabricMachineType);
        }

        public ActionResult ViewFabricMachineType(int id, int selectedID)
        {
            _oFabricMachineType = new FabricMachineType();
            FabricMachineType oFMT = new FabricMachineType();
            if (id > 0)
                _oFabricMachineType = _oFabricMachineType.Get(id, (int)Session[SessionInfo.currentUserID]);
            if (selectedID > 0)
                oFMT = oFMT.Get(selectedID, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SelectedFMT = oFMT;
            return View(_oFabricMachineType);
        }

        [HttpPost]
        public JsonResult Save(FabricMachineType oFabricMachineType)
        {
            _oFabricMachineType = new FabricMachineType();
            try
            {
                _oFabricMachineType = oFabricMachineType.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oFabricMachineType = new FabricMachineType();
                _oFabricMachineType.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFabricMachineType);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                FabricMachineType oFabricMachineType = new FabricMachineType();
                sFeedBackMessage = oFabricMachineType.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
