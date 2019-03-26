public class IMU : DeviceComponent
{

    private Rotation rotation;
    private bool tapped;

    public IMU(TwinObject device){
        this.device = device;
        rotation = new Rotation(0.0f,0.0f,0.0f);
        tapped = false;
    }

    public override void update()
    {
        tapped = false;
    }

    public void requestRotation(){
        device.sendGetMessage("imu/rotation");
    }

    public Rotation getRotation(){
        return rotation;
    }

    public void setRotation(Rotation rotation){
        rotation.setRotation(rotation);
    }

    public void setRotation(float roll, float pitch, float yaw){
        rotation.setRotation(roll, pitch, yaw);
    }

    public void setTapped(){
        tapped = true;
    }

    public bool justTapped(){
        return tapped;
    }
}