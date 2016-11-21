using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Platform.DAAS.OData.Core;
using Platform.DAAS.OData.BusinessManagement;

namespace UnitTestBusinessManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            TestBusinessSearchPaging();

            Console.Read();
        }

        static void TestBusinessSearchPaging()
        {
            BusinessManager bizManager = new BusinessManager();

            PagingArgument pagingArg = new PagingArgument()
            {
                 CurrentPageIndex = 0,
                 EachPageSize = 125
            };

            var bizArray = bizManager.SearchBusiness(null, null, pagingArg);

            if (bizArray != null && bizArray.Length > 0)
            {
                foreach (var biz in bizArray)
                {
                    Console.WriteLine(biz.ID);
                    Console.WriteLine(biz.Name);
                }
            }
            else
            {
                Console.WriteLine("None!");
            }
        }
    }
}
