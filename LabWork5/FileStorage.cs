using System.Text.Json;
using System.Text.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RESTStorage
{
    class Storage
    {
        static void Main()
        {
            var HttpServer = new Server();
            Task.Run(() => HttpServer.Launch());
            Console.ReadLine();
        }
    }
    public class Server
    {

        public void Launch()
        {
            var Listener = new HttpListener();

            try
            {
                Listener.Prefixes.Add("http://localhost:8000/");

                Listener.Start();

                Console.WriteLine("Ready to go");

                while (true)
                {
                    HttpListenerContext HTTPContext = Listener.GetContext();

                    var NewRequest = HTTPContext.Request.HttpMethod;

                    Console.WriteLine($"\n{NewRequest}");

                    HttpListenerResponse HTTPResponse = HTTPContext.Response;
                    try
                    {
                        switch (NewRequest)
                        {
                            case "GET":
                                {
                                    GET(HTTPContext.Request, HTTPResponse);
                                    break;
                                }
                            case "PUT":
                                {
                                    PUT(HTTPContext.Request, HTTPResponse);
                                    break;
                                }
                            case "HEAD":
                                {
                                    HEAD(HTTPContext.Request, HTTPResponse);
                                    break;
                                }
                            case "DELETE":
                                {
                                    DELETE(HTTPContext.Request, HTTPResponse);
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Unknown command");
                                    break;
                                }
                        }
                    }
                    catch
                    {
                        HTTPResponse.StatusCode = 404;
                        Console.WriteLine($"Error: {HTTPResponse.StatusCode} Not Found");
                    }
                }
            }
            finally
            {
                Listener.Stop();
            }
        }


        public void GET(HttpListenerRequest Request, HttpListenerResponse HTTPResp)
        {
            Stream outStream = HTTPResp.OutputStream;

            var StrWriter = new StreamWriter(outStream);
            string PathFull = Directory.GetCurrentDirectory() + Request.RawUrl;

            try
            {
                String getPath = Request.Url.LocalPath;

                var LocalPath = getPath.Substring(1);

                FileInfo FileToDownl = new FileInfo(LocalPath);

                if (FileToDownl.Exists) //rawurl
                {
                    HTTPResp.ContentType = "application/force-download";
                    HTTPResp.Headers.Add("Content-Transfer-Encoding", "binary");
                    HTTPResp.Headers.Add("Content-Disposition", $"attachment; filename={FileToDownl.Name}");

                    using (var outputt = HTTPResp.OutputStream)
                    {
                        HTTPResp.ContentLength64 = FileToDownl.Length;
                        var Buf = File.ReadAllBytes(LocalPath);
                        outputt.Write(Buf, 0, Buf.Length);
                    }

                    HTTPResp.StatusCode = 200;
                    Console.WriteLine($"Success: {HTTPResp.StatusCode} OK");
                    return; //copyto
                }
                

                if (File.Exists(PathFull))
                {
                    try
                    {
                        using (var file = File.Open(PathFull, FileMode.Open))
                        {
                            file.CopyTo(outStream);
                            file.Close();
                        }

                        HTTPResp.StatusCode = 200;
                        Console.WriteLine($"Success: {HTTPResp.StatusCode} OK");
                    }
                    catch
                    {
                        HTTPResp.StatusCode = 500;
                        Console.WriteLine($"Error: {HTTPResp.StatusCode} Internal Server Error");
                    }

                }

                if (!File.Exists(PathFull))
                {
                    try
                    {
                        var Res = new List<object>();

                        foreach (var entry in Directory.GetDirectories(PathFull).Concat(Directory.GetFiles(PathFull)))
                        {
                            Res.Add(new
                            {
                                Name = entry.Substring(Directory.GetCurrentDirectory().Length),
                                CreationTime = Directory.GetCreationTime(entry).GetDateTimeFormats('R')[0],
                                IsDir = !File.Exists(entry)
                            }); ; 
                        }

                        StrWriter.Write(JsonSerializer.Serialize(Res));
                        StrWriter.Flush();

                        HTTPResp.StatusCode = 200;
                        Console.WriteLine($"Success: {HTTPResp.StatusCode} OK");
                    }
                    catch
                    {
                        HTTPResp.StatusCode = 404;
                        Console.WriteLine($"Error: {HTTPResp.StatusCode} Not Found");
                    }
                }


            }
            catch
            {
                HTTPResp.StatusCode = 404;
                Console.WriteLine($"Error: {HTTPResp.StatusCode} Not Found");
            }
            finally
            {
                HTTPResp.OutputStream.Close();
                StrWriter.Dispose();
            }
        }

        public void PUT(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                String getPath = request.Url.LocalPath;
                var localPath = getPath.Substring(1);
                var index = localPath.LastIndexOf("/", StringComparison.Ordinal);
                var dirpath = localPath.Substring(0, index);

                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }
                using (var input = request.InputStream)
                {
                    FileStream fileStream = File.Create(localPath);
                    input.CopyTo(fileStream);
                    fileStream.Close();
                }

                response.StatusCode = 200;
                Console.WriteLine($"Success: {response.StatusCode} OK");

            }
            finally
            {
                response.OutputStream.Close();
            }


        }

        public void HEAD(HttpListenerRequest request, HttpListenerResponse HTTPResp)
        {
            try
            {
                string PathFull = Directory.GetCurrentDirectory() + request.RawUrl;
                if (Directory.Exists(PathFull))
                {
                    var info = new DirectoryInfo(PathFull);
                    HTTPResp.Headers.Add("Date", info.CreationTime.ToString());
                    HTTPResp.Headers.Add("Name", info.Name.ToString());
                    HTTPResp.Headers.Add("Directory", info.Root.ToString());
                    HTTPResp.Headers.Add("LastWriteTime", info.LastWriteTime.ToString());

                    HTTPResp.StatusCode = 200;
                    Console.WriteLine($"Success: {HTTPResp.StatusCode} OK");
                }
                else if (File.Exists(PathFull))
                {
                    FileInfo info = new FileInfo(PathFull);
                    HTTPResp.Headers.Add("Date", info.CreationTime.ToString());
                    HTTPResp.Headers.Add("Name", info.Name.ToString());
                    HTTPResp.Headers.Add("LastWriteTime", info.LastWriteTime.ToString());
                    HTTPResp.Headers.Add("Size", info.Length.ToString());

                    HTTPResp.StatusCode = 200;
                    Console.WriteLine($"Success: {HTTPResp.StatusCode} OK");
                }
                else
                {
                    HTTPResp.StatusCode = 404;
                    Console.WriteLine($"Error: {HTTPResp.StatusCode} Not Found");
                }

            }
            finally
            {
                HTTPResp.OutputStream.Close();
            }
        }

        public void DELETE(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                string name = Directory.GetCurrentDirectory() + "/";
                string fullPath = Directory.GetCurrentDirectory() + request.RawUrl;
                if (Directory.Exists(fullPath))
                {
                    Directory.Delete(fullPath, true);

                    response.StatusCode = 200;
                    Console.WriteLine($"Success: {response.StatusCode} OK");
                }
                else if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);

                    response.StatusCode = 200;
                    Console.WriteLine($"Success: {response.StatusCode} OK");
                }
                else
                {
                    response.StatusCode = 404;
                    Console.WriteLine($"Error: {response.StatusCode} Not Found");
                }
            }
            finally
            {
                response.OutputStream.Close();
            }
        }
    }
}
