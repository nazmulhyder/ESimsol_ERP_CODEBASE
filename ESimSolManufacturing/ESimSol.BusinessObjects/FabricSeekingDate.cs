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
    #region FabricSeekingDate
    [DataContract]
    public class FabricSeekingDate : BusinessObject
    {
        public FabricSeekingDate()
        {
            FabricRequestType = EnumFabricRequestType.None;
            FabricRequestTypeInt = 0;
            ErrorMessage = "";
            SeekingDate = DateTime.MinValue;
            NoOfSets = 0;
        }
        #region Properties
        public int FabricRequestTypeInt { get; set; }
        public int FabricID { get; set; }
        public EnumFabricRequestType FabricRequestType { get; set; }
        public DateTime SeekingDate { get; set; }
        public int NoOfSets { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SeekingDateST
        {
            get
            {
                return this.SeekingDate.ToString("dd MMM yyyy");
            }
        }
        public string FabricRequestTypeSt
        {
            get
            {
                return this.FabricRequestType.ToString();
            }
        }
      
        #endregion

        #region Functions


        public FabricSeekingDate Get(int nId, Int64 nUserID)
        {
            return FabricSeekingDate.Service.Get(nId, nUserID);
        }
        public FabricSeekingDate Save(Int64 nUserID)
        {
            return FabricSeekingDate.Service.Save(this, nUserID);
        }
        public string Delete(Int64 nUserID)
        {
            return FabricSeekingDate.Service.Delete(this, nUserID);
        }
        public static List<FabricSeekingDate> Gets(int nFabricID, Int64 nUserID)
        {
            return FabricSeekingDate.Service.Gets(nFabricID, nUserID);
        }
       
        #endregion

     
        #region ServiceFactory
        internal static IFabricSeekingDateService Service
        {
            get { return (IFabricSeekingDateService)Services.Factory.CreateService(typeof(IFabricSeekingDateService)); }
        }
        #endregion
    }
    #endregion

    #region IFabricSeekingDate interface
    [ServiceContract]
    public interface IFabricSeekingDateService
    {
        FabricSeekingDate Get(int id, Int64 nUserID);
        List<FabricSeekingDate> Gets(int nFabricID, Int64 nUserID);
        string Delete(FabricSeekingDate oFabricSeekingDate, Int64 nUserID);
        FabricSeekingDate Save(FabricSeekingDate oFabricSeekingDate, Int64 nUserID);
    }
    #endregion
}
