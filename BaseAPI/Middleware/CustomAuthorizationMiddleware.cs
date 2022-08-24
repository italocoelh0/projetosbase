using BalcaoUnicoBusiness.DTO;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WS.Construtivo_GAT.API.Middlewares
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _cfg;

        public CustomAuthorizationMiddleware(RequestDelegate next, IConfiguration cfg)
        {
            _next = next;
            _cfg = cfg;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            string authHeader = httpContext.Request.Headers["Authorization"];
            string responseJson = "";
            bool error = false;

            if (authHeader != null)
            {
                authHeader = authHeader.Contains("Bearer") ? authHeader.Replace("Bearer", "").Trim() : authHeader.Trim();

                if (authHeader != _cfg["Authorization:Token"])
                {
                    responseJson = JsonConvert.SerializeObject(new RetornoPadraoDTO { CodigoStatus = StatusCodes.Status401Unauthorized, Mensagem = "Chave de acesso inválida, favor verificá-la." });
                    error = true;
                }

            }
            else
            {
                responseJson = JsonConvert.SerializeObject(new RetornoPadraoDTO { CodigoStatus = StatusCodes.Status401Unauthorized, Mensagem = "Não foi possível autorizar a requisição, favor verificar a chave de acesso." });
                error = true;
            }

            if (error)
            {
                if (!httpContext.Response.HasStarted)
                {
                    httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    httpContext.Response.ContentType = "application/json";
                    await httpContext.Response.WriteAsync(responseJson);
                }
                else
                {
                    await httpContext.Response.WriteAsync(string.Empty);
                }
            }
            else
            {
                await _next.Invoke(httpContext);
            }

        }
    }
}
