using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region FabricSalesContractNote
    public class FabricSalesContractNote : BusinessObject
    {
        public FabricSalesContractNote()
        {
            FabricSalesContractNoteID = 0;
            FabricSalesContractID = 0;
            Note = "";
            FabricSalesContractID = 0;
            ErrorMessage = "";
            Sequence = 0;
        }

        #region Properties        
        public int FabricSalesContractNoteID { get; set; }
        public int FabricSalesContractID { get; set; }
        public string Note { get; set; }        
        public string ErrorMessage { get; set; }
        public int Sequence { get; set; }
 
        #endregion

        #region Functions
        public static List<FabricSalesContractNote> Gets(int nFSCID, Int64 nUserID)
        {
            return FabricSalesContractNote.Service.Gets(nFSCID, nUserID);
        }
        public static List<FabricSalesContractNote> GetsLog(int nFSCID, Int64 nUserID)
        {
            return FabricSalesContractNote.Service.GetsLog(nFSCID, nUserID);
        }
        public static List<FabricSalesContractNote> Gets(string sSQL, Int64 nUserID)
        {
            return FabricSalesContractNote.Service.Gets(sSQL, nUserID);            
        }
        public FabricSalesContractNote Get(int id, Int64 nUserID)
        {
            return FabricSalesContractNote.Service.Get(id, nUserID);            
        }
        public FabricSalesContractNote Save(Int64 nUserID)
        {
            return FabricSalesContractNote.Service.Save(this, nUserID);            
        }
        public string SaveAll(List<FabricSalesContractNote> oFabricSalesContractNotes, Int64 nUserID)
        {
            return FabricSalesContractNote.Service.SaveAll(oFabricSalesContractNotes, nUserID);            
        }
        public string Delete( Int64 nUserID)
        {
            return FabricSalesContractNote.Service.Delete(this, nUserID);            
        }
        public string DeleteAll(Int64 nUserID)
        {
            return FabricSalesContractNote.Service.DeleteAll(this, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricSalesContractNoteService Service
        {
            get { return (IFabricSalesContractNoteService)Services.Factory.CreateService(typeof(IFabricSalesContractNoteService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSalesContractNote interface
    public interface IFabricSalesContractNoteService
    {        
        FabricSalesContractNote Get(int id, Int64 nUserID);
        List<FabricSalesContractNote> Gets(int nFSCID, Int64 nUserID);
        List<FabricSalesContractNote> GetsLog(int nFSCID, Int64 nUserID);        
        List<FabricSalesContractNote> Gets(string sSQL, Int64 nUserID);
        FabricSalesContractNote Save(FabricSalesContractNote oFabricSalesContractNote, Int64 nUserID);        
        string SaveAll(List<FabricSalesContractNote> oFabricSalesContractNotes, Int64 nUserID);        
        string Delete(FabricSalesContractNote ooFabricSalesContractNote, Int64 nUserID);
        string DeleteAll(FabricSalesContractNote ooFabricSalesContractNote, Int64 nUserID);
    }
    #endregion
}
