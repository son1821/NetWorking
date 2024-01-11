using System.Net;
using System.Text;

namespace NetWorking2
{
    public class DenyAccessFacebook : DelegatingHandler
    {
        public DenyAccessFacebook(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {

            var host = request.RequestUri.Host.ToLower();
            Console.WriteLine($"Check in DenyAccessFacebook - {host}");
            if (host.Contains("facebook.com"))
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("Không được truy cập"));
                return await Task.FromResult<HttpResponseMessage>(response);
            }
            // Chuyển truy vấn cho base (thi hành InnerHandler)
            return await base.SendAsync(request, cancellationToken);
        }
    }
    public class ChangeUri : DelegatingHandler
    {
        public class DenyAccessFacebook : DelegatingHandler
        {
            public DenyAccessFacebook(HttpMessageHandler innerHandler) : base(innerHandler) { }

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                         CancellationToken cancellationToken)
            {

                var host = request.RequestUri.Host.ToLower();
                Console.WriteLine($"Check in DenyAccessFacebook - {host}");
                if (host.Contains("facebook.com"))
                {
                    var response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Content = new ByteArrayContent(Encoding.UTF8.GetBytes("Không được truy cập"));
                    return await Task.FromResult<HttpResponseMessage>(response);
                }
                // Chuyển truy vấn cho base (thi hành InnerHandler)
                return await base.SendAsync(request, cancellationToken);
            }
        }
        public ChangeUri(HttpMessageHandler innerHandler) : base(innerHandler) { }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                               CancellationToken cancellationToken)
        {
            var host = request.RequestUri.Host.ToLower();
            Console.WriteLine($"Check in  ChangeUri - {host}");
            if (host.Contains("google.com"))
            {
                // Đổi địa chỉ truy cập từ google.com sang github
                request.RequestUri = new Uri("https://github.com/");
            }
            // Chuyển truy vấn cho base (thi hành InnerHandler)
            return base.SendAsync(request, cancellationToken);
        }
    }
    public class MyHttpClientHandler : HttpClientHandler
    {
        public MyHttpClientHandler(CookieContainer cookie_container)
        {

            CookieContainer = cookie_container;     // Thay thế CookieContainer mặc định
            AllowAutoRedirect = false;                // không cho tự động Redirect
            AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            UseCookies = true;
        }
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                                     CancellationToken cancellationToken)
        {
            Console.WriteLine("Bất đầu kết nối " + request.RequestUri.ToString());
            // Thực hiện truy vấn đến Server
            var response = await base.SendAsync(request, cancellationToken);
            Console.WriteLine("Hoàn thành tải dữ liệu");
            return response;
        }
    }
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var url = "https://google.com.vn";
            // khoi tao cookie
            var cookie = new CookieContainer();
            //tao chuoi handler
            var bottomHandler = new MyHttpClientHandler(cookie);
            var changUriHandler = new ChangeUri(bottomHandler);
            var denyAccessFacebook = new DenyAccessFacebook(changUriHandler);
            ////tao handler
            //using var handler = new SocketsHttpHandler();
            ////thiet lap thuoc tinh handler
            //handler.AllowAutoRedirect = true;
            //handler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
            //handler.UseCookies = true;
            //handler.CookieContainer = cookie;

            using var httpClient = new HttpClient(denyAccessFacebook);
            using var httpRequestMessage = new HttpRequestMessage();
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.RequestUri = new Uri(url);
            httpRequestMessage.Headers.Add("User-Agent", "Mozilla/5.0");
            var parameter = new List<KeyValuePair<string,string>>();
            parameter.Add(new KeyValuePair<string, string>("key1","value1"));
            parameter.Add(new KeyValuePair<string, string>("key2", "value2"));
            httpRequestMessage.Content = new FormUrlEncodedContent(parameter);
            var response = await httpClient.SendAsync(httpRequestMessage);
            //lay cookie
            cookie.GetCookies(new Uri(url)).ToList().ForEach(c =>
            {
            Console.WriteLine($"{c.Name} : {c.Value} ");
            }
            );
            var html = await response.Content.ReadAsStringAsync();
            Console.WriteLine(html);

        }
    }
}