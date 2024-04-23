using Microsoft.AspNetCore.Mvc.Rendering;

namespace web_table.Web
{
    public static class SelectListHelper
    {
        public static IEnumerable<SelectListItem> ToSelectListItem<T>(this IEnumerable<T> list, Func<T, object> valueSelector, Func<T, string> textSelector, string[] selectedValue)
        {
            if (selectedValue == null || selectedValue.Length == 0) selectedValue = new string[0];

            bool isSelected(string value) => selectedValue != null && selectedValue.Contains(value);

            return list.Select(x => new SelectListItem
            {
                Value = valueSelector(x).ToString(),
                Text = textSelector(x),
                Selected = isSelected(valueSelector(x).ToString())
            }); 
        }
    }
}
