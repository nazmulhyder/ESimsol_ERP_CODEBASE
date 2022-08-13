using System;
using System.IO;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{

    #region VOrder

    public class VOrder : BusinessObject
    {
        public VOrder()
        {
            VOrderID = 0;
            BUID = 0;
            RefNo = "";
            VOrderRefType = EnumVOrderRefType.Manual;
            VOrderRefTypeInt = 1;
            VOrderRefID = 0;
            OrderNo = "";
            OrderDate = DateTime.Today;
            SubledgerID = 0;
            Remarks = "";
            SubledgerName = "";
            BUName = "";
            BUCode = "";
            ErrorMessage = "";
            VOrderRefNo = "";
            VOReferences = new List<VOReference>();
        }

        #region Properties
        public int VOrderID { get; set; }
        public int BUID { get; set; }
        public string RefNo { get; set; }
        public EnumVOrderRefType VOrderRefType { get; set; }
        public int VOrderRefTypeInt { get; set; }
        public int VOrderRefID { get; set; }
        public string VOrderRefNo { get; set; }
        public string OrderNo { get; set; }
        public DateTime OrderDate { get; set; }
        public int SubledgerID { get; set; }
        public string Remarks { get; set; }
        public string SubledgerName { get; set; }
        public string BUName { get; set; }
        public string BUCode { get; set; }          
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string OrderDateSt 
        { 
            get 
            { 
                return this.OrderDate.ToString("dd MMM yyyy"); 
            } 
        }
        public string VOrderRefTypeSt
        {
            get
            {
                return EnumObject.jGet(this.VOrderRefType);
            }
        }
        public List<VOReference> VOReferences { get; set; }
        #endregion

        #region Functions

        public static List<VOrder> Gets(long nUserID)
        {
            return VOrder.Service.Gets(nUserID);
        }
        public static List<VOrder> Gets(string sSQL, long nUserID)
        {
            return VOrder.Service.Gets(sSQL, nUserID);
        }
        public VOrder Get(int id, long nUserID)
        {
            return VOrder.Service.Get(id, nUserID);
        }
        public VOrder Save(long nUserID)
        {
            return VOrder.Service.Save(this, nUserID);
        }        
        public string Delete(int id, long nUserID)
        {
            return VOrder.Service.Delete(id, nUserID);
        }
        #endregion

        #region ServiceFactory

        internal static IVOrderService Service
        {
            get { return (IVOrderService)Services.Factory.CreateService(typeof(IVOrderService)); }
        }

        #endregion
    }
    #endregion

    #region IVOrder interface

    public interface IVOrderService
    {
        VOrder Get(int id, Int64 nUserID);
        List<VOrder> Gets(Int64 nUserID);
        List<VOrder> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        VOrder Save(VOrder oVOrder, Int64 nUserID);        
    }
    #endregion
}
