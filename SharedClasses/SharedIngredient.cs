using System;

namespace SharedClasses
{
    public class SharedIngredient
    {
        public Guid IngredientId { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
    }
}