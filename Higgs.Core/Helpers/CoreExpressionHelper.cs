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
            if(expression.Body is MemberExpression)
            {
                var me = (MemberExpression)expression.Body;

                yield return me.Member;
            }
            else if(expression.Body is NewExpression)
            {
                var ne = (NewExpression) expression.Body;
                foreach (var me in ne.Arguments.Select(arg => arg as MemberExpression))
                {
                    if (me == null || me.Expression != expression.Parameters[0])
                        throw new Exception(Resources.InvalidExpressionForGetMemberInfoes);

                    yield return me.Member;
                }
            }
            else
            {
                throw new Exception(Resources.InvalidExpressionForGetMemberInfoes);
            }
        }
    }
}
