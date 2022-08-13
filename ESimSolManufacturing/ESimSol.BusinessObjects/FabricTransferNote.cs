using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    public class FabricTransferNote
    {
        public FabricTransferNote()
        {
            FTNID = 0;
            FTNNo = "";
            Note = "";
            NoteDate = DateTime.Today;
            DisburseBy = 0;
            DisburseByDate = new DateTime(1900, 01, 01);
            ErrorMessage = "";

            DisburseByName = "";
            FTPListIDs = "";
            NoOfPackingList = 0;
            FTPLs = new List<FabricTransferPackingList>();
            Params = "";
            IsFTPLD = false;
            FTPLD = new FabricTransferPackingListDetail();
            FTPLDetailID = 0;
            CountDetail = 0;
            CountReceive = 0;
            FEONo = "";
            WUID = 0;
        }

        #region Properties
     
        public int FTNID { get; set; }
        public string FTNNo { get; set; }
        public string Note { get; set; }
        public DateTime NoteDate { get; set; }
        public int DisburseBy { get; set; }
        public DateTime DisburseByDate { get; set; }
        public string ErrorMessage { get; set; }
        public string DisburseByName { get; set; }
        public string FTPListIDs { get; set; }
        public int NoOfPackingList { get; set; } //FabricTransferPackingList
        public string Params { get; set; }
        public bool IsFTPLD { get; set; }
        public int CountDetail { get; set; }
        public int CountReceive { get; set; }
        public string FEONo { get; set; }
        public int WUID { get; set; }
       
        #endregion

        #region Derive Properties
        public List<FabricTransferPackingList> FTPLs { get; set; }
        public FabricTransferPackingListDetail FTPLD{ get; set; }
        public int FTPLDetailID { get; set; }
        public string NoteDateSt
        {
            get
            {
                return this.NoteDate.ToString("dd MMM yyyy");
            }
        }
        public string DisburseByDateSt
        {
            get
            {
                DateTime MinValue = new DateTime(1900, 01, 01);
                DateTime MinValue1 = new DateTime(0001, 01, 01);
                if (this.DisburseByDate == MinValue || this.DisburseByDate == MinValue1 || this.DisburseBy == 0)
                {
                    return "-";
                }
                else
                {
                    return DisburseByDate.ToString("dd MMM yyyy");
                }
            }
        }
        #endregion

        #region Functions
        public static List<FabricTransferNote> Gets(long nUserID)
        {
            return FabricTransferNote.Service.Gets(nUserID);
        }
        public static List<FabricTransferNote> Gets(string sSQL, long nUserID)
        {
            return FabricTransferNote.Service.Gets(sSQL, nUserID);
        }
        public FabricTransferNote Save(long nUserID)
        {
            return FabricTransferNote.Service.Save(this, nUserID);
        }
        public FabricTransferNote Receive(long nUserID)
        {
            return FabricTransferNote.Service.Receive(this, nUserID);
        }
        public FabricTransferNote Disburse(long nUserID)
        {
            return FabricTransferNote.Service.Disburse(this, nUserID);
        }
        public FabricTransferNote Get(int nEPIDID, long nUserID)
        {
            return FabricTransferNote.Service.Get(nEPIDID, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricTransferNote.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricTransferNoteService Service
        {
            get { return (IFabricTransferNoteService)Services.Factory.CreateService(typeof(IFabricTransferNoteService)); }
        }
        #endregion
    }

    #region IFabricTransferNote interface
    public interface IFabricTransferNoteService
    {
        List<FabricTransferNote> Gets(long nUserID);
        List<FabricTransferNote> Gets(string sSQL, long nUserID);
        FabricTransferNote Save(FabricTransferNote oFabricTransferNote, long nUserID);
        FabricTransferNote Receive(FabricTransferNote oFabricTransferNote, long nUserID);
        FabricTransferNote Disburse(FabricTransferNote oFabricTransferNote, long nUserID);
        FabricTransferNote Get(int nEPIDID, long nUserID);
        string Delete(int id, long nUserID);
    }
    #endregion
}
