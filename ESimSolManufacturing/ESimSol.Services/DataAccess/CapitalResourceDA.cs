using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class CapitalResourceDA
    {
        public CapitalResourceDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, CapitalResource oCapitalResource, int nDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_CapitalResource]"
                                    + "%n, %n, %s, %s, %n, %b, %s, %s, %s, %s, %s, %n, %s, %D, %D, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s, %b," +
                                      "%n, %s, %s, %s, %s, %s, %s, %s, %s, %s, %d, %s, %n,  %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s," +
                                      "%d,  %s, %n, %n, %n,%n,%n,%n,%n,%n,%n, %n, %n",
                                     oCapitalResource.CRID
                                    , oCapitalResource.CRGID
                                    , oCapitalResource.Code
                                    , oCapitalResource.Name
                                    , oCapitalResource.ParentID
                                    , oCapitalResource.IsLastLayer
                                    , oCapitalResource.Model
                                    , oCapitalResource.Brand
                                    , oCapitalResource.MadeIn
                                    , oCapitalResource.MadeBy
                                    , oCapitalResource.MachineCapacity
                                    , oCapitalResource.Warranty
                                    , oCapitalResource.WarrantyOn
                                    , NullHandler.GetNullValue(oCapitalResource.WarrantyStart)
                                    , NullHandler.GetNullValue(oCapitalResource.WarrantyEnd)
                                    , oCapitalResource.SerialNumberOnProduct
                                    , oCapitalResource.TagNo
                                    , oCapitalResource.ActualAssetValue
                                    , oCapitalResource.ValueAfterEvaluation
                                    , oCapitalResource.CNF_FOBValue_Foreign
                                    , oCapitalResource.CNF_FOBValue_Local
                                    , oCapitalResource.TotalLandedCost
                                    , oCapitalResource.InstallationCost
                                    , oCapitalResource.OtherCost
                                    , oCapitalResource.CurrencyID
                                    , oCapitalResource.Note
                                    , oCapitalResource.IsActive
                                    , oCapitalResource.SupplierID
                                    , oCapitalResource.SupplierAddress
                                    , oCapitalResource.SupplierContactPerson
                                    , oCapitalResource.SupplierContactPersonContact
                                    , oCapitalResource.SupplierNote
                                    , oCapitalResource.LAName
                                    , oCapitalResource.LAContactPerson
                                    , oCapitalResource.LAAddress
                                    , oCapitalResource.LAWorkshop
                                    , oCapitalResource.LANote
                                    , NullHandler.GetNullValue(oCapitalResource.InstallationDate)
                                    , oCapitalResource.InstallationNote
                                    , oCapitalResource.InstallationLocationID
                                    , oCapitalResource.BasicFunction
                                    , oCapitalResource.MachineLifeTime
                                    , oCapitalResource.PowerConsumption
                                    , oCapitalResource.TechnicalSpecification
                                    , oCapitalResource.PerformanceSpecification
                                    , oCapitalResource.PortOfShipment
                                    , oCapitalResource.LCNo
                                    , oCapitalResource.HSCode
                                    , oCapitalResource.SupplierEmail
                                    , oCapitalResource.SupplierPhone
                                    , oCapitalResource.SupplierFax
                                    , oCapitalResource.LAEmail
                                    , oCapitalResource.LAPhone
                                    , oCapitalResource.LAFax
                                    , NullHandler.GetNullValue(oCapitalResource.CommissioningDate)
                                    , oCapitalResource.CommissioningBy
                                    , oCapitalResource.InsuranceCost
                                    , oCapitalResource.CustomDutyCost
                                    , oCapitalResource.BUID
                                    , (int)oCapitalResource.ResourcesType
                                    , oCapitalResource.RackID
                                    , oCapitalResource.FinishGoodWeight
                                    , oCapitalResource.NaliWeight
                                    , oCapitalResource.FGWeightUnit
                                    , oCapitalResource.Cavity
                                    , nUserID
                                    , nDBOperation
                                  );
        }

        public static void Delete(TransactionContext tc, CapitalResource oCapitalResource, int nDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_CapitalResource]"
                                    + "%n, %n, %s, %s, %n, %b, %s, %s, %s, %s, %s, %n, %s, %D, %D, %s, %s, %n, %n, %n, %n, %n, %n, %n, %n, %s, %b," +
                                      "%n, %s, %s, %s, %s, %s, %s, %s, %s, %s, %d, %s, %n,  %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s," +
                                      "%d,  %s, %n, %n, %n,%n,%n,%n,%n,%n,%n, %n, %n",
                                     oCapitalResource.CRID
                                    , oCapitalResource.CRGID
                                    , oCapitalResource.Code
                                    , oCapitalResource.Name
                                    , oCapitalResource.ParentID
                                    , oCapitalResource.IsLastLayer
                                    , oCapitalResource.Model
                                    , oCapitalResource.Brand
                                    , oCapitalResource.MadeIn
                                    , oCapitalResource.MadeBy
                                    , oCapitalResource.MachineCapacity
                                    , oCapitalResource.Warranty
                                    , oCapitalResource.WarrantyOn
                                    , NullHandler.GetNullValue(oCapitalResource.WarrantyStart)
                                    , NullHandler.GetNullValue(oCapitalResource.WarrantyEnd)
                                    , oCapitalResource.SerialNumberOnProduct
                                    , oCapitalResource.TagNo
                                    , oCapitalResource.ActualAssetValue
                                    , oCapitalResource.ValueAfterEvaluation
                                    , oCapitalResource.CNF_FOBValue_Foreign
                                    , oCapitalResource.CNF_FOBValue_Local
                                    , oCapitalResource.TotalLandedCost
                                    , oCapitalResource.InstallationCost
                                    , oCapitalResource.OtherCost
                                    , oCapitalResource.CurrencyID
                                    , oCapitalResource.Note
                                    , oCapitalResource.IsActive
                                    , oCapitalResource.SupplierID
                                    , oCapitalResource.SupplierAddress
                                    , oCapitalResource.SupplierContactPerson
                                    , oCapitalResource.SupplierContactPersonContact
                                    , oCapitalResource.SupplierNote
                                    , oCapitalResource.LAName
                                    , oCapitalResource.LAContactPerson
                                    , oCapitalResource.LAAddress
                                    , oCapitalResource.LAWorkshop
                                    , oCapitalResource.LANote
                                    , NullHandler.GetNullValue(oCapitalResource.InstallationDate)
                                    , oCapitalResource.InstallationNote
                                    , oCapitalResource.InstallationLocationID
                                    , oCapitalResource.BasicFunction
                                    , oCapitalResource.MachineLifeTime
                                    , oCapitalResource.PowerConsumption
                                    , oCapitalResource.TechnicalSpecification
                                    , oCapitalResource.PerformanceSpecification
                                    , oCapitalResource.PortOfShipment
                                    , oCapitalResource.LCNo
                                    , oCapitalResource.HSCode
                                    , oCapitalResource.SupplierEmail
                                    , oCapitalResource.SupplierPhone
                                    , oCapitalResource.SupplierFax
                                    , oCapitalResource.LAEmail
                                    , oCapitalResource.LAPhone
                                    , oCapitalResource.LAFax
                                    , NullHandler.GetNullValue(oCapitalResource.CommissioningDate)
                                    , oCapitalResource.CommissioningBy
                                    , oCapitalResource.InsuranceCost
                                    , oCapitalResource.CustomDutyCost
                                    , oCapitalResource.BUID
                                    , (int)oCapitalResource.ResourcesType
                                    , oCapitalResource.RackID
                                    , oCapitalResource.FinishGoodWeight
                                    , oCapitalResource.NaliWeight
                                    , oCapitalResource.FGWeightUnit
                                    , oCapitalResource.Cavity
                                    , nUserID
                                    , nDBOperation
                                  );
        }

        public static IDataReader CopyCR(TransactionContext tc, int nCapitalResourceID, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_Copy_CapitalResource] %n,%n", nCapitalResourceID, nUserID);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nCRID, Int64 nUserId)
        {
            return tc.ExecuteReader("Select * From View_CapitalResource Where CRID=%n ", nCRID);
        }

        public static IDataReader Gets(TransactionContext tc, Int64 nUserId)
        {
            return tc.ExecuteReader("SELECT * FROM View_CapitalResource ");
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsResourceType(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_CapitalResource WHERE ParentID = 1 ORDER BY Name ASC");
        }
        public static IDataReader GetsResourceTypeWithBU(TransactionContext tc, int buid)
        {
            return tc.ExecuteReader("SELECT * FROM View_CapitalResource WHERE ParentID = 1 AND BUID = " + buid + " ORDER BY Name ASC");
        }
        public static IDataReader BUWiseGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CapitalResource Where BUID = %n AND ParentID > 1 Order By ResourcesType", nBUID);
        }
        public static IDataReader BUWiseResourceTypeGets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_CapitalResource Where BUID= %n AND ParentID = 1 Order By Name", nBUID);
        }
        public static IDataReader GetsByBUandResourceTypeWithName(TransactionContext tc, int nBUID, int nResourceType, string Name)
        {
            string sSQL = "SELECT * FROM View_CapitalResource Where ResourcesType = " + nResourceType + " AND IsActive = 1 AND BUID = " + nBUID;
            if (Name != "" && Name != null)
            {
                sSQL += " And Name Like '%" + Name + "%' Order By CRID";
            }
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
