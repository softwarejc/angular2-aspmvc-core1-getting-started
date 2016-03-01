using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace _4_IdentityServer
{
    /// <summary>
    /// This class loads the certificate used to add the server signature to issued tokens
    /// </summary>
    class SigningCertificate
    {
        public static X509Certificate2 Load()
        {
            var assembly = typeof(SigningCertificate).Assembly;
            var certificateResource = typeof(SigningCertificate).Namespace + "." + "certificate.pfx";
            using (var stream = assembly.GetManifestResourceStream(certificateResource))
            {
                return new X509Certificate2(ReadStream(stream), password: "idsrv3test");
            }
        }

        private static byte[] ReadStream(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
