using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationModelPlugin
{
    class LevelsUtils
    {
        public static Level GetLevel1(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Level level1 = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .OfType<Level>()
            .Where(t => t.Name.Equals("Уровень 1"))
                .FirstOrDefault();

            return level1;

        }
        public static Level GetLevel2(ExternalCommandData commandData)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            Level level2 = new FilteredElementCollector(doc)
            .OfClass(typeof(Level))
            .OfType<Level>()
            .Where(t => t.Name.Equals("Уровень 2"))
                .FirstOrDefault();

            return level2;

        }







    }
}
