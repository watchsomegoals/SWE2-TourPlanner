using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.BusinessLayer
{
    public static class TourItemFactory
    {
        private static ITourItemFactory instance;

        public static ITourItemFactory GetInstance()
        {
            if(instance == null)
            {
                instance = new TourItemFactoryImpl();
            }
            return instance;
        }
    }
}
