using System;

namespace LibraryManager.Repository
{
    public interface IUserRepository
    {
        void CreateIfNotExists(LibraryUser currentUser);
    }
}