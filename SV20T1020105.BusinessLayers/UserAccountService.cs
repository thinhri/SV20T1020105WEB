using SV20T1020105.DataLayers;
using SV20T1020105.DataLayers.SQLServer;
using SV20T1020105.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SV20T1020105.BusinessLayers
{
	public static class UserAccountService
	{
		private static readonly IUserAccountDAL employeeAccountDB;
		static UserAccountService()
		{
			employeeAccountDB = new EmployeeAccountDAL(Configuration.ConnectionString);
		}
		public static UserAccount? Authorize(string userName, string password)
		{
			//TODO: Kiểm tra thông tin đăng nhập của Employee
			return employeeAccountDB.Authorize(userName, password);
		}
		public static bool ChangePassword(string userName, string oldPassword, string newPassword)
		{

			//TODO: Thay đổi mật khẩu của Employee
			return employeeAccountDB.ChangePassword(userName, oldPassword, newPassword);
		}

/*		public static bool CheckPassword(string email, string oldPassword)
		{
			return CheckPassword(email, oldPassword);
		}
        public static UserAccount? GetUser(int id)
        {
            return employeeAccountDB.Get(id);
        }*/
    }

}
