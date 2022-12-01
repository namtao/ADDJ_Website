namespace ADDJ.Core
{
    /// <summary>
    /// Summary description for QualityImage
    /// </summary>
    public class QualityImage
    {
        public static long[] Quality(int contenLength)
        {
            var quality = new long[6];
            quality[0] = 100L;

            if (contenLength >= 3072000)
            {
                quality[1] = 85L;
                quality[2] = 85L;
                quality[3] = 85L;
                quality[4] = 85L;
                quality[5] = 85L;
            }
            else
            {
                if ((contenLength >= 2048000) && contenLength < 3072000)
                {
                    quality[1] = 87L;
                    quality[2] = 85L;
                    quality[3] = 85L;
                    quality[4] = 85L;
                    quality[5] = 85L;
                }
                else if ((contenLength > 1024000) && (contenLength < 2048000))
                {
                    quality[1] = 95L;
                    quality[2] = 90L;
                    quality[3] = 90L;
                    quality[4] = 90L;
                    quality[5] = 90L;
                }
                else
                {
                    quality[1] = 95L;
                    quality[2] = 95L;
                    quality[3] = 95L;
                    quality[4] = 95L;
                    quality[5] = 95L;
                }
            }
            return quality;
        }
    }
}