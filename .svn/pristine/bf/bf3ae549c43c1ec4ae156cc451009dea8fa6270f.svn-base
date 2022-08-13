using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region VoucherBillAging
    public class VoucherBillAging : BusinessObject
    {
        public VoucherBillAging()
        {
            VoucherBillID = 0;
            BillNo = "";
            AccountHeadID = 0;
            SubLedgerID = 0;
            ErrorMessage = "";
            BillDate = DateTime.Now;
            DueDate = DateTime.Now;
            AccountHeadName = "";
            SubLedgerCode = "";
            SubLedgerName = "";
            OverDueDays = 0;
            DueDays = 0;
            IsDebit = false;
            Slab1 = 0;
            Slab2 = 0;
            Slab3 = 0;
            Slab4 = 0;
            Slab5 = 0;
            Slab6 = 0;
            Slab7 = 0;
            Slab8 = 0;
            Slab9 = 0;
            Slab10 = 0;
            Slab11 = 0;
            Slab12 = 0;
            Slab13 = 0;
            Slab14 = 0;
            Slab15 = 0;
            Slab16 = 0;
            Slab17 = 0;
            Slab18 = 0;
            Slab19 = 0;
            Slab20 = 0;
        }

        #region Properties
        public int VoucherBillID { get; set; }
        public int AccountHeadID { get; set; }
        public int SubLedgerID { get; set; }//ACCostCenterID
        public string BillNo { get; set; }
        public DateTime BillDate { get; set; }
        public DateTime DueDate { get; set; }
        public double Amount { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsDebit { get; set; }
        public string AccountHeadName { get; set; }
        public string SubLedgerCode { get; set; }
        public string SubLedgerName { get; set; }
        public int OverDueDays { get; set; }
        public int DueDays { get; set; }
        public double Slab1 { get; set; }
        public double Slab2 { get; set; }
        public double Slab3 { get; set; }
        public double Slab4 { get; set; }
        public double Slab5 { get; set; }
        public double Slab6 { get; set; }
        public double Slab7 { get; set; }
        public double Slab8 { get; set; }
        public double Slab9 { get; set; }
        public double Slab10 { get; set; }
        public double Slab11 { get; set; }
        public double Slab12 { get; set; }
        public double Slab13 { get; set; }
        public double Slab14 { get; set; }
        public double Slab15 { get; set; }
        public double Slab16 { get; set; }
        public double Slab17 { get; set; }
        public double Slab18 { get; set; }
        public double Slab19 { get; set; }
        public double Slab20 { get; set; }
        
        #region Derive Property
        public string AmountSt
        {
            get
            {
                if (this.Amount > 0)
                {
                    return Global.MillionFormat(this.Amount) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab1St
        {
            get
            {
                if (this.Slab1 > 0)
                {
                    return Global.MillionFormat(this.Slab1) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab2St
        {
            get
            {
                if (this.Slab2 > 0)
                {
                    return Global.MillionFormat(this.Slab2) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab3St
        {
            get
            {
                if (this.Slab3 > 0)
                {
                    return Global.MillionFormat(this.Slab3) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab4St
        {
            get
            {
                if (this.Slab4 > 0)
                {
                    return Global.MillionFormat(this.Slab4) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab5St
        {
            get
            {
                if (this.Slab5 > 0)
                {
                    return Global.MillionFormat(this.Slab5) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab6St
        {
            get
            {
                if (this.Slab6 > 0)
                {
                    return Global.MillionFormat(this.Slab6) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab7St
        {
            get
            {
                if (this.Slab7 > 0)
                {
                    return Global.MillionFormat(this.Slab7) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab8St
        {
            get
            {
                if (this.Slab8 > 0)
                {
                    return Global.MillionFormat(this.Slab8) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab9St
        {
            get
            {
                if (this.Slab9 > 0)
                {
                    return Global.MillionFormat(this.Slab9) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab10St
        {
            get
            {
                if (this.Slab10 > 0)
                {
                    return Global.MillionFormat(this.Slab10) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab11St
        {
            get
            {
                if (this.Slab11 > 0)
                {
                    return Global.MillionFormat(this.Slab11) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab12St
        {
            get
            {
                if (this.Slab12 > 0)
                {
                    return Global.MillionFormat(this.Slab12) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab13St
        {
            get
            {
                if (this.Slab13 > 0)
                {
                    return Global.MillionFormat(this.Slab13) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab14St
        {
            get
            {
                if (this.Slab14 > 0)
                {
                    return Global.MillionFormat(this.Slab14) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab15St
        {
            get
            {
                if (this.Slab15 > 0)
                {
                    return Global.MillionFormat(this.Slab15) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab16St
        {
            get
            {
                if (this.Slab16 > 0)
                {
                    return Global.MillionFormat(this.Slab16) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab17St
        {
            get
            {
                if (this.Slab17 > 0)
                {
                    return Global.MillionFormat(this.Slab17) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab18St
        {
            get
            {
                if (this.Slab18 > 0)
                {
                    return Global.MillionFormat(this.Slab18) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab19St
        {
            get
            {
                if (this.Slab19 > 0)
                {
                    return Global.MillionFormat(this.Slab19) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string Slab20St
        {
            get
            {
                if (this.Slab20 > 0)
                {
                    return Global.MillionFormat(this.Slab20) + (this.IsDebit == true ? " Dr" : " Cr");
                }
                else
                {
                    return "";
                }
            }
        }
        public string PartyName { get { return this.SubLedgerID > 0 ? this.SubLedgerName : this.AccountHeadName; } }
        public int OverDueByDays { get { return this.OverDueDays < 0 ? 0 : this.OverDueDays; } }
        public int DueForDays { get { return this.DueDays < 0 ? 0 : this.DueDays; } }
      
        public string DueDateSt
        {
            get { return this.DueDate.ToString("dd MMM yyyy"); }
        }
        public string BillDateSt
        {
            get { return this.BillDate.ToString("dd MMM yyyy"); }
        }
       
        #endregion

        #endregion

        #region Functions
        public VoucherBillAging Get(int id, int nUserID)
        {
            return VoucherBillAging.Service.Get(id, nUserID);
        }
        public VoucherBillAging Save(int nUserID)
        {
            return VoucherBillAging.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return VoucherBillAging.Service.Delete(id, nUserID);
        }
        public static List<VoucherBillAging> GetsReceivableOrPayableBill(int nComponentType, int nUserID)
        {
            return VoucherBillAging.Service.GetsReceivableOrPayableBill(nComponentType, nUserID);
        }
        public static List<VoucherBillAging> Gets(int nUserID)
        {
            return VoucherBillAging.Service.Gets(nUserID);
        }
        public static List<VoucherBillAging> Gets(string sSQL, int nUserID)
        {
            return VoucherBillAging.Service.Gets(sSQL, nUserID);
        }
        public static List<VoucherBillAging> GetsBy(int nAccountHeadID, int nUserID)
        {
            return VoucherBillAging.Service.GetsBy(nAccountHeadID, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IVoucherBillAgingService Service
        {
            get { return (IVoucherBillAgingService)Services.Factory.CreateService(typeof(IVoucherBillAgingService)); }
        }
        #endregion
    }
    #endregion
    
    #region IVoucherBillAging interface
    public interface IVoucherBillAgingService
    {
        VoucherBillAging Get(int id, int nUserID);
        string Delete(int id, int nUserID);
        VoucherBillAging Save(VoucherBillAging oVoucherBillAging, int nUserID);
        List<VoucherBillAging> GetsBy(int nAccountHeadID, int nUserID);
        List<VoucherBillAging> Gets(string sSQL, int nUserID);
        List<VoucherBillAging> Gets(int nUserID);
        List<VoucherBillAging> GetsReceivableOrPayableBill(int nComponentType, int nUserID);
    }
    #endregion
}
