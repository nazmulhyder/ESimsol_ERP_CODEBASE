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
    public class CapitalResourceService : MarshalByRefObject, ICapitalResourceService
    {
        #region Private functions and declaration
        private CapitalResource MapObject(NullHandler oReader)
        {
            CapitalResource oCapitalResource = new CapitalResource();
            oCapitalResource.CRID = oReader.GetInt32("CRID");
            oCapitalResource.CRGID = oReader.GetInt32("CRGID");
            oCapitalResource.Code = oReader.GetString("Code");
            oCapitalResource.Name = oReader.GetString("Name");
            oCapitalResource.ParentID = oReader.GetInt32("ParentID");
            oCapitalResource.IsLastLayer = oReader.GetBoolean("IsLastLayer");
            oCapitalResource.Model = oReader.GetString("Model");
            oCapitalResource.Brand = oReader.GetString("Brand");
            oCapitalResource.MadeIn = oReader.GetString("MadeIn");
            oCapitalResource.MadeBy = oReader.GetString("MadeBy");
            oCapitalResource.MachineCapacity = oReader.GetString("MachineCapacity");
            oCapitalResource.Warranty = oReader.GetInt32("Warranty");
            oCapitalResource.WarrantyOn = oReader.GetString("WarrantyOn");
            oCapitalResource.WarrantyStart = oReader.GetDateTime("WarrantyStart");
            oCapitalResource.WarrantyEnd = oReader.GetDateTime("WarrantyEnd");
            oCapitalResource.SerialNumberOnProduct = oReader.GetString("SerialNumberOnProduct");
            oCapitalResource.TagNo = oReader.GetString("TagNo");
            oCapitalResource.ActualAssetValue = oReader.GetDouble("ActualAssetValue");
            oCapitalResource.ValueAfterEvaluation = oReader.GetDouble("ValueAfterEvaluation");
            oCapitalResource.CNF_FOBValue_Foreign = oReader.GetDouble("CNF_FOBValue_Foreign");
            oCapitalResource.CNF_FOBValue_Local = oReader.GetDouble("CNF_FOBValue_Local");
            oCapitalResource.TotalLandedCost = oReader.GetDouble("TotalLandedCost");
            oCapitalResource.InstallationCost = oReader.GetDouble("InstallationCost");
            oCapitalResource.OtherCost = oReader.GetDouble("OtherCost");
            oCapitalResource.CurrencyID = oReader.GetInt32("CurrencyID");
            oCapitalResource.Note = oReader.GetString("Note");
            oCapitalResource.IsActive = oReader.GetBoolean("IsActive");
            oCapitalResource.SupplierID = oReader.GetInt32("SupplierID");
            oCapitalResource.SupplierAddress = oReader.GetString("SupplierAddress");
            oCapitalResource.SupplierContactPerson = oReader.GetString("SupplierContactPerson");
            oCapitalResource.SupplierContactPersonContact = oReader.GetString("SupplierContactPersonContact");
            oCapitalResource.SupplierNote = oReader.GetString("SupplierNote");
            oCapitalResource.LAName = oReader.GetString("LAName");
            oCapitalResource.LAContactPerson = oReader.GetString("LAContactPerson");
            oCapitalResource.LAAddress = oReader.GetString("LAAddress");
            oCapitalResource.LAWorkshop = oReader.GetString("LAWorkshop");
            oCapitalResource.LANote = oReader.GetString("LANote");
            oCapitalResource.InstallationDate = oReader.GetDateTime("InstallationDate");
            oCapitalResource.InstallationNote = oReader.GetString("InstallationNote");
            oCapitalResource.InstallationLocationID = oReader.GetInt32("InstallationLocationID");
            oCapitalResource.BasicFunction = oReader.GetString("BasicFunction");
            oCapitalResource.MachineLifeTime = oReader.GetString("MachineLifeTime");
            oCapitalResource.PowerConsumption = oReader.GetString("PowerConsumption");
            oCapitalResource.TechnicalSpecification = oReader.GetString("TechnicalSpecification");
            oCapitalResource.PerformanceSpecification = oReader.GetString("PerformanceSpecification");
            oCapitalResource.PortOfShipment = oReader.GetString("PortOfShipment");
            oCapitalResource.LCNo = oReader.GetString("LCNo");
            oCapitalResource.ParentName = oReader.GetString("ParentName");
            oCapitalResource.HSCode = oReader.GetString("HSCode");
            oCapitalResource.SupplierEmail = oReader.GetString("SupplierEmail");
            oCapitalResource.SupplierPhone = oReader.GetString("SupplierPhone");
            oCapitalResource.SupplierFax = oReader.GetString("SupplierFax");
            oCapitalResource.LAEmail = oReader.GetString("LAEmail");
            oCapitalResource.LAPhone = oReader.GetString("LAPhone");
            oCapitalResource.LAFax = oReader.GetString("LAFax");
            oCapitalResource.CommissioningDate = oReader.GetDateTime("CommissioningDate");
            oCapitalResource.CommissioningBy = oReader.GetString("CommissioningBy");
            oCapitalResource.InsuranceCost = oReader.GetDouble("InsuranceCost");
            oCapitalResource.CustomDutyCost = oReader.GetDouble("CustomDutyCost");
            oCapitalResource.ContractorName = oReader.GetString("ContractorName");
            oCapitalResource.LocationName = oReader.GetString("LocationName");
            oCapitalResource.OperationUnitName = oReader.GetString("OperationUnitName");
            oCapitalResource.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCapitalResource.BUID = oReader.GetInt32("BUID");
            oCapitalResource.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oCapitalResource.Cavity = oReader.GetInt32("Cavity");
            oCapitalResource.ResourcesType = (EnumResourcesType)oReader.GetInt32("ResourcesType");
            oCapitalResource.ResourcesTypeInInt = oReader.GetInt32("ResourcesType");
            oCapitalResource.LocationID = oReader.GetInt32("LocationID");
            oCapitalResource.ShelfID = oReader.GetInt32("ShelfID");
            oCapitalResource.RackNo = oReader.GetString("RackNo");
            oCapitalResource.FinishGoodWeight = oReader.GetDouble("FinishGoodWeight");
            oCapitalResource.NaliWeight = oReader.GetDouble("NaliWeight");
            oCapitalResource.FGWeightUnit = oReader.GetInt32("FGWeightUnit");
            oCapitalResource.FGWeightUnitSymbol = oReader.GetString("FGWeightUnitSymbol");
            oCapitalResource.FGWeightUnitName = oReader.GetString("FGWeightUnitName");
            return oCapitalResource;
        }

        private CapitalResource CreateObject(NullHandler oReader)
        {
            CapitalResource oCapitalResource = new CapitalResource();
            oCapitalResource = MapObject(oReader);
            return oCapitalResource;
        }

        private List<CapitalResource> CreateObjects(IDataReader oReader)
        {
            List<CapitalResource> oCapitalResource = new List<CapitalResource>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CapitalResource oItem = CreateObject(oHandler);
                oCapitalResource.Add(oItem);
            }
            return oCapitalResource;
        }

        #endregion

        #region Interface implementation
        public CapitalResourceService() { }

        public CapitalResource IUD(CapitalResource oCapitalResource, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;

                    reader = CapitalResourceDA.InsertUpdate(tc, oCapitalResource, nDBOperation, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oCapitalResource = new CapitalResource();
                        oCapitalResource = CreateObject(oReader);
                    }
                    reader.Close();
                    tc.End();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    tc = TransactionContext.Begin(true);
                    CapitalResourceDA.Delete(tc, oCapitalResource, nDBOperation, nUserID);
                    tc.End();
                    oCapitalResource = new CapitalResource();
                    oCapitalResource.ErrorMessage = "Delete Successfully.";
                }


            }
            catch (Exception ex)
            {
                oCapitalResource = new CapitalResource();
                oCapitalResource.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
            }
            return oCapitalResource;
        }

        public CapitalResource Get(int nCRID, Int64 nUserId)
        {
            CapitalResource oAccountHead = new CapitalResource();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CapitalResourceDA.Get(tc, nCRID, nUserId);
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
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CapitalResource> Gets(string sSQL, Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.Gets(tc, sSQL);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public List<CapitalResource> GetsResourceType(Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.GetsResourceType(tc);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public List<CapitalResource> GetsResourceTypeWithBU(int buid, Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.GetsResourceTypeWithBU(tc, buid);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public List<CapitalResource> BUWiseGets(int nBUID, Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.BUWiseGets(tc, nBUID);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public List<CapitalResource> BUWiseResourceTypeGets(int nBUID, Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.BUWiseResourceTypeGets(tc, nBUID);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public List<CapitalResource> GetsByBUandResourceTypeWithName(int nBUID, int nResourceType, string Name, Int64 nUserID)
        {
            List<CapitalResource> oCapitalResource = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CapitalResourceDA.GetsByBUandResourceTypeWithName(tc, nBUID, nResourceType, Name);
                oCapitalResource = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CapitalResource", e);
                #endregion
            }

            return oCapitalResource;
        }
        public CapitalResource CopyCR(int nCapitalResourceID, Int64 nUserID)
        {
            TransactionContext tc = null;
            CapitalResource oCapitalResource = new CapitalResource();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CapitalResourceDA.CopyCR(tc, nCapitalResourceID, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCapitalResource = new CapitalResource();
                    oCapitalResource = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

            }
            catch (Exception ex)
            {
                oCapitalResource = new CapitalResource();
                oCapitalResource.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
            }
            return oCapitalResource;
        }
        public CapitalResource CopyCapitalResource(CapitalResource oCapitalResource, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                oCapitalResource.CRID = 0;
                oCapitalResource.Name = oCapitalResource.Name + "-copy";
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CapitalResourceDA.InsertUpdate(tc, oCapitalResource, (int)EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCapitalResource = new CapitalResource();
                    oCapitalResource = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                oCapitalResource = new CapitalResource();
                oCapitalResource.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
            }
            return oCapitalResource;
        }

        #endregion
    }
}