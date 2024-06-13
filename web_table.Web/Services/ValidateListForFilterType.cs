using System.ComponentModel.DataAnnotations;
using web_table.Web.ViewModel;

namespace web_table.Web.Services
{
    public class ValidateListForFilterType : ValidationAttribute
    {
        private readonly string _filterType;

        public ValidateListForFilterType(string filterType)
        {
            _filterType = filterType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var instance = (CreateFilterViewModel)validationContext.ObjectInstance;

            if(instance.FilterType == _filterType)
            {
                var list = value as List<Guid>;
                if(list == null || !list.Any())
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }

    }
}
