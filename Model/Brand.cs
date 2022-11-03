using alcolikLib.Model;

namespace alcolik.Model
{
    public class Brand : BaseModel
    {
        public string Slogan { get; set; }

        public IEnumerable<Alcool> Alcools { get; set; }
    }
}
