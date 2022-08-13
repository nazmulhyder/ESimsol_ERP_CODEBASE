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
    public class MoldRegisterService : MarshalByRefObject, IMoldRegisterService
    {
        #region Private functions and declaration
        private MoldRegister MapObject(NullHandler oReader)
        {
            MoldRegister oMoldRegister = new MoldRegister();
            oMoldRegister.CRID = oReader.GetInt32("CRID");
            oMoldRegister.Cavity = oReader.GetDouble("Cavity");
            oMoldRegister.Name = oReader.GetString("Name");
            oMoldRegister.Code = oReader.GetString("Code");
            oMoldRegister.BUID = oReader.GetInt32("BUID");
            oMoldRegister.RackID = oReader.GetInt32("RackID");
            oMoldRegister.LocationID = oReader.GetInt32("LocationID");
            oMoldRegister.ResourcesType = (EnumResourcesType)oReader.GetInt32("ResourcesType");
            oMoldRegister.ResourcesTypeInInt = oReader.GetInt32("ResourcesType");
            oMoldRegister.ShelfName = oReader.GetString("ShelfName");
            oMoldRegister.RackNo = oReader.GetString("RackNo");
            oMoldRegister.Remarks = oReader.GetString("Remarks");
            oMoldRegister.ContractorName = oReader.GetString("ContractorName");
            oMoldRegister.LocationName = oReader.GetString("LocationName");            
            
            return oMoldRegister;
        }

        private MoldRegister CreateObject(NullHandler oReader)
        {
            MoldRegister oMoldRegister = new MoldRegister();
            oMoldRegister = MapObject(oReader);
            return oMoldRegister;
        }

        private List<MoldRegister> CreateObjects(IDataReader oReader)
        {
            List<MoldRegister> oMoldRegister = new List<MoldRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MoldRegister oItem = CreateObject(oHandler);
                oMoldRegister.Add(oItem);
            }
            return oMoldRegister;
        }

        #endregion

        #region Interface implementation
        public MoldRegisterService() { }        
        public List<MoldRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<MoldRegister> oMoldRegister = new List<MoldRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MoldRegisterDA.Gets(tc, sSQL);
                oMoldRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MoldRegister", e);
                #endregion
            }

            return oMoldRegister;
        }
        #endregion
    }
}
