using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ESimSol.BusinessObjects;
using ICS.Core.Framework;
using System.Web.Script.Serialization;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using System.Reflection;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using iTextSharp;
using ESimSol.Reports;
using ESimSolFinancial.Controllers;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Drawing;
using System.Text;

namespace ESimSolIBS.Controllers
{
    public class OrderRecapController : Controller
    {
        #region DECLARATION
        OrderRecap _oOrderRecap = new OrderRecap();
        List<OrderRecap> _oOrderRecaps = new List<OrderRecap>();
        OrderRecapDetail _oOrderRecapDetail = new OrderRecapDetail();
        List<OrderRecapDetail> _oOrderRecapDetails = new List<OrderRecapDetail>();
        TechnicalSheet _oTechnicalSheet = new TechnicalSheet();
        List<TechnicalSheet> _oTechnicalSheets = new List<TechnicalSheet>();

        GarmentsClass _oGarmentsClass = new GarmentsClass();
        List<GarmentsClass> _oGarmentsClasses = new List<GarmentsClass>();
        OrderRecapSummery _oOrderRecapSummery = new OrderRecapSummery();
        List<OrderRecapSummery> _oOrderRecapSummerys = new List<OrderRecapSummery>();
        List<OrderRecapYarn> _oOrderRecapYarns = new List<OrderRecapYarn>();
        ApprovalRequest _oApprovalRequest = new ApprovalRequest();
        OrderRecapMgtReport _oOrderRecapMgtReport = new OrderRecapMgtReport();
        List<OrderRecapMgtReport> _oOrderRecapMgtReports = new List<OrderRecapMgtReport>();
        List<ORAssortment> _oORAssortments = new List<ORAssortment>();
        List<ORBarCode> _oORBarCodes = new List<ORBarCode>();
        ORAssortment _oORAssortment = new ORAssortment();
        ORBarCode _oORBarCode = new ORBarCode();
        SampleRequirement _oSampleRequirement = new SampleRequirement();
        List<RecapBillOfMaterial> _oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
        RecapBillOfMaterial _oRecapBillOfMaterial = new RecapBillOfMaterial();
        ReviseRequest _oReviseRequest = new ReviseRequest();
        OrderRecapComment _oOrderRecapComment = new OrderRecapComment();
        List<OrderRecapComment> _oOrderRecapComments = new List<OrderRecapComment>();
        #endregion

        #region Function
        #region MapColorSizeRationFromBarCode
        private List<ColorSizeRatio> MapColorSizeRationFromBarCode(List<ORBarCode> oORBarCodes, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (ORBarCode oItem in oORBarCodes)
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
                        #region Set BarCode
                        sPropertyName = "BarCode" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetBarCode(oSize.SizeCategoryID, oItem.ColorID, oORBarCodes), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oORBarCodes), null);
                        }
                        #endregion
                    }
                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }
        private string GetBarCode(int nSizeID, int nColorID, List<ORBarCode> oORBarCodes)
        {
            foreach (ORBarCode oItem in oORBarCodes)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.BarCode;
                }
            }
            return "";
        }
        private int GetObjectID(int nSizeID, int nColorID, List<ORBarCode> oORBarCodes)
        {
            foreach (ORBarCode oItem in oORBarCodes)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.ORBarCodeID;
                }
            }
            return 0;
        }

        private List<ORBarCode> MapORBarCodeFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, OrderRecap oOrderRecap)
        {
            List<ORBarCode> oORBarCodes = new List<ORBarCode>();
            ORBarCode oORBarCode = new ORBarCode();
            ORBarCode oTempORBarCode = new ORBarCode();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempORBarCode = new ORBarCode();
                    oTempORBarCode = GetObjIDAndBarCodeforBarCode(nCount, oItem);
                    if (oTempORBarCode.BarCode != "")
                    {
                        oORBarCode = new ORBarCode();
                        oORBarCode.ORBarCodeID = oTempORBarCode.ORBarCodeID;
                        oORBarCode.OrderRecapID = oOrderRecap.OrderRecapID;
                        oORBarCode.ColorID = oItem.ColorID;
                        oORBarCode.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oORBarCode.BarCode = oTempORBarCode.BarCode;
                        oORBarCodes.Add(oORBarCode);
                    }
                }
            }
            return oORBarCodes;
        }

        private ORBarCode GetObjIDAndBarCodeforBarCode(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            ORBarCode oORBarCode = new ORBarCode();
            switch (nCount)
            {
                case 1:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID1;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode1;
                    break;
                case 2:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID2;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode2;
                    break;
                case 3:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID3;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode3;
                    break;
                case 4:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID4;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode4;
                    break;
                case 5:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID5;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode5;
                    break;
                case 6:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID6;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode6;
                    break;
                case 7:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID7;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode7;
                    break;
                case 8:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID8;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode8;
                    break;
                case 9:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID9;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode9;
                    break;
                case 10:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID10;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode10;
                    break;
                case 11:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID11;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode11;
                    break;
                case 12:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID12;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode12;
                    break;
                case 13:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID13;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode13;
                    break;
                case 14:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID14;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode14;
                    break;
                case 15:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID15;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode15;
                    break;
                case 16:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID16;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode16;
                    break;
                case 17:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID17;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode17;
                    break;
                case 18:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID18;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode18;
                    break;
                case 19:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID19;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode19;
                    break;
                case 20:
                    oORBarCode.ORBarCodeID = oColorSizeRatio.OrderObjectID20;
                    oORBarCode.BarCode = oColorSizeRatio.BarCode20;
                    break;
            }
            return oORBarCode;
        }
        #endregion

        #region MapColorSizeRationFromAssortment
        private List<ColorSizeRatio> MapColorSizeRationFromAssortment(List<ORAssortment> oORAssortments, List<TechnicalSheetSize> oSizes, List<OrderRecapDetail> oOrderRecapDetails)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (ORAssortment oItem in oORAssortments)
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
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oORAssortments), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oORAssortments), null);
                        }
                        #endregion
                    }

                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oORAssortments), null);
                    }
                    #endregion
                    double nTotalColorWiseRecapDetailQTy = GetColorWiseTotalQty(oItem.ColorID, oOrderRecapDetails);
                    if (nTotalColorWiseRecapDetailQTy > 0 & oColorSizeRatio.ColorWiseTotal > 0)
                    {
                        oColorSizeRatio.ColorWiseCartonQty = Math.Ceiling((nTotalColorWiseRecapDetailQTy / oColorSizeRatio.ColorWiseTotal));
                    }

                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }
        private double GetQty(int nSizeID, int nColorID, List<ORAssortment> oORAssortments)
        {
            foreach (ORAssortment oItem in oORAssortments)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Qty;
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<ORAssortment> oORAssortments)
        {
            foreach (ORAssortment oItem in oORAssortments)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.ORAssortmentID;
                }
            }
            return 0;
        }
        private double GetColorWiseTotalQty(int nColorID, List<ORAssortment> oORAssortments)
        {
            double nTotalQty = 0;
            foreach (ORAssortment oItem in oORAssortments)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Qty;
                }
            }
            return nTotalQty;
        }

        private List<ORAssortment> MapORAssortmentFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, OrderRecap oOrderRecap)
        {
            List<ORAssortment> oORAssortments = new List<ORAssortment>();
            ORAssortment oORAssortment = new ORAssortment();
            ORAssortment oTempORAssortment = new ORAssortment();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempORAssortment = new ORAssortment();
                    oTempORAssortment = GetObjIDAndQtyforAssortment(nCount, oItem);
                    if (oTempORAssortment.Qty > 0)
                    {
                        oORAssortment = new ORAssortment();
                        oORAssortment.ORAssortmentID = oTempORAssortment.ORAssortmentID;
                        oORAssortment.OrderRecapID = oOrderRecap.OrderRecapID;
                        oORAssortment.ColorID = oItem.ColorID;
                        oORAssortment.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oORAssortment.Qty = oTempORAssortment.Qty;
                        oORAssortments.Add(oORAssortment);
                    }
                }
            }
            return oORAssortments;
        }

        private ORAssortment GetObjIDAndQtyforAssortment(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            ORAssortment oORAssortment = new ORAssortment();
            switch (nCount)
            {
                case 1:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID1;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID2;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID3;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID4;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID5;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID6;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID7;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID8;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID9;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID10;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID11;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID12;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID13;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID14;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID15;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID16;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID17;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID18;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID19;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oORAssortment.ORAssortmentID = oColorSizeRatio.OrderObjectID20;
                    oORAssortment.Qty = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oORAssortment;
        }



        #endregion

        #region MapColorSizeRationFromOrderRecapDetail
        private List<ColorSizeRatio> MapColorSizeRationFromOrderRecapDetail(List<OrderRecapDetail> oOrderRecapDetails, List<TechnicalSheetSize> oSizes)
        {
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
            int nColorID = 0; int nCount = 0; string sPropertyName = "";
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID != nColorID)
                {
                    oColorSizeRatio = new ColorSizeRatio();
                    oColorSizeRatio.ColorID = oItem.ColorID;
                    oColorSizeRatio.ColorName = oItem.ColorName;
                    oColorSizeRatio.UnitPrice = oItem.UnitPrice;
                    nCount = 0;
                    foreach (TechnicalSheetSize oSize in oSizes)
                    {
                        nCount++;
                        #region Set OrderQty
                        sPropertyName = "OrderQty" + nCount.ToString();
                        PropertyInfo prop = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != prop && prop.CanWrite)
                        {
                            prop.SetValue(oColorSizeRatio, GetQty(oSize.SizeCategoryID, oItem.ColorID, oOrderRecapDetails), null);
                        }
                        #endregion

                        #region Set ObjectID
                        sPropertyName = "OrderObjectID" + nCount.ToString();
                        PropertyInfo propobj = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                        if (null != propobj && propobj.CanWrite)
                        {
                            propobj.SetValue(oColorSizeRatio, GetObjectID(oSize.SizeCategoryID, oItem.ColorID, oOrderRecapDetails), null);
                        }
                        #endregion
                    }

                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseTotal";
                    PropertyInfo propobjtotal = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotal && propobjtotal.CanWrite)
                    {
                        propobjtotal.SetValue(oColorSizeRatio, GetColorWiseTotalQty(oItem.ColorID, oOrderRecapDetails), null);
                    }
                    #endregion
                    #region ColorWiseTotal
                    sPropertyName = "ColorWiseAmount";
                    PropertyInfo propobjtotalAmount = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjtotalAmount && propobjtotalAmount.CanWrite)
                    {
                        propobjtotalAmount.SetValue(oColorSizeRatio, GetColorWiseTotalAmount(oItem.ColorID, oOrderRecapDetails), null);
                    }
                    #endregion

                    #region ColorWiseProductionQty
                    sPropertyName = "ColorWiseProductionQty";
                    PropertyInfo propobjColorWiseProductionQty = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjColorWiseProductionQty && propobjColorWiseProductionQty.CanWrite)
                    {
                        propobjColorWiseProductionQty.SetValue(oColorSizeRatio, GetColorWiseProductionQty(oItem.ColorID, oOrderRecapDetails), null);
                    }
                    #endregion

                    #region ColorWiseYetProductionQty
                    sPropertyName = "ColorWiseYetProductionQty";
                    PropertyInfo propobjColorWiseYetProductionQty = oColorSizeRatio.GetType().GetProperty(sPropertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (null != propobjColorWiseYetProductionQty && propobjColorWiseYetProductionQty.CanWrite)
                    {
                        propobjColorWiseYetProductionQty.SetValue(oColorSizeRatio, GetColorWiseYetToProductionQty(oItem.ColorID, oOrderRecapDetails), null);
                    }
                    #endregion

                    oColorSizeRatios.Add(oColorSizeRatio);
                }
                nColorID = oItem.ColorID;
            }
            return oColorSizeRatios;
        }

        private double GetColorWiseTotalQty(int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            double nTotalQty = 0;
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalQty = nTotalQty + oItem.Quantity;
                }
            }
            return nTotalQty;
        }
        private double GetColorWiseTotalAmount(int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            double nTotalAmount = 0;
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    nTotalAmount = nTotalAmount + (oItem.Quantity * oItem.UnitPrice);
                }
            }
            return nTotalAmount;
        }
        

        private double GetQty(int nSizeID, int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.Quantity;
                }
            }
            return 0;
        }
        private int GetObjectID(int nSizeID, int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID && oItem.SizeID == nSizeID)
                {
                    return oItem.OrderRecapDetailID;
                }
            }
            return 0;
        }

        private double GetColorWiseProductionQty(int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            double nPoductionQty = 0;
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    return oItem.PoductionQty;
                }
            }
            return nPoductionQty;
        }

        private double GetColorWiseYetToProductionQty(int nColorID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            double nYetToPoductionQty = 0;
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.ColorID == nColorID)
                {
                    return oItem.YetToPoductionQty;
                }
            }
            return nYetToPoductionQty;
        }

        private List<OrderRecapDetail> MapOrderRecapDetailFromColorSizeRation(List<ColorSizeRatio> oColorSizeRatios, List<TechnicalSheetSize> oSizes, OrderRecap oOrderRecap)
        {
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
            OrderRecapDetail oTempOrderRecapDetail = new OrderRecapDetail();
            int nCount = 0;
            foreach (ColorSizeRatio oItem in oColorSizeRatios)
            {
                nCount = 0;
                foreach (TechnicalSheetSize oTempTechnicalSheetSize in oSizes)
                {
                    nCount++;
                    oTempOrderRecapDetail = new OrderRecapDetail();
                    oTempOrderRecapDetail = GetObjIDAndQty(nCount, oItem);
                    if (oTempOrderRecapDetail.Quantity > 0)
                    {
                        oOrderRecapDetail = new OrderRecapDetail();
                        oOrderRecapDetail.OrderRecapDetailID = oTempOrderRecapDetail.OrderRecapDetailID;
                        oOrderRecapDetail.OrderRecapID = oOrderRecap.OrderRecapID;
                        oOrderRecapDetail.ColorID = oItem.ColorID;
                        oOrderRecapDetail.SizeID = oTempTechnicalSheetSize.SizeCategoryID;
                        oOrderRecapDetail.MeasurementUnitID = oOrderRecap.MeasurementUnitID;
                        //oOrderRecapDetail.UnitPrice = oOrderRecap.UnitPrice;
                        oOrderRecapDetail.UnitPrice = oItem.UnitPrice;
                        oOrderRecapDetail.Quantity = oTempOrderRecapDetail.Quantity;
                        oOrderRecapDetail.Amount = oOrderRecapDetail.UnitPrice * oOrderRecapDetail.Quantity;
                        oOrderRecapDetails.Add(oOrderRecapDetail);
                    }
                }
            }
            return oOrderRecapDetails;
        }

        private OrderRecapDetail GetObjIDAndQty(int nCount, ColorSizeRatio oColorSizeRatio)
        {
            OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
            switch (nCount)
            {
                case 1:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID1;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty1;
                    break;
                case 2:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID2;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty2;
                    break;
                case 3:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID3;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty3;
                    break;
                case 4:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID4;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty4;
                    break;
                case 5:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID5;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty5;
                    break;
                case 6:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID6;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty6;
                    break;
                case 7:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID7;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty7;
                    break;
                case 8:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID8;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty8;
                    break;
                case 9:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID9;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty9;
                    break;
                case 10:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID10;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty10;
                    break;
                case 11:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID11;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty11;
                    break;
                case 12:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID12;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty12;
                    break;
                case 13:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID13;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty13;
                    break;
                case 14:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID14;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty14;
                    break;
                case 15:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID15;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty15;
                    break;
                case 16:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID16;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty16;
                    break;
                case 17:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID17;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty17;
                    break;
                case 18:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID18;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty18;
                    break;
                case 19:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID19;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty19;
                    break;
                case 20:
                    oOrderRecapDetail.OrderRecapDetailID = oColorSizeRatio.OrderObjectID20;
                    oOrderRecapDetail.Quantity = oColorSizeRatio.OrderQty20;
                    break;
            }
            return oOrderRecapDetail;
        }

        private double GetUnitPrice(List<OrderRecapDetail> oOrderRecapDetails)
        {
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.UnitPrice > 0)
                {
                    return oItem.UnitPrice;
                }
            }
            return 0;
        }
        private int GetUnitID(List<OrderRecapDetail> oOrderRecapDetails)
        {
            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.MeasurementUnitID > 0)
                {
                    return oItem.MeasurementUnitID;
                }
            }
            return 0;
        }

        #endregion

        private OrderRecap MakeOrderRecap(int nTSID)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            _oOrderRecapDetails = new List<OrderRecapDetail>();
            _oORAssortments = new List<ORAssortment>();
            _oORBarCodes = new List<ORBarCode>();
            
            oTechnicalSheet = oTechnicalSheet.Get(nTSID, (int)Session[SessionInfo.currentUserID]);
            oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(nTSID, (int)Session[SessionInfo.currentUserID]);
            oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(_oOrderRecapDetails, oOrderRecap.TechnicalSheetSizes);
            oOrderRecap.AssortmentColorSizeRatios = MapColorSizeRationFromAssortment(_oORAssortments, oOrderRecap.TechnicalSheetSizes, _oOrderRecapDetails);
            oOrderRecap.BarCodeColorSizeRatios = MapColorSizeRationFromBarCode(_oORBarCodes, oOrderRecap.TechnicalSheetSizes);
            oOrderRecap.Units = MeasurementUnit.GetsbyProductID(oTechnicalSheet.ProductID, (int)Session[SessionInfo.currentUserID]);
            oOrderRecap.MeasurementUnitID = 0;
            oOrderRecap.UnitPrice = 0.00;
            oOrderRecap.OrderRecapYarns = new List<OrderRecapYarn>();
            oOrderRecap.BarCodeComments = new List<BarCodeComment>();
            oOrderRecap.TechnicalSheetID = oTechnicalSheet.TechnicalSheetID;
            oOrderRecap.StyleNo = oTechnicalSheet.StyleNo;
            oOrderRecap.BuyerID = oTechnicalSheet.BuyerID;
            oOrderRecap.BuyerName = oTechnicalSheet.BuyerName;
            oOrderRecap.BrandName = oTechnicalSheet.BrandName;
            oOrderRecap.ProductID = oTechnicalSheet.ProductID;
            oOrderRecap.ProductName = oTechnicalSheet.ProductName;
            oOrderRecap.GG = oTechnicalSheet.GG;
            oOrderRecap.Count = oTechnicalSheet.Count;
            oOrderRecap.SpecialFinish = oTechnicalSheet.SpecialFinish;
            oOrderRecap.Weight = oTechnicalSheet.Weight;
            oOrderRecap.FabricID = oTechnicalSheet.YarnCategoryID;
            oOrderRecap.FabricName = oTechnicalSheet.FabricDescription;
            oOrderRecap.BusinessSessionID = oTechnicalSheet.BusinessSessionID;
            oOrderRecap.MerchandiserID = oTechnicalSheet.MerchandiserID;
            oOrderRecap.BuyerContactPersonID = oTechnicalSheet.BuyerConcernID;
            return oOrderRecap;
        }

        private bool HaveRateViewPermission(EnumRoleOperationType OperationType)
        {
            List<AuthorizationRoleMapping> oAuthorizationRoleMappings = new List<AuthorizationRoleMapping>();
            oAuthorizationRoleMappings = AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]);

            if ((int)Session[SessionInfo.currentUserID] == -9)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < oAuthorizationRoleMappings.Count; i++)
                {
                    if (oAuthorizationRoleMappings[i].OperationType == OperationType && oAuthorizationRoleMappings[i].ModuleName == EnumModuleName.OrderRecap)
                    {
                        return true;

                    }

                }
            }

            return false;
        }

        private List<SizeCategory> GetDistictSizes(List<OrderRecapDetail> oOrderRecapDetails)
        {
            List<SizeCategory> oSizeCategoryies = new List<SizeCategory>();
            SizeCategory oSizeCategory = new SizeCategory();
            List<OrderRecapDetail> oNewOrderRecapDetails = oOrderRecapDetails.OrderBy(CB => CB.SizeSequence).ToList();
            foreach (OrderRecapDetail oItem in oNewOrderRecapDetails)
            {
                if (!IsExist(oSizeCategoryies, oItem))
                {
                    oSizeCategory = new SizeCategory();
                    oSizeCategory.SizeCategoryID = oItem.SizeID;
                    oSizeCategory.SizeCategoryName = oItem.SizeName;
                    oSizeCategoryies.Add(oSizeCategory);
                }
            }

            return oSizeCategoryies;
        }

        private bool IsExist(List<SizeCategory> oSizeCategories, OrderRecapDetail oOrderRecapDetail)
        {
            foreach (SizeCategory oITem in oSizeCategories)
            {
                if (oITem.SizeCategoryID == oOrderRecapDetail.SizeID)
                {
                    return true;
                }
            }
            return false;
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

        #region SetListWithOrderRecaps
        //private List<OrderRecap> SetListWithOrderRecaps(List<OrderRecap> oOrderRecaps, List<OrderRecapDetail> oOrderRecapDetails, List<LabDipOrderDetail> oLabDipOrderDetails, List<DyeingOrderDetail> oDyeingOrderDetails, List<WorkOrderDetail> oWorkOrderDetails, List<PackageBreakdown> oPackageBreakdowns, List<SampleRequirement> oSampleRequirements)
        //{

        //    foreach (OrderRecap oItem in oOrderRecaps)
        //    {
        //        oItem.StyleCoverImage = GetThumImage(oItem.TechnicalSheetID);
        //        oItem.OrderRecapDetails = GetOrderRecapDetails(oItem.OrderRecapID, oOrderRecapDetails);
        //        //oItem.LabDipOrderDetails = GetLabDipDetails(oItem.TechnicalSheetID, oLabDipOrderDetails);
        //        //oItem.DyeingOrderDetails = GetDyeingOrderDetails(oItem.OrderRecapID, oDyeingOrderDetails);
        //        //oItem.WorkOrderDetails = GetWorkOrderDetails(oItem.OrderRecapID, oWorkOrderDetails);
        //        //oItem.PackageBreakdowns = GetPackageBreakDowns(oItem.OrderRecapID, oPackageBreakdowns);
        //        oItem.SampleRequirements = GetSampleRequirements(oItem.OrderRecapID, oSampleRequirements);
        //    }
        //    return oOrderRecaps;

        //}


        private List<OrderRecapDetail> GetOrderRecapDetails(int nOrderRecapID, List<OrderRecapDetail> oOrderRecapDetails)
        {
            List<OrderRecapDetail> oNewOrderRecapDetails = new List<OrderRecapDetail>();

            foreach (OrderRecapDetail oItem in oOrderRecapDetails)
            {
                if (oItem.OrderRecapID == nOrderRecapID)
                    oNewOrderRecapDetails.Add(oItem);
            }

            return oNewOrderRecapDetails;
        }
        //private List<LabDipOrderDetail> GetLabDipDetails(int nTechnicalSheetID, List<LabDipOrderDetail> oLabDipOrderDetails)
        //{
        //    List<LabDipOrderDetail> oNewLabDipOrderDetails = new List<LabDipOrderDetail>();

        //    foreach (LabDipOrderDetail oItem in oLabDipOrderDetails)
        //    {
        //        if (oItem.TechnicalSheetID == nTechnicalSheetID)
        //            oNewLabDipOrderDetails.Add(oItem);
        //    }

        //    return oNewLabDipOrderDetails;
        //}
        //private List<DyeingOrderDetail> GetDyeingOrderDetails(int nOrderRecapID, List<DyeingOrderDetail> oDyeingOrderDetails)
        //{
        //    List<DyeingOrderDetail> oNewDyeingOrderDetails = new List<DyeingOrderDetail>();

        //    foreach (DyeingOrderDetail oItem in oDyeingOrderDetails)
        //    {
        //        if (oItem.OrderRecapID == nOrderRecapID)
        //            oNewDyeingOrderDetails.Add(oItem);
        //    }

        //    return oNewDyeingOrderDetails;
        //}
        //private List<WorkOrderDetail> GetWorkOrderDetails(int nOrderRecapID, List<WorkOrderDetail> oWorkOrderDetails)
        //{
        //    List<WorkOrderDetail> oNewWorkOrderDetails = new List<WorkOrderDetail>();

        //    foreach (WorkOrderDetail oItem in oWorkOrderDetails)
        //    {
        //        if (oItem.OrderRecapID == nOrderRecapID)
        //            oNewWorkOrderDetails.Add(oItem);
        //    }

        //    return oNewWorkOrderDetails;
        //}
        //private List<PackageBreakdown> GetPackageBreakDowns(int nOrderRecapID, List<PackageBreakdown> oPackageBreakdowns)
        //{
        //    List<PackageBreakdown> oNewPackageBreakdowns = new List<PackageBreakdown>();

        //    foreach (PackageBreakdown oItem in oPackageBreakdowns)
        //    {
        //        if (oItem.OrderRecapID == nOrderRecapID)
        //            oNewPackageBreakdowns.Add(oItem);
        //    }

        //    return oNewPackageBreakdowns;
        //}

        private List<SampleRequirement> GetSampleRequirements(int nOrderRecapID, List<SampleRequirement> oSampleRequirements)
        {
            List<SampleRequirement> oNewSampleRequirements = new List<SampleRequirement>();

            foreach (SampleRequirement oItem in oSampleRequirements)
            {
                if (oItem.OrderRecapID == nOrderRecapID)
                    oNewSampleRequirements.Add(oItem);
            }

            return oNewSampleRequirements;
        }

        #endregion

        #region GetBuyerwithDatesummaries
        private List<RecapShipmentSummary> GetBuyerwithDatesummaries(List<RecapShipmentSummary> oRecapShipmentSummaries)
        {
            List<RecapShipmentSummary> oNewRecapShipmentSummaries = new List<RecapShipmentSummary>();
            foreach (RecapShipmentSummary oItem in oRecapShipmentSummaries)
            {
                if (oItem.DataViewType == 1)
                {
                    oNewRecapShipmentSummaries.Add(oItem);
                }
            }
            return oNewRecapShipmentSummaries;
        }
        #endregion
        #endregion

        #region View
        public ActionResult ViewOrderRecaps(int buid, int OT, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            TempData["OT"] = OT;            
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            ViewBag.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.BUID = buid;
            _oOrderRecaps = new List<OrderRecap>();
            //if (buid>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            //{
            //    _oOrderRecaps = OrderRecap.GetsByBUWithOrderType(buid, OT.ToString(), (int)Session[SessionInfo.currentUserID]);
            //}
            //else
            //{
            //    _oOrderRecaps = OrderRecap.Gets("SELECT * FROM View_OrderRecap  WHERE OrderType IN (" + OT.ToString() + ") AND ISNULL(OrderRecapStatus,0) = " + (int)EnumOrderRecapStatus.Initialized, (int)Session[SessionInfo.currentUserID]);
            //}        
            return View(_oOrderRecaps);
        }
        public ActionResult ViewOrderRecap(int id, int OT, int TSID, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            _oOrderRecap = new OrderRecap();
            List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ORAssortment> oORAssortments = new List<ORAssortment>();
            List<ORBarCode> oORBarCodes = new List<ORBarCode>();
            if (TSID <= 0)
            {
                if (id > 0)
                {
                    _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                    oOrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                    oORAssortments = ORAssortment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                    oORBarCodes = ORBarCode.Gets(id, (int)Session[SessionInfo.currentUserID]);
                    _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                    _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);
                    _oOrderRecap.AssortmentColorSizeRatios = MapColorSizeRationFromAssortment(oORAssortments, _oOrderRecap.TechnicalSheetSizes, oOrderRecapDetails);//Make on ORAssortment
                    _oOrderRecap.BarCodeColorSizeRatios = MapColorSizeRationFromBarCode(oORBarCodes, _oOrderRecap.TechnicalSheetSizes);//Make on ORBarCode
                    _oOrderRecap.Units = MeasurementUnit.GetsbyProductID(_oOrderRecap.ProductID, (int)Session[SessionInfo.currentUserID]);
                    _oOrderRecap.MeasurementUnitID = GetUnitID(oOrderRecapDetails);
                   // _oOrderRecap.UnitPrice = GetUnitPrice(oOrderRecapDetails);
                    _oOrderRecap.OrderRecapYarns = OrderRecapYarn.Gets(id, (int)EnumRecapRefType.OrderRecap, (int)Session[SessionInfo.currentUserID]);
                    _oOrderRecap.BarCodeComments = BarCodeComment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                }
                else
                {
                    _oOrderRecap.OrderRecapDetails = new List<OrderRecapDetail>();
                    _oOrderRecap.Units = new List<MeasurementUnit>();
                    _oOrderRecap.ColorSizeRatios = new List<ColorSizeRatio>();
                    _oOrderRecap.TechnicalSheetSizes = new List<TechnicalSheetSize>();
                    _oOrderRecap.AssortmentColorSizeRatios = new List<ColorSizeRatio>();
                    _oOrderRecap.BarCodeColorSizeRatios = new List<ColorSizeRatio>();
                    _oOrderRecap.OrderRecapYarns = new List<OrderRecapYarn>();
                    _oOrderRecap.ORAssortments = new List<ORAssortment>();
                    _oOrderRecap.ORBarCodes = new List<ORBarCode>();
                    _oOrderRecap.BarCodeComments = new List<BarCodeComment>();
                }
            }
            else
            {
                _oOrderRecap = MakeOrderRecap(TSID);
            }
            _oOrderRecap.OrderTypeInInt = OT;
            _oOrderRecap.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.MaterialTypes = MaterialType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecap);
        }
       

        public ActionResult ViewOrderRecapDetailOld(string Temp, double ts)
        {
            OrderRecapDetail _oOrderRecapDetail = new OrderRecapDetail();
            int nTechnicalSheetID = Convert.ToInt32(Temp.Split('~')[0]);
            int nProductID = Convert.ToInt32(Temp.Split('~')[1]);
            _oOrderRecapDetail.TechnicalSheetID = nTechnicalSheetID;
            _oOrderRecapDetail.Colors = TechnicalSheetColor.Gets(nTechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecapDetail.Sizes = TechnicalSheetSize.Gets(nTechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecapDetail.Units = MeasurementUnit.GetsbyProductID(nProductID, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecapDetail);
        }

        #region Copy
        [HttpPost]
        public JsonResult CopyOrderRecap(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                oOrderRecap.OrderRecapID = 0;//Set Sale Order ID is 0
                oOrderRecap.OrderRecapStatus = EnumOrderRecapStatus.Initialized;
                foreach (OrderRecapYarn oItem in oOrderRecap.OrderRecapYarns)
                {
                    oItem.RefObjectID = 0;
                    oItem.OrderRecapYarnID = 0;
                }
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecap.OrderRecapDetails = MapOrderRecapDetailFromColorSizeRation(oOrderRecap.ColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                if (oOrderRecap.AssortmentColorSizeRatios != null)
                {
                    if (oOrderRecap.AssortmentColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORAssortments = MapORAssortmentFromColorSizeRation(oOrderRecap.AssortmentColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                        foreach (ORAssortment oItem in oOrderRecap.ORAssortments)
                        {
                            oItem.ORAssortmentID = 0;
                            oItem.OrderRecapID = 0;
                        }
                    }
                }
                if (oOrderRecap.BarCodeColorSizeRatios != null)
                {
                    if (oOrderRecap.BarCodeColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORBarCodes = MapORBarCodeFromColorSizeRation(oOrderRecap.BarCodeColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                        foreach (ORBarCode oItem in oOrderRecap.ORBarCodes)
                        {
                            oItem.ORBarCodeID = 0;
                            oItem.OrderRecapID = 0;
                        }
                    }
                }
                foreach (OrderRecapDetail oItem in oOrderRecap.OrderRecapDetails)
                {
                    oItem.OrderRecapDetailID = 0;
                    oItem.OrderRecapID = 0;
                }
                _oOrderRecap = oOrderRecap.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult Save(OrderRecap oOrderRecap)
        {
            double nCMValue = 0;
            _oOrderRecap = new OrderRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                nCMValue = oOrderRecap.CMValue;
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecap.OrderRecapStatus = EnumOrderRecapStatus.Initialized;
                oOrderRecap.OrderRecapDetails = MapOrderRecapDetailFromColorSizeRation(oOrderRecap.ColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                oOrderRecap.AssortmentType = (EnumAssortmentType)oOrderRecap.AssortmentTypeInt;
                if (oOrderRecap.AssortmentColorSizeRatios != null)
                {
                    if (oOrderRecap.AssortmentColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORAssortments = MapORAssortmentFromColorSizeRation(oOrderRecap.AssortmentColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }
                if (oOrderRecap.BarCodeColorSizeRatios != null)
                {
                    if (oOrderRecap.BarCodeColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORBarCodes = MapORBarCodeFromColorSizeRation(oOrderRecap.BarCodeColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }
                _oOrderRecap = oOrderRecap.Save((int)Session[SessionInfo.currentUserID]);
                string sBaseAddress = Convert.ToString(Session[SessionInfo.BaseAddress]);
                if (sBaseAddress == "/hrm")
                {
                    _oOrderRecap = _oOrderRecap.UpdateCMValue(_oOrderRecap.OrderRecapID, nCMValue, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ApprovedOrderRecap(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecap.OrderType = EnumOrderType.BulkOrder;
                oOrderRecap.OrderRecapStatus = (EnumOrderRecapStatus)oOrderRecap.OrderRecapStatusInInt;
                oOrderRecap.OrderRecapDetails = MapOrderRecapDetailFromColorSizeRation(oOrderRecap.ColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                oOrderRecap.AssortmentType = (EnumAssortmentType)oOrderRecap.AssortmentTypeInt;
                if (oOrderRecap.AssortmentColorSizeRatios != null)
                {
                    if (oOrderRecap.AssortmentColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORAssortments = MapORAssortmentFromColorSizeRation(oOrderRecap.AssortmentColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }
                if (oOrderRecap.BarCodeColorSizeRatios != null)
                {
                    if (oOrderRecap.BarCodeColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORBarCodes = MapORBarCodeFromColorSizeRation(oOrderRecap.BarCodeColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }

                #region Set Order Type
                //Add By Faruk Temprary Solution for Order type fixed in thae case of order recap approved from Notification 
                OrderRecap oTempOrderRecap = new OrderRecap();
                oTempOrderRecap = oTempOrderRecap.Get(oOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecap.OrderTypeInInt = oTempOrderRecap.OrderTypeInInt;
                #endregion

                _oOrderRecap = oOrderRecap.ApprovedOrderRecap((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Revise and Revise Log
        public ActionResult ViewOrderRecapRevise(int id, int OT, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oOrderRecap = new OrderRecap();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ORAssortment> oORAssortments = new List<ORAssortment>();
            List<ORBarCode> oORBarCodes = new List<ORBarCode>();
            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                oOrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oORAssortments = ORAssortment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                oORBarCodes = ORBarCode.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);
                _oOrderRecap.AssortmentColorSizeRatios = MapColorSizeRationFromAssortment(oORAssortments, _oOrderRecap.TechnicalSheetSizes, oOrderRecapDetails);//Make on ORAssortment
                _oOrderRecap.BarCodeColorSizeRatios = MapColorSizeRationFromBarCode(oORBarCodes, _oOrderRecap.TechnicalSheetSizes);//Make on ORBarCode
                _oOrderRecap.Units = MeasurementUnit.GetsbyProductID(_oOrderRecap.ProductID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.MeasurementUnitID = GetUnitID(oOrderRecapDetails);
                //_oOrderRecap.UnitPrice = GetUnitPrice(oOrderRecapDetails);
                _oOrderRecap.OrderRecapYarns = OrderRecapYarn.Gets(id,(int)EnumRecapRefType.OrderRecap,   (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.BarCodeComments = BarCodeComment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                _oOrderRecap.OrderRecapDetails = new List<OrderRecapDetail>();
                _oOrderRecap.Units = new List<MeasurementUnit>();
                _oOrderRecap.ColorSizeRatios = new List<ColorSizeRatio>();
                _oOrderRecap.TechnicalSheetSizes = new List<TechnicalSheetSize>();
                _oOrderRecap.AssortmentColorSizeRatios = new List<ColorSizeRatio>();
                _oOrderRecap.BarCodeColorSizeRatios = new List<ColorSizeRatio>();
                _oOrderRecap.OrderRecapYarns = new List<OrderRecapYarn>();
                _oOrderRecap.ORAssortments = new List<ORAssortment>();
                _oOrderRecap.ORBarCodes = new List<ORBarCode>();
                _oOrderRecap.BarCodeComments = new List<BarCodeComment>();
            }
            _oOrderRecap.OrderTypeInInt = OT;
            _oOrderRecap.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            //_oOrderRecap.OrderRecapPackingPolicyList = OrderRecapPackingPolicy.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.MaterialTypes = MaterialType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecap);
        }
        [HttpPost]
        public JsonResult AcceptRevise(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecap.OrderRecapStatus = EnumOrderRecapStatus.Initialized;
                oOrderRecap.OrderRecapDetails = MapOrderRecapDetailFromColorSizeRation(oOrderRecap.ColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                if (oOrderRecap.AssortmentColorSizeRatios != null)
                {
                    if (oOrderRecap.AssortmentColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORAssortments = MapORAssortmentFromColorSizeRation(oOrderRecap.AssortmentColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }
                if (oOrderRecap.BarCodeColorSizeRatios != null)
                {
                    if (oOrderRecap.BarCodeColorSizeRatios.Count > 0)
                    {
                        oOrderRecap.ORBarCodes = MapORBarCodeFromColorSizeRation(oOrderRecap.BarCodeColorSizeRatios, oTechnicalSheetSizes, oOrderRecap);
                    }
                }
                _oOrderRecap = oOrderRecap.AcceptRevise((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #region Revise History  And Log
        public ActionResult OrderRecapReviseHistory(int id)//ODS ID
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecapLog WHERE OrderRecapID  = " + id;
            _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecaps);
        }
        public ActionResult ViewOrderRecapLog(int id, int OT, double ts)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oOrderRecap = new OrderRecap();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ORAssortment> oORAssortments = new List<ORAssortment>();
            List<ORBarCode> oORBarCodes = new List<ORBarCode>();
            _oOrderRecap = _oOrderRecap.GetByLog(id, (int)Session[SessionInfo.currentUserID]);
            oOrderRecapDetails = OrderRecapDetail.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            oORAssortments = ORAssortment.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            oORBarCodes = ORBarCode.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);
            _oOrderRecap.AssortmentColorSizeRatios = MapColorSizeRationFromAssortment(oORAssortments, _oOrderRecap.TechnicalSheetSizes, oOrderRecapDetails);
            _oOrderRecap.BarCodeColorSizeRatios = MapColorSizeRationFromBarCode(oORBarCodes, _oOrderRecap.TechnicalSheetSizes);
            _oOrderRecap.Units = MeasurementUnit.GetsbyProductID(_oOrderRecap.ProductID, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.MeasurementUnitID = GetUnitID(oOrderRecapDetails);
            //_oOrderRecap.UnitPrice = GetUnitPrice(oOrderRecapDetails);
            _oOrderRecap.OrderRecapYarns = OrderRecapYarn.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.OrderTypeInInt = OT;
            _oOrderRecap.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.Employees = Employee.Gets(EnumEmployeeDesignationType.Merchandiser, (int)Session[SessionInfo.currentUserID]);
            //_oOrderRecap.OrderRecapPackingPolicyList = OrderRecapPackingPolicy.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.CurrencyList = Currency.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(_oOrderRecap);
        }

        #endregion

        #endregion

        #region HTTP ChangeStatus
        [HttpPost]
        public JsonResult ChangeStatus(OrderRecap oOrderRecap)
        {
            try
            {
                if (oOrderRecap.ActionTypeInInt == 1)//Sale Order Request for Approval
                {
                    oOrderRecap.OrderRecapStatus = EnumOrderRecapStatus.RequestForApproval; // For Entry Approval Request Table Entry 
                    oOrderRecap.ApprovalRequest.RequestBy = (int)Session[SessionInfo.currentUserID];
                    oOrderRecap.ApprovalRequest.OperationType = EnumApprovalRequestOperationType.OrderRecap;
                    oOrderRecap.ActionType = EnumActionType.RequestForApproval;
                }
                else if (oOrderRecap.ActionTypeInInt == 2)
                {
                    oOrderRecap.ActionType = EnumActionType.UndoRequest;
                    oOrderRecap.ApprovalRequest = new ApprovalRequest();
                }
                else if (oOrderRecap.ActionTypeInInt == 3)
                {
                    //Sale Order Approve
                    string sPinCode = "";
                    string sPassword = Global.Decrypt(((User)Session[SessionInfo.CurrentUser]).Password);
                    if (oOrderRecap.PinCode != null)
                    {
                        sPinCode = oOrderRecap.PinCode;
                    }
                    if (sPinCode != sPassword)
                    {
                        _oOrderRecap = new OrderRecap();
                        _oOrderRecap.ErrorMessage = "Your Pin Code doesn't match";
                        JavaScriptSerializer aserializer = new JavaScriptSerializer();
                        string sajson = aserializer.Serialize(_oOrderRecap);
                        return Json(sajson, JsonRequestBehavior.AllowGet);
                    }
                    oOrderRecap.ActionType = EnumActionType.Approve;
                    oOrderRecap.ApprovalRequest = new ApprovalRequest();
                }
                else if (oOrderRecap.ActionTypeInInt == 4)
                {
                    oOrderRecap.ActionType = EnumActionType.UndoApprove;
                    oOrderRecap.ApprovalRequest = new ApprovalRequest();
                }
                else if (oOrderRecap.ActionTypeInInt == 5)// request for Revise
                {
                    oOrderRecap.ActionType = EnumActionType.Request_For_Revise;
                    oOrderRecap.ApprovalRequest = new ApprovalRequest();
                    oOrderRecap.OrderRecapStatus = EnumOrderRecapStatus.Request_For_Revise; // For Entry REvise Request Table Entry 
                    oOrderRecap.ReviseRequest.RequestBy = (int)Session[SessionInfo.currentUserID];
                    oOrderRecap.ReviseRequest.OperationType = EnumReviseRequestOperationType.OrderRecap;
                }

                _oOrderRecap = oOrderRecap.ChangeStatus((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Wait for Revise
        [HttpGet]
        public JsonResult WaitForRevise(int BUID, double ts)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_OrderRecap Where  OrderRecapStatus = " + (int)EnumOrderRecapStatus.Request_For_Revise;
            if (BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID = " + BUID ;
            }
            try
            {
                _oOrderRecaps = new List<OrderRecap>();
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public JsonResult ActiveInActive(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            try
            {
                _oOrderRecap = oOrderRecap.ActiveInActive((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ShippedUnShipped(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();            
            try
            {
                _oOrderRecap = oOrderRecap.ShippedUnShipped((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTSColors(TechnicalSheet oTechnicalSheet)
        {
            List<TechnicalSheetColor> oTechnicalSheetColors = new List<TechnicalSheetColor>();
            int nPAMID = (oTechnicalSheet.Note != "" || oTechnicalSheet.Note != null) ? Convert.ToInt32(oTechnicalSheet.Note) : 0;
            try
            {
                if (nPAMID > 0)
                {
                    List<PAMDetail> oPAMDetails = new List<PAMDetail>();
                    oPAMDetails = PAMDetail.Gets(nPAMID, (int)Session[SessionInfo.currentUserID]);
                    foreach(PAMDetail oItem in oPAMDetails)
                    {
                        TechnicalSheetColor oNewTechnicalSheetColor = new TechnicalSheetColor();
                        oNewTechnicalSheetColor.ColorCategoryID = oItem.ColorID;
                        oNewTechnicalSheetColor.ColorName = oItem.ColorName;
                        oNewTechnicalSheetColor.Quantity = oItem.ColorWiseYetToRecapQty;
                        oTechnicalSheetColors.Add(oNewTechnicalSheetColor);
                    }
                }
                else
                {
                    oTechnicalSheetColors = TechnicalSheetColor.Gets(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                }
            }
            catch (Exception ex)
            {
                TechnicalSheetColor oTechnicalSheetColor = new TechnicalSheetColor();
                oTechnicalSheetColor.ErrorMessage = ex.Message;
                oTechnicalSheetColors.Add(oTechnicalSheetColor);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheetColors);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewAddNewColor(int tsid, double ts)
        {
            List<ColorCategory> oColorCategorys = new List<ColorCategory>();
            oColorCategorys = ColorCategory.GetsTSNotAssignColor(tsid, (int)Session[SessionInfo.currentUserID]);
            TempData["message"] = tsid;
            return PartialView(oColorCategorys);
        }

        [HttpPost]
        public JsonResult SaveTechnicalSheetColor(List<TechnicalSheetColor> oTechnicalSheetColors)
        {
            OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
            string sFeedBackMessage = "";
            if (oTechnicalSheetColors.Count > 0)
            {
                try
                {
                    sFeedBackMessage = OrderRecap.PickNewColor(oTechnicalSheetColors, (int)Session[SessionInfo.currentUserID]);
                    if (sFeedBackMessage == "Data Save Successfully")
                    {
                        oOrderRecapDetail.ErrorMessage = sFeedBackMessage;
                        oOrderRecapDetail.Colors = TechnicalSheetColor.Gets(oTechnicalSheetColors[0].TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                        oOrderRecapDetail.Sizes = TechnicalSheetSize.Gets(oTechnicalSheetColors[0].TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                    }
                    else
                    {
                        oOrderRecapDetail.ErrorMessage = sFeedBackMessage;
                    }
                }
                catch (Exception ex)
                {
                    _oOrderRecap.ErrorMessage = ex.Message;
                }
            }
            else
            {
                oOrderRecapDetail.ErrorMessage = "Please Select at least one color";
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecapDetail);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Delete(int id)
        {
            string sErrorMease = "";
            try
            {
                _oOrderRecap = new OrderRecap();
                sErrorMease = _oOrderRecap.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                sErrorMease = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sErrorMease);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetOrderRecapDetails(int id, double ts)
        {
            _oOrderRecap = new OrderRecap();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            try
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                oOrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);

            }
            catch (Exception ex)
            {
                ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
                oColorSizeRatio.ErrorMessage = ex.Message;
                oColorSizeRatios.Add(oColorSizeRatio);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColorSizeRatios);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetContactPersonnel(int nBuyerID)
        {
            //OrderRecap oOrderRecap = new OrderRecap();
            _oOrderRecap.ContactPersonnelList = ContactPersonnel.GetsByContractor(nBuyerID, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetTechnicalSheetSize(TechnicalSheet oTechnicalSheet)
        {
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();
            oTechnicalSheetSizes = TechnicalSheetSize.Gets(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oTechnicalSheetSizes);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetPAMs(TechnicalSheet oTechnicalSheet)
        {
            List<PAM> oPAMs = new List<PAM>();
            oPAMs = PAM.Gets("SELECT * FROM View_PAM WHERE StyleID = " + oTechnicalSheet.TechnicalSheetID + " AND  YetToRecapQty>0", (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oPAMs);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult GetRecapBillOfMaterial(TechnicalSheet oTechnicalSheet)
        //{
        //    List<BillOfMaterial> oBillOfMaterials = new List<BillOfMaterial>();
        //    _oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
        //    oBillOfMaterials = BillOfMaterial.Gets(oTechnicalSheet.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
        //    foreach (BillOfMaterial oItem in oBillOfMaterials)
        //    {
        //        _oRecapBillOfMaterial = new RecapBillOfMaterial();
        //        _oRecapBillOfMaterial.OrderRecapID = 0;
        //        _oRecapBillOfMaterial.ProductID = oItem.ProductID;
        //        _oRecapBillOfMaterial.ItemDescription = oItem.ItemDescription;
        //        _oRecapBillOfMaterial.Reference = oItem.Reference;
        //        _oRecapBillOfMaterial.Description = oItem.ItemDescription;
        //        _oRecapBillOfMaterial.Construction = oItem.Construction;
        //        _oRecapBillOfMaterial.Color = oItem.ColorName;
        //        _oRecapBillOfMaterial.ProductID = oItem.ProductID;
        //        _oRecapBillOfMaterial.AttachFile = oItem.AttachFile;
        //        _oRecapBillOfMaterial.Sequence = oItem.Sequence;
        //        _oRecapBillOfMaterials.Add(_oRecapBillOfMaterial);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(_oRecapBillOfMaterials);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult GetMeasurementUnitByProduct(TechnicalSheet oTechnicalSheet)
        {
            List<MeasurementUnit> oMeasurementUnits = new List<MeasurementUnit>();
            oMeasurementUnits = MeasurementUnit.GetsbyProductID(oTechnicalSheet.ProductID, (int)Session[SessionInfo.currentUserID]);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oMeasurementUnits);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

     

        public ActionResult ApprovePiker(int nOrderRecapID)
        {
            _oOrderRecap = new OrderRecap();
            List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();

            if (nOrderRecapID > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(nOrderRecapID, (int)Session[SessionInfo.currentUserID]);
                oOrderRecapDetails = OrderRecapDetail.Gets(nOrderRecapID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);
                _oOrderRecap.OrderRecapDetails = oOrderRecapDetails;

            }
            return PartialView(_oOrderRecap);
        }

        [HttpGet]
        public JsonResult GetOrderRecaps(int id, double ts)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE TechnicalSheetID  = " + id + " AND ApproveBy !=0 AND ISNULL(IsActive,0)= 1";

            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetOrderRecapsByStyleAndNo(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE '%" + oOrderRecap.StyleNo + "%' OR StyleNo LIKE '%" + oOrderRecap.StyleNo + "%' AND ApproveBy !=0 AND ISNULL(IsActive,0)= 1";
            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

       

        #endregion

        #region MIS Reports
        public ActionResult OrderRecapReport(bool bIsCommissionStatement,int buid, int menuid)
        {
            #region Report Format
            List<EnumObject> oEnumObjects = new List<EnumObject>();
            List<EnumObject> oTempReportFormats = new List<EnumObject>();
            oTempReportFormats = EnumObject.jGets(typeof(EnumReportLayout));
            foreach (EnumObject oItem in oTempReportFormats)
            {
                if (bIsCommissionStatement)
                {
                    if (oItem.id == (int)EnumReportLayout.Factorywise || oItem.id == (int)EnumReportLayout.PartyWise || oItem.id == (int)EnumReportLayout.ShipmentDateWise || oItem.id == (int)EnumReportLayout.Buying_Commission_Statement)
                    {
                        oEnumObjects.Add(oItem);
                    }
                }
                else
                {
                    if (oItem.id == (int)EnumReportLayout.Factorywise || oItem.id == (int)EnumReportLayout.MerchandiserWise || oItem.id == (int)EnumReportLayout.ProductCatagoryWise || oItem.id == (int)EnumReportLayout.PartyWise || oItem.id == (int)EnumReportLayout.YarnFabricWise || oItem.id == (int)EnumReportLayout.AgentWise || oItem.id == (int)EnumReportLayout.MOBReport || oItem.id == (int)EnumReportLayout.DepartmentWise)
                    {
                        oEnumObjects.Add(oItem);
                    }
                }
            }
            #endregion

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            _oOrderRecaps = new List<OrderRecap>();
            ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            ViewBag.SubGenders = EnumObject.jGets(typeof(EnumSubGender));
            ViewBag.StyleDepartments = StyleDepartment.Gets((int)Session[SessionInfo.currentUserID]);
            ViewBag.ReportFormats = oEnumObjects;
            ViewBag.CommissionStatement = bIsCommissionStatement;
            ViewBag.BUID = buid;
            ViewBag.OrderRecapStatus = EnumObject.jGets(typeof(EnumOrderRecapStatus));
            ViewBag.ProductCategories = ProductCategory.GetsBUWiseLastLayer(buid, (int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecaps);
        }
        public ActionResult OrderRecapManagementReport(string sParam, int ReportFormat)
        {
            _oOrderRecapMgtReport = new OrderRecapMgtReport();
            rptOrderRecapMgt oReport = new rptOrderRecapMgt();
            rptOrderRecapMgtFormat1 oReportFormat1 = new rptOrderRecapMgtFormat1();
            byte[] abytes = null;
            
           Company oCompany = new Company();
            string sSQL = MakeQuery(sParam, ReportFormat);
            _oOrderRecapMgtReport.OrderRecapMgtReports = OrderRecapMgtReport.Gets(sSQL, ReportFormat, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.OrderRecap_Report_Format, (int)Session[SessionInfo.currentUserID]);
            if (oClientOperationSetting.Value=="")
            {
                string sMessage = "Please Set Order Recap Report Format in Client Operation Setting Menu. Thanks.";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
            if (_oOrderRecapMgtReport.OrderRecapMgtReports.Count > 0)
            {
                if (ReportFormat == (int)EnumReportLayout.Factorywise)
                {

                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.FactoryWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.FactoryWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }

                }
                else if (ReportFormat == (int)EnumReportLayout.MerchandiserWise)
                {

                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.MerchandiserWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.MerchandiserWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }

                }
                else if (ReportFormat == (int)EnumReportLayout.ProductCatagoryWise)
                {
                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.ProductCategoryWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.ProductCategoryWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }

                }
                else if (ReportFormat == (int)EnumReportLayout.PartyWise)
                {
                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.BuyerWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.BuyerWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }

                }
                else if (ReportFormat == (int)EnumReportLayout.YarnFabricWise)
                {
                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.YarnWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.YarnWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                }
                else if (ReportFormat == (int)EnumReportLayout.AgentWise)
                {
                    if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                    {
                        abytes = oReport.AgentWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                    else
                    {
                        abytes = oReportFormat1.AgentWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                    }
                }
                else if (ReportFormat == (int)EnumReportLayout.MOBReport)
                {
                    rptMOBReport oMOBReport = new rptMOBReport();
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReport.OrderRecapMgtReports)
                    {
                        oItem.StyleCoverImage = GetThumImageForPrint(oItem.TechnicalSheetID);
                    }
                    abytes = oMOBReport.PrepareReport(_oOrderRecapMgtReport, oCompany);
                }
                else if (ReportFormat == (int)EnumReportLayout.DepartmentWise)
                {
                    abytes = oReportFormat1.DepartmentWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                }
                else
                {
                    string sMessage = "There is no Report Format for print";
                    return RedirectToAction("MessageHelper", "User", new { message = sMessage });
                }
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }
        public ActionResult OrderRecapBuyingStatement(string sParam, int ReportFormat)
        {
            _oOrderRecapMgtReport = new OrderRecapMgtReport();
            rptBuyingStatement oReport = new rptBuyingStatement();

            byte[] abytes = null;
            Company oCompany = new Company();
            string sSQL = MakeQuery(sParam, ReportFormat);
            _oOrderRecapMgtReport.OrderRecapMgtReports = OrderRecapMgtReport.Gets(sSQL, ReportFormat, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            if (_oOrderRecapMgtReport.OrderRecapMgtReports.Count > 0)
            {
                if (ReportFormat == (int)EnumReportLayout.Buying_Commission_Statement)
                {
                    abytes = oReport.PrepareReportBuyingCommissionStatement(_oOrderRecapMgtReport, oCompany);
                }
                else if (ReportFormat == (int)EnumReportLayout.Factorywise)
                {
                    abytes = oReport.FactoryWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                }
                else if (ReportFormat == (int)EnumReportLayout.PartyWise)
                {

                    abytes = oReport.BuyerWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                }
                else if (ReportFormat == (int)EnumReportLayout.ShipmentDateWise)
                {
                    abytes = oReport.ShipmentDateWisePrepareReport(_oOrderRecapMgtReport, oCompany);
                }
                else
                {
                    string sMessage = "There is no Report Format for print";
                    return RedirectToAction("MessageHelper", "User", new { message = sMessage });
                }
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }

        }
        public void OrderRecapManagementReportXL(string sParam, int ReportFormat)
        {
            #region Get Data
            Company oCompany = new Company();
            string sSQL = MakeQuery(sParam, ReportFormat);
            _oOrderRecapMgtReports = OrderRecapMgtReport.Gets(sSQL, ReportFormat, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderRecap_Report_Format, (int)Session[SessionInfo.currentUserID]); 
            #endregion

            #region Export To Excel
            if (ReportFormat == (int)EnumReportLayout.MOBReport)
            {
                #region MOB Report
                foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                {
                    oItem.StyleCoverImage = GetThumImageForPrint(oItem.TechnicalSheetID);
                }

                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelRange HeaderCell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("MOB Report");
                    sheet.Name = "MOB Report";
                    sheet.Column(2).Width = 6;   // SL NO
                    sheet.Column(3).Width = 13;  // Style InputDate
                    sheet.Column(4).Width = 20;  //Style No
                    sheet.Column(5).Width = 9;  // Style Image
                    sheet.Column(6).Width = 20;  // Buyer Name
                    sheet.Column(7).Width = 15;  // Contact Person
                    sheet.Column(8).Width = 30;  // Merchandiser
                    sheet.Column(9).Width = 20;  // Recap No
                    sheet.Column(10).Width = 11; // Season
                    sheet.Column(11).Width = 20; // Garments
                    sheet.Column(12).Width = 13; // Order Dt
                    sheet.Column(13).Width = 13; // Shipment Dt
                    sheet.Column(14).Width = 40; // Color Range
                    sheet.Column(15).Width = 30; // Size Range
                    sheet.Column(16).Width = 11; // Order Qty
                    sheet.Column(17).Width = 11; // Cutting Qty
                    sheet.Column(18).Width = 13; // Sweeing Qty
                    sheet.Column(19).Width = 15; // Shipment Qty
                    sheet.Column(20).Width = 35; // Remarks
                    nEndCol = 20;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Factory Wise MOB Report"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    int nFactoryID = 0; int nCount = 0; bool IsTotalPrint = false;
                    double nTotalOrderQty = 0, nTotalCuttingQty = 0, nTotalSweeingQty = 0, nTotalShipmentQty = 0;
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                    {
                        nCount++;
                        if (nFactoryID != oItem.FactoryID)
                        {
                            if (IsTotalPrint == true)
                            {
                                #region Total
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                                cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 16);
                                sEndCell = Global.GetExcelCellName(nEndRow, 16);
                                cell = sheet.Cells[nRowIndex, 16]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                //cell.Style.Font.UnderLine = true;
                                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 17);
                                sEndCell = Global.GetExcelCellName(nEndRow, 17);
                                cell = sheet.Cells[nRowIndex, 17]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                //cell.Style.Font.UnderLine = true;
                                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 18);
                                sEndCell = Global.GetExcelCellName(nEndRow, 18);
                                cell = sheet.Cells[nRowIndex, 18]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                //cell.Style.Font.UnderLine = true;
                                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 19);
                                sEndCell = Global.GetExcelCellName(nEndRow, 19);
                                cell = sheet.Cells[nRowIndex, 19]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                //cell.Style.Font.UnderLine = true;
                                //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nCount = 1;


                            #region Factory Name
                            nRowIndex = nRowIndex + 2;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "Factory Name : " + oItem.FactoryName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Column Header
                            nRowIndex = nRowIndex + 1;
                            nStartRow = nRowIndex;
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style InputDate"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Image"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Contact Person"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Season"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Garments"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Color Range"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Size Range"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Cutting Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 18]; cell.Value = "Sweeing Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 19]; cell.Value = "Shipment Qty"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 20]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            HeaderCell = sheet.Cells[nRowIndex, 2, nRowIndex, 21];
                            nRowIndex = nRowIndex + 1;
                            IsTotalPrint = true;
                            nFactoryID = oItem.FactoryID;
                            #endregion
                        }

                        #region Data
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleInputDate.ToString("dd MMM yyyy"); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        if (oItem.StyleCoverImage != null)
                        {
                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            sheet.Row(nRowIndex).Height = 40; border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            ExcelPicture excelImage = null;
                            excelImage = sheet.Drawings.AddPicture(oItem.StyleNo + oItem.OrderRecapID.ToString(), oItem.StyleCoverImage);
                            excelImage.From.Column = 4;
                            excelImage.From.Row = nRowIndex - 1;
                            excelImage.SetSize(50, 50);
                            // 2x2 px space for better alignment
                            excelImage.From.ColumnOff = this.Pixel2MTU(2);
                            excelImage.From.RowOff = this.Pixel2MTU(2);
                        }
                        else
                        {
                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        }

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ContactPersonName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.SeasonName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.GarmentsName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.ColorRange; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.SizeRange; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.CuttingQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 18]; cell.Value = oItem.SweeingQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 19]; cell.Value = oItem.ShipmentQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 20]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                        nTotalCuttingQty = nTotalCuttingQty + oItem.CuttingQty;
                        nTotalSweeingQty = nTotalSweeingQty + oItem.SweeingQty;
                        nTotalShipmentQty = nTotalShipmentQty + oItem.ShipmentQty;
                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 16);
                    sEndCell = Global.GetExcelCellName(nEndRow, 16);
                    cell = sheet.Cells[nRowIndex, 16]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    //cell.Style.Font.UnderLine = true;
                    //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 17);
                    sEndCell = Global.GetExcelCellName(nEndRow, 17);
                    cell = sheet.Cells[nRowIndex, 17]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    //cell.Style.Font.UnderLine = true;
                    //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 18);
                    sEndCell = Global.GetExcelCellName(nEndRow, 18);
                    cell = sheet.Cells[nRowIndex, 18]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    //cell.Style.Font.UnderLine = true;
                    //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 19);
                    sEndCell = Global.GetExcelCellName(nEndRow, 19);
                    cell = sheet.Cells[nRowIndex, 19]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    //cell.Style.Font.UnderLine = true;
                    //cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    #region Grand Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 15]; cell.Merge = true;
                    cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = nTotalCuttingQty; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 18]; cell.Value = nTotalSweeingQty; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    cell = sheet.Cells[nRowIndex, 19]; cell.Value = nTotalShipmentQty; cell.Style.Font.Bold = true;
                    cell.Style.Font.UnderLine = true;
                    cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 20]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=MOB_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (ReportFormat == (int)EnumReportLayout.Factorywise)
            {
                if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                {
                    #region Factory Wise Deafult format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 20;  // Buyer Name                        
                        sheet.Column(5).Width = 20;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser
                        sheet.Column(10).Width = 11; // ONS Qty
                        sheet.Column(11).Width = 13; // ODS Qty
                        sheet.Column(12).Width = 13; // CM
                        sheet.Column(13).Width = 13; // Order Dt                        
                        nEndCol = 13;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Factory Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nFactoryID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0, nTotalONSQty = 0, nTotalODSQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nFactoryID != oItem.FactoryID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 11);
                                    cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Factory Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Factory Name : " + oItem.FactoryName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "ONS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "ODS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "CM Rate"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nFactoryID = oItem.FactoryID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ONSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ODSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.CMValue; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nTotalONSQty = nTotalONSQty + oItem.ONSQty;
                            nTotalODSQty = nTotalODSQty + oItem.ODSQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                        sEndCell = Global.GetExcelCellName(nEndRow, 10);
                        cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                        sEndCell = Global.GetExcelCellName(nEndRow, 11);
                        cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = nTotalONSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotalODSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else
                {
                    #region Factory Wise Custom Format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 30;  // Buyer Name                        
                        sheet.Column(5).Width = 30;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser                        
                        sheet.Column(10).Width = 13; // Order Dt                        
                        nEndCol = 10;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Factory Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nFactoryID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nFactoryID != oItem.FactoryID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Factory Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Factory Name : " + oItem.FactoryName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nFactoryID = oItem.FactoryID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
            else if (ReportFormat == (int)EnumReportLayout.PartyWise)
            {
                if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                {
                    #region Buyer Wise Deafult format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 20;  // Factory Name                        
                        sheet.Column(5).Width = 20;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser
                        sheet.Column(10).Width = 11; // ONS Qty
                        sheet.Column(11).Width = 13; // ODS Qty
                        sheet.Column(12).Width = 13; // CM
                        sheet.Column(13).Width = 13; // Order Dt                        
                        nEndCol = 13;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Buyer Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nBuyerID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0, nTotalONSQty = 0, nTotalODSQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nBuyerID != oItem.BuyerID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 11);
                                    cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Buyer Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Buyer Name : " + oItem.BuyerName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "ONS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "ODS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "CM Rate"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nBuyerID = oItem.BuyerID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ONSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ODSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.CMValue; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nTotalONSQty = nTotalONSQty + oItem.ONSQty;
                            nTotalODSQty = nTotalODSQty + oItem.ODSQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                        sEndCell = Global.GetExcelCellName(nEndRow, 10);
                        cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                        sEndCell = Global.GetExcelCellName(nEndRow, 11);
                        cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = nTotalONSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotalODSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else
                {
                    #region Buyer Wise Custom Format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 30;  // Factory Name                        
                        sheet.Column(5).Width = 30;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser                        
                        sheet.Column(10).Width = 13; // Order Dt                        
                        nEndCol = 10;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Buyer Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nBuyerID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nBuyerID != oItem.BuyerID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Buyer Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Buyer Name : " + oItem.BuyerName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nBuyerID = oItem.BuyerID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
            else if (ReportFormat == (int)EnumReportLayout.MerchandiserWise)
            {
                if ((Convert.ToInt32(oClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Default))
                {
                    #region Merchandiser Wise Deafult format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 20;  // Buyer Name                        
                        sheet.Column(5).Width = 20;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser
                        sheet.Column(10).Width = 11; // ONS Qty
                        sheet.Column(11).Width = 13; // ODS Qty
                        sheet.Column(12).Width = 13; // CM
                        sheet.Column(13).Width = 13; // Order Dt                        
                        nEndCol = 13;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Merchandiser Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nMerchandiserID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0, nTotalONSQty = 0, nTotalODSQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nMerchandiserID != oItem.MerchandiserID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 11);
                                    cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Factory Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Merchandiser Name : " + oItem.MerchandiserName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "ONS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = "ODS Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 12]; cell.Value = "CM Rate"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nMerchandiserID = oItem.MerchandiserID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.ONSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.ODSQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.CMValue; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nTotalONSQty = nTotalONSQty + oItem.ONSQty;
                            nTotalODSQty = nTotalODSQty + oItem.ODSQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                        sEndCell = Global.GetExcelCellName(nEndRow, 10);
                        cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 11);
                        sEndCell = Global.GetExcelCellName(nEndRow, 11);
                        cell = sheet.Cells[nRowIndex, 11]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 9]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = nTotalONSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = nTotalODSQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12, nRowIndex, 13]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
                else
                {
                    #region Merchandiser Wise Custom Format
                    int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                    ExcelRange cell;
                    ExcelFill fill;
                    OfficeOpenXml.Style.Border border;
                    using (var excelPackage = new ExcelPackage())
                    {
                        excelPackage.Workbook.Properties.Author = "ESimSol";
                        excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                        var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                        sheet.Name = "Order Recap Report";
                        sheet.Column(2).Width = 6;   // SL NO                        
                        sheet.Column(3).Width = 20;  // Style No                        
                        sheet.Column(4).Width = 30;  // Buyer Name                        
                        sheet.Column(5).Width = 30;  // Recap No                        
                        sheet.Column(6).Width = 11; // Order Qty
                        sheet.Column(7).Width = 13; // Shipment Dt
                        sheet.Column(8).Width = 11; // FOB
                        sheet.Column(9).Width = 30;  // Merchandiser                        
                        sheet.Column(10).Width = 13; // Order Dt                        
                        nEndCol = 10;

                        #region Report Header
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 1;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                        cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                        nRowIndex = nRowIndex + 2;

                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                        cell.Value = "Merchandiser Wise Order Recap Report"; cell.Style.Font.Bold = true;
                        cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                        nRowIndex = nRowIndex + 1;
                        #endregion

                        string sStartCell = "", sEndCell = "";
                        int nMerchandiserID = 0; int nCount = 0; bool IsTotalPrint = false;
                        double nTotalOrderQty = 0;
                        foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                        {
                            nCount++;
                            if (nMerchandiserID != oItem.MerchandiserID)
                            {
                                if (IsTotalPrint == true)
                                {
                                    #region Total
                                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                                    sEndCell = Global.GetExcelCellName(nEndRow, 6);
                                    cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                    cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                                    cell.Value = ""; cell.Style.Font.Bold = true;
                                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                    nRowIndex++;
                                    #endregion
                                }
                                nCount = 1;

                                #region Factory Name
                                nRowIndex = nRowIndex + 2;
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                                cell.Value = "Merchandiser Name : " + oItem.MerchandiserName; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                #endregion

                                #region Column Header
                                nRowIndex = nRowIndex + 1;
                                nStartRow = nRowIndex;
                                cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 5]; cell.Value = "Recap No"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Order Qty"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Shipment Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 8]; cell.Value = "FOB($)"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Order Dt"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                nRowIndex = nRowIndex + 1;
                                IsTotalPrint = true;
                                nMerchandiserID = oItem.MerchandiserID;
                                #endregion
                            }

                            #region Data
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.Numberformat.Format = "###0;(###0)";
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.OrderDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            nTotalOrderQty = nTotalOrderQty + oItem.OrderQty;
                            nEndRow = nRowIndex;
                            nRowIndex++;
                        }

                        #region Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        sStartCell = Global.GetExcelCellName(nStartRow + 1, 6);
                        sEndCell = Global.GetExcelCellName(nEndRow, 6);
                        cell = sheet.Cells[nRowIndex, 6]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        #region Grand Total
                        cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 5]; cell.Merge = true;
                        cell.Value = "Grand Total :"; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                        cell.Style.Font.UnderLine = true;
                        cell.Style.Font.UnderLineType = ExcelUnderLineType.DoubleAccounting;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7, nRowIndex, 10]; cell.Merge = true;
                        cell.Value = ""; cell.Style.Font.Bold = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        nRowIndex++;
                        #endregion

                        cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                        fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                        fill.BackgroundColor.SetColor(Color.White);

                        Response.ClearContent();
                        Response.BinaryWrite(excelPackage.GetAsByteArray());
                        Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Flush();
                        Response.End();
                    }
                    #endregion
                }
            }
            #endregion
        }
        public void OrderRecapBuyingStatementXL(string sParam, int ReportFormat)
        {
            #region Get Data
            Company oCompany = new Company();
            string sSQL = MakeQuery(sParam, ReportFormat);
            _oOrderRecapMgtReports = OrderRecapMgtReport.Gets(sSQL, ReportFormat, (int)Session[SessionInfo.currentUserID]);
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            #endregion

            #region Export To Excel
            if (ReportFormat == (int)EnumReportLayout.Buying_Commission_Statement)
            {
                #region Buying Commission Statement
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                    sheet.Name = "Buying Commission Statement";
                    sheet.Column(2).Width = 6;   // SL NO                        
                    sheet.Column(3).Width = 25;  // Factory Name                        
                    sheet.Column(4).Width = 25;  // Buyer Name                        
                    sheet.Column(5).Width = 20;  // Master LC NO                        
                    sheet.Column(6).Width = 20;  // LC Transfer No 
                    sheet.Column(7).Width = 20;  // Order No
                    sheet.Column(8).Width = 15;  // Order Qty
                    sheet.Column(9).Width = 15;  // FOB/Pcs
                    sheet.Column(10).Width = 15; // Total Amount
                    sheet.Column(11).Width = 15; // Factory FOB/Pcs
                    sheet.Column(12).Width = 15; // Factory Total
                    sheet.Column(13).Width = 15; // Buying Com/Pcs                    
                    sheet.Column(14).Width = 15; // Buying Com(%)
                    sheet.Column(15).Width = 15; // Buying Com
                    sheet.Column(16).Width = 15; // Delivery Date
                    sheet.Column(17).Width = 20; // Remarks
                    nEndCol = 17;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Buying Commission Statement"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    int nCount = 0;

                    #region Column Header
                    nRowIndex = nRowIndex + 1;
                    nStartRow = nRowIndex;
                    cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Transfer No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty(Pcs)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = "FOB/Pcs"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Factory FOB/Pcs"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Factory Total"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Buying Com/Pcs"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Buying Com(%)"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Buying Com"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    #region Data
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                    {
                        nCount++;                       
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCTransferNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        
                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.FactoryFOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.TotalFactoryAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.CommissionPerPc; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.CommissionInPercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.TotalCommission; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = "Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                    sEndCell = Global.GetExcelCellName(nEndRow, 8);
                    cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;                    
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                    sEndCell = Global.GetExcelCellName(nEndRow, 12);
                    cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                    sEndCell = Global.GetExcelCellName(nEndRow, 14);
                    cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                    sEndCell = Global.GetExcelCellName(nEndRow, 15);
                    cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    
                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (ReportFormat == (int)EnumReportLayout.Factorywise)
            {
                #region Factory Wise
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                    sheet.Name = "Buying Commission Statement";
                    sheet.Column(2).Width = 6;   // SL NO                        
                    sheet.Column(3).Width = 25;  // Style No
                    sheet.Column(4).Width = 25;  // Buyer Name                        
                    sheet.Column(5).Width = 20;  // Master LC NO                        
                    sheet.Column(6).Width = 20;  // LC Transfer No 
                    sheet.Column(7).Width = 20;  // Order No
                    sheet.Column(8).Width = 15;  // Order Qty
                    sheet.Column(9).Width = 15;  // FOB/Pcs
                    sheet.Column(10).Width = 15; // Total Amount
                    sheet.Column(11).Width = 15; // Factory FOB/Pcs
                    sheet.Column(12).Width = 15; // Factory Total
                    sheet.Column(13).Width = 15; // Buying Com/Pcs                    
                    sheet.Column(14).Width = 15; // Buying Com(%)
                    sheet.Column(15).Width = 15; // Buying Com
                    sheet.Column(16).Width = 15; // Delivery Date
                    sheet.Column(17).Width = 20; // Remarks 
                    nEndCol = 17;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Factory Wise Buying Commission Statement"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    int nFactoryID = 0; int nCount = 0; bool IsTotalPrint = false;                    
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                    {
                        nCount++;
                        if (nFactoryID != oItem.FactoryID)
                        {
                            if (IsTotalPrint == true)
                            {
                                #region Total
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                                cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                                sEndCell = Global.GetExcelCellName(nEndRow, 8);
                                cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                                sEndCell = Global.GetExcelCellName(nEndRow, 12);
                                cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                                sEndCell = Global.GetExcelCellName(nEndRow, 14);
                                cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                                sEndCell = Global.GetExcelCellName(nEndRow, 15);
                                cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nCount = 1;

                            #region Factory Name
                            nRowIndex = nRowIndex + 2;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "Factory Name : " + oItem.FactoryName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Column Header
                            nRowIndex = nRowIndex + 1;
                            nStartRow = nRowIndex;
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Transfer No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty(Pcs)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Factory FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Factory Total"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Buying Com/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Buying Com(%)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Buying Com"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex = nRowIndex + 1;
                            IsTotalPrint = true;
                            nFactoryID = oItem.FactoryID;
                            #endregion
                        }

                        #region Data
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCTransferNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.FactoryFOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.TotalFactoryAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.CommissionPerPc; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.CommissionInPercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.TotalCommission; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion
                        
                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                    sEndCell = Global.GetExcelCellName(nEndRow, 8);
                    cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                    sEndCell = Global.GetExcelCellName(nEndRow, 12);
                    cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                    sEndCell = Global.GetExcelCellName(nEndRow, 14);
                    cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                    sEndCell = Global.GetExcelCellName(nEndRow, 15);
                    cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (ReportFormat == (int)EnumReportLayout.PartyWise)
            {
                #region Buyer Wise
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                    sheet.Name = "Buying Commission Statement";
                    sheet.Column(2).Width = 6;   // SL NO                        
                    sheet.Column(3).Width = 25;  // Factory Name
                    sheet.Column(4).Width = 25;  // Style No                      
                    sheet.Column(5).Width = 20;  // Master LC NO                        
                    sheet.Column(6).Width = 20;  // LC Transfer No 
                    sheet.Column(7).Width = 20;  // Order No
                    sheet.Column(8).Width = 15;  // Order Qty
                    sheet.Column(9).Width = 15;  // FOB/Pcs
                    sheet.Column(10).Width = 15; // Total Amount
                    sheet.Column(11).Width = 15; // Factory FOB/Pcs
                    sheet.Column(12).Width = 15; // Factory Total
                    sheet.Column(13).Width = 15; // Buying Com/Pcs                    
                    sheet.Column(14).Width = 15; // Buying Com(%)
                    sheet.Column(15).Width = 15; // Buying Com
                    sheet.Column(16).Width = 15; // Delivery Date
                    sheet.Column(17).Width = 20; // Remarks 
                    nEndCol = 17;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Buyer Wise Buying Commission Statement"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    int nBuyerID = 0; int nCount = 0; bool IsTotalPrint = false;                    
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                    {
                        nCount++;
                        if (nBuyerID != oItem.BuyerID)
                        {
                            if (IsTotalPrint == true)
                            {
                                #region Total
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                                cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                                sEndCell = Global.GetExcelCellName(nEndRow, 8);
                                cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                                sEndCell = Global.GetExcelCellName(nEndRow, 12);
                                cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                                sEndCell = Global.GetExcelCellName(nEndRow, 14);
                                cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                                sEndCell = Global.GetExcelCellName(nEndRow, 15);
                                cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nCount = 1;

                            #region Buyer Name
                            nRowIndex = nRowIndex + 2;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "Buyer Name : " + oItem.BuyerName; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Column Header
                            nRowIndex = nRowIndex + 1;
                            nStartRow = nRowIndex;
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Transfer No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty(Pcs)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Factory FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Factory Total"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Buying Com/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Buying Com(%)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Buying Com"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex = nRowIndex + 1;
                            IsTotalPrint = true;
                            nBuyerID = oItem.BuyerID;
                            #endregion
                        }

                        #region Data
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCTransferNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.FactoryFOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.TotalFactoryAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.CommissionPerPc; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.CommissionInPercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.TotalCommission; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                    sEndCell = Global.GetExcelCellName(nEndRow, 8);
                    cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                    sEndCell = Global.GetExcelCellName(nEndRow, 12);
                    cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                    sEndCell = Global.GetExcelCellName(nEndRow, 14);
                    cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                    sEndCell = Global.GetExcelCellName(nEndRow, 15);
                    cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            else if (ReportFormat == (int)EnumReportLayout.ShipmentDateWise)
            {
                #region Shuipemt Wise
                int nRowIndex = 2, nStartRow = 2, nEndRow = 0, nStartCol = 2, nEndCol = 0;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;
                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap Report");
                    sheet.Name = "Buying Commission Statement";
                    sheet.Column(2).Width = 6;   // SL NO                        
                    sheet.Column(3).Width = 25;  // Style No
                    sheet.Column(4).Width = 25;  // Buyer Name                        
                    sheet.Column(5).Width = 20;  // Master LC NO                        
                    sheet.Column(6).Width = 20;  // LC Transfer No 
                    sheet.Column(7).Width = 20;  // Order No
                    sheet.Column(8).Width = 15;  // Order Qty
                    sheet.Column(9).Width = 15;  // FOB/Pcs
                    sheet.Column(10).Width = 15; // Total Amount
                    sheet.Column(11).Width = 15; // Factory FOB/Pcs
                    sheet.Column(12).Width = 15; // Factory Total
                    sheet.Column(13).Width = 15; // Buying Com/Pcs                    
                    sheet.Column(14).Width = 15; // Buying Com(%)
                    sheet.Column(15).Width = 15; // Buying Com
                    sheet.Column(16).Width = 15; // Delivery Date
                    sheet.Column(17).Width = 20; // Remarks 
                    nEndCol = 17;

                    #region Report Header
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Address; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 1;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = oCompany.Phone + ";  " + oCompany.Email + ";  " + oCompany.WebAddress; cell.Style.Font.Bold = false;
                    cell.Style.Font.Size = 12; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    nRowIndex = nRowIndex + 2;

                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                    cell.Value = "Factory Wise Buying Commission Statement"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.Font.UnderLine = true; cell.Style.Font.UnderLineType = ExcelUnderLineType.Single;
                    nRowIndex = nRowIndex + 1;
                    #endregion

                    string sStartCell = "", sEndCell = "";
                    DateTime dShipmentDate = DateTime.MinValue; int nCount = 0; bool IsTotalPrint = false;
                    foreach (OrderRecapMgtReport oItem in _oOrderRecapMgtReports)
                    {
                        nCount++;
                        if (dShipmentDate.ToString("dd MMM yyyy") != oItem.ShipmentDate.ToString("dd MMM yyyy"))
                        {
                            if (IsTotalPrint == true)
                            {
                                #region Total
                                cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                                cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                                sEndCell = Global.GetExcelCellName(nEndRow, 8);
                                cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                                sEndCell = Global.GetExcelCellName(nEndRow, 10);
                                cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                                sEndCell = Global.GetExcelCellName(nEndRow, 12);
                                cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                                sEndCell = Global.GetExcelCellName(nEndRow, 14);
                                cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                                sEndCell = Global.GetExcelCellName(nEndRow, 15);
                                cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                                cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                                cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                                border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                                nRowIndex++;
                                #endregion
                            }
                            nCount = 1;

                            #region Factory Name
                            nRowIndex = nRowIndex + 2;
                            cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, nEndCol]; cell.Merge = true;
                            cell.Value = "Shipment Date : " + oItem.ShipmentDateInString; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                            #endregion

                            #region Column Header
                            nRowIndex = nRowIndex + 1;
                            nStartRow = nRowIndex;
                            cell = sheet.Cells[nRowIndex, 2]; cell.Value = "SL NO"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 3]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 4]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 5]; cell.Value = "LC No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 6]; cell.Value = "Transfer No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 7]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 8]; cell.Value = "Qty(Pcs)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 9]; cell.Value = "FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 10]; cell.Value = "Total Amount"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 11]; cell.Value = "Factory FOB/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 12]; cell.Value = "Factory Total"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 13]; cell.Value = "Buying Com/Pcs"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 14]; cell.Value = "Buying Com(%)"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 15]; cell.Value = "Buying Com"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 16]; cell.Value = "Delivery Date"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            cell = sheet.Cells[nRowIndex, 17]; cell.Value = "Remarks"; cell.Style.Font.Bold = true;
                            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                            nRowIndex = nRowIndex + 1;
                            IsTotalPrint = true;
                            dShipmentDate = oItem.ShipmentDate;
                            #endregion
                        }

                        #region Data
                        cell = sheet.Cells[nRowIndex, 2]; cell.Value = nCount.ToString(); cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.Numberformat.Format = "###0;(###0)";
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 3]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 4]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 5]; cell.Value = oItem.MasterLCNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 6]; cell.Value = oItem.LCTransferNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 7]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 8]; cell.Value = oItem.OrderQty; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 9]; cell.Value = oItem.FOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 10]; cell.Value = oItem.TotalAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 11]; cell.Value = oItem.FactoryFOB; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 12]; cell.Value = oItem.TotalFactoryAmount; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 13]; cell.Value = oItem.CommissionPerPc; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 14]; cell.Value = oItem.CommissionInPercent; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 15]; cell.Value = oItem.TotalCommission; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        cell.Style.Numberformat.Format = "#,##0.00;(#,##0.00)";
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 16]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[nRowIndex, 17]; cell.Value = oItem.Remarks; cell.Style.Font.Bold = false; cell.Style.WrapText = true;
                        cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Left; cell.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                        border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                        #endregion

                        nEndRow = nRowIndex;
                        nRowIndex++;
                    }

                    #region Total
                    cell = sheet.Cells[nRowIndex, nStartCol, nRowIndex, 7]; cell.Merge = true;
                    cell.Value = "Sub Total :"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 8);
                    sEndCell = Global.GetExcelCellName(nEndRow, 8);
                    cell = sheet.Cells[nRowIndex, 8]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 10);
                    sEndCell = Global.GetExcelCellName(nEndRow, 10);
                    cell = sheet.Cells[nRowIndex, 10]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 12);
                    sEndCell = Global.GetExcelCellName(nEndRow, 12);
                    cell = sheet.Cells[nRowIndex, 12]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 14);
                    sEndCell = Global.GetExcelCellName(nEndRow, 14);
                    cell = sheet.Cells[nRowIndex, 14]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    sStartCell = Global.GetExcelCellName(nStartRow + 1, 15);
                    sEndCell = Global.GetExcelCellName(nEndRow, 15);
                    cell = sheet.Cells[nRowIndex, 15]; cell.Formula = "SUM(" + sStartCell + ":" + sEndCell + ")"; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    cell.Style.Numberformat.Format = "#,##0;(#,##0)";
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 16]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[nRowIndex, 17]; cell.Value = ""; cell.Style.Font.Bold = true;
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                    border = cell.Style.Border; border.Top.Style = border.Bottom.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;
                    nRowIndex++;
                    #endregion

                    cell = sheet.Cells[1, 1, nRowIndex, nEndCol + 1];
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid;
                    fill.BackgroundColor.SetColor(Color.White);

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=Recap_Report.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }
                #endregion
            }
            #endregion
        }

        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        public string MakeQuery(string sTemp, int nReportFormat)
        {

            string sReturnMain = "SELECT OrderRecapID, OrderRecapNo, BuyerID, AgentID ,TechnicalSheetID,ISNULL(FabricID,0), ISNULL(ProductionFactoryID,0), ISNULL(MerchandiserID,0),ShipmentDate, OrderDate, ISNULL(CMValue,0)  FROM OrderRecap";
            string sReturn = "";

            /*Order Date Set*/
            int nOrderDate = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dOrderStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dOrderEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            /*Shipment Date Set*/
            int nShipmentDate = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);


            string sOrderNo = sTemp.Split('~')[6];
            string sStyleNo = sTemp.Split('~')[7];
            string sBuyerIDs = sTemp.Split('~')[8];
            int nProductCatagoryID = Convert.ToInt32(sTemp.Split('~')[9]);

            /*Order Qty Range */
            int ncboOrderQty = Convert.ToInt32(sTemp.Split('~')[10]);
            double OrderStartQty = Convert.ToDouble(sTemp.Split('~')[11]);
            double OrderEndQty = Convert.ToDouble(sTemp.Split('~')[12]);

            /* FOB Range*/
            int ncboFOB = Convert.ToInt32(sTemp.Split('~')[13]);
            double StartFOB = Convert.ToDouble(sTemp.Split('~')[14]);
            double EndFOB = Convert.ToDouble(sTemp.Split('~')[15]);
            string sStatus = sTemp.Split('~')[16];
            string sMerchandiserIDs = sTemp.Split('~')[17];
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[18]);
            string sAgentIDs = sTemp.Split('~')[19];
            int nIsRunning = Convert.ToInt32(sTemp.Split('~')[20]);
            int nStyleDeptID = Convert.ToInt32(sTemp.Split('~')[21]);
            int nSubGender = Convert.ToInt32(sTemp.Split('~')[22]);
            int nTSType = Convert.ToInt32(sTemp.Split('~')[23]);
            int nBUID = Convert.ToInt32(sTemp.Split('~')[24]);

            #region Style No
            if (sStyleNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "   TechnicalSheetID  IN (SELECT TS.TechnicalSheetID FROM TechnicalSheet AS TS WHERE  TS.StyleNo ='" + sStyleNo + "')";
            }
            #endregion

            #region Prouduct Catagory

            if (nProductCatagoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "ProductID IN (SELECT ProductID FROM View_Product WHERE ProductCategoryID =" + nProductCatagoryID + ")";

            }

            #endregion

            #region Order No
            if (sOrderNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapNo ='" + sOrderNo + "'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN  (" + sBuyerIDs + ")";
            }

            #endregion

            #region Agent Name

            if (sAgentIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " AgentID IN  (" + sAgentIDs + ")";
            }

            #endregion

            #region IsRunning
            if (nIsRunning == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapID IN (SELECT HH.OrderRecapID FROM OrderRecap AS HH  WHERE HH.IsShippedOut=0)";
            }
            #endregion

            #region Style Dept
            if (nStyleDeptID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "   TechnicalSheetID  IN (SELECT TS.TechnicalSheetID FROM TechnicalSheet AS TS WHERE  TS.Dept = " + nStyleDeptID + ")";
            }
            #endregion

            #region Sub Gender
            if (nSubGender != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "   TechnicalSheetID  IN (SELECT TS.TechnicalSheetID FROM TechnicalSheet AS TS WHERE  TS.SubGender = " + nSubGender + ")";
            }
            #endregion

            #region TS Type
            if (nTSType > -1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "   TechnicalSheetID  IN (SELECT TS.TechnicalSheetID FROM TechnicalSheet AS TS WHERE  TS.TSType = " + nTSType + ")";
            }
            #endregion


            #region Merchander Wise

            if (sMerchandiserIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID IN  (" + sMerchandiserIDs + ")";
            }

            #endregion

            #region Business Session

            if (nSessionID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID = " + nSessionID;

            }

            #endregion

            #region BU

            if (nBUID> 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + nBUID;
            }

            #endregion
            #region Active 
            Global.TagSQL(ref sReturn);
            sReturn = sReturn + " IsActive = 1"; //only active Order Recap
            #endregion

            #region Status

            if (sStatus != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapStatus IN (" + sStatus + ")";

            }

            #endregion

            #region Order Date
            if (nOrderDate > 0)
            {
                if (nOrderDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dOrderStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate != '" + dOrderStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate > '" + dOrderStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate < '" + dOrderStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate>= '" + dOrderStartDate.ToString("dd MMM yyyy") + "' AND OrderDate < '" + dOrderEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nOrderDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate< '" + dOrderStartDate.ToString("dd MMM yyyy") + "' OR OrderDate > '" + dOrderEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }

            }
            #endregion

            #region Shipement Date
            if (nShipmentDate > 0)
            {
                if (nShipmentDate == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDate == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }

            }
            #endregion

            #region Order Qty Wise
            if (ncboOrderQty > 0)
            {
                if (ncboOrderQty == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  = " + OrderStartQty + ")";
                }
                if (ncboOrderQty == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  != " + OrderStartQty + ")";
                }
                if (ncboOrderQty == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  > " + OrderStartQty + ")";
                }
                if (ncboOrderQty == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  < " + OrderStartQty + ")";
                }
                if (ncboOrderQty == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  >=" + OrderStartQty + " AND ISNULL(SUM(Quantity),0)  < " + OrderEndQty + ")";
                }
                if (ncboOrderQty == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  ISNULL(SUM(Quantity),0)  <" + OrderStartQty + " OR ISNULL(SUM(Quantity),0)  > " + OrderEndQty + ")";
                }

            }
            #endregion

            #region FOB Wise
            if (ncboFOB > 0)
            {
                if (ncboFOB == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) =" + StartFOB + ")";
                }
                if (ncboFOB == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) !=" + StartFOB + ")";

                }
                if (ncboFOB == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) >" + StartFOB + ")";

                }
                if (ncboFOB == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) <" + StartFOB + ")";

                }
                if (ncboFOB == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) >=" + StartFOB + " AND (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) <" + EndFOB + ")";

                }
                if (ncboFOB == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + "OrderRecapID IN( SELECT OrderRecapID  FROM OrderRecapDetail  GROUP By OrderRecapID Having  (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) <" + StartFOB + " OR (ISNULL(SUM(UnitPrice),0) / Count(OrderRecapID)) >" + EndFOB + ")";

                }

            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion

            #region Remove InActive Order
            Global.TagSQL(ref sReturn);
            sReturn += " OrderRecapID IN (SELECT SO.OrderRecapID FROM OrderRecap AS SO WHERE SO.IsActive=1)";
            #endregion

            sReturnMain = sReturnMain + sReturn;


            return sReturnMain;

        }

        //#region LoadStatus
        //[HttpGet]
        //public JsonResult LoadStatus()
        //{
        //    List<enumloadStatus> oTempenumloadStatuss = new List<enumloadStatus>();
        //    enumloadStatus oenumloadStatus = new enumloadStatus();
        //    try
        //    {
        //        foreach (int oItem in Enum.GetValues(typeof(EnumOrderRecapStatus)))
        //        {
        //            oenumloadStatus = new enumloadStatus();
        //            oenumloadStatus.id = oItem;
        //            oenumloadStatus.Value = ((EnumOrderRecapStatus)oItem).ToString();
        //            oTempenumloadStatuss.Add(oenumloadStatus);
        //        }

        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oTempenumloadStatuss);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}

        //#endregion

        #endregion

        #region SEARCHING
        public ActionResult OrderRecapSearch()
        {
            OrderRecap oOrderRecap = new OrderRecap();
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Admin_User)
            {
               oOrderRecap.Employees = Employee.Gets((int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sSQL = "SELECT * FROM View_Employee WHERE EmployeeID = (SELECT EmployeeID FROM Users WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
                oOrderRecap.Employees = Employee.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            oOrderRecap.OrderRecapList = new List<OrderRecap>();
            oOrderRecap.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            oOrderRecap.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            //oRecipes = Recipe.Gets((int)Session[SessionInfo.currentUserID]);
            return PartialView(oOrderRecap);
        }
        private string GetSQL(string sTemp)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            int nCreateDateCom = Convert.ToInt32(sTemp.Split('~')[0]);
            DateTime dSOStartDate = Convert.ToDateTime(sTemp.Split('~')[1]);
            DateTime dSOEndDate = Convert.ToDateTime(sTemp.Split('~')[2]);

            int nChallanDateCom = Convert.ToInt32(sTemp.Split('~')[3]);
            DateTime dIssueStartDate = Convert.ToDateTime(sTemp.Split('~')[4]);
            DateTime dIssueEndDate = Convert.ToDateTime(sTemp.Split('~')[5]);

            string sStNo = sTemp.Split('~')[6];
            string sSONo = sTemp.Split('~')[9];
            string sBuyerIDs = sTemp.Split('~')[7];
            string sMerchandiserIDs = sTemp.Split('~')[8];

            int IsCheckedApproved = Convert.ToInt32(sTemp.Split('~')[10]);
            int IsCheckedNotApproved = Convert.ToInt32(sTemp.Split('~')[11]);
            int nProductCatagoryID = Convert.ToInt32(sTemp.Split('~')[12]);
            int nSessionID = Convert.ToInt32(sTemp.Split('~')[13]);
            string sFactoryIDs = sTemp.Split('~')[14];
            string sOrderTypes = sTemp.Split('~')[15];
            int nBrandID = Convert.ToInt32(sTemp.Split('~')[16]);
            int nShipmentDateCom = Convert.ToInt32(sTemp.Split('~')[17]);
            DateTime dShipmentStartDate = Convert.ToDateTime(sTemp.Split('~')[18]);
            DateTime dShipmentEndDate = Convert.ToDateTime(sTemp.Split('~')[19]);
            bool bIsRunning = Convert.ToBoolean(sTemp.Split('~')[20]);
            int BUID = Convert.ToInt32(sTemp.Split('~')[21]);
            string sReturn1 = "SELECT * FROM View_OrderRecap";
            string sReturn = "";

            #region OrderType
            if (sOrderTypes == null || sOrderTypes == "undefined")
            {
                sOrderTypes = "";
            }

            if (sOrderTypes != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderType IN(" + sOrderTypes + ")";
            }

            #endregion

            #region Style No

            if (sStNo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " StyleNo = '" + sStNo + "'";
            }
            #endregion

            #region Buyer Name

            if (sBuyerIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID IN (" + sBuyerIDs + ")";
            }
            #endregion

            #region Factory wise
            if (sFactoryIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID IN (" + sFactoryIDs + ")";
            }
            #endregion

            #region Merchandiser Name

            if (sMerchandiserIDs != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " MerchandiserID  IN (" + sMerchandiserIDs + ")";
            }
            #endregion

            #region Sale Order No

            if (sSONo != "")
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderRecapNo = '" + sSONo + "'";
            }

            #endregion

            #region Product Catagory

            if (nProductCatagoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM View_TechnicalSheet WHERE ProductID IN (SELECT ProductID FROM View_Product WHERE ProductCategoryID =" + nProductCatagoryID + "))";
            }

            #endregion

            #region Session
            if (nSessionID > 0)
            {

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID =" + nSessionID;

            }
            #endregion

            #region Brand
            if (nBrandID > 0)
            {

                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BrandID =" + nBrandID;

            }
            #endregion

            #region Order Date Wise
            if (nCreateDateCom > 0)
            {
                if (nCreateDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate = '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate != '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate > '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate < '" + dSOStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate>= '" + dSOStartDate.ToString("dd MMM yyyy") + "' AND OrderDate < '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nCreateDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " OrderDate< '" + dSOStartDate.ToString("dd MMM yyyy") + "' OR OrderDate > '" + dSOEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region Issue Date Wise
            if (nChallanDateCom > 0)
            {
                if (nChallanDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  = CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nChallanDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  != CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nChallanDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  > CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nChallanDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106))  < CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nChallanDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
                if (nChallanDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " CONVERT(DATE,CONVERT(VARCHAR(12),DBServerDateTime,106)) NOT BETWEEN CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueStartDate.ToString("dd MMM yyyy") + "',106)) AND CONVERT(DATE,CONVERT(VARCHAR(12),'" + dIssueEndDate.ToString("dd MMM yyyy") + "',106))";
                }
            }

            #endregion

            #region Shipment  Wise
            if (nShipmentDateCom > 0)
            {
                if (nShipmentDateCom == 1)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 2)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 3)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 4)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 5)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
                if (nShipmentDateCom == 6)
                {
                    Global.TagSQL(ref sReturn);
                    sReturn = sReturn + " ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                }
            }

            #endregion

            #region IsApproved
            if (IsCheckedApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApproveBy != 0";
            }
            #endregion

            #region IsNotApproved
            if (IsCheckedNotApproved == 1)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ApproveBy = 0";
            }
            #endregion

            #region Is running
            if (bIsRunning == true)
            {
                Global.TagSQL(ref sReturn);                
                sReturn = sReturn + " OrderRecapID IN (SELECT HH.OrderRecapID FROM OrderRecap AS HH  WHERE HH.IsShippedOut=0)";
            }
            #endregion

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn += " TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion

            #region Avoid Hidden Style
            Global.TagSQL(ref sReturn);
            sReturn += " TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
            #endregion

            if (BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID ="+BUID;
            }
            sReturn = sReturn1 + sReturn + " ORDER BY BuyerID, OrderRecapID";
            return sReturn;
        }

        [HttpGet]
        public JsonResult Gets(string Temp)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            try
            {
                string sSQL = GetSQL(Temp);
                oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderRecaps = new List<OrderRecap>();
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult GetCurrencyRate(int nCurrencyID)
        {
            List<CurrencyConversion> _oCurrencyConversions = new List<CurrencyConversion>();
            CurrencyConversion oCurrencyConversion = new CurrencyConversion();
            try
            {

                _oCurrencyConversions = CurrencyConversion.GetsByFromCurrency(nCurrencyID, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oCurrencyConversion = new CurrencyConversion();

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oCurrencyConversions);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult Amount(double value1, double ts, double value2)
        {
            double sAmount = 0;
            sAmount = value1 * value2;
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(sAmount);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ProductBuyerGet(int id)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            TechnicalSheet oTechnicalSheet = new TechnicalSheet();
            oOrderRecap.TechnicalSheet = oTechnicalSheet.Get(id, (int)Session[SessionInfo.currentUserID]);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult OrderRecapSearchByNo(OrderRecap oOrderRecap)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            try
            {
                oOrderRecaps = OrderRecap.Gets("SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%'", (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = ex.Message;
                oOrderRecaps.Add(oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }
        #region Style Search Picker
        [HttpPost]
        public JsonResult StyleSearch(TechnicalSheet oTechnicalSheet)
        {
            _oTechnicalSheets = new List<TechnicalSheet>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            try
            {
                string sSQL = "SELECT * FROM View_TechnicalSheet WHERE DevelopmentStatus >= " + (int)EnumDevelopmentStatus.ApprovedDone;
                if (oTechnicalSheet.BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + oTechnicalSheet.BUID;
                }
                if (!string.IsNullOrEmpty(oTechnicalSheet.StyleNo))
                {
                    sSQL += " AND StyleNo LIKE'%" + oTechnicalSheet.StyleNo + "%'";
                }
                _oTechnicalSheets = TechnicalSheet.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);   
            }
            catch (Exception ex)
            {
                _oTechnicalSheet = new TechnicalSheet();
                _oTechnicalSheet.ErrorMessage = ex.Message;
                _oTechnicalSheets.Add(_oTechnicalSheet);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oTechnicalSheets);
            return Json(sjson, JsonRequestBehavior.AllowGet);
            
        }

        [HttpPost]
        public JsonResult GetClass()
        {
            _oGarmentsClasses = new List<GarmentsClass>();
            try
            {
                string sSql = "";
                sSql = "select * from GarmentsClass where ParentClassID =1";
                _oGarmentsClasses = GarmentsClass.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oGarmentsClass = new GarmentsClass();
                _oGarmentsClass.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oGarmentsClasses);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult Refresh(string Search_creiteria)
        {
            try
            {
                string sSql = "";
                string sfirst_crieateria = Search_creiteria.Split('~')[0];

                _oTechnicalSheets = new List<TechnicalSheet>();
                _oOrderRecaps = new List<OrderRecap>();
                if (sfirst_crieateria == "StyleNo")
                {
                    string StyleNo = Search_creiteria.Split('~')[1];
                    sSql = "select * from View_OrderRecap where  OrderType =" + (int)EnumOrderType.BulkOrder + " AND TechnicalSheetID =(Select TechnicalSheetID from TechnicalSheet where StyleNo = '" + StyleNo + "')";
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    if (_oOrderRecaps.Count() <= 0)
                    {
                        if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                        {
                            _oTechnicalSheets = new List<TechnicalSheet>();
                            string sTSql = "SELECT * FROM View_TechnicalSheet WHERE StyleNo = '" + StyleNo + "'  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                            _oTechnicalSheets = TechnicalSheet.Gets(sTSql, (int)Session[SessionInfo.currentUserID]);
                            _oTechnicalSheet = _oTechnicalSheets[0];
                        }
                        else
                        {
                            _oTechnicalSheet = _oTechnicalSheet.GetByStyleNo(StyleNo, (int)Session[SessionInfo.currentUserID]);
                        }
                        if (_oTechnicalSheet.TechnicalSheetID > 0)
                        {
                            _oOrderRecap = new OrderRecap();
                            _oOrderRecap.OrderRecapID = 0;
                            _oOrderRecap.OrderRecapNo = "";
                            _oOrderRecap.TechnicalSheetID = _oTechnicalSheet.TechnicalSheetID;
                            _oOrderRecap.ProductID = _oTechnicalSheet.ProductID;
                            _oOrderRecap.BuyerID = _oTechnicalSheet.BuyerID;
                            _oOrderRecap.StyleNo = _oTechnicalSheet.StyleNo;
                            _oOrderRecap.BuyerName = _oTechnicalSheet.BuyerName;
                            _oOrderRecap.ColorRange = _oTechnicalSheet.ColorRange;
                            _oOrderRecap.SizeRange = _oTechnicalSheet.SizeRange;
                            _oOrderRecap.ProductName = _oTechnicalSheet.ProductName;
                            _oOrderRecap.FabricID = _oTechnicalSheet.YarnCategoryID;
                            _oOrderRecap.FabricName = _oTechnicalSheet.FabricDescription;
                            _oOrderRecap.BusinessSessionID = _oTechnicalSheet.BusinessSessionID;
                            _oOrderRecap.Description = "No Sale Order Made";
                            _oOrderRecaps.Add(_oOrderRecap);
                        }
                    }


                }
                else if (sfirst_crieateria == "OrderRecap")
                {
                    string OrderRecapNo = Search_creiteria.Split('~')[1];
                    sSql = "Select * from View_OrderRecap where OrderRecapNo ='" + OrderRecapNo + "'";
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += "  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                }
                else if (sfirst_crieateria == "BuyerID")
                {
                    string sBuyerID = Search_creiteria.Split('~')[1];
                    int BuyerID = Convert.ToInt32(sBuyerID);
                    sSql = "Select * from View_OrderRecap where   OrderType =" + (int)EnumOrderType.BulkOrder + " AND  BuyerID =" + BuyerID;
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += "  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    sSql = "select * from View_TechnicalSheet where(TechnicalSheetID NoT IN (Select TechnicalSheetID from OrderRecap where  OrderType =" + (int)EnumOrderType.BulkOrder + " AND    BuyerID =" + BuyerID + ")) AND BuyerID =" + BuyerID;
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += "  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oTechnicalSheets = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    foreach (TechnicalSheet item in _oTechnicalSheets)
                    {
                        _oOrderRecap = new OrderRecap();
                        _oOrderRecap.OrderRecapID = 0;
                        _oOrderRecap.OrderRecapNo = "";
                        _oOrderRecap.TechnicalSheetID = item.TechnicalSheetID;
                        _oOrderRecap.ProductID = item.ProductID;
                        _oOrderRecap.BuyerID = item.BuyerID;
                        _oOrderRecap.StyleNo = item.StyleNo;
                        _oOrderRecap.BuyerName = item.BuyerName;
                        _oOrderRecap.ColorRange = item.ColorRange;
                        _oOrderRecap.SizeRange = item.SizeRange;
                        _oOrderRecap.ProductName = item.ProductName;
                        _oOrderRecap.FabricID = item.YarnCategoryID;
                        _oOrderRecap.FabricName = item.FabricDescription;
                        _oOrderRecap.BusinessSessionID = _oTechnicalSheet.BusinessSessionID;
                        _oOrderRecap.Description = "No Sale Order Made";
                        _oOrderRecaps.Add(_oOrderRecap);
                    }

                }
                else
                {
                    string sGarmentsClassID = Search_creiteria.Split('~')[1];
                    string sDept = Search_creiteria.Split('~')[3];
                    int nGarmentsClassID = Convert.ToInt32(sGarmentsClassID);
                    int nDeptID = Convert.ToInt32(sDept);

                    sSql = "SELECT * FROM View_OrderRecap WHERE TechnicalSheetID IN (SELECT Distinct TechnicalSheetID FROM TechnicalSheet WHERE GarmentsClassID = " + nGarmentsClassID + " AND Dept = " + nDeptID + ")";
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += "  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                    sSql = "SELECT * FROM View_TechnicalSheet WHERE GarmentsClassID = " + nGarmentsClassID + " AND Dept = " + nDeptID + " AND TechnicalSheetID NOT IN (SELECT TechnicalSheetID FROM View_OrderRecap WHERE TechnicalSheetID IN (SELECT  TechnicalSheetID FROM TechnicalSheet WHERE GarmentsClassID = " + nGarmentsClassID + " AND Dept = " + nDeptID + "))";
                    #region User Set
                    if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                    {

                        sSql += "  AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                    }
                    #endregion
                    _oTechnicalSheets = TechnicalSheet.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
                    foreach (TechnicalSheet item in _oTechnicalSheets)
                    {
                        _oOrderRecap = new OrderRecap();
                        _oOrderRecap.OrderRecapID = 0;
                        _oOrderRecap.OrderRecapNo = "";
                        _oOrderRecap.TechnicalSheetID = item.TechnicalSheetID;
                        _oOrderRecap.ProductID = item.ProductID;
                        _oOrderRecap.BuyerID = item.BuyerID;
                        _oOrderRecap.StyleNo = item.StyleNo;
                        _oOrderRecap.BuyerName = item.BuyerName;
                        _oOrderRecap.ColorRange = item.ColorRange;
                        _oOrderRecap.SizeRange = item.SizeRange;
                        _oOrderRecap.ProductName = item.ProductName;
                        _oOrderRecap.FabricID = item.YarnCategoryID;
                        _oOrderRecap.FabricName = item.FabricDescription;
                        _oOrderRecap.BusinessSessionID = _oTechnicalSheet.BusinessSessionID;
                        _oOrderRecap.Description = "No Sale Order Made";
                        _oOrderRecaps.Add(_oOrderRecap);
                    }
                    //sSql = "SELECT CASE WHEN wq.OrderRecapID IS null THEN 0 ELSE wq.OrderRecapID END OrderRecapID, CASE WHEN wq.OrderRecapNo IS NULL THEN '' ELSE wq.OrderRecapNo END OrderRecapNo,CASE WHEN wq.TechnicalSheetID is null THEN wq.TSID ELSE wq.TechnicalSheetID END TechnicalSheetID,CASE WHEN wq.ProductID IS NULL THEN wq.PID ELSE wq.ProductID END ProductID,CASE WHEN wq.BuyerID IS NULL THEN wq.BID ELSE wq.BuyerID END BuyerID,BuyerContactPersonID,MerchandiserID,OrderType , CurrencyID, CurrencyName, UnitPrice, TotalQuantity, GG, Count, CurrencySymbol, Incoterms,TransportType, AgentID,CASE WHEN wq.FabricID IS NULL THEN wq.YCID ELSE wq.FabricID END FabricID,CASE WHEN wq.Description IS NULl THEN 'No Sale Order Made' ELSE wq.Description END Description,OrderDate, ShipmentDate,  ApproveBy, ApproveDate, SLNo,CollectionNo,CASE WHEN wq.StyleNo is null THEN wq.SN ELSE wq.StyleNo END StyleNo,CASE WHEN wq.BuyerName is null THEN wq.BN ELSE wq.BuyerName END BuyerName,MerchandiserName,CASE WHEN wq.ColorRange IS NULL THEN wq.CR ELSE wq.ColorRange END ColorRange,CASE WHEN wq.SizeRange IS NULL THEN wq.SR ELSE wq.SizeRange END SizeRange,CASE WHEN wq.FabricName IS NULL THEN wq.YCN ELSE wq.FabricName END FabricName,BuyerContactPerson, ApproveByName, CASE WHEN wq.ProductName  IS NULL THEN wq.PN ELSE wq.ProductName END ProductName,AgentName,Amount FROM (SELECT SO.*,TS.TechnicalSheetID AS TSID,TS.StyleNo as SN, TS.BuyerName as BN, TS.ProductID as PID, TS.BuyerID as BID, TS.YarnCategoryID as YCID, TS.ColorRange as CR, TS.SizeRange as SR, TS.ProductName as PN, TS.FabricDescription as YCN FROM View_OrderRecap  SO  FULL JOIN View_TechnicalSheet TS ON SO.TechnicalSheetID=TS.TechnicalSheetID WHERE (TS.Dept= " + DeptID + " OR TS.GarmentsClassID= " + GarmentsClassID + ") AND SO.OrderType = " + (int)EnumOrderRecapType.BulkOrder + ")wq";
                    //_oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);

                }

            }
            catch (Exception ex)
            {

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Waiting Search
        [HttpGet]
        public JsonResult WaitingSearch(int nStatusExtra, int OrderType, int BUID)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE  OrderType=" +OrderType.ToString() + " AND OrderRecapStatus = " + (int)EnumDevelopmentStatus.RequestForApproved + " AND OrderRecapID IN (SELECT OperationObjectID FROM ApprovalRequest WHERE OperationType = " + (int)EnumApprovalRequestOperationType.OrderRecap + " AND RequestTo = " + ((User)Session[SessionInfo.CurrentUser]).UserID + ")";
            try
            {
                if (BUID >0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSQL += " AND BUID = " + BUID ;
                }

                #region User wise get
                if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
                {

                    sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
                }
                #endregion
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetOrderRecapsByBU
        [HttpPost]
        public JsonResult GetOrderRecapsByBU(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sSql = "SELECT * FROM View_OrderRecap WHERE  ISNULL(IsActive,0)= 1" ;
            try
            {
                if (oOrderRecap.bIsDependsOnShipment)
                {
                    sSql += " AND ShipmentDate > DATEADD(MONTH,-1,GETDATE())";
                }
                if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
                {
                    sSql += " AND OrderRecapNo LIKE '%"+oOrderRecap.OrderRecapNo+"%'";
                }
                if (oOrderRecap.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSql += " AND BUID = " + oOrderRecap.BUID;
                }
                _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
        #region GetOrderRecapsForPicker
            [HttpPost]
        public JsonResult GetOrderRecapsForPicker(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);

            string sTechnicalSheetIDs = oOrderRecap.ErrorMessage.Split('~')[0];
            string sBuyerIDs = oOrderRecap.ErrorMessage.Split('~')[1];
            int nShipmentDateCom = Convert.ToInt32(oOrderRecap.ErrorMessage.Split('~')[2]);
            DateTime dShipmentStartDate = Convert.ToDateTime(oOrderRecap.ErrorMessage.Split('~')[3]);
            DateTime dShipmentEndDate = Convert.ToDateTime(oOrderRecap.ErrorMessage.Split('~')[4]);

            string sSql = "SELECT * FROM View_OrderRecap WHERE  ISNULL(IsActive,0)= 1 AND OrderRecapID NOT IN (SELECT HH.OrderRecapID FROM ProductionExecutionPlan AS HH)";
            try
            {

                if (!string.IsNullOrEmpty(sTechnicalSheetIDs))
                {
                    sSql += " AND TechnicalSheetID IN ("+sTechnicalSheetIDs+")";
                }
                if (!string.IsNullOrEmpty(sBuyerIDs))
                {
                    sSql += " AND BuyerID IN ("+sBuyerIDs+")";
                }
                if (nShipmentDateCom != 0)
                {

                    if (nShipmentDateCom == 1)
                    {

                        sSql += " AND ShipmentDate = '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 2)
                    {
                        sSql += " AND ShipmentDate != '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 3)
                    {
                        sSql += " AND ShipmentDate > '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 4)
                    {
                        sSql += " AND ShipmentDate < '" + dShipmentStartDate.ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 5)
                    {
                        sSql += " AND ShipmentDate>= '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' AND ShipmentDate < '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                    if (nShipmentDateCom == 6)
                    {
                        sSql += " AND ShipmentDate< '" + dShipmentStartDate.ToString("dd MMM yyyy") + "' OR ShipmentDate > '" + dShipmentEndDate.AddDays(1).ToString("dd MMM yyyy") + "'";
                    }
                }
                if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
                {
                    sSql += " AND OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%'";
                }
                if (oOrderRecap.BUID > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
                {
                    sSql += " AND BUID = " + oOrderRecap.BUID;
                }
                _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        #region Save Bar code Comments
        [HttpPost]
        public JsonResult SaveBarCodeCommets(OrderRecap oOrderRecap)
        {
            BarCodeComment oBarCodeComment = new BarCodeComment();
            List<BarCodeComment> oBarCodeComments = new List<BarCodeComment>();
            try
            {
                oBarCodeComments = BarCodeComment.Save(oOrderRecap, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                oBarCodeComment = new BarCodeComment();
                oBarCodeComments = new List<BarCodeComment>();
                oBarCodeComment.ErrorMessage = ex.Message;
                oBarCodeComments.Add(oBarCodeComment);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oBarCodeComments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [HttpPost]
        public ActionResult SetSessionValue(OrderRecap OrderRecap)
        {
            this.Session.Remove(SessionInfo.ParamObj);
            this.Session.Add(SessionInfo.ParamObj, OrderRecap);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(Global.SessionParamSetMessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #region Print
        public ActionResult OrderRecapPrintList( double ts)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecaps = new List<OrderRecap>();
            OrderRecap oOrderRecap = (OrderRecap)Session[SessionInfo.ParamObj];
            string sSql = "SELECT * FROM View_OrderRecap WHERE OrderRecapID IN (" + oOrderRecap.Param + ") ORDER BY ShipmentDate ASC";
            _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            foreach (OrderRecap oItem in _oOrderRecaps)
            {
                if (nUserID == -9 || nUserID == 9 || nUserID == 12 || nUserID == 24 || nUserID == 27)
                {
                    oItem.Amount = oItem.Amount;
                }
                else
                {
                    oItem.Amount = 0;
                }

            }
            _oOrderRecap.OrderRecapList = _oOrderRecaps;
            if (_oOrderRecap.OrderRecapList.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oOrderRecap.Company = oCompany;
                rptOrderRecapList oReport = new rptOrderRecapList();
                byte[] abytes = oReport.PrepareReport(_oOrderRecap);
                return File(abytes, "application/pdf");
            }
            else
            {

                string sMessage = "There is no data for print";
                return RedirectToAction("MessageHelper", "User", new { message = sMessage });
            }
        }
        public void ExportToExcel(double ts)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecaps = new List<OrderRecap>();
            OrderRecap oOrderRecap = (OrderRecap)Session[SessionInfo.ParamObj];
            string sSql = "SELECT * FROM View_OrderRecap WHERE OrderRecapID IN (" + oOrderRecap.Param + ") ORDER BY BuyerID, BusinessSessionID, ShipmentDate ASC";
            _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            int nUserID = Convert.ToInt32(Session[SessionInfo.currentUserID]);
            foreach (OrderRecap oItem in _oOrderRecaps)
            {
                if (nUserID == -9 || nUserID == 9 || nUserID == 12 || nUserID == 24 || nUserID == 27)
                {
                    oItem.Amount = oItem.Amount;
                }
                else
                {
                    oItem.Amount = 0;
                }

            }
            if (_oOrderRecaps.Count > 0)
            {
                Company oCompany = new Company();
                oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
                oCompany.CompanyLogo = GetCompanyLogo(oCompany);
                _oOrderRecap.Company = oCompany;


                int rowIndex = 2;
                ExcelRange cell;
                ExcelFill fill;
                OfficeOpenXml.Style.Border border;

                using (var excelPackage = new ExcelPackage())
                {
                    excelPackage.Workbook.Properties.Author = "ESimSol";
                    excelPackage.Workbook.Properties.Title = "Export from ESimSol";
                    var sheet = excelPackage.Workbook.Worksheets.Add("Order Recap List");
                    sheet.Name = "Order Recap List";

                    sheet.Column(2).Width = 10; //SL
                    sheet.Column(3).Width = 15; //Order No
                    sheet.Column(4).Width = 18; //Stytle No
                    sheet.Column(5).Width = 15; //Buyer Name
                    sheet.Column(6).Width = 15; //SEssion
                    sheet.Column(7).Width = 15; //Item Description
                    sheet.Column(8).Width = 15; //Dept/ Gender
                    sheet.Column(9).Width = 15; //Class
                    sheet.Column(10).Width = 15; //Sub Class
                    sheet.Column(11).Width = 18; //Fabric Code
                    sheet.Column(12).Width = 15; //Wash
                    sheet.Column(13).Width = 15; //Merchandise
                    sheet.Column(14).Width = 15; //Factory Name
                    sheet.Column(15).Width = 15; //Factory Shipment
                    sheet.Column(16).Width = 15; //Buyer Shipment
                    sheet.Column(17).Width = 18; //Order Quantity
                    sheet.Column(18).Width = 18; //Buyer Price
                    sheet.Column(19).Width = 18; //Factory Price


                    #region Report Header
                    sheet.Cells[rowIndex, 2, rowIndex, 29].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = oCompany.Name; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 20; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 1;

                    sheet.Cells[rowIndex, 2, rowIndex, 29].Merge = true;
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "Order Recap List"; cell.Style.Font.Bold = true;
                    cell.Style.Font.Size = 15; cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    rowIndex = rowIndex + 2;
                    #endregion

                    #region Column Header

                    cell = sheet.Cells[rowIndex, 2]; cell.Value = "SL"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = "Order No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = "Style No"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = "Buyer Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = "Session"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = "Item Description"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = "Dept/Gender"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = "Class"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = "Sub Class"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = "Fabric Code"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = "Wash"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = "Merchandiser"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = "Factory Name"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = "Factory Shipment"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "Buyer Shipment"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = "Order Quantity"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = "Buyer Price"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = "Factory Price "; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.Cyan);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                    rowIndex = rowIndex + 1;
                    #endregion

                    #region Report Data
                    int nSL = 0;

                    double nTotalOrderQty = 0;
                    foreach (OrderRecap oItem in _oOrderRecaps)
                    {

                        nSL++;
                        cell = sheet.Cells[rowIndex, 2]; cell.Value = nSL.ToString(); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 3]; cell.Value = oItem.OrderRecapNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 4]; cell.Value = oItem.StyleNo; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 5]; cell.Value = oItem.BuyerName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 6]; cell.Value = oItem.SessionName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 7]; cell.Value = oItem.ProductName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 8]; cell.Value = oItem.DeptName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 9]; cell.Value = oItem.ClassName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 10]; cell.Value = oItem.SubClassName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 11]; cell.Value = oItem.FabCode; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 12]; cell.Value = oItem.Wash; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 13]; cell.Value = oItem.MerchandiserName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 14]; cell.Value = oItem.FactoryName; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 15]; cell.Value = oItem.FactoryShipmentDateInString; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 16]; cell.Value = oItem.ShipmentDateInString; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 17]; cell.Value = oItem.TotalQuantity; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;


                        cell = sheet.Cells[rowIndex, 18]; cell.Value = oItem.UnitPrice; cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        cell = sheet.Cells[rowIndex, 19]; cell.Value = Global.MillionFormat(oItem.CMValue / 12); cell.Style.Font.Bold = false;
                        border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                        nTotalOrderQty += oItem.TotalQuantity;

                        rowIndex++;
                    }
                    #endregion

                    #region Total
                    cell = sheet.Cells[rowIndex, 2]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 3]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 4]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 5]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 6]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 7]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 8]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 9]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 10]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 11]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 12]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 13]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 14]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 15]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 16]; cell.Value = "Total"; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 17]; cell.Value = nTotalOrderQty; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 18]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    cell = sheet.Cells[rowIndex, 19]; cell.Value = ""; cell.Style.Font.Bold = true;
                    fill = cell.Style.Fill; fill.PatternType = ExcelFillStyle.Solid; fill.BackgroundColor.SetColor(Color.White);
                    border = cell.Style.Border; border.Bottom.Style = border.Top.Style = border.Left.Style = border.Right.Style = ExcelBorderStyle.Thin;

                    #endregion

                    Response.ClearContent();
                    Response.BinaryWrite(excelPackage.GetAsByteArray());
                    Response.AddHeader("content-disposition", "attachment; filename=OrderRecapList.xlsx");
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.Flush();
                    Response.End();
                }



            }

        }
        public ActionResult OrderRecapPrintPreview(int id)
        {
            _oOrderRecap = new OrderRecap();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();

            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.OrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ORAssortments = ORAssortment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.SizeCategories = GetDistictSizes(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.ColorCategories = GetDistictColors(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.OrderRecapYarns = OrderRecapYarn.Gets(id, (int)EnumRecapRefType.OrderRecap, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.RecapBillOfMaterials = RecapBillOfMaterial.Gets(_oOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]); //Gets Bill Ob Material List for selectd TS
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.OrderSheet_PreviewFormat, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetFrontImage = oTechnicalSheetImage.GetFrontImage(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetFrontImage.FrontImage = GetTechnicalSheetImage(_oOrderRecap.TechnicalSheetFrontImage);
                _oOrderRecap.TechnicalSheetBackImage = oTechnicalSheetImage.GetBackImage(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetBackImage.BackImage = GetTechnicalSheetImage(_oOrderRecap.TechnicalSheetBackImage);
                _oOrderRecap.ImageComments = ImageComment.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oOrderRecap.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            
            byte[] abytes;
            if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_1)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat1 oReport = new rptRecapOrderSheetPreviewFormat1();
                abytes = oReport.PrepareReportFormat1(_oOrderRecap);
            }
            else if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_2)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat2 oReport = new rptRecapOrderSheetPreviewFormat2();
                abytes = oReport.PrepareReportFormat2(_oOrderRecap);
            }
            else if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_3)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat3 oReport = new rptRecapOrderSheetPreviewFormat3();
                abytes = oReport.PrepareReportFormat3(_oOrderRecap);
            }
            else
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewDefaultFormat oReport = new rptRecapOrderSheetPreviewDefaultFormat();
                abytes = oReport.PrepareReportDefaultFormat(_oOrderRecap, oBusinessUnit);
            }
            return File(abytes, "application/pdf");

            //return this.ViewPdf("", "rptOrderRecapPreview", _oOrderRecap, 40, 40, 40, 40, false);
        }

        public ActionResult BarCodePrint(int id)
        {
            _oOrderRecap = new OrderRecap();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.OrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ORBarCodes = ORBarCode.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.SizeCategories = GetDistictSizes(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.ColorCategories = GetDistictColors(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.BarCodeComments = BarCodeComment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.Contractor = oContractor.Get(_oOrderRecap.BuyerID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;

            rptOrderRecapBarCodePrint oReport = new rptOrderRecapBarCodePrint();
            byte[] abytes = oReport.PrepareReport(_oOrderRecap);
            return File(abytes, "application/pdf");

        }

        public ActionResult OrderRecapPreviewLog(int id)
        {
            _oOrderRecap = new OrderRecap();
            Company oCompany = new Company();
            Contractor oContractor = new Contractor();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();

            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.GetByLog(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.OrderRecapDetails = OrderRecapDetail.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ORAssortments = ORAssortment.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.SizeCategories = GetDistictSizes(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.ColorCategories = GetDistictColors(_oOrderRecap.OrderRecapDetails);
                _oOrderRecap.OrderRecapYarns = OrderRecapYarn.GetsByLog(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.BillOfMaterials = BillOfMaterial.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]); //Gets Bill Ob Material List for selectd TS
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_PreviewFormat, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetFrontImage = oTechnicalSheetImage.GetFrontImage(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetFrontImage.FrontImage = GetTechnicalSheetImage(_oOrderRecap.TechnicalSheetFrontImage);
                _oOrderRecap.TechnicalSheetBackImage = oTechnicalSheetImage.GetBackImage(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetBackImage.BackImage = GetTechnicalSheetImage(_oOrderRecap.TechnicalSheetBackImage);
                _oOrderRecap.ImageComments = ImageComment.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            }
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecap.Company = oCompany;
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = oBusinessUnit.Get(_oOrderRecap.BUID, ((User)Session[SessionInfo.CurrentUser]).UserID);
            byte[] abytes;
            if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_1)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat1 oReport = new rptRecapOrderSheetPreviewFormat1();
                abytes = oReport.PrepareReportFormat1(_oOrderRecap);
            }
            else if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_2)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat2 oReport = new rptRecapOrderSheetPreviewFormat2();
                abytes = oReport.PrepareReportFormat2(_oOrderRecap);
            }
            else if (Convert.ToInt32(_oOrderRecap.ClientOperationSetting.Value) == (int)EnumClientOperationValueFormat.Format_3)
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewFormat3 oReport = new rptRecapOrderSheetPreviewFormat3();
                abytes = oReport.PrepareReportFormat3(_oOrderRecap);
            }
            else
            {
                //get for responsible person
                _oOrderRecap.ClientOperationSetting = oClientOperationSetting.Get((int)EnumOperationType.OrderSheet_Responsible_Person, (int)Session[SessionInfo.currentUserID]);
                rptRecapOrderSheetPreviewDefaultFormat oReport = new rptRecapOrderSheetPreviewDefaultFormat();
                abytes = oReport.PrepareReportDefaultFormat(_oOrderRecap, oBusinessUnit);
            }
            return File(abytes, "application/pdf");

        }
        #endregion Print

        #region Order Recap Summery

        public ActionResult OrderRecapSummary(int OT, int ispre, int count, int smr, int sminr, int mr, string ReportHeader, int BuyerID, int BrandID, int FactoryID, int Category, int BusinessSessionID, int nSortBy,int buid, int menuid)
        {

            //if ispre=1 it will be Previous other wise it will be next
            //count=how much record next or previous
            //smr=Max row number from selected record
            //sminr=Min row number from selected record
            //mr=Max row from all data

            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);

            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            #region set Start & End Point
            int nStartRow = 0;
            int nEndRow = 0;
            //int nBuyerID = 0;
            //nBuyerID = BuyerID;
            if (ispre != 1)
            {
                nStartRow = smr + 1;
                nEndRow = smr + count;

                if (nStartRow > mr)
                {
                    nStartRow = sminr;
                    nEndRow = smr;
                }
            }
            else
            {
                nStartRow = sminr - count;
                //nBuyerID = BuyerID;

                if (nStartRow < 0)
                {
                    nStartRow = 1;
                }

                if (sminr > count)
                {
                    nEndRow = sminr - 1;
                }
                else
                {
                    nEndRow = smr;
                }
            }
            #endregion

            _oOrderRecapSummerys = new List<OrderRecapSummery>();
            _oOrderRecapSummery = new OrderRecapSummery();
            string sSQL = "";
            string sOrderRecapIDs = "0";
            if (BuyerID == 0 && Category == 0 && BusinessSessionID == 0)
            {
                _oOrderRecapSummerys = new List<OrderRecapSummery>();
            }
            else
            {
                sSQL = GetRecapSummarySQL(OT, BuyerID, Category, BusinessSessionID, BrandID, FactoryID, buid);
                _oOrderRecapSummerys = OrderRecapSummery.GetsRecapWithOrderRecapSummerys(OT, nStartRow, nEndRow, sSQL, sOrderRecapIDs, false, nSortBy, (int)Session[SessionInfo.currentUserID]);
                if (_oOrderRecapSummerys.Count > 0)
                {
                    sOrderRecapIDs = OrderRecapSummery.IDInString(_oOrderRecapSummerys);
                }
            }

            #region Searching Data
            _oOrderRecapSummery.OrderRecapSummeryList = _oOrderRecapSummerys;
            if ((int)Session[SessionInfo.FinancialUserType] != (int)EnumFinancialUserType.Normal_User)
            {
                _oOrderRecapSummery.ContractorList = Contractor.GetsByNamenType("", ((int)EnumContractorType.Buyer).ToString(), buid, (int)Session[SessionInfo.currentUserID]);
            }
            else
            {
                string sContractorSQL = "SELECT * FROM Contractor WHERE ContractorID IN (SELECT ContractorID FROM UserWiseContractorConfigure WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ") Order by [Name]";
                _oOrderRecapSummery.ContractorList = (Contractor.Gets(sContractorSQL, (int)Session[SessionInfo.currentUserID]));
            }
            sSQL = "SELECT * FROM VIEW_ProductCategory WHERE ParentCategoryID=3";
            _oOrderRecapSummery.ProductCategorys = ProductCategory.GetsBUWiseLastLayer(buid, (int)Session[SessionInfo.currentUserID]);//here 3=Finish Goods 
            _oOrderRecapSummery.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);//here business year get distinct DB Server Date year
            #endregion

            #region Temp Data
            TempData["message"] = count;
            TempData["OrderRecapIDs"] = sOrderRecapIDs;
            TempData["BuyerID"] = BuyerID;
            TempData["BrandID"] = BrandID;
            TempData["FactoryID"] = FactoryID;
            TempData["Category"] = Category;
            TempData["BusinessSessionID"] = BusinessSessionID;
            TempData["nSortBy"] = nSortBy;
            TempData["ReportHeader"] = ReportHeader;
            TempData["OT"] = OT;
            TempData["BUID"] = buid;
            if (_oOrderRecapSummerys.Count > 0)
            {
                TempData["numberlist"] = "(" + _oOrderRecapSummerys[0].RowNumber.ToString() + " - " + _oOrderRecapSummerys[_oOrderRecapSummerys.Count - 1].RowNumber.ToString() + ") of " + _oOrderRecapSummerys[0].MaxRowNumber.ToString();
            }
            #endregion
            _oOrderRecapSummery.Brands = Brand.Gets((int)Session[SessionInfo.currentUserID]);
            _oOrderRecapSummery.Factorys = Contractor.GetsByNamenType("", ((int)EnumContractorType.Factory).ToString(),buid, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecapSummery.IsRateView = HaveRateViewPermission(EnumRoleOperationType.RateView);
            _oOrderRecapSummery.IsCMValue = HaveRateViewPermission(EnumRoleOperationType.ViewCMValue);
            return View(_oOrderRecapSummery);
        }

        public string GetRecapSummarySQL(int OT, int BuyerID, int ProductCategoryID, int nBUsinessSessionID, int nBrandID, int nFactoryID, int buid)
        {
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT OrderRecapID FROM OrderRecap";
            string sReturn = "";

            #region BUID
            if (buid > 0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BUID = " + buid.ToString();
            }
            #endregion

            #region Order Type
            if (OT > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " OrderType = " + OT.ToString();
            }
            #endregion

            #region Buyer Name
            if (BuyerID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BuyerID = " + BuyerID.ToString();
            }
            #endregion

            #region Product Category
            if (ProductCategoryID > 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE ProductID IN (SELECT ProductID FROM View_Product WHERE ProductCategoryID =" + ProductCategoryID.ToString() + "))";
            }
            #endregion

            #region Merchandiser
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "MerchandiserID = (SELECT EmployeeID FROM Users WHERE UserID = " + (int)Session[SessionInfo.currentUserID] + ")";
            }
            #endregion

            #region Business Year
            if (nBUsinessSessionID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " BusinessSessionID =" + nBUsinessSessionID.ToString();
            }
            #endregion

            #region Brand
            if (nBrandID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + "TechnicalSheetID IN (SELECT TechnicalSheetID FROM TechnicalSheet WHERE BrandID=" + nBrandID + " )";
            }
            #endregion

            #region Factory
            if (nFactoryID != 0)
            {
                Global.TagSQL(ref sReturn);
                sReturn = sReturn + " ProductionFactoryID = " + nFactoryID.ToString();
            }
            #endregion

            #region Avoid Hidden Style
            Global.TagSQL(ref sReturn);
            sReturn += " TechnicalSheetID NOT IN (SELECT TT.TechnicalSheetID FROM UserWiseStyleConfigure AS TT  WHERE TT.UserID=" + ((int)Session[SessionInfo.currentUserID]).ToString() + ")";
            #endregion

            sSQL = sSQL + sReturn;
            return sSQL;
        }

        public Image GetThumImage(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                //img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);                
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetThumImageForPrint(int id)
        {
            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetThumbnail.ThumbnailImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        public Image GetLargeImage(int id)
        {
            TechnicalSheetImage oTechnicalSheetImage = new TechnicalSheetImage();
            oTechnicalSheetImage = oTechnicalSheetImage.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                //img.Save(Server.MapPath("~/Content/") + "styleimage.jpg", ImageFormat.Jpeg);                
                return img;
            }
            else
            {
                return null;
            }
        }
        #region View Order Recap Detail
        public ActionResult ViewOrderRecapDetail(int id)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecapDetails = new List<OrderRecapDetail>();
            _oOrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(_oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);

            return View(_oOrderRecap);
        }

        [HttpGet]
        public JsonResult UpdateCMValue(int id, double CMValue)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            try
            {

                oOrderRecap = _oOrderRecap.UpdateCMValue(id, CMValue, (int)Session[SessionInfo.currentUserID]);

            }
            catch (Exception ex)
            {
                oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion


        public ActionResult PrintRecapSummary(int OT, string RecapIDs, string ReportHeader, int nSortBy)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser( ((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            List<OrderRecapSummery> oOrderRecapSummerys = new List<OrderRecapSummery>();
            _oOrderRecapSummerys = new List<OrderRecapSummery>();
            Company oCompany = new Company();
            string sOrderType = "Order Recap Summary";
            if (OT == 2)
            {
                sOrderType = "Sales Man Sample Summary";
            }

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oOrderRecapSummerys = OrderRecapSummery.GetsRecapWithOrderRecapSummerys(OT, 0, 0, "", RecapIDs, true, nSortBy, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "SELECT * FROM View_OrderRecapYarn WHERE RefObjectID IN(" + OrderRecapSummery.IDInString(oOrderRecapSummerys) + ") AND RefType = "+(int)EnumRecapRefType.OrderRecap;
            _oOrderRecapYarns = OrderRecapYarn.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            foreach (OrderRecapSummery oItem in oOrderRecapSummerys)
            {
                oItem.StyleCoverImage = GetThumImageForPrint(oItem.TechnicalSheetID);
                oItem.FabricOptionA = GetOptionWiseFabric(0, oItem.OrderRecapID);
                oItem.FabricOptionB = GetOptionWiseFabric(1, oItem.OrderRecapID);
                oItem.FabricOptionC = GetOptionWiseFabric(2, oItem.OrderRecapID);
                oItem.FabricOptionD = GetOptionWiseFabric(3, oItem.OrderRecapID);
                _oOrderRecapSummerys.Add(oItem);
            }

            bool IsRateView = HaveRateViewPermission(EnumRoleOperationType.RateView);
            bool IsCMValueView = HaveRateViewPermission(EnumRoleOperationType.ViewCMValue);
            rptOrderRecapSummerys oReport = new rptOrderRecapSummerys();
            byte[] abytes = oReport.PrepareReport(sOrderType, _oOrderRecapSummerys, oCompany, ReportHeader, IsRateView, IsCMValueView);
            return File(abytes, "application/pdf");
        }

        private string GetOptionWiseFabric(int nIdex, int nOrderRecapID)
        {
            List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
            foreach (OrderRecapYarn oItem in _oOrderRecapYarns)
            {
                if (oItem.RefObjectID == nOrderRecapID)
                {
                    oOrderRecapYarns.Add(oItem);
                }
            }

            if (nIdex < oOrderRecapYarns.Count)
            {
                if (oOrderRecapYarns[nIdex].YarnPly != "")
                {
                    return oOrderRecapYarns[nIdex].YarnName + ", " + oOrderRecapYarns[nIdex].YarnPly;
                }
                else
                {
                    return oOrderRecapYarns[nIdex].YarnName;
                }
            }
            else
            {
                return "";
            }
        }
        #endregion

        #region Order Recap Picker
        public ActionResult OrderRecapPicker(int nBuyerID, double ts)
        {
            _oOrderRecap = new OrderRecap();
            //string sSql = "SELECT * FROM View_OrderRecap WHERE BuyerID =" + nBuyerID + " AND ApproveBy !=0 AND  TotalQuantity>PIAttachQty";
            string sSql = "SELECT * FROM View_OrderRecap WHERE BuyerID IN(SELECT  " + nBuyerID + " UNION SELECT BuyerID FROM BuyerGroupDetail WHERE BuyerGroupID = (SELECT BuyerGroupID FROM BuyerGroupDetail WHERE BuyerID = " + nBuyerID + ") ) AND ApproveBy !=0 AND  TotalQuantity > PIAttachQty";
            _oOrderRecap.OrderRecapList = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            @ViewBag.BusinessSessions = BusinessSession.Gets(true, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oOrderRecap);
        }

        [HttpPost]
        public JsonResult GetBuyerWiseOrderRecaps(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSql = "SELECT * FROM View_OrderRecap WHERE BuyerID IN(SELECT  " + oOrderRecap.BuyerID + " UNION SELECT BuyerID FROM BuyerGroupDetail WHERE BuyerGroupID = (SELECT BuyerGroupID FROM BuyerGroupDetail WHERE BuyerID = " + oOrderRecap.BuyerID + ") ) AND ApproveBy !=0 AND  TotalQuantity > PIAttachQty";

            if (oOrderRecap.BuyerID == 0) //For OrderRecapRegister
                sSql = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%' AND BUID = " + oOrderRecap.BUID + " ORDER BY OrderRecapID";

            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetShipmentWiseOrderRecaps(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSql = "SELECT * FROM View_OrderRecap WHERE BuyerID IN(SELECT  " + oOrderRecap.BuyerID + " UNION SELECT BuyerID FROM BuyerGroupDetail WHERE BuyerGroupID = (SELECT BuyerGroupID FROM BuyerGroupDetail WHERE BuyerID = " + oOrderRecap.BuyerID + ") ) AND ApproveBy !=0 AND  TotalQuantity > PIAttachQty";

            if (oOrderRecap.BuyerID == 0) //For OrderRecapRegister
                sSql = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%' AND BUID = " + oOrderRecap.BUID;

            sSql += " AND OrderRecapID IN (SELECT OrderRecapID FROM ShipmentSchedule) ORDER BY OrderRecapID";
            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSql, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Print Follow Up Sheet
        public ActionResult PrintFollowUpSheet(string RecapIDs)
        {
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByModuleAndUser(((int)EnumModuleName.OrderRecap).ToString(), (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));

            _oOrderRecaps = new List<OrderRecap>();
            _oOrderRecapDetails = new List<OrderRecapDetail>();
            //List<LabDipOrderDetail> oLabDipOrderDetails = new List<LabDipOrderDetail>();
            List<DyeingOrderDetail> oDyeingOrderDetails = new List<DyeingOrderDetail>();
            List<WorkOrderDetail> oWorkOrderDetails = new List<WorkOrderDetail>();
            //List<PackageBreakdown> oPackageBreakdowns = new List<PackageBreakdown>();
            List<SampleRequirement> oSampleRequirements = new List<SampleRequirement>();

            Company oCompany = new Company();
            string sORSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapID IN (" + RecapIDs + ")";
            string sORDSQL = "SELECT  0 AS OrderRecapDetailID, 0 AS OrderRecapLogID, 0 AS OrderRecapDetailLogID, OrderRecapID, 0 AS SizeID,  '' as UnitSymbol, '' AS SizeCategoryName, 0 AS MeasurementUnitID, 0 AS PoductionQty, 0 AS YetToPoductionQty, 0 AS UnitPrice, 0 AS Amount, CurrencySymbol, '' AS UnitName,  ColorID, ColorName, ColorSequence, 0 AS SizeSequence, SUM(Quantity) AS Quantity FROM View_OrderRecapDetail WHERE OrderRecapID IN (" + RecapIDs + ") GROUP BY ColorID, ColorName, OrderRecapID, CurrencySymbol, ColorSequence";
            string sLODSQL = "SELECT * FROM View_LabDipOrderDetail WHERE LabDipOrderID IN (SELECT LabDipOrderID FROM LabDipOrder WHERE TechnicalSheetID IN (SELECT TechnicalSheetID FROM View_OrderRecap WHERE OrderRecapID IN (" + RecapIDs + ")))";
            string sDODSQL = "SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID IN (SELECT DyeingOrderID FROM DyeingOrder WHERE TechnicalSheetID IN (SELECT TechnicalSheetID FROM View_OrderRecap WHERE OrderRecapID IN (" + RecapIDs + ")))";
            string sNWODSQL = "SELECT * FROM View_WorkOrderDetail WHERE WorkOrderID IN (SELECT WorkOrderID FROM WorkOrder WHERE AccessoriesType = " + (int)EnumAccessoriesType.Normal + ") AND OrderRecapID IN (" + RecapIDs + ")"; // Normal work Order Gets
            string sPWOSQL = "SELECT * FROM View_PackageBreakdown WHERE WorkOrderDetailID IN ( SELECT WorkOrderDetailID FROM View_WorkOrderDetail WHERE WorkOrderID IN (SELECT WorkOrderID FROM WorkOrder WHERE AccessoriesType = " + (int)EnumAccessoriesType.Package + ") AND OrderRecapID IN (" + RecapIDs + "))"; // Package Work Order
            string sSRSQL = "SELECT * FROM View_SampleRequirement WHERE OrderRecapID IN (" + RecapIDs + ")";

            _oOrderRecaps = OrderRecap.Gets(sORSQL, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecapDetails = OrderRecapDetail.Gets(sORDSQL, (int)Session[SessionInfo.currentUserID]);
            //oLabDipOrderDetails = LabDipOrderDetail.Gets(sLODSQL, (int)Session[SessionInfo.currentUserID]);
            oDyeingOrderDetails = DyeingOrderDetail.Gets(sDODSQL, (int)Session[SessionInfo.currentUserID]);
            oWorkOrderDetails = WorkOrderDetail.Gets(sNWODSQL, (int)Session[SessionInfo.currentUserID]);
            //oPackageBreakdowns = PackageBreakdown.Gets(sPWOSQL, (int)Session[SessionInfo.currentUserID]);
            oSampleRequirements = SampleRequirement.Gets(sSRSQL, (int)Session[SessionInfo.currentUserID]);

          //  _oOrderRecaps = SetListWithOrderRecaps(_oOrderRecaps, _oOrderRecapDetails, oLabDipOrderDetails, oDyeingOrderDetails, oWorkOrderDetails, oPackageBreakdowns, oSampleRequirements);

            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            bool IsRateView = HaveRateViewPermission(EnumRoleOperationType.RateView);
            bool IsCMValueView = HaveRateViewPermission(EnumRoleOperationType.ViewCMValue);
            rptFollowUpSheet oReport = new rptFollowUpSheet();
            byte[] abytes = oReport.PrepareReport(_oOrderRecaps, oCompany);
            return File(abytes, "application/pdf");
        }
        #endregion

        #region Order Recap Update Info
        public ActionResult ViewOrderRecapUpdateInfo(int id)
        {
            _oOrderRecap = new OrderRecap();
            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.SampleRequirements = SampleRequirement.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            _oOrderRecap.SampleTypes = SampleType.Gets((int)Session[SessionInfo.currentUserID]);
            return View(_oOrderRecap);
        }

        #region Add Sample Requirement
        [HttpPost]
        public JsonResult SaveSampleRequirement(SampleRequirement oSampleRequirement)
        {
            _oSampleRequirement = new SampleRequirement();
            try
            {
                _oSampleRequirement = oSampleRequirement.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oSampleRequirement.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oSampleRequirement);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete Sample Requirement
        [HttpGet]
        public JsonResult DeleteSampleRequirement(int id, double ts)
        {
            string feedbackmessage = "";
            try
            {
                _oSampleRequirement = new SampleRequirement();
                feedbackmessage = _oSampleRequirement.Delete(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                feedbackmessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(feedbackmessage);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Update Order Recap Info
        [HttpPost]
        public JsonResult UpdateInfo(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            try
            {
                _oOrderRecap = oOrderRecap.UpdateInfo((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecap);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Commercial Summery
        public ActionResult ViewOrderRecapCommercialSummery(int id, double ts)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecapDetails = new List<OrderRecapDetail>();
            MasterLC oMasterLC = new MasterLC();
            if (id > 0)
            {
                _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecapDetails = OrderRecapDetail.Gets(id, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.TechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecap.ColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(_oOrderRecapDetails, _oOrderRecap.TechnicalSheetSizes);
                //_oOrderRecap.MasterLC = oMasterLC.GetByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
                //_oOrderRecap.DyeingOrders = DyeingOrder.GetsByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
                //_oOrderRecap.WorkOrders = WorkOrder.GetsByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
                //_oOrderRecap.ImportLCs = ImportLC.GetsByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);
                //_oOrderRecap.ImportPIs = ImportPI.GetsByOrderRecap(id, (int)Session[SessionInfo.currentUserID]);


            }
            else
            {
                _oOrderRecap.DyeingOrders = new List<DyeingOrder>();
                _oOrderRecap.WorkOrders = new List<WorkOrder>();
                _oOrderRecap.MasterLC = new MasterLC();
                _oOrderRecap.ImportLCs = new List<ImportLC>();
                _oOrderRecap.ImportPIs = new List<ImportPI>();

            }
            return PartialView(_oOrderRecap);
        }

        #endregion

        public Image GetCompanyLogo(Company oCompany)
        {
            if (oCompany.OrganizationLogo != null)
            {
                MemoryStream m = new MemoryStream(oCompany.OrganizationLogo);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                //img.Save(Response.OutputStream, ImageFormat.Jpeg);
                img.Save(Server.MapPath("~/Content/") + "companyLogo.jpg", ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }
        public Image GetTechnicalSheetImage(TechnicalSheetImage oTechnicalSheetImage)
        {
            if (oTechnicalSheetImage.LargeImage != null)
            {
                MemoryStream m = new MemoryStream(oTechnicalSheetImage.LargeImage);
                System.Drawing.Image img = System.Drawing.Image.FromStream(m);
                img.Save(Response.OutputStream, ImageFormat.Jpeg);
                return img;
            }
            else
            {
                return null;
            }
        }

        #region Search By Order Recap Picker
        public ActionResult ViewRecapSearch(string sTemp, double ts)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE '%" + sTemp + "%'";
            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {
                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            return PartialView(_oOrderRecaps);
        }
        #endregion

        [HttpPost]
        public ActionResult GetCoverImageInBase64(int id)
        {

            TechnicalSheetThumbnail oTechnicalSheetThumbnail = new TechnicalSheetThumbnail();
            oTechnicalSheetThumbnail = oTechnicalSheetThumbnail.GetFrontImage(id, (int)Session[SessionInfo.currentUserID]);
            if (oTechnicalSheetThumbnail.ThumbnailImage == null)
            {
                oTechnicalSheetThumbnail.ThumbnailImage = new byte[10];
            }
            return Json(new { base64imgage = Convert.ToBase64String(oTechnicalSheetThumbnail.ThumbnailImage) }, JsonRequestBehavior.AllowGet);
        }

        #region Order Recap Details Summary
        #region View Order Recap Details Summery
        public ActionResult ViewOrderRecapDetailsSummery(int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<OrderDetailsSummary> oOrderDetailsSummaries = new List<OrderDetailsSummary>();

            //this.Session.Add(SessionInfo.AuthorizationRolesMapping, AuthorizationRoleMapping.GetsByDBObjectAndUser("'MasterLCSummery'", (int)Session[SessionInfo.currentUserID], (int)Session[SessionInfo.currentUserID]));
            return View(oOrderDetailsSummaries);
        }
        #endregion

        #region HTTP Get Order Details Summary
        [HttpPost]
        public JsonResult GetOrderDetailsSummary(OrderRecap oOrderRecap)
        {
            List<OrderDetailsSummary> oOrderDetailsSummaries = new List<OrderDetailsSummary>();
            _oOrderRecaps = new List<OrderRecap>();
            _oOrderRecaps = oOrderRecap.OrderRecapList;
            try
            {
                oOrderDetailsSummaries = OrderDetailsSummary.Gets(oOrderRecap.Description, (int)Session[SessionInfo.currentUserID]); // here use descrioption for IDs
            }
            catch (Exception ex)
            {
                OrderDetailsSummary OrderDetailsSummary = new OrderDetailsSummary();
                OrderDetailsSummary.ErrorMessage = ex.Message;
                oOrderDetailsSummaries.Add(OrderDetailsSummary);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderDetailsSummaries);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #endregion

        #region Recap Shipment Summery
        public ActionResult ViewRecapShipmentSummery(int buid, int menuid)
        {
            this.Session.Remove(SessionInfo.MenuID);
            this.Session.Add(SessionInfo.MenuID, menuid);
            this.Session.Remove(SessionInfo.AuthorizationRolesMapping);
            List<RecapShipmentSummary> oRecapShipmentSummaries = new List<RecapShipmentSummary>();
            ViewBag.BUID = buid;
            return View(oRecapShipmentSummaries);
        }

     

        [HttpGet]
        public JsonResult GetShipmentSummery(string sYear, int buid)
        {
            List<RecapShipmentSummary> oRecapShipmentSummaries = new List<RecapShipmentSummary>();
            List<RecapShipmentSummary> oMonthWiseSummaries = new List<RecapShipmentSummary>();
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();

            try
            {
                oRecapShipmentSummaries = RecapShipmentSummary.Gets(sYear, buid, (int)Session[SessionInfo.FinancialUserType], (int)Session[SessionInfo.currentUserID]);
                foreach (RecapShipmentSummary oItem in oRecapShipmentSummaries)
                {
                    if (oItem.DataViewType == 3)
                    {
                        oMonthWiseSummaries.Add(oItem);
                    }
                }
                foreach (RecapShipmentSummary oItem in oMonthWiseSummaries)
                {
                    oItem.BuyerWithMonthWiseSummaries = GetBuyerWithMonthWiseList(oItem.ShipmentMonth, oRecapShipmentSummaries);
                }
                oRecapShipmentSummary.MonthWiseSummaries = FillupMonthCycle(oMonthWiseSummaries, sYear);
                oRecapShipmentSummary.BuyerWithDateWiseSummaries = GetBuyerwithDatesummaries(oRecapShipmentSummaries);
                oRecapShipmentSummary.RecapShipmentSummarys = _oRecapShipmentSummarys;
            }
            catch (Exception ex)
            {
                oRecapShipmentSummary = new RecapShipmentSummary();
                oRecapShipmentSummary.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oRecapShipmentSummary);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        public List<RecapShipmentSummary> _oRecapShipmentSummarys = new List<RecapShipmentSummary>();

        private List<RecapShipmentSummary> GetBuyerWithMonthWiseList(int nMonth, List<RecapShipmentSummary> oRecapShipmentSummaries)
        {
            List<RecapShipmentSummary> oTempRecapShipmentSummaries = new List<RecapShipmentSummary>();
            List<RecapShipmentSummary> oShipmentRecaps = new List<RecapShipmentSummary>();
            RecapShipmentSummary oTempRecapShipmentSummary = new RecapShipmentSummary();
            int nTempID = 0;
            foreach (RecapShipmentSummary oItem in oRecapShipmentSummaries)
            {
                if (oItem.ShipmentMonth == nMonth && oItem.DataViewType == 2)
                {
                    nTempID = 0; nTempID = _oRecapShipmentSummarys.Count + 1;
                    oShipmentRecaps = new List<RecapShipmentSummary>();
                    oShipmentRecaps = GetMonthWiseList(nMonth, oItem.BuyerID, oRecapShipmentSummaries);
                    oTempRecapShipmentSummary = new RecapShipmentSummary();
                    oTempRecapShipmentSummary.BuyerID = oItem.BuyerID;
                    oTempRecapShipmentSummary.BuyerName = oItem.BuyerName;
                    oTempRecapShipmentSummary.ShipmentMonthInString = oItem.ShipmentMonthInString;
                    oTempRecapShipmentSummary.TempID = nTempID;
                    oTempRecapShipmentSummary.RecapShipmentSummarys = oShipmentRecaps;
                    _oRecapShipmentSummarys.Add(oTempRecapShipmentSummary);

                    oItem.TempID = nTempID;
                    oItem.ShipmentCount = oShipmentRecaps.Count;
                    oTempRecapShipmentSummaries.Add(oItem);
                }
            }
            return oTempRecapShipmentSummaries;
        }

        private List<RecapShipmentSummary> GetMonthWiseList(int nMonth, int nBuyerID, List<RecapShipmentSummary> oRecapShipmentSummaries)
        {
            List<RecapShipmentSummary> oTempRecapShipmentSummaries = new List<RecapShipmentSummary>();
            foreach (RecapShipmentSummary oItem in oRecapShipmentSummaries)
            {
                if (oItem.ShipmentMonth == nMonth && oItem.BuyerID == nBuyerID && oItem.DataViewType == 1)
                {
                    oTempRecapShipmentSummaries.Add(oItem);
                }
            }
            return oTempRecapShipmentSummaries;
        }

        private List<RecapShipmentSummary> FillupMonthCycle(List<RecapShipmentSummary> oMonthWiseSummaries, string sYear)
        {
            List<RecapShipmentSummary> oTempMonthWiseSummaries = new List<RecapShipmentSummary>();
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();
            for (int i = 1; i <= 12; i++)
            {
                oRecapShipmentSummary = GetMonth(oMonthWiseSummaries, i);
                if (oRecapShipmentSummary.Qty > 0)
                {
                    oTempMonthWiseSummaries.Add(oRecapShipmentSummary);
                }
                else
                {
                    oRecapShipmentSummary = new RecapShipmentSummary();
                    oRecapShipmentSummary.ShipmentMonth = i;
                    oRecapShipmentSummary.ShipmentMonthInString = ((EnumMonthName)i).ToString() + "-" + sYear;
                    oRecapShipmentSummary.Qty = 0;
                    oTempMonthWiseSummaries.Add(oRecapShipmentSummary);
                }
            }
            return oTempMonthWiseSummaries;
        }

        private RecapShipmentSummary GetMonth(List<RecapShipmentSummary> oMonthWiseSummaries, int nMonth)
        {
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();
            foreach (RecapShipmentSummary oItem in oMonthWiseSummaries)
            {
                if (oItem.ShipmentMonth == nMonth)
                {
                    return oItem;
                }
            }
            return oRecapShipmentSummary;
        }

        public ActionResult PrintRecapShipmentSummary(string sYear, int buid)
        {
            List<RecapShipmentSummary> oRecapShipmentSummaries = new List<RecapShipmentSummary>();
            List<RecapShipmentSummary> oMonthWiseSummaries = new List<RecapShipmentSummary>();
            RecapShipmentSummary oRecapShipmentSummary = new RecapShipmentSummary();
            oRecapShipmentSummaries = RecapShipmentSummary.Gets(sYear, buid,(int)Session[SessionInfo.FinancialUserType], (int)Session[SessionInfo.currentUserID]);
            foreach (RecapShipmentSummary oItem in oRecapShipmentSummaries)
            {
                if (oItem.DataViewType == 3)
                {
                    oMonthWiseSummaries.Add(oItem);
                }
            }
            foreach (RecapShipmentSummary oItem in oMonthWiseSummaries)
            {
                oItem.BuyerWithMonthWiseSummaries = GetBuyerWithMonthWiseList(oItem.ShipmentMonth, oRecapShipmentSummaries);
            }
            oRecapShipmentSummary.MonthWiseSummaries = FillupMonthCycle(oMonthWiseSummaries, sYear);
            //oRecapShipmentSummary.BuyerWithDateWiseSummaries = GetBuyerwithDatesummaries(oRecapShipmentSummaries);

            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            oRecapShipmentSummary.Company = oCompany;
            rptRecapShipmentSummary oReport = new rptRecapShipmentSummary();
            byte[] abytes = oReport.PrepareReport(oRecapShipmentSummary);
            return File(abytes, "application/pdf");
        }

        #endregion

        //#region Gets Order Type
        //[HttpPost]
        //public JsonResult GetOrderType(OrderRecap oOrderRecap)
        //{
        //    List<OrderTypeObj> oOrderTypeObjs = new List<OrderTypeObj>();
        //    try
        //    {
        //        oOrderTypeObjs = OrderTypeObj.Gets();
        //    }
        //    catch (Exception ex)
        //    {
        //        OrderTypeObj oOrderTypeObj = new OrderTypeObj();
        //        oOrderTypeObj.Value = ex.Message;
        //        oOrderTypeObjs.Add(oOrderTypeObj);
        //    }
        //    JavaScriptSerializer serializer = new JavaScriptSerializer();
        //    string sjson = serializer.Serialize(oOrderTypeObjs);
        //    return Json(sjson, JsonRequestBehavior.AllowGet);
        //}
        //#endregion

        #region Search Style OR Recap by Press Enter
        [HttpGet]
        public JsonResult SearchStyleAndRecap(string sTempData, bool bIsStyle, int BUID, double ts)
        {
            _oOrderRecaps = new List<OrderRecap>();
            ClientOperationSetting oClientOperationSetting = new ClientOperationSetting();
            oClientOperationSetting = oClientOperationSetting.GetByOperationType((int)EnumOperationType.IsOperationWithBusinessUnit, (int)Session[SessionInfo.currentUserID]);
            string sSQL = "";
            if (bIsStyle == true)
            {
                sSQL = "SELECT * FROM View_OrderRecap WHERE StyleNo LIKE ('%" + sTempData + "%')";
            }
            else
            {
                sSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE ('%" + sTempData + "%')";
            }
            if (BUID>0 && Convert.ToBoolean(Convert.ToInt32(oClientOperationSetting.Value)) == true)//if apply style configuration business unit
            {
                sSQL += " AND BUID = " + BUID;
            }

            #region User Set
            if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            {

                sSQL += " AND TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            }
            #endregion
            try
            {
                OrderRecap oOrderRecap = new OrderRecap();
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOrderRecapsByOrderRecapNo(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap WHERE OrderRecapNo LIKE ('%" + oOrderRecap.OrderRecapNo + "%') AND ISNULL(OrderRecapID,0) NOT IN (SELECT OrderRecapID FROM View_JobDetail) AND TechnicalSheetID = " + oOrderRecap.TechnicalSheetID + " ";
            
            try
            {
                //OrderRecap oOrderRecap = new OrderRecap();
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Get Sale Order
        [HttpPost]
        public JsonResult GetOrderRecaps(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap", sTemp = ""; 
            if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
            {
                Global.TagSQL(ref sTemp);
                sTemp += "OrderRecapNo LIKE ('%" + oOrderRecap.OrderRecapNo + "%')";
            }
            if (oOrderRecap.TechnicalSheetID>0)
            {
                Global.TagSQL(ref sTemp);
                sTemp += " TechnicalSheetID =" + oOrderRecap.TechnicalSheetID;
            }
            Global.TagSQL(ref sTemp);
            sTemp += " BUID = " + oOrderRecap.BUID + " AND ISNULL(IsActive,0)= 1";
            //#region User Set
            //if ((int)Session[SessionInfo.FinancialUserType] == (int)EnumFinancialUserType.Normal_User)
            //{
            //    Global.TagSQL(ref sTemp);    
            //    sTemp += " TechnicalSheetID IN (SELECT * FROM [dbo].[Fn_AuthorizeStyle](" + (int)Session[SessionInfo.currentUserID] + "))";
            //}
            sSQL += sTemp;
            //#endregion
            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetOrderRecapsForYarnRequisition(OrderRecap oOrderRecap)
        {
            _oOrderRecaps = new List<OrderRecap>();
            string sSQL = "SELECT * FROM View_OrderRecap AS HH", sTemp = "";

            #region BUID
            if (oOrderRecap.BUID > 0)
            {
                Global.TagSQL(ref sTemp);
                sTemp = sTemp + " HH.BUID = " + oOrderRecap.BUID.ToString();
            }
            #endregion

            #region BusinessSessionID
            if (oOrderRecap.BusinessSessionID > 0)
            {
                Global.TagSQL(ref sTemp);
                sTemp = sTemp + " HH.BusinessSessionID = " + oOrderRecap.BusinessSessionID.ToString();
            }
            #endregion

            #region BuyerID
            if (oOrderRecap.BuyerID > 0)
            {
                Global.TagSQL(ref sTemp);
                sTemp = sTemp + " HH.BuyerID = " + oOrderRecap.BuyerID.ToString();
            }
            #endregion

            if (!string.IsNullOrEmpty(oOrderRecap.OrderRecapNo))
            {
                Global.TagSQL(ref sTemp);
                sTemp = sTemp + "(HH.StyleNo  LIKE '%" + oOrderRecap.OrderRecapNo + "%' OR HH.OrderRecapNo LIKE '%" + oOrderRecap.OrderRecapNo + "%')";
            }
            sSQL = sSQL + sTemp + " ORDER BY HH.StyleNo, HH.OrderRecapNo ASC";

            try
            {
                _oOrderRecaps = OrderRecap.Gets(sSQL, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecap = new OrderRecap();
                _oOrderRecap.ErrorMessage = ex.Message;
                _oOrderRecaps.Add(_oOrderRecap);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecaps);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public JsonResult GetOrderRecapColors(OrderRecap oOrderRecap)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecapDetails = new List<OrderRecapDetail>();
            List<ColorSizeRatio> oColorSizeRatios = new List<ColorSizeRatio>();
            List<TechnicalSheetSize> oTechnicalSheetSizes = new List<TechnicalSheetSize>();

            try
            {
                _oOrderRecap = _oOrderRecap.Get(oOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                _oOrderRecapDetails = OrderRecapDetail.Gets(oOrderRecap.OrderRecapID, (int)Session[SessionInfo.currentUserID]);
                oTechnicalSheetSizes = TechnicalSheetSize.Gets(_oOrderRecap.TechnicalSheetID, (int)Session[SessionInfo.currentUserID]);
                oColorSizeRatios = MapColorSizeRationFromOrderRecapDetail(_oOrderRecapDetails, oTechnicalSheetSizes);
            }
            catch (Exception ex)
            {
                //ColorSizeRatio oColorSizeRatio = new ColorSizeRatio();
                oColorSizeRatios = new List<ColorSizeRatio>();
                //oColorSizeRatio.ErrorMessage = ex.Message;
                //oColorSizeRatios.Add(oColorSizeRatio);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oColorSizeRatios);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }            
        #endregion

        #region Sale Order Comments
        [HttpPost]
        public JsonResult SaveOrderRecapComment(OrderRecapComment oOrderRecapComment)
        {
            _oOrderRecapComment = new OrderRecapComment();
            try
            {
                _oOrderRecapComment = oOrderRecapComment;
                _oOrderRecapComment.CommentsBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
                _oOrderRecapComment = _oOrderRecapComment.Save(((User)Session[SessionInfo.CurrentUser]).UserID);
            }
            catch (Exception ex)
            {
                _oOrderRecapComment = new OrderRecapComment();
                _oOrderRecapComment.ErrorMessage = ex.Message;

            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecapComment);
            return Json(sjson, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public JsonResult GetsComment(int id)
        {
            _oOrderRecapComments = new List<OrderRecapComment>();

            try
            {
                _oOrderRecapComments = OrderRecapComment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                OrderRecapComment oOrderRecapComment = new OrderRecapComment();
                oOrderRecapComment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecapComments);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult RemoveComment(int id)
        {
            string sFeedBackMessage = "";
            try
            {
                OrderRecapComment oOrderRecapComment = new OrderRecapComment();

                sFeedBackMessage = oOrderRecapComment.Delete(id, (int)Session[SessionInfo.currentUserID]);
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
        public JsonResult SOEditComment(OrderRecapComment oOrderRecapComment)
        {
            _oOrderRecapComment = new OrderRecapComment();
            try
            {
                _oOrderRecapComment = oOrderRecapComment;
                _oOrderRecapComment.CommentsBy = ((User)Session[SessionInfo.CurrentUser]).UserName;
                _oOrderRecapComment = _oOrderRecapComment.Save((int)Session[SessionInfo.currentUserID]);
            }
            catch (Exception ex)
            {
                _oOrderRecapComment = new OrderRecapComment();
                _oOrderRecapComment.ErrorMessage = ex.Message;
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(_oOrderRecapComment);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PrintOrderRecapComments(int id)
        {
            _oOrderRecap = new OrderRecap();
            _oOrderRecapComment = new OrderRecapComment();
            _oOrderRecapComment.OrderRecapComments = OrderRecapComment.Gets(id, (int)Session[SessionInfo.currentUserID]);
            _oOrderRecap = _oOrderRecap.Get(id, (int)Session[SessionInfo.currentUserID]);
            Company oCompany = new Company();
            oCompany = oCompany.Get(1, (int)Session[SessionInfo.currentUserID]);
            oCompany.CompanyLogo = GetCompanyLogo(oCompany);
            _oOrderRecapComment.Company = oCompany;

            string Messge = "Order Recap Comment List [Style No : " + _oOrderRecap.StyleNo + " Recap No : " + _oOrderRecap.OrderRecapNo + " ]";
            rptOrderRecapComments oReport = new rptOrderRecapComments();

            byte[] abytes = oReport.PrepareReport(_oOrderRecapComment, Messge);
            return File(abytes, "application/pdf");

        }
        #endregion

        #region Get Product BU, User and Name wise
        [HttpPost]
        public JsonResult GetFinishGoods(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.FinishGoods, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.FinishGoods, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult GetYarnCategory(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.RawMaterial, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.RawMaterial, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
         [HttpPost]
        public JsonResult GetPocketLinkFabric(Product oProduct)
        {
            List<Product> oProducts = new List<Product>();
            try
            {
                if (oProduct.ProductName != null && oProduct.ProductName != "")
                {
                    oProducts = Product.GetsPermittedProductByNameCode(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.PocketLinkFabric, oProduct.ProductName, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
                else
                {
                    oProducts = Product.GetsPermittedProduct(oProduct.BUID, EnumModuleName.OrderRecap, EnumProductUsages.PocketLinkFabric, ((User)Session[SessionInfo.CurrentUser]).UserID);
                }
            }
            catch (Exception ex)
            {
                oProduct = new Product();
                oProduct.ErrorMessage = ex.Message;
                oProducts.Add(oProduct);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oProducts);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        //
        [HttpPost]
         public JsonResult GetStyleFabrics(TechnicalSheet oTechnicalSheet)
        {
            List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
            try
            {
                StringBuilder sSQL = new StringBuilder("SELECT * FROM View_OrderRecapYarn WHERE RefObjectID =" + oTechnicalSheet.TechnicalSheetID + " AND RefType = "+(int)EnumRecapRefType.TechnicalSheet);
                oOrderRecapYarns = OrderRecapYarn.Gets(sSQL.ToString(), ((User)Session[SessionInfo.CurrentUser]).UserID);
               foreach(OrderRecapYarn oItem in oOrderRecapYarns)
               {
                   oItem.RefType = EnumRecapRefType.OrderRecap;
                   oItem.RefObjectID = 0;
                   oItem.OrderRecapYarnID = 0;
               }
            }
            catch (Exception ex)
            {
                OrderRecapYarn oOrderRecapYarn = new OrderRecapYarn();
                oOrderRecapYarn.ErrorMessage = ex.Message;
                oOrderRecapYarns.Add(oOrderRecapYarn);
            }
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string sjson = serializer.Serialize(oOrderRecapYarns);
            return Json(sjson, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}