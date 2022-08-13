using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class COSImageService : MarshalByRefObject, ICOSImageService
    {
        #region Private functions and declaration
        private COSImage MapObject(NullHandler oReader)
        {
            COSImage oCOSImage = new COSImage();
            oCOSImage.COSImageID = oReader.GetInt32("COSImageID");
            oCOSImage.OperationType = (EnumOperationType)oReader.GetInt32("OperationType");
            oCOSImage.OperationTypeInInt = oReader.GetInt32("OperationType");
            oCOSImage.ImageTitle = oReader.GetString("ImageTitle");
            oCOSImage.LargeImage = oReader.GetBytes("LargeImage");
            oCOSImage.COSVFormat = (EnumClientOperationValueFormat)oReader.GetInt32("COSVFormat");
            oCOSImage.COSVFormatInInt = oReader.GetInt32("COSVFormat");
            return oCOSImage;
        }

        private COSImage CreateObject(NullHandler oReader)
        {
            COSImage oCOSImage = new COSImage();
            oCOSImage = MapObject(oReader);
            return oCOSImage;
        }

        private List<COSImage> CreateObjects(IDataReader oReader)
        {
            List<COSImage> oCOSImage = new List<COSImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                COSImage oItem = CreateObject(oHandler);
                oCOSImage.Add(oItem);
            }
            return oCOSImage;
        }

        #endregion

        #region Interface implementation
        public COSImageService() { }

       
        #region new System
        public COSImage Save(COSImage oCOSImage, Int64 nUserID)
        {
            List<COSImage> oCOSImages = new List<COSImage>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Get all Image and Check Is exist or not
                IDataReader reader = null;
                reader = COSImageDA.GetsByOperationTypeANDCOSFormat(tc, (int)oCOSImage.OperationType, (int)oCOSImage.COSVFormat);
                oCOSImages = CreateObjects(reader);
                reader.Close();
                if (oCOSImages.Count > 0)
                {
                    oCOSImage.ErrorMessage = "Alerady Exist Image for Selected Types.";
                    return oCOSImage;
                }
                #endregion
                if (oCOSImage.COSImageID <= 0)
                {
                    oCOSImage.COSImageID = COSImageDA.GetNewID(tc);
                    COSImageDA.Insert(tc, oCOSImage, nUserID);
                }
                else
                {
                    COSImageDA.Update(tc, oCOSImage, nUserID);
                }
                tc.End();
                BusinessObject.Factory.SetObjectState(oCOSImage, ObjectState.Saved);
            }
            catch (Exception e)
            {
                #region Handle Exception
                oCOSImage = new COSImage();
                oCOSImage.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oCOSImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                COSImageDA.Delete(tc, id);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //throw new ServiceException(e.Message, e);
                #endregion
            }
            return "Data Delete Successfully";
        }
        #endregion


        public COSImage Get(int id, Int64 nUserId)
        {
            COSImage oAccountHead = new COSImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = COSImageDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get COSImage", e);
                #endregion
            }

            return oAccountHead;
        }
        public COSImage GetByOperationAndCOSFormat(int nOperationType,int nCOSFormat, Int64 nUserId)
        {
            COSImage oAccountHead = new COSImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = COSImageDA.GetsByOperationTypeANDCOSFormat(tc, nOperationType, nCOSFormat);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get COSImage", e);
                #endregion
            }

            return oAccountHead;
        }
        //
        public List<COSImage> Gets( Int64 nUserID)
        {
            List<COSImage> oCOSImage = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = COSImageDA.Gets(tc);
                oCOSImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get COSImage", e);
                #endregion
            }

            return oCOSImage;
        }


        #endregion
    }
}
