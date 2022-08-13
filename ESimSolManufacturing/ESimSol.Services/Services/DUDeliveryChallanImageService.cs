using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DUDeliveryChallanImageService : MarshalByRefObject, IDUDeliveryChallanImageService
    {
        #region Private functions and declaration
        private DUDeliveryChallanImage MapObject(NullHandler oReader)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            oDUDeliveryChallanImage.DUDeliveryChallanImageID = oReader.GetInt32("DUDeliveryChallanImageID");
            oDUDeliveryChallanImage.DUDeliveryChallanID = oReader.GetInt32("DUDeliveryChallanID");
            oDUDeliveryChallanImage.Name = oReader.GetString("Name");
            oDUDeliveryChallanImage.ContractNo = oReader.GetString("ContractNo");
            oDUDeliveryChallanImage.Note = oReader.GetString("Note");
            oDUDeliveryChallanImage.Picture = oReader.GetBytes("Picture");
            oDUDeliveryChallanImage.PictureName = oReader.GetString("PictureName");
            return oDUDeliveryChallanImage;
        }

        private DUDeliveryChallanImage CreateObject(NullHandler oReader)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            oDUDeliveryChallanImage = MapObject(oReader);
            return oDUDeliveryChallanImage;
        }

        private List<DUDeliveryChallanImage> CreateObjects(IDataReader oReader)
        {
            List<DUDeliveryChallanImage> oDUDeliveryChallanImage = new List<DUDeliveryChallanImage>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUDeliveryChallanImage oItem = CreateObject(oHandler);
                oDUDeliveryChallanImage.Add(oItem);
            }
            return oDUDeliveryChallanImage;
        }

        #endregion

        #region Interface implementation
        public DUDeliveryChallanImageService() { }
        public DUDeliveryChallanImage Save(DUDeliveryChallanImage oDUDeliveryChallanImage, Int64 nUserId)
        {
            DUDeliveryChallanImage oTempDUDeliveryChallanImage = new DUDeliveryChallanImage();
            oTempDUDeliveryChallanImage.Picture = oDUDeliveryChallanImage.Picture;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oDUDeliveryChallanImage.Picture = null;
                if (oDUDeliveryChallanImage.DUDeliveryChallanImageID <= 0)
                {
                    reader = DUDeliveryChallanImageDA.InsertUpdate(tc, oDUDeliveryChallanImage, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DUDeliveryChallanImageDA.InsertUpdate(tc, oDUDeliveryChallanImage, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanImage = new DUDeliveryChallanImage();
                    oDUDeliveryChallanImage = CreateObject(oReader);
                }
                reader.Close();

                oTempDUDeliveryChallanImage.DUDeliveryChallanImageID = oDUDeliveryChallanImage.DUDeliveryChallanImageID;
                if (oTempDUDeliveryChallanImage.Picture != null)
                {
                    DUDeliveryChallanImageDA.UpdatePicture(tc, oTempDUDeliveryChallanImage, nUserId);
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDUDeliveryChallanImage.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return oDUDeliveryChallanImage;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
                oDUDeliveryChallanImage.DUDeliveryChallanImageID = id;
                DUDeliveryChallanImageDA.Delete(tc, oDUDeliveryChallanImage, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete PIPrintSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DUDeliveryChallanImage Get(int id, Int64 nUserId)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanImageDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDUDeliveryChallanImage;
        }
        public DUDeliveryChallanImage GetByDeliveryChallan(int id, Int64 nUserId)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanImageDA.GetByDeliveryChallan(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }

            return oDUDeliveryChallanImage;
        }
        public DUDeliveryChallanImage GetByBU(int buid, Int64 nUserId)
        {
            DUDeliveryChallanImage oDUDeliveryChallanImage = new DUDeliveryChallanImage();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDeliveryChallanImageDA.GetByBU(tc, buid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDeliveryChallanImage = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PIPrintSetup", e);
                #endregion
            }
            return oDUDeliveryChallanImage;
        }

        public List<DUDeliveryChallanImage> Gets(Int64 nUserId)
        {
            List<DUDeliveryChallanImage> oDUDeliveryChallanImage = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryChallanImageDA.Gets(tc);
                oDUDeliveryChallanImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Setup", e);
                #endregion
            }

            return oDUDeliveryChallanImage;
        }

        public List<DUDeliveryChallanImage> Gets(string sSQL, Int64 nUserId)
        {
            List<DUDeliveryChallanImage> oDUDeliveryChallanImage = null;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DUDeliveryChallanImageDA.Gets(tc, sSQL);
                oDUDeliveryChallanImage = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Delivery Setup", e);
                #endregion
            }

            return oDUDeliveryChallanImage;
        }

        #endregion
    }
}

