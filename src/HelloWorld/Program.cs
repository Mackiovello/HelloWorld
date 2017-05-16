using System.Linq;
using System.Collections.Generic;
using Starcounter;
using Simplified.Ring1;
using Simplified.Ring2;

namespace HelloWorld
{
    [Database]
    public class Spender : Person
    {
        public QueryResultRows<Expense> Expenses => Db.SQL<Expense>("SELECT e FROM Expense e WHERE e.Spender = ?", this);
        public decimal CurrentBalance => Db.SQL<decimal>("SELECT SUM(e.Amount) FROM Expense e WHERE e.Spender = ?", this).First;
    }

    [Database]
    public class Expense : Something
    {
        public Spender Spender;
        public decimal Amount;
    }

    class Program
    {
        static void Main()
        {
            Db.Transact(() =>
            {
                var anyone = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                if (anyone == null)
                { 
                    new Spender()
                    {
                        FirstName = "John",
                        LastName = "Doe"
                    };
                }
            });

            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/HelloWorld", () =>
            {
                return Db.Scope(() =>
                {
                    var person = Db.SQL<Spender>("SELECT s FROM Spender s").First;
                    var json = new PersonJson()
                    {
                        Data = person
                    };

                    if (Session.Current == null)
                    {
                        Session.Current = new Session(SessionOptions.PatchVersioning);
                    }
                    json.Session = Session.Current;

                    return json;
                });
            });

            Handle.GET("/HelloWorld/partial/expense/{?}", (string id) => new ExpenseJson { Data = DbHelper.FromID(DbHelper.Base64DecodeObjectID(id)) });
            Blender.MapUri("/HelloWorld/partial/expense/{?}", "expense");
        }
    }
}