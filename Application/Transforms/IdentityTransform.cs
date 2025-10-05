using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transforms
{
    public static class IdentityTransform
    {
        public static string UserNameAlreadyExists(string userName)
        {
            return $"Tên người dùng {userName} đã tồn tại!";
        }

        public static string EmailAlreadyExists(string userName)
        {
            return $"Email {userName} đã được đăng ký!";
        }

        public static string PhoneNumberAlreadyExists(string userName)
        {
            return $"Số điện thoại {userName} đã được đăng ký!";
        }

        public static string UserNotExists(string userName)
        {
            return $"Không tìm thấy người dùng {userName}!";
        }

        public static string InvalidCredentials(string userName)
        {
            return $"Thông tin xác thực của người dùng {userName} không hợp lệ!";
        }

        public static string ForbiddenException()
        {
            return "Bạn không được phép truy cập tài nguyên này!";
        }

        public static string UnauthorizedException()
        {
            return "Unauthorized!";
        }



        public static string InvalidCredentials()
        {
            return "Tên đăng nhập hoặc mật khẩu không đúng";
        }

        public static string UserNotFound()
        {
            return "Không tìm thấy người dùng";
        }

        public static string EmailAlreadyExists()
        {
            return "Email đã tồn tại trong hệ thống";
        }

        public static string UsernameAlreadyExists()
        {
            return "Tên đăng nhập đã tồn tại trong hệ thống";
        }

        public static string PasswordTooShort()
        {
            return "Mật khẩu phải có ít nhất 6 ký tự";
        }

        public static string PasswordRequiresNonAlphanumeric()
        {
            return "Mật khẩu phải chứa ít nhất một ký tự đặc biệt";
        }

        public static string PasswordRequiresDigit()
        {
            return "Mật khẩu phải chứa ít nhất một chữ số";
        }

        public static string PasswordRequiresUpper()
        {
            return "Mật khẩu phải chứa ít nhất một chữ hoa";
        }
    }
}
