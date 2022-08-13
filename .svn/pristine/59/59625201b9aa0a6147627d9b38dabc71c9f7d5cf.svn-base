using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSolFinancial.Controllers;
using ESimSol.BusinessObjects;
using ESimSol.BusinessObjects.ReportingObject;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.Reports;
using iTextSharp.text.pdf;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.Hosting;
using System.Xml.Serialization;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;

namespace ESimSolFinancial.Controllers
{
	public class PackingListController : Controller
	{
		#region Declaration
		PackingList _oPackingList = new PackingList();
		List<PackingList> _oPackingLists = new  List<PackingList>();
        ColorSizeRatio _oColorSizeRatio = new ColorSizeRatio();
        List<TechnicalSheetSize> _oTechnicalSheetSizes = new List<TechnicalSheetSize>();
		#endregion

		#region Functions
        private List<PackingListDetail> MapPackingListDetailFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, PackingList oPackingList)
        {
            List<PackingListDetail> oPackingListDetails = new List<PackingListDetail>();
            PackingListDetail oPackingListDetail = new PackingListDetail();
            PackingListDetail oTempPackingListDetail = new PackingListDetail();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempPackingListDetail = new PackingListDetail();
                    oTempPackingListDetail = GetObjIDAndQty(nCount, oItem);
                    if (oTempPackingListDetail.Qty > 0)
                    {
                        oPackingListDetail = new PackingListDetail();
                        oPackingListDetail.PackingListDetailID = oTempPackingListDetail.PackingListDetailID;
                        oPackingListDetail.PackingListID = oPackingList.PackingListID;
                        oPackingListDetail.ColorID = oItem.ColorID;
                        oPackingListDetail.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oPackingListDetail.Qty = oTempPackingListDetail.Qty;
                        oPackingListDetails.Add(oPackingListDetail);
                    }
                }
            }
            return oPackingListDetails;
        }
        private PackingListDetail GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            PackingListDetail oPackingListDetail = new PackingListDetail();
            switch (nCount)
            {
                case 1:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID1;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID2;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID3;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID4;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID5;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID6;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID7;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID8;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID9;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID10;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID11;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID12;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID13;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID14;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID15;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID16;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID17;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID18;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID19;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oPackingListDetail.PackingListDetailID = oColorSizeRatio.OrderObjectID20;
                    oPackingListDetail.Qty = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oPackingListDetail;
        }
        private List<ColorCategory> GetDistictColors(List<OrderRecapDetail> oOrderRecapDetails)
        {
            List<ColorCategory> oColorCategories = new List<ColorCategory>();
            ColorCategory oColorCategory = new ColorCategory();
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (!IsExist(oColorCategories, oItem))
                {
                    oColorCategory = new ColorCategory();
                    oColorCategory.ColorCategoryID = oItem.ColorID;
                    oColorCategory.ColorName = oItem.ColorName;
                    oColorCategories.Add(oColorCategory);
                }
            }
            return oColorCategories;
        }
        private bool IsExist(List<ColorCategory> oColorCategories, OrderRecapDetail oOrderRecapDetail)
        {
            foreach (ColorCategory oITem in oColorCategories)
            {
                if (oITem.ColorCategoryID == oOrderRecapDetail.ColorID)
                {
                    return true;
                }
            }
            return false;
        }

        private List<ColorSizeRatio> MapColorSizeRationFromPackingListDetail(List<PackingListDetail> oPackingListDetails, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (oItem.ColorID != nColorID)
                {
                    oColorSizeRatio = new ColorSizeRatio();
                    oColorSizeRatio.ColorID = oItem.ColorID;
                    oColorSizeRatio.ColorName = oItem.ColorName;
                    nCount = 0;
                    foreach (TechnicalSheetSize oSize in oSizes)
                    {
                        nCount++;
                        #region Set OrderQty
                        sPropertyName = "OrderQty" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oPackingListDetails), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oPackingListDetails), null);
                        }
                        #endregion
                    }

                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oPackingListDetails), null);
                    }
                    #endregion

                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }


        private double GetColorWiseTotalQty(int nColorID, List<PackingListDetail> oPackingListDetails)
        {
            double nTotalQty = 0;
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Qty;
                }
            }
            return nTotalQty;
        }

        private double GetQty(int nSizeID, int nColorID, List<PackingListDetail> oPackingListDetails)
        {
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Qty;
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<PackingListDetail> oPackingListDetails)
        {
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.PackingListDetailID;
                }
            }
            return 0;
        }

        private List<ColorCategory> GetDistictColors(List<PackingListDetail> oPackingListDetails)
        {
            List<ColorCategory> oColorCategories = new List<ColorCategory>();
            ColorCategory oColorCategory = new ColorCategory();
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (!IsExistPackColor(oColorCategories, oItem))
                {
                    oColorCategory = new ColorCategory();
                    oColorCategory.ColorCategoryID = oItem.ColorID;
                    oColorCategory.ColorName = oItem.ColorName;
                    oColorCategories.Add(oColorCategory);
                }
            }
            return oColorCategories;
        }
        private bool IsExistPackColor(List<ColorCategory> oColorCategories, PackingListDetail oPackingListDetail)
        {
            foreach (ColorCategory oITem in oColorCategories)
            {
                if (oITem.ColorCategoryID == oPackingListDetail.ColorID)
                {
                    return true;
                }
            }
            return false;
        }

        private List<SizeCategory> GetDistictSizes(List<PackingListDetail> oPackingListDetails)
        {
            List<SizeCategory> oSizeCategories = new List<SizeCategory>();
            SizeCategory oSizeCategory = new SizeCategory();
            foreach (PackingListDetail oItem in oPackingListDetails)
            {
                if (!IsExistPackSize(oSizeCategories, oItem))
                {
                    oSizeCategory = new SizeCategory();
                    oSizeCategory.SizeCategoryID = oItem.SizeID;
                    oSizeCategory.SizeCategoryName = oItem.SizeName;
                    oSizeCategories.Add(oSizeCategory);
                }
            }
            return oSizeCategories;
        }
        private bool IsExistPackSize(List<SizeCategory> oSizeCategories, PackingListDetail oPackingListDetail)
        {
            foreach (SizeCategory oITem in oSizeCategories)
            {
                if (oITem.SizeCategoryID == oPackingListDetail.SizeID)
                {
                    return true;
                }
            }
            return false;
        }

		#endregion

		#region Actions

		public ActionResult ViewPackingLists(int CIID)//commercial Invoice id
		{
			this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
			this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.PackingList).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
			_oPackingLists = new List<PackingList>();
            string sSQL = "SELECT * FROM View_PackingList WHERE CIDID IN (SELECT CommercialInvoiceDetailID FROM CommercialInvoiceDetail WHERE CommercialInvoiceID = "+CIID+")";
			_oPackingLists = PackingList.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
			return View(_oPackingLists);
		}

		public ActionResult ViewPackingList(int id)
		{
			_oPackingList = new PackingList();
			if (id > 0)
			{
				_oPackingList = _oPackingList.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oPackingList.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oPackingList.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oPackingList.PackingListDetails = PackingListDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oPackingList.ColorSizeRatios = MapColorSizeRationFromPackingListDetail(_oPackingList.PackingListDetails, _oPackingList.TechnicalSheetSizes);

			}
			return View(_oPackingList);
		}


		#endregion

        #region Json Gets
        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                PackingList oPackingList = new PackingList();
                sFeedBackMessage = oPackingList.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sFeedBackMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sFeedBackMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region GEt Color Size Ratio
        [HttpGet]
        public JsonResult GetColorSizeRatio(int nOrderRecapID)//Get Color Size Ratio
        {
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();

            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            try
            {
                oOrderRecapDetails = OrderRecapDetail.Gets(nOrderRecapID, (int)Session[SessionInfo.currentUserID]);
                oColorCategorys = GetDistictColors(oOrderRecapDetails);
                foreach (ColorCategory oItem in oColorCategorys)
                {
                    _oColorSizeRatio = new ColorSizeRatio();
                    _oColorSizeRatio.ColorID = oItem.ColorCategoryID;
                    _oColorSizeRatio.ColorName = oItem.ColorName;
                    oColorSizeRatios.Add(_oColorSizeRatio);
                }

            }
            catch (Exception ex)
            {
                ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
                oColorSizeRatio.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColorSizeRatios);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Json Posts

        [HttpPost]
        public JsonResult Save(PackingList oPackingList)
        {
            _oPackingList = new PackingList();

            try
            {
                _oPackingList = oPackingList;
                _oTechnicalSheetSizes = TechnicalSheetSize.Gets(_oPackingList.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oPackingList.PackingListDetails = MapPackingListDetailFromColorSizeRation(_oPackingList.ColorSizeRatios, _oTechnicalSheetSizes, _oPackingList);
                _oPackingList = _oPackingList.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oPackingList = new PackingList();
                _oPackingList.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oPackingList);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        } 

		
        #endregion

        #region Print 
        public Image GetCompanyLogo(Company oCompany)
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
         public ActionResult PackingListPreview(int id)
        {
           _oPackingList = new PackingList();
           Contractor oFactory = new Contractor();
           ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
           _oPackingList = _oPackingList.Get(id, (int)Session[SessionInfo.currentUserID]);
           _oPackingList.PackingListDetails = PackingListDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
           _oPackingList.SizeCategories = GetDistictSizes(_oPackingList.PackingListDetails);
           _oPackingList.ColorCategories = GetDistictColors(_oPackingList.PackingListDetails);
           oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.ChairmanName, (int)Session[SessionInfo.currentUserID]);//Get Client Operation Setting
           _oPackingList.ErrorMessage = oClientOperationSetting.Value;//set commer cial manger name
           Company oCompany = new Company();
           oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
           oCompany.CompanyLogo = GetCompanyLogo(oCompany);
           _oPackingList.Company = oCompany;
           oFactory = oFactory.Get(_oPackingList.ProductionFactoryID, (int)Session[SessionInfo.currentUserID]);
           rptPackingList oReport = new rptPackingList();
           byte[] abytes = oReport.PrepareReport(_oPackingList, oFactory);
           return File(abytes, "application/pdf");
        }
        #endregion
    }

}
