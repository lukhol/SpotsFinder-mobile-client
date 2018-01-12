using NUnit.Framework;
using SpotFinder.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotFinder.Tests.Services
{
    [TestFixture]
    public class GoogleUserProviderTests
    {
        private GoogleUserProvider googleUserProvider;

        public GoogleUserProviderTests()
        {

        }

        [SetUp]
        public void SetUp()
        {
            //googleUserProvider = new GoogleUserProvider();
        }

        [Test]
        public void Test()
        {
            var uri = "https://www.google.pl/?code=4%2FnL8OT1ZZ4p2AASaCBXrkXWiFtDp9x0o9c_DXejGQOCg&authuser=0&session_state=5bd39b66055d2bbc489afdbe638ed53d81a5cc9d..931b&prompt=consent&gws_rd=ssl";
            ExtractCodeFromGoogleReturnUrl(uri);
        }

        private string ExtractCodeFromGoogleReturnUrl(string url)
        {
            var codePhrase = "?code=";

            if (!url.Contains(codePhrase))
                throw new ArgumentException("Url is not valid. Google url should contains '?code='.");

            var urlWithoutFirstPart = url.Remove(0, url.IndexOf(codePhrase) + codePhrase.Length);
            var code = urlWithoutFirstPart.Substring(0, urlWithoutFirstPart.IndexOf("&"));

            return code;
        }
    }
}
