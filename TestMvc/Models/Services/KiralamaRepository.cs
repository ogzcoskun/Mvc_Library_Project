using System.Linq.Expressions;
using TestMvc.Utility;

namespace TestMvc.Models.Services
{
    public class KiralamaRepository : Repository<Kiralama>, IKiralamaRepository
    {

        private UygulamaDbContext _uygulamaDbContext;

        public KiralamaRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Kiralama kiralama)
        {
            _uygulamaDbContext.Update(kiralama);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
