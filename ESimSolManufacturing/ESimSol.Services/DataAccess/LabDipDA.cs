using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LabDipDA
    {
        public LabDipDA() { }

        #region Insert Update Delete Function
        public static IDataReader IUD(TransactionContext tc, LabDip oLabDip, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip]"
                                   + "%n, %s, %n, %n, %n, %n, %n, %n, %s, %n, %s, %n, %s, %n, %n, %n, %n, %D, %D, %n, %b, %n, %n,%n,%s, %n, %n, %n",
                                 oLabDip.LabDipID, oLabDip.LabdipNo, oLabDip.ContractorID, oLabDip.ContactPersonnelID, oLabDip.DeliveryToID, oLabDip.DeliveryToContactPersonnelID,
                                 oLabDip.DeliveryZoneID, oLabDip.RelabOn, oLabDip.PH, oLabDip.LightSourceID, oLabDip.BuyerRefNo, (int)oLabDip.PriorityLevel, oLabDip.Note, (int)oLabDip.OrderStatus,
                                 (int)oLabDip.LabDipFormat, (int)oLabDip.OrderReferenceType, oLabDip.OrderReferenceID, oLabDip.SeekingDate, oLabDip.OrderDate, oLabDip.MktPersonID, oLabDip.ISTwisted,
                                 oLabDip.FabricID, oLabDip.FSCDetailID,oLabDip.IsInHouse, oLabDip.DeliveryNote, oLabDip.LDTwistType, nUserID, nDBOperation);
        }
        public static IDataReader IUD_LD_Challan(TransactionContext tc, LabDip oLabDip, int nDBOperation, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_Challan]"
                                   + "%n, %s, %n, %n",
                                 oLabDip.LabDipID, oLabDip.ChallanNo, nUserID, nDBOperation);
        }
        public static IDataReader IUD_LD_Fabric(TransactionContext tc, LabDip oLabDip, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_Fabric]"
                                   + "%n, %n, %n",
                                 oLabDip.FSCDetailID, oLabDip.FabricID, nUserID);
        }
        #endregion

        #region Labdip Order Status
        public static IDataReader ChangeOrderStatus(TransactionContext tc, int nLabDipID, int nCurrentStatus, int nNextStatus, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDipHistory]"
                                  + "%n, %n, %n, %n",nLabDipID, nCurrentStatus, nNextStatus, nUserID);
        }
        #endregion

        #region Labdip Relab
        public static IDataReader Relab(TransactionContext tc, int nLabDipID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_Relab]" + "%n, %n", nLabDipID, nUserID);
        }
        #endregion

        #region Labdip Dyeing Order
        public static IDataReader CreateLabdipByDO(TransactionContext tc, int nDyeingOrderID, int nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_LabDip_DTMAVL]" + "%n, %n", nDyeingOrderID, nUserID);
        }
        #endregion

        #region Get Functions
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabDip WHERE LabDipID=%n", nID);
        }
        public static IDataReader Gets(TransactionContext tc, int nLabdipID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabDip WHERE LabdipID=%n", nLabdipID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("Select top(100)* from view_Labdip where OrderStatus in (0,1) order by OrderDate Desc");
        }
        public static IDataReader GetsPendingRecepie(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("SELECT * FROM [VIEW_LabdipAssignPersonnelList] %q ORDER BY EmployeeID", sSQL);
        }
        public static IDataReader GetsByOrderType(TransactionContext tc, int nOrdertype, string sSQL)
        {
            return tc.ExecuteReader("SELECT *  FROM View_LabDip WHERE Type=%n %q Order By OrderCreateDate, EWYDLNo", nOrdertype, sSQL);
        }
        public static IDataReader GetByFSD(TransactionContext tc, int nFSCDetailID)
        {
            return tc.ExecuteReader("SELECT * FROM View_LabDip WHERE FSCDetailID=%n", nFSCDetailID);
        }
        public static IDataReader GetByFSDMap(TransactionContext tc, string nFSCDetailIDs)
        {
            return tc.ExecuteReader("SELECT * FROM Labdip WHERE LabdipID IN (SELECT LabdipID FROM FSCLabMapping WHERE FSCDetailID IN ("+ nFSCDetailIDs + "))");
        }
        #endregion
    }
}
