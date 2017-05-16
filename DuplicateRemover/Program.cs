using System;
using Starcounter;

namespace DuplicateRemover
{
    class Program
    {
        static void Main()
        {
            Application.Current.Use(new HtmlFromJsonProvider());
            Application.Current.Use(new PartialToStandaloneHtmlProvider());

            Handle.GET("/DuplicateRemover/partial/RemoveDuplicatesButton", () =>
            {
                return new RemoveDuplicatesButton();
            });

            Blender.MapUri("/DuplicateRemover/partial/RemoveDuplicatesButton", "controls");
        }
    }
}