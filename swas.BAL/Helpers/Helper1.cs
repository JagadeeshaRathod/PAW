
using swas.BAL.DTO;

namespace swas.UI.Helpers
{
    public static class Helper1
    {
        public static string LoginDetails(Login Logins)
        {
            string result = $"{Logins.Rank.Trim()} {Logins.Offr_Name.Trim()} / {Logins.UserName.Trim()}";

            return result;
            //return Logins.Rank.Trim() + " " + Logins.Offr_Name.Trim()+" / "+Logins.UserName.Trim()+ "";
        }
    }
}
