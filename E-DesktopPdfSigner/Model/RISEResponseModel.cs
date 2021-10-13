using Newtonsoft.Json;

namespace DesktopPdfSigner
{
    
    public class RISEResponseModel
    {
        public static RISEResponseModel<T> Create<T>() where T : class
        {
            return new RISEResponseModel<T>();
        }
    }
}
