using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region Rack
    public class Rack : BusinessObject
    {
        public Rack()
        {
            RackID = 0;
            RackNo = "";
            ShelfID = 0;
            Remarks = "";
            ShelfNo = "";
            ShelfName = "";
            ErrorMessage = "";
        }
        #region Properties
        public int RackID { get; set; }
        public string RackNo { get; set; }
        public int ShelfID { get; set; }
        public string Remarks { get; set; }
        public string RackNoOrName { get; set; }
        public string ShelfNo { get; set; }
        public string ShelfName { get; set; }
        public string ErrorMessage { get; set; }
        #endregion
        #region Derived
        public string RackWithShelfNo
        {
            get
            {
                return this.RackNo + "[" + this.ShelfName + "]";
            }
        }
        public string ShelfWithRackNo
        {
            get
            {
                return this.ShelfName + "[" + this.RackNo + "]";
            }
        }
        #endregion

        #region Functions

        public Rack Get(int id, int nUserID)
        {
            return Rack.Service.Get(id, nUserID);
        }
        public Rack Save(int nUserID)
        {
            return Rack.Service.Save(this, nUserID);
        }
        public string Delete(int id, int nUserID)
        {
            return Rack.Service.Delete(id, nUserID);
        }
        public static List<Rack> Gets(int nUserID)
        {
            return Rack.Service.Gets(nUserID);
        }

        public static List<Rack> Gets(int nShelfID, int nUserID)
        {
            return Rack.Service.Gets(nShelfID,nUserID);
        }
        public static List<Rack> BUWiseGets(int nBUID, int nUserID)
        {
            return Rack.Service.BUWiseGets(nBUID, nUserID);
        }
        public static List<Rack> Gets(string sSQL, int nUserID)
        {
            return Rack.Service.Gets(sSQL, nUserID);
        }
   
        #endregion


        #region ServiceFactory
        internal static IRackService Service
        {
            get { return (IRackService)Services.Factory.CreateService(typeof(IRackService)); }
        }
        #endregion
    }
    #endregion

    

    #region IRack interface
    public interface IRackService
    {
        Rack Get(int id, int nUserID);
        List<Rack> Gets(int nUserID);
        List<Rack> Gets(int nShelfID, int nUserID);
        List<Rack> BUWiseGets(int nBUID, int nUserID);
        string Delete(int id, int nUserID);
        Rack Save(Rack oRack, int nUserID);
        List<Rack> Gets(string sSQL, int nUserID);
    
    }
    #endregion
}