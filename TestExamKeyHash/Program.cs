using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestExamKeyHash
{
    internal class Program
    {
		[DllImport("seb_x64.dll", CallingConvention = CallingConvention.Cdecl)]
		[return: MarshalAs(UnmanagedType.BStr)]
		private static extern string CalculateBrowserExamKey(string configurationKey, string salt);

		private static string ToString(byte[] bytes)
		{
			return BitConverter.ToString(bytes).ToLower().Replace("-", string.Empty);
		}

		static void Main(string[] args)
        {
			var configurationKey = "9e9ed794dab5a72fa2e001a15b1894f9a40d1418af61234d7e4de5d91654bb50";

			byte[] salt = new byte[32];

			salt[0] = 215;
			salt[1] = 90;
			salt[2] = 181;
			salt[3] = 181;
			salt[4] = 166;
			salt[5] = 102;
			salt[6] = 67;
			salt[7] = 223;
			salt[8] = 47;
			salt[9] = 15;
			salt[10] = 2;
			salt[11] = 78;
			salt[12] = 53;
			salt[13] = 142;
			salt[14] = 32;
			salt[15] = 229;
			salt[16] = 174;
			salt[17] = 193;
			salt[18] = 217;
			salt[19] = 224;
			salt[20] = 97;
			salt[21] = 83;
			salt[22] = 122;
			salt[23] = 131;
			salt[24] = 29;
			salt[25] = 137;
			salt[26] = 11;
			salt[27] = 23;
			salt[28] = 71;
			salt[29] = 88;
			salt[30] = 114;
			salt[31] = 135;

			var hash = CalculateBrowserExamKey(configurationKey, ToString(salt));
			Console.WriteLine(hash);

			Console.ReadKey();
        }
    }
}
