using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Blog.Core
{
    public class PreFlightHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Headers.Contains("Origin"))
            {
                var origin = request.Headers.GetValues("Origin").ToList()[0];

                if (request.Method.Method.Equals("OPTIONS"))
                {
                    var response = new HttpResponseMessage { StatusCode = HttpStatusCode.OK };

                    response.Headers.Add("Access-Control-Allow-Origin", origin);
                    response.Headers.Add("Access-Control-Allow-Headers", "origin, content-type, accept, accessToken");
                    response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
                    response.Headers.Add("Access-Control-Expose-Headers", "accessToken");
                    response.Headers.Add("Access-Control-Max-Age", "600");
                    

                    var tsc = new TaskCompletionSource<HttpResponseMessage>();
                    tsc.SetResult(response);
                    return tsc.Task;
                }
                else
                    return base.SendAsync(request, cancellationToken).ContinueWith(x =>
                    {
                        x.Result.Headers.Add("Access-Control-Allow-Origin", origin);
                        x.Result.Headers.Add("Access-Control-Allow-Headers", "origin, content-type, accept, accessToken");
                        x.Result.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE");
                        x.Result.Headers.Add("Access-Control-Allow-Credentials", "true");
                        x.Result.Headers.Add("Access-Control-Expose-Headers", "accessToken");
                        x.Result.Headers.Add("Access-Control-Max-Age", "600");

                        return x.Result;
                    });
            }
            else
                return base.SendAsync(request, cancellationToken);
        }
    }
}