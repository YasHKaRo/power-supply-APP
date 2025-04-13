using power_supply_APP.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace power_supply_APP.ViewModel
{
    public enum UserRole
    {
        Admin,
        User
    }
    public static class AuthManager
    {
        // По умолчанию роль пользователя - User
        public static UserRole CurrentUserRole { get; set; } = UserRole.User;
    }
}
