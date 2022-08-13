using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;


namespace ESimSol.Services.DataAccess
{
    public class ContractorDA
    {
        public ContractorDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Contractor oContractor, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Contractor]"
                                    + "%n,  %s, %s, %s, %s,%s, %s, %s, %s,%s, %s, %n, %s, %b,%s,%s,%s,%s,%n, %n ",
                                    oContractor.ContractorID, oContractor.Name, oContractor.Origin, oContractor.Address, oContractor.Address2, oContractor.Address3, oContractor.Phone, oContractor.Phone2, oContractor.Email, oContractor.ShortName, oContractor.Fax, oContractor.ActualBalance, oContractor.Note, oContractor.Activity, oContractor.TIN, oContractor.VAT, oContractor.GroupName,oContractor.Zone, nUserId, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, Contractor oContractor, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Contractor]"
                                    + "%n,  %s, %s, %s, %s,%s, %s, %s, %s,%s, %s, %n, %s, %b,%s,%s,%s,%s,%n, %n ",
                                   oContractor.ContractorID, oContractor.Name, oContractor.Origin, oContractor.Address, oContractor.Address2, oContractor.Address3, oContractor.Phone, oContractor.Phone2, oContractor.Email, oContractor.ShortName, oContractor.Fax, oContractor.ActualBalance, oContractor.Note, oContractor.Activity, oContractor.TIN, oContractor.VAT, oContractor.GroupName, oContractor.Zone, nUserId, (int)eEnumDBOperation);
        }

        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM Contractor WHERE ContractorID=%n", nID);
        }

        public static void CommitActivity(TransactionContext tc, int  id, bool Activity, Int64 nUserId)
        {
            tc.ExecuteNonQuery("Update Contractor SET Activity =%b  WHERE ContractorID = %n", Activity, id);
        }
        
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Contractor ORDER BY Name ASC");
        }
        public static IDataReader GetsByBU(TransactionContext tc, int nBUID)
        {
            return tc.ExecuteReader("SELECT * FROM Contractor AS TT WHERE TT.ContractorID IN (SELECT HH.ContractorID FROM BUWiseParty AS HH WHERE HH.BUID=%n) ORDER BY TT.Name ASC", nBUID);
        }
        public static IDataReader GetsForAccount(TransactionContext tc, int nContractorType, int nReferenceType)
        {
            return tc.ExecuteReader("SELECT * FROM Contractor where ContractorType=%n AND Activity=1 and ContractorID not in (Select ReferenceObjectID from COA_ChartsOfAccount where ReferenceObjectID>0 and ReferenceType=%n) order by Name", nContractorType, nReferenceType);


        }
        public static IDataReader Gets(TransactionContext tc, int eContractorType)
        {
            return tc.ExecuteReader("SELECT * FROM Contractor WHERE ContractorType=%n Order by [Name]", eContractorType);
        }
        public static IDataReader GetsByName(TransactionContext tc, string sName, int eContractorType)
        {
         
            return tc.ExecuteReader("SELECT * FROM Contractor WHERE Name LIKE ('%" + sName + "')  AND ContractorID in (Select ContractorID from ContractorType where ContractorType  in (" + eContractorType.ToString() + ")) AND Activity =1 Order by [Name]");
            
        }
        public static IDataReader GetsByNamenType(TransactionContext tc, string sName, string eContractorType, int nBUID)
        {
            string sSQL = "SELECT * FROM Contractor Where ContractorID>0 And Activity =1 ";
            if (eContractorType.Length>0)
            {
                sSQL = sSQL+" And ContractorID in (Select ContractorID from ContractorType where ContractorType  in("+ eContractorType+"))";
            }
            if (sName.Trim()!="")
            {
                sSQL = sSQL+" And Name LIKE ('%" + sName.Trim() + "%')";
            }
            if(nBUID>0)
            {
                sSQL = sSQL + " And ContractorID IN (SELECT ContractorID FROM BUWiseParty WHERE BUID = "+nBUID+")";
            }
            sSQL = sSQL + " Order by [Name]";


            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader UpdateNeedCutOff(TransactionContext tc, Contractor oContractor)
        {
            return tc.ExecuteReader("UPDATE Contractor SET IsNeedCutOff = %b  WHERE ContractorID = %n SELECT * FROM Contractor WHERE ContractorID = %n", oContractor.IsNeedCutOff, oContractor.ContractorID, oContractor.ContractorID);
            
        }
        public static IDataReader UpdateCountry(TransactionContext tc, Contractor oContractor)
        {
            return tc.ExecuteReader("UPDATE Contractor SET CountryID = %n  WHERE ContractorID = %n SELECT * FROM View_Contractor WHERE ContractorID = %n", oContractor.CountryID, oContractor.ContractorID, oContractor.ContractorID);
            
        }
        #endregion
    }
}
