using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class CRWiseSparePartsService : MarshalByRefObject, ICRWiseSparePartsService
    {
        #region Private functions and declaration
        private CRWiseSpareParts MapObject(NullHandler oReader)
        {
            CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
            oCRWiseSpareParts.CRWiseSparePartsID = oReader.GetInt32("CRWiseSparePartsID");
            oCRWiseSpareParts.CRID = oReader.GetInt32("CRID");
            oCRWiseSpareParts.SparePartsID = oReader.GetInt32("SparePartsID");
            oCRWiseSpareParts.BUID = oReader.GetInt32("BUID");
            oCRWiseSpareParts.ReqPartsQty = oReader.GetDouble("ReqPartsQty");
            oCRWiseSpareParts.Remarks = oReader.GetString("Remarks");
            oCRWiseSpareParts.ProductName = oReader.GetString("ProductName");
            oCRWiseSpareParts.ProductCode = oReader.GetString("ProductCode");
            oCRWiseSpareParts.TotalLotBalance = oReader.GetDouble("TotalLotBalance");
            return oCRWiseSpareParts;
        }

        private CRWiseSpareParts CreateObject(NullHandler oReader)
        {
            CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
            oCRWiseSpareParts = MapObject(oReader);
            return oCRWiseSpareParts;
        }

        private List<CRWiseSpareParts> CreateObjects(IDataReader oReader)
        {
            List<CRWiseSpareParts> oCRWiseSpareParts = new List<CRWiseSpareParts>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CRWiseSpareParts oItem = CreateObject(oHandler);
                oCRWiseSpareParts.Add(oItem);
            }
            return oCRWiseSpareParts;
        }

        #endregion

        #region Interface implementation
        public CRWiseSparePartsService() { }
        public List<CRWiseSpareParts> GetsbyName(string sCRWiseSpareParts, int nUserID)
        {
            List<CRWiseSpareParts> oCRWiseSpareParts = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CRWiseSparePartsDA.GetsbyName(tc, sCRWiseSpareParts);
                NullHandler oReader = new NullHandler(reader);
                oCRWiseSpareParts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSpareParts;
        }
        public CRWiseSpareParts Get(int id, int nUserId)
        {
            CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CRWiseSparePartsDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCRWiseSpareParts = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSpareParts;
        }
        public List<CRWiseSpareParts> Gets(int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CRWiseSparePartsDA.Gets(tc);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        public List<CRWiseSpareParts> Gets(int nCRID, int nBUID, int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CRWiseSparePartsDA.Gets(tc, nCRID, nBUID);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        public List<CRWiseSpareParts> GetsByNameCRAndBUID(string sName, int nCRID, int nBUID, int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CRWiseSparePartsDA.GetsByNameCRAndBUID(tc, sName, nCRID, nBUID);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        public List<CRWiseSpareParts> GetsByNameCRAndBUIDWithLot(string sName, int nCRID, int nBUID, int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CRWiseSparePartsDA.GetsByNameCRAndBUIDWithLot(tc, sName, nCRID, nBUID);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        public List<CRWiseSpareParts> Gets(string sSQL, int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CRWiseSparePartsDA.Gets(tc, sSQL);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
                oCRWiseSpareParts.CRWiseSparePartsID = id;
                CRWiseSparePartsDA.Delete(tc, oCRWiseSpareParts, EnumDBOperation.Delete, nUserId);
                tc.End();
                return "Delete sucessfully";
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                CRWiseSpareParts oCRWiseSpareParts = new CRWiseSpareParts();
                oCRWiseSpareParts.ErrorMessage = e.Message.Split('!')[0];
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete CRWiseSpareParts. Because of " + e.Message, e);
                #endregion
            }

        }
        public CRWiseSpareParts Save(CRWiseSpareParts oCRWiseSpareParts, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCRWiseSpareParts.CRWiseSparePartsID <= 0)
                {
                    reader = CRWiseSparePartsDA.InsertUpdate(tc, oCRWiseSpareParts, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = CRWiseSparePartsDA.InsertUpdate(tc, oCRWiseSpareParts, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCRWiseSpareParts = new CRWiseSpareParts();
                    oCRWiseSpareParts = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCRWiseSpareParts.ErrorMessage = e.Message;
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save CRWiseSpareParts. Because of " + e.Message, e);
                #endregion
            }
            return oCRWiseSpareParts;
        }
        public List<CRWiseSpareParts> SaveFromCopy(List<CRWiseSpareParts> oCRWiseSparePartss, int nUserId)
        {
            List<CRWiseSpareParts> objCRWiseSparePartss = new List<CRWiseSpareParts>();
            CRWiseSpareParts objCRWiseSpareParts = new CRWiseSpareParts();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (CRWiseSpareParts oItem in oCRWiseSparePartss)
                {
                    if (oItem.CRID > 0)
                    {
                        IDataReader reader = null;
                        reader = CRWiseSparePartsDA.SaveFromCopy(tc, oItem, EnumDBOperation.Insert, nUserId);


                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            objCRWiseSpareParts = new CRWiseSpareParts();
                            objCRWiseSpareParts = CreateObject(oReader);
                            objCRWiseSparePartss.Add(objCRWiseSpareParts);
                        }
                        reader.Close();
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }
            return objCRWiseSparePartss;
        }
        public List<CRWiseSpareParts> BUWiseGets(int nBUID, int nUserId)
        {
            List<CRWiseSpareParts> oCRWiseSparePartss = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CRWiseSparePartsDA.BUWiseGets(tc, nBUID);
                oCRWiseSparePartss = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CRWiseSpareParts", e);
                #endregion
            }

            return oCRWiseSparePartss;
        }
        #endregion

    }

}
