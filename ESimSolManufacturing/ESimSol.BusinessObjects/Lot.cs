using System;
using System.IO;
using System.Data;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region Lot
    
    public class Lot : BusinessObject
    {
        public Lot()
        {
            LotID = 0;
            ProductID = 0;
            LotNo = "";
            LogNo = "";
            Balance = 0.0;
            MUnitID = 0;
            UnitPrice = 0.0;
            CurrencyID = 0;
            ParentLotID = 0;
            ParentType = EnumTriggerParentsType.None;
            ParentID = 0;
            WorkingUnitID = 0;
            ModelReferenceID = 0;
            StyleID = 0;
            ColorID = 0;
            SizeID = 0;
            ProductCode = "";
            ProductName = "";
            LocationName = "";
            OperationUnitName = "";
            StyleNo = "";
            BuyerName = "";
            ColorName = "";
            SizeName = "";
            ModelReferenceName = "";
            Currency = "";
            UnitType = EnumWoringUnitType.None;
            UnitTypeInInt = 0;
            ReportingBalance = 0;
            ReportUnitSymbol = "";
            Origin = "";
            ErrorMessage = "";
            PCID = 0;
            Layout = 0;
            ContractorID = 0;
            ContractorName = "";
            SupplierName = "";
            AgingDays = 0;
            ProductBaseID = 0;
            Params = string.Empty;
            Lots = new List<Lot>();
            Company = new Company();
            //IsRunning = false;
            LotStatus = EnumLotStatus.Open;
            WeightPerCartoon = 0;
            ConePerCartoon = 0;
            FinishDia = "";
            MCDia = "";
            GSM = "";
            LotGrade = "";
            WUWeftLot = "";
            WUWrapLot = "";
            FCUnitPrice = 0;
            FCCurrencyID = 0;
            BalanceInMtr = 0;
            Shade = "";
            Stretch_Length = "";
            RackID = 0;
            ProductCategoryID = 0;
            ProductCategoryName = "";
            ProductGroupName = "";
            YetQty = 0;
        }

        #region Properties For Lot
        public int LotID { get; set; }
        public int ProductID { get; set; }
        public string LotNo { get; set; }
        public string LogNo { get; set; }
        public double Balance { get; set; }
        public double BalanceInMtr { get; set; }
        public int MUnitID { get; set; }
        public double UnitPrice { get; set; }
        public double StockValue { get; set; }
        public int CurrencyID { get; set; } 
        public int ParentLotID { get; set; }
        public int ProductBaseID { get; set; }
        public EnumTriggerParentsType ParentType { get; set; }
        public EnumWoringUnitType UnitType { get; set; }
        public EnumLotStatus LotStatus { get; set; }
        public int UnitTypeInInt { get; set; }
        public string Shade { get; set; }
        public string Stretch_Length { get; set; }
        public int ParentID { get; set; }
        public int WorkingUnitID { get; set; }
        public int ModelReferenceID { get; set; }
        public int StyleID { get; set; }
        public int ColorID { get; set; }
        public int SizeID { get; set; }
        public string StyleNo { get; set; }
        public string BuyerName { get; set; }
        public string ColorName { get; set; }
        public string SizeName { get; set; }
        public string ModelReferenceName { get; set; }
        public string Currency { get; set; }
        public double ReportingBalance { get; set; }
        public string ReportUnitSymbol { get; set; }
        public string Origin { get; set; }
        public int ContractorID { get; set; }
        public string SupplierName { get; set; }
        //public bool IsRunning { get; set; }
        public double WeightPerCartoon { get; set; }
        public double ConePerCartoon { get; set; }
        public string FinishDia { get; set; }
        public string MCDia { get; set; }
        public string GSM { get; set; }
        public string LotGrade { get; set; }
        public string WUWrapLot { get; set; }
        public string WUWeftLot { get; set; }
        public double FCUnitPrice { get; set; }     //Foreign Currency Unit Price
        public int FCCurrencyID { get; set; }
        public int RackID { get; set; }
        public int ProductCategoryID { get; set; }
        public string ProductCategoryName { get; set; }
        public string ProductGroupName { get; set; }
        public double YetQty { get; set; }
        #endregion

        #region Derive Properties
        public string RackNo { get; set; }
        public int ShelfID { get; set; }
        public string ShelfName { get; set; }
        public string SupplierShortName { get; set; }
        public string ContractorName { get; set; }
        public string FCSymbol { get; set; }        //Foreign Currency Symbol
        public string Params { get; set; }
        public List<Lot> Lots { get; set; }
        public Company Company { get; set; }
        public double LotBalance
        {
            get
            {
                return this.Balance;
            }
        }
        public string ParentTypeSt
        {
            get
            {
                return EnumObject.jGet(this.ParentType);
            }
        }
        public string ShelfWithRackNo
        {
            get
            {
                if (string.IsNullOrEmpty(ShelfName) && string.IsNullOrEmpty(RackNo))
                {
                    return "";
                }
                return this.ShelfName + "[" + this.RackNo + "]";
            }
        }
        public int PCID { get; set; }
        public int Layout { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string OperationUnitName { get; set; }
        public string LocationName { get; set; }
        public string OUShortName { get; set; }
        public string LocationShortName { get; set; }
        public int BUID { get; set; }
        public string OrderRecapNo { get; set; }
        public string StoreName { get; set; }
        public int OrderRecapID { get; set; }
        public int TotalQuantity { get; set; }
        public int AlreadyShipmentQty { get; set; }
        public int YetToShipmentQty { get; set; }
        public string ProductNameCode
        {            
            get 
            {
                if (this.LotID > 0)
                {
                    return this.ProductName + "[" + this.ProductCode + "]";
                }
                else
                {
                    return "";
                }
            }
        }
        public string LotNoWithSupplierName
        {
            get
            {
                 return this.LotNo+""+this.ContractorName;  
            }
        }
        public string BalanceSt
        {            
            get 
            {
                if (this.MUName !=null)
                {
                    return Global.MillionFormat(this.Balance) + " " + this.MUName.PadLeft(4);
                }
                else
                {
                    return "";
                }
            }
        }
        public string StoreShortName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.LocationShortName))
                {
                    if (!string.IsNullOrEmpty(this.OUShortName))
                        return this.LocationShortName + "[" + this.OUShortName + "]";
                    else 
                        return this.LocationShortName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(this.OUShortName))
                        return "[" + this.OUShortName + "]";
                    else
                        return "";
                }
            }
        }
        public string ReportingBalanceSt
        {
            get 
            {
                if (this.ReportUnitSymbol != null)
                {
                    return Global.MillionFormat(this.ReportingBalance) + " " + this.ReportUnitSymbol.PadLeft(4);
                }
                else
                {
                    return "";
                }
            }
        }
        public string BalanceQtySt
        {
            get { return Global.MillionFormat(this.Balance); }

        }
        public string UnitPriceSt
        {
            get { return Global.MillionFormat(this.UnitPrice); }

        }
        public string FCUnitPriceSt
        {
            get { return this.FCUnitPrice.ToString("#,##0.000"); }

        }
        public string StockValueSt
        {
            get { return Global.MillionFormat(this.StockValue); }

        }
        public string WorkingUnitName
        {
            get { return this.LocationName + "[" + this.OperationUnitName + "]";}

        }
        public string LotWithBalance
        {
            get { return this.LotNo + "[" + Global.MillionFormat(this.Balance) + "]" +"["+ this.MUName +"]"; }

        }
        public string Amount{ get; set; }
        public string ErrorMessage { get; set; }
        public string MUName { get; set; }
        public int AgingDays { get; set; }
        public DateTime LastDate { get; set; }
        public string LastDateSt
        {
            get
            {
                return LastDate.ToString("dd MMM yyyy");
            }
        }
      
        public string LotStatusSt
        {
            get { return EnumObject.jGet(this.LotStatus); }
        }
        #endregion

        #region Functions
        public Lot UploadLot(int nUserID)
        {
            return Lot.Service.UploadLot(this, nUserID);
        }
        public Lot Get(int nLotID, long nUserID)
        {
            return Lot.Service.Get(nLotID, nUserID);
        }
        public Lot Get(int eParentType, int nParentID, int nWorkingUnitID, int nProductID, long nUserID)
        {
            return Lot.Service.Get(eParentType, nParentID, nWorkingUnitID, nProductID, nUserID);
        }
        public Lot GetByProductID( int nProductID, bool bIsZeroBalance, long nUserID)
        {
            return Lot.Service.GetByProductID( nProductID, bIsZeroBalance, nUserID);
        }
        public static Lot GetByLotNo(string sLotNo, int nBUID, int nStoreID, long nUserID)
        {
            return Lot.Service.GetByLotNo(sLotNo, nBUID, nStoreID, nUserID);
        }
      
        public static List<Lot> GetsBy(string sProductIds, string sWorkingUnitID, long nUserID)
        {
            return Lot.Service.GetsBy(sProductIds, sWorkingUnitID, nUserID);
        }
        public static List<Lot> GetsZeroBalance(string sProductIds, string sWorkingUnitID, long nUserID)
        {
            return Lot.Service.GetsZeroBalance(sProductIds, sWorkingUnitID, nUserID);
        }
        public static List<Lot> Gets(string sSQL, long nUserID)
        {
            return Lot.Service.Gets(sSQL, nUserID);
        }        
        public Lot Save(long nUserID)
        {
            return Lot.Service.Save(this, nUserID);
        }
        public Lot UpdateRack(long nUserID)
        {
            return Lot.Service.UpdateRack(this, nUserID);
        }
        public Lot UpdateLotPrice(long nUserID)
        {
            return Lot.Service.UpdateLotPrice(this, nUserID);
        }
        
        public Lot CommitIsRunning(long nUserID)
        {
            return Lot.Service.CommitIsRunning(this, nUserID);
        }
        public string UpdateStatus(long nUserID)
        {
            return Lot.Service.UpdateStatus(this, nUserID);
        }
        public static DataSet GetsDataSet(string sSQL, Int64 nUserID)
        {
            return Lot.Service.GetsDataSet(sSQL, nUserID);
        }
        #endregion

        #region User Define Functions
        #region Object Mapping
        private static Lot MappingObject(DataRow oDataRow)
        {
            Lot oLot = new Lot();
            oLot.LotID = (oDataRow["LotID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["LotID"]);
            oLot.LotNo = (oDataRow["LotNo"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LotNo"]);
            oLot.ProductID = (oDataRow["ProductID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ProductID"]);
            oLot.ProductCode = (oDataRow["ProductCode"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ProductCode"]);
            oLot.ProductName = (oDataRow["ProductName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ProductName"]);
            oLot.ProductCategoryID = (oDataRow["ProductCategoryID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ProductCategoryID"]);
            oLot.ProductCategoryName = (oDataRow["ProductCategoryName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ProductCategoryName"]);
            oLot.ProductBaseID = (oDataRow["ProductBaseID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["ProductBaseID"]);
            oLot.ProductGroupName = (oDataRow["ProductGroupName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ProductGroupName"]);
            oLot.ColorName = (oDataRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ColorName"]);
            oLot.WorkingUnitID = (oDataRow["WorkingUnitID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["WorkingUnitID"]);
            oLot.OperationUnitName = (oDataRow["OperationUnitName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["OperationUnitName"]);
            oLot.LocationName = (oDataRow["LocationName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["LocationName"]);
            oLot.ColorName = (oDataRow["ColorName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ColorName"]);
            oLot.MUName = (oDataRow["MUName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["MUName"]);
            oLot.SupplierName = (oDataRow["SupplierName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["SupplierName"]);
            oLot.StyleNo = (oDataRow["StyleNo"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["StyleNo"]);
            oLot.Balance = (oDataRow["Balance"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["Balance"]);
            oLot.ReportingBalance = (oDataRow["ReportingBalance"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ReportingBalance"]);
            oLot.ReportUnitSymbol = (oDataRow["ReportUnitSymbol"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["ReportUnitSymbol"]);
            oLot.UnitPrice = (oDataRow["UnitPrice"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["UnitPrice"]);
            oLot.StockValue = (oDataRow["StockValue"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["StockValue"]);
            oLot.WeightPerCartoon = (oDataRow["WeightPerCartoon"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["WeightPerCartoon"]);
            oLot.ConePerCartoon = (oDataRow["ConePerCartoon"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["ConePerCartoon"]);
            oLot.BuyerName = (oDataRow["BuyerName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["BuyerName"]);
            oLot.MCDia = (oDataRow["MCDia"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["MCDia"]);
            oLot.FinishDia = (oDataRow["FinishDia"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["FinishDia"]);
            oLot.GSM = (oDataRow["GSM"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["GSM"]);
            oLot.Shade = (oDataRow["Shade"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["Shade"]);
            oLot.Stretch_Length = (oDataRow["Stretch_Length"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["Stretch_Length"]);
            oLot.RackNo = (oDataRow["RackNo"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["RackNo"]);
            oLot.SizeName = (oDataRow["SizeName"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["SizeName"]);
            oLot.FCUnitPrice = (oDataRow["FCUnitPrice"] == DBNull.Value) ? 0 : Convert.ToDouble(oDataRow["FCUnitPrice"]);
            oLot.FCSymbol = (oDataRow["FCSymbol"] == DBNull.Value) ? "" : Convert.ToString(oDataRow["FCSymbol"]);
            //oLot.FCCurrencyID = (oDataRow["FCCurrencyID"] == DBNull.Value) ? 0 : Convert.ToInt32(oDataRow["FCCurrencyID"]);
            return oLot;
        }
        #endregion

        #region CreateObject
        public static Lot CreateObject(DataRow oDataRow)
        {
            Lot oLot = new Lot();
            oLot = MappingObject(oDataRow);
            return oLot;
        }
        #endregion

        #region CreateObjects
        public static List<Lot> CreateObjects(DataTable oDataTable)
        {
            List<Lot> oLots = new List<Lot>();
            foreach (DataRow oDataRow in oDataTable.Rows)
            {
                Lot oItem = CreateObject(oDataRow);
                oLots.Add(oItem);
            }
            return oLots;
        }
        public static List<Lot> CreateObjects(DataRow[] oDataRows)
        {
            List<Lot> oLots = new List<Lot>();
            foreach (DataRow oDataRow in oDataRows)
            {
                Lot oItem = CreateObject(oDataRow);
                oLots.Add(oItem);
            }
            return oLots;
        }
        #endregion
        #endregion

        #region ServiceFactory
        internal static ILotService Service
        {
            get { return (ILotService)Services.Factory.CreateService(typeof(ILotService)); }
        }
        #endregion
             
        #region NonDB
       

        #endregion
    }
    #endregion
    
    #region ILot
    
    public interface ILotService
    {
        Lot UploadLot(Lot oLot, int nUserID);
        Lot Get(int nLotID, Int64 nUserID);
        Lot Get(int eParentType, int nParentID, int nWorkingUnitID, int nProductID, Int64 nUserID);
        Lot GetByProductID(int nProductID, bool bIsZeroBalance, Int64 nUserID);
        Lot GetByLotNo(string sLotNo, int nBUID, int nStoreID, Int64 nUserID);
        List<Lot> GetsBy(string sProductIds, string sWorkingUnitID, Int64 nUserID);
        List<Lot> GetsZeroBalance(string sProductIds, string sWorkingUnitID, Int64 nUserID);
        List<Lot> Gets(string sSQL, Int64 nUserID);
        Lot Save(Lot oLot, long nUserID);
        Lot UpdateRack(Lot oLot, long nUserID);
        Lot UpdateLotPrice(Lot oLot, long nUserID);
        DataSet GetsDataSet(string sSQL, Int64 nUserID);
        Lot CommitIsRunning(Lot oLot, Int64 nUserID);
        string UpdateStatus(Lot oLot, Int64 nUserID);
    }
    #endregion 
}