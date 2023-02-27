using ProductsClient.Classes;
using System;

namespace ProductsClient.Model
{
    public class ArrivalProduct : BaseVM
    {
        private float _count;

        public ArrivalProduct()
        {
            Note = new NoteProduct();
            Product = new Product();
        }
        public ArrivalProduct(ArrivalProduct arrivalProduct)
        {
            Id = arrivalProduct.Id;
            Note = arrivalProduct.Note;
            Product = arrivalProduct.Product;
            Count = arrivalProduct.Count;
            PurchaseCost = arrivalProduct.PurchaseCost;
            SellCost = arrivalProduct.SellCost;
            ValidUntil = arrivalProduct.ValidUntil;
        }

        public uint Id { get; set; }
        public NoteProduct Note { get; set; }
        public Product Product { get; set; }
        public float Count
        {
            get => _count;
            set { _count = value; OnPropertyChanged(); }
        }
        public DateTime ValidUntil { get; set; } = DateTime.Now;
        public float PurchaseCost { get; set; }
        public float SellCost { get; set; }

        public override string ToString()
        {
            return $"{Id} {Note.Id} {Product.Name} {Product.IdName} {Count} {Product.Unit.Name} {ValidUntil} {PurchaseCost}";
        }
    }
}
