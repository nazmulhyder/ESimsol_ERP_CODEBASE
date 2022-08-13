using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class ImportPIDA
    {
        public ImportPIDA() { }

        #region Insert, Update, Delete

        public static IDataReader InsertUpdate(TransactionContext tc, ImportPI oImportPI, EnumDBOperation eEnumDBImportPI, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPI]"
                                    + "%n,                      %s,                                 %n,                                         %n,                                           %s,            %d,                     %d,                 %n,                         %n,                     %n,                                 %n,                     %n, "
                                    +" %n,                      %n,                                 %n,                                         %s,                                           %n,                                 %d, "
                                    + "%n,                      %b,                                 %b,                                         %n,                                           %b, %n, %d, %n, %d, %d, %s, %s, %n, %b, %n, %s,%n,%n,%n, %n, %n, %n, %n, %n , %n",
                                   oImportPI.ImportPIID,        oImportPI.ImportPINo,             oImportPI.BUID,                         oImportPI.ProductType,                              oImportPI.SLNo,                         oImportPI.IssueDate,    oImportPI.ReceiveDate,  
                                   oImportPI.ImportPIStatusInt, oImportPI.SupplierID,             oImportPI.ContactPersonID,              oImportPI.ConcernPersonID,                          oImportPI.CurrencyID, 
                                   oImportPI.TotalValue,        oImportPI.BankBranchID_Advise,    oImportPI.LCTermID,                     oImportPI.ContainerNo,                              oImportPI.ImportPITypeInt,              NullHandler.GetNullValue(oImportPI.AskingDeliveryDate), 
                                   oImportPI.ShipmentTermInt,   oImportPI.IsTransShipmentAllow,   oImportPI.IsPartShipmentAllow,          oImportPI.OverDueRate,                              oImportPI.IsLIBORrate,                  oImportPI.ApprovedBy,
                                   oImportPI.DateOfApproved,    oImportPI.VersionNumber,          oImportPI.ValidityDate,                 NullHandler.GetNullValue(oImportPI.AskingLCDate),   oImportPI.DeliveryClause,
                                   oImportPI.PaymentClause,     oImportPI.ShipmentBy,             oImportPI.IsReviseRequest,              oImportPI.PaymentInstructionTypeInt,                oImportPI.Note,                         oImportPI.AgentID, oImportPI.AgentContactPersonID, 
                                   oImportPI.RateUnit,          oImportPI.RefTypeInInt,           oImportPI.PIEntryTypeInInt,             oImportPI.ConvertionRate,                           oImportPI.DiscountAmount,               nUserId,                                            (int)eEnumDBImportPI);               
	                                                                                                                        
        }
        public static void Delete(TransactionContext tc, ImportPI oImportPI, EnumDBOperation eEnumDBImportPI, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ImportPI]" + "%n, %s, %n,%n, %s, %d,%D, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %d, %n, %b, %b, %n, %b, %n, %d, %n, %d, %d, %s, %s, %n, %b, %n, %s,%n,%n,%n, %n, %n,%n, %n, %n, %n",
                                   oImportPI.ImportPIID, oImportPI.ImportPINo, oImportPI.BUID, oImportPI.ProductType, oImportPI.SLNo, oImportPI.IssueDate, oImportPI.ReceiveDate, oImportPI.ImportPIStatusInt, oImportPI.SupplierID, oImportPI.ContactPersonID, oImportPI.ConcernPersonID, oImportPI.CurrencyID, oImportPI.TotalValue, oImportPI.BankBranchID_Advise, oImportPI.LCTermID, oImportPI.ContainerNo, oImportPI.ImportPITypeInt, NullHandler.GetNullValue(oImportPI.AskingDeliveryDate), oImportPI.ShipmentTermInt, oImportPI.IsTransShipmentAllow, oImportPI.IsPartShipmentAllow, oImportPI.OverDueRate, oImportPI.IsLIBORrate, oImportPI.ApprovedBy, oImportPI.DateOfApproved, oImportPI.VersionNumber, oImportPI.ValidityDate, NullHandler.GetNullValue(oImportPI.AskingLCDate), oImportPI.DeliveryClause, oImportPI.PaymentClause, oImportPI.ShipmentBy, oImportPI.IsReviseRequest, oImportPI.PaymentInstructionTypeInt, oImportPI.Note, oImportPI.AgentID, oImportPI.AgentContactPersonID, oImportPI.RateUnit, oImportPI.RefTypeInInt, oImportPI.PIEntryTypeInInt, oImportPI.ConvertionRate,  oImportPI.DiscountAmount, nUserId, (int)eEnumDBImportPI);
        }
        public static IDataReader AcceptRevise(TransactionContext tc, ImportPI oImportPI, EnumDBOperation eEnumDBImportPI, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ImportPILog]" + "%n, %s, %n, %s,%D, %D, %n, %n, %n, %n, %n, %n, %n, %n, %s, %n, %d, %n, %b, %b, %n, %b, %n, %d, %d,  %s, %s, %n, %b, %n, %s,%n,%n,%n, %n,%n,%b, %n, %n",
                                   oImportPI.ImportPIID, oImportPI.ImportPINo, oImportPI.BUID, oImportPI.SLNo, oImportPI.IssueDate, oImportPI.ReceiveDate, oImportPI.ImportPIStatusInt, oImportPI.SupplierID, oImportPI.ContactPersonID, oImportPI.ConcernPersonID, oImportPI.CurrencyID, oImportPI.TotalValue, oImportPI.BankBranchID_Advise, oImportPI.LCTermID, oImportPI.ContainerNo, oImportPI.ImportPITypeInt, NullHandler.GetNullValue(oImportPI.AskingDeliveryDate), oImportPI.ShipmentTermInt, oImportPI.IsTransShipmentAllow, oImportPI.IsPartShipmentAllow, oImportPI.OverDueRate, oImportPI.IsLIBORrate, oImportPI.VersionNumber, oImportPI.ValidityDate, NullHandler.GetNullValue(oImportPI.AskingLCDate), oImportPI.DeliveryClause, oImportPI.PaymentClause, oImportPI.ShipmentBy, oImportPI.IsReviseRequest, oImportPI.PaymentInstructionTypeInt, oImportPI.Note, oImportPI.AgentID, oImportPI.AgentContactPersonID, oImportPI.RateUnit, oImportPI.RefTypeInInt, oImportPI.PIEntryTypeInInt, oImportPI.IsCreateReviseNo, oImportPI.ConvertionRate, nUserId);
        }
        #endregion
        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, int nImportPIID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE ImportPIID=%n", nImportPIID);
        }

        public static IDataReader Get(TransactionContext tc, string sImportPINo)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE ImportPINo=%s ", sImportPINo);
        }
        public static IDataReader GetsByImportPIType(TransactionContext tc, string PCTypesIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE ImportPIType IN (" + PCTypesIDs + ") AND ImportPIStatus != '" + (int)EnumImportPIState.Accepted + "'");
        }
        public static IDataReader GetsByLCID(TransactionContext tc, int nLCID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE ImportPIID In (SELECT ImportPIID FROM ImportLCDetail WHERE ImportLCID = %n )", nLCID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader Gets(TransactionContext tc, int nSupplierID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE SupplierID=%n ", nSupplierID);
        }

        public static IDataReader GetsImportPI(TransactionContext tc, int nSupplierID, string sStatus, string sPCType)
        {
            string sSQL = " select * from View_ImportPI as aa where SupplierID=" + nSupplierID + "";            
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader UpdateAmount(TransactionContext tc, ImportPI oImportPI)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ImportPI Set TotalValue=%n WHERE ImportPIID=%n", oImportPI.TotalValue, oImportPI.ImportPIID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ImportPI WHERE ImportPIID=%n", oImportPI.ImportPIID);
        }

        #endregion

    }
}