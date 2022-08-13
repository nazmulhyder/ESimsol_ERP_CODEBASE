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
    public class DyeingOrderNote
    {
        public DyeingOrderNote()
        {
            DyeingOrderNoteID = 0;
            DyeingOrderID = 0;
            OrderNote = "";
            ErrorMessage = "";
            DyeingOrderNotes = new List<DyeingOrderNote>();
            DyeingOrder = new DyeingOrder();
            ContractorID = 0;
        }

        #region Properties
        public int DyeingOrderNoteID { get; set; }
        public int DyeingOrderID { get; set; }
        public string OrderNote { get; set; }
        public int ContractorID { get; set; }// for Para 
        public string ErrorMessage { get; set; }
        public List<DyeingOrderNote> DyeingOrderNotes { get; set; }
        public DyeingOrder DyeingOrder { get; set; }
        #endregion

        #region Functions
        public static List<DyeingOrderNote> Gets(int nUserID)
        {
            return DyeingOrderNote.Service.Gets(nUserID);
        }
        public static List<DyeingOrderNote> Gets(string sSQL, int nUserID)
        {
            return DyeingOrderNote.Service.Gets(sSQL, nUserID);
        }
        public DyeingOrderNote Save(int nUserID)
        {
            return DyeingOrderNote.Service.Save(this, nUserID);
        }
        public static List<DyeingOrderNote> GetByOrderID(int nOrderID, int nUserID)
        {
            return DyeingOrderNote.Service.GetByOrderID(nOrderID, nUserID);
        }
        public static List<DyeingOrderNote> GetByConID(DyeingOrderNote oDyeingOrderNote, int nUserID)
        {
            return DyeingOrderNote.Service.GetByConID(oDyeingOrderNote, nUserID);
        }
        public string SaveAll( Int64 nUserID)
        {
            return DyeingOrderNote.Service.SaveAll(this, nUserID);
        }
        public string Delete(int nId, int nUserID)
        {
            return DyeingOrderNote.Service.Delete(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDyeingOrderNoteService Service
        {
            get { return (IDyeingOrderNoteService)Services.Factory.CreateService(typeof(IDyeingOrderNoteService)); }
        }
        #endregion
    }

    #region IDyeingOrderNote interface
    public interface IDyeingOrderNoteService
    {
        List<DyeingOrderNote> Gets(int nUserID);
        List<DyeingOrderNote> Gets(string sSQL, int nUserID);
        DyeingOrderNote Save(DyeingOrderNote oDyeingOrderNote, int nUserID);
        string SaveAll(DyeingOrderNote oDyeingOrderNote, Int64 nUserID);       
        List<DyeingOrderNote> GetByOrderID(int nOrderID, int nUserID);
        List<DyeingOrderNote> GetByConID(DyeingOrderNote oDyeingOrderNote, int nUserID);
        string Delete(int id, int nUserID);
    }
    #endregion
}
