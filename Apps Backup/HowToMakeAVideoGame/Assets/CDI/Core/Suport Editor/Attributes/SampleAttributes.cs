using UnityEngine;

namespace cdi
{
    public class SampleAttributes : MonoBehaviour
    {
        [Information("You need to define what layer(s) this character will consider a walkable platform/moving platform etc. By default, you want Platforms, MovingPlatforms, OneWayPlatforms, MovingOneWayPlatforms, in this order.")]
        public float hp;
    }
}