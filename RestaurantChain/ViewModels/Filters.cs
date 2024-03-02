using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RestaurantChain.Models;
namespace RestaurantChain.ViewModels
{
    public class Filters
    {
        public IEnumerable<Menu> Items { get; set; }
        public IEnumerable<Menu> AllItems { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public IEnumerable<Cart> Carts { get; set; }

        public IEnumerable<Hotel> Litems { get; set; }
        public IEnumerable<Hotel> Hotels { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Location> Locations { get; set; }
    }
}