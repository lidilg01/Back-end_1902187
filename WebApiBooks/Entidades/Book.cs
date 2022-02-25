using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiBooks.Entidades
{
    public class Book
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo Title es requerido")]
        [StringLength(maximumLength:12, ErrorMessage = "El campo {0} solo puede tener 12 caracteres")]
        public string Title { get; set; }

        [Range (20,1000, ErrorMessage ="El campo de paginas no esta dentro del rango")]
        [NotMapped]
        public int Pages { get; set; }

        [CreditCard]
        [NotMapped]
        public string CredCard { get; set; }

        [Url]
        [NotMapped]
        public string Url   { get; set; }
        public List<Genre> Genre { get; set;}
    }
}
