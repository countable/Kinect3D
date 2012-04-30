using System.Windows.Media;

namespace ShapeGame
{
    using System.Windows.Media.Media3D;
    using System.Windows.Controls;
    using System;
    using System.Windows;
    class Cube
    {
        private Model3DGroup cube;
        private ModelVisual3D model;
        private DirectionalLight myDirectionalLight;
        private float obesity;
        private RotateTransform3D rotation;
        private Transform3DGroup trans;
        private ScaleTransform3D scale;
        private TranslateTransform3D translation;
        private AxisAngleRotation3D axis;
        private Vector3D up;

        #region Constructor

        public Cube(Viewport3D myViewport3D, float obesity)
        {

            this.up = ((PerspectiveCamera)myViewport3D.Camera).UpDirection;
            this.trans = new Transform3DGroup();
            this.scale = new ScaleTransform3D();
            this.translation = new TranslateTransform3D();
            this.rotation = new RotateTransform3D();
            this.axis = new AxisAngleRotation3D();

            this.obesity = obesity;   
            this.cube = new Model3DGroup();

            Point3D p0 = new Point3D(-1, -1, -1);
            Point3D p1 = new Point3D(1, -1, -1);
            Point3D p2 = new Point3D(1, -1, 1);
            Point3D p3 = new Point3D(-1, -1, 1);
            Point3D p4 = new Point3D(-1, 0, -1);
            Point3D p5 = new Point3D(1, 0, -1);
            Point3D p6 = new Point3D(1, 0, 1);
            Point3D p7 = new Point3D(-1, 0, 1);

            //front side triangles
            cube.Children.Add(CreateTriangleModel(p3, p2, p6));
            cube.Children.Add(CreateTriangleModel(p3, p6, p7));
            //right side triangles
            cube.Children.Add(CreateTriangleModel(p2, p1, p5));
            cube.Children.Add(CreateTriangleModel(p2, p5, p6));
            //back side triangles
            cube.Children.Add(CreateTriangleModel(p1, p0, p4));
            cube.Children.Add(CreateTriangleModel(p1, p4, p5));
            //left side triangles
            cube.Children.Add(CreateTriangleModel(p0, p3, p7));
            cube.Children.Add(CreateTriangleModel(p0, p7, p4));
            //top side triangles
            cube.Children.Add(CreateTriangleModel(p7, p6, p5));
            cube.Children.Add(CreateTriangleModel(p7, p5, p4));
            //bottom side triangles
            cube.Children.Add(CreateTriangleModel(p2, p3, p0));
            cube.Children.Add(CreateTriangleModel(p2, p0, p1));

            this.model = new ModelVisual3D();
            this.model.Content = cube;
            this.myDirectionalLight = new DirectionalLight();
            this.myDirectionalLight.Color = Colors.Green;
            this.myDirectionalLight.Direction = new Vector3D(-2, -3, 1);
            this.cube.Children.Add(myDirectionalLight);
            myViewport3D.Children.Add(model);

            trans.Children.Add(scale);
            trans.Children.Add(rotation);
            trans.Children.Add(translation);

            this.cube.Transform = trans;
        }

        public void update(Point3D pt1)
        {

        }

        public void update(Microsoft.Kinect.SkeletonPoint pt1, Microsoft.Kinect.SkeletonPoint pt2)
        {            
            Point3D diff = Vector3D.Subtract(new Vector3D(pt1.X, pt1.Y, pt1.Z), new Point3D(pt2.X, pt2.Y, pt2.Z));
            Vector3D differenceVec = new Vector3D(diff.X, diff.Y, -diff.Z);
            
            this.axis.Axis = Vector3D.CrossProduct(up, differenceVec);
            this.axis.Angle = -Vector3D.AngleBetween(up, differenceVec);

            this.scale.ScaleX = differenceVec.Length*obesity;
            this.scale.ScaleY = differenceVec.Length;
            this.scale.ScaleZ = differenceVec.Length*obesity;

            this.translation.OffsetZ = pt1.Z;
            this.translation.OffsetX = -pt1.X;
            this.translation.OffsetY = pt1.Y;

            this.rotation.Rotation = axis;
        }


        #endregion Constructor

        #region HelperMethods

        private Vector3D CalculateNormal(Point3D p0, Point3D p1, Point3D p2)
        {
            Vector3D v0 = new Vector3D(
                p1.X - p0.X, p1.Y - p0.Y, p1.Z - p0.Z);
            Vector3D v1 = new Vector3D(
                p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            return Vector3D.CrossProduct(v0, v1);
        }

        private Model3DGroup CreateTriangleModel(Point3D p0, Point3D p1, Point3D p2)
        {
            MeshGeometry3D mesh = new MeshGeometry3D();
            mesh.Positions.Add(p0);
            mesh.Positions.Add(p1);
            mesh.Positions.Add(p2);
            mesh.TriangleIndices.Add(0);
            mesh.TriangleIndices.Add(1);
            mesh.TriangleIndices.Add(2);
            Vector3D normal = CalculateNormal(p0, p1, p2);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            mesh.Normals.Add(normal);
            Material material = new DiffuseMaterial(
                new SolidColorBrush(Colors.White));
            GeometryModel3D model = new GeometryModel3D(
                mesh, material);
            Model3DGroup group = new Model3DGroup();
            group.Children.Add(model);
            return group;
        }

        #endregion HelperMethods
    }


}
