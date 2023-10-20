using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System.Text.RegularExpressions;

namespace NET1705_FService.API.Helper
{
    public class Validation
    {
        public static bool GetBuildingName(string name)
        {
            string pattern = @"^S\d{3}$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(name))
            {
                return false;
            }
            return true;
        }

        public static bool CheckPhoneNumber(string phoneNumber)
        {
            string phonePattern = @"^\d{10}$";
            return Regex.IsMatch(phoneNumber, phonePattern);
        }
        public static bool CheckName(string name)
        {
            if (string.IsNullOrEmpty(name) || name.Length < 3) 
            {
                return false;
            }
            return true;
        }
    }
}
