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

    public class RecapBillOfMaterialService : MarshalByRefObject, IRecapBillOfMaterialService
    {
        #region Private functions and declaration
        private RecapBillOfMaterial MapObject(NullHandler oReader)
        {
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            oRecapBillOfMaterial.RecapBillOfMaterialID = oReader.GetInt32("RecapBillOfMaterialID");
            oRecapBillOfMaterial.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oRecapBillOfMaterial.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oRecapBillOfMaterial.ProductID = oReader.GetInt32("ProductID");
            oRecapBillOfMaterial.ColorID = oReader.GetInt32("ColorID");
            oRecapBillOfMaterial.SizeID = oReader.GetInt32("SizeID");
            oRecapBillOfMaterial.ItemDescription = oReader.GetString("ItemDescription");
            oRecapBillOfMaterial.Reference = oReader.GetString("Reference");
            oRecapBillOfMaterial.Construction = oReader.GetString("Construction");
            oRecapBillOfMaterial.Sequence = oReader.GetInt32("Sequence");
            oRecapBillOfMaterial.MUnitID = oReader.GetInt32("MUnitID");
            oRecapBillOfMaterial.ReqQty = oReader.GetDouble("ReqQty");
            oRecapBillOfMaterial.CuttingQty = oReader.GetDouble("CuttingQty");
            oRecapBillOfMaterial.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oRecapBillOfMaterial.ProductCode = oReader.GetString("ProductCode");
            oRecapBillOfMaterial.ProductName = oReader.GetString("ProductName");
            oRecapBillOfMaterial.ColorName = oReader.GetString("ColorName");
            oRecapBillOfMaterial.SizeName = oReader.GetString("SizeName");
            oRecapBillOfMaterial.Symbol = oReader.GetString("Symbol");
            oRecapBillOfMaterial.UnitName = oReader.GetString("UnitName");
            oRecapBillOfMaterial.POCode = oReader.GetString("POCode");
            oRecapBillOfMaterial.Remarks = oReader.GetString("Remarks");
            oRecapBillOfMaterial.StyleNo = oReader.GetString("StyleNo");
            oRecapBillOfMaterial.AttachFile = oReader.GetBytes("AttachFile");
            oRecapBillOfMaterial.IsAttachmentExist = oReader.GetBoolean("IsAttachmentExist");
            oRecapBillOfMaterial.OrderQty = oReader.GetDouble("OrderQty");
            oRecapBillOfMaterial.BookingQty = oReader.GetDouble("BookingQty");
            oRecapBillOfMaterial.BookingConsumption = oReader.GetDouble("BookingConsumption");
            oRecapBillOfMaterial.BookingConsumptionInPercent = oReader.GetDouble("BookingConsumptionInPercent");

            return oRecapBillOfMaterial;
        }

        private RecapBillOfMaterial CreateObject(NullHandler oReader)
        {
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            oRecapBillOfMaterial = MapObject(oReader);
            return oRecapBillOfMaterial;
        }

        private List<RecapBillOfMaterial> CreateObjects(IDataReader oReader)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterial = new List<RecapBillOfMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RecapBillOfMaterial oItem = CreateObject(oHandler);
                oRecapBillOfMaterial.Add(oItem);
            }
            return oRecapBillOfMaterial;
        }

        #endregion

        #region Interface implementation
        public RecapBillOfMaterialService() { }

        public RecapBillOfMaterial Save(RecapBillOfMaterial oRecapBillOfMaterial, Int64 nUserID)
        {
            int nOrderRecapID = 0;
            string sRecapBillOfMaterialIDs = "";
            TransactionContext tc = null;
            List<RecapBillOfMaterial> oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            oRecapBillOfMaterials = oRecapBillOfMaterial.RecapBillOfMaterials;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (RecapBillOfMaterial oItem in oRecapBillOfMaterials)
                {
                    nOrderRecapID = oItem.OrderRecapID;
                    IDataReader reader;
                    if (oItem.RecapBillOfMaterialID <= 0)
                    {
                        reader = RecapBillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = RecapBillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRecapBillOfMaterial = new RecapBillOfMaterial();
                        oRecapBillOfMaterial = CreateObject(oReader);
                        sRecapBillOfMaterialIDs = sRecapBillOfMaterialIDs + oRecapBillOfMaterial.RecapBillOfMaterialID.ToString() + ",";
                    }
                    reader.Close();
                }
                if (sRecapBillOfMaterialIDs.Length > 0)
                {
                    sRecapBillOfMaterialIDs = sRecapBillOfMaterialIDs.Remove(sRecapBillOfMaterialIDs.Length - 1, 1);
                }
                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.OrderRecapID = nOrderRecapID;
                RecapBillOfMaterialDA.Delete(tc, oRecapBillOfMaterial, EnumDBOperation.Delete, nUserID, sRecapBillOfMaterialIDs);

                oRecapBillOfMaterials = new List<RecapBillOfMaterial>();
                IDataReader readers = null;
                readers = RecapBillOfMaterialDA.Gets(tc, nOrderRecapID);
                oRecapBillOfMaterials = CreateObjects(readers);
                readers.Close();

                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.RecapBillOfMaterials = oRecapBillOfMaterials;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.ErrorMessage = e.Message;
                #endregion
            }
            return oRecapBillOfMaterial;
        }
        public RecapBillOfMaterial UpdateImage(RecapBillOfMaterial oRecapBillOfMaterial, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                byte[] attachfile = oRecapBillOfMaterial.AttachFile;
                tc = TransactionContext.Begin(true);
                if (oRecapBillOfMaterial.RecapBillOfMaterialID > 0 && oRecapBillOfMaterial.AttachFile != null)
                {
                    RecapBillOfMaterialDA.UpdateImage(tc, oRecapBillOfMaterial.AttachFile, oRecapBillOfMaterial.RecapBillOfMaterialID, nUserID);
                    IDataReader reader = RecapBillOfMaterialDA.Get(tc, oRecapBillOfMaterial.RecapBillOfMaterialID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oRecapBillOfMaterial = CreateObject(oReader);
                    }
                    reader.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save RecapBillOfMaterial. Because of " + e.Message, e);
                #endregion
            }
            return oRecapBillOfMaterial;
        }
        public RecapBillOfMaterial GetWithAttachFile(int id, Int64 nUserId)
        {
            RecapBillOfMaterial oRecapBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.GetWithImage(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecapBillOfMaterial = CreateObject(oReader);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oRecapBillOfMaterial;
        }
        public RecapBillOfMaterial DeleteImage(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
            try
            {
                tc = TransactionContext.Begin(true);
                RecapBillOfMaterialDA.DeleteImage(tc, id);
                IDataReader reader = RecapBillOfMaterialDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRecapBillOfMaterial = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRecapBillOfMaterial.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oRecapBillOfMaterial;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.RecapBillOfMaterialID = id;
                RecapBillOfMaterialDA.Delete(tc, oRecapBillOfMaterial, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete RecapBillOfMaterial. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
        public RecapBillOfMaterial Get(int id, Int64 nUserId)
        {
            RecapBillOfMaterial oAccountHead = new RecapBillOfMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RecapBillOfMaterialDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get RecapBillOfMaterial", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<RecapBillOfMaterial> Gets(Int64 nUserID)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.Gets(tc);
                oRecapBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecapBillOfMaterial", e);
                #endregion
            }

            return oRecapBillOfMaterial;
        }

        public List<RecapBillOfMaterial> Gets(int nOrderRecapID, Int64 nUserID)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.Gets(tc, nOrderRecapID);
                oRecapBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecapBillOfMaterial", e);
                #endregion
            }

            return oRecapBillOfMaterial;
        }

        public List<RecapBillOfMaterial> PasteRecapBillOfMaterials(int nOldTSID, int nNewTSID, Int64 nUserID)
        {
            List<RecapBillOfMaterial> oTempRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            List<RecapBillOfMaterial> oNewRecapBillOfMaterials = new List<RecapBillOfMaterial>();
            RecapBillOfMaterial oTempRecapBillOfMaterial = new RecapBillOfMaterial();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.Gets(tc, nOldTSID);
                oTempRecapBillOfMaterials = CreateObjects(reader);
                reader.Close();
                foreach (RecapBillOfMaterial oItem in oTempRecapBillOfMaterials)
                {
                    int nOldBMID = oItem.RecapBillOfMaterialID;
                    oItem.RecapBillOfMaterialID = 0;
                    oItem.OrderRecapID = nNewTSID;
                    reader = RecapBillOfMaterialDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempRecapBillOfMaterial = new RecapBillOfMaterial();
                        oTempRecapBillOfMaterial = CreateObject(oReader);
                        oNewRecapBillOfMaterials.Add(oTempRecapBillOfMaterial);
                    }
                    reader.Close();
                    if (oTempRecapBillOfMaterial.RecapBillOfMaterialID > 0)
                    {
                        RecapBillOfMaterialDA.UpdateImageFromBM(tc, nOldBMID, oTempRecapBillOfMaterial.RecapBillOfMaterialID);
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
                throw new ServiceException("Failed to Get RecapBillOfMaterial", e);
                #endregion
            }

            return oNewRecapBillOfMaterials;
        }
        public List<RecapBillOfMaterial> GetsWithImage(int nOrderRecapID, Int64 nUserID)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.GetsWithImage(tc, nOrderRecapID);
                oRecapBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RecapBillOfMaterial", e);
                #endregion
            }

            return oRecapBillOfMaterial;
        }

        public List<RecapBillOfMaterial> ResetSequence(List<RecapBillOfMaterial> oRecapBillOfMaterials, Int64 nUserID)
        {
            List<RecapBillOfMaterial> oTempRecapBillOfMaterials = new List<RecapBillOfMaterial>();

            TransactionContext tc = null;
            int nTempOrderRecapID = oRecapBillOfMaterials[0].OrderRecapID;
            try
            {
                tc = TransactionContext.Begin();
                foreach (RecapBillOfMaterial oItem in oRecapBillOfMaterials)
                {
                    RecapBillOfMaterialDA.UpDateSequence(tc, oItem.RecapBillOfMaterialID, oItem.Sequence);
                }
                IDataReader reader;
                reader = RecapBillOfMaterialDA.Gets(tc, nTempOrderRecapID);
                oTempRecapBillOfMaterials = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTempRecapBillOfMaterials = new List<RecapBillOfMaterial>();
                RecapBillOfMaterial oRecapBillOfMaterial = new RecapBillOfMaterial();
                oRecapBillOfMaterial.ErrorMessage = e.Message;
                oTempRecapBillOfMaterials.Add(oRecapBillOfMaterial);
                #endregion
            }

            return oTempRecapBillOfMaterials;
        }


        public List<RecapBillOfMaterial> Gets_Report(int id, Int64 nUserID)
        {
            List<RecapBillOfMaterial> oRecapBillOfMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RecapBillOfMaterialDA.Gets_Report(tc, id);
                oRecapBillOfMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecap", e);
                #endregion
            }

            return oRecapBillOfMaterial;
        }
        #endregion
    }
    
 
}
