using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data.Entity;
using System.Data.SqlClient;
using Platform.DAAS.OData.Framework;
using DIS.Data.DataContract;

namespace DIS.Data.DataController
{
    public class ProductKeyRepository : ODataSQLServerDBRepository<KeyInfo>
    {
        private DataContext context = null;
        private string connectionString = null;

        public ProductKeyRepository(string DBConnectionString)
        {
            this.connectionString = DBConnectionString;
        }
        //public override void Initialize(string Server, int Port, string UserName, string Password, string Catalog, string Schema)
        //{
        //    base.Initialize(Server, Port, UserName, Password, Catalog, Schema);
        //}

        public override string GetConnectionString()
        {
            return this.connectionString;
        }

        public override void SetConnectionString(string ConnectionString)
        {
            this.connectionString = ConnectionString;
        }

        public override KeyInfo Post(KeyInfo Item)
        {
            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                this.context.ProductKeys.Add(Item);

                this.context.SaveChanges();
            }

            return Item;
        }
        //public override bool Delete(string KeyName, object KeyValue)
        //{
        //    return base.Delete(KeyName, KeyValue);
        //}

        public override IQueryable<KeyInfo> Get()
        {
            IQueryable<KeyInfo> result;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                result = this.context.ProductKeys.AsQueryable();
            }

            return result;
        }

        public override IQueryable<KeyInfo> Get(string KeyName, object KeyValue)
        {
            IQueryable<KeyInfo> result;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                result = this.context.ProductKeys.Where(p => p.KeyId == (long)(KeyValue)).AsQueryable();
            }

            return result;
        }

        public override KeyInfo Patch(string KeyName, object KeyValue, KeyInfo Item)
        {
            KeyInfo result;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                result = this.context.ProductKeys.FirstOrDefault(p => p.KeyId == (long)(KeyValue));

                if (result.KeyState != Item.KeyState)
                {
                    result.KeyState = Item.KeyState;
                    result.KeyStateChanged = true;
                }

                if ((result.OemOptionalInfo == null) && (Item.OemOptionalInfo != null))
                {
                    result.OemOptionalInfo = Item.OemOptionalInfo;
                }
                else if ((result.OemOptionalInfo != null) && (Item.OemOptionalInfo != null))
                {
                    if (result.ZCHANNEL_REL_ID != Item.ZCHANNEL_REL_ID)
                    {
                        result.ZCHANNEL_REL_ID = Item.ZCHANNEL_REL_ID;
                    }

                    if (result.ZFRM_FACTOR_CL1 != Item.ZFRM_FACTOR_CL1)
                    {
                        result.ZFRM_FACTOR_CL1 = Item.ZFRM_FACTOR_CL1;
                    }

                    if (result.ZFRM_FACTOR_CL2 != Item.ZFRM_FACTOR_CL2)
                    {
                        result.ZFRM_FACTOR_CL2 = Item.ZFRM_FACTOR_CL2;
                    }

                    if (result.ZMANUF_GEO_LOC != Item.ZMANUF_GEO_LOC)
                    {
                        result.ZMANUF_GEO_LOC = Item.ZMANUF_GEO_LOC;
                    }

                    if (result.ZOEM_EXT_ID != Item.ZOEM_EXT_ID)
                    {
                        result.ZOEM_EXT_ID = Item.ZOEM_EXT_ID;
                    }

                    if (result.ZPC_MODEL_SKU != Item.ZPC_MODEL_SKU)
                    {
                        result.ZPC_MODEL_SKU = Item.ZPC_MODEL_SKU;
                    }

                    if (result.ZPGM_ELIG_VALUES != Item.ZPGM_ELIG_VALUES)
                    {
                        result.ZPGM_ELIG_VALUES = Item.ZPGM_ELIG_VALUES;
                    }

                    if (result.ZSCREEN_SIZE != Item.ZSCREEN_SIZE)
                    {
                        result.ZSCREEN_SIZE = Item.ZSCREEN_SIZE;
                    }

                    if (result.ZTOUCH_SCREEN != Item.ZTOUCH_SCREEN)
                    {
                        result.ZTOUCH_SCREEN = Item.ZTOUCH_SCREEN;
                    }
                }
                
                
                result.ModifiedDate = DateTime.Now;

                this.context.SaveChanges();
            }

            return result;
        }

        public int AddKeys(KeyInfo[] Keys)
        {
            int result = -9;

            using (this.context = new DataContext(new SqlConnection(this.connectionString), false))
            {
                this.context.ProductKeys.AddRange(Keys);

                result = this.context.SaveChanges();
            }

            return result;
        }
    }
}
