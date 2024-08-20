using System.Linq.Expressions;
using TestMvc.Utility;

namespace TestMvc.Models.Services
{
    public class KitapTuruRepository : Repository<KitapTuru>, IKitapTuruRepository
    {

        private UygulamaDbContext _uygulamaDbContext;

        public KitapTuruRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(KitapTuru kitapTuru)
        {
            _uygulamaDbContext.Update(kitapTuru);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
