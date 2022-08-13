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
	public class LabdipChallanController : Controller
	{
		#region Declaration

		LabdipChallan _oLabdipChallan = new LabdipChallan();
		List<LabdipChallan> _oLabdipChallans = new  List<LabdipChallan>();
        List<LabDipDetail> _oLabDipDetails = new List<LabDipDetail>();
		#endregion

		#region Functions

		#endregion

		#region Actions
		public ActionResult ViewLabdipChallans(int buid,int menuid)
		{
			this.Session.Remove(SessionInfo.MenuID);
			this.Session.Add(SessionInfo.MenuID, menuid);
			_oLabdipChallans = new List<LabdipChallan>();
            _oLabdipChallans = LabdipChallan.Gets("SELECT * FROM View_LabDipChallan WHERE ISNULL([Status],0)!=2", (int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            ViewBag.CompareOperators = EnumObject.jGets(typeof(EnumCompareOperator));
			return View(_oLabdipChallans);
		}

		public ActionResult ViewLabdipChallan(int id)
		{
			_oLabdipChallan = new LabdipChallan();
            if (id > 0)
            {
                _oLabdipChallan = _oLabdipChallan.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oLabdipChallan.LabDipDetails = LabDipDetail.Gets("SELECT * FROM View_LabDipDetail WHERE labdipChallanID=" + _oLabdipChallan.LabdipChallanID, (int)Session[SessionInfo.currentUserID]);
            }
            else
                _oLabdipChallan.PrepareBy = ((User)Session[SessionInfo.CurrentUser]).UserName;

			return View(_oLabdipChallan);
		}

		[HttpPost]
		public JsonResult Save(LabdipChallan oLabdipChallan)
		{
			_oLabdipChallan = new LabdipChallan();
			try
			{
				_oLabdipChallan = oLabdipChallan;
				_oLabdipChallan = _oLabdipChallan.Save((int)Session[SessionInfo.currentUserID]);
			}
			catch (Exception ex)
			{
				_oLabdipChallan = new LabdipChallan();
				_oLabdipChallan.ErrorMessage = ex.Message;
			}
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			string sjson = serializer.Serialize(_oLabdipChallan);
			return Json(sjson, JsonRequestBehavior.AllowGet);
		}
        [HttpPost]
        public JsonResult CreateChallan(LabDip oLabDip)
        {
            LabDip _oLabDip = new LabDip();
            try
            {
                _oLabDip = LabDip.Get(oLabDip.LabDipID, (int)Session[SessionInfo.currentUserID]);

                _oLabdipChallan = new LabdipChallan() 
                {
                    ContractorID=_oLabDip.ContractorID,
                    ChallanDate=DateTime.Now,
                    DeliveryZoneID=oLabDip.DeliveryZoneID,
                    Remarks="",
                    LabDipDetails=LabDipDetail.Gets("SELECT * FROM LabdipDetail WHERE ISNULL(LabDipChallanID,0)=0 AND LabDipID="+oLabDip.LabDipID,(int)Session[SessionInfo.currentUserID])
                };

                if (_oLabdipChallan.LabDipDetails.Count == 0)
                    throw new Exception("Lab Dip Challan Is Already Created!!");

                _oLabdipChallan = _oLabdipChallan.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLabdipChallan = new LabdipChallan();
                _oLabdipChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabdipChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateStatus(LabdipChallan oLabdipChallan)
        {
            _oLabdipChallan = new LabdipChallan();
            try
            {
                _oLabdipChallan = oLabdipChallan;
                _oLabdipChallan = _oLabdipChallan.UpdateStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oLabdipChallan = new LabdipChallan();
                _oLabdipChallan.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabdipChallan);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(LabdipChallan oLabdipChallan)
		{
			string sFeedBackMessage = "";
			try
			{
				LabdipChallan _oLabdipChallan = new LabdipChallan();
                sFeedBackMessage = _oLabdipChallan.Delete(oLabdipChallan.LabdipChallanID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult RemoveDetail(LabDipDetail oLabDipDetail)
        {
            string sFeedBackMessage = "";
            try
            {
                LabdipChallan _oLabdipChallan = new LabdipChallan();
                sFeedBackMessage = _oLabdipChallan.RemoveDetail(oLabDipDetail.LabDipDetailID, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult GetLDDetails(LabDipDetail oLabDipDetail)
        {
            _oLabDipDetails = new List<LabDipDetail>();
            try
            {
                string sReturn1 = "SELECT * FROM View_LabDipDetail";
                string sReturn  = " WHERE ISNULL(LabDipChallanID,0) = 0 ";

                #region  ContractorID
                if (oLabDipDetail.ContractorID>0)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabDipID IN ( SELECT LabDipID FROM LabDip WHERE ContractorID IN (" + oLabDipDetail.ContractorID + "))";
                }
                //else throw new Exception("Please Select a Contractor!");
                #endregion

                #region LabdipNo
                if (!string.IsNullOrEmpty(oLabDipDetail.LabdipNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " LabdipNo LIKE '%" + oLabDipDetail.LabdipNo + "%'";
                }
                #endregion

                #region ColorNo
                if (!string.IsNullOrEmpty(oLabDipDetail.ColorNo))
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ISNULL(LDNo,'')+ISNULL(ColorNo,'')  LIKE '%" + oLabDipDetail.ColorNo + "%'";
                }
                #endregion

                string sSQL = sReturn1 + " " + sReturn + " Order By LabdipNo ";
                _oLabDipDetails = LabDipDetail.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
                if (_oLabDipDetails.Count <= 0) { throw new Exception("No information found."); }

            }
            catch (Exception ex)
            {
                oLabDipDetail = new LabDipDetail();
                oLabDipDetail.ErrorMessage = ex.Message;
                _oLabDipDetails.Add(oLabDipDetail);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oLabDipDetails);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
		#endregion

        #region ADV SEARCH
    
        [HttpPost]
        public JsonResult AdvSearch(LabDipDetail oLabDipDetail)
        {
            List<LabdipChallan> oLabdipChallans = new List<LabdipChallan>();
            LabdipChallan _oLabdipChallan = new LabdipChallan();
            string sSQL = MakeSQL(oLabDipDetail);
            if (sSQL == "Error")
            {
                _oLabdipChallan = new LabdipChallan();
                _oLabdipChallan.ErrorMessage = "Please select a searching critaria.";
                oLabdipChallans = new List<LabdipChallan>();
            }
            else
            {
                oLabdipChallans = new List<LabdipChallan>();
                oLabdipChallans = LabdipChallan.Gets(sSQL, ((User)Session[SessionInfo.CurrentUser]).UserID);
                if (oLabdipChallans.Count == 0)
                {
                    oLabdipChallans = new List<LabdipChallan>();
                }
            }
            var jsonResult = Json(oLabdipChallans, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
        }
        private string MakeSQL(LabDipDetail oLabDipDetail)
        {
            string sParams = oLabDipDetail.Params;

            int nDateCriteria_Challan = 0;
            DateTime dStart_Challan = DateTime.Today,  dEnd_Challan = DateTime.Today;
            string sChallanNo = "",sContractorIDs = "";
            bool yetToHO = false, yetToBuyer = false;
            if (!String.IsNullOrEmpty(sParams))
            {
                int nCount = 0;
                sChallanNo = sParams.Split('~')[nCount++];
                nCount++; nCount++;
                nDateCriteria_Challan = Convert.ToInt32(sParams.Split('~')[nCount++]);
                dStart_Challan = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                dEnd_Challan = Convert.ToDateTime(sParams.Split('~')[nCount++]);
                sContractorIDs = sParams.Split('~')[nCount++];
                yetToHO = Convert.ToBoolean(sParams.Split('~')[nCount++]);
                yetToBuyer = Convert.ToBoolean(sParams.Split('~')[nCount++]);
            }

            string sReturn1 = "SELECT * FROM View_LabdipChallan AS EB";
            string sReturn = "";

            #region DATE SEARCH
            MakeSQLByDate(ref sReturn, "ChallanDate", nDateCriteria_Challan, dStart_Challan, dEnd_Challan);
            #endregion

            #region ChallanNo
            if (!string.IsNullOrEmpty(sChallanNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.ChallanNoFull LIKE '%" + sChallanNo + "%'";
            }
            #endregion

            #region LabdipNo
            if (!string.IsNullOrEmpty(oLabDipDetail.LabdipNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipChallanID IN ( SELECT LabdipChallanID FROM View_LabdipDetail WHERE LabdipNo LIKE '%" + oLabDipDetail.LabdipNo + "%')";
            }
            #endregion
            
            #region ColorNo
            if (!string.IsNullOrEmpty(oLabDipDetail.ColorNo))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " LabdipChallanID IN ( SELECT LabdipChallanID FROM View_LabdipDetail WHERE ISNULL(LDNo,'')+ISNULL(ColorNo,'') LIKE '%" + oLabDipDetail.ColorNo + "%')";

            }
            #endregion

            #region Contractor IDs
            if (!string.IsNullOrEmpty(sContractorIDs))
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ContractorID IN ("+sContractorIDs+") ";
            }
            #endregion

            #region YetToHO
            if (yetToHO)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.[Status] ="+ 0;
            }
            #endregion

            #region yetToBuyer
            if (yetToBuyer)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " EB.[Status] =" + 1;
            }
            #endregion

            sReturn = sReturn1 + sReturn;
            return sReturn;
        }
        private void MakeSQLByDate(ref string sReturn, string sSearchDate, int nDateCriteria, DateTime dStartDate, DateTime dEndDate)
        {
            #region DATE
            if (nDateCriteria > 0)
            {
                Global.TagSQL(ref sReturn);
                if (nDateCriteria == (int)EnumCompareOperator.EqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + "= '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotEqualTo)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " != '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.GreaterThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " > '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.SmallerThan)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " < '" + dStartDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.Between)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
                else if (nDateCriteria == (int)EnumCompareOperator.NotBetween)
                {
                    sReturn = sReturn + " EB." + sSearchDate + " NOT BETWEEN '" + dStartDate.ToString("dd MMM yyy") + "' AND '" + dEndDate.ToString("dd MMM yyy") + "' ";
                }
            }
            #endregion
        }
        #endregion

        #region PDF PRINT
        public ActionResult PrintLabDipChallan(int nLDCID)
        {
            List<LabDip> oLabDips = new List<LabDip>();
            Contractor oContractor = new Contractor();
            try
            {
                if (nLDCID > 0)
                {
                    _oLabdipChallan = _oLabdipChallan.Get(nLDCID, (int)Session[SessionInfo.currentUserID]);
                    _oLabdipChallan.LabDipDetails = LabDipDetail.Gets("SELECT * FROM View_LabDipDetail WHERE labdipChallanID=" + _oLabdipChallan.LabdipChallanID, (int)Session[SessionInfo.currentUserID]);

                    oLabDips = LabDip.Gets("SELECT * FROM View_Labdip WHERE LabdipID IN (SELECT LabdipID FROM View_LabdipDetail WHERE LabdipChallanID=" + _oLabdipChallan.LabdipChallanID + ")", ((User)Session[SessionInfo.CurrentUser]).UserID);
                    if (_oLabdipChallan.LabDipDetails.Count > 0)
                    {
                        //sLDDetailIDs = string.Join(",", _oLabdipChallan.LabDipDetails.Select(x => x.LabDipDetailID).Distinct().ToList());
                        //if (!string.IsNullOrEmpty(sLDDetailIDs))
                        //{
                        //    _oDeliveryZones = DeliveryZone.Gets("Select * from Deliveryzone where DeliveryZoneID in (Select DeliveryZoneID from Labdip where LabdipID in (Select Distinct(LabdipID) from LabdipDetail where LabdipDetailID in (" + sLDDetailIDs + ")))", ((User)Session[SessionInfo.CurrentUser]).UserID);
                        //}
                        if (oLabDips.Count > 0)
                        {
                            _oLabdipChallan.DeliveryZoneName = oLabDips[0].DeliveryNote;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, ((User)Session[SessionInfo.CurrentUser]).UserID);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);

            string[] nLabDipInfo={"","",""} ;
            if (oLabDips.Any()) 
            {
                nLabDipInfo = new string[] { 
                    string.Join(", ", oLabDips.Select(x=>x.LabdipNo)),
                    string.Join(", ", oLabDips.Select(x=>x.MktPerson)),
                    string.Join(", ", oLabDips.Select(x=>x.ContractorCPName))
                };
            }



            rptLabdipChallan oReport = new rptLabdipChallan();
            byte[] abytes = oReport.PrepareReport(_oLabdipChallan, oCompany, oBusinessUnit, nLabDipInfo);
            return File(abytes, "application/pdf");

        }
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
        #endregion
    }

}
