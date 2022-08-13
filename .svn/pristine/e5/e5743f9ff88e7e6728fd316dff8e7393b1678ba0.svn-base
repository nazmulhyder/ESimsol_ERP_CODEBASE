using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class UserImageService : MarshalByRefObject, IUserImageService
    {
        #region Private functions and declaration
        private UserImage MapObject(NullHandler oReader)
        {
            UserImage oUserImage = new UserImage();
            oUserImage.UserImageID = oReader.GetInt32("UserImageID");
            oUserImage.UserID = oReader.GetInt32("UserID");
            oUserImage.ImageType = (EnumUserImageType)oReader.GetInt16("ImageType");
            oUserImage.ImageFile = oReader.GetBytes("ImageFile");

            return oUserImage;

        }

        private UserImage CreateObject(NullHandler oReader)
        {
            UserImage oUserImage = MapObject(oReader);
            return oUserImage;
        }

        private List<UserImage> CreateObjects(IDataReader oReader)
        {
            List<UserImage> oUserImage = new List<UserImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UserImage oItem = CreateObject(oHandler);
                oUserImage.Add(oItem);
            }
            return oUserImage;
        }

        #endregion

        #region Interface implementation
        public UserImageService() { }

        public UserImage IUD(UserImage oUserImage, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                //IDataReader reader;
                oUserImage.ErrorMessage=UserImageDA.IUD(tc, oUserImage, nUserID, nDBOperation);
                //NullHandler oReader = new NullHandler(reader);
                //if (reader.Read())
                //{

                //    oUserImage = CreateObject(oReader);
                //}
                //reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUserImage.ErrorMessage = e.Message;
                oUserImage.UserImageID = 0;
                #endregion
            }
            return oUserImage;
        }


        public UserImage Get(int nUserImageID, Int64 nUserId)
        {
            UserImage oUserImage = new UserImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserImageDA.Get(nUserImageID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUserImage = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get UserImage", e);
                oUserImage.ErrorMessage = e.Message;
                #endregion
            }

            return oUserImage;
        }

        public UserImage GetbyUser(int nUserImageID, int ImageType, Int64 nUserId)
        {
            UserImage oUserImage = new UserImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UserImageDA.GetbyUser( nUserImageID, ImageType, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oUserImage = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get UserImage", e);
                oUserImage.ErrorMessage = e.Message;
                #endregion
            }

            return oUserImage;
        }



        public List<UserImage> Gets(int nUserID_Im, Int64 nUserID)
        {
            List<UserImage> oUserImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserImageDA.Gets(nUserID_Im, tc);
                oUserImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UserImage", e);
                #endregion
            }
            return oUserImage;
        }
        public List<UserImage> Gets(string sSQL, Int64 nUserID)
        {
            List<UserImage> oUserImages = new List<UserImage>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UserImageDA.Gets(sSQL, tc);
                oUserImages = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oUserImages = new List<UserImage>();
                #endregion
            }
            return oUserImages;
        }

        #endregion
 
    }
}
