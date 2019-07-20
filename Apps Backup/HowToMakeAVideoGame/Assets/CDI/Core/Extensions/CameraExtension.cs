using UnityEngine;
namespace cdi
{
    public static class CameraExtension
    {
        /// <summary>
        /// Match kích thước của object với view port thông qua size of ortho camera.
        /// Ta có: 
        /// size = vh/200f;
        /// Screen.width/Screen.height = vw/vh;
        /// Trong đó vh là chiều cao của view port, vw là chiều rộng của view port
        /// => vh = size * 200f
        /// và vw = Screen.width/Screen.height * vh = Screen.width/Screen.height * size * 200f;
        /// Để đối tượng nằm trong view port thì cần điều kiện sau
        /// objHeight < vh * fillHeightPercent và objWidth <= vw * fillWidthPercent
        /// Suy ra size phải thỏa mãn 2 điều kiện
        /// size > objHeight / (fillHeightPercent * 200f)
        /// và size > objWidth * Screen.height / (fillWidthPercent * 200f * Screen.width)
        /// </summary>
        /// <param name="objWidth">Chiều ngang của đối tượng cần hiển thị đầy đủ trong camera tính bằng pixcel</param>
        /// <param name="objHeight">Chiều cao của đối tượng cần hiển thị đầy đủ trong camera bằng pixcel</param>
        /// <param name="fillWidthPercent">Độ lấp đầy chiều ngang</param>
        /// /// <param name="fillHeightPercent">Độ lấp đầy chiều dọc</param>
        public static void MatchObjectBySize(this Camera camera, float objWidth, float objHeight, float fillWidthPercent = 1f, float fillHeightPercent = 1f)
        {
            float s1 = objHeight / (fillHeightPercent * 200f);
            float s2 = objWidth * Screen.height / (fillWidthPercent * 200f * Screen.width);
            camera.orthographicSize = Mathf.Max(s1, s2);
        }
    }
}