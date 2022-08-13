using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region BillOfMaterial

    public class BillOfMaterial : BusinessObject
    {
        public BillOfMaterial()
        {
            BillOfMaterialID = 0;
            TechnicalSheetID = 0;
            ProductID = 0;
            ColorID = 0;
            SizeID = 0;
            ItemDescription = "";
            Reference = "";
            Construction = "";
            Sequence = 0;
            MUnitID = 0;
            ReqQty = 0;
            CuttingQty = 0;
            ConsumptionQty = 0;
            ProductCode = "";
            ProductName = "";
            ColorName = "";
            SizeName = "";
            Symbol = "";
            UnitName = "";
            StyleNo = "";
            OrderQty = 0;
            BookingQty  = 0;
            BookingConsumption  = 0;
            BookingConsumptionInPercent = 0;
            POCode = "";
            Remarks = "";

            AttachFile = null;
            IsApplyColor = false;
            IsApplySize = false;
            IsApplyMeasurement = false;
            IsAttachmentExist = false;
            ErrorMessage = "";
        }

        #region Properties
        public int BillOfMaterialID { get; set; }
        public int TechnicalSheetID { get; set; }
        public int ProductID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public string ItemDescription { get; set; }
        public string Reference { get; set; }
        public string Construction { get; set; }
        public int Sequence { get; set; }
        public int MUnitID { get; set; }
        public double ReqQty { get; set; }
        public double CuttingQty { get; set; }
        public double ConsumptionQty { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string Symbol { get; set; }
        public string UnitName { get; set; }
        public byte[] AttachFile { get; set; }
        public bool IsAttachmentExist { get; set; }
        public string StyleNo { get; set; }
        public double OrderQty { get; set; }
        public double BookingQty { get; set; }
        public double BookingConsumption { get; set; }
        public double BookingConsumptionInPercent { get; set; }
        public string POCode { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public System.Drawing.Image AttachImage { get; set; }
        public bool IsApplyColor { get; set; }
        public bool IsApplySize { get; set; }
        public bool IsApplyMeasurement { get; set; }
        public List<BillOfMaterial> BillOfMaterials { get; set; }
        public string ReqQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.ReqQty);
            }
        }
        public string CuttingQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.CuttingQty);
            }
        }
        public string ConsumptionQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.ConsumptionQty);
            }
        }
        public string AttachFileInString
        {
            get
            {
                if (this.IsAttachmentExist)
                {
                    return this.BillOfMaterialID.ToString();
                }
                else
                {
                    return "0";
                }
            }
        }
        #endregion

        #region Functions
        public BillOfMaterial UpdateImage(long nUserID)
        {
            return BillOfMaterial.Service.UpdateImage(this, nUserID);
        }
        public BillOfMaterial DeleteImage(int id, long nUserID)
        {
            return BillOfMaterial.Service.DeleteImage(id, nUserID);
        }
        public static BillOfMaterial GetWithAttachFile(int id, long nUserID)
        {
            return BillOfMaterial.Service.GetWithAttachFile(id, nUserID);
        }
        public static List<BillOfMaterial> Gets_Report(int id, long nUserID)
        {
            return BillOfMaterial.Service.Gets_Report(id, nUserID);
        }
        public static List<BillOfMaterial> Gets(long nUserID)
        {
            return BillOfMaterial.Service.Gets(nUserID);
        }
        public static List<BillOfMaterial> Gets(int nTechnicalSheetID, long nUserID)
        {
            return BillOfMaterial.Service.Gets(nTechnicalSheetID, nUserID);
        }
        public static List<BillOfMaterial> PasteBillOfMaterials(int nOldTSID, int nNewTSID, long nUserID)
        {
            return BillOfMaterial.Service.PasteBillOfMaterials(nOldTSID, nNewTSID, nUserID);
        }
        public static List<BillOfMaterial> GetsWithImage(int nTechnicalSheetID, long nUserID)
        {
            return BillOfMaterial.Service.GetsWithImage(nTechnicalSheetID, nUserID);
        }
        public static List<BillOfMaterial> ResetSequence(List<BillOfMaterial> oBillOfMaterials, long nUserID)
        {
            return BillOfMaterial.Service.ResetSequence(oBillOfMaterials, nUserID);
        }
        public BillOfMaterial Get(int id, long nUserID)
        {
            return BillOfMaterial.Service.Get(id, nUserID);
        }
        public BillOfMaterial Save(long nUserID)
        {
            return BillOfMaterial.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return BillOfMaterial.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IBillOfMaterialService Service
        {
            get { return (IBillOfMaterialService)Services.Factory.CreateService(typeof(IBillOfMaterialService)); }
        }

        #endregion
    }
    #endregion

    #region IBillOfMaterial interface

    public interface IBillOfMaterialService
    {
        BillOfMaterial Get(int id, Int64 nUserID);
        List<BillOfMaterial> Gets(Int64 nUserID);
        List<BillOfMaterial> Gets(int nTechnicalSheetID, Int64 nUserID);
        List<BillOfMaterial> PasteBillOfMaterials(int nOldTSID, int nNewTSID, Int64 nUserID);
        List<BillOfMaterial> GetsWithImage(int nTechnicalSheetID, Int64 nUserID);
        List<BillOfMaterial> ResetSequence(List<BillOfMaterial> oBillOfMaterials, Int64 nUserID);
        List<BillOfMaterial> Gets_Report(int id, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        BillOfMaterial Save(BillOfMaterial oBillOfMaterial, Int64 nUserID);
        BillOfMaterial UpdateImage(BillOfMaterial oBillOfMaterial, Int64 nUserID);
        BillOfMaterial GetWithAttachFile(int id, Int64 nUserID);
        BillOfMaterial DeleteImage(int id, Int64 nUserID);
    }
    #endregion
}