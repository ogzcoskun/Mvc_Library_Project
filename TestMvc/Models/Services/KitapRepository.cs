using System.Linq.Expressions;
using TestMvc.Utility;

namespace TestMvc.Models.Services
{
    public class KitapRepository : Repository<Kitap>, IKitapRepository
    {

        private UygulamaDbContext _uygulamaDbContext;

        public KitapRepository(UygulamaDbContext uygulamaDbContext) : base(uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
        }

        public void Guncelle(Kitap kitap)
        {
            _uygulamaDbContext.Update(kitap);
        }

        public void Kaydet()
        {
            _uygulamaDbContext.SaveChanges();
        }
    }
}
