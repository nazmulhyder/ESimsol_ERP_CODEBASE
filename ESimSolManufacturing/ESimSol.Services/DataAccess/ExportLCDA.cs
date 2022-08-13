using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.DataAccess
{
    public class ExportLCDA
    {
        public ExportLCDA() { }

        #region Insert Function
        public static IDataReader InsertUpdate(TransactionContext tc, ExportLC oExportLC, EnumDBOperation eEnumDBExportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportLC]"
                                    + "%n, %s, %s, %D, %n, %n,%n,%n, %n, %n, %n, %d, %d, %n, %s, %b, %s, %b, %b, %b, %b, %n, %s, %n, %d, %b,  %s, %s, %s,%b,%n,%b, %n, %s, %n, %s, %s, %d, %n, %n, %n, %n,%n, %n, %n",
                                     oExportLC.ExportLCID 
	                                ,oExportLC.ExportLCNo 
                                    ,oExportLC.FileNo 
	                                ,oExportLC.OpeningDate 
	                                ,oExportLC.BBranchID_Advice 
	                                ,oExportLC.BBranchID_Nego 
	                                ,oExportLC.BBranchID_Issue
	                                ,oExportLC.ApplicantID 
	                                ,oExportLC.ContactPersonalID 
	                                ,oExportLC.Amount 
	                                ,oExportLC.CurrencyID 
	                                ,oExportLC.ShipmentDate
	                                ,oExportLC.ExpiryDate 
	                                ,oExportLC.CurrentStatus
	                                ,oExportLC.Remark
	                                ,oExportLC.AtSightDiffered 
	                                ,oExportLC.ShipmentFrom 
	                                ,oExportLC.PartialShipmentAllowed
	                                ,oExportLC.TransShipmentAllowed 
	                                ,oExportLC.LiborRate
	                                ,oExportLC.BBankFDD
	                                ,oExportLC.OverDueRate
	                                ,oExportLC.OverDuePeriod
	                                ,oExportLC.VersionNo
	                                ,oExportLC.LCRecivedDate
	                                ,oExportLC.IsForeignBank 
	                                ,oExportLC.FrightPrepaid
	                                ,oExportLC.DarkMedium
	                                ,oExportLC.Year 
	                                ,oExportLC.GetOriginalCopy
	                                ,oExportLC.DCharge
	                                ,oExportLC.Stability
	                                ,oExportLC.LCTramsID
                                    ,oExportLC.GarmentsQty
                                    ,oExportLC.NegoDays
                                    ,oExportLC.HSCode
                                    ,oExportLC.AreaCode
                                    ,oExportLC.AmendmentDate
                                    ,oExportLC.BUID
                                    ,oExportLC.DeliveryToID
                                    ,oExportLC.BankBranchID_Forwarding
                                    ,oExportLC.PaymentInstruction
                                    , (int)oExportLC.ExportLCType
                                    ,nUserId,
                                    (int)eEnumDBExportLC);
        }
        
        public static void Delete(TransactionContext tc, ExportLC oExportLC, EnumDBOperation eEnumDBExportLC, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_ExportLC]"
                                    + "%n, %s, %s, %D, %n, %n,%n,%n, %n, %n, %n, %d, %d, %n, %s, %b, %s, %b, %b, %b, %b, %n, %s, %n, %d, %b,  %s, %s, %s,%b,%n,%b, %n, %s, %n, %s, %s, %d, %n, %n, %n, %n,%n,  %n, %n",
                                     oExportLC.ExportLCID
                                    , oExportLC.ExportLCNo
                                    , oExportLC.FileNo
                                    , oExportLC.OpeningDate
                                    , oExportLC.BBranchID_Advice
                                    , oExportLC.BBranchID_Nego
                                    , oExportLC.BBranchID_Issue
                                    , oExportLC.ApplicantID
                                    , oExportLC.ContactPersonalID
                                    , oExportLC.Amount
                                    , oExportLC.CurrencyID
                                    , oExportLC.ShipmentDate
                                    , oExportLC.ExpiryDate
                                    , oExportLC.CurrentStatus
                                    , oExportLC.Remark
                                    , oExportLC.AtSightDiffered
                                    , oExportLC.ShipmentFrom
                                    , oExportLC.PartialShipmentAllowed
                                    , oExportLC.TransShipmentAllowed
                                    , oExportLC.LiborRate
                                    , oExportLC.BBankFDD
                                    , oExportLC.OverDueRate
                                    , oExportLC.OverDuePeriod
                                    , oExportLC.VersionNo
                                    , oExportLC.LCRecivedDate
                                    , oExportLC.IsForeignBank
                                    , oExportLC.FrightPrepaid
                                    , oExportLC.DarkMedium
                                    , oExportLC.Year
                                    , oExportLC.GetOriginalCopy
                                    , oExportLC.DCharge
                                    , oExportLC.Stability
                                    , oExportLC.LCTramsID
                                    , oExportLC.GarmentsQty
                                    , oExportLC.NegoDays
                                    , oExportLC.HSCode
                                    , oExportLC.AreaCode
                                    , oExportLC.AmendmentDate
                                    , oExportLC.BUID
                                    , oExportLC.DeliveryToID
                                    , oExportLC.BankBranchID_Forwarding
                                    , oExportLC.PaymentInstruction
                                    , (int)oExportLC.ExportLCType
                                    , nUserId,
                                    (int)eEnumDBExportLC);
        }

        public static IDataReader InsertUpdateLog(TransactionContext tc, ExportLC oExportLC, EnumDBOperation eEnumDBExportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportLC_Log]"  + "%n,%s  ,%d	, %n, %n, %n, %n, %n, %n, %n,%d,%d	,%s	,%b	,%s	,%s	,%b	,%b	,%b	, %n, %s, %n,%d	,%b	,%s	,%s	,%s	,%b	, %n	,%b	, %n	,%s	, %n,%s,%s,%d	, %n, %n, %n, %n,%b	, %n, %n" ,
                                     oExportLC.ExportLCID
                                    , oExportLC.ExportLCNo
                                    , oExportLC.OpeningDate
                                    , oExportLC.BBranchID_Advice
                                    , oExportLC.BBranchID_Nego
                                    , oExportLC.BBranchID_Issue
                                    , oExportLC.ApplicantID   ////7
                                    , oExportLC.ContactPersonalID
                                    , oExportLC.Amount
                                    , oExportLC.CurrencyID //10
                                    , oExportLC.ShipmentDate
                                    , oExportLC.ExpiryDate
                                    , oExportLC.Remark
                                    , oExportLC.AtSightDiffered
                                    , oExportLC.ShipmentFrom
                                    , oExportLC.PartialShipmentAllowed
                                    , oExportLC.TransShipmentAllowed
                                    , oExportLC.LiborRate
                                    , oExportLC.BBankFDD
                                    , oExportLC.OverDueRate//20
                                    , oExportLC.OverDuePeriod
                                    , oExportLC.VersionNo
                                    , oExportLC.LCRecivedDate
                                    , oExportLC.IsForeignBank
                                    , oExportLC.FrightPrepaid
                                    , oExportLC.DarkMedium
                                    , oExportLC.Year
                                    , oExportLC.GetOriginalCopy
                                    , oExportLC.DCharge
                                    , oExportLC.Stability //30
                                    , oExportLC.LCTramsID
                                    , oExportLC.GarmentsQty
                                    , oExportLC.NegoDays
                                    , oExportLC.HSCode
                                    , oExportLC.AreaCode
                                    , oExportLC.AmendmentDate
                                    , oExportLC.BUID
                                    , oExportLC.DeliveryToID
                                    , oExportLC.BankBranchID_Forwarding
                                    , oExportLC.PaymentInstruction
                                    , oExportLC.IsAmendment
                                    , nUserId,
                                    (int)eEnumDBExportLC);
        }
   
        #endregion

        #region Delete Function
        public static void Delete(TransactionContext tc, int nID)
        {
            tc.ExecuteNonQuery("Update [PI] SET ApprovedStatus=2, LCID=0 WHERE LCID=%n", nID);//2 PiStatus Piissue 
            tc.ExecuteNonQuery("DELETE FROM ExportLC WHERE ExportLCID=%n", nID);
        }
        #endregion

        #region Update Function
        public static IDataReader UpdateForGetOrginalCopy(TransactionContext tc, int nExportLCID)
        {
            string sSQL1 = SQLParser.MakeSQL("Update ExportLC Set GetOriginalCopy=~GetOriginalCopy Where ExportLCID=%n", nExportLCID);
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE ExportLCID=" + nExportLCID);
        }

        public static IDataReader InsertUpdateLCStatus(TransactionContext tc, ExportLC oExportLC, EnumDBOperation eEnumDBExportLC, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_ExportLCStatusUpdate]"
                                    + "%n, %n, %s, %n, %n",
                                    oExportLC.ExportLCID
                                   , (int)oExportLC.CurrentStatus
                                   , oExportLC.Remark
                                    , nUserId,
                                    (int)eEnumDBExportLC);

        }

        public static IDataReader UpdateUDInfo(TransactionContext tc, ExportLC oExportLC,int nOperation)
        {
            string sSQL1 = "";
            if (nOperation == 1)
            {
                 sSQL1 = SQLParser.MakeSQL("UPDATE ExportLC SET NoteUD=%s WHERE ExportLCID=%n", oExportLC.NoteUD, oExportLC.ExportLCID);
            }
            else if (nOperation == 2)
            {
                 sSQL1 = SQLParser.MakeSQL("UPDATE ExportLC SET HaveQuery=%b,NoteQuery=%s WHERE ExportLCID=%n", oExportLC.HaveQuery, oExportLC.NoteQuery, oExportLC.ExportLCID);
            }
            tc.ExecuteNonQuery(sSQL1);
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE ExportLCID=" + oExportLC.ExportLCID);
          
        }
       
       
        #endregion

        #region Generation Function
        public static int GetNewID(TransactionContext tc)
        {
            return tc.GenerateID("ExportLC", "ExportLCID");
        }
        public static int GetNewLogID(TransactionContext tc)
        {
            return tc.GenerateID("ExportLCLog", "ExportLCID");
        }
        public static string GetAutoBLNo(TransactionContext tc, string sYear)
        {

            object MaxSNo = tc.ExecuteScalar("SELECT MAX(CONVERT(INTEGER, FileNo)) FROM ExportLC WHERE Year=%s", sYear);
            //if(MaxRSNo){return 1}//no need to execute for only once in the life time of this table-when there will no data
            int Temp = Convert.ToInt32(MaxSNo);
            Temp++;
            return Convert.ToString(Temp);
        }

        #endregion

        #region Get & Exist Function
        public static bool GetStability(TransactionContext tc, int nExportLCID)
        {
            object obj = tc.ExecuteScalar("SELECT Stability FROM ExportLC WHERE ExportLCID=%n ", nExportLCID);
            if (obj == null) return false;
            bool aaaa = false;
            aaaa = Convert.ToBoolean(obj);
            return aaaa;
        }
     
        public static IDataReader Get(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE ExportLCID=%n ", nID);
        }
        public static IDataReader GetLog(TransactionContext tc, int nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLCLog WHERE  ExportLCID=%n", nID);
        }
        public static IDataReader Get(TransactionContext tc,int nBUID, string sExportLCNo)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE BUID=%n and ExportLCNo=%s ", nBUID,sExportLCNo);
        }
     
        public static IDataReader GetByPIID(TransactionContext tc, int nPIID)
        {
            return tc.ExecuteReader("select * from View_ExportLC where exportlcid in(select LCID from [PI] where PIID =%n)", nPIID);
        }
        public static IDataReader Getlog(TransactionContext tc, DateTime dLogDate)
        {
            return tc.ExecuteReader("SELECT  * FROM ExportLCLog WHERE LogDate = %d", dLogDate);
        }
        public static IDataReader GetBylog(TransactionContext tc, int nLogOf)
        {
            return tc.ExecuteReader("SELECT  * FROM ExportLCLog WHERE ExportLCID in (SELECT MAX(ExportLCID) FROM  ExportLCLog WHERE LogOf = %n)", nLogOf);
        }
        public static IDataReader GetBy(TransactionContext tc, int nMax)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE ExportLCID=%n", nMax);
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC where CurrentStatus in (0,1) and BUID=%n", nBUID);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader("%q Order By [BankBranchID_Advice],[LCRecivedDate]", sSQL);
        }

        public static IDataReader GetsSQL(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, int nBUID, DateTime dLCDate)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE DBServerDate>=%d AND DBServerDate<%d  and BUID=%n order by CONVERT(INTEGER, FileNo) ", dLCDate, dLCDate.AddDays(1), nBUID);
        }

        //GetLogForVersionNo

        public static IDataReader GetLogForVersionNo(TransactionContext tc, int nExportLCID, int nVersionNo)
        {
            return tc.ExecuteReader("Select * from View_ExportLCLog WHERE ExportLCID=%n AND VersionNo=%n ", nExportLCID, nVersionNo);
        }
        public static IDataReader GetsLog(TransactionContext tc, int nExportLCID)
        {
            return tc.ExecuteReader("Select * from View_ExportLCLog WHERE ExportLCID=%n  ", nExportLCID);
        }
       
        public static IDataReader GetsForPolicy(TransactionContext tc, string sContractorsIDs)
        {
            return tc.ExecuteReader("SELECT * FROM View_ExportLC WHERE (ExportLCID NOT IN (SELECT DISTINCT LCID  FROM PolicyLC)) AND (ApplicantID IN (%q))", sContractorsIDs);
        }

        internal static bool IsExist(TransactionContext tc, string sExportLCNo, string sAmendmentNo, int nExportLCID)
        {
            object obj = tc.ExecuteScalar("SELECT ExportLCID FROM  View_ExportLC WHERE ExportLCNo=%s AND AmendmentNo=%s", sExportLCNo, sAmendmentNo);

            if (obj == null) return false;
            if ((int)obj == nExportLCID) return false;
            return true;
        }
      
    
        public static IDataReader GetExportLCPartyPerformance(TransactionContext tc, string sContractorID, DateTime dStartDate, DateTime dEndDate)
        {
            return tc.ExecuteReader(CommandType.StoredProcedure, "[sp_RPT_ExportLCPartyPerformance]",  sContractorID,  dStartDate,  dEndDate);
        }
        public static IDataReader GetExportLC_LCInHand(TransactionContext tc, string sContractorID )
        {
            return tc.ExecuteReader("Select * from View_LCBillProduct left join View_LCBill on View_LCBill.LCBillID=View_LCBillProduct.LCBillID where View_LCBillProduct.LCBillID " +" in (Select LCBill.LCBillID from LCBill where LCBill.IsActive=1 and [State]<5 ) and View_LCBill.ApplicantID in (%q) order by ApplicantID", sContractorID);
        }
        #endregion

    }
}
