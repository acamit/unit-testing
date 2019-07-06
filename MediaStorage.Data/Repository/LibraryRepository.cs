using MediaStorage.Data.Read;
using MediaStorage.Data.Write;

namespace MediaStorage.Data.Repository
{
    public class LibraryRepository
    {
        public LibraryReadRepository LibraryReadRepository = new LibraryReadRepository();
        public LibraryWriteRepository LibraryWriteRepository = new LibraryWriteRepository();
    }
}
