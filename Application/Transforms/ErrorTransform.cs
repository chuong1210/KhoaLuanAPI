using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Transforms
{

    public static class ErrorTransform
    {
        public static string EntityNotFound(string entityName, string id)
        {
            return $"{entityName} với ID '{id}' không tồn tại";
        }

        public static string EntityAlreadyExists(string entityName, string field, string value)
        {
            return $"{entityName} với {field} '{value}' đã tồn tại";
        }

        public static string InvalidOperation(string operation)
        {
            return $"Thao tác '{operation}' không hợp lệ";
        }

        public static string ValidationFailed(string field, string message)
        {
            return $"{field}: {message}";
        }

        public static string InsufficientStock(string productName)
        {
            return $"Sản phẩm '{productName}' không đủ số lượng trong kho";
        }

        public static string InvalidVoucher(string reason)
        {
            return $"Voucher không hợp lệ: {reason}";
        }

        public static string OrderNotFound()
        {
            return "Không tìm thấy đơn hàng";
        }

        public static string CannotCancelOrder(string reason)
        {
            return $"Không thể hủy đơn hàng: {reason}";
        }
    }
}
