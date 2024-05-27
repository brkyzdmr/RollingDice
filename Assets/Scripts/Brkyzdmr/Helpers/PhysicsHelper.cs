using UnityEngine;

namespace Brkyzdmr.Helpers
{
    public static class PhysicsHelper
    {
        /// <summary>
        /// Determines if a given Rigidbody has stopped moving by checking both linear and angular velocity.
        /// </summary>
        /// <param name="rb">The Rigidbody to check.</param>
        /// <returns>True if the Rigidbody has stopped moving, false otherwise.</returns>
        public static bool HasRigidbodyStopped(Rigidbody rb)
        {
            return rb.velocity == Vector3.zero && rb.angularVelocity == Vector3.zero;
        }

        /// <summary>
        /// Calculates the initial velocity required to launch a projectile from one position to another
        /// in a gravitational field.
        /// </summary>
        /// <param name="from">The position from which the projectile is launched.</param>
        /// <param name="to">The target position where the projectile should land.</param>
        /// <returns>The initial velocity vector required to launch the projectile.</returns>
        /// <remarks>
        ///  Source: "http://hyperphysics.phy-astr.gsu.edu/hbase/traj.html"
        /// </remarks>
        public static Vector3 CalculateProjectileLaunchVelocity(Vector3 from, Vector3 to)
        {
            var distance = to - from;
            var height = distance.y;
            var halfRange = new Vector3(distance.x, 0, distance.z);
            var Vy = Mathf.Sqrt(Mathf.Abs(-2 * Physics.gravity.y * height));
            var Vxz = -(halfRange * Physics.gravity.y) / Vy;
            return new Vector3(Vxz.x, Vy, Vxz.z) / 2;
        }
    }
}