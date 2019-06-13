using ExactFramework.Configuration;
using ExactFramework.Handlers;

namespace ExactFramework.Component.Examples
{
    ///<summary>
    ///Digital representation of an IMU component.
    ///An IMU is an inertial measuring unit.
    ///</summary>
    public class IMU : DeviceComponent
    {
        
        ///<summary>
        ///Rotation variable to store the current rotation of the IMU.
        ///</summary>
        private Rotation rotation;

        ///<summary>
        ///Bool variable for whether the IMU has registered a tap event or not.
        ///</summary>
        private bool tapped;

        ///<summary>
        ///Old constructor for the IMU unit before the new component system. Sets the device, default rotation and tapped to false.
        ///</summary>
        ///<param name="device">Twin object class it's connected to.</param>
        public IMU(TwinObject device)
        {
            this.device = device;
            rotation = new Rotation(0.0f, 0.0f, 0.0f);
            tapped = false;
        }

        public override void Start()
        {
            base.Start();
            rotation = new Rotation(0.0f, 0.0f, 0.0f);
            tapped = false;
        }

        void Update()
        {
            tapped = false;
        }

        ///<summary>
        ///Method to set the tapped variable to true. Called when the IMU receives a tap even over MQTT.
        ///</summary>
        void OnTapped()
        {
            tapped = true;
            device.InvokeEvent("OnTapped");
        }

        ///<summary>
        ///Method to request the rotation values of the physical IMU component. Sends a request message over MQTT.
        ///</summary>
        public void RequestRotation()
        {
            device.SendGetMessage("imu/rotation");
        }

        ///<summary>
        ///Returns the rotation values of the digital IMU component.
        ///</summary>
        ///<returns>Rotation values.</returns>
        public Rotation GetRotation()
        {
            return rotation;
        }

        ///<summary>
        ///Sets the rotation of the digital IMU device.
        ///</summary>
        private void SetRotation(Rotation rotation)
        {
            rotation.SetRotation(rotation);
        }

        ///<summary>
        ///Sets the rotation of the digital IMU device.
        ///</summary>
        ///<param name="roll">Roll value of the rotation.</param>
        ///<param name="pitch">Pitch value of the rotation.</param>
        ///<param name="yaw">Yaw value of the rotation.</param>
        private void SetRotation(float roll, float pitch, float yaw)
        {
            rotation.SetRotation(roll, pitch, yaw);
            device.InvokeEvent("OnRotationChange");
        }

        ///<summary>
        ///Returns the tapped value of the IMU device, true if it was just tapped.
        ///</summary>
        public bool JustTapped()
        {
            return tapped;
        }

        public override void UpdateComponent(string eventType, byte[] payload)
        {
            if(eventType == "tapped"){
                OnTapped();
            }else if(eventType == "rotation"){
                int roll = payload[0] + payload[1];
                int pitch = payload[2] + payload[3];
                int yaw = payload[4] + payload[5];
                SetRotation(roll, pitch, yaw);
            }
        }
    }
}