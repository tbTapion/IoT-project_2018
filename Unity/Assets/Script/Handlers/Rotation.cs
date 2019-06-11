
namespace ExactFramework.Handlers
{
    /*
    Rotation data object used by the IMU device component. Holds rotation data; yaw, roll, pitch.
     */
    public class Rotation
    {

        public float yaw, roll, pitch; //Rotation data.

        /*
        Constructor. Takes in rotation data-
        
        Parameter type: float, float, float
        Parameter: roll, pitch, yaw
         */
        public Rotation(float roll, float pitch, float yaw)
        {
            this.yaw = yaw;
            this.roll = roll;
            this.pitch = pitch;
        }

        /*
        Sets the rotation data of this object.
        
        Parameter type: float, float, float
        Parameter: roll, pitch, yaw
         */
        public void SetRotation(float roll, float pitch, float yaw)
        {
            this.yaw = yaw;
            this.roll = roll;
            this.pitch = pitch;
        }

        /*
        Sets the rotation data of this object from another rotation object's data.

        Parameter type: Rotation
        Parameter: rotation
         */
        public void SetRotation(Rotation rotation)
        {
            SetRotation(rotation.roll, rotation.pitch, rotation.yaw);
        }
    }
}