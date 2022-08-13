using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess; 

namespace ESimSol.Services.DataAccess
{


    public class InspectionCertificateDA
    {
        public InspectionCertificateDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, InspectionCertificate oInspectionCertificate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC[SP_IUD_InspectionCertificate]" + "%n, %s, %d, %n, %n, %n, %s, %d, %n, %s, %d, %s, %d, %s,  %s, %s, %s, %n, %n, %n",
                                    oInspectionCertificate.InspectionCertificateID, oInspectionCertificate.RefNo,oInspectionCertificate.ICDate,oInspectionCertificate.CommercialInvoiceID,oInspectionCertificate.ShipperID,oInspectionCertificate.ManufacturerID,oInspectionCertificate.InvoiceNo,oInspectionCertificate.InvoiceDate,oInspectionCertificate.InvoiceValue,oInspectionCertificate.MasterLCNo,oInspectionCertificate.MasterLCDate,oInspectionCertificate.BillOfLadingNo,oInspectionCertificate.BillOfLadingDate,oInspectionCertificate.Vessel,oInspectionCertificate.PortOfLoading,oInspectionCertificate.FinalDestination,oInspectionCertificate.Remarks,oInspectionCertificate.AuthorizeCompany, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, InspectionCertificate oInspectionCertificate, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC[SP_IUD_InspectionCertificate]" + "%n, %s, %d, %n, %n, %n, %s, %d, %n, %s, %d, %s, %d, %s,  %s, %s, %s, %n, %n, %n",
                                    oInspectionCertificate.InspectionCertificateID, oInspectionCertificate.RefNo, oInspectionCertificate.ICDate, oInspectionCertificate.CommercialInvoiceID, oInspectionCertificate.ShipperID, oInspectionCertificate.ManufacturerID, oInspectionCertificate.InvoiceNo, oInspectionCertificate.InvoiceDate, oInspectionCertificate.InvoiceValue, oInspectionCertificate.MasterLCNo, oInspectionCertificate.MasterLCDate, oInspectionCertificate.BillOfLadingNo, oInspectionCertificate.BillOfLadingDate, oInspectionCertificate.Vessel, oInspectionCertificate.PortOfLoading, oInspectionCertificate.FinalDestination, oInspectionCertificate.Remarks, oInspectionCertificate.AuthorizeCompany, nUserID, (int)eEnumDBOperation);
        }


        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)  //nID  is Commercial Invoice ID
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificate WHERE CommercialInvoiceID = %n", nID);
        }

        public static IDataReader GetIC(TransactionContext tc, long nID)  //nID  is IC  ID
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificate WHERE InspectionCertificateID = %n", nID);
        }
        
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificate");
        }

        public static IDataReader Gets(TransactionContext tc, int id)
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificate WHERE LCTransferID = %n", id);
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    

}
