
namespace Lab19Exercitii.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Age { get; set; }
        public int? AddressID { get; set; }
        public Address Address { get; set; }
    }
}
