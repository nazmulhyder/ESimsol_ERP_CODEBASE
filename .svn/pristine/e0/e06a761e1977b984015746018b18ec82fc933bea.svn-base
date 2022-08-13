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
    public class PaymentAttachment
    {
        public PaymentAttachment()
        {
            PAID = 0;
            PaymentID = 0;
            FileType = 0;
            Attachment = null;
        }

        #region Properties
        public int PAID { get; set; }
        public int PaymentID { get; set; }
        public int FileType { get; set; }
        public byte[] Attachment { get; set; }
        #endregion

        #region Property For Image
        public System.Drawing.Image AttachmentImage { get; set; } //CompanyLogo
        public System.Drawing.Image AttachmentTitle { get; set; } //CompanyTitle
        #endregion

        #region Functions
        public static List<PaymentAttachment> Gets(long nUserID)
        {
            return PaymentAttachment.Service.Gets(nUserID);
        }

        public PaymentAttachment Get(int nId, long nUserID)
        {
            return PaymentAttachment.Service.Get(nId, nUserID);
        }
        public PaymentAttachment Save(long nUserID)
        {
            return PaymentAttachment.Service.Save(this, nUserID);
        }

        public string Delete(int nID, long nUserID)
        {
            return PaymentAttachment.Service.Delete(nID, nUserID);
        }

        #region ServiceFactory
        internal static IPaymentAttachmentService Service
        {
            get { return (IPaymentAttachmentService)Services.Factory.CreateService(typeof(IPaymentAttachmentService)); }
        }
        #endregion

        #endregion
    }

    #region IPaymentAttachment interface

    public interface IPaymentAttachmentService
    {
        PaymentAttachment Get(int id, long nUserID);
        List<PaymentAttachment> Gets(long nUserID);
        string Delete(int nID, long nUserID);
        PaymentAttachment Save(PaymentAttachment oOrganizationalInformation, long nUserID);
    }
    #endregion
}
