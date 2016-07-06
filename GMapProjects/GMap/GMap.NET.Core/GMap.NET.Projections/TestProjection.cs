
namespace GMap.NET.Projections
{
   using System;

   /// <summary>
   /// GEOGCS[\"WGS 84\",DATUM[\"WGS_1984\",SPHEROID[\"WGS 84\",6378137,298.257223563,AUTHORITY[\"EPSG\",\"7030\"]],AUTHORITY[\"EPSG\",\"6326\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.01745329251994328,AUTHORITY[\"EPSG\",\"9122\"]],AUTHORITY[\"EPSG\",\"4326\"]]
   /// PROJCS[\"Lietuvos Koordinoei Sistema 1994\",GEOGCS[\"LKS94 (ETRS89)\",DATUM[\"Lithuania_1994_ETRS89\",SPHEROID[\"GRS 1980\",6378137,298.257222101,AUTHORITY[\"EPSG\",\"7019\"]],TOWGS84[0,0,0,0,0,0,0],AUTHORITY[\"EPSG\",\"6126\"]],PRIMEM[\"Greenwich\",0,AUTHORITY[\"EPSG\",\"8901\"]],UNIT[\"degree\",0.0174532925199433,AUTHORITY[\"EPSG\",\"9108\"]],AUTHORITY[\"EPSG\",\"4126\"]],PROJECTION[\"Transverse_Mercator\"],PARAMETER[\"latitude_of_origin\",0],PARAMETER[\"central_meridian\",24],PARAMETER[\"scale_factor\",0.9998],PARAMETER[\"false_easting\",500000],PARAMETER[\"false_northing\",0],UNIT[\"metre\",1,AUTHORITY[\"EPSG\",\"9001\"]],AUTHORITY[\"EPSG\",\"2600\"]]
   /// </summary>
   public class TestProjection : PureProjection
   {
       const double MinLatitude = 31.314030292850077;
       const double MaxLatitude = 31.727142886150034;
       const double MinLongitude = 104.53565984320007;
       const double MaxLongitude = 105.08758767680006;

       Size tileSize = new Size(512, 512);
       public override Size TileSize
       {
           get
           {
               return tileSize;
           }
       }

       public override double Axis
       {
           get
           {
               return 6378137;
           }
       }

       public override double Flattening
       {
           get
           {
               return (1.0 / 298.257223563);
           }
       }

       public override Point FromLatLngToPixel(double lat, double lng, int zoom)
       {
           Point ret = Point.Empty;
           Console.WriteLine("在FromLatLngToPixel ：lat:" + lat + "   MinLatitude:" + MinLatitude + " MaxLatitude:"+MaxLatitude);
           Console.WriteLine("在FromLatLngToPixel ：lng:" + lng + "   MinLongitude:" + MinLongitude + " MaxLongitude:" + MaxLongitude);
           lat = Clip(lat, MinLatitude, MaxLatitude);
           lng = Clip(lng, MinLongitude, MaxLongitude);
         
           Size s = GetTileMatrixSizePixel(zoom);
           double mapSizeX = s.Width;
           double mapSizeY = s.Height;

           double scale = 360.0 / mapSizeX;

           ret.Y = (int)((90.0 - lat) / scale);
           ret.X = (int)((lng + 180.0) / scale);

           return ret;
       }

       public override PointLatLng FromPixelToLatLng(int x, int y, int zoom)
       {
           PointLatLng ret = PointLatLng.Empty;

           Size s = GetTileMatrixSizePixel(zoom);
           double mapSizeX = s.Width;
           double mapSizeY = s.Height;

           double scale = 360.0 / mapSizeX;

           ret.Lat = 90 - (y * scale);
           ret.Lng = (x * scale) - 180;

           return ret;
       }

       /// <summary>
       /// Clips a number to the specified minimum and maximum values.
       /// </summary>
       /// <param name="n">The number to clip.</param>
       /// <param name="minValue">Minimum allowable value.</param>
       /// <param name="maxValue">Maximum allowable value.</param>
       /// <returns>The clipped value.</returns>
       double Clip(double n, double minValue, double maxValue)
       {
           return Math.Min(Math.Max(n, minValue), maxValue);
       }

       public override Size GetTileMatrixMaxXY(int zoom)
       {
           int y = (int)Math.Pow(2, zoom);
           return new Size((2 * y) - 1, y - 1);
       }

       public override Size GetTileMatrixMinXY(int zoom)
       {
           return new Size(0, 0);
       }

       public override double GetGroundResolution(int zoom, double latitude)
       {
           return GetTileMatrixResolution(zoom);
       }

       public double GetTileMatrixResolution(int zoom)
       {  
           double ret = 0;
           switch (zoom)
           {
               case 0:
                   {
                       ret = 0.3515625;
                   }
                   break;

               case 1:
                   {
                       ret = 0.17578125;
                   }
                   break;

               case 2:
                   {
                       ret = 0.087890625;
                   }
                   break;

               case 3:
                   {
                       ret = 0.0439453125;
                   }
                   break;

               case 4:
                   {
                       ret = 0.02197265625;
                   }
                   break;

               case 5:
                   {
                       ret = 0.010986328125;
                   }
                   break;

               case 6:
                   {
                       ret = 0.0054931640625;
                   }
                   break;

               case 7:
                   {
                       ret = 0.00274658203125;
                   }
                   break;
               case 8:
                   {
                       ret = 0.001373291015625;
                   }
                   break;
               case 9:
                   {
                       ret = 6.866455078125E-4;
                   }
                   break;
               case 10:
                   {
                       ret = 3.4332275390625E-4;
                   }
                   break;
               case 11:
                   {
                       ret = 1.71661376953125E-4;
                   }
                   break;
               case 12:
                   {
                       ret = 8.58306884765629E-5;
                   }
                   break;
               case 13:
                   {
                       ret = 4.29153442382814E-5;
                   }
                   break;
               case 14:
                   {
                       ret = 2.14576721191407E-5;
                   }
                   break;
               case 15:
                   {
                       ret = 1.07288360595703E-5;
                   }
                   break;
           }
           Console.WriteLine("计算分辨率：zoom:" + zoom + "  GetTileMatrixResolution 分辨率:"+ret);
           return ret;
       }
   }
}
