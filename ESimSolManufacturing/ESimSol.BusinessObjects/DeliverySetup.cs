using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region DeliverySetup
    public class DeliverySetup : BusinessObject
    {
        public DeliverySetup()
        {
            DeliverySetupID = 0;
            PrintHeader = "";
            OrderPrintNo = EnumExcellColumn.A;
            ChallanPrintNo = EnumExcellColumn.A;
            BUID = 0;
            DCPrefix = "";
            GPPrefix = "";
            ImagePad = null;
            ImagePadName = "";
            PrintFormatType = EnumPrintFormatType.None;
            ByteInString = "";
            ErrorMessage = "";
        }

        #region Properties 
        public int DeliverySetupID { get; set; }        
        public string PrintHeader{ get; set; }
        public EnumExcellColumn OrderPrintNo { get; set; }
        public EnumExcellColumn ChallanPrintNo { get; set; }
        public string DCPrefix { get; set; }
        public string GPPrefix { get; set; }
        public double OverDCQty { get; set; }
        public double OverDeliverPercentage { get; set; }
        public string ImagePadName { get; set; }
        public byte[] ImagePad { get; set; }
        public string ByteInString { get; set; }
        public bool IsImg { get; set; }
        public System.Drawing.Image Image { get; set; }
        public EnumPrintFormatType PrintFormatType { get; set; }
        public int BUID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion
        public string OrderPrintNoSt
        {
            get
            {
                
                return this.OrderPrintNo.ToString();
            }
        }
        public string ChallanPrintNoSt
        {
            get
            {
                return this.ChallanPrintNo.ToString();
            }
        }
        public string PrintFormatTypeSt
        {
            get
            {
                return EnumObject.jGet(this.PrintFormatType);
            }
        }

        #region Functions
        public static List<DeliverySetup> Gets(Int64 nUserID)
        {
            return DeliverySetup.Service.Gets(nUserID);            
        }
        public static List<DeliverySetup> Gets(string sSQL, Int64 nUserID)
        {
            return DeliverySetup.Service.Gets(sSQL, nUserID);
        }
        public DeliverySetup Get(int id, Int64 nUserID)
        {
            return DeliverySetup.Service.Get(id, nUserID);            
        }
        public DeliverySetup GetByBU(int buid, Int64 nUserID)
        {
            return DeliverySetup.Service.GetByBU(buid, nUserID);
        }
    
        public DeliverySetup Save(DeliverySetup oDeliverySetup, Int64 nUserID)
        {
            return DeliverySetup.Service.Save(oDeliverySetup, nUserID);            
        }
        public string Delete(int id, Int64 nUserID)
        {
            return DeliverySetup.Service.Delete(id, nUserID);            
        }
        #endregion

        #region ServiceFactory
        internal static IDeliverySetupService Service
        {
            get { return (IDeliverySetupService)Services.Factory.CreateService(typeof(IDeliverySetupService)); }
        }
        #endregion
    }
    #endregion

    #region IDeliverySetup interface
    public interface IDeliverySetupService
    {
        List<DeliverySetup> Gets(Int64 nUserID);
        DeliverySetup Get(int id, Int64 nUserID);
        DeliverySetup GetByBU(int buid, Int64 nUserID);       
        List<DeliverySetup> Gets(string sSQL, Int64 nUserID);
        DeliverySetup Save(DeliverySetup oDeliverySetup, Int64 nUserID);
        string Delete(int id, Int64 nUserID);        
    }
    #endregion
}

