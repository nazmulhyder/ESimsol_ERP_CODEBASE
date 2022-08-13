using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;



namespace ESimSol.BusinessObjects
{
    #region LeaveLedger

    public class LeaveLedger : BusinessObject
    {
        public LeaveLedger()
        {
            LeaveName = "";
            TotalDay = 0;

            AppliedFull = 0;
            AppliedHalf = 0;
            AppliedShort = 0;

            RecommendedFull = 0;
            RecommendedHalf = 0;
            RecommendedShort = 0;

            ApprovedFull = 0;
            ApprovedHalf = 0;
            ApprovedShort = 0;

            Paid = 0;
            Unpaid = 0;
            ErrorMessage = "";
            Params = "";
        }

        #region Properties

        public string LeaveName { get; set; }

        public int TotalDay { get; set; }

        public int AppliedFull { get; set; }

        public int AppliedHalf { get; set; }

        public int AppliedShort { get; set; }

        public int RecommendedFull { get; set; }

        public int RecommendedHalf { get; set; }

        public int RecommendedShort { get; set; }

        public int ApprovedFull { get; set; }

        public int ApprovedHalf { get; set; }

        public int ApprovedShort { get; set; }

        public int Paid { get; set; }

        public int Unpaid { get; set; }

        public string ErrorMessage { get; set; }

        public string Params { get; set; }

        #endregion

        #region Derive Property

        public int Addition { get; set; }
        public int Deduction { get; set; }

        public int RemainingLeave { get { return this.TotalDay + this.Addition - this.Deduction; } }
        public string HalfString { get { if (ApprovedHalf % 2 == 1) return ",1 Half"; else return " "; } }
        public string ShortString { get { if (ApprovedShort % 3 == 1) return ",2 Short"; else if (ApprovedShort % 3 == 2) return ",1 Short"; else return " "; } }

        public int Half { get { if (ApprovedHalf % 2 == 1) return ApprovedHalf - 1; else return 0; } }
        public int Short { get { if (ApprovedShort % 3 == 1) return ApprovedShort - 1; else if (ApprovedShort % 3 == 2) return ApprovedShort - 2; else return 0; } }


        public int Balance
        {
            get
            {
                if (this.RemainingLeave > 0)
                {
                    if (ApprovedHalf % 2 == 1 && (ApprovedShort % 3 == 1 || ApprovedShort % 3 == 2))
                        return RemainingLeave - 2 - ApprovedFull - Half / 2 - Short / 3;
                    else if (ApprovedHalf % 2 == 1 || ApprovedShort % 3 == 1 || ApprovedShort % 3 == 2)
                        return RemainingLeave - 1 - ApprovedFull - Half / 2 - Short / 3;
                    else
                        return RemainingLeave - ApprovedFull - Half / 2 - Short / 3;
                }
                else
                {
                    return 0;
                }
            }
        }

        public string BalanceString { get {  return (this.Balance>0)?  Balance.ToString() + " Full" + HalfString + ShortString :  ""; } }


        public string TransferLeaveInStr
        {
            get
            {
                if (this.Addition > 0 || this.Deduction > 0)
                {
                    return "[+]: " + this.Addition.ToString() + ",    [-]: " + this.Deduction.ToString();
                }
                else
                {
                    return "--";
                }
            }
        }

        #endregion Derive Property

        #region Functions
        public static List<LeaveLedger> Gets(int nEmployeeID, int nAcsID, long nUserID)
        {
            return LeaveLedger.Service.Gets(nEmployeeID, nAcsID, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ILeaveLedgerService Service
        {
            get { return (ILeaveLedgerService)Services.Factory.CreateService(typeof(ILeaveLedgerService)); }
        }

        #endregion
    }
    #endregion

    #region ILeaveLedger interface

    public interface ILeaveLedgerService
    {

        List<LeaveLedger> Gets(int nEmployeeID, int nAcsID, Int64 nUserID);
    }
    #endregion
}
