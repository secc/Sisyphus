using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Sisyphus.ViewModels;

namespace Sisyphus.Utilities
{
    public static class RockAuthentication
    {
        public static List<Claim> AuthenticateUser( UserLogin userLogin )
        {
            var baseUrl = "https://rock.secc.org";
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            var client = new HttpClient();

            var url = baseUrl + "/api/Auth/Login";
            var dict = new Dictionary<string, string> {
                { "Username", userLogin.Username },
                { "Password", userLogin.Password} };
            var response = client.PostAsync( url, new FormUrlEncodedContent( dict ) );
            response.Wait();

            IEnumerable<string> headers;
            if ( !response.Result.Headers.TryGetValues( "Set-Cookie", out headers ) )
            {
                return null;
            }

            var cookieContainer = new CookieContainer();
            using ( var handler = new HttpClientHandler() { CookieContainer = cookieContainer } )
            using ( var cookieClient = new HttpClient( handler ) { BaseAddress = new Uri( baseUrl ) } )
            {
                foreach ( var cookie in headers )
                {
                    var a = cookie.Split( "=" );
                    var b = a[1].Split( ";" );
                    cookieContainer.Add( new Uri( baseUrl ), new Cookie( a[0], b[0] ) );
                }

                //Check Admin
                response = cookieClient.PostAsync( baseUrl + "/api/Lava/RenderTemplate", new StringContent( "{{ CurrentPerson  | Group: '2', 'All' }}" ) );
                response.Wait();
                var result = response.Result;
                if ( result.IsSuccessStatusCode )
                {
                    var content = result.Content.ReadAsStringAsync();
                    content.Wait();
                    if ( !string.IsNullOrWhiteSpace( content.Result ) )
                    {
                        return new List<Claim>{
                            new Claim("User", content.Result),
                            new Claim("Role", ClaimRoles.Administrator)
                        };
                    }

                }
            }

            return null;
        }
    }
}
