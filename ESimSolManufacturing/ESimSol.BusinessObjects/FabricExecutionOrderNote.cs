using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using System.Drawing;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region FabricExecutionOrderNote

    public class FabricExecutionOrderNote : BusinessObject
    {
        #region  Constructor
        public FabricExecutionOrderNote()
        {
            FEONID = 0;
            FEOID = 0;
            Note = "";
            ErrorMessage = "";
        }
        #endregion

        #region Properties
        public int FEONID { get; set; }
        public int FEOID { get; set; }
        public string Note { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions
        public static FabricExecutionOrderNote Get(int nFEONID, long nUserID)
        {
            return FabricExecutionOrderNote.Service.Get(nFEONID, nUserID);
        }
        public static List<FabricExecutionOrderNote> Gets(int nFEOID, long nUserID)
        {
            return FabricExecutionOrderNote.Service.Gets(nFEOID, nUserID);
        }
        public static List<FabricExecutionOrderNote> Gets(string sSQL, long nUserID)
        {
            return FabricExecutionOrderNote.Service.Gets(sSQL, nUserID);
        }
        public FabricExecutionOrderNote IUD(int nDBOperation, long nUserID)
        {
            return FabricExecutionOrderNote.Service.IUD(this, nDBOperation, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IFabricExecutionOrderNoteService Service
        {
            get { return (IFabricExecutionOrderNoteService)Services.Factory.CreateService(typeof(IFabricExecutionOrderNoteService)); }
        }
        #endregion


    }
    #endregion


    #region IFabricExecutionOrderNote interface
    public interface IFabricExecutionOrderNoteService
    {
        FabricExecutionOrderNote Get(int nFEONID, long nUserID);
        List<FabricExecutionOrderNote> Gets(int nFEOID, long nUserID);
        List<FabricExecutionOrderNote> Gets(string sSQL, long nUserID);
        FabricExecutionOrderNote IUD(FabricExecutionOrderNote oFabricExecutionOrderNote, int nDBOperation, long nUserID);

    }
    #endregion
}