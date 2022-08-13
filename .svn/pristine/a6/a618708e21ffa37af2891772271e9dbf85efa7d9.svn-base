using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region OrderRecapYarn
    
    public class OrderRecapYarn : BusinessObject
    {
        public OrderRecapYarn()
        {
            OrderRecapYarnID = 0;
            RefObjectID = 0;
            YarnID = 0;
            YarnPly = "";
            YarnCode = "";
            YarnName = "";
            RefType = EnumRecapRefType.None;
            YarnType = EnumRecapYarnType.None;
            RefObjectLogID = 0;
            OrderRecapYarnLogID = 0;
            Note = "";
            ErrorMessage = "";            
        }

        #region Properties
         
        public int OrderRecapYarnID{ get; set; }
         
		public int RefObjectID{ get; set; }
         
        public int RefObjectLogID { get; set; }
        public EnumRecapRefType RefType { get; set; }
        public EnumRecapYarnType YarnType { get; set; }
         
        public int OrderRecapYarnLogID { get; set; }
         
		public int YarnID{ get; set; }
         
        public string YarnPly { get; set; }
         
		public string YarnCode{ get; set; }
         
        public string YarnName { get; set; }
        public string Note { get; set; }       
        public string ErrorMessage { get; set; }       
        #endregion

        #region Derived Property    
        public string YarnTypeSt
        {
            get
            {
                return EnumObject.jGet(this.YarnType);
            }
        }
        public string RefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.RefType);
            }
        }
        #endregion

        #region Functions       
        public static List<OrderRecapYarn> Gets(long nUserID)
        {
            return OrderRecapYarn.Service.Gets( nUserID);
        }

        public static List<OrderRecapYarn> Gets(string sSQL, long nUserID)
        {
            return OrderRecapYarn.Service.Gets(sSQL, nUserID);
        }

        public static List<OrderRecapYarn> Gets(int nRefID, int nRefType, long nUserID)
        {

            return OrderRecapYarn.Service.Gets(nRefID, nRefType ,nUserID);
        }

        public static List<OrderRecapYarn> GetsByLog(int id, long nUserID)//Sale Order Log ID
        {
            return OrderRecapYarn.Service.GetsByLog(id, nUserID);
        }
        public OrderRecapYarn Get(int id, long nUserID)
        {
            return OrderRecapYarn.Service.Get(id, nUserID);
        }

        public OrderRecapYarn Save(long nUserID)
        {

            return OrderRecapYarn.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID, string sOrderRecapYarnIDs)
        {
            return OrderRecapYarn.Service.Delete(id, nUserID,sOrderRecapYarnIDs);
        }

        #endregion

        #region ServiceFactory

        internal static IOrderRecapYarnService Service
        {
            get { return (IOrderRecapYarnService)Services.Factory.CreateService(typeof(IOrderRecapYarnService)); }
        }
        #endregion
    }
    #endregion

    #region IOrderRecapYarn interface
     
    public interface IOrderRecapYarnService
    {
         
        OrderRecapYarn Get(int id, Int64 nUserID);        
         
        List<OrderRecapYarn> Gets(Int64 nUserID);
         
        List<OrderRecapYarn> Gets(string sSQL, Int64 nUserID);
         
        List<OrderRecapYarn> Gets(int nRefID, int nRefType, Int64 nUserID);
        
         
        List<OrderRecapYarn> GetsByLog(int id, Int64 nUserID);
         
        string Delete(int id, Int64 nUserID, string sOrderRecapYarnIDs);
         
        OrderRecapYarn Save(OrderRecapYarn oOrderRecapYarn, Int64 nUserID);
    }
    #endregion
}
