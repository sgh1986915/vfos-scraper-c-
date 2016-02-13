using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace VFPO
{
    public static class Utilities
    {
        public static string TryGetCurrencySymbol(string ISOCurrencySymbol)
        {
            string symbol = string.Empty;
            symbol = CultureInfo
                .GetCultures(CultureTypes.AllCultures)
                .Where(c => !c.IsNeutralCulture)
                .Select(culture =>
                {
                    try
                    {
                        return new RegionInfo(culture.LCID);
                    }
                    catch
                    {
                        return null;
                    }
                })
                .Where(ri => ri != null && ri.ISOCurrencySymbol == ISOCurrencySymbol)
                .Select(ri => ri.CurrencySymbol)
                .FirstOrDefault();
            return symbol;
        }
    }
}
