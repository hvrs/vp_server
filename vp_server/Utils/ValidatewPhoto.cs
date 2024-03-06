using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace vp_server.Utils
{
    public class ValidatewPhoto
    {
        public bool IsImage(byte[] data )
        {

            using (var imageReadStream = new MemoryStream(data))
            {
                try
                {
                    using (var possibleImage = Image.FromStream(imageReadStream));
                    return true;
                }
                catch 
                {
                    return false;
                }

            }
        }

    }
}
