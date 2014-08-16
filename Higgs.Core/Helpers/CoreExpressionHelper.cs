using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Higgs.Core.Helpers
{
    public static class CoreExpressionHelper
    {
        public static IEnumerable<MemberInfo> GetMemberInfoesFromExpression<T>(Expression<Func<T,object>> expression)
        {
            var body = expression.Body as MemberExpression;

            if(body != null)
            {
                var me = body;

                yield return me.Member;
            }
            else
            {
                var newExpression = expression.Body as NewExpression;

                if(newExpression != null)
                {
                    var ne = newExpression;
                    foreach (var me in ne.Arguments.Select(arg => arg as MemberExpression))
                    {
                        if (me == null || me.Expression != expression.Parameters[0])
                            throw new Exception(Resources.InvalidExpressionForGetMemberInfoes);

                        yield return me.Member;
                    }
                }
                else
                {
                    var unaryExpression = expression.Body as UnaryExpression;

                    if (unaryExpression != null)
                    {
                        var unary = unaryExpression;
                        var me = unary.Operand as MemberExpression;

                        if (me != null)
                        {
                            yield return me.Member;
                        }
                        else
                        {
                            throw new Exception(Resources.InvalidExpressionForGetMemberInfoes);
                        }
                    }
                    else
                    {
                        throw new Exception(Resources.InvalidExpressionForGetMemberInfoes);
                    }
                }
            }
        }
    }
}
