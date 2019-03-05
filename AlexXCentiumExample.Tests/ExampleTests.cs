using NUnit.Framework;
using System.Net.Http;
using System.Threading.Tasks;

namespace AlexXCentiumExample.Tests
{
    public class ExampleTests
    {
        /** 
         * ValidUrl: Tests to see if the application works with a valid URL
         **/
        [Test]
        public async Task ValidUrl()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:65401/home/analysis?url=http://www.ediblesbyjack.com");
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                if(!string.IsNullOrEmpty(res) && !res.Contains("<strong>Error Message:</strong>"))
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        /** 
         * ValidUrl: Tests to see if the application fails with an invalid URL
         **/
        [Test]
        public async Task NotValidUrl()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:65401/home/analysis?url=http://www.thisurlshouldnotwork.com");
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(res) && !res.Contains("<strong>Error Message:</strong>"))
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

        /** 
         * ValidUrl: Tests to see if the application fails with misformatted url (no http)
         **/
        [Test]
        public async Task UrlFormatError()
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync("http://localhost:65401/home/analysis?url=www.ediblesbyjack.com");
            if (response.IsSuccessStatusCode)
            {
                string res = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(res) && !res.Contains("<strong>Error Message:</strong>"))
                {
                    Assert.Pass();
                }
            }
            Assert.Fail();
        }

    }
}