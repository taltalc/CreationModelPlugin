using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreationModelPlugin
{
    class CreateUtils
    {
        public static List<Wall> CreateWalls(ExternalCommandData commandData)

        {
            Document doc = commandData.Application.ActiveUIDocument.Document;

            List<Level> levels = LevelsUtils.GetLevels(commandData);
            
            Level level1 = levels
                .Where(t => t.Name.Equals("Уровень 1"))
                .FirstOrDefault();
           
            Level level2= levels
                .Where(t => t.Name.Equals("Уровень 2"))
                .FirstOrDefault();

            double width = UnitUtils.ConvertToInternalUnits(10000, UnitTypeId.Millimeters);
            double depth = UnitUtils.ConvertToInternalUnits(5000, UnitTypeId.Millimeters);

            double x = width / 2;
            double y = depth / 2;

            List<XYZ> points = new List<XYZ>();
            points.Add(new XYZ(-x, -y, 0));
            points.Add(new XYZ(-x, y, 0));
            points.Add(new XYZ(x, y, 0));
            points.Add(new XYZ(x, -y, 0));
            points.Add(new XYZ(-x, -y, 0));

            List<Wall> walls = new List<Wall>();

            Transaction tr = new Transaction(doc, "Создание стен");
            tr.Start();
            for (int i = 0; i < 4; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                Wall wall = Wall.Create(doc, line, level1.Id, false);
                walls.Add(wall);
                wall.get_Parameter(BuiltInParameter.WALL_HEIGHT_TYPE).Set(level2.Id);
            }


            tr.Commit();

            return walls;



        }
    }
}
