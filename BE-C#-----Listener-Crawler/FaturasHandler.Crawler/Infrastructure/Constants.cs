namespace FaturasHandler.Crawler.Infrastructure
{
    public static class Constants
    {
        public static readonly string ISSPassword = GetISSPassword();

        private static string GetISSPassword()
        {
            return "1q@W3e4r5t6y7u";
        }

        public static readonly string ATPassword = GetATPassword();

        private static string GetATPassword()
        {
            return "jwN6r9xv7GM3V@tZ";
        }

        public static readonly string AmazonPassword = GetAmazonPassword();

        private static string GetAmazonPassword()
        {
            return "wNUfg#1Xkq%LZ3mHm8!R";
        }

        //public static readonly string NIF = GetNIF();

        //private static string GetNIF()
        //{
        //    return "302372180";
        //}

        public static readonly string BwinPassword = GetBWINPassword();

        private static string GetBWINPassword()
        {
            return "RZgKLDHqnWnhf4BVtzS9";
        }
    }
}
