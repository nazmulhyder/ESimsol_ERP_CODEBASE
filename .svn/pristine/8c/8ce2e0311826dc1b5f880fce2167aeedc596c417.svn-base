using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;
using System.Data.SqlClient;


namespace ESimSol.Services.DataAccess
{
    public class CandidateDA
    {
        public CandidateDA() { }

        #region Insert Update Delete Function

        public static IDataReader IUD(TransactionContext tc, Candidate oCandidate, Int64 nUserID, int nDBOperation)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Candidate] %n,%u,%u,%u,%u,%d,%s,%s,%u,%u,%u,%u,%u,%u,%u,%u,%u,%u,%n,%n,%u,%u,%d,%n",
                   oCandidate.CandidateID, oCandidate.Code, oCandidate.Name, oCandidate.FatherName, oCandidate.MotherName, oCandidate.DateOfBirth, oCandidate.Gender,
                   oCandidate.MaritalStatus, oCandidate.Nationalism, oCandidate.NationalID, oCandidate.Religious, oCandidate.PresentAddress,
                   oCandidate.ParmanentAddress, oCandidate.ContactNo, oCandidate.AlternateContactNo, oCandidate.Email, oCandidate.AlternateEmail,
                   oCandidate.Objective, oCandidate.PresentSalary, oCandidate.ExpectedSalary, oCandidate.CareerSummary, oCandidate.SpecialQualification,
                   oCandidate.LastUpdatedDateTime, nDBOperation);

        }
        public static void UpdatePhoto(TransactionContext tc, Candidate oCandidate, Int64 nUserID)
        {

            SqlParameter Photo = new SqlParameter();
            Photo.SqlDbType = SqlDbType.Image;
            Photo.ParameterName = "Photopic";
            Photo.Value = oCandidate.Photo;

            string sSQL = SQLParser.MakeSQL("UPDATE Candidate SET Photo=%q"
                + " WHERE CandidateID=%n", "@Photopic", oCandidate.CandidateID);

            tc.ExecuteNonQueryCommText(CommandType.Text, sSQL, Photo);
        }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nCandidateID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Candidate WHERE CandidateID=%n", nCandidateID);
        }
        public static IDataReader Get(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM Candidate");
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }

        #endregion
    }
}
