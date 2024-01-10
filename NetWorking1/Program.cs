
using System.Net;
using System.Net.Http.Headers;
using System.Net.NetworkInformation;
using System.Text;

namespace NetWorking1
{
    class Program
    {
        static void ShowHeader(HttpResponseHeaders headers)
        {
            Console.WriteLine("Cac Header");
            foreach(var header in headers)
            {
                Console.WriteLine($"{header.Key} : {header.Value }");
            }
        }
        public static async Task<string>  GetWebContent(string url)
        {
            using var httpClient = new HttpClient();
            try
            {//Them header
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
                //httpResponseMessage.Headers;
                ShowHeader(httpResponseMessage.Headers);
                string html = await httpResponseMessage.Content.ReadAsStringAsync();
                return html;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "lỗi";
            }



        }//đọc content bằng ReadAsStringAsync()
        public static async Task<byte[]> DowloadDataBytes(string url)
        {
            using var httpClient = new HttpClient();
            try
            {//Them header
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(url);
                ShowHeader(httpResponseMessage.Headers);
                var bytes = await httpResponseMessage.Content.ReadAsByteArrayAsync();
                return bytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }



        }//đọc content bằng ReadAsByteAsync()
        public static async Task DowloadStream(string url, string filename )
        {
             var httpClient = new HttpClient();
            try
            {//Them header
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
                var httpResponseMessage = await httpClient.GetAsync(url);
                ShowHeader(httpResponseMessage.Headers);
                using var stream = await httpResponseMessage.Content.ReadAsStreamAsync();
                using var streamwrite = File.OpenWrite(filename);
                int SIZEBUFFER = 500;
                var buffer = new byte[SIZEBUFFER];
                bool endread = false;
                do
                {
                    int numBytes = await stream.ReadAsync(buffer, 0, SIZEBUFFER);
                    if (numBytes == 0)
                    {
                        endread = true;
                    }
                    else
                    {
                       await streamwrite.WriteAsync(buffer, 0, numBytes);
                    }
                } while (!endread);


                
                   
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
              
            }



        }//đọc content bằng ReadAsByteAsync()
        static async Task Main(string[] args)
        {
          //Tạo request với SendAsync
         using var htmlClinent = new HttpClient();

         var httpRequestMessage = new HttpRequestMessage();

        httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri("https://postman-echo.com/post");
            httpRequestMessage.Headers.Add("User-Agent","Mozilla/5.0");
            //string data = @"{
            //    ""key1"" : ""giaitri1"",
            //        ""key2"":""giaitri2"",
            //}";
            //Console.WriteLine(data);
            //httpRequestMessage.Content => Form HTML, File....
            //POST => FORM HTML 
            /*
             key1 => value1  [input]
            key2=> [value2-1, value 2-2]  [Multi Select]
           
             */
            //var parameters = new List<KeyValuePair<string, string>>();
            //parameters.Add(new KeyValuePair<string, string>("key1", "value1")) ;
            //parameters.Add(new KeyValuePair<string, string>("key2", "value2-1"));
            //parameters.Add(new KeyValuePair<string, string>("key2", "value2-1"));
            var content = new MultipartFormDataContent();
            //upload file 1.txt
            Stream fileStream = File.OpenRead("1.txt");
            var fileUpload = new StreamContent(fileStream);
            content.Add(fileUpload,"fileupload","abc.xyz");

            content.Add(new StringContent("value1"),"key1");
           

            httpRequestMessage.Content = content;
            var httpResponseMessage=await htmlClinent.SendAsync(httpRequestMessage);
            ShowHeader(httpResponseMessage.Headers);
            var html =await httpResponseMessage.Content.ReadAsStringAsync();
            Console. WriteLine(html);
                     
        }
    }
}
