using swas.BAL.DTO;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace swas.BAL.Utility
{
    ///Created and Reviewed by : Sub Maj Sanal
    ///Reviewed Date : 30 Jul 23
    ///Tested By :- 
    ///Tested Date : 
    ///Start
    public static class ExtensionMethods
    {

        public static ProjIDRes FirstSecond(string Projname, int projid, int psmid)
        {
            ProjIDRes pwds = new ProjIDRes();

            if (Projname.Length >= 2)
            {
                pwds.ProjWdOne = Projname?.Length >= 1 ? Projname[0].ToString().ToUpper() : "A";

                if (pwds.ProjWdOne == "" || pwds.ProjWdOne == "-" || pwds.ProjWdOne == "/" || pwds.ProjWdOne == "_")
                {
                    pwds.ProjWdTwo = "M";
                }


                int lastIndex = Projname.Length - 1;
                pwds.ProjWdTwo = Projname[lastIndex].ToString().ToUpper();

                if (pwds.ProjWdTwo == "" || pwds.ProjWdTwo == "-" || pwds.ProjWdTwo == "/" || pwds.ProjWdTwo == "_")
                {
                    pwds.ProjWdTwo = "I";
                }
                DateTime currentDate = DateTime.Now;


                string dayAbbreviation = currentDate.ToString("ddd");

                string abvn = dayAbbreviation?.Length >= 1 ? dayAbbreviation[1].ToString().ToUpper() : "A";
                abvn = abvn + dayAbbreviation[0].ToString().ToUpper();


                pwds.PorjPin = pwds.ProjWdOne + projid + pwds.ProjWdTwo + abvn;
                return pwds;


            }
            else
            {
                pwds.ProjWdOne = "M";
                pwds.ProjWdTwo = "S";
                return pwds;
            }

        }



        public static string GetDisplayName(this Enum enumValue)
        {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? enumValue.ToString();
        }

        public static string ToCastString(this object value)
        {
            return value != null ? value.ToString() : null;
        }
        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value ?? 0);
        }
        public static long ToInt64(this object value)
        {
            return Convert.ToInt64(value ?? 0);
        }
        public static short ToInt16(this object value)
        {
            return Convert.ToInt16(value ?? 0);
        }
        public static bool IsNotNull(this object value)
        {
            return value != null;
        }
        public static bool IsGreaterThanZero(this int value)
        {
            return value > 0;
        }
        public static bool IsGreaterThanZero(this long value)
        {
            return value > 0;
        }
        public static bool IsGreaterThanZero(this short value)
        {
            return value > 0;
        }
        public static DateTime ToDateTime(this string value)
        {
            return Convert.ToDateTime(value);
        }
        public static DateTime ToDateTimeFromUnix(this long unixTimeStamp)
        {
            DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }
        public static long ToUnixTime(this DateTime datetime)
        {
            return ((DateTimeOffset)datetime).ToUnixTimeSeconds();
        }

    }
    public static class Get
    {
        public static DateTime DateTimeNow()
        {
            return DateTime.Now;
        }
        public static long UnixTimeNow()
        {
            return DateTimeOffset.Now.ToUnixTimeSeconds();
        }

    }







}

