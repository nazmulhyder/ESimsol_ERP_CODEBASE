using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region LetterSetup
    public class LetterSetup
    {
        public LetterSetup()
        {
            LSID = 0;
            Code = "";
            Name = "";
            Body = "";
            Remark = "";
            IsEnglish = true;
            ErrorMessage = "";
            IsPadFormat = false;
            PageSize = "";
            MarginTop = 0;
            MarginBottom = 0;
            MarginLeft = 0;
            MarginRight = 0;
            HeaderName = "";
            HeaderFontSize = 8;
            HeaderTextAlign = 2;
        }

        #region Properties
        public int LSID { get; set; }
        public double MarginTop { get; set; }
        public double MarginBottom { get; set; }
        public double MarginLeft { get; set; }
        public double MarginRight { get; set; }
        public bool IsEnglish { get; set; }
        public bool IsPadFormat { get; set; }
        public string Code { get; set; }
        public string PageSize { get; set; }
        public string Name { get; set; }
        public string HeaderName { get; set; }
        public int HeaderFontSize { get; set; }
        public int HeaderTextAlign { get; set; }
        public string Body { get; set; }
        public string Remark { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Functions


        public static List<LetterSetup> Gets(string sSQL, long nUserID)
        {
            return LetterSetup.Service.Gets(sSQL, nUserID);
        }
        public LetterSetup Get(int id, long nUserID)
        {
            return LetterSetup.Service.Get(id, nUserID);
        }
        public static LetterSetup Get(string sSQL, long nUserID)
        {
            return LetterSetup.Service.Get(sSQL, nUserID);
        }
        public LetterSetup IUD(int nDBOperation, long nUserID)
        {
            return LetterSetup.Service.IUD(this, nDBOperation, nUserID);
        }
        public string Delete(long nUserID)
        {
            return LetterSetup.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static ILetterSetupService Service
        {
            get { return (ILetterSetupService)Services.Factory.CreateService(typeof(ILetterSetupService)); }
        }
        #endregion
    }
    #endregion

    #region ILetterSetupService interface

    public interface ILetterSetupService
    {
        LetterSetup Get(int id, Int64 nUserID);
        List<LetterSetup> Gets(string sSQL, Int64 nUserID);
        LetterSetup Get(string sSQL, Int64 nUserID);
        string Delete(LetterSetup oLetterSetup, Int64 nUserID);
        LetterSetup IUD(LetterSetup oLetterSetup, int nDBOperation, Int64 nUserID);
      
    }
    #endregion
}


