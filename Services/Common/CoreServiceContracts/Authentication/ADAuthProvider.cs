using ServiceStack;
using ServiceStack.Auth;
using System.DirectoryServices;


namespace CoreServiceContracts.Authentication
{
    public class ADAuthProvider : CredentialsAuthProvider
    {

        public const string C_UserName = "MGG-UserName";
        public const string C_MachineName = "MGG-MachineName";
        public const string C_App = "MGG-App";

        /// <summary>
        /// Authenticate the user using the Windows Identity
        /// </summary>
        /// <param name="authService"></param>
        /// <param name="session"></param>
        /// <param name="request"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public override async Task<object> AuthenticateAsync(IServiceBase authService, IAuthSession session, Authenticate request, CancellationToken token = default)
        {
            /// First, try to read the header (external call scenario)
            string? windowsIdentity = authService.Request.Headers[C_UserName];

            // If the header is not present (in-process scenario), check the Items dictionary.
            if (string.IsNullOrEmpty(windowsIdentity))
            {
                windowsIdentity = authService.Request.Items[C_UserName] as string;
            }

            if (string.IsNullOrEmpty(windowsIdentity))
                throw HttpError.Unauthorized("No valid Windows user detected.");

            if(!IsLDAPUser(windowsIdentity))
                throw HttpError.Unauthorized($"Not an authorized user '{windowsIdentity}' to interact with the service");

            session.IsAuthenticated = true;
            session.UserAuthName = windowsIdentity;
            await authService.SaveSessionAsync(session);

            return new AuthenticateResponse { UserName = windowsIdentity, SessionId = session.Id };
        }


        /// <summary>
        /// Validates with the LDAP server
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        private bool IsLDAPUser(string username)
        {
            try
            {
                string domainName = "mggcap";
                string ldapPath = $"LDAP://{domainName}";

                if (!username.Contains("\\"))
                    username = $"{domainName}\\{username}";

                string userAccount = username.Split('\\')[1];

#pragma warning disable CA1416 // Validate platform compatibility
                using (DirectoryEntry entry = new DirectoryEntry(ldapPath))
                {
                    using (DirectorySearcher searcher = new DirectorySearcher(entry))
                    {
                        searcher.Filter = $"(samAccountName={userAccount})";
                        searcher.PropertiesToLoad.Add("cn");

                        SearchResult? result = searcher.FindOne();

                        if (result != null)
                        {
                            return true;
                        }
                    }
                }
#pragma warning restore CA1416 // Validate platform compatibility
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LDAP Authentication failed: {ex.Message}");
            }
            return false;
        }

    }
}
