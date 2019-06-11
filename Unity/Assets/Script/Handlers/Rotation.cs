
namespace ExactFramework.Handlers
{
    ///<summary>
    ///Rotation data object used by the IMU device component. Holds rotation data; yaw, roll, pitch.
    ///<summary>
    public class Rotation
    {
        public float yaw, roll, pitch; //Rotation data.

        ///<summary>
        ///Constructor. Takes in rotation data.
        ///</summary>
        ///<param name="roll">float value</param>
        ///<param name="pitch">float value</param>
        ///<param name="yaw">float value</param>
        public Rotation(float roll, float pitch, float yaw)
        {
            this.yaw = yaw;
            this.roll = roll;
            this.pitch = pitch;
        }

        ///<summary>
        ///Sets the rotation data of this object with variables.
        ///</summary>
        ///<param name="roll">float value</param>
        ///<param name="pitch">float value</param>
        ///<param name="yaw">float value</param>
        public void SetRotation(float roll, float pitch, float yaw)
        {
            this.yaw = yaw;
            this.roll = roll;
            this.pitch = pitch;
        }

        ///<summary>
        ///Sets the rotation data of this object from another rotation object's data.
        ///</summary>
        ///<param name="rotation">Other Rotation object.</param>
        public void SetRotation(Rotation rotation)
        {
            SetRotation(rotation.roll, rotation.pitch, rotation.yaw);
        }
    }
}