using web_tabel.Domain;

namespace web_tabel.Services
{
    public class Constants
    {
        public const string AUTH_COOKIE_NAME = ".web_table.AuthCookie";
        public const string UNAUTHORIZED_PATH = "/Error/Unauthorized";
        public const string FORBIDDEN_PATH = "/Error/Forbidden";
        
        public const string ADMIN_ROLE = "Admin";
        public const string PAYROLL_ROLE = "Payroll";
        public const string TIMEKEEPER_ROLE = "Timekeeper";
        public const string USER_ROLE = "User";

        private static readonly object _unitOfWork;

        public static async Task<T> GetConstantValue<T>(string name)
        {
            using(UnitOfWork uow = new UnitOfWork())
            {
                var constant = uow.ConstantRepository.SingleOrDefault(c => c.Name == name);

                if (constant == null)
                {
                    await SetConstantValue<T>(name, default(T));
                    return default(T);
                }

                var convertedValue = (T)Convert.ChangeType(constant.Value, typeof(T));
                return convertedValue;
            }
        }

        public static async Task SetConstantValue<T>(string Name, T value)
        {
            using(UnitOfWork uow = new UnitOfWork())
            {
                var exist = await uow.ConstantRepository.SingleOrDefaultAsync(c => c.Name == Name);
                if (exist != null)
                {
                    exist.Value = value.ToString();
                    await uow.ConstantRepository.UpdateAsync(exist);
                }
                else
                {
                    var constant = new Constant
                    {
                        Name = Name,
                        ValueType = typeof(T).ToString(),
                        Value = value.ToString(),
                    };
                    await uow.ConstantRepository.InsertAsync(constant);
                }
                await uow.SaveAsync();
            }
            
        }
    }
}
