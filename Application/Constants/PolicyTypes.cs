using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants
{
    public static class PolicyTypes
    {
        public const string TERMS = "TERMS"; // Điều khoản sử dụng
        public const string PRIVACY = "PRIVACY"; // Chính sách bảo mật
        public const string RETURN = "RETURN"; // Chính sách đổi trả
        public const string WARRANTY = "WARRANTY"; // Chính sách bảo hành
        public const string SHIPPING = "SHIPPING"; // Chính sách giao hàng

        public static readonly List<string> All = new()
        {
            TERMS, PRIVACY, RETURN, WARRANTY, SHIPPING
        };

        public static string GetDisplayName(string type)
        {
            return type switch
            {
                TERMS => "Điều khoản sử dụng",
                PRIVACY => "Chính sách bảo mật",
                RETURN => "Chính sách đổi trả",
                WARRANTY => "Chính sách bảo hành",
                SHIPPING => "Chính sách giao hàng",
                _ => type
            };
        }
    }
    }
