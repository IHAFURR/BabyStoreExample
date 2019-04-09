using System;
using System.ComponentModel.DataAnnotations;
using BabyStore.DAL;
using System.Linq;
using System.Web;
using System.Collections.Generic;

namespace BabyStore.Models
{
    public class BasketLine
    {
        public int ID { get; set; }
        public string BasketID { get; set; }
        public int ProductId { get; set; }
        [Range(0, 50, ErrorMessage = "Please enter a quantity between 0 and 50" )]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Product Product { get; set; }
    }

    public class Basket
    {
        private string BasketID { get; set; }
        public const string BasketSessionKey = "BasketID";
        private StoreContext storeContext = new StoreContext();

        private string GetBasketID()
        {
            if (HttpContext.Current.Session[BasketSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[BasketSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempBasckedID = Guid.NewGuid();
                    HttpContext.Current.Session[BasketSessionKey] = tempBasckedID;
                }
            }
            return HttpContext.Current.Session[BasketSessionKey].ToString();
        }

        public static Basket GetBasket()
        {
            Basket basket = new Basket();
            basket.BasketID = basket.GetBasketID();
            return basket;
        }

        public void AddToBasket(int productID, int quantity)
        {
            var basketLine = storeContext.BasketLine.FirstOrDefault(b => b.BasketID == BasketID && b.ProductId == productID);

            if (basketLine == null)
            {
                basketLine = new BasketLine
                {
                    ProductId = productID,
                    BasketID = BasketID,
                    Quantity = quantity,
                    DateCreated = DateTime.Now
                };

                storeContext.BasketLine.Add(basketLine);
            }
            else
            {
                basketLine.Quantity += quantity;
            }
            storeContext.SaveChanges();
        }

        public void RemoveBasketLine(int productID)
        {
            var basketLine = storeContext.BasketLine.FirstOrDefault(b => b.BasketID == BasketID && b.ProductId == productID);
            if (basketLine != null)
            {
                storeContext.BasketLine.Remove(basketLine);
            }            

            storeContext.SaveChanges();
        }

        public void UpdateBasket(List<BasketLine> lines)
        {
            foreach (var line in lines)
            {
                var basketLine = storeContext.BasketLine.FirstOrDefault(b => b.BasketID == BasketID && b.ProductId == line.ProductId);
                if (basketLine != null)
                {
                    if (line.Quantity == 0)
                    {
                        RemoveBasketLine(line.ProductId);
                    }
                    else
                    {
                        basketLine.Quantity = line.Quantity;
                    }
                }
            }
            storeContext.SaveChanges();
        }

        public void EmptyBasket()
        {
            var basketLines = storeContext.BasketLine.Where(b => b.BasketID == BasketID);

            foreach (var line in basketLines)
            {
                storeContext.BasketLine.Remove(line);
            }
            storeContext.SaveChanges();
        }

        public List<BasketLine> GetBasketLines()
        {
            return storeContext.BasketLine.Where(b => b.BasketID == BasketID).ToList();
        }

        public decimal GetTotalCost()
        {
            decimal basketTotal = decimal.Zero;

            if (GetBasketLines().Count() > 0)
            {
                basketTotal = storeContext.BasketLine.Where(b => b.BasketID == BasketID).Sum(b => b.Product.Price * b.Quantity);
            }

            return basketTotal;
        }

        public int GetNumberOfItems()
        {
            int numberOfItems = 0;
            if (GetBasketLines().Count() > 0)
            {
                numberOfItems = storeContext.BasketLine.Where(b => b.BasketID == BasketID).Sum(b => b.Quantity);
            }
            return numberOfItems;
        }

        public void MigrateBasket(string userName)
        {
            // find the current basket and store it in the memory using ToList()
            var basket = storeContext.BasketLine.Where(b => b.BasketID == BasketID).ToList();

            // find if the user already has a basket or not and store it in the memory usning ToList()
            var userBasket = storeContext.BasketLine.Where(b => b.BasketID == userName).ToList();

            // if the user has a basket  then add  the current items to it
            if (userBasket != null)
            {
                // set the basketID to the username
                string prevID = BasketID;
                BasketID = userName;
                // add the lines  in anonymous basket to the user's basket
                foreach (var line in basket)
                {
                    AddToBasket(line.ProductId, line.Quantity);
                }
                // delete the lines  in anonymous basket from the database
                BasketID = prevID;
                EmptyBasket();
            }
            else
            {
                // if user does not have a basket then just migrate this one
                foreach (var line in basket)
                {
                    line.BasketID = userName;
                }
                storeContext.SaveChanges();
            }
            HttpContext.Current.Session[BasketSessionKey] = userName;
        }
    }    
}