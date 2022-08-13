using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;


namespace ESimSol.BusinessObjects
{

    #region HeadDisplayConfigure
    public class HeadDisplayConfigure : BusinessObject
    {
        public HeadDisplayConfigure()
        {
            HeadDisplayConfigureID=0;
            VoucherTypeID=0;
            IsDebit=true;
            SubGroupID=0;
            AccountHeadCodeName = "";
            VoucherName = "";
            IsDebit = true;
            ErrorMessage = "";
            HeadDisplayConfigures = new List<HeadDisplayConfigure>();
        }
        
        #region Properties
        public int HeadDisplayConfigureID { get; set; }
        public int VoucherTypeID { get; set; }
        public bool IsDebit { get; set; }
        public int SubGroupID { get; set; }
        public string AccountHeadCodeName { get; set; }
        public string VoucherName { get; set; }
        public string ErrorMessage { get; set; }
        public string IsDebitInString
        {
            get
            {
                if (this.IsDebit)
                {
                    return "Debit";
                }
                else
                {
                    return "Credit";
                }
            }
        }
        public List<HeadDisplayConfigure> HeadDisplayConfigures { get; set; }
        #endregion

        #region Functions
        public static List<HeadDisplayConfigure> Gets(int nVoucherTypeID, int nUserID)
        {
            return HeadDisplayConfigure.Service.Gets(nVoucherTypeID, nUserID);
        }
        public HeadDisplayConfigure Get(int id, int nUserID)
        {
            return HeadDisplayConfigure.Service.Get(id, nUserID);
        }
        public HeadDisplayConfigure Save(int nUserID)
        {
            return HeadDisplayConfigure.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return HeadDisplayConfigure.Service.Delete(id, nUserID);
        }
        public static List<HeadDisplayConfigure> Gets(int nUserID)
        {
            return HeadDisplayConfigure.Service.Gets(nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IHeadDisplayConfigureService Service
        {
            get { return (IHeadDisplayConfigureService)Services.Factory.CreateService(typeof(IHeadDisplayConfigureService)); }
        }
        #endregion
    }
    #endregion

    //#region HeadDisplayConfigures
    //public class HeadDisplayConfigures : IndexedBusinessObjects
    //{
    //    #region Collection Class Methods
    //    public void Add(HeadDisplayConfigure item)
    //    {
    //        base.AddItem(item);
    //    }
    //    public void Remove(HeadDisplayConfigure item)
    //    {
    //        base.RemoveItem(item);
    //    }
    //    public HeadDisplayConfigure this[int index]
    //    {
    //        get { return (HeadDisplayConfigure)GetItem(index); }
    //    }
    //    public int GetIndex(int id)
    //    {
    //        return base.GetIndex(new ID(id));
    //    }
    //    #endregion
    //}
    //#endregion

    #region IHeadDisplayConfigure interface
    public interface IHeadDisplayConfigureService
    {
        HeadDisplayConfigure Get(int id, int nUserID);
        List<HeadDisplayConfigure> Gets(int nUserID);
        List<HeadDisplayConfigure> Gets(int nVoucherTypeID, int nUserID);
        string Delete(int id, int nUserID);
        HeadDisplayConfigure Save(HeadDisplayConfigure oHeadDisplayConfigure, int nUserID);
    }
    #endregion
}
