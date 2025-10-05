using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// Application/Transforms/SuccessTransform.cs
namespace Application.Transforms
{
    public static class SuccessTransform
    {
        public static string Created(string entityName)
        {
            return $"{entityName} đã được tạo thành công";
        }

        public static string Updated(string entityName)
        {
            return $"{entityName} đã được cập nhật thành công";
        }

        public static string Deleted(string entityName)
        {
            return $"{entityName} đã được xóa thành công";
        }

        public static string OperationSuccess(string operation)
        {
            return $"{operation} thành công";
        }

        public static string OrderPlaced()
        {
            return "Đặt hàng thành công";
        }

        public static string OrderCancelled()
        {
            return "Đơn hàng đã được hủy";
        }

        public static string PaymentSuccess()
        {
            return "Thanh toán thành công";
        }

        public static string ReturnRequestCreated()
        {
            return "Yêu cầu trả hàng đã được gửi";
        }
    }
}
