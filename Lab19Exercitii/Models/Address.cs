﻿namespace Lab19Exercitii.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Number { get; set; }
        public List<Student> Students { get; set; }
    }
}
