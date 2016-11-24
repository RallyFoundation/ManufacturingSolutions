using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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

            List<SearchingArgument> searchingArgs = new List<SearchingArgument>() { new SearchingArgument
            {
                FieldName = "Name",
                FieldValue = "New",
                LogicalOperator = LogicalOperatorEnum.And,
                Operator = OperatorEnum.StartsWith //OperatorEnum.NotInclude //OperatorEnum.Includes
            },
            new SearchingArgument
            {
                FieldName = "Name",
                FieldValue = "New",
                LogicalOperator = LogicalOperatorEnum.Or,
                Operator = OperatorEnum.NotEndWith //OperatorEnum.NotInclude //OperatorEnum.Includes
            },
            new SearchingArgument
            {
                FieldName = "Name",
                FieldValue = "New",
                LogicalOperator = LogicalOperatorEnum.Or,
                Operator = OperatorEnum.Includes //OperatorEnum.NotInclude //OperatorEnum.Includes
            },
            new SearchingArgument
            {
                FieldName = "CreationTime",
                FieldValue = DateTime.Now,
                LogicalOperator = LogicalOperatorEnum.And,
                Operator = OperatorEnum.LessThan
            }};

            //var bizArray = bizManager.SearchBusiness(null, null, pagingArg);

            var bizArray = bizManager.SearchBusiness(bizManager.CreateBusinessQueryExpression, searchingArgs, pagingArg);

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

        static Expression<Func<Business, bool>> GetExpression(IList<SearchingArgument> SearchingArgs)
        {
            Expression<Func<Business, bool>> expression = null;

            ParameterExpression parameter = Expression.Parameter(typeof(Business), "b");

            MemberExpression member = Expression.Field(parameter, "");

            ConstantExpression value = Expression.Constant(null);

            BinaryExpression binaryOperator = Expression.Equal(member, value);

            BinaryExpression binaryLogical = Expression.And(null, null);

            List<BinaryExpression> binaryExprs = new List<BinaryExpression>();

            if (SearchingArgs != null && SearchingArgs.Count > 0)
            {
                foreach (var arg in SearchingArgs)
                {
                    member = Expression.Field(parameter, arg.FieldName);
                    value = Expression.Constant(arg.FieldValue);

                    //if (arg.Operator == OperatorEnum.Includes)
                    //{
                    //    //expression = Expression.Parameter(typeof(String), arg.FieldName) => Expression.Parameter(typeof(String), arg.FieldName)arg.FieldValue
                    //}

                    switch (arg.Operator)
                    {
                        case OperatorEnum.EqualTo:
                            binaryOperator = Expression.Equal(member, value);
                            break;
                        case OperatorEnum.NotEqualTo:
                            binaryOperator = Expression.NotEqual(member, value);
                            break;
                        case OperatorEnum.GreaterThan:
                            binaryOperator = Expression.GreaterThan(member, value);
                            break;
                        case OperatorEnum.GreaterThanOrEqualTo:
                            binaryOperator = Expression.GreaterThanOrEqual(member, value);
                            break;
                        case OperatorEnum.In:
                            break;
                        case OperatorEnum.NotIn:
                            break;
                        case OperatorEnum.Is:
                            break;
                        case OperatorEnum.IsNot:
                            break;
                        case OperatorEnum.LessThan:
                            break;
                        case OperatorEnum.LessThanOrEqualTo:
                            break;
                        case OperatorEnum.StartsWith:
                            break;
                        case OperatorEnum.NotStartWith:
                            break;
                        case OperatorEnum.EndsWith:
                            break;
                        case OperatorEnum.NotEndWith:
                            break;
                        case OperatorEnum.Includes:
                            //binaryOperator = BinaryExpression.IsTrue(BinaryExpression.Call(member, "Contains", new Type[] { typeof (string)}, value));
                            //binaryOperator = BinaryExpression.Call(member.Expression, "Contains", new Type[] { typeof(string) }, value);
                            break;
                        case OperatorEnum.NotInclude:
                            //binaryOperator = BinaryExpression.IsFalse(BinaryExpression.Call(member, "Contains", new Type[] { typeof(string) }, value));
                            break;
                        default:
                            break;
                    }

                    binaryExprs.Add(binaryOperator);

                    for (int i = 0; i < SearchingArgs.Count; i++)
                    {
                        for (int j = 0; j < binaryExprs.Count; j++)
                        {
                            //if (SearchingArgs[i].LogicalOperator == LogicalOperatorEnum.And)
                            //{
                                
                                
                            //}

                            if (i == 0 && j == 0)
                            {
                                binaryLogical = (SearchingArgs[i].LogicalOperator == LogicalOperatorEnum.And) ? Expression.And(binaryExprs[j], binaryExprs[j + 1]) : Expression.Or(binaryExprs[j], binaryExprs[j + 1]);
                            }
                            else if (j != (binaryExprs.Count - 1))
                            {
                                binaryLogical = (SearchingArgs[i].LogicalOperator == LogicalOperatorEnum.And) ? Expression.And(binaryLogical, binaryExprs[j + 1]) : Expression.Or(binaryLogical, binaryExprs[j + 1]);
                            }
                        }
                    }
                }
            }

            expression = Expression.Lambda<Func<Business, bool>>(binaryLogical, new ParameterExpression[] { parameter });

            return expression;
        }
    }
}
