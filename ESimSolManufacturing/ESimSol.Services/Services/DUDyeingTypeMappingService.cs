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
   public class DUDyeingTypeMappingService :MarshalByRefObject ,IDUDyeingTypeMappingService
    {
        #region Private functions and declaration
        private static  DUDyeingTypeMapping MapObject(NullHandler oReader)
        {
             DUDyeingTypeMapping  oDUDyeingTypeMapping = new  DUDyeingTypeMapping();
             oDUDyeingTypeMapping.DyeingTypeMappingID = oReader.GetInt32("DyeingTypeMappingID");
             oDUDyeingTypeMapping.DyeingType = (EumDyeingType)oReader.GetInt16("DyeingType");
             oDUDyeingTypeMapping.DyeingTypeInt = oReader.GetInt16("DyeingType");
             oDUDyeingTypeMapping.ProductID = oReader.GetInt32("ProductID");
             oDUDyeingTypeMapping.Unit = oReader.GetDouble("Unit");
            return  oDUDyeingTypeMapping;
        }

        public static  DUDyeingTypeMapping CreateObject(NullHandler oReader)
        {
             DUDyeingTypeMapping  oDUDyeingTypeMapping = MapObject(oReader);
            return  oDUDyeingTypeMapping;
        }

        private List< DUDyeingTypeMapping> CreateObjects(IDataReader oReader)
        {
            List< DUDyeingTypeMapping>  oDUDyeingTypeMappings = new List< DUDyeingTypeMapping>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                 DUDyeingTypeMapping oItem = CreateObject(oHandler);
                 oDUDyeingTypeMappings.Add(oItem);
            }
            return  oDUDyeingTypeMappings;
        }

        #endregion
       #region Interface implementation
        public DUDyeingTypeMappingService() { }

        public DUDyeingTypeMapping IUD(DUDyeingTypeMapping oDUDTM, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                List<DUDyeingTypeMapping> oDUDTMs = oDUDTM.DUDyeingTypeMappings;

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval || nDBOperation == (int)EnumDBOperation.Revise)
                {
                    foreach (DUDyeingTypeMapping oItem in oDUDTMs)
                    {
                        reader = DUDyeingTypeMappingDA.IUD(tc, oItem, nDBOperation, nUserID);
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oDUDTM = new DUDyeingTypeMapping();
                            oDUDTM = CreateObject(oReader);
                        }
                        reader.Close();
                    }
                   
                    
                  

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = DUDyeingTypeMappingDA.IUD(tc, oDUDTM, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oDUDTM.ErrorMessage = Global.DeleteMessage;
                }
                else
                {
                    throw new Exception("Invalid Operation In Service");
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDTM = new DUDyeingTypeMapping();
                oDUDTM.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oDUDTM;
        }

        public DUDyeingTypeMapping Get(int nDyeingTypeMappingID, Int64 nUserId)
        {
            DUDyeingTypeMapping oDUDTM = new DUDyeingTypeMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUDyeingTypeMappingDA.Get(tc, nDyeingTypeMappingID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUDTM = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDTM = new DUDyeingTypeMapping();
                oDUDTM.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oDUDTM;
        }

        public List<DUDyeingTypeMapping> Gets(string sSQL, Int64 nUserID)
        {
            List<DUDyeingTypeMapping> oDUDTMs = new List<DUDyeingTypeMapping>();
            DUDyeingTypeMapping oDUDTM = new DUDyeingTypeMapping();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUDyeingTypeMappingDA.Gets(tc, sSQL);
                oDUDTMs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oDUDTM.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oDUDTMs.Add(oDUDTM);
                #endregion
            }

            return oDUDTMs;
        }


      

        #endregion
    }
}
