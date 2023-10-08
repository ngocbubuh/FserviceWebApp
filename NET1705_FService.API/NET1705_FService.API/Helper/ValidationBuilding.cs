using NET1705_FService.Repositories.Data;
using NET1705_FService.Repositories.Models;
using System.Text.RegularExpressions;

namespace NET1705_FService.API.Helper
{
    public class ValidationBuilding
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
    }
}
