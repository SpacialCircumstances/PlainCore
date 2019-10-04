using System;

namespace PlainCore.Content
{
    public class AssetLoadException: Exception
    {
        public AssetLoadException(string message) : base(message)
        {
        }

        public AssetLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public AssetLoadException()
        {
        }
    }
}
