namespace cdi
{
    public class WeightsFloatRandom
    {
        readonly float[] thresholds;

        public WeightsFloatRandom(params float[] weights)
        {
            thresholds = new float[weights.Length];
            thresholds[0] = weights[0];
            for (int i = 1; i < weights.Length; i++)
            {
                thresholds[i] = thresholds[i - 1] + weights[i];
            }
        }

        /// <summary>
        /// Lấy ngẫu nhiên một slot theo trọng số
        /// </summary>
        /// <param name="weights">Danh sách trọng số của các slot tương ứng</param>
        /// <returns>Số thứ tự của slot (theo số thứ tự của array)</returns>
        public int Random()
        {
            float res = UnityEngine.Random.Range(0, thresholds[thresholds.Length - 1]);
            for (int slot = 0; slot < thresholds.Length; slot++)
            {
                if (res < thresholds[slot])
                    return slot;
            }
            throw new System.Exception("WeightsRandom could not found res " + res);
        }
    }
}
