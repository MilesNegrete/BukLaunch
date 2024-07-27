using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class GoogleAuthHelper
{
    static string[] Scopes = { Google.Apis.Books.v1.BooksService.Scope.Books };
    static string ApplicationName = "GoogleBooksAPIExample";

    public async Task<UserCredential> AuthenticateAsync()
    {
        UserCredential credential;

        using (var stream = new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
        {
            string credPath = "token.json";
            credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                GoogleClientSecrets.Load(stream).Secrets,
                Scopes,
                "user",
                CancellationToken.None,
                new FileDataStore(credPath, true));
        }

        return credential;
    }
}

