using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;

using System.Web.Script.Serialization;
using ReportManagement;
using ICS.Core.Utility;


namespace ESimSolFinancial.Controllers
{
    public class DyeingSolutiontemplateController : PdfViewController
    {
        #region Declaration
        List<DyeingSolutionTemplet> _oDyeingSolutionTemplets = new List<DyeingSolutionTemplet>();
        //TDSTree _oTDST = new TDSTree();
        #endregion

        #region Dyeing Process
        public ActionResult View_DyeingProcess(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DyeingSolution> oDSs = new List<DyeingSolution>();
            string sSQL="SELECT * FROM DyeingSolution WHERE DyeingSolutionType=1";
            oDSs=DyeingSolution.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
         
            return View(oDSs);
        }

        [HttpPost]
        public JsonResult DyeProcess_IU(DyeingSolution oDyeProcess)// Dyeing Solution of process Type
        {
            DyeingSolution oDS = new DyeingSolution();
            oDyeProcess.DyeingSolutionType = EnumDyeingSolutionType.Process;
            try
            {
                if (oDyeProcess.DyeingSolutionID > 0)
                {
                    oDS = oDyeProcess.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oDS = oDyeProcess.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDS = new DyeingSolution();
                oDS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
      
        
        [HttpPost]
        public JsonResult DyeProcess_Delete(DyeingSolution oDyeProcess)// Dyeing Solution of process Type
        {
            DyeingSolution oDS = new DyeingSolution();
            oDyeProcess.DyeingSolutionType = EnumDyeingSolutionType.Process;
            try
            {
                if (oDyeProcess.DyeingSolutionID > 0)
                {
                    oDS = oDyeProcess.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else { oDS.ErrorMessage = "Please Select an Item to Delete.!!"; }
            }
            catch (Exception ex)
            {
                oDS = new DyeingSolution();
                oDS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDS.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
       
        
        [HttpPost]
        public JsonResult GetsDyeingProcess(DyeingSolution oDyeingSolution)
        {
            List<DyeingSolution> oDyeProcesss = new List<DyeingSolution>();
            string sSQL = "SELECT * FROM DyeingSolution WHERE DyeingSolutionType=" + (int)EnumDyeingSolutionType.Process;
            try
            {
                oDyeProcesss = DyeingSolution.Gets(sSQL,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                DyeingSolution oDS = new DyeingSolution();
                oDS.ErrorMessage = ex.Message;
                oDyeProcesss.Add(oDS);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeProcesss);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Dyeing Solution 
        public ActionResult View_DyeingSolution(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            List<DyeingSolution> oDSs = new List<DyeingSolution>();
            string sSQL = "SELECT * FROM DyeingSolution WHERE DyeingSolutionType In (1,2)";
            oDSs = DyeingSolution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            ViewBag.DPs = oDSs.Where(x => x.DyeingSolutionType == EnumDyeingSolutionType.Process).ToList();
            oDSs = oDSs.Where(x => x.DyeingSolutionType == EnumDyeingSolutionType.Solution).ToList();
            ViewBag.ProductTypes = EnumObject.jGets(typeof(EnumProductNature)).Where(x => x.id == (int)EnumProductNature.Dyes || x.id == (int)EnumProductNature.Chemical).ToList();
            ViewBag.DyeingRecipeType = EnumObject.jGets(typeof(EnumDyeingRecipeType));
            ViewBag.BUID = buid;
            return View(oDSs);
        }

        [HttpPost]
        public JsonResult DyeSolution_IU(DyeingSolution oDyeProcess)// Dyeing Solution of process Type
        {
            DyeingSolution oDS = new DyeingSolution();
            oDyeProcess.DyeingSolutionType = EnumDyeingSolutionType.Solution;
            try
            {
                if (oDyeProcess.DyeingSolutionID > 0)
                {
                    oDS = oDyeProcess.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oDS = oDyeProcess.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDS = new DyeingSolution();
                oDS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetDyeingSolution(DyeingSolution oDyeingSolution)
        {
            try
            {
                if (oDyeingSolution.DyeingSolutionID <= 0) throw new Exception("Please select a valid item.");
                oDyeingSolution = DyeingSolution.Get(oDyeingSolution.DyeingSolutionID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeingSolution(DyeingSolution oDyeingSolution)
        {
            List<DyeingSolution> oDyeingSolutions = new List<DyeingSolution>();
            try
            {
                string sSQL = "SELECT * FROM DyeingSolution WHERE DyeingSolutionType="+ (int)EnumDyeingSolutionType.Solution;
                oDyeingSolutions = DyeingSolution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = ex.Message;
                oDyeingSolutions.Add(oDyeingSolution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolutions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeingSolutionByName(DyeingSolution oDyeingSolution)
        {
            List<DyeingSolution> oDyeingSolutions = new List<DyeingSolution>();
            try
            {
                string sSQL = "SELECT * FROM DyeingSolution WHERE DyeingSolutionID > 0 ";

                if ((int)oDyeingSolution.DyeingSolutionType > 0) 
                {
                    sSQL += " AND DyeingSolutionType=" + (int)EnumDyeingSolutionType.Solution;
                }

                if (!string.IsNullOrEmpty(oDyeingSolution.Name)) 
                {
                    sSQL += " AND Name LIKE '%"+oDyeingSolution.Name+"%'";
                }

                oDyeingSolutions = DyeingSolution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = ex.Message;
                oDyeingSolutions.Add(oDyeingSolution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolutions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsDyeingSolutionByNameTwo(DyeingSolution oDyeingSolution)
        {
            List<DyeingSolution> oDyeingSolutions = new List<DyeingSolution>();
            try
            {
                string sSQL = "SELECT * FROM DyeingSolution WHERE DyeingSolutionID > 0 ";

                if ((int)oDyeingSolution.DyeingSolutionType > 0)
                {
                    sSQL += " AND DyeingSolutionType=" + (int)oDyeingSolution.DyeingSolutionType;
                }

                if (!string.IsNullOrEmpty(oDyeingSolution.Name))
                {
                    sSQL += " AND Name LIKE '%" + oDyeingSolution.Name + "%'";
                }

                oDyeingSolutions = DyeingSolution.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = ex.Message;
                oDyeingSolutions.Add(oDyeingSolution);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolutions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult DyeSolution_Copy(DyeingSolution oDyeingSolution)
        {      
            try
            {
                if (oDyeingSolution.DyeingSolutionID <= 0) throw new Exception("Please select a valid dyeing solution from list.");
                oDyeingSolution = oDyeingSolution.Copy(oDyeingSolution.DyeingSolutionID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDyeingSolution.ErrorMessage != "")
                {
                    throw new Exception(oDyeingSolution.ErrorMessage);
                }
            }
            catch (Exception ex)
            {
                oDyeingSolution = new DyeingSolution();
                oDyeingSolution.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolution);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region  Dyeing Templete

        [HttpPost]
        public JsonResult GetDyeingSolutionTemplet(DyeingSolutionTemplet oDST)// Dyeing Solution of process Type
        {

            try
            {
                if (oDST.DyeingSolutionID <= 0) throw new Exception("Please select a valid dyeing solution.");
                oDST = DyeingSolutionTemplet.Get(oDST.DSTID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDST = new DyeingSolutionTemplet();
                oDST.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDST);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetsDyeingSolutionTemplet(DyeingSolutionTemplet oDST)// Dyeing Solution of process Type
        {

            try
            {
                if (oDST.DyeingSolutionID <= 0) throw new Exception("Please select a valid dyeing solution.");
                List<DyeingSolutionTemplet> oDSTs = new List<DyeingSolutionTemplet>();

                string sSQL = "SELECT * FROM VIEW_DyeingSolutionTemplet WHERE DyeingSolutionID=" + oDST.DyeingSolutionID + " Order By Sequence";
                oDSTs = DyeingSolutionTemplet.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oDSTs.Count <= 0)
                {
                    //Insert root
                    DyeingSolutionTemplet oTempDST = new DyeingSolutionTemplet();
                    oTempDST.DyeingSolutionID = oDST.DyeingSolutionID;
                    oTempDST.ProcessID = 0;
                    oTempDST.ParentID = 0;
                    oTempDST.IsDyesChemical = false;
                    oTempDST.TempTime = "N/A";
                    oTempDST.GL = 0;
                    oTempDST.Percentage = 0;
                    oTempDST.Note = "N/A";
                    oTempDST = oTempDST.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oDSTs.Add(oTempDST);
                }
                oDSTs.Where(x => (x.ProcessID == 0 && x.ParentID == 0 && x.IsDyesChemical == false)).ToList().ForEach(x => x.ProcessName = x.DyeingSolutionName);
                oDST = this.MakeTree(oDSTs);


            }
            catch (Exception ex)
            {
                oDST = new DyeingSolutionTemplet();
                oDST.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDST);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DyeSolutionTemplate_IU(DyeingSolutionTemplet oDST)// Dyeing Solution Template
        {
            try
            {
                if (oDST.DSTID > 0)
                {
                    oDST = oDST.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oDST = oDST.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oDST = new DyeingSolutionTemplet();
                oDST.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDST);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DyeSolutionTemplate_Delete(DyeingSolutionTemplet oDST)// Dyeing Solution Template
        {
            try
            {
                if (oDST.DSTID <= 0) throw new Exception("Please select a valid item from list.");
                oDST = oDST.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oDST = new DyeingSolutionTemplet();
                oDST.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDST.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Tree Generator


        private DyeingSolutionTemplet MakeTree(List<DyeingSolutionTemplet> oDSTs)
        {
            DyeingSolutionTemplet oDST = new DyeingSolutionTemplet();
            if (oDSTs.Count() > 0)
            {
                oDST = oDSTs.Where(x => x.ParentID == 0).ElementAtOrDefault(0);
                this.AddChildNode(ref oDST, oDSTs.Where(x => x.DSTID != oDST.DSTID).ToList());
            }
            return oDST;
        }

        public void AddChildNode(ref DyeingSolutionTemplet oDST, List<DyeingSolutionTemplet> oDSTs)
        {
            int nParentID = oDST.DSTID;
            oDST.children = oDSTs.Where(x => x.ParentID == nParentID).ToList();
            if (oDST.children.Count() > 0 && oDSTs.Count() > 0)
            {
                oDSTs.RemoveAll(x => x.ParentID == nParentID);
                foreach (DyeingSolutionTemplet OItem in oDST.children)
                {
                    DyeingSolutionTemplet oTempNode = OItem;
                    this.AddChildNode(ref oTempNode, oDSTs);
                }
            }

        }
        #region Sequence
        [HttpPost]
        public JsonResult GetDSTChilds(DyeingSolutionTemplet oDyeingSolutionTemplet)
        {
            _oDyeingSolutionTemplets = new List<DyeingSolutionTemplet>();
            try
            {
                DyeingSolutionTemplet oTempDST = new DyeingSolutionTemplet();
                oTempDST = DyeingSolutionTemplet.Get(oDyeingSolutionTemplet.ParentID, (int)Session[SessionInfo.currentUserID]);
                oDyeingSolutionTemplet.ProcessName = "Selected parent proces : " + oTempDST.DyeingSolutionName;
                string sSQL = "SELECT * FROM VIEW_DyeingSolutionTemplet WHERE ParentID = " + oDyeingSolutionTemplet.ParentID + " Order By Sequence";
                oDyeingSolutionTemplet.children = DyeingSolutionTemplet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDyeingSolutionTemplet = new DyeingSolutionTemplet();
                oDyeingSolutionTemplet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolutionTemplet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult RefreshMenuSequence(DyeingSolutionTemplet oDyeingSolutionTemplet)
        {
            try
            {
                oDyeingSolutionTemplet = oDyeingSolutionTemplet.RefreshSequence((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oDyeingSolutionTemplet.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oDyeingSolutionTemplet);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #endregion

        #region Get Product BU, User and Name wise ( write by Mamun)
        [HttpPost]
        public JsonResult GetProducts(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            EnumProductUsages eEnumProductUsages = new EnumProductUsages();
            try
            {

                if (oProduct.ProductCategoryID == (int)EnumProductNature.Dyes)
                {
                    eEnumProductUsages = EnumProductUsages.Dyes;

                }
                else if (oProduct.ProductCategoryID == (int)EnumProductNature.Chemical)
                {
                    eEnumProductUsages = EnumProductUsages.Chemical;
                }
                else
                {
                    eEnumProductUsages = EnumProductUsages.Dyes_Chemical;
                }

                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.Labdip, eEnumProductUsages, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.Labdip, eEnumProductUsages, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }

            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            var jsonResult = Json(oProducts, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }

        #endregion
    }
}

