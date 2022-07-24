using Microsoft.AspNetCore.Mvc.Razor;

namespace ECC.WebApp.MVC.Extensions
{
    public static class RazorHelpers
    {
        public static string StockMsg(this RazorPage page, int qtd)
        {
            return qtd > 0 ? $"Only{qtd} in sock!" : "Out of stock";
        }

        public static string CurrencyFormatter(this RazorPage page, decimal price)
        {
            return price > 0 ? string.Format(Thread.CurrentThread.CurrentCulture, "{0:C}", price) : "Free";
        }
    }
}
