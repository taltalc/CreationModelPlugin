using Autodesk.Revit.ApplicationServices;
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



            Level level1 = LevelsUtils.GetLevel1(commandData);
            Level level2 = LevelsUtils.GetLevel2(commandData);

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


        public static FamilyInstance AddDoor(Document doc, Level level1, Wall wall)
        {


            FamilySymbol doorType = new FilteredElementCollector(doc)
             .OfClass(typeof(FamilySymbol))
             .OfCategory(BuiltInCategory.OST_Doors)
             .OfType<FamilySymbol>()
             .Where(t => t.Name.Equals("0915 x 2134 мм"))
             .Where(t => t.FamilyName.Equals("Одиночные-Щитовые"))
             .FirstOrDefault();

            LocationCurve hostCurve = wall.Location as LocationCurve;
            XYZ point1 = hostCurve.Curve.GetEndPoint(0);
            XYZ point2 = hostCurve.Curve.GetEndPoint(1);
            XYZ point = (point1 + point2) / 2;

            if (!doorType.IsActive)
                doorType.Activate();

            FamilyInstance door = doc.Create.NewFamilyInstance(point, doorType, wall, level1,
                Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

            return door;
        }



        public static FamilyInstance AddWindow(Document doc, Level level1, Wall wall)
        {


            FamilySymbol windowType = new FilteredElementCollector(doc)
             .OfClass(typeof(FamilySymbol))
             .OfCategory(BuiltInCategory.OST_Windows)
             .OfType<FamilySymbol>()
             .Where(t => t.Name.Equals("0915 x 1830 мм"))
             .Where(t => t.FamilyName.Equals("Фиксированные"))
             .FirstOrDefault();

            LocationCurve hostCurve = wall.Location as LocationCurve;
            XYZ point1 = hostCurve.Curve.GetEndPoint(0);
            XYZ point2 = hostCurve.Curve.GetEndPoint(1);
            XYZ point = (point1 + point2) / 2;
            XYZ xyz = new XYZ(0, 0, 3);
            XYZ oPoint = point + xyz;

            if (!windowType.IsActive)
                windowType.Activate();


            FamilyInstance window = doc.Create.NewFamilyInstance(oPoint, windowType, wall, level1,
                Autodesk.Revit.DB.Structure.StructuralType.NonStructural);

            return window;
        }

        public static ExtrusionRoof AddRoof(Document doc,Level level2, List<Wall> walls)

        {
           

            RoofType roofType = new FilteredElementCollector(doc)
             .OfClass(typeof(RoofType))
             .OfType<RoofType>()
             .Where(t => t.Name.Equals("Типовой - 400мм"))
             .Where(t => t.FamilyName.Equals("Базовая крыша"))
             .FirstOrDefault();


            CurveArray curveArray = new CurveArray();
            curveArray.Append(Line.CreateBound(new XYZ(-18.05, -9.84, 13.12), new XYZ(-18.05, 0, 18.05)));
            curveArray.Append(Line.CreateBound(new XYZ(-18.05, 0, 18.05), new XYZ(-18.05, 9.84, 13.12)));

            ReferencePlane plane = doc.Create.NewReferencePlane(new XYZ(0, 0, 0), new XYZ(0, 0, 20), new XYZ(0, 20, 0), doc.ActiveView);
            ExtrusionRoof extrusionRoof = doc.Create.NewExtrusionRoof(curveArray, plane, level2, roofType, -18.05, 18.05);

            return extrusionRoof;

        }
    }
}
