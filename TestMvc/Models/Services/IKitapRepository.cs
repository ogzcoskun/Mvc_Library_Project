namespace TestMvc.Models.Services
{
    public interface IKitapRepository : IRepository<Kitap>
    {
        void Guncelle(Kitap kitap);
        void Kaydet();
    }
}
