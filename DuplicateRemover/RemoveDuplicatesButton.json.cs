using System;
using Simplified.Ring1;
using Starcounter;

namespace DuplicateRemover
{
    partial class RemoveDuplicatesButton : Json
    {
        void Handle(Input.RemoveDuplicatesTrigger action)
        {
            QueryResultRows<Something> items = Db.SQL<Something>("SELECT s FROM Simplified.Ring1.Something s");

            FindDuplicates(items);
        }

        private void FindDuplicates(QueryResultRows<Something> items)
        {
            throw new NotImplementedException();
        }
    }
}
