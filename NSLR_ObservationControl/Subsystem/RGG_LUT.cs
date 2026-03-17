using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Annotations;

namespace NSLR_ObservationControl.Subsystem
{
    class RGG_LUT
    {        

        public class ObservationSiteInfo  //거창NSLR 위치정보
        {
            public const double LATITUDE = 35.5901;
            public const double LONGITUDE = 127.9192;
            public const double ALTITUDE = 923.302744;
        }

        const double EARTH_RADIUS = 6371000;
        const double SpeedOfLight = 299792458; // 빛의 속도 (m/s)

        private static void ValidateInputs(double ra, double dec, double altitude, double lat, double lon)
        {
            if (ra < 0 || ra >= 360) throw new ArgumentException("적경은 0도 이상 360도 미만이어야 합니다.");
            if (dec < -90 || dec > 90) throw new ArgumentException("적위는 -90도 이상 90도 이하여야 합니다.");
            if (altitude < 0) throw new ArgumentException("고도는 0 이상이어야 합니다.");
            if (lat < -90 || lat > 90) throw new ArgumentException("위도는 -90도 이상 90도 이하여야 합니다.");
            if (lon < -180 || lon >= 180) throw new ArgumentException("경도는 -180도 이상 180도 미만이어야 합니다.");
        }

        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////        
        private static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        public static double CalculateToF1(double satelliteLong, double satelliteLat, double satelliteAltitude)
        {
            try
            {
                // 입력값 유효성 검사
                ValidateInputs(satelliteLong, satelliteLat, satelliteAltitude, ObservationSiteInfo.LATITUDE, ObservationSiteInfo.LONGITUDE);

                // 위성의 적경(Ra)과 적위(Dec)를 라디안으로 변환
                double SAT_LongRad = DegreesToRadians(satelliteLong);
                double SAT_LatRad = DegreesToRadians(satelliteLat);

                // 위성의 직교 좌표 계산 (고도 고려)
                double satelliteRadius = EARTH_RADIUS + satelliteAltitude * 1000;
                double satelliteX = satelliteRadius * Math.Cos(SAT_LatRad) * Math.Cos(SAT_LongRad);
                double satelliteY = satelliteRadius * Math.Cos(SAT_LatRad) * Math.Sin(SAT_LongRad);
                double satelliteZ = satelliteRadius * Math.Sin(SAT_LatRad);


                // 지상국의 위도와 경도를 라디안으로 변환
                double Grnd_LatRad = DegreesToRadians(ObservationSiteInfo.LATITUDE);
                double Grnd_LongRad = DegreesToRadians(ObservationSiteInfo.LONGITUDE);

                // 지상국의 직교 좌표 계산
                double groundX = EARTH_RADIUS * Math.Cos(Grnd_LatRad) * Math.Cos(Grnd_LongRad);
                double groundY = EARTH_RADIUS * Math.Cos(Grnd_LatRad) * Math.Sin(Grnd_LongRad);
                double groundZ = EARTH_RADIUS * Math.Sin(Grnd_LatRad);

                // 위성과 지상국 사이의 직선 거리 계산 (km)
                double distance = Math.Sqrt(
                    Math.Pow(satelliteX - groundX, 2) +
                    Math.Pow(satelliteY - groundY, 2) +
                    Math.Pow(satelliteZ - groundZ, 2));

                // ToF 계산 (초)
                double tof = (2 * distance) / SpeedOfLight;

                return tof;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"입력 오류: {ex.Message}");
                return -1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"예상치 못한 오류 발생: {ex.Message}");
                return -1;
            }
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////        
        /// 


        static int cnt = 1;
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////        
        public static double CalculateToF2(double satelliteLong, double satelliteLat, double satelliteAltitude)
        {
            // 1. 좌표 변환
            double[] satelliteCartesian = EquatorialToCartesian(satelliteLong, satelliteLat, satelliteAltitude);
            double[] groundStationCartesian = GeodeticToCartesian(ObservationSiteInfo.LATITUDE, ObservationSiteInfo.LONGITUDE);

            // 2. 거리 계산
            double distance = CalculateDistance(satelliteCartesian, groundStationCartesian);

            // 3. ToF 계산
            double tof = (2 * distance)  / SpeedOfLight;
            if((cnt++ %10000) ==1)
            Console.WriteLine($"CalculateToF2()...distance {distance} tof {tof}");
            return tof;
        }

        private static double[] EquatorialToCartesian(double Long, double Lat, double satelliteAltitude)
        {
            //var Sa = satelliteAltitude;
            var Sa = satelliteAltitude * 1000; //미터 단위로 통일 
            double satelliteRadius = EARTH_RADIUS + Sa;
            double LongRadian;
            double LatRadian;
            LongRadian = Long * Math.PI / 180;
            LatRadian = Lat * Math.PI / 180;
            double x = satelliteRadius * Math.Cos(LatRadian) * Math.Cos(LongRadian);
            double y = satelliteRadius * Math.Cos(LatRadian) * Math.Sin(LongRadian);
            double z = satelliteRadius * Math.Sin(LatRadian);
            return new double[] { x, y, z };
        }

        private static double[] GeodeticToCartesian(double lat, double lon)
        {
            var Sa = EARTH_RADIUS + ObservationSiteInfo.ALTITUDE;
            double x = Sa * Math.Cos(lat*Math.PI/180) * Math.Cos(lon * Math.PI / 180);
            double y = Sa * Math.Cos(lat*Math.PI/180) * Math.Sin(lon * Math.PI / 180);
            double z = Sa * Math.Sin(lat*Math.PI/180);
            return new double[] { x, y, z };
        }

        private static double CalculateDistance(double[] point1, double[] point2)
        {
            double dx = point2[0] - point1[0];
            double dy = point2[1] - point1[1];
            double dz = point2[2] - point1[2];
            return Math.Sqrt(dx * dx + dy * dy + dz * dz);
        }
        /// //////////////////////////////////////////////////////////////////////////////////////////////////////////        
    }

}