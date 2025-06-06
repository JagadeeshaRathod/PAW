using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using swas.BAL.Utility;
using System.Linq.Expressions;

namespace swas.BAL.Helpers
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 10 Aug 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public static class DropdownHelper
    {
        public static List<SelectListItem> GetDropdownList<TModel, TValue>(
            IEnumerable<TModel> items,
            Expression<Func<TModel, TValue>> valueExpression,
            Expression<Func<TModel, string>> textExpression,
            object selectedValue = null,
            string defaultOptionText = "------ Select -------",
            string defaultOptionValue = "")
        {
            var valueSelector = valueExpression.Compile();
            var textSelector = textExpression.Compile();

            var selectList = items.Select(item => new SelectListItem
            {
                Value = valueSelector(item).ToString(),
                Text = textSelector(item),
                Selected = (selectedValue != null && valueSelector(item).Equals(selectedValue))
            }).ToList();

            return selectList;
        }
    }

}
