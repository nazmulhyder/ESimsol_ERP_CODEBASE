using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core.DataAccess;

namespace ESimSol.Services.DataAccess
{


    public class InspectionCertificateDetailDA
    {
        public InspectionCertificateDetailDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, InspectionCertificateDetail oInspectionCertificateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sInspectionCertificateDetailIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_InspectionCertificateDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n,%s",
                                    oInspectionCertificateDetail.InspectionCertificateDetailID, oInspectionCertificateDetail.InspectionCertificateID, oInspectionCertificateDetail.CommercialInvoiceDetailID,oInspectionCertificateDetail.OrderQty,oInspectionCertificateDetail.ShipedQty,oInspectionCertificateDetail.CartonQty, nUserID, (int)eEnumDBOperation, "");
        }
        public static void Delete(TransactionContext tc, InspectionCertificateDetail oInspectionCertificateDetail, EnumDBOperation eEnumDBOperation, Int64 nUserID, String sInspectionCertificateDetailIDs)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_InspectionCertificateDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n,%s",
                                    oInspectionCertificateDetail.InspectionCertificateDetailID, oInspectionCertificateDetail.InspectionCertificateID, oInspectionCertificateDetail.CommercialInvoiceDetailID, oInspectionCertificateDetail.OrderQty, oInspectionCertificateDetail.ShipedQty, oInspectionCertificateDetail.CartonQty, nUserID, (int)eEnumDBOperation, sInspectionCertificateDetailIDs);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificateDetail WHERE InspectionCertificateDetailID=%n", nID);
        }
        public static IDataReader Gets(int nInspectionCertificateID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_InspectionCertificateDetail where InspectionCertificateID =%n", nInspectionCertificateID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        #endregion
    }
    
   
}
