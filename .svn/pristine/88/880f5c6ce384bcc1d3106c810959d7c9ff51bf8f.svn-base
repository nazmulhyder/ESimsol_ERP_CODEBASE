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
    #region RecapBillOfMaterial
    
    public class RecapBillOfMaterial : BusinessObject
    {
        public RecapBillOfMaterial()
        {
            RecapBillOfMaterialID = 0;
            TechnicalSheetID = 0;
            OrderRecapID = 0;
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
            BookingQty = 0;
            BookingConsumption = 0;
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
        public int RecapBillOfMaterialID { get; set; }
        public int OrderRecapID { get; set; }
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
        public bool IsApplyColor { get; set; }
        public bool IsApplySize { get; set; }
        public bool IsApplyMeasurement { get; set; }
        public System.Drawing.Image AttachImage { get; set; }
        public List<RecapBillOfMaterial> RecapBillOfMaterials { get; set; }
        public string ReqQtySt
        {
            get
            {
                return Global.MillionFormatActualDigit(this.ReqQty)+ " "+this.UnitName;
            }
        }
      
        #endregion

        #region Functions
        public RecapBillOfMaterial UpdateImage(long nUserID)
        {
            return RecapBillOfMaterial.Service.UpdateImage(this, nUserID);
        }
        public RecapBillOfMaterial DeleteImage(int id, long nUserID)
        {
            return RecapBillOfMaterial.Service.DeleteImage(id, nUserID);
        }
        public static RecapBillOfMaterial GetWithAttachFile(int id, long nUserID)
        {
            return RecapBillOfMaterial.Service.GetWithAttachFile(id, nUserID);
        }
        public static List<RecapBillOfMaterial> Gets_Report(int id, long nUserID)
        {
            return RecapBillOfMaterial.Service.Gets_Report(id, nUserID);
        }
        public static List<RecapBillOfMaterial> Gets(long nUserID)
        {
            return RecapBillOfMaterial.Service.Gets(nUserID);
        }
        public static List<RecapBillOfMaterial> Gets(int nTechnicalSheetID, long nUserID)
        {
            return RecapBillOfMaterial.Service.Gets(nTechnicalSheetID, nUserID);
        }
        public static List<RecapBillOfMaterial> PasteRecapBillOfMaterials(int nOldTSID, int nNewTSID, long nUserID)
        {
            return RecapBillOfMaterial.Service.PasteRecapBillOfMaterials(nOldTSID, nNewTSID, nUserID);
        }
        public static List<RecapBillOfMaterial> GetsWithImage(int nTechnicalSheetID, long nUserID)
        {
            return RecapBillOfMaterial.Service.GetsWithImage(nTechnicalSheetID, nUserID);
        }
        public static List<RecapBillOfMaterial> ResetSequence(List<RecapBillOfMaterial> oRecapBillOfMaterials, long nUserID)
        {
            return RecapBillOfMaterial.Service.ResetSequence(oRecapBillOfMaterials, nUserID);
        }
        public RecapBillOfMaterial Get(int id, long nUserID)
        {
            return RecapBillOfMaterial.Service.Get(id, nUserID);
        }
        public RecapBillOfMaterial Save(long nUserID)
        {
            return RecapBillOfMaterial.Service.Save(this, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return RecapBillOfMaterial.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

 
        internal static IRecapBillOfMaterialService Service
        {
            get { return (IRecapBillOfMaterialService)Services.Factory.CreateService(typeof(IRecapBillOfMaterialService)); }
        }

        #endregion
    }
    #endregion

    #region IRecapBillOfMaterial interface

    public interface IRecapBillOfMaterialService
    {
        RecapBillOfMaterial Get(int id, Int64 nUserID);
        List<RecapBillOfMaterial> Gets(Int64 nUserID);
        List<RecapBillOfMaterial> Gets(int nTechnicalSheetID, Int64 nUserID);
        List<RecapBillOfMaterial> PasteRecapBillOfMaterials(int nOldTSID, int nNewTSID, Int64 nUserID);
        List<RecapBillOfMaterial> GetsWithImage(int nTechnicalSheetID, Int64 nUserID);
        List<RecapBillOfMaterial> ResetSequence(List<RecapBillOfMaterial> oRecapBillOfMaterials, Int64 nUserID);
        List<RecapBillOfMaterial> Gets_Report(int id, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        RecapBillOfMaterial Save(RecapBillOfMaterial oRecapBillOfMaterial, Int64 nUserID);
        RecapBillOfMaterial UpdateImage(RecapBillOfMaterial oRecapBillOfMaterial, Int64 nUserID);
        RecapBillOfMaterial GetWithAttachFile(int id, Int64 nUserID);
        RecapBillOfMaterial DeleteImage(int id, Int64 nUserID);
    }
    #endregion
    

}
