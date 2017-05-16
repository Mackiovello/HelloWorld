using System;
using System.Collections.Generic;
using System.Linq;
using Starcounter;

namespace HelloWorld
{
    partial class PersonJson : Json, IBound<Spender>
    {
        static PersonJson()
        {
            //DefaultTemplate.Expenses.ElementType.InstanceType = typeof(ExpenseJson);
        }

        public string FullName => FirstName + " " + LastName;

        void Handle(Input.SaveTrigger action)
        {
            Transaction.Commit();
        }

        void Handle(Input.CancelTrigger action)
        {
            Transaction.Rollback();
        }

        void Handle(Input.NewExpenseTrigger action)
        {
            new Expense()
            {
                Spender = this.Data as Spender,
                Amount = 1
            };

            this.PopulateExpenses();
        }

        void Handle(Input.DeleteAllTrigger action)
        {
            this.ExpensePartials.Clear();
            Db.SlowSQL("DELETE FROM Expense WHERE Spender = ?", this.Data);
            //this.Expenses.Clear();
        }

        protected void PopulateExpenses()
        {
            IEnumerable<Json> jsons = this.Data.Expenses.Select(x => Self.GET<Json>("/HelloWorld/partial/expense/" + x.GetObjectID()));

            this.ExpensePartials.Clear();
            jsons.ToList().ForEach(x => this.ExpensePartials.Add(x));
        }

        //public IEnumerable<Json> ExpensePartials
        //{
        //    get
        //    {
        //        return this.Data.Expenses.Select(x => Self.GET<Json>("/HelloWorld/partial/expense/" + x.GetObjectID()));
        //    }
        //}
    }
}