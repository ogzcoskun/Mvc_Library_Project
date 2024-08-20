using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TestMvc.Utility;

namespace TestMvc.Models.Services
{
    public class Repository<T> : IRepository<T> where T : class
    {

        private readonly UygulamaDbContext _uygulamaDbContext;
        internal DbSet<T> dbSet;


        public Repository(UygulamaDbContext uygulamaDbContext)
        {
            _uygulamaDbContext = uygulamaDbContext;
            this.dbSet = _uygulamaDbContext.Set<T>();
            _uygulamaDbContext.Kitaplar.Include(k => k.KitapTuru).Include(k => k.KitapTuruId);
        }

        public void Ekle(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filtre, string? includeProps = null)
        {
            IQueryable<T> sorgu = dbSet;
            sorgu= sorgu.Where(filtre);

            if (includeProps != null)
            {
                foreach (var prop in includeProps.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    sorgu = sorgu.Include(prop);
                }
            }

            return sorgu.FirstOrDefault();
        }

        public IEnumerable<T> GetAll(string? includeProps = null )
        {
            IQueryable<T> sorgu = dbSet;

            if(includeProps != null)
            {
                foreach(var prop in includeProps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    sorgu = sorgu.Include(prop);
                }
            }

            return sorgu;
        }

        public void Sil(T entity)
        {
            dbSet.Remove(entity);
        }

        public void SilAralık(IEnumerable<T> entities)
        {
            dbSet.RemoveRange(entities);
        }
    }
}
