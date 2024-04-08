using System.Globalization;

namespace SV20T1020105.Web
{
    public static class Converter
    {
        /// <summary>
        /// Chuyen chuoi s sang gia tri kieu DateTime (neu chuyen khong thanh cong
        /// thi gia tri tra ve null)
        /// </summary>
        /// <param name="s"></param>
        /// <param name="formats"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string s, string formats = "d/M/yyyy;d-M-yyyy;d.M.yyyy")
        {
            try
            {
                return DateTime.ParseExact(s, formats.Split(';'), CultureInfo.InvariantCulture);
            }
            catch
            {
                return null;
            }
        }
    }
}
