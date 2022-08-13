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
    #region ThemeUpload

    public class ThemeUpload : BusinessObject
    {

        #region Constructor
        public ThemeUpload()
        {
            ThemeUploadID = 0;
            File = null;
            FileName = "";
            FilePath = "";

            ErrorMessage = "";
        }
        #endregion

        #region Properties

        public int ThemeUploadID { get; set; }

        public string ErrorMessage { get; set; }

        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }


        #endregion

        #region Derived Property
        public Company Company { get; set; }

        #endregion

        #region Functions

        public ThemeUpload Get(int id, long nUserID)
        {
            return ThemeUpload.Service.Get(id, nUserID);
        }

        public ThemeUpload Save(long nUserID)
        {
            return ThemeUpload.Service.Save(this, nUserID);
        }

        public string Delete(int id, long nUserID)
        {
            return ThemeUpload.Service.Delete(id, nUserID);
        }

        #endregion




        #region ServiceFactory

        internal static IThemeUploadService Service
        {
            get { return (IThemeUploadService)Services.Factory.CreateService(typeof(IThemeUploadService)); }
        }

        #endregion


    }
    #endregion

    #region IThemeUpload interface

    public interface IThemeUploadService
    {

        ThemeUpload Get(int id, Int64 nUserID);
        string Delete(int id, Int64 nUserID);
        ThemeUpload Save(ThemeUpload oThemeUpload, Int64 nUserID);

    }
    #endregion
}
