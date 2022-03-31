using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationModelPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class CreationModel : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Wall> walls = CreateUtils.CreateWalls(commandData);
            Level level1 = LevelsUtils.GetLevel1(commandData);
            Level level2 = LevelsUtils.GetLevel2(commandData);
            Transaction tr = new Transaction(doc, "Добавление двери, окон и крыши");
            tr.Start();

            CreateUtils.AddDoor(doc, level1, walls[0]);
            CreateUtils.AddWindow(doc, level1, walls[1]);
            CreateUtils.AddWindow(doc, level1, walls[2]);
            CreateUtils.AddWindow(doc, level1, walls[3]);
            CreateUtils.AddRoof(doc, level2, walls);

            tr.Commit();


            return Result.Succeeded;
        }
        
        
        

    }
}
