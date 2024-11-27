using System;
using System.Collections.Generic;

namespace atmos
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Checkout checkout = new Checkout();
            checkout.ScanItem("A99");
            checkout.ScanItem("B15");
            checkout.ScanItem("A99");
            checkout.ScanItem("B15");
            checkout.ScanItem("A99");
            checkout.ScanItem("C40");

            int total = checkout.CalculateTotal();
            Console.WriteLine("Total is: "+ total);

        }

        public class Checkout {

            private Dictionary<string, (int price, string offer)> pricingTable = new Dictionary<string, (int price, string offer)>
            {
                { "A99", (50, "3 for 130") },
                { "B15", (30, "2 for 45") },
                { "C40", (60, null) },
                { "T34", (99, null) }
            };

            private Dictionary<string, int> basket = new Dictionary<string, int>();
            public void ScanItem(string name) {
                
                if (!pricingTable.ContainsKey(name))
                {
                    Console.WriteLine("Item"+ name +"is not in table");
                    return;
                }


                if (basket.ContainsKey(name))
                {
                    basket[name] += 1;
                }
                else { 
                    basket.Add(name, 1);
                }
            }


            public int CalculateTotal() { 
                int total = 0;

                foreach (var item in basket) {
                    string sku = item.Key;
                    int quantity = item.Value;
                    int price = pricingTable[sku].price;

                    if (string.IsNullOrEmpty(pricingTable[sku].offer))
                    {
                        total += quantity * price;
                    }
                    else 
                    {  
                        string[] offer_words = pricingTable[sku].offer.Trim().Split(' ');

                        int offerQuantity = Convert.ToInt32(offer_words[0]);
                        int offerPrice = Convert.ToInt32(offer_words[2]);

                        int offerGroups = quantity / offerQuantity;
                        int remainingItems = quantity % offerQuantity;

                        total += (offerGroups * offerPrice) + (remainingItems * price);
                    }
                }

                return total;
            }
        }
    }
}
