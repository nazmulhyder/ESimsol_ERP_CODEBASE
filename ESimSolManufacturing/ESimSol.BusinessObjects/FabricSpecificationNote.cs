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
    #region FabricSpecificationNote
    public class FabricSpecificationNote : BusinessObject
    {
        public FabricSpecificationNote()
        {
            Note = "";
            ErrorMessage = "";
        }
        #region Properties
        public int FabricSpecificationNoteID { get; set; }
        public int FEOSID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion


        #region Functions
        public static List<FabricSpecificationNote> Gets(int nUserID)
        {
            return FabricSpecificationNote.Service.Gets(nUserID);
        }
        public static List<FabricSpecificationNote> Gets(int nFEOSID, int nUserID)
        {
            return FabricSpecificationNote.Service.Gets(nFEOSID, nUserID);
        }
        public FabricSpecificationNote Save(FabricSpecificationNote oFabricSpecificationNote, int nUserID)
        {
            return FabricSpecificationNote.Service.Save(oFabricSpecificationNote, nUserID);
        }
        public string Delete(int nId, long nUserID)
        {
            return FabricSpecificationNote.Service.Delete(nId, nUserID);
        }
        public static FabricSpecificationNote Get(int nId, int nUserID)
        {
            return FabricSpecificationNote.Service.Get(nId, nUserID);
        }
        #endregion
        #region ServiceFactory
        internal static IFabricSpecificationNoteService Service
        {
            get { return (IFabricSpecificationNoteService)Services.Factory.CreateService(typeof(IFabricSpecificationNoteService)); }
        }
        #endregion
    }
    #endregion
    public interface IFabricSpecificationNoteService
    {
        List<FabricSpecificationNote> Gets(long nUserID);
        List<FabricSpecificationNote> Gets(int nFEOSID, long nUserID);
        FabricSpecificationNote Get(int nId, long nUserID);
        FabricSpecificationNote Save(FabricSpecificationNote oFabricSpecificationNote, long nUserID);
        string Delete(int id, long nUserID);


    }
}