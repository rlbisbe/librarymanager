using LibraryManager.Repository;
using Microsoft.AspNet.Mvc;

namespace LibraryManager.Controllers
{
    [Authorize]
    public class BooksController : Controller
    {
        private IBookRepository _bookRepository;

        public BooksController(IBookRepository repository)
        {
            _bookRepository = repository;
        }

        // GET: /Book
        public IActionResult Index()
        {
            var model = _bookRepository.ListAll();
            return View(model);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Book/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Book book)
        {
            try
            {
                _bookRepository.Add(book);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
