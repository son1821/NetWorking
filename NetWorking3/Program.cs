using Newtonsoft.Json;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

namespace NetWorking3
{

    class Program
   {
        class MyHttpServer
        {
            private HttpListener listener;
            public MyHttpServer(string[] prefixes)
            {
                if (!HttpListener.IsSupported)
                {
                    throw new Exception("HttpListener is not Support");
                }

                listener = new HttpListener();

                foreach (string prefix in prefixes)
                {
                    listener.Prefixes.Add(prefix);
                }
            }
            public async Task Starting()
            {
                listener.Start();
                Console.WriteLine("HTTP Started");
                do
                {

                    Console.WriteLine(DateTime.Now.ToLongTimeString() + " wating client");
                    var context = await listener.GetContextAsync();
                    await ProcessRequest(context);
                    Console.WriteLine("Client connected");

                } while (listener.IsListening);
            }
            //xu ly request va reponse
            async Task ProcessRequest(HttpListenerContext context)
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;
                Console.WriteLine($"{request.HttpMethod} {request.RawUrl} {request.Url.AbsolutePath}");
                var outputStream = response.OutputStream;
                switch (request.Url.AbsolutePath)
                {
                    case "/":
                        {

                            var buffer = Encoding.UTF8.GetBytes("Xin chao cac ban");
                            response.ContentLength64 = buffer.Length;
                            await outputStream.WriteAsync(buffer, 0, buffer.Length);
                            
                        }
                        break;
                    case "/json":
                        {
                            response.Headers.Add("Content-Type", "application/json");
                            var product = new
                            {
                                Name = "Macbook Pro",
                                Price = 3000
                            };
                            var json = JsonConvert.SerializeObject(product);
                            var buffer = Encoding.UTF8.GetBytes(json);
                            response.ContentLength64 = buffer.Length;
                            await outputStream.WriteAsync(buffer, 0, buffer.Length);
                        }
                        break;
                    case "/image2.png":
                        {
                            response.Headers.Add("Content-Type", "image/png");
                            var buffer = await File.ReadAllBytesAsync("image2.png");
                            response.ContentLength64 = buffer.Length;
                            await outputStream.WriteAsync(buffer,0,buffer.Length);


                        }
                        break;
                    default:
                        {
                            response.StatusCode = (int) HttpStatusCode.NotFound;
                            var buffer = Encoding.UTF8.GetBytes("Ban da nhap sai dia chi");
                            response.ContentLength64 = buffer.Length;
                            await outputStream.WriteAsync(buffer, 0, buffer.Length);
                        }
                        break;
                }

                outputStream.Close();
            }
        }

        static async Task Main(string[] args)
        {

            var server = new MyHttpServer(new string[] { "http://localhost:8080/" });
            await server.Starting();

            //kiem tra
            //if (HttpListener.IsSupported)
            //{
            //    Console.WriteLine("Support HttpListener");

            //}
            //else
            //{
            //    Console.WriteLine("Not Support HttpListner");
            //    throw new Exception("Not Support HttpListner");
            //}
            ////khoi tao http listener
            //var sever = new HttpListener();
            //sever.Prefixes.Add("http://localhost:8080/");

            //sever.Start();
            //Console.WriteLine("Sever HTTP Start");
            //do
            //{
            //    var context = await sever.GetContextAsync();
            //    Console.WriteLine("Client Connected ");

            //    var response = context.Response;

            //    var outputStream = response.OutputStream;
            //    response.Headers.Add("content-type", "text/html");
            //    var html = "<h1>Hello World</h1>";
            //    var bytes = Encoding.UTF8.GetBytes(html);
            //    await outputStream.WriteAsync(bytes, 0, bytes.Length);
            //    outputStream.Close();

            //} while (sever.IsListening);


        }
    }
}