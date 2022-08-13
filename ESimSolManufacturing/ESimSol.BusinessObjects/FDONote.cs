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
    public class FDONote
    {
        public FDONote()
        {
            FDONoteID = 0;
            FDOID = 0;
            Note = "";
            ErrorMessage = "";
            FDONotes = new List<FDONote>();
        }

        #region Properties
        public int FDONoteID { get; set; }
        public int FDOID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        public List<FDONote> FDONotes { get; set; }
        #endregion

        #region Functions
        public static List<FDONote> Gets(string sSQL, int nUserID)
        {
            return FDONote.Service.Gets(sSQL, nUserID);
        }
        public static List<FDONote> GetByOrderID(int nOrderID, int nUserID)
        {
            return FDONote.Service.GetByOrderID(nOrderID, nUserID);
        }
        public string SaveAll( Int64 nUserID)
        {
            return FDONote.Service.SaveAll(this, nUserID);
        }
        public string Delete(int nId, int nUserID)
        {
            return FDONote.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFDONoteService Service
        {
            get { return (IFDONoteService)Services.Factory.CreateService(typeof(IFDONoteService)); }
        }
        #endregion
    }

    #region IFDONote interface
    public interface IFDONoteService
    {
        List<FDONote> Gets(string sSQL, int nUserID);
        string SaveAll(FDONote oFDONote, Int64 nUserID);       
        List<FDONote> GetByOrderID(int nOrderID, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
