using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    #region KnitDyeingProgramAttachment
    public class KnitDyeingProgramAttachment : BusinessObject
	{
        public KnitDyeingProgramAttachment()
		{
            KnitDyeingProgramAttachmentID = 0;
            KnitDyeingProgramID = 0; 
			FileName = ""; 
			AttachFile = null; 
			FileType = ""; 
			Remarks = ""; 
			ArticleNo = "";
            RefTypeInInt = 0;
            KnitDyeingProgramAttachments = new List<KnitDyeingProgramAttachment>();
			ErrorMessage = "";
		}

		#region Property
        public int KnitDyeingProgramAttachmentID { get; set; }
        public int KnitDyeingProgramID { get; set; }
		public string FileName { get; set; }
		public byte[] AttachFile { get; set; }
		public string FileType { get; set; }
		public string Remarks { get; set; }
		public string ArticleNo { get; set; }
        public int RefTypeInInt { get; set; }
		public string ErrorMessage { get; set; }
		#endregion 

		#region Derived Property
        public List<KnitDyeingProgramAttachment> KnitDyeingProgramAttachments { get; set; }
        public string AttatchFileinString
        {
            get
            {
                return KnitDyeingProgramAttachmentID.ToString();
            }
        }
		#endregion 

		#region Functions 
		
        public KnitDyeingProgramAttachment Save(long nUserID)
		{
            return KnitDyeingProgramAttachment.Service.Save(this, nUserID);
		}
        public static List<KnitDyeingProgramAttachment> Gets(int id, long nUserID)
        {
            return KnitDyeingProgramAttachment.Service.Gets(id, nUserID);
        }

        public static KnitDyeingProgramAttachment GetWithAttachFile(int id, long nUserID)
        {
            return KnitDyeingProgramAttachment.Service.GetWithAttachFile(id, nUserID);
        }
        public static List<KnitDyeingProgramAttachment> Gets(string sSQL, long nUserID)
        {
            return KnitDyeingProgramAttachment.Service.Gets(sSQL, nUserID);
        }
        public string Delete(int id, long nUserID)
        {
            return KnitDyeingProgramAttachment.Service.Delete(id, nUserID);
        }
		#endregion

		#region ServiceFactory
        internal static IKnitDyeingProgramAttachmentService Service
		{
            get { return (IKnitDyeingProgramAttachmentService)Services.Factory.CreateService(typeof(IKnitDyeingProgramAttachmentService)); }
		}
		#endregion
	}
	#endregion

    #region IKnitDyeingProgramAttachment interface
    public interface IKnitDyeingProgramAttachmentService 
	{
        List<KnitDyeingProgramAttachment> Gets(int id, Int64 nUserID);
        KnitDyeingProgramAttachment GetWithAttachFile(int id, Int64 nUserID); 
        KnitDyeingProgramAttachment Save(KnitDyeingProgramAttachment oKnitDyeingProgramAttachment, Int64 nUserID);
        List<KnitDyeingProgramAttachment> Gets(string sSQL, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
	}
	#endregion
}
