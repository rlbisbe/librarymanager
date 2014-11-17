using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Linq;

namespace LibraryManager.Repository
{
    public class UserRepository : DocumentDBRepository, IUserRepository
    {
        public UserRepository() : base("LibraryManager", "UserCollection") { }

        public async void CreateIfNotExists(LibraryUser currentUser)
        {
            var userExists = Client.CreateDocumentQuery<LibraryUser>(Collection.DocumentsLink).Where(x => x.Email == currentUser.Email).AsEnumerable().Any();
            if (userExists)
            {
                return;
            }

            await Client.CreateDocumentAsync(Collection.DocumentsLink, currentUser);
        }
    }
}