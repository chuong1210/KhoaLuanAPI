namespace KhoaLuanTotNghiepAPI.Constants
{
    public static class CONSTANT_CLAIM_TYPES
    {
        public const string USER_ID = "UserId";
        public const string USER_PROFILE_ID = "UserProfileId";
        public const string USERNAME = "Username";
        public const string EMAIL = "Email";
        public const string ROLES = "Roles";
    }

    public static class ROLES
    {
        public const string ADMIN = "Admin";
        public const string SELLER = "Seller";
        public const string BUYER = "Buyer";
        public const string SHIPPER = "Shipper";
    }

    public static class ORDER_STATUS
    {
        public const string PENDING = "pending";
        public const string PROCESSING = "processing";
        public const string SHIPPED = "shipped";
        public const string DELIVERED = "delivered";
        public const string CANCELLED = "cancelled";
    }

    public static class TRANSFER_STATUS
    {
        public const string COMING = "COMING";
        public const string ARRIVED = "ARRIVED";
        public const string GONE = "GONE";
    }

    public static class RETURN_STATUS
    {
        public const string WAITING = "WAITING";
        public const string APPROVED = "APPROVED";
        public const string REJECT = "REJECT";
    }
}
