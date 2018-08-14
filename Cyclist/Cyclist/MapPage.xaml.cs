using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Maps;
using Mapsui.Layers;
using BruTile.Predefined;

using Mapsui.Geometries;
using Mapsui.Layers;
using Mapsui.Providers;
//using Mapsui.Samples.Common.Helpers;
using Mapsui.Styles;
using Mapsui.Utilities;
using System.Reflection;
using Mapsui.Projection;

namespace Cyclist
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MapPage : Xamarin.Forms.ContentPage
    {
        private static List<Point> SataticPoints;

        public MapPage()
        {
            InitializeComponent();

            SataticPoints = new List<Point>();
            Point point = SphericalMercator.FromLonLat(34.7609650, 32.0477280);
            SataticPoints.Add(point);

            mapView.MoveToCenter(new Position(32.0477280d, 34.7609650d));
            mapView.PropertyChanged += (s, e) => System.Diagnostics.Debug.WriteLine(e.PropertyName);
            mapView.Map.Layers.Add(OpenStreetMap.CreateTileLayer());

            SataticPoints.Add(new Point(mapView.Map.Envelope.Left, mapView.Map.Envelope.Bottom));
            SataticPoints.Add(new Point(mapView.Map.Envelope.Right, mapView.Map.Envelope.Top));
            //mapView.Map.Layers.Add(CreatePolygonLayer());
            //mapView.Map.Layers.Add(CreateLayerWithStyleOnLayer(mapView.Map.Envelope, 10));
            mapView.Map.Layers.Add(CreateLayerWithStyleOnFeature(mapView.Map.Envelope, 10));


            //mapView.Map.Layers.Add(new TileLayer(KnownTileSources.Create(KnownTileSource.OpenStreetMap)) { Name = "Open Street Map" });
            //mapView.Map.Widgets.Add(new ScaleBarWidget { 

            mapView.Map.Viewport.ViewportChanged += (s, e) => System.Diagnostics.Debug.WriteLine(e.PropertyName);

            

        }

        public static ILayer CreatePolygonLayer()
        {
            return new Layer("Polygons")
            {
                DataSource = new MemoryProvider(CreatePolygon()),
                Style = new VectorStyle
                {
                    Fill = new Brush(new Mapsui.Styles.Color(150, 150, 30, 128)),
                    Outline = new Pen
                    {
                        Color = Mapsui.Styles.Color.Orange,
                        Width = 1,
                        PenStyle = PenStyle.DashDotDot, //.Solid,
                        PenStrokeCap = PenStrokeCap.Round
                    }
                }
            };
        }

        private static List<Polygon> CreatePolygon()
        {
            var result = new List<Polygon>();

            var polygon = new Polygon();

            
            polygon.ExteriorRing.Vertices.Add(new Point(32.0549300, 34.7595488));
            polygon.ExteriorRing.Vertices.Add(new Point(32.0569669, 34.7600209));
            polygon.ExteriorRing.Vertices.Add(new Point(32.0578762, 34.7641407));
            polygon.ExteriorRing.Vertices.Add(new Point(32.0527113, 34.7654497));
            polygon.ExteriorRing.Vertices.Add(new Point(32.0549300, 34.7595488));
            /*var linearRing = new LinearRing();
            linearRing.Vertices.Add(new Point(1000000, 1000000));
            linearRing.Vertices.Add(new Point(9000000, 1000000));
            linearRing.Vertices.Add(new Point(9000000, 9000000));
            linearRing.Vertices.Add(new Point(1000000, 9000000));
            linearRing.Vertices.Add(new Point(1000000, 1000000));
            polygon.InteriorRings.Add(linearRing);*/

            result.Add(polygon);

            /*polygon = new Polygon();
            polygon.ExteriorRing.Vertices.Add(new Point(-10000000, 0));
            polygon.ExteriorRing.Vertices.Add(new Point(-15000000, 5000000));
            polygon.ExteriorRing.Vertices.Add(new Point(-10000000, 10000000));
            polygon.ExteriorRing.Vertices.Add(new Point(-5000000, 5000000));
            polygon.ExteriorRing.Vertices.Add(new Point(-10000000, 0));*/
            /*var linearRing2 = new LinearRing();
            linearRing2.Vertices.Add(new Point(-10000000, 1000000));
            linearRing2.Vertices.Add(new Point(-6000000, 5000000));
            linearRing2.Vertices.Add(new Point(-10000000, 9000000));
            linearRing2.Vertices.Add(new Point(-14000000, 5000000));
            linearRing2.Vertices.Add(new Point(-10000000, 1000000));
            polygon.InteriorRings.Add(linearRing2);*/

            //result.Add(polygon);

            return result;
        }

        private static ILayer CreateLayerWithStyleOnLayer(BoundingBox envelope, int count = 25)
        {
            return new Layer("Style on Layer")
            {
                DataSource = new MemoryProvider(GenerateRandomPoints(envelope, count)),
                Style = CreateBitmapStyle("Cyclist.Images.ic_place_black_24dp.png")
            };
        }

        private static ILayer CreateLayerWithStyleOnFeature(BoundingBox envelope, int count = 25)
        {
            var style = CreateBitmapStyle("Cyclist.Images.loc.png");

            return new Layer("Style on feature")
            {
                DataSource = new MemoryProvider(GenerateRandomFeatures(envelope, count, style)),
                Style = null
            };
        }

        private static IEnumerable<IFeature> GenerateRandomFeatures(BoundingBox envelope, int count, IStyle style)
        {
            var result = new List<Feature>();
            var points = makePoints(envelope, SataticPoints, 123);
            foreach (var point in points)
            {
                result.Add(new Feature { Geometry = point, Styles = new List<IStyle> { style } });
            }
            return result;
        }

        private static SymbolStyle CreateBitmapStyle(string embeddedResourcePath)
        {
            var bitmapId = GetBitmapIdForEmbeddedResource(embeddedResourcePath);
            return new SymbolStyle { BitmapId = bitmapId, SymbolScale = 0.1 };
        }

        private static int GetBitmapIdForEmbeddedResource(string imagePath)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var image = assembly.GetManifestResourceStream(imagePath);
            var bitmapId = BitmapRegistry.Instance.Register(image);
            return bitmapId;
        }

        public static IEnumerable<IGeometry> GenerateRandomPoints(BoundingBox envelope, int count = 25, int seed = 192)
        {
            Random _random = new Random(seed);

            var result = new List<IGeometry>();

            for (var i = 0; i < count; i++)
            {
                result.Add(new Point(
                    _random.NextDouble() * envelope.Width + envelope.Left,
                    _random.NextDouble() * envelope.Height + envelope.Bottom));
            }

            return result;
        }

        public static IEnumerable<IGeometry> makePoints(BoundingBox envelope, List<Point> points, int seed)
        {
            Random _random = new Random(seed);

            var result = new List<IGeometry>();

            for (var i = 0; i < points.Count ; i++)
            {
                result.Add(points[i]);
            }

            return result;
        }
    }
}