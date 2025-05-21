using E_commerce_23TH0024.Data;

namespace E_commerce_23TH0024.Service
{
    public class BaseService
    {
        protected readonly ApplicationDbContext _context;

        public BaseService(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
