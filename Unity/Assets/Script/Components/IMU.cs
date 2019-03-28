public class IMU : DeviceComponent
{

    private Rotation rotation;
    private bool tapped;

    public IMU(TwinObject device){
        this.device = device;
        rotation = new Rotation(0.0f,0.0f,0.0f);
        tapped = false;
    }

    private void Start()
    {
        rotation = new Rotation(0.0f, 0.0f, 0.0f);
        tapped = false;
    }

    private void Update()
    {
        tapped = false;
    }

    void OnTapped()
    {
        tapped = true;
    }

    public void RequestRotation(){
        device.SendGetMessage("imu/rotation");
    }

    public Rotation GetRotation(){
        return rotation;
    }

    public void SetRotation(Rotation rotation){
        rotation.SetRotation(rotation);
    }

    public void SetRotation(float roll, float pitch, float yaw){
        rotation.SetRotation(roll, pitch, yaw);
    }

    public bool JustTapped(){
        return tapped;
    }
}