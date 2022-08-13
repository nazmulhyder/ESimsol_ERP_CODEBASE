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
using iTextSharp.text.pdf;
using System.Web.Services;
using System.Dynamic;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace ESimSolFinancial.Controllers
{
    public class FabricExecutionOrderSpecificationController : Controller
    {
        #region Declaration
        List<FabricExecutionOrderSpecification> _oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
        FabricSalesContractDetail _oFEOrder = new FabricSalesContractDetail();
        FabricSalesContractDetail _oFEODetail = new FabricSalesContractDetail();
        List<FabricSalesContractDetail> _oFEODetails = new List<FabricSalesContractDetail>();
        List<Product> _oProducts = new List<Product>();

        string _sErrorMessage = "";
        #endregion
        public System.Drawing.Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                string fileDirectory = Server.MapPath("~/Content/CompanyLogo.jpg");
                if (System.IO.File.Exists(fileDirectory))
                {
                    System.IO.File.Delete(fileDirectory);
                }
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(fileDirectory, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        #region Fabric Execution Order Specification
        public ActionResult ViewFabricSpecifications(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricSpecification).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);
            _oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
            _oFabricExecutionOrderSpecifications = FabricExecutionOrderSpecification.Gets("Select top(100)* from View_FabricExecutionOrderSpecification where isnull(ApproveBy,0)=0 Order by FEOSID desc", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
            ViewBag.FabricSpeTypes = EnumObject.jGets(typeof(EnumFabricSpeType));
            return View(_oFabricExecutionOrderSpecifications);
        }
        public ActionResult ViewSpecification(int nFSCDID, int nFEOSID, double nts)
        {
            List<AuthorizationRoleMapping> oARMs = new List<AuthorizationRoleMapping>();
            oARMs = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.FabricSpecification).ToString() , (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, oARMs);
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            Contractor oContractor = new Contractor();
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            oFEOS.FSCDID = nFSCDID;
            oFEOS.FEOSID = nFEOSID;
            oFEOS = FabricSpecification(oFEOS, true);
            if (oFEOS.FEOSID > 0)
            {
                oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            if (oFEOS.IsOutSide && oFEOS.ContractorID>0)
            {
                oContractor = oContractor.Get(oFEOS.ContractorID,((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            ViewBag.FSCDID = nFSCDID;
            ViewBag.Contractor = oContractor;
            ViewBag.FabricSpecificationNotes = oFabricSpecificationNotes;
            ViewBag.WarpWeftTypes = EnumObject.jGets(typeof(EnumWarpWeft));
            return View(oFEOS);
        }

        public ActionResult ViewFabricSpecification(int nFSCDID, double nts)
        {
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            FabricSalesContractDetail oFabricSalesContractDetail = new FabricSalesContractDetail();

            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            oFEOS.FSCDID = nFSCDID;
            oFEOS = FabricSpecification(oFEOS, true);
            if (oFEOS.FEOSID > 0)
            {
                oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            if (nFSCDID > 0)
            {
                oFabricSalesContractDetail = oFabricSalesContractDetail.Get(nFSCDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);

                ViewBag.FNExecutionOrderProcessList =  FNExecutionOrderProcess.Gets(nFSCDID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            }
            ViewBag.FSCDID = nFSCDID;
            ViewBag.FabricSalesContractDetail = oFabricSalesContractDetail;
            //ViewBag.FabricDyeingRecipe = FabricDyeingRecipe.Gets("Select * from View_FabricDyeingRecipe WHERE FEOSID = '" + nFSCDID + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.FNExecutionOrderProcessList = FNExecutionOrderProcess.Gets(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)(Session[SessionInfo.CurrentUser])).UserID);
            ViewBag.FabricSpecificationNotes = oFabricSpecificationNotes;
            return View(oFEOS);
        }

        public JsonResult GetFEOSByFEO(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                List<FabricExecutionOrderSpecification> oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
                if (oFEOS.FSCDID > 0)
                {
                    string sSQL = "SELECT * FROM View_FabricExecutionOrderSpecification WHERE FSCDID = " + oFEOS.FSCDID;
                    oFabricExecutionOrderSpecifications = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (oFabricExecutionOrderSpecifications.Count > 0)
                    {
                        oFEOS = oFabricExecutionOrderSpecifications[0];
                    }
                    else
                    {
                        oFEOS = new FabricExecutionOrderSpecification();
                    }
                }
                else
                {
                    oFEOS = new FabricExecutionOrderSpecification();
                }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveFEOSpecification(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                List<FabricQtyAllow> oFabricQtyAllows = new List<FabricQtyAllow>();
                //oFabricQtyAllows = FabricQtyAllow.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);

                if (oFEOS.IsTEndsAdd)
                { oFEOS.GroundEnds = (oFEOS.TotalEnds + oFEOS.TotalEndsAdd) - oFEOS.SelvedgeEnds - oFEOS.SelvedgeEndTwo; }
                else { oFEOS.GroundEnds = (oFEOS.TotalEnds - oFEOS.TotalEndsAdd) - oFEOS.SelvedgeEnds - oFEOS.SelvedgeEndTwo; }

                if (oFEOS.FEOSID <= 0)
                {
                    oFabricQtyAllows = FabricQtyAllow.Getsby(oFEOS.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFEOS.FabricQtyAllows = oFabricQtyAllows;
                    oFEOS = oFEOS.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFEOS = oFEOS.IUD((int)EnumDBOperation.Update, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                if (oFEOS.FEOSID > 0)
                {
                    oFEOS = FabricSpecification(oFEOS, true);
                }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveLog(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                if (oFEOS.IsTEndsAdd)
                { oFEOS.GroundEnds = (oFEOS.TotalEnds + oFEOS.TotalEndsAdd) - oFEOS.SelvedgeEnds - oFEOS.SelvedgeEndTwo; }
                else { oFEOS.GroundEnds = (oFEOS.TotalEnds - oFEOS.TotalEndsAdd) - oFEOS.SelvedgeEnds - oFEOS.SelvedgeEndTwo; }

                if (oFEOS.FEOSID <= 0)
                {
                    oFEOS = oFEOS.IUD((int)EnumDBOperation.Insert, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oFEOS = oFEOS.IUD((int)EnumDBOperation.Revise, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                //if (oFEOS.FEOSID > 0)
                //{
                    oFEOS = FabricSpecification(oFEOS, true);
                //}
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFEOSpecification(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                if (oFEOS.FEOSID <= 0) { throw new Exception("Please select an valid item."); }

                oFEOS = oFEOS.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFDetail(FabricExecutionOrderSpecificationDetail oFEOSDetail)
        {
            try
            {
                if (oFEOSDetail.FEOSDID<=0) { throw new Exception("Please select an valid item."); }

                oFEOSDetail = oFEOSDetail.IUD((int)EnumDBOperation.Delete, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                oFEOSDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOSDetail.ErrorMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
    
        [HttpPost]
        public JsonResult DeleteFDetailAll(FabricExecutionOrderSpecificationDetail oFabricExecutionOrderSpecificationDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricExecutionOrderSpecificationDetail.DeleteAll(((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult ApproveFEOSpecification(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                if (oFEOS.FEOSID <= 0) { throw new Exception("Please select an valid item."); }
                oFEOS = oFEOS.IUD((int)EnumDBOperation.Approval, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFEOS.FEOSID > 0) { oFEOS = FabricSpecification(oFEOS, true); }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveDyeingOrder(FabricExecutionOrderSpecification oFEOS)
        {
            string sCode = "";
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricDispo> oFabricDispos = new List<FabricDispo>();
            bool _bRePro = false;
            if (oFEOS.ProdtionTypeInt == 2 || oFEOS.ProdtionTypeInt == 3)
            {
                _bRePro = true;
            }
            try
            {
                oFabricSCReport = oFabricSCReport.Get(oFEOS.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFEOS = FabricExecutionOrderSpecification.Get(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricDispos = FabricDispo.Gets("SELECT * FROM FabricDispo where FabricDispo.BusinessUnitType=" + (int)EnumBusinessUnitType.Dyeing + " and IsYD=1 and FabricOrderType in (" + oFabricSCReport.OrderType + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFEOS.IsYD == true) // if yarn Dyed
                {
                  
                    if (oFabricDispos.Count > 0)
                    {
                        sCode = oFabricDispos.Where(x => x.IsReProduction == _bRePro && x.BusinessUnitType == EnumBusinessUnitType.Dyeing && x.IsYD == true).FirstOrDefault().Code;
                        if (!string.IsNullOrEmpty(oFEOS.ExeNo))
                        {
                            oFEOS.ExeNo = oFEOS.ExeNo.Remove(0, 1);
                        }
                        oFEOS.ExeNo = sCode + "" + oFEOS.ExeNo;
                    }
                }
                else
                {
                    oFEOS.ExeNo =  oFEOS.ExeNo;
                }
                if (oFEOS.FEOSID <= 0) { throw new Exception("Please select an valid item."); }
                oFEOS = oFEOS.IUD_DO( ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oFEOS.FEOSID > 0) { oFEOS = FabricSpecification(oFEOS, true); }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateAllowance(FabricExecutionOrderSpecificationDetail oFEDetail)
        {

            try
            {
                //oFEDetail.ColorName = oFEDetail.ColorName.Trim();
                oFEDetail = oFEDetail.UpdateAllowance((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFEDetail = new FabricExecutionOrderSpecificationDetail();
                oFEDetail.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateOutSide(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                oFEOS = oFEOS.UpdateOutSide((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetFEOSpecification(FabricExecutionOrderSpecification oFEOS)
        {
            try
            {
                if (oFEOS.FSCDID <= 0) { throw new Exception("Please select an valid item."); }
                oFEOS = FabricSpecification(oFEOS, true);
                if (oFEOS.FEOSID > 0)
                {
                    oFEOS.FabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
                oFEOS.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFEOS(FabricExecutionOrderSpecification oFEOS)
        {
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            FabricExecutionOrderSpecification oFEO = new FabricExecutionOrderSpecification();
            try
            {
                if (oFEOS.SCNoFull.Trim() == "") { throw new Exception("Please enter valid sales contact no."); }
                string sSQL = "select * from View_FabricExecutionOrderSpecification Where ApproveBy>0 And FEONo Like '%" + oFEOS.SCNoFull.Trim() + "%'";

                oFEOSs = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFEOSs = new List<FabricExecutionOrderSpecification>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult GetsLabdips(FabricExecutionOrderSpecificationDetail oFabricPattern)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sReturn1 = "SELECT top(100)* FROM View_LabdipDetail ";
                string sReturn = "";
                if (!string.IsNullOrEmpty(oFabricPattern.ColorName))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ColorName LIKE '%" + oFabricPattern.ColorName + "%'";
                }
                if (!string.IsNullOrEmpty(oFabricPattern.ColorNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ColorNo LIKE '%" + oFabricPattern.ColorNo + "%'";
                }
                if (oFabricPattern.ProductID > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "  ProductID = " + oFabricPattern.ProductID;
                }
                //if (oFabricPattern.FabricID > 0)
                //{
                //    Global.TagSQL(ref sReturn);
                //    sReturn = sReturn + "  FabricID = " + oFabricPattern.FabricID;
                //}

                string sSQL = sReturn1 + " " + sReturn + " ORDER BY ColorName";
                oLabDipDetails = LabDipDetail.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oLabDipDetails = new List<LabDipDetail>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public FabricExecutionOrderSpecification FabricSpecification(FabricExecutionOrderSpecification oFEOS, bool IsCustomBreakDown)
        {
            FabricSCReport oFabricSCReport = new FabricSCReport();
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            string sSQL = string.Empty;
            if (oFEOS.FEOSID> 0)
                sSQL = "Select * from View_FabricExecutionOrderSpecification Where FEOSID= " + oFEOS.FEOSID + " Order By FEOSID DESC";
            else if (oFEOS.FSCDID> 0)
                sSQL = "Select * from View_FabricExecutionOrderSpecification Where FSCDID= " + oFEOS.FSCDID + " and isnull(IsOutSide,0)=0 and isnull(ProdtionType,0) in (" + (int)EnumDispoProType.None + "," + (int)EnumDispoProType.General + ") Order By FEOSID DESC";
            if (sSQL!=string.Empty)
                oFEOSs = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFEOSs.Count() > 0)
            {
                if (oFEOS.FEOSID > 0)
                {
                    oFEOS = oFEOSs.Where(x => x.FEOSID == oFEOS.FEOSID).ElementAtOrDefault(0);
                    if (oFEOS == null) { oFEOS = new FabricExecutionOrderSpecification(); }
                }
                else if (oFEOS.FEOSID <= 0) { oFEOS = oFEOSs[0]; }

                oFEOS.FabricSpecifications = oFEOSs.Where(x => x.FEOSID != oFEOS.FEOSID).Select(x => new FabricSpecification { FEOSID = x.FEOSID, SpecificationNo = x.FESONo, ApproveBy = x.ApproveBy }).ToList();
                oFEOS.FEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                oFEOS.FEOSDetails = FabricExecutionOrderSpecificationDetail.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);



                //if (oFEOS.FEOSDetailsWeft.Count > 0)
                //{
                //    if (oFEOS.FEOSDetailsWeft.FirstOrDefault() != null && oFEOS.FEOSDetailsWeft.FirstOrDefault().FEOSDID > 0 && oFEOS.FEOSDetailsWeft.Where(x => x.TwistedGroup > 0).Count() > 0)
                //    {
                //        List<FabricExecutionOrderSpecificationDetail> oFEOSDetailsWeft = new List<FabricExecutionOrderSpecificationDetail>();
                //        oFEOS.FEOSDetailsWeft.ForEach((item) => { oFEOSDetailsWeft.Add(item); });
                //        oFEOS.FEOSDetailsWeft = this.TwistedDetails(oFEOS.FEOSDetailsWeft);
                //        oFEOS.FEOSDetailsWeft[0].CellRowSpansWeft = this.RowMerge(oFEOSDetailsWeft.Where(x => x.IsWarp == false).ToList());
                //    }
                //}

                oFEOS.YarnDyeingBreakDowns = GenerateYarnDyeingBreakDown(oFEOS.FEOSDetails, oFEOS);
            }
            else
            {
                FabricPattern oFabricPattern = new FabricPattern();
                sSQL = "SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFEOS.FSCDID;
                var result = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (result.Any() && result.First().FabricSalesContractDetailID > 0)
                    oFabricSCReport = result.First();
                //oFabricSCReport = oFabricSCReport.Get(oFEOS.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                List<Fabric> oFabrics = new List<Fabric>();
                if (oFEOS.FSCDID > 0)
                {
                    oFabrics = Fabric.Gets("Select * from View_Fabric where FabricID in (Select FabricID from FabricSalesContractDetail where FabricSalesContractDetailID=oFEOS.FSCDID)", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if(oFabrics.Count>0)
                    {
                        oFEOS.NoOfFrame = oFabrics[0].NoOfFrame.ToString();
                    }
                    if (oFabricSCReport.FabricSalesContractDetailID > 0)
                    {
                        oFEOS.Qty = oFabricSCReport.Qty;
                        oFEOS.QtyOrder = oFabricSCReport.Qty;
                        oFEOS.FabricID = oFabricSCReport.FabricID;
                        oFEOS.SCNoFull = oFabricSCReport.SCNoFull;
                        oFEOS.FabricNo = oFabricSCReport.FabricNo;
                        oFEOS.PINo = oFabricSCReport.PINo;
                        oFEOS.Construction = oFabricSCReport.Construction;
                        oFEOS.Composition = oFabricSCReport.ProductName;
                        oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                        oFEOS.Weave = oFabricSCReport.FabricWeaveName;
                        oFEOS.BuyerName = oFabricSCReport.BuyerName;
                        oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                        oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                        oFEOS.FinishWidthFS = oFabricSCReport.FabricWidth;
                        oFEOS.HLReference = oFabricSCReport.HLReference;
                        oFEOS.RefNo = oFabricSCReport.HLReference;
                        oFEOS.Crimp = 3.5;
                        if (oFabricSCReport.OrderType == (int)EnumFabricRequestType.Sample || oFabricSCReport.OrderType == (int)EnumFabricRequestType.SampleFOC)
                        {
                            if (oFEOS.OrderQtyInMeter <= 50) { oFEOS.QtyExtraMet = 70 - oFEOS.OrderQtyInMeter; }
                            if (oFEOS.OrderQtyInMeter > 50) { oFEOS.QtyExtraMet = 20; }
                            oFEOS.GreigeDemand = Math.Round((oFEOS.OrderQtyInMeter + oFEOS.QtyExtraMet) / ((100.0 - 8.0) / 100.0),2);
                            oFEOS.LoomPPAdd = 10;
                            oFEOS.ReqLoomProduction =Math.Round(oFEOS.LoomPPAdd+oFEOS.GreigeDemand / ((100.0 - 2.0) / 100.0),2);
                            oFEOS.WarpLenAdd = 40;
                            oFEOS.RequiredWarpLength =Math.Round( oFEOS.WarpLenAdd + oFEOS.ReqLoomProduction / ((100.0 - 10.5) / 100.0),2);
                           

                        }
                        else
                        {
                            oFEOS.GreigeDemand = (oFEOS.OrderQtyInMeter + oFEOS.QtyExtraMet);
                            oFEOS.ReqLoomProduction = oFEOS.GreigeDemand ;
                            oFEOS.LoomPPAdd = 0;
                            oFEOS.RequiredWarpLength = oFEOS.ReqLoomProduction;
                            oFEOS.WarpLenAdd = 0;
                        }
                        oFEOS.FEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                    }
                }
                sSQL = "Select * from View_FabricPattern Where ApproveBy<>0 And IsActive=1 And FabricID=" + oFEOS.FabricID + "";
                List<FabricPattern> oFabricPatterns = new List<FabricPattern>();
                oFabricPatterns = FabricPattern.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricExecutionOrderSpecificationDetail oFEOSDetail=new FabricExecutionOrderSpecificationDetail();
                if (oFabricPatterns.Count() == 1 && oFabricPatterns[0].FPID > 0)
                {
                    int nSlNo = 0;
                    oFabricPattern = oFabricPatterns[0];
                    if (oFabricPattern.FPID > 0)
                    {
                        oFabricPattern.FPDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (FabricPatternDetail oItem in oFabricPattern.FPDetails)
                        {
                            nSlNo = nSlNo + 1;
                            oFEOSDetail=new FabricExecutionOrderSpecificationDetail();
                            oFEOSDetail.IsWarp = oItem.IsWarp;
                            oFEOSDetail.PantonNo = oItem.PantonNo;
                            oFEOSDetail.ProductID = oItem.ProductID;
                            if (oItem.IsWarp==true &&(oFabricSCReport.OrderType == (int)EnumFabricRequestType.Sample || oFabricSCReport.OrderType == (int)EnumFabricRequestType.SampleFOC))
                            {
                                oFEOSDetail.AllowanceWarp =10.0;
                            }
                            if ((oFabricSCReport.OrderType == (int)EnumFabricRequestType.Sample || oFabricSCReport.OrderType == (int)EnumFabricRequestType.SampleFOC))
                            {
                                oFEOSDetail.Allowance = 10.0;
                            }
                            oFEOSDetail.ProductName = oItem.ProductName;
                            oFEOSDetail.ProductShortName = oItem.ProductShortName;
                            oFEOSDetail.LabdipDetailID = oItem.LabDipDetailID;
                            oFEOSDetail.EndsCount = oItem.EndsCount;
                            oFEOSDetail.ColorName = oItem.ColorName;
                            oFEOSDetail.TwistedGroupInt = oItem.TwistedGroupInt;
                            oFEOSDetail.TwistedGroup = oItem.TwistedGroup;
                            oFEOSDetail.Value = oItem.Value;
                            oFEOSDetail.SLNo = nSlNo;
                            oFEOS.FEOSDetails.Add(oFEOSDetail); 
                        }
                    }
                }

            }
            if (oFEOS.FEOSDetails.Count > 0)
            {
                if (oFEOS.FEOSDetails.FirstOrDefault() != null && oFEOS.FEOSDetails.FirstOrDefault().ProductID > 0 && oFEOS.FEOSDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                {
                    List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                    oFEOS.FEOSDetails.ForEach((item) => { oFEOSDetails.Add(item); });
                    oFEOS.FEOSDetails = this.TwistedDetails(oFEOS.FEOSDetails);
                    oFEOS.FEOSDetails[0].CellRowSpans = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == true).ToList());
                    oFEOS.FEOSDetails[0].CellRowSpansWeft = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == false).ToList());
                }
            }
            return oFEOS;
        }
        //public List<dynamic> GenerateWarpWeftPlanFEOSDetail(List<FabricExecutionOrderSpecificationDetail> oFEOSDWarpOrWefts, FabricExecutionOrderSpecification oFEOS)
        //{
        //    List<dynamic> oDynamicList = new List<dynamic>();
        //    List<PlanGroup> oPlanGroups = new List<PlanGroup>();

        //    if (oFEOSDWarpOrWefts.Count() > 0)
        //    {
        //        bool IsWarp = oFEOSDWarpOrWefts[0].IsWarp;
        //        oFEOSDWarpOrWefts = oFEOSDWarpOrWefts.OrderBy(x => x.FEOSDID).ToList();
        //        oPlanGroups = new List<PlanGroup>();
        //        oPlanGroups = oFEOSDWarpOrWefts.GroupBy(x => new { x.ColorName, x.ProductCode, x.ProductName, x.ProductShortName, x.Value, x.ValueMin }).Select(x => new PlanGroup
        //        {
        //            ColorName = x.Key.ColorName,
        //            ProductCode = x.Key.ProductCode,
        //            ProductName = x.Key.ProductName,
        //            ProductShortName = x.Key.ProductShortName,
        //            Value = x.Key.Value,
        //            ValueMin = x.Key.ValueMin,
        //            RowCount = x.Count(),
        //        }).ToList();


        //        int nEndsRow = ((oPlanGroups.Max(x => x.RowCount) < 24) ? 24 : oPlanGroups.Max(x => x.RowCount));

        //        foreach (PlanGroup oItem in oPlanGroups)
        //        {
        //            bool bIsInitialSapn = true;
        //            int nCount = 0;
        //            dynamic obj = new ExpandoObject();
        //            var oExpObj = obj as IDictionary<string, Object>;
        //            oExpObj.Add("ProductName", oItem.ProductShortName);
        //            oExpObj.Add("ColorName", oItem.ColorName);
        //            for (int i = 0; i < oFEOSDWarpOrWefts.Count(); i++)
        //            {
        //                if (oItem.ColorName == oFEOSDWarpOrWefts[i].ColorName && oItem.ProductCode == oFEOSDWarpOrWefts[i].ProductCode)
        //                {
        //                    bIsInitialSapn = false;
        //                    oExpObj.Add("E" + (nCount++).ToString(), oFEOSDWarpOrWefts[i].EndsCount);
        //                }
        //                else
        //                {
        //                    int nSelectedIndex = oPlanGroups.IndexOf(oItem);
        //                    int nPreviousIndex = (i <= 0) ? 0 : oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFEOSDWarpOrWefts[i - 1].ColorName && x.ProductCode == oFEOSDWarpOrWefts[i - 1].ProductCode)).ElementAtOrDefault(0));
        //                    int nCurrentIndex = oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFEOSDWarpOrWefts[i].ColorName && x.ProductCode == oFEOSDWarpOrWefts[i].ProductCode)).ElementAtOrDefault(0));

        //                    if (nCurrentIndex < nPreviousIndex)
        //                    {
        //                        List<FabricExecutionOrderSpecificationDetail> oFEOSDs = new List<FabricExecutionOrderSpecificationDetail>();
        //                        oFEOSDs = (oFEOSDWarpOrWefts.GetRange(0, oPlanGroups.Count() - 1).Where(x => (x.ColorName == oItem.ColorName && x.ProductCode == oItem.ProductCode)).Count() > 0 || !bIsInitialSapn) ? oFEOSDWarpOrWefts.Where(x => x.FEOSDID >= oFEOSDWarpOrWefts[i].FEOSDID).ToList() : oFEOSDWarpOrWefts;

        //                        List<int> oPrecedences = new List<int>();
        //                        if (oFEOSDs.Count() > 0)
        //                        {
        //                            oFEOSDs = oFEOSDs.OrderBy(x => x.FEOSDID).ToList();

        //                            for (int j = oPlanGroups.Where(x => (x.ColorName != oItem.ColorName && x.ProductCode != oItem.ProductCode)).Count(); j < oFEOSDs.Count(); j++)
        //                            {
        //                                int nIndex = oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFEOSDs[j].ColorName && x.ProductCode == oFEOSDs[j].ProductCode)).ElementAtOrDefault(0));
        //                                if (oPrecedences.Count() > 0 && oPrecedences.LastOrDefault() > nIndex) { break; }
        //                                oPrecedences.Add(nIndex);
        //                            }
        //                            if (!oPrecedences.Contains(nSelectedIndex)) { oExpObj.Add("E" + (nCount++).ToString(), ""); }
        //                        }
        //                    }
        //                }
        //            }
        //            while (nCount < nEndsRow) { oExpObj.Add("E" + (nCount++).ToString(), ""); }


        //            int nEndsCount = oFEOSDWarpOrWefts.Where(x => (x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Sum(x => x.EndsCount);
        //            oExpObj.Add("TotalEndsCount", nEndsCount);

        //            double nWeightInKg = 0;
        //            if (IsWarp) { nWeightInKg = (nEndsCount <= 0 || oFEOS.RequiredWarpLength <= 0 || oFEOS.GroundEnds <= 0 || oItem.Value <= 0) ? 0 : (nEndsCount * oFEOS.RequiredWarpLength * oFEOS.GroundEnds) / (1693.3 * oItem.Value * oFEOSDWarpOrWefts.Sum(x => x.EndsCount)); }
        //            else { nWeightInKg = (nEndsCount <= 0 || oFEOS.FinishPick <= 0 || oFEOS.GreigeDemand <= 0 || oItem.Value <= 0) ? 0 : (nEndsCount * oFEOS.FinishPick * oFEOS.GreigeDemand * 69) / (1693.3 * oItem.Value * oFEOSDWarpOrWefts.Sum(x => x.EndsCount)); }
        //            oExpObj.Add("Weight", Math.Round(nWeightInKg, 2));

        //            oDynamicList.Add(oExpObj);
        //        }
        //    }



        //    return oDynamicList;
        //}
        //public List<dynamic> GenerateWarpWeftPlanFPDetail(List<FabricPatternDetail> oFPDWarpOrWefts, FabricExecutionOrderSpecification oFEOS)
        //{
        //    List<dynamic> oDynamicList = new List<dynamic>();
        //    List<PlanGroup> oPlanGroups = new List<PlanGroup>();
        //    if (oFPDWarpOrWefts.Count() > 0)
        //    {
        //        bool IsWarp = oFPDWarpOrWefts[0].IsWarp;

        //        oFPDWarpOrWefts = oFPDWarpOrWefts.OrderBy(x => x.FPDID).ToList();
        //        oPlanGroups = new List<PlanGroup>();
        //        oPlanGroups = oFPDWarpOrWefts.GroupBy(x => new { x.ColorName, x.ProductCode, x.ProductName, x.ProductShortName, x.Value }).Select(x => new PlanGroup
        //        {
        //            ColorName = x.Key.ColorName,
        //            ProductCode = x.Key.ProductCode,
        //            ProductName = x.Key.ProductName,
        //            ProductShortName = x.Key.ProductShortName,
        //            Value = x.Key.Value,
        //            RowCount = x.Count(),
        //        }).ToList();


        //        int nEndsRow = ((oPlanGroups.Max(x => x.RowCount) < 24) ? 24 : oPlanGroups.Max(x => x.RowCount));

        //        foreach (PlanGroup oItem in oPlanGroups)
        //        {
        //            bool bIsInitialSapn = true;
        //            int nCount = 0;
        //            dynamic obj = new ExpandoObject();
        //            var oExpObj = obj as IDictionary<string, Object>;
        //            oExpObj.Add("ProductName", oItem.ProductShortName);
        //            oExpObj.Add("ColorName", oItem.ColorName);

        //            for (int i = 0; i < oFPDWarpOrWefts.Count(); i++)
        //            {
        //                if (oItem.ColorName == oFPDWarpOrWefts[i].ColorName && oItem.ProductCode == oFPDWarpOrWefts[i].ProductCode)
        //                {
        //                    bIsInitialSapn = false;
        //                    oExpObj.Add("E" + (nCount++).ToString(), oFPDWarpOrWefts[i].EndsCount);
        //                }
        //                else
        //                {
        //                    int nSelectedIndex = oPlanGroups.IndexOf(oItem);
        //                    int nPreviousIndex = (i <= 0) ? 0 : oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFPDWarpOrWefts[i - 1].ColorName && x.ProductCode == oFPDWarpOrWefts[i - 1].ProductCode)).ElementAtOrDefault(0));
        //                    int nCurrentIndex = oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFPDWarpOrWefts[i].ColorName && x.ProductCode == oFPDWarpOrWefts[i].ProductCode)).ElementAtOrDefault(0));

        //                    if (nCurrentIndex < nPreviousIndex)
        //                    {
        //                        List<FabricPatternDetail> oFPDs = new List<FabricPatternDetail>();
        //                        oFPDs = (oFPDWarpOrWefts.GetRange(0, oPlanGroups.Count() - 1).Where(x => (x.ColorName == oItem.ColorName && x.ProductCode == oItem.ProductCode)).Count() > 0 || !bIsInitialSapn) ? oFPDWarpOrWefts.Where(x => x.FPDID >= oFPDWarpOrWefts[i].FPDID).ToList() : oFPDWarpOrWefts;

        //                        List<int> oPrecedences = new List<int>();
        //                        if (oFPDs.Count() > 0)
        //                        {
        //                            oFPDs = oFPDs.OrderBy(x => x.FPDID).ToList();

        //                            for (int j = oPlanGroups.Where(x => (x.ColorName != oItem.ColorName && x.ProductCode != oItem.ProductCode)).Count(); j < oFPDs.Count(); j++)
        //                            {
        //                                int nIndex = oPlanGroups.IndexOf(oPlanGroups.Where(x => (x.ColorName == oFPDs[j].ColorName && x.ProductCode == oFPDs[j].ProductCode)).ElementAtOrDefault(0));
        //                                if (oPrecedences.Count() > 0 && oPrecedences.LastOrDefault() > nIndex) { break; }
        //                                oPrecedences.Add(nIndex);
        //                            }
        //                            if (!oPrecedences.Contains(nSelectedIndex)) { oExpObj.Add("E" + (nCount++).ToString(), ""); }
        //                        }
        //                    }
        //                }
        //            }
        //            while (nCount < nEndsRow) { oExpObj.Add("E" + (nCount++).ToString(), ""); }


        //            int nEndsCount = oFPDWarpOrWefts.Where(x => (x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Sum(x => x.EndsCount);
        //            oExpObj.Add("TotalEndsCount", nEndsCount);

        //            double nWeightInKg = 0;
        //            if (IsWarp) { nWeightInKg = (nEndsCount <= 0 || oFEOS.RequiredWarpLength <= 0 || oFEOS.GroundEnds <= 0 || oItem.Value <= 0) ? 0 : (nEndsCount * oFEOS.RequiredWarpLength * oFEOS.GroundEnds) / (1693.3 * oItem.Value * oFPDWarpOrWefts.Sum(x => x.EndsCount)); }
        //            else { nWeightInKg = (nEndsCount <= 0 || oFEOS.FinishPick <= 0 || oFEOS.GreigeDemand <= 0 || oItem.Value <= 0) ? 0 : (nEndsCount * oFEOS.FinishPick * oFEOS.GreigeDemand * 69) / (1693.3 * oItem.Value * oFPDWarpOrWefts.Sum(x => x.EndsCount)); }
        //            oExpObj.Add("Weight", Math.Round(nWeightInKg, 2));

        //            oDynamicList.Add(oExpObj);
        //        }
        //    }



        //    return oDynamicList;
        //}
        public List<dynamic> GenerateYarnDyeingBreakDown(List<FabricExecutionOrderSpecificationDetail> oFEOSDWarpOrWefts, FabricExecutionOrderSpecification oFEOS)
        {
            List<dynamic> oDynamicList = new List<dynamic>();
            List<PlanGroup> oPlanGroups = new List<PlanGroup>();
            FabricExecutionOrderSpecificationDetail oFEOSD=new FabricExecutionOrderSpecificationDetail();
            oFEOSDWarpOrWefts = oFEOSDWarpOrWefts.OrderBy(x => x.FEOSDID).ToList();

            oPlanGroups = new List<PlanGroup>();
            oPlanGroups = oFEOSDWarpOrWefts.GroupBy(x => new {x.FEOSID, x.ProductID, x.ProductCode, x.ProductName, x.ProductShortName, x.ColorName, x.LabdipDetailID, x.Value, x.Allowance,x.BatchNo,x.LDNo }).Select(x => new PlanGroup
            {
                ProductID = x.Key.ProductID,
                ColorName = x.Key.ColorName,
                ProductCode = x.Key.ProductCode,
                ProductName = x.Key.ProductName,
                ProductShortName = x.Key.ProductShortName,
                LabdipDetailID = x.Key.LabdipDetailID,
                Value = x.Key.Value,
                Allowance = x.Key.Allowance,
                BatchNo = x.Key.BatchNo,
                LDNo = x.Key.LDNo,
                FEOSID = x.Key.FEOSID,
                RowCount = x.Count(),
            }).ToList();

            double nWarpWeightInkg = 0, nWeftWeightInkg = 0, nTotalkg = 0;
            foreach (PlanGroup oItem in oPlanGroups)
            {
                nWarpWeightInkg = 0; nWeftWeightInkg = 0;

                nWarpWeightInkg = oFEOSDWarpOrWefts.Where(x => (x.IsWarp == true && x.ProductID == oItem.ProductID && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName && x.LabdipDetailID == oItem.LabdipDetailID)).Sum(x => x.Qty);

                nWeftWeightInkg = oFEOSDWarpOrWefts.Where(x => (x.IsWarp == false && x.ProductID == oItem.ProductID && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName && x.LabdipDetailID == oItem.LabdipDetailID)).Sum(x => x.Qty);

                //if (oFEOS.FabricQtyAllows.Count > 0)
                //{
                //    if (nWarpWeightInkg > 0 && nWarpWeightInkg > 0)
                //    {
                //        if (oFEOS.FabricQtyAllows.FirstOrDefault() != null && oFEOS.FabricQtyAllows.FirstOrDefault().Qty_From > 0 && oFEOS.FabricQtyAllows.Where(b => b.Qty_From <= (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && b.Qty_To > Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2) && b.AllowType == EnumFabricQtyAllowType.Dyeing && b.WarpWeftType == EnumWarpWeft.WarpnWeft).Count() > 0)
                //        {
                //            oItem.Allowance = oFEOS.FabricQtyAllows.Where(x => (x.Qty_From <= (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && x.Qty_To > (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && x.AllowType == EnumFabricQtyAllowType.WarpnWeft && x.WarpWeftType == EnumWarpWeft.WarpnWeft)).First().Percentage;
                //        }
                //    }
                //    else
                //    {
                //        if (oFEOS.FabricQtyAllows.FirstOrDefault() != null && oFEOS.FabricQtyAllows.FirstOrDefault().Qty_From > 0 && oFEOS.FabricQtyAllows.Where(b => b.Qty_From <= (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && b.Qty_To > Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2) && b.AllowType == EnumFabricQtyAllowType.Dyeing && b.WarpWeftType == EnumWarpWeft.Warp).Count() > 0)
                //        {
                //            oItem.Allowance = oFEOS.FabricQtyAllows.Where(x => (x.Qty_From <= (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && x.Qty_To > (Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2)) && x.AllowType == EnumFabricQtyAllowType.WarpnWeft && x.WarpWeftType == EnumWarpWeft.Warp)).First().Percentage;
                //        }
                //    }

                //    oFEOSD.LDNo = oItem.LDNo; oFEOSD.BatchNo = oItem.BatchNo; oFEOSD.Allowance = oItem.Allowance; oFEOSD.ColorName = oItem.ColorName;
                //    oFEOSD.LabdipDetailID = oItem.LabdipDetailID; oFEOSD.ProductID = oItem.ProductID; oFEOSD.Value = oItem.Value; oFEOSD.FEOSID = oItem.FEOSID;
                //    oFEOSD = oFEOSD.UpdateAllowance((int)Session[SessionInfo.currentUserID]);

                //}

                //if (oFEOSDWarpOrWefts.Where(x => (x.IsWarp == true && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Count() > 0)
                //{
                //    int nEndsCount = oFEOSDWarpOrWefts.Where(x => (x.IsWarp == true && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Sum(x => x.EndsCount);
                //    nWarpWeightInkg = (nEndsCount <= 0 || oFEOS.RequiredWarpLength <= 0 || oFEOS.GroundEnds <= 0 || oItem.Value <= 0) ? 0 : Math.Round((nEndsCount * oFEOS.RequiredWarpLength * oFEOS.GroundEnds) / (1693.3 * oItem.Value * oFEOSDWarpOrWefts.Where(x => x.IsWarp == true).Sum(x => x.EndsCount)), 2);
                //}
                //if (oFEOSDWarpOrWefts.Where(x => (x.IsWarp == false && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Count() > 0)
                //{
                //    int nEndsCount = oFEOSDWarpOrWefts.Where(x => (x.IsWarp == false && x.ProductCode == oItem.ProductCode && x.ColorName == oItem.ColorName)).Sum(x => x.EndsCount);
                //    nWeftWeightInkg = (nEndsCount <= 0 || oFEOS.FinishPick <= 0 || oFEOS.GreigeDemand <= 0 || oItem.Value <= 0) ? 0 : Math.Round((nEndsCount * oFEOS.FinishPick * oFEOS.GreigeDemand * oFEOS.Crimp) / (1693.3 * oItem.Value * oFEOSDWarpOrWefts.Where(x => x.IsWarp == false).Sum(x => x.EndsCount)), 2);
                //}
            
                dynamic obj = new ExpandoObject();
                var oExpObj = obj as IDictionary<string, Object>;
                oExpObj.Add("BatchNo", oItem.BatchNo);
                oExpObj.Add("LDNo", oItem.LDNo);
                oExpObj.Add("LabdipDetailID", oItem.LabdipDetailID);
                oExpObj.Add("ProductID", oItem.ProductID);
                oExpObj.Add("Value", oItem.Value);
                oExpObj.Add("FEOSID", oItem.FEOSID);
                //oExpObj.Add("Allowance", oItem.Allowance);
                oExpObj.Add("ProductName", oItem.ProductName);
                oExpObj.Add("ColorName", oItem.ColorName);
                oExpObj.Add("WarpWeightInkg", nWarpWeightInkg);
                oExpObj.Add("WeftWeightInkg", nWeftWeightInkg);
                oExpObj.Add("TotalWeightInkg", Math.Round((nWarpWeightInkg + nWeftWeightInkg), 2));
                oExpObj.Add("Allowance", Math.Round((oItem.Allowance), 2));
                nTotalkg=(nWarpWeightInkg + nWeftWeightInkg) /((100-oItem.Allowance) / 100);
                if (nTotalkg < 1) { nTotalkg = 1;}
                oExpObj.Add("WeightWithAllowanceInkg", nTotalkg);
                oDynamicList.Add(oExpObj);
            }
            return oDynamicList;
        }
       
        //public ActionResult PrintFEOSpecification(int nFEOSID, double nts)
        //{
        //    FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
        //    oFEOS = FabricExecutionOrderSpecification.Get(nFEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    if (oFEOS.FEOSID > 0)
        //    {
        //        oFEOS.FEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
        //        oFEOS.FEOSDetails = FabricExecutionOrderSpecificationDetail.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

        //        List<FabricExecutionOrderSpecificationDetail> oFEOSDsWarp = new List<FabricExecutionOrderSpecificationDetail>();
        //        List<FabricExecutionOrderSpecificationDetail> oFEOSDsWeft = new List<FabricExecutionOrderSpecificationDetail>();

        //        oFEOSDsWarp = oFEOS.FEOSDetails.Where(x => x.IsWarp == true).ToList();
        //        oFEOSDsWeft = oFEOS.FEOSDetails.Where(x => x.IsWarp == false).ToList();

        //        if (oFEOSDsWarp.Count() > 0) { oFEOS.WarpPlans = GenerateWarpWeftPlanFEOSDetail(oFEOSDsWarp, oFEOS).ToList(); }
        //        if (oFEOSDsWeft.Count() > 0) { oFEOS.WeftPlans = GenerateWarpWeftPlanFEOSDetail(oFEOSDsWeft, oFEOS).ToList(); }
        //        oFEOS.YarnDyeingBreakDowns = GenerateYarnDyeingBreakDown(oFEOS.FEOSDetails, oFEOS);
        //    }
        //    Company oCompany = new Company();
        //    oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
        //    oCompany.CompanyLogo = GetCompanyLogo(oCompany);
        //    rptFEOSpecification oReport = new rptFEOSpecification();
        //    byte[] abytes = oReport.PrepareReport(oFEOS, oCompany);
        //    return File(abytes, "application/pdf");

        //}
        public ActionResult PrintFabricSpecification(int nId, double nts)
        {
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            List<FabricDeliverySchedule> oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            List<FabricDyeingRecipe> oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
            Fabric oFabric = new Fabric();
            List<FabricDispo> oFabricDispos = new List<FabricDispo>();
            
            oFEOS.FEOSID = nId;
            oFEOS = FabricSpecificationEntry(oFEOS, false);

            FabricSCReport oFabricSCReport = new FabricSCReport();
            string sSQL = "SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFEOS.FSCDID;
            var result = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricSalesContract oFSC = new FabricSalesContract();
           
            if (result.Any() && result.First().FabricSalesContractDetailID > 0)
            {
                oFabricSCReport = result.First();
                oFSC = oFSC.Get(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oFabricDispos = FabricDispo.Gets("SELECT * FROM FabricDispo where FabricDispo.FabricOrderType=" + oFSC.OrderType , ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFebricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFabric = oFabric.Get(oFabricSCReport.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (oFEOS.FEOSID > 0)
            {
                oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            Contractor oContractor = new Contractor();
            if (oFEOS.IsOutSide && oFEOS.ContractorID > 0) // if Out side 
            {
                oContractor = oContractor.Get(oFEOS.ContractorID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            rptFabricSpecification oReport = new rptFabricSpecification();
            byte[] abytes = oReport.PrepareReport(oFEOS, oFabricSCReport, oFSC, oFabric, oCompany, oFebricDeliverySchedules, oFabricSalesContractNotes, oFabricSpecificationNotes, oFabricDispos, oFabricDyeingRecipes, oContractor);

            return File(abytes, "application/pdf");
        }
        public ActionResult PrintFabricJobOrder(int nId, double nts)
        {
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            List<FabricDeliverySchedule> oFebricDeliverySchedules = new List<FabricDeliverySchedule>();
            List<FabricSalesContractNote> oFabricSalesContractNotes = new List<FabricSalesContractNote>();
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            List<FabricDyeingRecipe> oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
            Fabric oFabric = new Fabric();
            oFEOS.FEOSID = nId;
            oFEOS = FabricSpecificationEntry(oFEOS, false);

            FabricSCReport oFabricSCReport = new FabricSCReport();
            string sSQL = "SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFEOS.FSCDID;
            var result = FabricSCReport.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            FabricSalesContract oFSC = new FabricSalesContract();

            if (result.Any() && result.First().FabricSalesContractDetailID > 0)
            {
                oFabricSCReport = result.First();
                oFSC = oFSC.Get(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            oFebricDeliverySchedules = FabricDeliverySchedule.GetsFSCID(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oFabricSalesContractNotes = FabricSalesContractNote.Gets(oFabricSCReport.FabricSalesContractID, ((User)Session[SessionInfo.CurrentUser]).UserID);

            oFabric = oFabric.Get(oFabricSCReport.FabricID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(oFSC.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFEOS.FEOSID > 0)
            {
                oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricDyeingRecipes = FabricDyeingRecipe.Gets("Select * from View_FabricDyeingRecipe WHERE FEOSID = " + oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            rptFabricSpecification oReport = new rptFabricSpecification();
            byte[] abytes = oReport.PrepareReportJobOrder(oFEOS, oFabricSCReport, oFSC, oFabric, oCompany, oFebricDeliverySchedules, oFabricSalesContractNotes, oFabricSpecificationNotes, oBusinessUnit, oFabricDyeingRecipes);

            return File(abytes, "application/pdf");
        }

        public FabricExecutionOrderSpecification FabricSpecificationEntry(FabricExecutionOrderSpecification oFEOS, bool IsCustomBreakDown)
        {
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
            string sSQL = string.Empty;
            if (oFEOS.FSCDID > 0)
                sSQL = "Select * from View_FabricExecutionOrderSpecification Where FSCDID= " + oFEOS.FSCDID + " Order By FEOSID DESC";
            else if (oFEOS.FEOSID > 0)
                sSQL = "Select * from View_FabricExecutionOrderSpecification Where FEOSID= " + oFEOS.FEOSID + " Order By FEOSID DESC";

            if (sSQL != string.Empty)
                oFEOSs = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            if (oFEOSs.Count() > 0)
            {
                if (oFEOS.FEOSID > 0)
                {
                    oFEOS = oFEOSs.Where(x => x.FEOSID == oFEOS.FEOSID).ElementAtOrDefault(0);
                    if (oFEOS == null) { oFEOS = new FabricExecutionOrderSpecification(); }
                }
                else if (oFEOS.FEOSID <= 0) { oFEOS = oFEOSs[0]; }

                //oFEOS.FabricSpecifications = oFEOSs.Where(x => x.FEOSID != oFEOS.FEOSID).Select(x => new FabricSpecification { FEOSID = x.FEOSID, SpecificationNo = x.FESONo, ApproveBy = x.ApproveBy }).ToList();
                oFEOS.FEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                oFEOS.FEOSDetails = FabricExecutionOrderSpecificationDetail.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
              
            }
            return oFEOS;
        }

        #endregion

        #region Fabric Specification Note
        public ActionResult ViewFabricSpecificationNote(int menuid)
        {
            FabricSpecificationNote oFabricSpecificationNote = new FabricSpecificationNote();
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            oFabricSpecificationNotes = FabricSpecificationNote.Gets(((User)Session[SessionInfo.CurrentUser]).UserID);
            return View(oFabricSpecificationNotes);
        }
        [HttpPost]
        public JsonResult SaveFabricSpecificationNote(FabricSpecificationNote oFabricSpecificationNote)
        {
            try
            {
                oFabricSpecificationNote = oFabricSpecificationNote.Save(oFabricSpecificationNote, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                oFabricSpecificationNote = new FabricSpecificationNote();
                oFabricSpecificationNote.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSpecificationNote);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteFabricSpecificationNote(FabricSpecificationNote oFabricSpecificationNote)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFabricSpecificationNote.Delete(oFabricSpecificationNote.FabricSpecificationNoteID, ((User)Session[SessionInfo.CurrentUser]).UserID);
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
        public JsonResult SaveFabricSpecificationProduct(List<DyeingSolution> oDyeingSolutions)
        {
            FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
            List<FabricDyeingRecipe> oFabricDyeingRecipes = new List<FabricDyeingRecipe>();
            string ErrorMessage = "";
            try
            {
                int nId = Convert.ToInt32(oDyeingSolutions[0].ErrorMessage);
                for (int i = 0; i < oDyeingSolutions.Count();i++ )
                {
                    oFabricDyeingRecipe.FEOSID = nId;
                    oFabricDyeingRecipe.DyeingSolutionID = oDyeingSolutions[i].DyeingSolutionID;
                    oFabricDyeingRecipe.Save(oFabricDyeingRecipe, ((User)Session[SessionInfo.CurrentUser]).UserID);
                    oFabricDyeingRecipes = FabricDyeingRecipe.Gets("Select * from View_FabricDyeingRecipe WHERE FEOSID = '" + nId + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch
            {
                ErrorMessage = "Data Not Saved";
            }
            return Json(oFabricDyeingRecipes);
        }
        [HttpPost]
        public JsonResult DeleteFabricSpecificationProduct(DyeingSolution oDyeingSolution)
        {
            string ErrorMessage = "";
            try
            {
                int nId = Convert.ToInt32(oDyeingSolution.ErrorMessage); // need to change
                FabricDyeingRecipe oFabricDyeingRecipe = new FabricDyeingRecipe();
                oFabricDyeingRecipe.Delete("DELETE FROM FabricDyeingRecipe WHERE FEOSID = '" + nId + "' AND DyeingSolutionID = '" + oDyeingSolution.DyeingSolutionID + "'", ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch
            {
                ErrorMessage = "Data Not Deleted";
            }
            return Json(ErrorMessage);
        }
        [HttpPost]
        public JsonResult GetsByExeNo(FabricExecutionOrderSpecification oFEO)
        {
            List<FabricExecutionOrderSpecification> oFEOSs = new List<FabricExecutionOrderSpecification>();
          
            try
            {
                string sSQL = "select top(100)* from View_FabricExecutionOrderSpecification where isnull(ExeNo,'')!=''";

                if (!string.IsNullOrEmpty(oFEO.ExeNo))
                {
                    Global.TagSQL(ref sSQL);
                    sSQL = sSQL + " ExeNo LIKE '%" + oFEO.ExeNo + "%' ";
                }
                sSQL = sSQL + " order by FEOSID DESC";

                oFEOSs = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

            }
            catch (Exception ex)
            {
                oFEOSs = new List<FabricExecutionOrderSpecification>();
            }

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOSs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetsFEOSID(FabricExecutionOrderSpecification oFEOS)
        {
            FabricExecutionOrderSpecification oFEO = new FabricExecutionOrderSpecification();
            FabricSCReport oFabricSCReport = new FabricSCReport();
           int nSL=0;
            try
            {
               var result = FabricSCReport.Gets("SELECT * FROM View_FabricSalesContractReport Where FabricSalesContractDetailID =" + oFEOS.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                if (result.Any() && result.First().FabricSalesContractDetailID > 0)
                    oFabricSCReport = result.First();

                oFEO = FabricExecutionOrderSpecification.Get(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFEOS.FEOSDetails = FabricExecutionOrderSpecificationDetail.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);

                foreach (FabricExecutionOrderSpecificationDetail oItem in oFEOS.FEOSDetails)
                {
                    nSL=nSL+1;
                    oItem.SLNo = nSL;
                }


                if (oFabricSCReport.FabricSalesContractDetailID > 0)
                {
                    oFEOS.Qty = oFabricSCReport.Qty;
                    oFEOS.FabricID = oFabricSCReport.FabricID;
                    oFEOS.SCNoFull = oFabricSCReport.SCNoFull;
                    oFEOS.FabricNo = oFabricSCReport.FabricNo;
                    oFEOS.PINo = oFabricSCReport.PINo;
                    oFEOS.Construction = oFabricSCReport.Construction;
                    oFEOS.Composition = oFabricSCReport.ProductName;
                    oFEOS.Qty = oFabricSCReport.Qty;
                    oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                    oFEOS.Weave = oFabricSCReport.FabricWeaveName;
                    oFEOS.BuyerName = oFabricSCReport.BuyerName;
                    oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                    oFEOS.FinishType = oFabricSCReport.FinishTypeName;
                    oFEOS.FinishWidthFS = oFabricSCReport.FabricWidth;
                    oFEOS.HLReference = oFabricSCReport.HLReference;
                    oFEOS.FSCDID = oFabricSCReport.FabricSalesContractDetailID;
                }
              
                if (oFEOS.FEOSDetails.Count > 0)
                {
                    if (oFEOS.FEOSDetails.FirstOrDefault() != null && oFEOS.FEOSDetails.FirstOrDefault().FEOSDID > 0 && oFEOS.FEOSDetails.Where(x => x.TwistedGroup > 0).Count() > 0)
                    {
                        List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                        oFEOS.FEOSDetails.ForEach((item) => { oFEOSDetails.Add(item); });
                        oFEOS.FEOSDetails = this.TwistedDetails(oFEOS.FEOSDetails);
                        //oFEOS.FEOSDetails[0].CellRowSpans = this.RowMerge(oFEOSDetails);
                        oFEOS.FEOSDetails[0].CellRowSpans = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == true).ToList());
                        oFEOS.FEOSDetails[0].CellRowSpansWeft = this.RowMerge(oFEOSDetails.Where(x => x.IsWarp == false).ToList());
                    }
                }
                oFEOS.FEOSID = 0;
                oFEOS.FEOSDetails.ForEach(o => o.FEOSDID = 0);
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOS);
            return Json(sjson, JsonRequestBehavior.AllowGet);
           
        }
        [HttpPost]
        public JsonResult GetsFSpecNote(FabricExecutionOrderSpecification oFEOS)
        {
            List<FabricSpecificationNote> oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            try
            {
                oFabricSpecificationNotes = FabricSpecificationNote.Gets(oFEOS.FEOSID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                oFabricSpecificationNotes.ForEach(o => o.FabricSpecificationNoteID = 0);
                oFabricSpecificationNotes.ForEach(o => o.FEOSID = 0);
            }
            catch (Exception ex)
            {
                oFabricSpecificationNotes = new List<FabricSpecificationNote>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFabricSpecificationNotes);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult GetsPatarn(FabricExecutionOrderSpecification oFEOS)
        {
            string sSQL = "";
            FabricPattern oFabricPattern = new FabricPattern();
            List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
            try
            {

                sSQL = "Select * from View_FabricPattern Where ApproveBy<>0 And IsActive=1 And FabricID=" + oFEOS.FabricID + "";
                List<FabricPattern> oFabricPatterns = new List<FabricPattern>();
                oFabricPatterns = FabricPattern.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                FabricExecutionOrderSpecificationDetail oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                if (oFabricPatterns.Count() == 1 && oFabricPatterns[0].FPID > 0)
                {
                    oFabricPattern = oFabricPatterns[0];
                    if (oFabricPattern.FPID > 0)
                    {
                        oFabricPattern.FPDetails = FabricPatternDetail.Gets(oFabricPattern.FPID, ((User)Session[SessionInfo.CurrentUser]).UserID);
                        foreach (FabricPatternDetail oItem in oFabricPattern.FPDetails)
                        {
                            oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                            oFEOSDetail.IsWarp = oItem.IsWarp;
                            oFEOSDetail.PantonNo = oItem.PantonNo;
                            oFEOSDetail.ProductID = oItem.ProductID;
                            oFEOSDetail.ProductName = oItem.ProductName;
                            oFEOSDetail.ProductShortName = oItem.ProductShortName;
                            oFEOSDetail.LabdipDetailID = oItem.LabDipDetailID;
                            oFEOSDetail.EndsCount = oItem.EndsCount;
                            oFEOSDetail.ColorName = oItem.ColorName;
                            oFEOSDetail.TwistedGroup = oItem.TwistedGroup;
                            oFEOSDetail.Value = oItem.Value;
                            oFEOSDetails.Add(oFEOSDetail);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                oFEOS = new FabricExecutionOrderSpecification();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oFEOSDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        #endregion 

        #region Merger Row
        private List<CellRowSpan> RowMerge(List<FabricExecutionOrderSpecificationDetail> oFabricExecutionOrderSpecificationDetails)
        {

            List<CellRowSpan> oCellRowSpans = new List<CellRowSpan>();
            int[] rowIndex = new int[1];
            int[] rowSpan = new int[1];

            List<FabricExecutionOrderSpecificationDetail> oTWGLDDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricExecutionOrderSpecificationDetail> oLDDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricExecutionOrderSpecificationDetail> oTempLDDetails = new List<FabricExecutionOrderSpecificationDetail>();

            oTWGLDDetails = oFabricExecutionOrderSpecificationDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricExecutionOrderSpecificationDetails.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricExecutionOrderSpecificationDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricExecutionOrderSpecificationDetails.FirstOrDefault().FEOSDID == oTWGLDDetails.FirstOrDefault().FEOSDID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();

                    oFabricExecutionOrderSpecificationDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricExecutionOrderSpecificationDetails.FirstOrDefault().FEOSDID == oLDDetails.FirstOrDefault().FEOSDID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FEOSDID == oLDDetails.FirstOrDefault().FEOSDID).ToList();

                    oFabricExecutionOrderSpecificationDetails.RemoveAll(x => x.FEOSDID == oTempLDDetails.FirstOrDefault().FEOSDID);
                    oLDDetails.RemoveAll(x => x.FEOSDID == oTempLDDetails.FirstOrDefault().FEOSDID);
                }

                rowIndex[0] = rowIndex[0] + rowSpan[0];
                rowSpan[0] = oTempLDDetails.Count();
                oCellRowSpans.Add(MakeSpan.GenerateRowSpan("TwistedGroup", rowIndex[0], rowSpan[0]));
            }
            return oCellRowSpans;
        }
        private List<FabricExecutionOrderSpecificationDetail> TwistedDetails(List<FabricExecutionOrderSpecificationDetail> oFabricExecutionOrderSpecificationDetails)
        {
            List<FabricExecutionOrderSpecificationDetail> oTwistedLDDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricExecutionOrderSpecificationDetail> oTWGLDDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricExecutionOrderSpecificationDetail> oLDDetails = new List<FabricExecutionOrderSpecificationDetail>();
            List<FabricExecutionOrderSpecificationDetail> oTempLDDetails = new List<FabricExecutionOrderSpecificationDetail>();

            oTWGLDDetails = oFabricExecutionOrderSpecificationDetails.Where(x => x.TwistedGroup > 0).ToList();
            oLDDetails = oFabricExecutionOrderSpecificationDetails.Where(x => x.TwistedGroup == 0).ToList();

            while (oFabricExecutionOrderSpecificationDetails.Count() > 0)
            {
                if (oTWGLDDetails.FirstOrDefault() != null && oFabricExecutionOrderSpecificationDetails.FirstOrDefault().FEOSDID == oTWGLDDetails.FirstOrDefault().FEOSDID)
                {
                    oTempLDDetails = oTWGLDDetails.Where(x => x.TwistedGroup == oTWGLDDetails.FirstOrDefault().TwistedGroup).ToList();
                    oFabricExecutionOrderSpecificationDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);
                    oTWGLDDetails.RemoveAll(x => x.TwistedGroup == oTempLDDetails.FirstOrDefault().TwistedGroup);

                }
                else if (oLDDetails.FirstOrDefault() != null && oFabricExecutionOrderSpecificationDetails.FirstOrDefault().FEOSDID == oLDDetails.FirstOrDefault().FEOSDID)
                {
                    oTempLDDetails = oLDDetails.Where(x => x.FEOSDID == oLDDetails.FirstOrDefault().FEOSDID).ToList();

                    oFabricExecutionOrderSpecificationDetails.RemoveAll(x => x.FEOSDID == oTempLDDetails.FirstOrDefault().FEOSDID);
                    oLDDetails.RemoveAll(x => x.FEOSDID == oTempLDDetails.FirstOrDefault().FEOSDID);
                }
                oTwistedLDDetails.AddRange(oTempLDDetails);
            }
            return oTwistedLDDetails;
        }
        #endregion
        private string MakeSQL(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification)
        {
            string sTemp = oFabricExecutionOrderSpecification.ErrorMessage;
            string sReturn1 = "";
            string sReturn = "";
            if (!string.IsNullOrEmpty(sTemp))
            {
                sReturn1 = "SELECT * FROM View_FabricExecutionOrderSpecification";

                if (!string.IsNullOrEmpty(oFabricExecutionOrderSpecification.ExeNo))
                {
                    sReturn1 = "SELECT top(10)* FROM View_FabricExecutionOrderSpecification";
                }

                #region Set Values
                int nCboIssueDate = Convert.ToInt32(sTemp.Split('~')[0]);
                DateTime dFromIssueDate = DateTime.Now;
                DateTime dToIssueDate = DateTime.Now;

                if (nCboIssueDate > 0)
                {
                    dFromIssueDate = Convert.ToDateTime(sTemp.Split('~')[1]);
                    dToIssueDate = Convert.ToDateTime(sTemp.Split('~')[2]);
                }

                int nIsOutSide = Convert.ToInt32(sTemp.Split('~')[3]);
                int nProdtionType = Convert.ToInt32(sTemp.Split('~')[4]);
                int nIsYD = Convert.ToInt32(sTemp.Split('~')[5]);
                int nFSpcType = Convert.ToInt32(sTemp.Split('~')[6]);
                int nApproveBy = Convert.ToInt32(sTemp.Split('~')[7]);
                int nForwardToDO = Convert.ToInt32(sTemp.Split('~')[8]);

                #endregion

                #region Make Query

                #region SCNoFull
                if (!string.IsNullOrEmpty(oFabricExecutionOrderSpecification.SCNoFull))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "SCNoFull LIKE '%" + oFabricExecutionOrderSpecification.SCNoFull + "%' ";
                }
                #endregion

                #region FabricNo
                if (!string.IsNullOrEmpty(oFabricExecutionOrderSpecification.FabricNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FabricNo LIKE '%" + oFabricExecutionOrderSpecification.FabricNo + "%' ";
                }
                #endregion

                #region ExeNo
                if (!string.IsNullOrEmpty(oFabricExecutionOrderSpecification.ExeNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ExeNo LIKE '%" + oFabricExecutionOrderSpecification.ExeNo + "%' ";
                }
                #endregion

                #region Issue Date
                if (nCboIssueDate != (int)EnumCompareOperator.None)
                {
                    Global.TagSQL(ref sReturn);
                    if (nCboIssueDate == (int)EnumCompareOperator.EqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboIssueDate == (int)EnumCompareOperator.NotEqualTo)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboIssueDate == (int)EnumCompareOperator.GreaterThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboIssueDate == (int)EnumCompareOperator.SmallerThan)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboIssueDate == (int)EnumCompareOperator.Between)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106))  BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                    else if (nCboIssueDate == (int)EnumCompareOperator.NotBetween)
                    {
                        sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),IssueDate,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dFromIssueDate.ToString("dd MMM yyyy") + "',106))  AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dToIssueDate.ToString("dd MMM yyyy") + "',106)) ";
                    }
                }
                #endregion

                #region nIsOutSide
                if (nIsOutSide == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsOutSide = 1";
                }
                else if (nIsOutSide == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsOutSide = 0";
                }
                #endregion

                #region nProdtionType
                if (nProdtionType >0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ProdtionType = " + nProdtionType;
                }
             
                #endregion

                #region IsYD
                if (nIsYD == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsYD = 1";
                }
                else if (nIsYD == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "IsYD = 2";
                }
                #endregion

                #region nFSpcType
                if (nFSpcType > 0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "FSpcType = " + nFSpcType;
                }
                #endregion

                #region nApproveBy
                if (nApproveBy == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ApproveBy != 0";
                }
                else if (nApproveBy == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "ApproveBy = 0";
                }
                #endregion
                #region nForwardToDO
                if (nForwardToDO == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "isnull(ForwardToDOby,0) != 0";
                }
                else if (nForwardToDO == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "isnull(ForwardToDOby,0) = 0";
                }
                #endregion


                #endregion

            }
            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        [HttpPost]
        public JsonResult Search(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification)
        {
            string sSQL = MakeSQL(oFabricExecutionOrderSpecification);
            List<FabricExecutionOrderSpecification> oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
            oFabricExecutionOrderSpecifications = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            return Json(oFabricExecutionOrderSpecifications);
        }
        
        public ActionResult SetSessionSearchCriteria(FabricExecutionOrderSpecification oFabricExecutionOrderSpecification)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, oFabricExecutionOrderSpecification);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public void ExportExcel(double ts)
        {
            Company oCompany = new Company();
            FabricExecutionOrderSpecification oFabricExecutionOrderSpecification = new FabricExecutionOrderSpecification();
            try
            {
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oFabricExecutionOrderSpecification = (FabricExecutionOrderSpecification)Session[SessionInfo.ParamObj];
                if (oFabricExecutionOrderSpecification == null)
                {
                    throw new Exception("Invalid Searching Criteria!");
                }

                string sSQL = MakeSQL(oFabricExecutionOrderSpecification);
                List<FabricExecutionOrderSpecification> oFabricExecutionOrderSpecifications = new List<FabricExecutionOrderSpecification>();
                oFabricExecutionOrderSpecifications = FabricExecutionOrderSpecification.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);

                #region Excel Body
                int rowIndex = 2;
                int nMaxColumn = 0;
                int colIndex = 2;
                ExcelRange cell;
                Border border;
                ExcelFill fill;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Specification");
                    sheet.Name = "Fabric Specification";

                    #region Declare Column
                    colIndex = 1;
                    sheet.Column(++colIndex).Width = 8;  //SL
                    sheet.Column(++colIndex).Width = 20; //PO No
                    sheet.Column(++colIndex).Width = 20; //Mkt Ref
                    sheet.Column(++colIndex).Width = 20; //Dispo No
                    sheet.Column(++colIndex).Width = 15; //Type
                    sheet.Column(++colIndex).Width = 15; //Qty(Y)
                    sheet.Column(++colIndex).Width = 15; //Qty(M)
                    sheet.Column(++colIndex).Width = 30; //Construction
                    sheet.Column(++colIndex).Width = 30; //Composition
                    sheet.Column(++colIndex).Width = 15; //Date
                    sheet.Column(++colIndex).Width = 20; //ApprovedBy
                    sheet.Column(++colIndex).Width = 15; //Is ForwardToDye
                    nMaxColumn = colIndex;
                    #endregion

                    #region Report Header
                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;

                    cell = sheet.Cells[rowIndex, 2, rowIndex, nMaxColumn]; cell.Merge = true; cell.Style.Font.Bold = true; cell.Style.Font.Size = 15;
                    cell.Value = "Fabric Specification"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex++;
                    #endregion

                    #region Column Header
                    colIndex = 2;
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "SL"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "PO No"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Mkt Ref"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Dispo No"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Type"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Qty(Y)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Qty(M)"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Construction"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Composition"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;


                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "Date"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "ApprovedBy"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, colIndex]; cell.Value = "ForwardToDye"; cell.Style.Font.Bold = false;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.WhiteSmoke);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;
                    #endregion

                    #region Report Body
                    int nCount = 0;
                    string sStartCell = "", sEndCell = "";
                    int nStartRow = 0, nEndRow = 0, nStartCol = 0;
                    string sDataColumn = "", sNumberFormat = ""; double nOrderQty = 0;
                    nStartRow = rowIndex;
                    foreach (FabricExecutionOrderSpecification oItem in oFabricExecutionOrderSpecifications)
                    {
                        //SL 
                        colIndex = 2;
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = ++nCount; cell.Style.Numberformat.Format = "##,###;(##,###)"; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //PO No
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.SCNoFull; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Mkt Ref
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.FabricNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Dispo No
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ExeNo; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Type
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ProdtionTypeSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Qty(Y)
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.Qty; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Qty(M)
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.OrderQtyInMeter; cell.Style.Font.Bold = false; cell.Style.Numberformat.Format = "#,##,##0.00;(#,##,##0.00)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Construction
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.Construction; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //Composition
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.Composition; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //IssueDate
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.IssueDateSt; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //ApproveBy
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.ApproveByName; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        colIndex = colIndex + 1;

                        //ForwardToDye
                        cell = sheet.Cells[rowIndex, colIndex]; cell.Value = oItem.FortoDO; cell.Style.Font.Bold = false;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top; border = cell.Style.Border;
                        border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                        nEndRow = rowIndex;
                        rowIndex++;
                    }
                    #endregion

                    #region Grand Total
                    cell = sheet.Cells[rowIndex, 2, rowIndex, 6]; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Value = "Grand Total"; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;

                    colIndex = 7;
                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    
                    colIndex = 8;
                    sStartCell = Global.GetExcelCellName(nStartRow, colIndex);
                    sEndCell = Global.GetExcelCellName(nEndRow, colIndex);
                    cell = sheet.Cells[rowIndex, colIndex]; cell.Style.Font.Bold = false; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")";
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center; cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    colIndex = colIndex + 1;

                    cell = sheet.Cells[rowIndex, 9, rowIndex, 13]; cell.Merge = true; cell.Style.Font.Bold = true;
                    cell.Value = ""; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Medium;
                    rowIndex++;
                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricSpecification.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
                
            }
            catch (Exception ex)
            {
                #region Errormessage
                ExcelRange cell;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Fabric Specification");
                    sheet.Name = "Fabric Specification";

                    cell = sheet.Cells[2, 2, 2, 5]; cell.Merge = true; cell.Value = ex.Message; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=FabricSpecification.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
        }



        #region Receive Yarn
        public ActionResult View_FEOReceiveYarn(int nFSCDID)
        {
            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
            FabricSalesContractDetail oFSCD = new FabricSalesContractDetail();
            oFSCD = oFSCD.Get(nFSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            if (oFSCD.FabricSalesContractDetailID > 0)
            {
                string sSQL = "Select * from View_FabricExecutionOrderYarnReceive Where FSCDID=" + oFSCD.FabricSalesContractDetailID + "";
                oFEOYRs = FabricExecutionOrderYarnReceive.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            List<WorkingUnit> oWUs = new List<WorkingUnit>();
            oWUs = WorkingUnit.Gets( ((User)Session[SessionInfo.CurrentUser]).UserID);
            ViewBag.WUs = oWUs;
            ViewBag.FEOYR = oFEOYRs;
            return View(oFSCD);
        }
        [HttpPost]
        public ActionResult GetAFEODetails(FabricSalesContractDetail oFabricSalesContractDetail)
        {
            _oFEODetails = new List<FabricSalesContractDetail>();
            try
            {
                _oFEODetails = FabricSalesContractDetail.Gets(oFabricSalesContractDetail.FabricSalesContractDetailID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oFEODetail = new FabricSalesContractDetail();
                _oFEODetail.ErrorMessage = ex.Message;
                _oFEODetails.Add(_oFEODetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oFEODetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult DeleteFEOYR(FabricExecutionOrderYarnReceive oFEOYR)
        {
            string sFeedBackMessage = "";
            try
            {
                sFeedBackMessage = oFEOYR.Delete(oFEOYR.FEOYID, oFEOYR.FSCDID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public JsonResult FEOYarnReceive(FabricExecutionOrderYarnReceive oFEOYR)
        //{
        //    try
        //    {
        //        if (oFEOYR.FEOYRs.Count() <= 0)
        //        {
        //            if (oFEOYR.LotID <= 0 && oFEOYR.ChallanDetailID > 0) { throw new Exception("No lot found."); }
        //            if (oFEOYR.FEOYID > 0) { throw new Exception("Already received."); }
        //            oFEOYR = oFEOYR.Receive(((User)Session[SessionInfo.CurrentUser]).UserID);
        //        }
        //        else
        //        {
        //            List<FabricExecutionOrderYarnReceive> oFEOYRs = new List<FabricExecutionOrderYarnReceive>();
        //            oFEOYRs = oFEOYR.Receives(((User)Session[SessionInfo.CurrentUser]).UserID);
        //            oFEOYR.FEOYRs = oFEOYRs;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oFEOYR = new FabricExecutionOrderYarnReceive();
        //        oFEOYR.ErrorMessage = ex.Message;
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oFEOYR);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

      


        #endregion 

      
    }

}
