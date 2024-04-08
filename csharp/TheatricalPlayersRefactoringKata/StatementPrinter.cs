using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");
            

            foreach(var perf in invoice.Performances) 
            {
                var play = plays[perf.PlayID];
                var thisAmount = 0;
                switch (play.Type) 
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if (perf.Audience > 30) {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (perf.Audience > 20) {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }
        
        public string PrintHTML(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = "";
            result += "<html>\n";
            result += "\t<body>\n";
            result += string.Format("\t\t<h1>Statement for {0}</h1>\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");
            result += "\t\t<table>\n";
            result += "\t\t\t\t<tr><th>play</th><th>seats</th><th>cost</th></tr>\n";

            CalcPrice(invoice, plays, ref totalAmount, ref volumeCredits, ref result, cultureInfo);
            result += "\t\t</table>\n";
            result += String.Format(cultureInfo, "\t\t<p>Amount owed is <em>{0:C}</em></p>\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("\t\t<p>You earned <em>{0}</em> credits</p>\n", volumeCredits);
            result += "\t</body>\n";
            result += "</html>\n";
            
            return result.Replace("\t", "  ");;
        }

        private static void CalcPrice(Invoice invoice, Dictionary<string, Play> plays, ref int totalAmount, ref int volumeCredits, ref string result, CultureInfo cultureInfo)
        {
            foreach (var perf in invoice.Performances)
            {
                var play = plays[perf.PlayID];
                result += $"\t\t\t\t<tr><td>{play.Name}</td><td>{perf.Audience}</td><td>";

                var thisAmount = 0;
                switch (play.Type)
                {
                    case "tragedy":
                        thisAmount = 40000;
                        if (perf.Audience > 30)
                        {
                            thisAmount += 1000 * (perf.Audience - 30);
                        }
                        break;
                    case "comedy":
                        thisAmount = 30000;
                        if (perf.Audience > 20)
                        {
                            thisAmount += 10000 + 500 * (perf.Audience - 20);
                        }
                        thisAmount += 300 * perf.Audience;
                        break;
                    default:
                        throw new Exception("unknown type: " + play.Type);
                }
                // add volume credits
                volumeCredits += Math.Max(perf.Audience - 30, 0);
                // add extra credit for every ten comedy attendees
                if ("comedy" == play.Type) volumeCredits += (int)Math.Floor((decimal)perf.Audience / 5);

                // print line for this order
                //result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", play.Name, Convert.ToDecimal(thisAmount / 100), perf.Audience);
                totalAmount += thisAmount;
                result += $"${totalAmount}</td></tr>\n";
            }
        }
    }
}
