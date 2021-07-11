using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSharpHttpListener
{
    public class Network
    {
        private HttpListener _httpListener;

        public async Task ConnectHttp()
        {
            try
            {
                _httpListener = new HttpListener();

                _httpListener.Prefixes.Add("http://localhost:8095/");
                _httpListener.Start();

                while (true)
                {
                    var context = await _httpListener.GetContextAsync();
                    ThreadPool.QueueUserWorkItem(async o =>
                    {
                        using var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding);
                        var command = await reader.ReadToEndAsync();

                        try
                        {
                            context.Response.StatusCode = 200;
                            var response = await Process(command);
                            var bytes = Encoding.UTF8.GetBytes(response);
                            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                            context.Response.ContentLength64 = bytes.Length;
                            await context.Response.OutputStream.WriteAsync(bytes, 0, bytes.Length);
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(e.Message);
                        }
                    });
                }
            }
            catch
            {
                DisconnectHttp();
                await ConnectHttp();
            }
        }

        public void DisconnectHttp()
        {
            _httpListener.Stop();
        }
        public Task<string> Process(string command)
        {
            MessageBox.Show(command);

            return Task.Factory.StartNew(() => "true");
        }
    }
}
